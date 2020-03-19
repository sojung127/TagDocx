using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.WindowsAPICodePack.Dialogs; 

namespace adc
{
    /// <summary>
    /// setTagPage.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class classificateDocumentPage00 : Page
    {
        string selectedFolder;

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
            classificateDocumentPage01 page = new classificateDocumentPage01();
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
    }
}
