using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Linq;

namespace TSP_Proekt
{
    /// <summary>
    /// Interaction logic for AdminMenu.xaml
    /// </summary>
    public partial class AdminMenu : Window
    {
        public AdminMenu()
        {
            InitializeComponent();
        }

        MySql.Data.MySqlClient.MySqlConnection conn;
        string connectionString;

        private void AdminMenu_Load(object sender, RoutedEventArgs e)
        {
            connectionString = "server=localhost;username=root;password=19621741;database=tsp_proekt";
            conn = new MySql.Data.MySqlClient.MySqlConnection(connectionString);

            string mySelect = "SELECT bookid FROM books WHERE isforarchive=?arch";

            MySqlCommand cmd = new MySqlCommand(mySelect, conn);
            cmd.Parameters.AddWithValue("?arch", "yes");
            MySqlDataAdapter sda = new MySqlDataAdapter(cmd);
            DataTable dt = new DataTable(mySelect);
            sda.Fill(dt);

            string res = string.Join(Environment.NewLine, dt.Rows.OfType<DataRow>().Select(x => string.Join(" | ", x.ItemArray)));

            MessageBox.Show("BOOK ID:\n" + res, "Books to Archive Later");
        }

        private void GoBackButton(object sender, RoutedEventArgs e)
        {
            AdminLogin f = new AdminLogin();
            f.Show();
            Close();
        }

        private void CreateOperatorButton(object sender, RoutedEventArgs e)
        {
            CreateOperator f = new CreateOperator();
            f.Show();
            Close();
        }

        private void ReferencesButton(object sender, RoutedEventArgs e)
        {
            References f = new References();
            f.Show();
            Close();
        }

        private void BookOptionsButton(object sender, RoutedEventArgs e)
        {
            BookOptions f = new BookOptions();
            f.Show();
            Close();
        }
    }
}
