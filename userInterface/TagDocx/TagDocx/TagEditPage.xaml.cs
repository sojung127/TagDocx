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

using System.Data;
using MySql.Data.MySqlClient;
using System.Diagnostics;

namespace TagDocx
{
    /// <summary>
    /// TagEditPage.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class TagEditPage : Page
    {
        int id;
        string connectionString = "SERVER=localhost;DATABASE=adcs;UID=godocx;PASSWORD=486;";
        string[] tagList = new string[10];
        string[] changedTag = new string[10];
        TextBox search;
        public TagEditPage(int id,TextBox text)
        {
            InitializeComponent();
            this.id = id;
            search = text;
            for (int i = 0; i < 10; i++)
                tagList[i] = "";
            MySqlConnection connection = new MySqlConnection(connectionString);

            string query = "select content_tag from content where id=" + id+";";

                    int len = 0;
            try
            {
                connection.Open();
                MySqlCommand cmd = new MySqlCommand(query, connection);
                MySqlDataAdapter adp = new MySqlDataAdapter(cmd);

                DataSet ds = new DataSet();
                adp.Fill(ds);

                if (ds.Tables.Count > 0)
                {
                    foreach (DataRow r in ds.Tables[0].Rows)
                    {
                        tagList[len++] = r["content_tag"].ToString();
                    }
                }
                query = "select NAME from document where id=" + id + ";";
                cmd = new MySqlCommand(query, connection);
                adp = new MySqlDataAdapter(cmd);

                ds = new DataSet();
                adp.Fill(ds);

                if (ds.Tables.Count > 0)
                {
                    foreach (DataRow r in ds.Tables[0].Rows)
                    {
                        FileName.Text = r["NAME"].ToString();
                    }
                }

            }
            catch(Exception e)
            {
                Debug.WriteLine(e.StackTrace);
            }
            finally
            {
                connection.Close();
            }
            for(int k = 0; k < 10; k++)
            {
                changedTag[k] = tagList[k];
            }
            // 박스들을 그룹으로 묶는법을 못 찾아서 일일이 수정함
            TextBox[] tb=new TextBox[10];
            tb[0] = tb1;
            tb[1] = tb2;
            tb[2] = tb3;
            tb[3] = tb4;
            tb[4] = tb5;
            tb[5] = tb6;
            tb[6] = tb7;
            tb[7] = tb8;
            tb[8] = tb9;
            tb[9] = tb10;
            for(int i = 0; i < len; i++)
            {
                tb[i].Text = tagList[i];
            }
        }
        private void TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox tb = sender as TextBox;
            if (tb.Name.Equals("Search"))
            {
               
                return;
            }
            string name = tb.Name;
            int index;
            if (name.Length > 3)
            {
                index = 9;
            }
            else
            {
                index = int.Parse(name.Substring(2,1))-1;
            }

            changedTag[index] = tb.Text;

            string changedText = tb.Text;
            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MySqlConnection connection = new MySqlConnection(connectionString);

            string[] query = new string[10];
            for(int i = 0; i < 10; i++)
            {
                query[i] = "";
                if (changedTag[i].Length == 0 && tagList[i].Length!=0)
                {
                    query[i] = "delete from content where id=" + id + " and content_tag='" + tagList[i] + "';";
                }
                else if(tagList[i].Length==0 && changedTag[i].Length != 0)
                {
                    query[i] = "insert into content values(" + id + ",'" + changedTag[i] + "');";
                }else
                {
                    query[i] = "update content set content_tag='" + changedTag[i] + "' where id=" + id + " and content_tag='" + tagList[i] + "';";
                }
            }

            try
            {
                connection.Open();
                MySqlCommand cmd;
                for (int i = 0; i < 10; i++)
                {
                    if (query[i].Length != 0)
                    {
                        cmd = new MySqlCommand(query[i], connection);
                        Debug.WriteLine(cmd.ExecuteNonQuery());
                    }
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.StackTrace);
            }
            finally
            {
                connection.Close();
            }
            //TextBox search = search;
            try
            {
           
            string origing = search.Text;
            search.Text = "";
            search.Text = origing;

            }
            catch(Exception xe)
            {
                Debug.WriteLine(xe.ToString());
            }
            Window.GetWindow(this).Close();
        }
    }
}
