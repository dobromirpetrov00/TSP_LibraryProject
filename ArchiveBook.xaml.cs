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
    /// Interaction logic for ArchiveBook.xaml
    /// </summary>
    public partial class ArchiveBook : Window
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public ArchiveBook()
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
            else if (rbAva.IsChecked == false && rbRR.IsChecked == false)
            {
                checkCorr.Text = "CHOOSE FROM AVAILABLE OR READING ROOM";
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
                    if (rbAva.IsChecked == true)
                    {
                        string rmExemplars = "UPDATE exemplars SET states_stateid=2 WHERE books_bookid=@exid";
                        MySqlCommand rmExemp = new MySqlCommand(rmExemplars, conn);
                        rmExemp.Parameters.AddWithValue("@exid", bookIdGot);

                        string rmBooksst = "UPDATE booksstored SET available=available-1, total=total-1 WHERE bookid=@bksstid";
                        MySqlCommand rmBksst = new MySqlCommand(rmBooksst, conn);
                        rmBksst.Parameters.AddWithValue("@bksstid", bookIdGot);

                        conn.Open();
                        rmExemp.ExecuteNonQuery();
                        rmBksst.ExecuteNonQuery();
                        conn.Close();

                        checkCorr.Text = "BOOK ARCHIVED SUCCESSFULLY";
                        checkCorr.Background = Brushes.White;
                        checkCorr.Foreground = Brushes.Green;

                        log.Info("Book archived from 'available'!");
                    }
                    else
                    {
                        string rmExemplars = "UPDATE exemplars SET states_stateid=2 WHERE books_bookid=@exid";
                        MySqlCommand rmExemp = new MySqlCommand(rmExemplars, conn);
                        rmExemp.Parameters.AddWithValue("@exid", bookIdGot);

                        string rmBooksst = "UPDATE booksstored SET readingroom=readingroom-1, total=total-1 WHERE bookid=@bksstid";
                        MySqlCommand rmBksst = new MySqlCommand(rmBooksst, conn);
                        rmBksst.Parameters.AddWithValue("@bksstid", bookIdGot);

                        conn.Open();
                        rmExemp.ExecuteNonQuery();
                        rmBksst.ExecuteNonQuery();
                        conn.Close();

                        checkCorr.Text = "BOOK ARCHIVED SUCCESSFULLY";
                        checkCorr.Background = Brushes.White;
                        checkCorr.Foreground = Brushes.Green;

                        log.Info("Book archived from 'reading room'!");
                    }
                }
                else
                {
                    checkCorr.Text = "THE BOOK ID DOES NOT EXIST";
                    checkCorr.Background = Brushes.White;
                    checkCorr.Foreground = Brushes.Red;

                    log.Error("Tried to archive a book with non-existing ID!");
                }
            }
        }
    }
}
