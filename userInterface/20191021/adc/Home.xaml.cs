using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace adc
{
    /// <summary>
    /// Home.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class Home : Page
    {
        public Home()
        {
            InitializeComponent();
        }



        private void BtnSetTagPage(object sender, RoutedEventArgs e)
        {
            setTagPage page = new setTagPage();
            NavigationService.Navigate(page);
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
