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


        public void GetTag()
        {
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
