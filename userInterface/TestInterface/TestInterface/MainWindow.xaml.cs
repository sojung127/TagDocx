using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.ComponentModel;
using System.IO;
using System.Diagnostics;
using MySql.Data.MySqlClient;
using System.Text.RegularExpressions;
using System.Collections;
using System.Data;
using System.Timers;
using System.Security.Permissions;


namespace TestInterface
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    /// 

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
        List<string> notinDBfiles = new List<string>();
        List<Items> itemslist = new List<Items>();

        private BackgroundWorker _bgWorker = new BackgroundWorker();

        private int _workerState;

        public int WorkerState {
            get { return _workerState; }
            set {
                _workerState = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("WorkerState"));
            }
        }
        public MainWindow()
        {
            InitializeComponent();

            
            DataContext = this;

            _bgWorker.DoWork += (s, e) => {
                /*
                for (int i = 0; i <= 10000; i++) {
                    System.Threading.Thread.Sleep(1000);
                    Console.WriteLine(i);
                }*/

                LookforDoc();
                GetItems();
                MessageBox.Show("Workisdone");
            };

            _bgWorker.RunWorkerAsync();
            
            /*
            LookforDoc();
            GetItems();*/

        }

        #region INotifyPropertyChanged Member

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        [System.Runtime.InteropServices.DllImport("Kernel32")]
        public static extern void FreeConsole();


        //test
        //string selectedForder=찾을 문서가 있는 경로;
        //string docuname=찾을문서이름;


        private void LookforDoc()
        {
            //string dirPath, string selectedFolder, string docuname
            this.itemslist = new List<Items>();
            string dirPath = @"C:\Users\YooJin\Desktop\AutomaticDocumentClassificationService\Dataset\한글\기사\경제";
            string filename;
            MySqlConnection connection = new MySqlConnection(db_information);
            List<Items> itemslist = new List<Items>();
            List<string> filelist = new List<string>();
            notinDBfiles = new List<string>();

            if (System.IO.Directory.Exists(dirPath))
            {
                System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(dirPath);
                foreach (System.IO.FileInfo File in di.GetFiles())
                {
                    filename = File.Name;
                    filelist.Add(filename);
                }
            }

            /*
            //폴더 이름 출력
            Console.WriteLine("FOLDER NAME : " + File.Name);
            //폴더내부 파일 개수 출력
            Console.WriteLine("FILE COUNT IN FOLDER : " + item.GetFiles().Length);
            //폴더 생성 날짜 출력
            Console.WriteLine("CREATE DATE : " + item.CreationTime);
            */
            try
            {
                connection.Open();
                DataSet tds = new DataSet();
                string searchPath = dirPath.Replace("\\", "/");
                

                foreach (string name in filelist)
                {
                    tds = new DataSet();

                    MySqlCommand cmd = new MySqlCommand("SELECT * FROM document WHERE PATH='" + searchPath + "'and NAME='" + name + "'", connection);

                    MySqlDataAdapter adp = new MySqlDataAdapter(cmd);
                    adp.Fill(tds);
                    //Console.WriteLine(tds);
                    if (tds.Tables[0].Rows.Count == 0) //비어있으면
                    {
                        //Console.WriteLine("없음");
                        notinDBfiles.Add(name);
                        //GetTag(dirPath, name);
                    }
                    else
                    {
                        //Console.WriteLine(name);
                    }
                }
            }
            catch (MySqlException ex)
            {
                connection.Close();
                //Console.WriteLine("안연결");
            }

            finally
            {
                connection.Close();
            }

            string command = dirPath+ " ";
            foreach (string name in notinDBfiles) {
                command += name + " ";
            }
            //Console.WriteLine(command);
            GetTag(command);
            
        }
        
        public void GetTag(string files)
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
                    //WorkingDirectory = @"C:\Users\pyj\MyWorks\AutomaticDocumentClassificationService\Scoring\",
                    WorkingDirectory = @"C:\Users\YooJin\Desktop\AutomaticDocumentClassificationService\Scoring\",
                    //WorkingDirectory = @"C:\Users\소정\Desktop\졸업프로젝트\AutomaticDocumentClassificationService\Scoring\",
                    WindowStyle = ProcessWindowStyle.Hidden,
                    CreateNoWindow = true
                }
            };

            process.Start();
            // Pass multiple commands to cmd.exe
            using (var sw = process.StandardInput)
            {
                if (sw.BaseStream.CanWrite)
                {
                    // Vital to activate Anaconda
                    sw.WriteLine(@"C:\Temp\Anaconda3\Scripts\activate.bat");
                    //sw.WriteLine(@"C:\Users\user\Anaconda3\Scripts\activate.bat");
                    //sw.WriteLine(@"C:\ProgramData\Anaconda3\Scripts\activate.bat");
                    //sw.WriteLine(@"C:\Users\pyj\Anaconda3\Scripts\activate.bat");
                    // Activate your environment
                    //sw.WriteLine("activate tensorflow");
                    // Any other commands you want to run
                    //sw.WriteLine("set KERAS_BACKEND=tensorflow");
                    // run your script. You can also pass in arguments
                    string command = @"python C:\Users\YooJin\Desktop\AutomaticDocumentClassificationService\test\modelTagging.py " + files;
                    //string command = @"python C:\Users\소정\Desktop\졸업프로젝트\AutomaticDocumentClassificationService\Scoring\Tagging.py " + this.folderpath;
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
                    //Console.WriteLine(path + " "+form+" "+context+" "+name+"\n");
                }
                //FileList.ItemsSource = Items.GetInstance();
            }

            FreeConsole();

            /*
            for (int i = 0; i < itemslist.Count; i++) {
                //Console.WriteLine(itemslist[i].Path);
                string p = itemslist[i].Path.Replace("\\", "/");
                Console.WriteLine(p);
            }*/
            
            
        }

        void GetItems()
        {
            int id;
            string path, form, context, c, name, sql1, sql2;

            try
            {
                using (MySqlConnection conn = new MySqlConnection(db_information))
                {
                    conn.Open();
                    if (conn.State == System.Data.ConnectionState.Open)
                        MessageBox.Show("서버에 연결");

                    DataSet tds = new DataSet();
                    MySqlCommand cmd = new MySqlCommand("SELECT * FROM document;", conn);
                    MySqlDataAdapter adp = new MySqlDataAdapter(cmd);
                    adp.Fill(tds);

                    id = tds.Tables[0].Rows.Count;
                    
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
            catch (Exception ex) { MessageBox.Show(ex.Message); }

        }
    }

}
