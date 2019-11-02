using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace adc
{
    /// <summary>
    /// setTagPage.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class classificateDocumentPage : Page
    {
        public classificateDocumentPage()
        {
            InitializeComponent();
        }

        private void BtnNextStep(object sender, RoutedEventArgs e)
        {
            classificateDocumentPage00 page = new classificateDocumentPage00();
            NavigationService.Navigate(page);
        }

        private void BtnBackToHome(object sender, RoutedEventArgs e)
        {
            Home page = new Home();
            NavigationService.Navigate(page);
        }

    }
}
