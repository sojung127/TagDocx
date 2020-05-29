using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TagDocx
{
    /// <summary>
    /// MainPage.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private void ToSearch(object sender, RoutedEventArgs e)
        {

            SearchPage page = new SearchPage();
            NavigationService.Navigate(page);

        }

        private void BtnClassificateDocPage(object sender, RoutedEventArgs e)
        {
            ClassificationPage page = new ClassificationPage();
            NavigationService.Navigate(page);
        }
    }


}
