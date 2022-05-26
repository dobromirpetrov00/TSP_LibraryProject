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
    /// Interaction logic for SignOutReader.xaml
    /// </summary>
    public partial class SignOutReader : Window
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public SignOutReader()
        {
            InitializeComponent();
            log4net.Config.XmlConfigurator.Configure();
        }

        MySql.Data.MySqlClient.MySqlConnection conn;
        string connectionString;

        private int userIdGot, parsedValue;
        private string usernameGot, passwordGot;

        private void SignOutReaderButton(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrWhiteSpace(userid.Text))
            {
                checkCorr.Text = "ENTER A READER ID";
                checkCorr.Background = Brushes.White;
                checkCorr.Foreground = Brushes.Red;
            }
            else if (!int.TryParse(userid.Text, out parsedValue))
            {
                checkCorr.Text = "THE READER ID MUST BE ONLY NUMBERS";
                checkCorr.Background = Brushes.White;
                checkCorr.Foreground = Brushes.Red;
            }
            else if (String.IsNullOrWhiteSpace(username.Text))
            {
                checkCorr.Text = "ENTER A USERNAME";
                checkCorr.Background = Brushes.White;
                checkCorr.Foreground = Brushes.Red;
            }
            else if (String.IsNullOrWhiteSpace(password.Password.ToString()))
            {
                checkCorr.Text = "ENTER A PASSWORD";
                checkCorr.Background = Brushes.White;
                checkCorr.Foreground = Brushes.Red;
            }
            else
            {
                userIdGot = int.Parse(userid.Text);
                usernameGot = username.Text;
                passwordGot = password.Password.ToString();

                connectionString = "server=localhost;username=root;password=19621741;database=tsp_proekt";
                conn = new MySql.Data.MySqlClient.MySqlConnection(connectionString);

                string checkIfExQ = "SELECT COUNT(*) FROM users WHERE userid=@bkid AND username=@usnm AND password=@pswd";
                MySqlCommand check = new MySqlCommand(checkIfExQ, conn);
                check.Parameters.AddWithValue("@bkid", userIdGot);
                check.Parameters.AddWithValue("@usnm", usernameGot);
                check.Parameters.AddWithValue("@pswd", passwordGot);

                conn.Open();

                long checkEx = (long)check.ExecuteScalar();

                conn.Close();

                if (checkEx > 0)
                {
                    string checkIfLend = "SELECT COUNT(*) FROM lendbooks WHERE users_userid=@bkid";
                    MySqlCommand checkLend = new MySqlCommand(checkIfLend, conn);
                    checkLend.Parameters.AddWithValue("@bkid", userIdGot);

                    conn.Open();

                    long checkLnd = (long)checkLend.ExecuteScalar();

                    conn.Close();

                    if (checkLnd > 0)
                    {
                        checkCorr.Text = "THE READER HAS LENT BOOKS";
                        checkCorr.Background = Brushes.White;
                        checkCorr.Foreground = Brushes.Red;
                    }
                    else
                    {
                        string rmUserinfos = "DELETE FROM userinfos WHERE users_userid=@exid";
                        MySqlCommand rmExemp = new MySqlCommand(rmUserinfos, conn);
                        rmExemp.Parameters.AddWithValue("@exid", userIdGot);

                        string rmForms = "DELETE FROM forms WHERE users_userid=@bksstid";
                        MySqlCommand rmBksst = new MySqlCommand(rmForms, conn);
                        rmBksst.Parameters.AddWithValue("@bksstid", userIdGot);

                        string rmUsers = "DELETE FROM users WHERE userid=@bkid";
                        MySqlCommand rmBk = new MySqlCommand(rmUsers, conn);
                        rmBk.Parameters.AddWithValue("@bkid", userIdGot);

                        conn.Open();
                        rmExemp.ExecuteNonQuery();
                        rmBksst.ExecuteNonQuery();
                        rmBk.ExecuteNonQuery();
                        conn.Close();

                        checkCorr.Text = "SIGNED OUT READER SUCCESSFULLY";
                        checkCorr.Background = Brushes.White;
                        checkCorr.Foreground = Brushes.Green;

                        log.Info("Reader profile signed out successfully!");
                    }
                    
                }
                else
                {
                    checkCorr.Text = "WRONG DATA";
                    checkCorr.Background = Brushes.White;
                    checkCorr.Foreground = Brushes.Red;

                    log.Error("Tried to sign out reader profile with wrong data!");
                }
            }
        }

        private void GoBackButton(object sender, RoutedEventArgs e)
        {
            OperatorMenu f = new OperatorMenu();
            f.Show();
            Close();
        }
    }
}
