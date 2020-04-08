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
using System.Data;
using System.Drawing;
using System.Diagnostics;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Collections;
using MySql.Data.MySqlClient;


namespace adc
{
    /// <summary>
    /// searchDocumentPage01.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class searchDocumentPage01 : Page
    {
        List<string> tags;

        public searchDocumentPage01(List<string> vs) // 태그 리스트로 받아옴 
        {
            InitializeComponent();
            tags = vs;

            var connectionString = "SERVER=localhost;DATABASE=adcs;UID=root;PASSWORD=1771094;";
            var connection = new MySqlConnection(connectionString);
            try
            {
                connection.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT DISTINCT CONTENT_TAG FROM CONTENT", connection);
                MySqlDataAdapter adp = new MySqlDataAdapter(cmd);
                //DataSet ds = new DataSet();
                // 윈도우 폼의 LoadDataBinding에 데이터 넣기
                //adp.Fill(ds, "searchDocument00");

                //dataGrid1.DataContext = ds;

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


        private void BtntoMain(object sender, RoutedEventArgs e)
        {
            Home page = new Home();
            NavigationService.Navigate(page);
        }

        // data Table --> replace to DB later
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {

            var  connectionString = "SERVER=localhost;DATABASE=adcs;UID=root";
            var connection = new MySqlConnection(connectionString);
            try
            {
                connection.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT DISTINCT CONTENT_TAG FROM CONTENT", connection);
                MySqlDataAdapter adp = new MySqlDataAdapter(cmd);
                //DataSet ds = new DataSet();
                // 윈도우 폼의 LoadDataBinding에 데이터 넣기
                //adp.Fill(ds, "Page_Loaded");

                //dataGrid1.DataContext = ds;

            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                connection.Close();
            }


            //DataTable 생성
            DataTable dataTable = new DataTable();
            var dtkey = new DataColumn[1];

            //컬럼 생성
            dataTable.Columns.Add("ID", typeof(string));
            dataTable.Columns.Add("DATAPATH", typeof(string));
            dataTable.Columns.Add("TTAGLIST", typeof(string));
            dataTable.Columns.Add("CTAGLIST", typeof(string));
            dataTable.Columns.Add("FOLDERPATH", typeof(string));


            // 이전 페이지에서 선택한 태그들
            // tags에 리스트로 담겨있고 ctags에 문자열로 모았음(list, string 둘중 편한 것 선택
            
                        
            string ttag = "";           // 형식태그 String'형식1'
            string ctags = @"";         // 내용태그 정규표현식 형태로 (@".*태그1.*태그3.*")
            int length = tags.Count();
            if (ttag != "") ttag = "";
            if (ctags != "") ctags = "";
                foreach (string i in tags)
            {
                //ctags = ctags + i 
                if (tags.IndexOf(i) == 0)
                    ttag = ttag + i;
                else ctags = ctags + ".*" + i;
                
            }
            ctags = ctags + ".*";
            Console.WriteLine(" ");
            Console.WriteLine(ttag.GetType());
            Console.WriteLine(ctags);
                      
            dataTable.Rows.Add(new string[] { "1", "1.pdf", "논문", " 여성 봉사 복지", "C:\\capston" });
            dataTable.Rows.Add(new string[] { "2", "2.pdf", "논문", " 여성 복지", "C:\\capston" });
            dataTable.Rows.Add(new string[] { "3", "3.pdf", "논문", " 여성 ", "C:\\capston" });
            dataTable.Rows.Add(new string[] { "4", "4.pdf", "논문", " 봉사", "C:\\capston" });
            dataTable.Rows.Add(new string[] { "5", "5.pdf", "논문", " 복지 ", "C:\\capston" });

            dataTable.Rows.Add(new string[] { "6", "1.pdf", "기사", " 여성 봉사 복지", "C:\\capston" });
            dataTable.Rows.Add(new string[] { "7", "2.pdf", "기사", " 여성 복지", "C:\\capston" });
            dataTable.Rows.Add(new string[] { "8", "3.pdf", "기사", " 여성 ", "C:\\capston" });
            dataTable.Rows.Add(new string[] { "9", "4.pdf", "기사", " 봉사", "C:\\capston" });
            dataTable.Rows.Add(new string[] { "10", "5.pdf", "기사", " 복지 ", "C:\\capston" });

            dataTable.Rows.Add(new string[] { "11", "1.pdf", "지원서", " 여성 봉사 복지", "C:\\capston" });
            dataTable.Rows.Add(new string[] { "12", "2.pdf", "지원서", " 여성 복지", "C:\\capston" });
            dataTable.Rows.Add(new string[] { "13", "3.pdf", "지원서", " 여성 ", "C:\\capston" });
            dataTable.Rows.Add(new string[] { "14", "4.pdf", "지원서", " 봉사", "C:\\capston" });
            dataTable.Rows.Add(new string[] { "15", "5.pdf", "지원서", " 복지 ", "C:\\capston" });

            dataTable.Rows.Add(new string[] { "16", "1.pdf", "공고", " 여성 봉사 복지", "C:\\capston" });
            dataTable.Rows.Add(new string[] { "17", "2.pdf", "공고", " 여성 복지", "C:\\capston" });
            dataTable.Rows.Add(new string[] { "18", "3.pdf", "공고", " 여성 ", "C:\\capston" });
            dataTable.Rows.Add(new string[] { "19", "4.pdf", "공고", " 봉사", "C:\\capston" });
            dataTable.Rows.Add(new string[] { "20", "5.pdf", "공고", " 복지 ", "C:\\capston" });
            //DataTable의 Default View를 바인딩하기 (원본 데이터테이블)

            // datacolumn으로 primary key 설정
            DataColumn[] primarykey = new DataColumn[1];
            primarykey[0] = dataTable.Columns["ID"];

            // 복합키 설정 
            dataTable.PrimaryKey = primarykey;

            // 형식태그 확인 결과 테이블 
            DataTable semiTable = new DataTable();
            semiTable.Columns.Add("ID", typeof(string));
            semiTable.Columns.Add("DATAPATH", typeof(string));
            semiTable.Columns.Add("TTAGLIST", typeof(string));
            semiTable.Columns.Add("CTAGLIST", typeof(string));
            semiTable.Columns.Add("FOLDERPATH", typeof(string));

            // 내용태그 확인 결과 테이블 
            DataTable resultTable = new DataTable();
            resultTable.Columns.Add("ID", typeof(string));
            resultTable.Columns.Add("DATAPATH", typeof(string));
            resultTable.Columns.Add("TTAGLIST", typeof(string));
            resultTable.Columns.Add("CTAGLIST", typeof(string));
            resultTable.Columns.Add("FOLDERPATH", typeof(string));


            // 1. 형식태그 선별 
            Console.WriteLine("형식태그");
            //string typeTag = "TTAGLIST = '논문'";
            string typeTag = string.Format("TTAGLIST = '{0}'", ttag);
            Console.Write(typeTag);
            DataRow[] semiRows = dataTable.Select(typeTag);

            //신규 데이터 테이블에 row 추가
            foreach (DataRow dr in semiRows)
            {
                semiTable.Rows.Add(dr.ItemArray);
            }

            Console.WriteLine(semiTable.Rows.Count);
            for (int i = 0; i < semiTable.Rows.Count; i++)
            {
                for (int col = 0; col < semiTable.Columns.Count; col++)
                {
                    Console.Write("{0}  ", semiTable.Rows[i][col].ToString());
                }
                Console.WriteLine(" ");
            }


            // 2. 내용태그 선별 
            Console.WriteLine("내용태그");
            //Regex reg = new Regex(@".*태그1.*태그3.*"); // 태그1*태그3
            Regex reg = new Regex(ctags);
            //Regex reg = new Regex(@".*여성.*복지.*봉사.*");
            //matching rows
            ArrayList al = new ArrayList();
            foreach (DataRow row in semiTable.Select())
                if (reg.Match(row["CTAGLIST"].ToString()).Success)
                    al.Add(row);
            DataRow[] finalRows = (DataRow[])al.ToArray(typeof(DataRow));

            //display rows 
            foreach (var row in finalRows)
            {
                // Console.WriteLine(row["CTAGLIST"]);
                resultTable.Rows.Add(row.ItemArray);
            }
            Console.WriteLine(resultTable.Rows.Count);
            for (int i = 0; i < resultTable.Rows.Count; i++)
            {
                for (int col = 0; col < resultTable.Columns.Count; col++)
                {
                    Console.Write("{0} ", resultTable.Rows[i][col].ToString());
                }
                Console.WriteLine(" ");
            }
           
            //resultTable의 Default View를 바인딩하기 (원본 데이터테이블)
            dataGrid1.ItemsSource = resultTable.DefaultView; //re

            

        }

        //FolderBrowserDialog + System.Windows.Forms
        private void openFolder_Click(object sender, RoutedEventArgs e)
        {
            String filePath = @"FOLDERPATH";
            System.Diagnostics.Process.Start(filePath);

            Process wordProcess = new Process();
            wordProcess.StartInfo.FileName = filePath;
            wordProcess.StartInfo.UseShellExecute = true;
            wordProcess.Start();

            /*
            Process process = new Process();
            process.StartInfo.UseShell Execute = true;
            process.StartInfo.FileNme = _sSelected;
            process.Start();
            
        }
        //fileBrowserDiaglog 
        private void openFile_click(object sender, RoutedEventArgs e)
        {
            /*
            Process wordProcess = new Process();
            wordProcess.StartInfo.FileName = pathToYourDocument;
            wordProcess.StartInfo.UseShellExecute = true;
            wordProcess.Start();
            */
        }

        private void dataGrid1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }


    }
}