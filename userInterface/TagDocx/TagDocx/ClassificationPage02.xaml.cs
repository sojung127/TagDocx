using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.WindowsAPICodePack.Dialogs;
using System.Data;
using System.Collections.Generic; //List collection 써야하니까!
using System.Linq; //리스트 중복제거 함수 쓰려고 추가
using System;
using Microsoft.VisualBasic.Logging;
using System.IO.Packaging;

namespace TagDocx
{
    /// <summary>
    /// setTagPage.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class ClassificationPage02 : Page
    {
        string folderName = null;
        List<string> G0 = new List<string>();
        List<string> G1 = new List<string>();
        List<string> G2 = new List<string>();


        DataTable dt = new DataTable();

        DataTable cd0 = new DataTable();  //묶음0
        DataTable cd1 = new DataTable();  //묶음1 
        DataTable cd2 = new DataTable();  //묶음2


        public ClassificationPage02(DataTable dt, List<string> G0, List<string> G1, List<string> G2 )
        {
            InitializeComponent();

            this.dt = dt; //문서 datatable 가져오기
            this.G0 = G0;
            this.G1 = G1;
            this.G2 = G2;

            dataGridCustomers0.ItemsSource = null; // dt.DefaultView;
            dataGridCustomers1.ItemsSource = null;// dt.DefaultView;
            dataGridCustomers2.ItemsSource = null;// dt.DefaultView;
            //Console.WriteLine("데이터불러옴");
          

            if (G0.Count() != 0)
            {
                try
                {
                    dataGridCustomers0.ItemsSource = catagoDocs("묶음0").DefaultView;
                }
                catch (System.NullReferenceException e) { }
            }
            if (G1.Count() != 0)
            {
                try { dataGridCustomers1.ItemsSource = catagoDocs("묶음1").DefaultView; }
                catch (System.NullReferenceException e) { }
            }
            if (G2.Count() != 0)
            {
                try { dataGridCustomers2.ItemsSource = catagoDocs("묶음2").DefaultView; }
                catch (System.NullReferenceException e) { }
            }

            //위에표시라벨
            묶음0태그들.Content = ltos(G0);
            묶음1태그들.Content = ltos(G1);
            묶음2태그들.Content = ltos(G2);
        }



        private string ltos(List<string> li)
        {
            string temp=null;
            for (int i=0; i < li.Count(); i++)
            {
                    temp += li[i];
            }
            return temp;
        }


        public string filterTypeTag(List<string> 묶음)
        {
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
            태그모음스트링 = 태그모음스트링.Substring(0, 태그모음스트링.Length - 1) + ")";
            if (태그모음스트링 == ")") return null;
            //('태그' ~ 형태로 형식태그 선택한거 모음 출력한후 tag in 으로 select함)
            Console.WriteLine(태그모음스트링);
            return 태그모음스트링;
        }

        private void BtnEnd(object sender, RoutedEventArgs e)
        {
            ClassificationPage page = new ClassificationPage(); //일단 메인으로
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

        public string filterContentTag(List<string> 묶음)
        {
            string 쿼리형식 = "(";
            for (int i = 0; i < 묶음.Count(); i++)
            {
                if (묶음[i] != "공고" && 묶음[i] != "논문" && 묶음[i] != "기사" && 묶음[i] != "지원서" && 묶음[i] != "기타")
                {
                    쿼리형식 += "'" + 묶음[i] + "'";
                    if (i < 묶음.Count() - 1) 쿼리형식 += ",";
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
                DataRow[] 묶음0 = null;
                DataTable 필터링후0 = null;
                Console.WriteLine(G0);
                

                //typetag로 먼저 필터링하기
                string 태그쿼리형식0 = filterTypeTag(G0);
                if (태그쿼리형식0 != "()" && 태그쿼리형식0 != null) //형식태그가 있는 경우
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

                string 쿼리형식0 = filterContentTag(G0);
                //일단 태그 5개까지만선택가능
                //내용태그도있는경우
                if (쿼리형식0 != "()") 묶음0 = 필터링후0.Select("CONTENT_TAG in " + 쿼리형식0);
                //묶음0t = dt.Select("Type_Tag in " + 쿼리형식);
                /*for(int i = 0; i < 묶음0.Length; i++)
                {
                    cd0.Rows.Add(묶음0[i]);
                }*/
                if (묶음0 == null)  //내용태그 결과가 없는경우 기존 뽑아놨던거 그대로 출력
                {
                    getdocpath(cd0, ltos(G0));//문
                    return 필터링후0;
                }
                else //아니면 내용태그까지 한 결과를 출력
                {
                    try
                    {
                        cd0 = 묶음0.CopyToDataTable();
                        cd0 = cd0.DefaultView.ToTable(true, "ID", "NAME","TYPE_TAG","CONTENT_TAG","PATH"); //중복ID제거
                        getdocpath(cd0, ltos(G0));//문
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

                DataRow[] 묶음1 = null;

                //typetag로 먼저 필터링하기
                string 태그쿼리형식1 = filterTypeTag(G1);
                //형식태그가있는경우
                if (태그쿼리형식1 != null && 태그쿼리형식1 != "()")
                {
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

                string 쿼리형식1 = filterContentTag(G1);
                //일단 태그 5개까지만선택가능
                //내용태그가 있는경우
                if (쿼리형식1 != "()") //내용태그가있을때
                {
                    묶음1 = 필터링후1.Select("CONTENT_TAG in " + 쿼리형식1);
                    cd1 = 묶음1.CopyToDataTable();
                    cd1 = cd1.DefaultView.ToTable(true, "ID", "NAME", "TYPE_TAG", "CONTENT_TAG", "PATH"); //중복ID제거
                    return cd1;
                }
                if (묶음1 == null)  //내용태그 결과가 없는경우 기존 뽑아놨던거 그대로 출력
                {
                    getdocpath(cd1, ltos(G1));//문서옮기기
                    return 필터링후1;
                }
                else //아니면 내용태그까지 한 결과를 출력
                {
                    try
                    {
                        cd1 = 묶음1.CopyToDataTable();
                        cd1 = cd1.DefaultView.ToTable(true, "ID", "NAME", "TYPE_TAG", "CONTENT_TAG", "PATH"); //중복ID제거
                        getdocpath(cd1, ltos(G1));//문서옮기기
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
                DataTable 필터링후2 = null;
                /*rows = dt.Select("CONTENT_TAG in " + 쿼리형식 + " OR TYPE_TAG in " + 쿼리형식);
                foreach (DataRow r in rows)
                    r.Delete();
                 */ // 중복허락
                //남은dt

                DataRow[] 묶음2 = null;


                //typetag로 먼저 필터링하기
                string 태그쿼리형식2 = filterTypeTag(G2);
                if (태그쿼리형식2 != null && 태그쿼리형식2 != "()") //형식태그있는경우
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

                string 쿼리형식2 = filterContentTag(G2);
                //일단 태그 5개까지만선택가능
                if (쿼리형식2 != "()") //내용태그도있을때
                {
                    묶음2 = 필터링후2.Select("CONTENT_TAG in " + 쿼리형식2);
                    cd2 = 묶음2.CopyToDataTable();
                    cd2 = cd2.DefaultView.ToTable(true, "ID", "NAME", "TYPE_TAG", "CONTENT_TAG", "PATH"); //중복ID제거
                    return cd2;
                }

                else //내용태그결과가 없을때 
                {
                    if (묶음2 == null)  //내용태그 결과가 없는경우 기존 뽑아놨던거 그대로 출력
                    {
                        getdocpath(cd2, ltos(G2));//문서옮기기
                        return 필터링후2;
                    }
                    else //아니면 내용태그까지 한 결과를 출력
                    {
                        try
                        {
                            cd2 = 묶음2.CopyToDataTable();
                            cd2 = cd2.DefaultView.ToTable(true, "ID", "NAME", "TYPE_TAG", "CONTENT_TAG", "PATH"); //중복ID제거
                            getdocpath(cd2, ltos(G2));//문서옮기기
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
        private DataGrid findtabindex()
        {
            int tabindex = 묶음결과박스.SelectedIndex;
            if (tabindex == 0) return dataGridCustomers0;
            if (tabindex == 1) return dataGridCustomers1;
            else return dataGridCustomers2;
        }
        private DataTable findtabdata()
        {
            int tabindex = 묶음결과박스.SelectedIndex;
            if (tabindex == 0) return cd0;
            if (tabindex == 1) return cd1;
            else return cd2;
        }
        private void ssdelete(object sender, RoutedEventArgs e)
        {
            //선택한폴더삭제하기
            DataGrid cdatagrid = findtabindex();
            cdatagrid.Items.Remove(cdatagrid.SelectedItem);
        }

        

        //파일 문서 경로 바꾸기
        private static void changePath(string filePath, string fileName, string nfn)
        {
            //filePath는 지금경로
            Console.WriteLine(filePath);
            Console.WriteLine(fileName);
            //fileName(확장자포함)
            string deskPath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);

            string oldFile = filePath + "/" + fileName;   //파일이름포함한 기존경로  (NAME은 파일 타입 포함)
            string newFile = deskPath + "/ADCSforder" + "/" +nfn+"/"+ fileName;    //새로운경로+파일이름 경로

            Console.WriteLine(oldFile + "->" + newFile);

            System.IO.File.Move(oldFile, newFile);
        }

        private static void getdocpath(DataTable db,string newfilename)
        {
            string name=null;
            string path = null;
            for (int i = 0; i < db.Rows.Count; i++)
            {
                name = db.Rows[i][1].ToString();
                path = db.Rows[i][4].ToString();
                changePath(path, name, newfilename);
            }
            Console.WriteLine(name+"->"+path);
        }
    }
}
