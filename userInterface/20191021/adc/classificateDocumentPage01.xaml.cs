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
    public partial class classificateDocumentPage01 : Page
    {
        string folderName = null;
        DataSet ds = new DataSet();  
        DataTable dt = new DataTable(); //가지고놀dbtable ds[0]이 dt임




        List<string> Tags = new List<string>();
        List<string> 묶음_태그 = new List<string>(); //묶음번호가index고 그 내용이 선택된 태그

        

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
            for(int i = 0; i < rows.Length; i++)
            {
                Tags.Add(rows[i]["TYPE_TAG"].ToString()); 
                Tags.Add(rows[i]["CONTENT_TAG"].ToString());
                Tags = Tags.Distinct().ToList(); //중복제거
            }


            setList(); //리스트 박스로 출력하기
            묶음_태그.Add("");
            묶음_태그.Add("");
            묶음_태그.Add("");
        }

        public void setList() //listbox에 item 추가
        {
            for (int i=0; i < Tags.Count; i++)
            {
                태그목록리스트.Items.Add(Tags[i]);
                태그목록리스트.SelectionMode = SelectionMode.Multiple; //복수선택
            }
        }

        private void BtnPreStep(object sender, RoutedEventArgs e)
        {
            classificateDocumentPage00 page = new classificateDocumentPage00();
            NavigationService.Navigate(page);
        }

        private void BtnNextStep(object sender, RoutedEventArgs e)
        {
            
            // 넘기기
            classificateDocumentPage02 page = new classificateDocumentPage02(dt,묶음_태그);


            NavigationService.Navigate(page);
        }


        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void 묶음에추가_Click(object sender, RoutedEventArgs e)
        {
            //넣은거 또 넣는거 처리 안됨, 그 태그리스트에 선택되었던거 선택된 표시로 해놓는거 처리안됨..

            int 묶음 =0;
            묶음 = 묶음박스.SelectedIndex; //tabpage는 0 부터 시작
            string[] tmplen = 묶음_태그[묶음].Split();

            if (tmplen.Length == 5 || 태그목록리스트.SelectedItems.Count>5)
            {
                MessageBox.Show("태그는 5개까지 선택가능 합니다.", "태그제한", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            if (묶음>묶음_태그.Count- 1&& 태그목록리스트.SelectedItem != null) //처음 태그 추가
            {
       
                //묶음_태그.Add(this.태그목록리스트.SelectedItems.ToString());
                //묶음_태그.Add(""); //빈 스트링 추가
                for(int i = 0; i < 태그목록리스트.SelectedItems.Count; i++)
                {
                    묶음_태그[묶음] = 묶음_태그[묶음] + ' ' + 태그목록리스트.SelectedItems[i].ToString();
                }
            }
            else if(묶음<=묶음_태그.Count-1 && 태그목록리스트.SelectedItem != null) //있는데다 추가
            {
                for (int i = 0; i < 태그목록리스트.SelectedItems.Count; i++)
                {
                    묶음_태그[묶음] = 묶음_태그[묶음] + ' ' + 태그목록리스트.SelectedItems[i].ToString();
                }
            }
            else//암것도아님
            {
                묶음_태그[묶음] = "";
            }
            임시태그목록.Content= 묶음_태그[묶음];

            묶음_태그[묶음]=묶음_태그[묶음].Trim();

            if (묶음 == 0) showSelectedTags(묶음태그리스트0);
            if (묶음 == 1) showSelectedTags(묶음태그리스트1);
            if (묶음 == 2) showSelectedTags(묶음태그리스트2);
        }
        private void 묶음에서삭제_Click(object sender, RoutedEventArgs e)
        {

        }
        private void showSelectedTags(ListBox 묶음태그리스트)
        {
            int tabindex = 0;
            묶음태그리스트.Items.Clear();//reset
            태그목록리스트.SelectedIndex = -1;
            임시태그목록.Content = 묶음박스.SelectedIndex;
            tabindex = 묶음박스.SelectedIndex;

            if (tabindex <= 묶음_태그.Count - 1 && 묶음_태그[tabindex] != null)
            {
                string[] sts = 묶음_태그[tabindex].Split(' ');
                for (int i = 0; i < sts.Length; i++)
                {
                    묶음태그리스트.Items.Add(sts[i]);
                }
            }
        }
        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int tabindex = 묶음박스.SelectedIndex;
            if (tabindex == 0) showSelectedTags(묶음태그리스트0);
            if (tabindex == 1) showSelectedTags(묶음태그리스트1);
            if (tabindex == 2) showSelectedTags(묶음태그리스트2);

            /*int tabindex = 0;
            태그목록리스트.SelectedIndex = -1;
            임시태그목록.Content = 묶음박스.SelectedIndex;
            tabindex = 묶음박스.SelectedIndex;
            if(tabindex<=묶음_태그.Count-1 && 묶음_태그[tabindex] != null) 
            {
                string[] sts = 묶음_태그[tabindex].Split(' ');
                for(int i=0; i<sts.Length; i++)
                {
                    int tmpindex=태그목록리스트.Items.IndexOf(sts[i]); //해당태그의 인덱스가져오기
                    태그목록리스트.
                    태그목록리스트.SelectedItem.Add(sts[i]);
                }
            }*/

        }

        

        /*private void 묶음추가클릭(object sender, RoutedEventArgs e) => 다음 버전에서 구현
        {
            TabItem newTabItem = new TabItem
            {
                Header = "Test",
                Name = "Test"
            };
           묶음박스.Items.Add(newTabItem);
            ListBox newListBox = new ListBox
            {

            };
            묶음박스.Items[(묶음박스.Items.Count) - 1].Add(newListBox);


        }*/
        
       
    }
}
