using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace adc
{
    /// <summary>
    /// searchDocumentPage00.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class searchDocumentPage00 : Page
    {
        public searchDocumentPage00()
        {
            InitializeComponent();
        }

        private void BtnNextStep(object sender, RoutedEventArgs e)
        {
            searchDocumentPage01 page = new searchDocumentPage01();
            NavigationService.Navigate(page);
        }

        private void BtnPreStep(object sender, RoutedEventArgs e)
        {
            searchDocumentPage page = new searchDocumentPage();
            NavigationService.Navigate(page);
        }
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
        private void textBox1_TextChanged(object sender, TextChangedEventArgs e)
        {
            
        }
        


        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void TextBox_TextChanged_1(object sender, TextChangedEventArgs e)
        {

        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

        }
    }
}
