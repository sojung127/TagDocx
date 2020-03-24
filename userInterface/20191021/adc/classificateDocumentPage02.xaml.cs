using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.WindowsAPICodePack.Dialogs;
using System.Data;
using System.Collections.Generic; //List collection 써야하니까!
using System.Linq; //리스트 중복제거 함수 쓰려고 추가
using System;

namespace adc
{
    /// <summary>
    /// setTagPage.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class classificateDocumentPage02 : Page
    {
        string folderName = null;
       
 
        DataTable dt = new DataTable();

        public classificateDocumentPage02( DataTable dt)
        {
            InitializeComponent();
           
            this.dt = dt; //문서 datatable 가져오기
        }

        private void BtnEnd(object sender, RoutedEventArgs e)
        {
            classificateDocumentPage page = new classificateDocumentPage(); //일단 메인으로
            NavigationService.Navigate(page);
        }

        private void BtnFindFolder(object sender, RoutedEventArgs e)
        {
            // CommonOpenFileDialog 클래스 생성
            CommonOpenFileDialog dialog = new CommonOpenFileDialog();
            // 처음 보여줄 폴더 설정(안해도 됨)
            //dialog.InitialDirectory = "";
            dialog.IsFolderPicker = true;
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                folderName = dialog.FileName; // 테스트용, 폴더 선택이 완료되면 선택된 폴더를 label에 출력
            }
        }


        private void TabControl_SelectionChanged(object sender, RoutedEventArgs e)
        {

        }
    }
}
