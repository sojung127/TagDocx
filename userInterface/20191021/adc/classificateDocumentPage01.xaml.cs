using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.WindowsAPICodePack.Dialogs;
using System.Data;

namespace adc
{
    /// <summary>
    /// setTagPage.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class classificateDocumentPage01 : Page
    {
        string folderName = null;
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();

        public classificateDocumentPage01(DataSet data)
        {
            InitializeComponent();
            this.ds = data;
            dt = ds.Tables[0]; //datatable로 바꿈
            getTypeTag();
        }

        public void getTypeTag()
        {
            DataRow[] rows = dt.Select();
            string tmp="";//빈문자열
            for(int i = 0; i < rows.Length; i++)
            {
                tmp=tmp+" "+rows[i]["TYPE_TAG"];
          
            }
            임시태그목록.Content = tmp;
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
