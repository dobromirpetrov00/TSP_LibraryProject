using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
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
using log4net;

namespace TSP_Proekt
{
    /// <summary>
    /// Interaction logic for LendBookInfo.xaml
    /// </summary>
    public partial class LendBookInfo : Window
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public LendBookInfo()
        {
            InitializeComponent();
            log4net.Config.XmlConfigurator.Configure();
        }

        MySql.Data.MySqlClient.MySqlConnection conn;
        string connectionString;

        private void GoBackButton(object sender, RoutedEventArgs e)
        {
            ReferencesOperator f = new ReferencesOperator();
            f.Show();
            Close();
        }

        private void GetInfoButton(object sender, RoutedEventArgs e)
        {
            connectionString = "server=localhost;username=root;password=19621741;database=tsp_proekt";
            conn = new MySql.Data.MySqlClient.MySqlConnection(connectionString);

            string mySelect = "SELECT lendbookid, users_userid, books_bookid, lenddate FROM lendbooks";

            MySqlCommand cmd = new MySqlCommand(mySelect, conn);
            MySqlDataAdapter sda = new MySqlDataAdapter(cmd);
            DataTable dt = new DataTable("lendbooks");
            sda.Fill(dt);
            infoGrid.ItemsSource = dt.DefaultView;

            MessageBox.Show("SUCCESSFULLY GOT LEND BOOKS!", "SUCCESS!");

            log.Info("Got info for lended books successfully!");
        }
    }
}
