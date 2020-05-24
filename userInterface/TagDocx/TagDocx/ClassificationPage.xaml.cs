using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.WindowsAPICodePack.Dialogs;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Data;

namespace TagDocx
{
    /// <summary>
    /// setTagPage.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class ClassificationPage : Page
    {
        string selectedFolder;
        DataSet ds = new DataSet(); //document table


        public ClassificationPage()
        {
            InitializeComponent();
        }

        private void BtnPreStep(object sender, RoutedEventArgs e)
        {
            MainPage page = new MainPage();
            NavigationService.Navigate(page);
        }
        private void BtnNextStep(object sender, RoutedEventArgs e)
        {
            //불러온 datadet 다음 페이지로 넘겨주기
            ClassificationPage01 page = new ClassificationPage01(ds);
            NavigationService.Navigate(page);
            FindDoc();  //폴더에서 문서들가져오기
        }


        private void BtnFindFolder(object sender, RoutedEventArgs e)
        {
            // CommonOpenFileDialog 클래스 생성
            CommonOpenFileDialog dialog = new CommonOpenFileDialog();
            // 처음 보여줄 폴더 설정(안해도 됨)
            //dialog.InitialDirectory = "";
            dialog.IsFolderPicker = true;
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {

                folderName.Text = dialog.FileName.Replace("\\", ""); // 테스트용, 폴더 선택이 완료되면 선택된 폴더를 label에 출력
                selectedFolder = dialog.FileName.Replace("\\", ""); ; //선택된 폴더이름저장
            }
        }
        private void FindDoc()
        {

            // string connectionString = "SERVER=localhost;DATABASE=adcs;UID=root;PASSWORD=1771094;";
            string connectionString = @"SERVER=127.0.0.1;DATABASE=adcs;UID=godocx;PASSWORD=486;";
            MySqlConnection connection = new MySqlConnection(connectionString);
            try
            {
                connection.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM document WHERE PATH='" + selectedFolder + "'", connection);
                /*MySqlDataAdapter adp = new MySqlDataAdapter(cmd);
                // 윈도우 폼의 LoadDataBinding에 데이터 넣기
                adp.Fill(ds, "LoadDataBinding");
                dataGridCustomers.DataContext = ds;*/

                //MySqlCommand cmd = new MySqlCommand("SELECT * FROM document inner join content on document.ID = content.ID where document.PATH='" + selectedFolder + "'", connection);
                MySqlDataAdapter adp = new MySqlDataAdapter(cmd);
                // 윈도우 폼의 LoadDataBinding에 데이터 넣기
                adp.Fill(ds, "LoadDataBinding");
                //dataGridCustomers.DataContext = ds;

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
    }
}
