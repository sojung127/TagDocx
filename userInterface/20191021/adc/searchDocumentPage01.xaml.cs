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



namespace adc
{
    /// <summary>
    /// searchDocumentPage01.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class searchDocumentPage01 : Page
    {
        List<string> tags;

        public searchDocumentPage01(List<string> vs)
        {
            InitializeComponent();
            tags = vs;
        }
        
        
        private void BtntoMain(object sender, RoutedEventArgs e)
        {
            Home page = new Home();
            NavigationService.Navigate(page);
        }
        
       // data Table --> replace to DB later
          private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            //DataTable 생성
            DataTable dataTable = new DataTable();
            var dtkey = new DataColumn[1];

            //컬럼 생성
            dataTable.Columns.Add("ID", typeof(string));
            dataTable.Columns.Add("DATAPATH", typeof(string));
            dataTable.Columns.Add("TTAGLIST", typeof(string));
            dataTable.Columns.Add("CTAGLIST", typeof(string));
            dataTable.Columns.Add("FOLDERPATH", typeof(string));
            
  

            //데이터 생성
            // 이전 페이지에서 선택한 태그들
            // tags에 리스트로 담겨있고 ctags에 문자열로 모았음(list, string 둘중 편한 것 선택)
            string ctags = "";
            foreach (string i in tags)
            {
                ctags = ctags +i+ " " ;
                
            }
            
            dataTable.Rows.Add(new string[] { "1", "1.pdf", "형식1"," 태그1 태그2 태그3", "C:\\capston" });
            dataTable.Rows.Add(new string[] { "2", "2.pdf", "형식1"," 태그1 태그3", "C:\\capston" });
            dataTable.Rows.Add(new string[] { "3", "3.pdf", "형식1"," 태그1 ", "C:\\capston" });
            dataTable.Rows.Add(new string[] { "4", "4.pdf", "형식1"," 태그2", "C:\\capston" });
            dataTable.Rows.Add(new string[] { "5", "5.pdf", "형식1"," 태그3 ", "C:\\capston" });

            //seperated content tag string seperated by comma
            string commaSeperatedString = String.Join(",", dataTable.AsEnumerable().Select(x => x.Field<string>("CTAGLIST").ToString()).ToArray());
            //DataTable의 Default View를 바인딩하기 (원본 데이터테이블)
            dataGrid1.ItemsSource = dataTable.DefaultView;

            // datacolumn으로 primary key 설정
            DataColumn[] primarykey = new DataColumn[1];
            primarykey[0] = dataTable.Columns["ID"];

            // 복합키 설정 
            dataTable.PrimaryKey = primarykey;

            // 임시 데이터 테이블 
            DataTable semiTable = new DataTable();
            semiTable.Columns.Add("ID", typeof(string));
            semiTable.Columns.Add("DATAPATH", typeof(string));
            semiTable.Columns.Add("TTAGLIST", typeof(string));
            semiTable.Columns.Add("CTAGLIST", typeof(string));
            semiTable.Columns.Add("FOLDERPATH", typeof(string));

            // 결과 데이터 테이블 
            DataTable resultTable = new DataTable();
            resultTable.Columns.Add("ID", typeof(string));
            resultTable.Columns.Add("DATAPATH", typeof(string));
            resultTable.Columns.Add("TTAGLIST", typeof(string));
            resultTable.Columns.Add("CTAGLIST", typeof(string));
            resultTable.Columns.Add("FOLDERPATH", typeof(string));
           

            Console.WriteLine("형식태그");
            // 1. 형식태그 선별 
            string typeTag = "TTAGLIST = '형식1'";
            DataRow[] semiRows = dataTable.Select(typeTag);

            /*
            for (int i = 0; i < semiRows.Length; i++)
            {
                // resultTable.Rows.Add(semiRows[i]); //
                Console.Write(semiRows[i][1]);
                Console.WriteLine(semiRows[i][3]);
            }
            */
            //신규 데이터 테이블에 row 추가
            foreach (DataRow dr in semiRows)
            {
                semiTable.Rows.Add(dr.ItemArray); 
            }
                   
            Console.WriteLine(semiTable.Rows.Count);
            for (int i = 0; i < semiTable.Rows.Count; i++)
            {
                for(int col = 0; col < semiTable.Columns.Count; col++)
                {
                    Console.Write("{0}", semiTable.Rows[i][col].ToString());
                 }
                Console.WriteLine(" ");
             }
            

            // 2. 내용태그 선별 
            Console.WriteLine("내용태그");
           // string contentTag = "CTAGLIST LIKE'%태그1 태그3%'";
            Regex reg = new Regex(@".*태그1.*태그3.*"); // 태그1*태그3

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
                    Console.Write("{0}", resultTable.Rows[i][col].ToString());
                }
                Console.WriteLine(" ");
            }
            //resultTable의 Default View를 바인딩하기 (원본 데이터테이블)
            //dataGrid1.ItemsSource = resultTable.DefaultView; //re

            /*
            string sortOrder = "DATAPATH ASC";
            // DataRow[] finalRows = dataTable.Select(contentTag, sortOrder);
            DataRow[] finalRows = semiTable.Select(contentTag);
            foreach (DataRow dr2 in finalRows)
            {
                resultTable.Rows.Add(dr2.ItemArray);
            }
            Console.WriteLine(resultTable.Rows.Count);


            for (int i = 0; i < finalRows.Length; i++)
            {
                Console.Write(finalRows[i][0]);
                Console.WriteLine(finalRows[i][3]);
            }
            */


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
