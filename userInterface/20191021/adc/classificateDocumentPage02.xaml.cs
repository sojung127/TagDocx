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

            dataGridCustomers0.ItemsSource = dt.DefaultView;
            dataGridCustomers1.ItemsSource = dt.DefaultView;
            dataGridCustomers2.ItemsSource = dt.DefaultView;
            //Console.WriteLine("데이터불러옴");

            dataGridCustomers0.ItemsSource=catagoDocs("묶음0").DefaultView;
            dataGridCustomers1.ItemsSource = catagoDocs("묶음1").DefaultView;
            dataGridCustomers2.ItemsSource = catagoDocs("묶음2").DefaultView;

            묶음0태그들.Content = 묶음_태그[0];
            묶음1태그들.Content = 묶음_태그[1];
            묶음2태그들.Content = 묶음_태그[2];
        }
        public DataTable DataTableCollection { get; set; }

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

        private DataTable catagoDocs(string 묶음이름)
        {
            DataRow[] 묶음0;
            Console.WriteLine(묶음_태그[0]);
            string[] 묶음0split = 묶음_태그[0].Trim().Split();

            //쿼리에 넣을 형식으로 바꾸기
            string 쿼리형식 = "(";
            for (int i = 0; i < 묶음0split.Length; i++)
            {
                쿼리형식 += "'" + 묶음0split[i] + "'";
                if (i < 묶음0split.Length - 1) 쿼리형식 += ",";
            }
            쿼리형식 += ")";
            Console.WriteLine(쿼리형식);
            if (묶음이름 == "묶음0")
            {
                //1. 묶음_태그에 있는 태그들 모두가진 문서들을 먼저 데려가고
           

                //일단 태그 5개까지만선택가능
                묶음0 = dt.Select("CONTENT_TAG in " + 쿼리형식 + " OR TYPE_TAG in " + 쿼리형식);
                //묶음0t = dt.Select("Type_Tag in " + 쿼리형식);
                /*for(int i = 0; i < 묶음0.Length; i++)
                {
                    cd0.Rows.Add(묶음0[i]);
                }*/
                cd0 = 묶음0.CopyToDataTable();
                cd0 = cd0.DefaultView.ToTable(true, "ID", "NAME"); //중복ID제거

                return cd0;
            }
            //2. 남은 문서들은 태그중 우선순위가 높은 묶음_태그 쪽으로 감

            if (묶음이름 == "묶음1")
            {
                DataRow[] rows;
                rows = dt.Select("CONTENT_TAG in " + 쿼리형식 + " OR TYPE_TAG in " + 쿼리형식);
                foreach (DataRow r in rows)
                    r.Delete();
                //남은dt

                DataRow[] 묶음1;
                string[] 묶음1split = 묶음_태그[1].Trim().Split();

                //쿼리에 넣을 형식으로 바꾸기
                string 쿼리형식1 = "(";
                for (int i = 0; i < 묶음1split.Length; i++)
                {
                    쿼리형식1 += "'" + 묶음1split[i] + "'";
                    if (i < 묶음1split.Length - 1) 쿼리형식1 += ",";
                }
                쿼리형식1 += ")";
                Console.WriteLine(쿼리형식1);
                쿼리형식 = 쿼리형식1;
                //일단 태그 5개까지만선택가능
                묶음1 = dt.Select("CONTENT_TAG in " + 쿼리형식1 + " OR TYPE_TAG in " + 쿼리형식1);

                cd1 = 묶음1.CopyToDataTable();
                cd1 = cd1.DefaultView.ToTable(true, "ID", "NAME"); //중복ID제거
                return cd1;
            }

            if (묶음이름 == "묶음2")
            {
                DataRow[] rows;
                rows = dt.Select("CONTENT_TAG in " + 쿼리형식 + " OR TYPE_TAG in " + 쿼리형식);
                foreach (DataRow r in rows)
                    r.Delete();
                //남은dt

                DataRow[] 묶음2;
                string[] 묶음2split = 묶음_태그[2].Trim().Split();

                //쿼리에 넣을 형식으로 바꾸기
                string 쿼리형식2 = "(";
                for (int i = 0; i < 묶음2split.Length; i++)
                {
                    쿼리형식2 += "'" + 묶음2split[i] + "'";
                    if (i < 묶음2split.Length - 1) 쿼리형식2 += ",";
                }
                쿼리형식2 += ")";
                Console.WriteLine(쿼리형식2);

                //일단 태그 5개까지만선택가능
                묶음2 = dt.Select("CONTENT_TAG in " + 쿼리형식2 + " OR TYPE_TAG in " + 쿼리형식2);

                cd2 = 묶음2.CopyToDataTable();
                cd2 = cd2.DefaultView.ToTable(true, "ID", "NAME"); //중복ID제거
                return cd2;
            }
            else return null;
        }
    }
}


//형식에대하여...-> 맹락이 두개임
//1. 기사인데 학교, 교복에대한 기사
//2. 기사면다ok(ex. 기사끼리, 논문끼리, 형식끼리끼리) =>현재코드 (사용자가 어떤의도로 할지 몰라서 형식태그도 그냥 일반태그로취급해서 우선 묶음이 가져감.