using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using System;
using System.Diagnostics;
using System.ComponentModel;
using System.IO;

using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
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
        List<string> selected;
        public searchDocumentPage00()
        {
            InitializeComponent();
            selected = new List<string>();

            var connectionString = "SERVER=localhost;DATABASE=adcs;UID=root;PASSWORD=ewhayeeun;";
            var connection = new MySqlConnection(connectionString);
            try
            {
                connection.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT DISTINCT CONTENT_TAG FROM CONTENT", connection);
                MySqlDataAdapter adp = new MySqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                // 윈도우 폼의 LoadDataBinding에 데이터 넣기
                adp.Fill(ds, "LoadDataBinding");
                
                listbox1.DataContext = ds;
                
            }
            catch (MySqlException ex)
            {
               // MessageBox.Show(ex.ToString());
                Console.WriteLine("DB 연결 실패" + "(" + ex.Message + ")");
            }
            finally
            {
                connection.Close();
              
            }
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

            string connectionString = "SERVER=localhost;DATABASE=adcs;UID=root;PASSWORD=ewhayeeun;";
            var connection = new MySqlConnection(connectionString);
            try
            {
                connection.Open();
                TextBox tb = sender as TextBox;
                string sql = String.Concat("SELECT DISTINCT CONTENT_TAG FROM CONTENT where content_tag like '%", tb.Text);
                sql = String.Concat(sql, "%'");
                MySqlCommand cmd = new MySqlCommand(sql, connection);
                MySqlDataAdapter adp = new MySqlDataAdapter(cmd);
                DataSet ds = new DataSet();

                // 윈도우 폼의 LoadDataBinding에 데이터 넣기
                adp.Fill(ds, "LoadDataBinding");
                listbox1.DataContext = ds;
                
            }
            catch (MySqlException ex)
            {
               // MessageBox.Show(ex.ToString());
                Console.WriteLine("DB 연결 실패" + "(" + ex.Message + ")");
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


        private void FindButton_Clicked(object sender, RoutedEventArgs e)
        {
            for(int i = 0; i < 4; i++)
            {
                string content = listBox2.Items[i].ToString();
                int len = "system.windows.controls.listboxitem:".Length;
                if (listBox2.Items[i].ToString().Length < len)
                {
                    if (String.Compare(content, "선택해주세요") != 0)
                    {
                        selected.Add(content);
                    }
                }
                
            }
            
            //입력된 태그리스트 전달
            searchDocumentPage01 page = new searchDocumentPage01(selected);
            NavigationService.Navigate(page);

           

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
                
            }
            listBox2.Items[0] = btn.Content.ToString();

        }

        private void Listbox1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var row = ((DataRowView) listbox1.SelectedItem).Row;
            string selectedValue = row[0].ToString();
            bool changed = false;
            
           

            for (int i = 1; i <= 3; i++)
            {
                int len = "system.windows.controls.listboxitem:".Length;
                if (listBox2.Items[i].ToString().Length > len)
                {
                    if(String.Compare(listBox2.Items[i].ToString().Substring(len)," 선택해주세요")==0)
                    {
                        listBox2.Items[i] = selectedValue;
                        changed = true;
                        break;
                    }
                }
                

               
                else if (String.Compare(listBox2.Items[i].ToString(),"선택해주세요")==0)
                {
                    listBox2.Items[i] = selectedValue;
                    changed = true;
                    break;
                }
            }
            if (!changed)
            {
                MessageBox.Show("내용태그는 3개까지 선택가능합니다.");
            }

            //listBox2.Items[1] = selectedValue;
        }


        private void ListBox2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string empty = "선택해주세요";
            if (listBox2.SelectedIndex != -1)
                listBox2.Items[listBox2.SelectedIndex] = empty;

        }
    }
}

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
