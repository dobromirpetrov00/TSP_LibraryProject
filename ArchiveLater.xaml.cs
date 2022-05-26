using System;
using System.Collections.Generic;
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
    /// Interaction logic for ArchiveLater.xaml
    /// </summary>
    public partial class ArchiveLater : Window
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public ArchiveLater()
        {
            InitializeComponent();
            log4net.Config.XmlConfigurator.Configure();
        }

        MySql.Data.MySqlClient.MySqlConnection conn;
        string connectionString;

        private void GoBackButton(object sender, RoutedEventArgs e)
        {
            BookOptions f = new BookOptions();
            f.Show();
            Close();
        }

        private int bookIdGot, parsedValue;

        private void ArchiveBookButton(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrWhiteSpace(bookid.Text))
            {
                checkCorr.Text = "ENTER A BOOK ID";
                checkCorr.Background = Brushes.White;
                checkCorr.Foreground = Brushes.Red;
            }
            else if (!int.TryParse(bookid.Text, out parsedValue))
            {
                checkCorr.Text = "THE BOOK ID MUST BE ONLY NUMBERS";
                checkCorr.Background = Brushes.White;
                checkCorr.Foreground = Brushes.Red;
            }
            else
            {
                bookIdGot = int.Parse(bookid.Text);

                connectionString = "server=localhost;username=root;password=19621741;database=tsp_proekt";
                conn = new MySql.Data.MySqlClient.MySqlConnection(connectionString);

                string checkIfExQ = "SELECT COUNT(*) FROM books WHERE bookid=@bkid";
                MySqlCommand check = new MySqlCommand(checkIfExQ, conn);
                check.Parameters.AddWithValue("@bkid", bookIdGot);

                conn.Open();

                long checkEx = (long)check.ExecuteScalar();

                conn.Close();

                if (checkEx > 0)
                {
                    string rmExemplars = "UPDATE books SET isforarchive=@yes WHERE bookid=@exid";
                    MySqlCommand rmExemp = new MySqlCommand(rmExemplars, conn);
                    rmExemp.Parameters.AddWithValue("@exid", bookIdGot);
                    rmExemp.Parameters.AddWithValue("@yes", "yes");

                    conn.Open();
                    rmExemp.ExecuteNonQuery();
                    conn.Close();

                    checkCorr.Text = "BOOK SET FOR ARCHIVE LATER SUCCESSFULLY";
                    checkCorr.Background = Brushes.White;
                    checkCorr.Foreground = Brushes.Green;

                    log.Info("Book was set to archive for later successfully!");
                }
                else
                {
                    checkCorr.Text = "THE BOOK ID DOES NOT EXIST";
                    checkCorr.Background = Brushes.White;
                    checkCorr.Foreground = Brushes.Red;

                    log.Error("Tried to set a book to archive for later with non-existing ID!");
                }
            }
        }
    }
}
