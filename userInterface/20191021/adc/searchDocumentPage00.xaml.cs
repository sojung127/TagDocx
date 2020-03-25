using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using System;
using System.Diagnostics;
using System.ComponentModel;
using System.IO;

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
using MySql.Data.MySqlClient;
using System.Data;

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
            string connectionString = "SERVER=localhost;DATABASE=adcs;UID=root;";

            MySqlConnection connection = new MySqlConnection(connectionString);
            try
            {
                connection.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT CONTENT_TAG FROM CONTENT", connection);
                MySqlDataAdapter adp = new MySqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                // 윈도우 폼의 LoadDataBinding에 데이터 넣기
                adp.Fill(ds, "LoadDataBinding");
                dataGridCustomers.DataContext = ds;
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                connection.Close();
            }
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

            string connectionString = "SERVER=localhost;DATABASE=adcs;UID=root;";

            MySqlConnection connection = new MySqlConnection(connectionString);
            try
            {
                connection.Open();
                TextBox tb = sender as TextBox;
                string sql = String.Concat("SELECT CONTENT_TAG FROM CONTENT where content_tag like '%", tb.Text);
                sql = String.Concat(sql, "%'");
                MySqlCommand cmd = new MySqlCommand(sql, connection);
                MySqlDataAdapter adp = new MySqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                // 윈도우 폼의 LoadDataBinding에 데이터 넣기
                adp.Fill(ds, "LoadDataBinding");
                dataGridCustomers.DataContext = ds;
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                connection.Close();
            }

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

            /*
             * psi 이용 버전1 (가상환경 켜기가 안됨)
             * ProcessStartInfo psi = new ProcessStartInfo();
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

            * psi 이용 버전2 (가상환경 설정 가능 오류는 뜨지 않으나 출력에 문제가 있음)
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
                    //sw.WriteLine("set KERAS_BACKEND=tensorflow");
                    // run your script. You can also pass in arguments
                    sw.WriteLine("python C:\\Users\\소정\\Desktop\\졸업프로젝트\\AutomaticDocumentClassificationService\\Scoring\\tag_printing.py");
                    sw.WriteLine("../Dataset/기사/사회/");
                }
            }

            // read multiple output lines
            while (!process.StandardOutput.EndOfStream)
            {
                var line = process.StandardOutput.ReadToEnd();
                Console.WriteLine(line);
            }*/

        }

        private void TypeButton_Clicked(object sender, RoutedEventArgs e)
        {
            var tmp = new SolidColorBrush(Colors.Yellow);
            var yellow = tmp.ToString();
            var selectBtn = types.Children.OfType<Button>().FirstOrDefault(r => r.Background.ToString() == yellow);
            Button btn = sender as Button;
            btn.Background = new SolidColorBrush(Colors.Yellow);
            if (selectBtn != null)
            {
                selectBtn.Background = new SolidColorBrush(Colors.LightGray);
                RefreshTagList(selectBtn.Content.ToString());
                ListBox listBox = listBox2;
                listBox.Items[0] = btn.Content.ToString();
            }


        }

        private void RefreshTagList(string clicked)
        {

        }
    }
}
