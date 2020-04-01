using System;
using System.Collections.Generic;
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
using System.IO;
using System.Diagnostics;
using MySql.Data.MySqlClient;
using System.Text.RegularExpressions;
using System.Collections;

namespace adc
{
    /// <summary>
    /// TagResultPage.xaml에 대한 상호 작용 논리
    /// </summary>
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

        public static List<Items> GetInstance() {
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

    public partial class TagResultPage : Page
    {
        string folderpath;
        static string db_information = @"Server=localhost;Database=adcs;Uid=root;Pwd=1771094;";
        string savingpath;

        public TagResultPage()
        {
            InitializeComponent();
            
        }

        public TagResultPage(string path) : this()
        {
            this.folderpath = path;
            this.savingpath = path;
            //this.Loaded += new RoutedEventHandler(PathLoaded);
            FolderPath.Text = this.folderpath;
            GetTag();
        }

        private void GoToMainButton_Click(object sender, RoutedEventArgs e)
        {
            GetItems();
            Home pg = new Home();
            NavigationService.Navigate(pg);
        }

        [System.Runtime.InteropServices.DllImport("Kernel32")]
        public static extern void FreeConsole();

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
                    WorkingDirectory = @"C:\Users\pyj\MyWorks\AutomaticDocumentClassificationService\Scoring\",
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
                    sw.WriteLine(@"C:\Users\pyj\Anaconda3\Scripts\activate.bat");
                    // Activate your environment
                    //sw.WriteLine("activate tensorflow");
                    // Any other commands you want to run
                    //sw.WriteLine("set KERAS_BACKEND=tensorflow");
                    // run your script. You can also pass in arguments
                    string command = @"python C:\Users\pyj\MyWorks\AutomaticDocumentClassificationService\Scoring\Tagging.py " + this.folderpath;
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

                if (result.Count > 3) {
                    path = result[0].Groups[0].ToString().Substring(5);
                    path = path.Substring(0, path.Length - 2);
                    form = result[1].Groups[0].ToString().Substring(5);
                    form = form.Substring(0, form.Length - 2);
                    context = result[2].Groups[0].ToString().Substring(6);
                    context = context.Substring(0, context.Length - 3);
                    name = result[3].Groups[0].ToString().Substring(5);
                    name = name.Substring(0, name.Length - 2);
                    Items.GetInstance().Add(new Items(count++, path, form, context, name));
                }
                FileList.ItemsSource = Items.GetInstance();
            }

            FreeConsole();
            
            /*
            //1) Create engine
            var engine = IronPython.Hosting.Python.CreateEngine();

            //2) Provide script and arguments
            var script = @"C:\Users\YooJin\Desktop\AutomaticDocumentClassificationService\Scoring\Tagging.py";
            var source = engine.CreateScriptSourceFromFile(script);//실행시킬 파이썬 경로
           
            var argv = new List<string>();
            argv.Add(""); //Tagging.py
            argv.Add(this.folderpath);

            engine.GetSysModule().SetVariable("argv", argv);

            //3)Output redirect
            var elO = engine.Runtime.IO;

            var errors = new MemoryStream();
            elO.SetErrorOutput(errors, Encoding.Default);

            var results = new MemoryStream();
            elO.SetOutput(results, Encoding.Default);

            //4) Execute script
            var scope = engine.CreateScope();

            string AppPath = @"C:\Users\YooJin\Anaconda3\";
            ICollection<string> searchPaths = engine.GetSearchPaths();
            searchPaths.Add(AppPath+@"Lib");
            searchPaths.Add(AppPath + @"Lib\site-packages");
            searchPaths.Add(@"C:\Users\YooJin\AppData\Local\Programs\Python\Python36-32\DLLs");

            engine.SetSearchPaths(searchPaths);

            source.Execute(scope);

            //5)Display output
            string str(byte[] x) => Encoding.Default.GetString(x);

            Console.WriteLine("Errors:");
            Console.WriteLine(str(errors.ToArray()));
            Console.WriteLine();
            Console.WriteLine("REsults:");
            Console.WriteLine(str(results.ToArray()));
            /*
            string result = "";
            try
            {
                var script = @"C:\Users\YooJin\Desktop\AutomaticDocumentClassificationService\Scoring\Tagging.py";
                var source = engine.CreateScriptSourceFromFile(script);//실행시킬 파이썬 경로
                source.Execute(scope);

                var report = scope.GetVariable < Func object= "" >> ("run");// 파이썬 소스에서 정의한 함수 불러오기 <func .....="" param1="" param2="" return=""> >("함수명")
                                                                             //Console.WriteLine(report(id,passwd));
                return = Convert.ToString(report(IDataObject, passwd));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }*/
        }

        void PathLoaded(object sender, RoutedEventArgs e) {
            FolderPath.Text = this.folderpath;
        }

        void GetItems() {
            int id;
            string path, form, context, c, name, sql1, sql2;

            try
            {
                using (MySqlConnection conn = new MySqlConnection(db_information))
                {
                    conn.Open();
                    if (conn.State == System.Data.ConnectionState.Open)
                        MessageBox.Show("서버에 연결");

                    foreach (Items a in FileList.Items)
                    {
                        id = a.Id;
                        path = savingpath;
                        form = a.Form;
                        context = a.Context;
                        name = a.Name;
                        
                        sql1 = "insert into document values("+id+",\""+name+"\",\""+form+ "\",\""+path+"\")";
                        MySqlCommand cmd = new MySqlCommand(sql1, conn);
                        cmd.ExecuteNonQuery();

                        Regex reg = new Regex(@"\'.*?\'");
                        MatchCollection result = reg.Matches(context);
                        foreach (Match mm in result) {
                            c = mm.Groups[0].ToString().Substring(1);
                            c = c.Substring(0, c.Length - 1);
                            //Console.WriteLine(c);
                            sql2 = "insert into content values(" + id + ",\"" + c + "\")";
                            cmd = new MySqlCommand(sql2, conn);
                            cmd.ExecuteNonQuery();

                        }
                        
                        //MessageBox.Show("입력 성공");
                    }
                    
                    conn.Close();
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            
        }
    }
    
}
