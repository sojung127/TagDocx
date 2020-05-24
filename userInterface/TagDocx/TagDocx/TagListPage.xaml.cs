using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MySql.Data.MySqlClient;
using System.Data;

namespace TagDocx
{
    /// <summary>
    /// TagListPage.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class TagListPage : Page
    {
        public TagListPage()
        {
            InitializeComponent();
        string connectionString = "SERVER=localhost;DATABASE=adcs;UID=godocx;PASSWORD=486;";
        MySqlConnection connection;
            connection = new MySqlConnection(connectionString);
            try
            {
                connection.Open();
                string sql = "SELECT DISTINCT CONTENT_TAG FROM CONTENT;";
                MySqlCommand cmd = new MySqlCommand(sql, connection);
                MySqlDataAdapter adp = new MySqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                // 윈도우 폼의 LoadDataBinding에 데이터 넣기
                adp.Fill(ds, "TagListBinding");
                listbox1.DataContext = ds;
                Console.WriteLine("hey");

            }
            catch (MySqlException ex)
            {
                // MessageBox.Show(ex.ToString());
                Console.WriteLine("DB 연결 실패" + "(" + ex.Message + ")");
            }
            finally
            {
                connection.Close();
            }
        }
    }
}
