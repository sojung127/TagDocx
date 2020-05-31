using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.WindowsAPICodePack.Dialogs;


namespace TagDocx
{
    /// <summary>
    /// setTagPage.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class SettingPage : Page
    {
        string selectedFolder;

        public SettingPage()
        {
            InitializeComponent();
            string getDir = Properties.Settings.Default.customerDir;
            Console.WriteLine(getDir);
            string[] templi = null;
            if (getDir != "")
            {
                templi = getDir.Split('<');
                for (int i = 0; i < templi.Length; i++)
                {
                    if (templi[i] != "")
                    {  //스플릿 한것중 빈것이 아닐 때 만 항목에 추가 (split했을 때 빈 문자열 결과 방지)
                        flist.Items.Add(templi[i]);
                    }
                }
            }
        }

        private void BtnPreStep(object sender, RoutedEventArgs e)
        {
            MainPage page = new MainPage();
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
                selectedFolder = dialog.FileName; //선택된 폴더이름저장
            }

            flist.Items.Add(selectedFolder);

        }

        private void sDelete(object sender, RoutedEventArgs e)
        {
            flist.Items.Remove(flist.SelectedItem);
        }

        private void aDelete(object sender, RoutedEventArgs e)
        {
            flist.Items.Clear();
        }

        private void gohome(object sender, RoutedEventArgs e)
        {

            flistSave();
            MainPage page = new MainPage();
            NavigationService.Navigate(page);
        }


       
        private void flistSave()
        {
            string temp = null;
            
            for (int i = 0; i < flist.Items.Count; i++)
            {
                temp = temp + flist.Items[i].ToString() + "<";
            }
            Properties.Settings.Default.customerDir = temp;
            Console.WriteLine(temp);
        }
    }
}