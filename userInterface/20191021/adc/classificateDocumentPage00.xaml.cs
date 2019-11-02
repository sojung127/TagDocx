using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace adc
{
    /// <summary>
    /// setTagPage.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class classificateDocumentPage00 : Page
    {
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

        }

    }
}
