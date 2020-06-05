using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Timers;
using System.Security.Permissions;
using System.IO;
using System.Diagnostics;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Collections;
using System.Data;
using MySql.Data.MySqlClient;

namespace TagDocx
{

    class Items
    {
        public Items(int id, string path, string form, string context, string name)
        {
            Id = id;
            Path = path;
            Form = form;
            Context = context;
            Name = name;
        }
        private static List<Items> instance;

        public static List<Items> GetInstance()
        {
            if (instance == null)
                instance = new List<Items>();

            return instance;
        }
        public int Id { get; set; }
        public string Path { get; set; }
        public string Form { get; set; }
        public string Context { get; set; }
        public string Name { get; set; }
    }

    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        string folderpath = @"C:\Users\YooJin\Desktop\AutomaticDocumentClassificationService\Dataset\한글\기사\문화";
        static string db_information = @"Server=localhost;Database=adcs;Uid=godocx;Pwd=486;";
        string savingpath;
        List<string> regularTaggingfolder = new List<string>(); // 정기적 태깅 폴더 담을 리스트 
        List<string> folderlist = new List<string>(); //하위 폴더들 담을 리스트
        List<string> filelist = new List<string>(); //파일들 담을 리스트
        List<string> notinDBfiles = new List<string>(); // DB에 없는 파일들 담을 리스트
        List<Items> itemslist = new List<Items>(); //태그 결과 담을 itemslist

        private BackgroundWorker _bgWorker = new BackgroundWorker();

        private int _workerState;

        public int WorkerState
        {
            get { return _workerState; }
            set
            {
                _workerState = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("WorkerState"));
            }
        }

        #region INotifyPropertyChanged Member

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        [System.Runtime.InteropServices.DllImport("Kernel32")]
        public static extern void FreeConsole();

        public MainWindow()
        {
            //프로그램 처음 시작시 저장폴더를 만듬
            string deskPath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            DirectoryInfo di = new DirectoryInfo(deskPath + "/ADCSforder");

            if(di.Exists == false)
            {
                di.Create();
            }

            Timer timer = new System.Timers.Timer();
            timer.Interval = 1000; // 1 초
            timer.Elapsed += new ElapsedEventHandler(timer_Elapsed);
            timer.Start();

            Console.WriteLine("Press Enter to exit");
            Console.ReadLine();

            //시작하고 변수들 초기화
            InitializeComponent();
            

            /* 백그라운드 워커
            DataContext = this;

            _bgWorker.DoWork += (s, e) =>
            {
                
                for (int i = 0; i <= 10000; i++) {
                    System.Threading.Thread.Sleep(1000);
                    Console.WriteLine(i);
                }

                LookforDoc();
                GetItems();
                MessageBox.Show("Workisdone");
            };

            _bgWorker.RunWorkerAsync();
            */
            MainHome.Content = new MainPage();

            //StartPeriodicTagging();
        }
        private void GetList() {
            //여기에서 property에 있는거 가져와야 함
            string customerDir = Properties.Settings.Default.customerDir;

            string [] test = customerDir.Split('<');

            for (int i = 0; i < test.Length-1; i++) {
                this.regularTaggingfolder.Add(test[i]);
            }
        }

        public void StartPeriodicTagging()
        {
            this.folderlist = new List<string>();
            this.filelist = new List<string>();
            this.itemslist = new List<Items>();
            
            //string dirPath = @"C:\Users\YooJin\Desktop\AutomaticDocumentClassificationService\Dataset\한글\기사\문화";
            string filelist = "";

            //폴더 경로로 폴더에 있는 하위 폴더들 찾기
            GetList();

            foreach (string dirpath in this.regularTaggingfolder) {
                
                GetFolders(dirpath);
                this.folderlist.Add(dirpath);
            }
            
            foreach (string name in this.folderlist)
            {
                GetFilesAndTag2(name);
            }
            
            Console.WriteLine("hello");
            WriteFile();
            Console.WriteLine("hello");

            /*
            foreach (string name in this.notinDBfiles)
            {
                filelist += "\""+name + "\" ";
            }
            */
            GetTag();
            GetItems();
            
        }

        private void GetFolders(string dirPath)
        {
            if (System.IO.Directory.Exists(dirPath))
            {
                System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(dirPath);
                foreach (var item in di.GetDirectories())
                {
                    GetFolders(dirPath + "\\" + item.Name);
                    this.folderlist.Add(dirPath + "\\" + item.Name);
                }
            }
            else
            {
                Console.WriteLine(dirPath + "를 찾을 수 없습니다");
            }
        }

        private void GetFilesAndTag2(string dirPath)
        {
            this.filelist = new List<string>();
            MySqlConnection connection = new MySqlConnection(db_information);
            List<Items> itemslist = new List<Items>();
            notinDBfiles = new List<string>();

            //폴더에 속한 문서들 불러옴
            if (System.IO.Directory.Exists(dirPath))
            {
                System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(dirPath);
                foreach (System.IO.FileInfo File in di.GetFiles())
                {
                    //Console.WriteLine(File.Name);
                    this.filelist.Add(File.Name);
                }
            }
            else
            {
                Console.WriteLine(dirPath + "를 찾을 수 없습니다");
            }
            
            //notinDBfiles = new List<string>(); //리스트 초기화

            try
            {
                connection.Open();
                DataSet tds = new DataSet();
                string searchPath = dirPath.Replace("\\", "/"); //DB를 찾기 위해 경로를 \\를 /로 바꿔줌


                foreach (string name in filelist)
                {
                    //Console.WriteLine("지금 여기입니다. "+name);
                    tds = new DataSet();

                    MySqlCommand cmd = new MySqlCommand("SELECT * FROM document WHERE PATH='" + searchPath + "'and NAME='" + name + "'", connection);
                    MySqlDataAdapter adp = new MySqlDataAdapter(cmd);
                    adp.Fill(tds);
                    //Console.WriteLine(tds);
                    if (tds.Tables[0].Rows.Count == 0) //비어있으면
                    {
                        //Console.WriteLine(name);
                        notinDBfiles.Add(dirPath + "\\" + name);
                        //GetTag(dirPath, name);
                    }
                    else
                    {
                        Console.WriteLine("안 비어있음");
                    }
                }
            }
            catch (MySqlException ex)
            {
                connection.Close();
                //Console.WriteLine("안연결");
            }
            finally { connection.Close(); }
            
            //Console.WriteLine(command);

            //GetTag(command);
        }


        //타이머할일
        static void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            //Console.WriteLine("timer!");   // <- 여기다가 하고싶은 작업 넣으면 됨!
        }

        private void GridBarTitle_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
        private void Maximize_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = (this.WindowState == WindowState.Normal) ? WindowState.Maximized : WindowState.Normal;
        }

        private void Mimimize_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }


        private void CloseButtonFechar_Click(object sender, RoutedEventArgs e)
        {
            //Application.Current.Shutdown();
            this.Visibility = Visibility.Hidden;

        }
       
        private void WriteFile() {
            using (StreamWriter outputFile = new StreamWriter(@"C:\Users\YooJin\Desktop\AutomaticDocumentClassificationService\filelist.txt"))
            {
                foreach (string line in this.notinDBfiles)
                {
                    //Console.WriteLine(line);
                    outputFile.WriteLine(line);
                }
            }
        }


        public void GetTag()
        {
            // Set working directory and create process
            var workingDirectory = System.IO.Path.GetFullPath("Scripts");

            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "cmd.exe",
                    RedirectStandardInput = true,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    WorkingDirectory = @"C:\Users\YooJin\Desktop\AutomaticDocumentClassificationService\Scoring\",
                    //WorkingDirectory = "C:\\Users\\소정\\Desktop\\졸업프로젝트\\AutomaticDocumentClassificationService\\Scoring\\",
                    //WindowStyle = ProcessWindowStyle.Normal,
                    WindowStyle = ProcessWindowStyle.Hidden,
                   // WorkingDirectory = "C:\\Users\\소정\\Desktop\\졸업프로젝트\\AutomaticDocumentClassificationService\\Scoring\\",
                    //WorkingDirectory = "C:\\AutomaticDocumentClassificationService\\Scoring\\",
                    CreateNoWindow = true
                }
            };

            process.Start();
            //Pass multiple commands to cmd.exe
            using (var sw = process.StandardInput)
            {
                if (sw.BaseStream.CanWrite)
                {
                    // Vital to activate Anaconda
                    //sw.WriteLine("C:\\Temp\\Anaconda3\\Scripts\\activate.bat");
                    sw.WriteLine("C:\\Users\\user\\Anaconda3\\Scriptsctivate.bat");
                    // Activate your environment
                    sw.WriteLine("activate tensorflow");
                    // run your script. You can also pass in arguments
                    //string command = "python modelTagging2.py " + files;
                    //string command = @"python C:\Users\YooJin\Desktop\AutomaticDocumentClassificationService\test\modelTagging.py " + files;
                    string command = @"python C:\Users\YooJin\Desktop\AutomaticDocumentClassificationService\test\modelTagging.py";
                    sw.WriteLine(command);

                }
            }

            int count = 0;
            string path, form, context, name;

            // read multiple output lines
            while (!process.StandardOutput.EndOfStream)
            {
                var line = process.StandardOutput.ReadLine();
                //Console.WriteLine(line);

                Regex reg = new Regex(@"<GET.*?>");
                MatchCollection result = reg.Matches(line);

                if (result.Count > 3)
                {
                    path = result[0].Groups[0].ToString().Substring(5);
                    path = path.Substring(0, path.Length - 2);
                    path = path.Replace("\\", "/");
                    form = result[1].Groups[0].ToString().Substring(5);
                    form = form.Substring(0, form.Length - 2);
                    context = result[2].Groups[0].ToString().Substring(6);
                    context = context.Substring(0, context.Length - 3);
                    name = result[3].Groups[0].ToString().Substring(5);
                    name = name.Substring(0, name.Length - 2);
                    //Items.GetInstance().Add(new Items(count++, path, form, context, name));
                    Items newitem = new Items(count++, path, form, context, name);
                    this.itemslist.Add(newitem);
                    //Console.WriteLine(path + " " + form + " " + context + " " + name + "\n");
                }
                //FileList.ItemsSource = Items.GetInstance();
            }

            FreeConsole();

            /*
            for (int i = 0; i < itemslist.Count; i++)
            {
                Console.WriteLine(itemslist[i].Path);
                string p = itemslist[i].Path.Replace("\\", "/");
                Console.WriteLine(p);
            }*/


        }

        void GetItems()
        {
            int id=1;
            string path, form, context, c, name, sql1, sql2;

            try
            {
                using (MySqlConnection conn = new MySqlConnection(db_information))
                {
                    conn.Open();
                    //if (conn.State == System.Data.ConnectionState.Open)
                    //    MessageBox.Show("서버에 연결");

                    DataSet tds = new DataSet();
                    MySqlCommand cmd = new MySqlCommand("SELECT max(id) FROM document;", conn);
                    MySqlDataAdapter adp = new MySqlDataAdapter(cmd);
                    adp.Fill(tds);

                    try
                    {
                        id = (int.Parse(tds.Tables[0].Rows[0][0].ToString())) + 1;
                    }
                    catch (Exception e) {
                        Console.WriteLine(e.Message);
                    }
                    
                    //Console.WriteLine(tds.Tables[0].Rows[0][0]);
                    // 삭제될 경우 대비해서 max count로 변경
                    //id = int.Parse(tds.Tables[0].Rows[0][0].ToString())+1;
                    Console.WriteLine(id);

                    foreach (Items a in this.itemslist)
                    {
                        path = a.Path;
                        form = a.Form;
                        context = a.Context;
                        name = a.Name;

                        sql1 = "insert into document values(" + id + ",\"" + name + "\",\"" + form + "\",\"" + path + "\")";
                        cmd = new MySqlCommand(sql1, conn);
                        cmd.ExecuteNonQuery();

                        Regex reg = new Regex(@"\'.*?\'");
                        MatchCollection result = reg.Matches(context);
                        foreach (Match mm in result)
                        {
                            c = mm.Groups[0].ToString().Substring(1);
                            c = c.Substring(0, c.Length - 1);
                            //Console.WriteLine(c);
                            sql2 = "insert into content values(" + id + ",\"" + c + "\")";
                            cmd = new MySqlCommand(sql2, conn);
                            cmd.ExecuteNonQuery();
                        }

                        //MessageBox.Show("입력 성공");
                        id++;
                    }
                    conn.Close();
                }
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }

        }




    }

}
