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
    /// Interaction logic for GetBackBook.xaml
    /// </summary>
    public partial class GetBackBook : Window
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public GetBackBook()
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

        private int userIdGot, bookIdGot, lendBookIdGot, parsedValue;
        private String dateGot;

        private void GetBookBackButton(object sender, RoutedEventArgs e)
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
            else if (String.IsNullOrWhiteSpace(lendbookid.Text))
            {
                checkCorr.Text = "ENTER A LENDBOOK ID";
                checkCorr.Background = Brushes.White;
                checkCorr.Foreground = Brushes.Red;
            }
            else if (!int.TryParse(lendbookid.Text, out parsedValue))
            {
                checkCorr.Text = "THE LENDBOOK ID MUST BE ONLY NUMBERS";
                checkCorr.Background = Brushes.White;
                checkCorr.Foreground = Brushes.Red;
            }
            else
            {
                userIdGot = int.Parse(userid.Text);
                bookIdGot = int.Parse(bookid.Text);
                lendBookIdGot = int.Parse(lendbookid.Text);

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

                string checkIfExLendBook = "SELECT COUNT(*) FROM lendbooks WHERE lendbookid=@lndbkid";
                MySqlCommand checkLendBook = new MySqlCommand(checkIfExLendBook, conn);
                checkLendBook.Parameters.AddWithValue("@lndbkid", lendBookIdGot);

                conn.Open();

                long checkExLndBk = (long)checkLendBook.ExecuteScalar();

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
                else if (!(checkExLndBk > 0))
                {
                    checkCorr.Text = "THE LENDBOOK ID DOES NOT EXIST";
                    checkCorr.Background = Brushes.White;
                    checkCorr.Foreground = Brushes.Red;
                }
                else
                {
                    string qrCheckAll = "SELECT COUNT(*) FROM lendbooks WHERE lendbookid=@lbid3 AND users_userid=@usid3 AND books_bookid=@bkid3";
                    MySqlCommand cmdCheckAll = new MySqlCommand(qrCheckAll, conn);
                    cmdCheckAll.Parameters.AddWithValue("@lbid3", lendBookIdGot);
                    cmdCheckAll.Parameters.AddWithValue("@usid3", userIdGot);
                    cmdCheckAll.Parameters.AddWithValue("@bkid3", bookIdGot);

                    conn.Open();

                    long checkAllGot = (long)cmdCheckAll.ExecuteScalar();

                    conn.Close();

                    if (!(checkAllGot > 0))
                    {
                        checkCorr.Text = "THIS IS NOT A BOOK LENT BY THIS READER";
                        checkCorr.Background = Brushes.White;
                        checkCorr.Foreground = Brushes.Red;
                    }
                    else
                    {
                        string qrLendbooks = "DELETE FROM lendbooks WHERE lendbookid=@lndbkid4";
                        MySqlCommand cmdLendBooks = new MySqlCommand(qrLendbooks, conn);
                        cmdLendBooks.Parameters.AddWithValue("@lndbkid4", lendBookIdGot);

                        string qrBooksstored = "UPDATE booksstored SET available = available + 1, total = total + 1 WHERE bookid = @bkid4";
                        MySqlCommand cmdBooksstored = new MySqlCommand(qrBooksstored, conn);
                        cmdBooksstored.Parameters.AddWithValue("@bkid4", bookIdGot);

                        string qrExemplars = "UPDATE exemplars SET states_stateid=1 WHERE books_bookid=@exid4";
                        MySqlCommand cmdExemplars = new MySqlCommand(qrExemplars, conn);
                        cmdExemplars.Parameters.AddWithValue("@exid4", bookIdGot);

                        conn.Open();
                        cmdLendBooks.ExecuteNonQuery();
                        cmdBooksstored.ExecuteNonQuery();
                        cmdExemplars.ExecuteNonQuery();
                        conn.Close();

                        checkCorr.Text = "BOOK RETURNED SUCCESSFULLY";
                        checkCorr.Background = Brushes.White;
                        checkCorr.Foreground = Brushes.Green;

                        log.Info("Book was returned successfully!");
                    }
                }
            }
        }
    }
}