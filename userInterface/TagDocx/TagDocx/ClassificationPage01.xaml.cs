using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.WindowsAPICodePack.Dialogs;
using System.Data;
using System.Collections.Generic; //List collection 써야하니까!
using System.Linq; //리스트 중복제거 함수 쓰려고 추가
using System;

namespace TagDocx
{
    /// <summary>
    /// setTagPage.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class ClassificationPage01 : Page
    {

        DataSet ds = new DataSet();
        DataTable dt = new DataTable(); //가지고놀dbtable ds[0]이 dt임




        List<string> Tags = new List<string>();
        List<string> 묶음_태그 = new List<string>(); //묶음번호가index고 그 내용이 선택된 태그
        string[,] stags = new string[3,5];   //


        public ClassificationPage01(DataSet data)
        {
            InitializeComponent();
            this.ds = data;
            dt = ds.Tables[0]; //datatable로 바꿈
            getTypeTag();
        }

        public void getTypeTag()
        {
            DataRow[] rows = dt.Select();
            for (int i = 0; i < rows.Length; i++)
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
            for (int i = 0; i < Tags.Count; i++)
            {
                태그목록리스트.Items.Add(Tags[i]);
                태그목록리스트.SelectionMode = SelectionMode.Multiple; //복수선택
            }
        }

        private void BtnPreStep(object sender, RoutedEventArgs e)
        {
            ClassificationPage page = new ClassificationPage();
            NavigationService.Navigate(page);
        }

        private void BtnNextStep(object sender, RoutedEventArgs e)
        {

            // 넘기기
            ClassificationPage02 page = new ClassificationPage02(dt, 묶음_태그);

            Console.WriteLine("0묶음");
            Console.WriteLine(묶음_태그[0]);
            Console.WriteLine("1묶음");
            Console.WriteLine(묶음_태그[1]);
            Console.WriteLine("2묶음");
            Console.WriteLine(묶음_태그[2]);
            NavigationService.Navigate(page);
        }


        private void 묶음에추가_Click(object sender, RoutedEventArgs e)
        {

            int 묶음 = 0;
            묶음 = 묶음박스.SelectedIndex; //tabpage는 0 부터 시작
            string[] tmplen = 묶음_태그[묶음].Split();

            if (tmplen.Length == 5 || 태그목록리스트.SelectedItems.Count > 5)
            {
                MessageBox.Show("태그는 5개까지 선택가능 합니다.", "태그제한", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            if (묶음 > 묶음_태그.Count - 1 && 태그목록리스트.SelectedItem != null) //처음 태그 추가
            {

                //묶음_태그.Add(this.태그목록리스트.SelectedItems.ToString());
                //묶음_태그.Add(""); //빈 스트링 추가
                for (int i = 0; i < 태그목록리스트.SelectedItems.Count; i++)
                {
                    묶음_태그[묶음] = 묶음_태그[묶음] + ' ' + 태그목록리스트.SelectedItems[i].ToString();
                }
            }
            else if (묶음 <= 묶음_태그.Count - 1 && 태그목록리스트.SelectedItem != null) //있는데다 추가
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
            //임시태그목록.Content= 묶음_태그[묶음];

            묶음_태그[묶음] = 묶음_태그[묶음].Trim();

            if (묶음 == 0) showSelectedTags(묶음태그리스트0);
            if (묶음 == 1) showSelectedTags(묶음태그리스트1);
            if (묶음 == 2) showSelectedTags(묶음태그리스트2);
        }
        private void 묶음비우기_Click(object sender, RoutedEventArgs e)
        {
            int tabindex = 묶음박스.SelectedIndex; //어느 묶음 선택했는지
            if (tabindex == 0) { 묶음_태그[0] = ""; showSelectedTags(묶음태그리스트0); }
            if (tabindex == 1) { 묶음_태그[1] = ""; showSelectedTags(묶음태그리스트1); }
            if (tabindex == 2) { 묶음_태그[2] = ""; showSelectedTags(묶음태그리스트2); }
        }
        private void showSelectedTags(ListBox 묶음태그리스트)
        {
            int tabindex = 0;
            묶음태그리스트.Items.Clear();//reset
            태그목록리스트.SelectedIndex = -1;
            //임시태그목록.Content = 묶음박스.SelectedIndex;
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

        private void click_search(object sender, RoutedEventArgs e)
        {
            string 검색어 = 태그검색어.Text;
            foreach (string searchtag in 태그목록리스트.Items)
            {
                if (searchtag == 검색어)
                {
                    태그목록리스트.SelectedItems.Add(searchtag); //선택되게표시하고
                    int curIndex = 태그목록리스트.Items.IndexOf(searchtag);
                    태그목록리스트.ScrollIntoView(태그목록리스트.Items[curIndex]);
                    return; //찾아서 나가기
                }
            }
            //못찾아서 못나가면
            MessageBox.Show("찾는 태그가 없습니다.", "없는 태그", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void TextBox_GotMouseCapture(object sender, System.Windows.Input.MouseEventArgs e)
        {
            태그검색어.SelectAll();
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
