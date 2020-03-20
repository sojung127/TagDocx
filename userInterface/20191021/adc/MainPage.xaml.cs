using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using System.Runtime.Serialization;

namespace A.D.C.S
{
    /// <summary>
    /// Home.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainPage : Page
    {
        public MainPage()
        {

        }



        private void GoTagPage(object sender, RoutedEventArgs e)
        {
            TagResultPage tagresultpage = new TagResultPage();
            NavigationService.Navigate(tagresultpage);
        }

        private void BtnSearchDocPage(object sender, RoutedEventArgs e)
        {
            searchDocumentPage page = new searchDocumentPage();
            NavigationService.Navigate(page);
        }

        private void BtnClassificateDocPage(object sender, RoutedEventArgs e)
        {
            classificateDocumentPage page = new classificateDocumentPage();
            NavigationService.Navigate(page);
        }
    }
}
