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
    /// Interaction logic for GiveBook.xaml
    /// </summary>
    public partial class GiveBook : Window
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public GiveBook()
        {
            InitializeComponent();
            log4net.Config.XmlConfigurator.Configure();
        }

        MySql.Data.MySqlClient.MySqlConnection conn;
        string connectionString;

        private void GoBackButton(object sender, RoutedEventArgs e)
        {
            OperatorMenu f = new OperatorMenu();
            f.Show();
            Close();
        }

        private int userIdGot, bookIdGot, parsedValue;
        private String dateGot;

        private void GiveBookButton(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrWhiteSpace(userid.Text))
            {
                checkCorr.Text = "ENTER A USER ID";
                checkCorr.Background = Brushes.White;
                checkCorr.Foreground = Brushes.Red;
            }
            else if (!int.TryParse(userid.Text, out parsedValue))
            {
                checkCorr.Text = "THE USER ID MUST BE ONLY NUMBERS";
                checkCorr.Background = Brushes.White;
                checkCorr.Foreground = Brushes.Red;
            }
            else if (String.IsNullOrWhiteSpace(bookid.Text))
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
                userIdGot = int.Parse(userid.Text);
                bookIdGot = int.Parse(bookid.Text);

                connectionString = "server=localhost;username=root;password=19621741;database=tsp_proekt";
                conn = new MySql.Data.MySqlClient.MySqlConnection(connectionString);

                string checkIfExQ = "SELECT COUNT(*) FROM books WHERE bookid=@bkid";
                MySqlCommand check = new MySqlCommand(checkIfExQ, conn);
                check.Parameters.AddWithValue("@bkid", bookIdGot);

                conn.Open();

                long checkExBk = (long)check.ExecuteScalar();

                conn.Close();

                string checkIfExUser = "SELECT COUNT(*) FROM users WHERE userid=@usid";
                MySqlCommand checkUs = new MySqlCommand(checkIfExUser, conn);
                checkUs.Parameters.AddWithValue("@usid", userIdGot);

                conn.Open();

                long checkExUs = (long)checkUs.ExecuteScalar();

                conn.Close();

                if (!(checkExUs > 0))
                {
                    checkCorr.Text = "THE USER ID DOES NOT EXIST";
                    checkCorr.Background = Brushes.White;
                    checkCorr.Foreground = Brushes.Red;
                }
                else if (!(checkExBk > 0))
                {
                    checkCorr.Text = "THE BOOK ID DOES NOT EXIST";
                    checkCorr.Background = Brushes.White;
                    checkCorr.Foreground = Brushes.Red;
                }
                else
                {
                    // check if available > 0
                    string qrCheckAvail = "SELECT available FROM booksstored WHERE bookid=@bkid";
                    MySqlCommand cmdCheckAvail = new MySqlCommand(qrCheckAvail, conn);
                    cmdCheckAvail.Parameters.AddWithValue("@bkid", bookIdGot);

                    conn.Open();
                    int checkAvail = (int)cmdCheckAvail.ExecuteScalar();
                    conn.Close();

                    if (!(checkAvail > 0))
                    {
                        checkCorr.Text = "NO BOOKS AVAILABLE TO LEND";
                        checkCorr.Background = Brushes.White;
                        checkCorr.Foreground = Brushes.Red;
                    }
                    else
                    {
                        dateGot = DateTime.Now.Date.ToString("yyyy-MM-dd");

                        string qrLendbooks = "INSERT INTO lendbooks(users_userid, books_bookid, lenddate) VALUES(?usid2, ?bkid2, ?lddt)";
                        MySqlCommand cmdLendBooks = new MySqlCommand(qrLendbooks, conn);
                        cmdLendBooks.Parameters.AddWithValue("@usid2", userIdGot);
                        cmdLendBooks.Parameters.AddWithValue("@bkid2", bookIdGot);
                        cmdLendBooks.Parameters.AddWithValue("@lddt", dateGot);

                        string qrBooksstored = "UPDATE booksstored SET available = available - 1, total = total - 1 WHERE bookid = @bksstid";
                        MySqlCommand cmdBooksstored = new MySqlCommand(qrBooksstored, conn);
                        cmdBooksstored.Parameters.AddWithValue("@bksstid", bookIdGot);

                        string qrExemplars = "UPDATE exemplars SET states_stateid=3 WHERE books_bookid=@exid";
                        MySqlCommand cmdExemplars = new MySqlCommand(qrExemplars, conn);
                        cmdExemplars.Parameters.AddWithValue("@exid", bookIdGot);

                        conn.Open();
                        cmdLendBooks.ExecuteNonQuery();
                        cmdBooksstored.ExecuteNonQuery();
                        cmdExemplars.ExecuteNonQuery();
                        conn.Close();

                        string qrLendBookId = "SELECT LAST_INSERT_ID()";
                        MySqlCommand cmdLendBookId = new MySqlCommand(qrLendBookId, conn);

                        conn.Open();
                        ulong lendBookIdGot = (ulong)cmdLendBookId.ExecuteScalar();
                        conn.Close();

                        showLendId.Text = "LENDBOOK ID: " + lendBookIdGot;
                        showLendId.Background = Brushes.White;
                        showLendId.Foreground = Brushes.Green;

                        checkCorr.Text = "BOOK LENDED SUCCESSFULLY";
                        checkCorr.Background = Brushes.White;
                        checkCorr.Foreground = Brushes.Green;

                        log.Info("Lendbook ID: " + lendBookIdGot + " was lended successfully!");
                    }
                }
            }
        }
    }
}