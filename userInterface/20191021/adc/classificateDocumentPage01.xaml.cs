using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.WindowsAPICodePack.Dialogs; 

namespace adc
{
    /// <summary>
    /// setTagPage.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class classificateDocumentPage01 : Page
    {
        string folderName = null;

        public classificateDocumentPage01()
        {
            InitializeComponent();
        }

        private void BtnPreStep(object sender, RoutedEventArgs e)
        {
            classificateDocumentPage00 page = new classificateDocumentPage00();
            NavigationService.Navigate(page);
        }
        private void BtnNextStep(object sender, RoutedEventArgs e)
        {
            classificateDocumentPage02 page = new classificateDocumentPage02();
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
                folderName = dialog.FileName; // 테스트용, 폴더 선택이 완료되면 선택된 폴더를 label에 출력
            }
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void 묶음에추가_Click(object sender, RoutedEventArgs e)
        {

        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
