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
    /// Interaction logic for AddBook.xaml
    /// </summary>
    public partial class AddBook : Window
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public AddBook()
        {
            InitializeComponent();
            log4net.Config.XmlConfigurator.Configure();
        }

        MySql.Data.MySqlClient.MySqlConnection conn;
        string connectionString;

        private string booknameGot, authorGot, genreGot;
        private int yearGot, totalGot, availableGot, readingroomGot, parsedValue;

        private void GoBackButton(object sender, RoutedEventArgs e)
        {
            BookOptions f = new BookOptions();
            f.Show();
            Close();
        }

        private void CreateOperatorButton(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrWhiteSpace(bookname.Text))
            {
                checkCorr.Text = "ENTER A BOOK NAME";
                checkCorr.Background = Brushes.White;
                checkCorr.Foreground = Brushes.Red;
            }
            else if (bookname.Text.Length > 50)
            {
                checkCorr.Text = "THE BOOK NAME MUST BE LESS THAN 50 CHARACTERS";
                checkCorr.Background = Brushes.White;
                checkCorr.Foreground = Brushes.Red;
            }
            else if (String.IsNullOrWhiteSpace(author.Text))
            {
                checkCorr.Text = "ENTER AN AUTHOR NAME";
                checkCorr.Background = Brushes.White;
                checkCorr.Foreground = Brushes.Red;
            }
            else if (author.Text.Length > 50)
            {
                checkCorr.Text = "THE AUTHOR NAME MUST BE LESS THAN 50 CHARACTERS";
                checkCorr.Background = Brushes.White;
                checkCorr.Foreground = Brushes.Red;
            }
            else if (String.IsNullOrWhiteSpace(genre.Text))
            {
                checkCorr.Text = "ENTER A GENRE";
                checkCorr.Background = Brushes.White;
                checkCorr.Foreground = Brushes.Red;
            }
            else if (genre.Text.Length > 30)
            {
                checkCorr.Text = "THE GENRE MUST BE LESS THAN 50 CHARACTERS";
                checkCorr.Background = Brushes.White;
                checkCorr.Foreground = Brushes.Red;
            }
            else if (String.IsNullOrWhiteSpace(year.Text))
            {
                checkCorr.Text = "ENTER AN YEAR";
                checkCorr.Background = Brushes.White;
                checkCorr.Foreground = Brushes.Red;
            }
            else if (!int.TryParse(year.Text, out parsedValue))
            {
                checkCorr.Text = "THE YEAR MUST BE ONLY NUMBERS";
                checkCorr.Background = Brushes.White;
                checkCorr.Foreground = Brushes.Red;
            }
            else if (year.Text.Length > 4)
            {
                checkCorr.Text = "THE YEAR MUST BE MAXIMUM 4 DIGITS";
                checkCorr.Background = Brushes.White;
                checkCorr.Foreground = Brushes.Red;
            }
            else if (String.IsNullOrWhiteSpace(total.Text))
            {
                checkCorr.Text = "ENTER TOTAL BOOKS";
                checkCorr.Background = Brushes.White;
                checkCorr.Foreground = Brushes.Red;
            }
            else if (!int.TryParse(total.Text, out parsedValue))
            {
                checkCorr.Text = "THE TOTAL BOOKS MUST BE ONLY NUMBERS";
                checkCorr.Background = Brushes.White;
                checkCorr.Foreground = Brushes.Red;
            }
            else if (String.IsNullOrWhiteSpace(available.Text))
            {
                checkCorr.Text = "ENTER AVAILABLE BOOKS";
                checkCorr.Background = Brushes.White;
                checkCorr.Foreground = Brushes.Red;
            }
            else if (!int.TryParse(available.Text, out parsedValue))
            {
                checkCorr.Text = "THE AVAILABLE BOOKS MUST BE ONLY NUMBERS";
                checkCorr.Background = Brushes.White;
                checkCorr.Foreground = Brushes.Red;
            }
            else if (String.IsNullOrWhiteSpace(readingroom.Text))
            {
                checkCorr.Text = "ENTER READING ROOM BOOKS";
                checkCorr.Background = Brushes.White;
                checkCorr.Foreground = Brushes.Red;
            }
            else if (!int.TryParse(readingroom.Text, out parsedValue))
            {
                checkCorr.Text = "THE READING ROOM BOOKS MUST BE ONLY NUMBERS";
                checkCorr.Background = Brushes.White;
                checkCorr.Foreground = Brushes.Red;
            }
            else if (int.Parse(available.Text) + int.Parse(readingroom.Text) != int.Parse(total.Text))
            {
                checkCorr.Text = "THE BOOK PIECES DO NOT MATCH";
                checkCorr.Background = Brushes.White;
                checkCorr.Foreground = Brushes.Red;
            }
            else
            {
                booknameGot = bookname.Text;
                authorGot = author.Text;
                genreGot = genre.Text;
                yearGot = int.Parse(year.Text);
                string isforarch = "no";
                totalGot = int.Parse(total.Text);
                availableGot = int.Parse(available.Text);
                readingroomGot = int.Parse(readingroom.Text);

                connectionString = "server=localhost;username=root;password=19621741;database=tsp_proekt";
                conn = new MySql.Data.MySqlClient.MySqlConnection(connectionString);

                string books = "INSERT INTO books(bookname, author, genre, year, isforarchive) VALUES(?bn, ?au, ?gn, ?yr, ?ar)";
                string booksstored = "INSERT INTO booksstored(bookid, total, available, readingroom) VALUES(?bkidd, ?tt, ?ava, ?rdrm)";
                string exemplars = "INSERT INTO exemplars(books_bookid, states_stateid) VALUES(?bkid, ?stid)";
                string getId = "SELECT LAST_INSERT_ID()";

                MySqlCommand bookIns = new MySqlCommand(books, conn);
                MySqlCommand getLastId = new MySqlCommand(getId, conn);
                MySqlCommand booksstoredIns = new MySqlCommand(booksstored, conn);
                MySqlCommand exemplarsIns = new MySqlCommand(exemplars, conn);

                conn.Open();

                bookIns.Parameters.AddWithValue("?bn", booknameGot);
                bookIns.Parameters.AddWithValue("?au", authorGot);
                bookIns.Parameters.AddWithValue("?gn", genreGot);
                bookIns.Parameters.AddWithValue("?yr", yearGot);
                bookIns.Parameters.AddWithValue("?ar", isforarch);
                bookIns.ExecuteNonQuery();

                var bookLastId = getLastId.ExecuteScalar();

                booksstoredIns.Parameters.AddWithValue("?bkidd", bookLastId);
                booksstoredIns.Parameters.AddWithValue("?tt", totalGot);
                booksstoredIns.Parameters.AddWithValue("?ava", availableGot);
                booksstoredIns.Parameters.AddWithValue("?rdrm", readingroomGot);
                booksstoredIns.ExecuteNonQuery();

                exemplarsIns.Parameters.AddWithValue("?bkid", bookLastId);
                exemplarsIns.Parameters.AddWithValue("?stid", 1);
                exemplarsIns.ExecuteNonQuery();

                conn.Close();

                ulong i = (ulong)bookLastId;
                checkCorr.Text = "BOOK WITH ID: " + i + " ADDED SUCCESSFULLY";
                checkCorr.Background = Brushes.White;
                checkCorr.Foreground = Brushes.Green;

                log.Info("Book added successfully!");
            }
        }
    }
}
