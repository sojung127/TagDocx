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


        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void 묶음에추가_Click(object sender, RoutedEventArgs e)
        {

        }
        private void 묶음에서삭제_Click(object sender, RoutedEventArgs e)
        {

        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
