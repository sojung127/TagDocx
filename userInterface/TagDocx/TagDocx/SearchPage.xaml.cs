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

using System.Data;
using MySql.Data.MySqlClient;
using System.Configuration;
using System.Diagnostics;
using System.Text.RegularExpressions; //패턴매칭(Regex)등 사용위해 정규식표현 using
using Microsoft.WindowsAPICodePack.Dialogs;
using Microsoft.VisualBasic.FileIO;
using System.Security.Permissions;
using System.IO;
using System.Collections.Specialized;


namespace TagDocx
{
    /// <summary>
    /// SearchPage.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class SearchPage : Page
    {
        string connectionString = "SERVER=localhost;DATABASE=adcs;UID=godocx;PASSWORD=486;";
        int[] selectID=new int[100];
        string[] filePath=new string[100];
        string[] fileName=new string[100];
        MySqlConnection connection;
        int temp;
        public SearchPage()
        {
            InitializeComponent();
            temp = 0;
            connection = new MySqlConnection(connectionString);


            string query_start = "select document.id, name, path, type_tag, c.content_tag, substring_index(document.name, '.', -1) as ext FROM document left join (select id,group_concat(content_tag) as content_tag from content";
            string query_end = " group by id )as c on c.id = document.id where c.id = document.id union select document.id,name,path,null,null,substring_index(document.name, '.', -1) from document where document.id NOT IN (SELECT DISTINCT CONTENT.ID FROM CONTENT);";

            string final_query = query_start + query_end;
            try
            {
                connection.Open();
                MySqlCommand cmd = new MySqlCommand(final_query, connection);
                MySqlDataAdapter adp = new MySqlDataAdapter(cmd);

                DataSet ds = new DataSet();



                adp.Fill(ds, "LoadDataBinding");
                SearchResult.DataContext = ds;
            }
            catch (MySqlException ex)
            {
                Debug.WriteLine(ex.ToString());
            }
            finally
            {
                connection.Close();
            }

        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

            TextBox tb = sender as TextBox;
            string[] words = tb.Text.Split(' ');

            string query_start = "select document.id, name, path, type_tag, c.content_tag, substring_index(name, '.', -1) as ext from document left join (select id,group_concat(content_tag) as content_tag from content where id in "
                + "(select id from content where ";
            string select_content="";
            string select_docu="";


            select_content += "content_tag like '%" + words[0] + "%' ";
            select_docu += "union select id from document where name like '%" + words[0] + "%' or type_tag like '%"+words[0]+"%' ";

            if (words.Length >1)
            {
                for (int i = 1; i < words.Length; i++)
                {
                    if (words[i].Length != 0)
                    {
                        select_content += "or content_tag like '%" + words[i] + "%' ";
                        select_docu += "or name like '%" + words[i] + "%' " +" or type_tag like '%" + words[i] + "%' ";
                    }
                }
            }


            string query_end = ") group by id )as c on c.id = document.id where c.id = document.id union select document.id,name,path,null,null,substring_index(document.name, '.', -1) from document where document.id NOT IN (SELECT DISTINCT CONTENT.ID FROM CONTENT);";

            string final_query = query_start + select_content + select_docu + query_end;
            try
            {
                connection.Open();
                MySqlCommand cmd = new MySqlCommand(final_query, connection);
                MySqlDataAdapter adp = new MySqlDataAdapter(cmd);

                DataSet ds = new DataSet();

                adp.Fill(ds, "LoadDataBinding");
                SearchResult.DataContext = ds;
            }catch (MySqlException ex)
            {
                Debug.WriteLine(ex.ToString());
            }
            finally
            {
                connection.Close();
            }


            

        }
        int selectedItems;
        private void Listbox1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
            ;
            Console.WriteLine(" 현재 선택된 갯수 "+ SearchResult.SelectedItems.Count);
            if (temp == 0)
            {
                selectedItems = SearchResult.SelectedItems.Count;
                if (selectedItems > 100)
                {
                    //MessageBox.Show("최대 100개까지만 선택가능합니다.");
                    Application.Current.Properties["TM_Msg"] = "최대 100개까지만 선택가능합니다.";

                    Window Splash_Message = new ToastMessage();
                    Splash_Message.Show();
                }
                for(int i = 0; i < selectedItems; i++)
                {
                    DataRowView rv = (DataRowView)SearchResult.SelectedItems[i];
                    selectID[i] = int.Parse(rv[0].ToString());
                    fileName[i] = rv[1].ToString();
                    Debug.WriteLine(rv[2].ToString()[rv[2].ToString().Length - 1]);
                    if( rv[2].ToString()[rv[2].ToString().Length-1]!='/')
                        filePath[i] = rv[2].ToString() +"/"+ rv[1].ToString();
                    else
                        filePath[i] = rv[2].ToString() + rv[1].ToString();


                    Debug.WriteLine(filePath[i] + " " + selectID[i]);
                }
            }
        }
        private void menuitem_click(object sender, RoutedEventArgs e)
        {
            if (selectedItems > 1)
            {
                //MessageBox.Show("태그는 하나의 문서에 대해서만 수정가능합니다.");
                Application.Current.Properties["TM_Msg"] = "태그는 하나의 문서에 대해서만 수정가능합니다.";

                Window Splash_Message = new ToastMessage();
                Splash_Message.Show();

            }
            else if (selectedItems == 1)
            {
                if (!File.Exists(filePath[0])){
                    showDeleteMessage();
                    DeleteFromDB(0);

                }
                else
                {
                    Window win = new TagEditWindow(selectID[0],Search);
                    win.Show();

                }

            }
        }

        private void ListDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Debug.WriteLine(filePath[0]);
            
            if (filePath.Length != 0 && selectedItems == 1)
            {
                if(System.IO.File.Exists(filePath[0]))
                    System.Diagnostics.Process.Start(filePath[0]);
                else
                {
                    showDeleteMessage();
                    DeleteFromDB(0);
                }
            }

            
            filePath[0] = "";
        }

        private void fileDelete(object sender, RoutedEventArgs e)
        {
            temp = selectedItems;
            bool allFileExisted = true;
            try
            {
                connection.Open();
                for (int i = 0; i < temp; i++)
                {
                    if (System.IO.File.Exists(filePath[i]))
                    {
                        FileSystem.DeleteFile
                    (
                         filePath[i],
                         UIOption.AllDialogs,
                         RecycleOption.SendToRecycleBin
                    );
                    }
                    else
                    {
                        MySqlCommand cmd2 = new MySqlCommand("delete from content where id=" + selectID[i] + ";", connection);
                        cmd2.ExecuteNonQuery();
                        cmd2 = new MySqlCommand("delete from document where id=" + selectID[i] + ";", connection);
                        cmd2.ExecuteNonQuery();
                        allFileExisted = false;
                        connection.Close();
                        return;
                    }
                    // 파일 정보 DB에서 삭제
                    if (!allFileExisted)
                    {
                        showDeleteMessage();
                    }
                    MySqlCommand cmd = new MySqlCommand("delete from content where id=" + selectID[i] + ";", connection);
                    cmd.ExecuteNonQuery();
                    cmd = new MySqlCommand("delete from document where id=" + selectID[i] + ";", connection);
                    cmd.ExecuteNonQuery();
                }
                connection.Close();

            }
            catch (System.IO.IOException ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                temp = 0;

            }
            Search.Text += " ";
            Search.Text = Search.Text.ToString().TrimEnd();
        }

        private void fileMove(object sender, RoutedEventArgs e)
        {
            
            temp = selectedItems;
            string folder="";
            bool allfileexist = true;
            // 폴더 열기 다이얼로그 생성
            CommonOpenFileDialog fileDialog = new CommonOpenFileDialog();
            fileDialog.IsFolderPicker = true;
            if (fileDialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                folder = fileDialog.FileName;
                folder=folder.Replace("\\","/");
                try
                {
                    for (int i = 0; i < temp; i++)
                    {
                            Debug.WriteLine("이동: " + filePath[i] + " to " + folder + "/" + fileName[i]);
                        if (System.IO.File.Exists(filePath[i]))
                        {

                    connection.Open();
                            System.IO.File.Move(filePath[i], folder + "/" + fileName[i]);
                            MySqlCommand cmd = new MySqlCommand("update document set path='" + folder + "/" + "' where id=" + selectID[i] + ";", connection);
                            cmd.ExecuteNonQuery();
                    connection.Close();

                        }
                        else
                        {
                            allfileexist = false;
                            DeleteFromDB(i);
                        }

                       
                    }
                    if (!allfileexist)
                    {
                        showDeleteMessage();
                    }

                }
                catch (System.IO.IOException ex)
                {
                    Console.WriteLine(ex.Message);
                }
                finally
                {
                    temp = 0;

                }
                Search.Text += " ";
                Search.Text = Search.Text.ToString().TrimEnd();
            }

           
            
        }

        private void fileCopy(object sender, RoutedEventArgs e)
        {
            temp = selectedItems;

            // 선택한 파일들 클립보드에 복사
            bool allFileExist = true;
            bool oneFileExist = false;
            StringCollection paths = new StringCollection();
            for (int i = 0; i < temp; i++)
            {
                if (System.IO.File.Exists(filePath[i]))
                {
                    paths.Add(filePath[i]);
                    oneFileExist = true;
                }
                else
                {
                    DeleteFromDB(i);
                    allFileExist = false;
                }

            }
            if (allFileExist)
            {
                Clipboard.SetFileDropList(paths);
                Application.Current.Properties["TM_Msg"] = "클립보드에 파일이 복사되었습니다.";

                Window Splash_Message = new ToastMessage();
                Splash_Message.Show();

            }else if (oneFileExist)
            {
                Application.Current.Properties["TM_Msg"] = "클립보드에 일부 파일이 복사되었습니다.";

                Window Splash_Message = new ToastMessage();
                Splash_Message.Show();
            }
            else
            {
                showDeleteMessage();
            }
            // 파일리스트 새로고침
            Search.Text += " ";
            Search.Text = Search.Text.ToString().TrimEnd();
        }


        private void Click_Exit(object sender, RoutedEventArgs e)
        {
           Application.Current.Shutdown();
        }

        private void BackToMainPage(object sender, RoutedEventArgs e)
        {
           MainPage page = new MainPage();
           NavigationService.Navigate(page);
        }

        private void SelectAll_Exectued(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("SelectAll Executed");
            SearchResult.SelectAll();
        }

        private void DeselectAll_Executed(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("deSelectAll Executed");
            SearchResult.UnselectAll();

        }

        private void Maximize_Click(object sender, RoutedEventArgs e)
        {

            Application.Current.MainWindow.WindowState = (Application.Current.MainWindow.WindowState == WindowState.Normal) ? WindowState.Maximized : WindowState.Normal;
        }

        private void Minimize_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.WindowState = WindowState.Minimized;
        }

        private void ShowTagList(object sender, RoutedEventArgs e)
        {

            Window win = new TagListWindow();
            win.Show();
        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {

        }

        private void filePathCopy(object sender, RoutedEventArgs e)
        {

            if (System.IO.File.Exists(filePath[0]))
            {
                Clipboard.SetText(filePath[0]);

                Application.Current.Properties["TM_Msg"] = "클립보드에 경로가 복사되었습니다.";
                Window Splash_Message = new ToastMessage();
                Splash_Message.Show();
            }
            else
            {

                showDeleteMessage();
                
                DeleteFromDB(0);
            }
            

        }

        void DeleteFromDB(int i)
        {
            
            connection.Open();
            MySqlCommand cmd = new MySqlCommand("delete from content where id=" + selectID[i] + ";", connection);
            cmd.ExecuteNonQuery();
            cmd = new MySqlCommand("delete from document where id=" + selectID[i] + ";", connection);
            cmd.ExecuteNonQuery();
            connection.Close();

            Search.Text += " ";
            Search.Text = Search.Text.ToString().TrimEnd();
        }
        void showDeleteMessage()
        {
            Application.Current.Properties["TM_Msg"] = "파일 경로가 변경되어 목록을 새로고침합니다.";
            Window Splash_Message = new ToastMessage();
            Splash_Message.Show();

        }
    }

}
