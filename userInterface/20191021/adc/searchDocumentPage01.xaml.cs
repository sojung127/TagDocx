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



namespace adc
{
    /// <summary>
    /// searchDocumentPage01.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class searchDocumentPage01 : Page
    {
        public searchDocumentPage01()
        {
            InitializeComponent();
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

            //컬럼 생성
            dataTable.Columns.Add("DATAPATH", typeof(string));
            dataTable.Columns.Add("TAGLIST", typeof(string));
            dataTable.Columns.Add("FOLDERPATH", typeof(string));

            //데이터 생성
            dataTable.Rows.Add(new string[] { "1.pdf", "태그1 태그2 태그3", "C:\\capston" });
            dataTable.Rows.Add(new string[] { "2.pdf", "태그1 태그3", "C:\\capston" });
            dataTable.Rows.Add(new string[] { "3.pdf", "태그1 ", "C:\\capston" });
            dataTable.Rows.Add(new string[] { "4.pdf", "태그2", "C:\\capston" });
            dataTable.Rows.Add(new string[] { "5.pdf", "태그3 ", "C:\\capston" });

            //DataTable의 Default View를 바인딩하기
            dataGrid1.ItemsSource = dataTable.DefaultView;

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

    }
}
