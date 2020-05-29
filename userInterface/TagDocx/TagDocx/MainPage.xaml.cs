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
            //저장되어있던 사용자 태깅폴더지정string변수 값 가져오기
            checkDir();
        
        }

        private void checkDir()
        {
            string customerDir = Properties.Settings.Default.customerDir;
            if (customerDir == "") //설정 전
            {
                notice.Content="정기적 태깅 작업을 할 폴더를 지정해주세요.";
            }
            else{//lastTagedTime 값을notice에 넣어 화면에 출력하기
                notice.Content = Properties.Settings.Default.lastTagedTime;
            }
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

        private void BtnSetting(object sender, RoutedEventArgs e)
        {
            SettingPage page = new SettingPage();
            NavigationService.Navigate(page);
        }
    }


}
