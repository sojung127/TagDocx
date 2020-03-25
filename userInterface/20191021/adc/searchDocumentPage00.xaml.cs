using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using System;
using System.Diagnostics;
using System.ComponentModel;
using System.IO;
namespace adc
{
    /// <summary>
    /// searchDocumentPage00.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class searchDocumentPage00 : Page
    {
        public searchDocumentPage00()
        {
            InitializeComponent();
        }

        private void BtnNextStep(object sender, RoutedEventArgs e)
        {
            searchDocumentPage01 page = new searchDocumentPage01();
            NavigationService.Navigate(page);
        }

        private void BtnPreStep(object sender, RoutedEventArgs e)
        {
            searchDocumentPage page = new searchDocumentPage();
            NavigationService.Navigate(page);
        }
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {



        }
        private void textBox1_TextChanged(object sender, TextChangedEventArgs e)
        {

        }



        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Home page = new Home();
            NavigationService.Navigate(page);
        }

        private void TextBox_TextChanged_1(object sender, TextChangedEventArgs e)
        {

        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            searchDocumentPage01 page = new searchDocumentPage01();
            NavigationService.Navigate(page);
        }
    }
}
            /*ProcessStartInfo psi = new ProcessStartInfo();
            //C:\Temp\Anaconda3\envs\tensorflow
            psi.FileName = @"C:\Temp\Anaconda3\envs\tensorflow\python.exe";//파이썬 설치 경로(아나콘다 자기가 사용하는 가상환경 폴더 경로)
            psi.Arguments = $"\"C:\\Users\\소정\\Desktop\\졸업프로젝트\\AutomaticDocumentClassificationService\\Scoring\\tag_printing.py\"";

            psi.UseShellExecute = false;
            psi.CreateNoWindow = true;
            psi.RedirectStandardOutput = true;
            psi.RedirectStandardError = true;

            var errors = "";
            var results = "";

            using (var process = Process.Start(psi))
            {
                errors = process.StandardError.ReadToEnd();
                results = process.StandardOutput.ReadToEnd();
            }

            Console.WriteLine(errors);
            Console.WriteLine(results);
            //

            // Set working directory and create process
            var workingDirectory = Path.GetFullPath("Scripts");
            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "cmd.exe",
                    RedirectStandardInput = true,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    WorkingDirectory = "C:\\Users\\소정\\Desktop\\졸업프로젝트\\AutomaticDocumentClassificationService\\Scoring\\"
                }
            };
            process.Start();
            // Pass multiple commands to cmd.exe
            using (var sw = process.StandardInput)
            {
                if (sw.BaseStream.CanWrite)
                {
                    // Vital to activate Anaconda
                    sw.WriteLine("C:\\Temp\\Anaconda3\\Scripts\\activate.bat");
                    // Activate your environment
                    sw.WriteLine("activate tensorflow");
                    // Any other commands you want to run
                    sw.WriteLine("set KERAS_BACKEND=tensorflow");
                    // run your script. You can also pass in arguments
                    sw.WriteLine("python C:\\Users\\소정\\Desktop\\졸업프로젝트\\AutomaticDocumentClassificationService\\Scoring\\tag_printing.py");
                    sw.WriteLine("../Dataset/기사/사회/");
                }
            }

            // read multiple output lines
            while (!process.StandardOutput.EndOfStream)
            {
                var line = process.StandardOutput.ReadLine();
                Console.WriteLine(line);
            }
            


        }



    }
}
 */