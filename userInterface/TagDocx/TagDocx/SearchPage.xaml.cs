﻿using System;
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

namespace TagDocx
{
    /// <summary>
    /// SearchPage.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class SearchPage : Page
    {
        string connectionString = "SERVER=localhost;DATABASE=adcs;UID=godocx;PASSWORD=486;";
        public SearchPage()
        {
            InitializeComponent();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            MySqlConnection connection = new MySqlConnection(connectionString);

            TextBox tb = sender as TextBox;
            string[] words = tb.Text.Split(' ');

            string query_start = "select document.id, name, path, c.content_tag from document left join (select id,group_concat(content_tag) as content_tag from content where id in "
                + "(select id from content where ";
            string select_content="";
            string select_docu="";

            if (words.Length != 0)
            {
                select_content += "content_tag like '%" + words[0] + "%'";
                select_docu += "union select id from document where name like '%"+words[0]+"%'";
            }
            if (words.Length >1)
            {
                for (int i = 1; i < words.Length; i++)
                {
                    if (words[i].Length != 0)
                    {
                        select_content += "or content_tag like '%" + words[i] + "%' ";
                        select_docu += "or name like '%" + words[i] + "%' ";
                    }
                }
            }
            

            string query_end=") group by id )as c on c.id = document.id where c.id = document.id;";

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
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                connection.Close();
            }
        }
        int selectID = -1;
        private void Listbox1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            foreach (DataRowView rv in e.AddedItems)
            {
                Debug.WriteLine("Row contents:", rv.Row.ItemArray[0].ToString());
                selectID = int.Parse(rv.Row.ItemArray[0].ToString());
            }
        }
        private void menuitem_click(object sender, RoutedEventArgs e)
        {
            Window win = new TagEditWindow(selectID,Search);
            win.Show();
        }
        void popup_open(object sender,MouseButtonEventArgs e, SelectionChangedEventArgs s)
        {

        }

        private void ButtonFechar_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
