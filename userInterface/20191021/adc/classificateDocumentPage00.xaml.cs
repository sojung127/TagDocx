using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.WindowsAPICodePack.Dialogs;
using MySql.Data.MySqlClient;
using System.Data;

namespace adc
{
    /// <summary>
    /// setTagPage.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class classificateDocumentPage00 : Page
    {
        string selectedFolder;
        DataSet ds = new DataSet(); //document table


        public classificateDocumentPage00()
        {
            InitializeComponent();
        }

        private void BtnPreStep(object sender, RoutedEventArgs e)
        {
            classificateDocumentPage page = new classificateDocumentPage();
            NavigationService.Navigate(page);
        }
        private void BtnNextStep(object sender, RoutedEventArgs e)
        {
            //불러온 datadet 다음 페이지로 넘겨주기
            classificateDocumentPage01 page = new classificateDocumentPage01(ds);
            NavigationService.Navigate(page);
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
                folderName.Text = dialog.FileName; // 테스트용, 폴더 선택이 완료되면 선택된 폴더를 label에 출력
                selectedFolder = dialog.FileName; //선택된 폴더이름저장
            }
        }
        private void BtnFindDoc(object sender, RoutedEventArgs e)
        {
            string connectionString = "SERVER=localhost;DATABASE=adcs;UID=root;PASSWORD=1771094;";
            MySqlConnection connection = new MySqlConnection(connectionString);
            try
            {
                connection.Open();
                /*MySqlCommand cmd = new MySqlCommand("SELECT * FROM document WHERE PATH='"+selectedFolder+"'", connection);
                MySqlDataAdapter adp = new MySqlDataAdapter(cmd);
                // 윈도우 폼의 LoadDataBinding에 데이터 넣기
                adp.Fill(ds, "LoadDataBinding");
                dataGridCustomers.DataContext = ds;*/

                MySqlCommand cmd = new MySqlCommand("SELECT * FROM document inner join content on document.ID = content.ID where document.PATH='" + selectedFolder + "'", connection);
                MySqlDataAdapter adp = new MySqlDataAdapter(cmd);
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
    }
}
