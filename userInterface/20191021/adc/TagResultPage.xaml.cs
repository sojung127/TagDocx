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
using IronPython;
using IronPython.Hosting;
using IronPython.Runtime;
using IronPython.Modules;
using System;
using System.IO;
using System.Diagnostics;
using System.ComponentModel;

namespace adc
{
    /// <summary>
    /// TagResultPage.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class TagResultPage : Page
    {
        string folderpath;

        public TagResultPage()
        {
            InitializeComponent();
            
        }

        public TagResultPage(string path) : this()
        {
            this.folderpath = path;
            //this.Loaded += new RoutedEventHandler(PathLoaded);
            FolderPath.Text = this.folderpath;
            GetTag();
        }

        private void GoToMainButton_Click(object sender, RoutedEventArgs e)
        {
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
                    WorkingDirectory = @"C:\Users\YooJin\Desktop\AutomaticDocumentClassificationService\Scoring\"
                }
            };
            process.Start();
            // Pass multiple commands to cmd.exe
            using (var sw = process.StandardInput)
            {
                if (sw.BaseStream.CanWrite)
                {
                    // Vital to activate Anaconda
                    sw.WriteLine(@"C:\ProgramData\Anaconda3\Scripts\activate.bat");
                    // Activate your environment
                    //sw.WriteLine("activate tensorflow");
                    // Any other commands you want to run
                    //sw.WriteLine("set KERAS_BACKEND=tensorflow");
                    // run your script. You can also pass in arguments
                    string command = @"python C:\Users\YooJin\Desktop\AutomaticDocumentClassificationService\Scoring\Tagging.py " + this.folderpath;
                    sw.WriteLine(command);

                }
            }

            // read multiple output lines
            while (!process.StandardOutput.EndOfStream)
            {
                var line = process.StandardOutput.ReadLine();
                Console.WriteLine(line.GetType());
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
    }
}
