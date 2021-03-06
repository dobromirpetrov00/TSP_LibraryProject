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
using log4net;

namespace TSP_Proekt
{
    /// <summary>
    /// Interaction logic for SubmittedForms.xaml
    /// </summary>
    public partial class SubmittedForms : Window
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public SubmittedForms()
        {
            InitializeComponent();
            log4net.Config.XmlConfigurator.Configure();
        }

        MySql.Data.MySqlClient.MySqlConnection conn;
        string connectionString;

        private void GoBackButton(object sender, RoutedEventArgs e)
        {
            References f = new References();
            f.Show();
            Close();
        }

        private void GetInfoButton(object sender, RoutedEventArgs e)
        {
            connectionString = "server=localhost;username=root;password=19621741;database=tsp_proekt";
            conn = new MySql.Data.MySqlClient.MySqlConnection(connectionString);

            string mySelect = "SELECT formid, creationdate, users_userid FROM forms";

            MySqlCommand cmd = new MySqlCommand(mySelect, conn);
            MySqlDataAdapter sda = new MySqlDataAdapter(cmd);
            DataTable dt = new DataTable("forms");
            sda.Fill(dt);
            infoGrid.ItemsSource = dt.DefaultView;

            MessageBox.Show("SUCCESSFULLY GOT SUBMITTED FORMS!", "SUCCESS!");

            log.Info("Got info for submitted forms successfully!");
        }
    }
}
