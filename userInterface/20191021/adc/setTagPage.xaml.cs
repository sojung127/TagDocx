using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using System.Windows.Forms;

namespace adc {
    /// <summary>
    /// setTagPage.xaml에 대한 상호 작용 논리
    /// </summary>

    public partial class setTagPage : Page
{

    public setTagPage()
    {
        InitializeComponent();

        }
    private void BtnNextStep(object sender, RoutedEventArgs e)
    {
            string path = FolderPath.Text;
            TagResultPage pg = new TagResultPage(path);

            NavigationService.Navigate(pg);
        //NavigationService.Navigate(pg);
    }
    private void BtnBackToHome(object sender, RoutedEventArgs e)
    {
        Home page = new Home();
        NavigationService.Navigate(page);
    }

    private void FolerBrowse_Click(object sender, RoutedEventArgs e)
    {
            FolderBrowserDialog Browse = new FolderBrowserDialog();

            if (Browse.ShowDialog() == DialogResult.OK) {
                FolderPath.Text = Browse.SelectedPath;
            }

    }

} }
