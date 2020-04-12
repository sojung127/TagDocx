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
        List<string> 묶음_태그 = new List<string>();


        DataTable dt = new DataTable();

        DataTable cd0 = new DataTable();  //묶음0
        DataTable cd1 = new DataTable();  //묶음1 
        DataTable cd2 = new DataTable();  //묶음2
        
        
        public classificateDocumentPage02( DataTable dt, List<string> 묶음_태그)
        {
            InitializeComponent();
           
            this.dt = dt; //문서 datatable 가져오기
            this.묶음_태그 = 묶음_태그;

            dataGridCustomers0.ItemsSource = null; // dt.DefaultView;
            dataGridCustomers1.ItemsSource = null;// dt.DefaultView;
            dataGridCustomers2.ItemsSource = null;// dt.DefaultView;
            //Console.WriteLine("데이터불러옴");

            if (묶음_태그[0] != null)
            {
                try
                {
                    dataGridCustomers0.ItemsSource = catagoDocs("묶음0").DefaultView;
                }
                catch (System.NullReferenceException e) { }
            }
            if (묶음_태그[1] != null) {
                try { dataGridCustomers1.ItemsSource = catagoDocs("묶음1").DefaultView; }
                catch(System.NullReferenceException e) { }
                }
            if (묶음_태그[2] != null)
            {
                try { dataGridCustomers2.ItemsSource = catagoDocs("묶음2").DefaultView; }
                catch (System.NullReferenceException e) { }
            }

            //위에표시라벨
            묶음0태그들.Content = 묶음_태그[0];
            묶음1태그들.Content = 묶음_태그[1];
            묶음2태그들.Content = 묶음_태그[2];
        }
        public string filterTypeTag(string[] 묶음){
            string 태그모음스트링 = "(";
            foreach (string tag in 묶음)
            {
                if (tag == "공고")
                {
                    태그모음스트링 += "'공고'";
                    태그모음스트링 += ",";
                }
                else if (tag == "논문")
                {
                    태그모음스트링 += "'논문'";
                    태그모음스트링 += ",";
                }
                else if (tag == "기사")
                {
                    태그모음스트링 += "'기사'";
                    태그모음스트링 += ",";
                }
                else if (tag == "지원서")
                {
                    태그모음스트링 += "'지원서'";
                    태그모음스트링 += ",";
                }
                else if (tag == "기타")
                {
                    태그모음스트링 += "'기타'";
                    태그모음스트링 += ",";
                }
            }
            태그모음스트링=태그모음스트링.Substring(0,태그모음스트링.Length-1)+ ")";
            if (태그모음스트링 == ")") return null;
            //('태그' ~ 형태로 형식태그 선택한거 모음 출력한후 tag in 으로 select함)
            Console.WriteLine(태그모음스트링);
            return 태그모음스트링;
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

        public string filterContentTag(string[] 묶음)
        { 
            string 쿼리형식 = "(";
            for (int i = 0; i < 묶음.Length; i++)
            {
                if (묶음[i] != "공고" && 묶음[i] != "논문" && 묶음[i] != "기사" && 묶음[i] != "지원서" && 묶음[i] != "기타") { 
                    쿼리형식 += "'" + 묶음[i] + "'";
                    if (i < 묶음.Length - 1) 쿼리형식 += ",";
                }
            }
            쿼리형식 += ")";
            Console.WriteLine(쿼리형식);
            return 쿼리형식;
        }
            
        private DataTable catagoDocs(string 묶음이름)
        {
            if (묶음이름 == "묶음0")
            {
                DataRow[] 묶음0=null;
                DataTable 필터링후0=null;
                Console.WriteLine(묶음_태그[0]);
                string[] 묶음0split = 묶음_태그[0].Trim().Split();

                //typetag로 먼저 필터링하기
                string 태그쿼리형식0=filterTypeTag(묶음0split);
                if (태그쿼리형식0 != "()") //형식태그가 있는 경우
                {
                    묶음0 = dt.Select("TYPE_TAG in " + 태그쿼리형식0);
                    필터링후0 = 묶음0.CopyToDataTable(); //"필터링후"에서 내용태그로 필터링하면됨
                    묶음0 = null;
                }
                else 필터링후0 = dt;


                //쿼리에 넣을 형식으로 바꾸기
               /* string 쿼리형식 = "(";
                for (int i = 0; i < 묶음0split.Length; i++)
                {
                    쿼리형식 += "'" + 묶음0split[i] + "'";
                    if (i < 묶음0split.Length - 1) 쿼리형식 += ",";
                }
                쿼리형식 += ")";*/

                string 쿼리형식0 = filterContentTag(묶음0split);
                //일단 태그 5개까지만선택가능
                //내용태그도있는경우
                if(쿼리형식0!="()") 묶음0 = 필터링후0.Select("CONTENT_TAG in " + 쿼리형식0);
                //묶음0t = dt.Select("Type_Tag in " + 쿼리형식);
                /*for(int i = 0; i < 묶음0.Length; i++)
                {
                    cd0.Rows.Add(묶음0[i]);
                }*/
                if (묶음0 == null)  //내용태그 결과가 없는경우 기존 뽑아놨던거 그대로 출력
                {
                    return 필터링후0;
                }
                else //아니면 내용태그까지 한 결과를 출력
                {
                    try
                    {
                        cd0 = 묶음0.CopyToDataTable();
                        cd0 = cd0.DefaultView.ToTable(true, "ID", "NAME"); //중복ID제거
                        return cd0;
                    }
                    catch (System.InvalidOperationException e)
                    {
                        return null;
                    }
                }
            }
            //2. 남은 문서들은 태그중 우선순위가 높은 묶음_태그 쪽으로 감

            if (묶음이름 == "묶음1")
            {
                DataRow[] rows;
                DataTable 필터링후1 = null;
                /*rows = dt.Select("CONTENT_TAG in " + 쿼리형식 + " OR TYPE_TAG in " + 쿼리형식);
                foreach (DataRow r in rows)
                    r.Delete();
                    */ //중복허락
                //남은dt

                DataRow[] 묶음1=null;
                string[] 묶음1split = 묶음_태그[1].Trim().Split();

                //typetag로 먼저 필터링하기
                string 태그쿼리형식1 = filterTypeTag(묶음1split);
                //형식태그가있는경우
                if (태그쿼리형식1 != null) { 
                    묶음1 = dt.Select("TYPE_TAG in " + 태그쿼리형식1);
                    필터링후1 = 묶음1.CopyToDataTable(); //"필터링후"에서 내용태그로 필터링하면됨
                    묶음1 = null;
                 }
                else 필터링후1 = dt;


                //쿼리에 넣을 형식으로 바꾸기
                /*string 쿼리형식1 = "(";
                for (int i = 0; i < 묶음1split.Length; i++)
                {
                    쿼리형식1 += "'" + 묶음1split[i] + "'";
                    if (i < 묶음1split.Length - 1) 쿼리형식1 += ",";
                }
                쿼리형식1 += ")";
                Console.WriteLine(쿼리형식1);*/

                string 쿼리형식1 = filterContentTag(묶음1split);
                //일단 태그 5개까지만선택가능
                //내용태그가 있는경우
                if (쿼리형식1 != "()") //내용태그가있을때
                {
                    묶음1 = 필터링후1.Select("CONTENT_TAG in " + 쿼리형식1);
                    cd1 = 묶음1.CopyToDataTable();
                    cd1 = cd1.DefaultView.ToTable(true, "ID", "NAME"); //중복ID제거
                    return cd1;
                }
                if (묶음1 == null)  //내용태그 결과가 없는경우 기존 뽑아놨던거 그대로 출력
                {
                    return 필터링후1;
                }
                else //아니면 내용태그까지 한 결과를 출력
                {
                    try
                    {
                        cd1 = 묶음1.CopyToDataTable();
                        cd1 = cd1.DefaultView.ToTable(true, "ID", "NAME"); //중복ID제거
                        return cd1;
                    }
                    catch (System.InvalidOperationException e)
                    {
                        return null;
                    }
                }
            }

            if (묶음이름 == "묶음2")
            {
                DataRow[] rows;
                DataTable 필터링후2=null;
                /*rows = dt.Select("CONTENT_TAG in " + 쿼리형식 + " OR TYPE_TAG in " + 쿼리형식);
                foreach (DataRow r in rows)
                    r.Delete();
                 */ // 중복허락
                //남은dt

                DataRow[] 묶음2=null;
                string[] 묶음2split = 묶음_태그[2].Trim().Split();

                //typetag로 먼저 필터링하기
                string 태그쿼리형식2 = filterTypeTag(묶음2split);
                if (태그쿼리형식2 != null) //형식태그있는경우
                {
                    묶음2 = 필터링후2.Select("TYPE_TAG in " + 태그쿼리형식2);
                    필터링후2 = 묶음2.CopyToDataTable(); //"필터링후"에서 내용태그로 필터링하면됨
                    묶음2 = null;
                }
                else 필터링후2 = dt;
                //쿼리에 넣을 형식으로 바꾸기
               /* string 쿼리형식2 = "(";
                for (int i = 0; i < 묶음2split.Length; i++)
                {
                    쿼리형식2 += "'" + 묶음2split[i] + "'";
                    if (i < 묶음2split.Length - 1) 쿼리형식2 += ",";
                }
                쿼리형식2 += ")";
                Console.WriteLine(쿼리형식2);*/

                string 쿼리형식2 = filterContentTag(묶음2split);
                //일단 태그 5개까지만선택가능
                if (쿼리형식2 != "()") //내용태그도있을때
                {
                    묶음2 = 필터링후2.Select("CONTENT_TAG in " + 쿼리형식2);
                    cd2 = 묶음2.CopyToDataTable();
                    cd2 = cd2.DefaultView.ToTable(true, "ID", "NAME"); //중복ID제거
                    return cd2;
                }

                else //내용태그결과가 없을때 
                {
                    if (묶음2 == null)  //내용태그 결과가 없는경우 기존 뽑아놨던거 그대로 출력
                    {
                        return 필터링후2;
                    }
                    else //아니면 내용태그까지 한 결과를 출력
                    {
                        try
                        {
                            cd2 = 묶음2.CopyToDataTable();
                            cd2 = cd2.DefaultView.ToTable(true, "ID", "NAME"); //중복ID제거
                            return cd2;
                        }
                        catch (System.InvalidOperationException e)
                        {
                            return null;
                        }
                    }
                }

                
            }
            else return null;
        }
    }
}


//일단 묶음별 중복허락하고 함