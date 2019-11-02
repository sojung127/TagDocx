using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace adc
{
    /// <summary>
    /// searchDocumentPage01.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class searchDocumentPage01 : Page
    {
        private void BtnNextStep(object sender, RoutedEventArgs e)
        {

        }

        private void BtnPreStep(object sender, RoutedEventArgs e)
        {
            searchDocumentPage00 page = new searchDocumentPage00();
            NavigationService.Navigate(page);
        }
    }
}
