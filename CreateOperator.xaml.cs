using System;
using System.Drawing;
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
    /// Interaction logic for CreateOperator.xaml
    /// </summary>
    public partial class CreateOperator : Window
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public CreateOperator()
        {
            InitializeComponent();
            log4net.Config.XmlConfigurator.Configure();
        }

        MySql.Data.MySqlClient.MySqlConnection conn;
        string connectionString;

        private void GoBackButton(object sender, RoutedEventArgs e)
        {
            AdminMenu f = new AdminMenu();
            f.Show();
            Close();
        }

        private String usernGot, passGot, nameGot, phoneGot, emailGot, dateGot;
        private int parsedValue;

        private bool EmailDot(string email)
        {
            try
            {
                var dotIndex = email.IndexOf(".");
                return dotIndex > email.IndexOf("@") && dotIndex + 1 != email.Length;
            }
            catch
            {
                return false;
            }
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        private void CreateOperatorButton(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrWhiteSpace(username.Text))
            {
                checkCorr.Text = "ENTER A USERNAME";
                checkCorr.Background = Brushes.White;
                checkCorr.Foreground = Brushes.Red;
            }
            else if (!(username.Text.Length > 2 && username.Text.Length < 11))
            {
                checkCorr.Text = "THE USERNAME MUST BE BETWEEN 3 AND 10 CHARACTERS";
                checkCorr.Background = Brushes.White;
                checkCorr.Foreground = Brushes.Red;
            }
            else if (String.IsNullOrWhiteSpace(password.Password.ToString()))
            {
                checkCorr.Text = "ENTER A PASSWORD";
                checkCorr.Background = Brushes.White;
                checkCorr.Foreground = Brushes.Red;
            }
            else if (!(password.Password.ToString().Length > 2 && password.Password.ToString().Length < 11))
            {
                checkCorr.Text = "THE PASSWORD MUST BE BETWEEN 3 AND 10 CHARACTERS";
                checkCorr.Background = Brushes.White;
                checkCorr.Foreground = Brushes.Red;
            }
            else if (String.IsNullOrWhiteSpace(name.Text))
            {
                checkCorr.Text = "ENTER TWO NAMES";
                checkCorr.Background = Brushes.White;
                checkCorr.Foreground = Brushes.Red;
            }
            else if (String.IsNullOrWhiteSpace(phone.Text))
            {
                checkCorr.Text = "ENTER A PHONE";
                checkCorr.Background = Brushes.White;
                checkCorr.Foreground = Brushes.Red;
            }
            else if (!int.TryParse(phone.Text, out parsedValue))
            {
                checkCorr.Text = "THE PHONE MUST BE ONLY NUMBERS";
                checkCorr.Background = Brushes.White;
                checkCorr.Foreground = Brushes.Red;
            }
            else if (phone.Text.Length != 10)
            {
                checkCorr.Text = "THE PHONE NUMBER MUST BE 10 DIGITS LONG";
                checkCorr.Background = Brushes.White;
                checkCorr.Foreground = Brushes.Red;
            }
            else if (String.IsNullOrWhiteSpace(email.Text))
            {
                checkCorr.Text = "ENTER AN EMAIL";
                checkCorr.Background = Brushes.White;
                checkCorr.Foreground = Brushes.Red;
            }
            else if (!IsValidEmail(email.Text) || !EmailDot(email.Text))
            {
                checkCorr.Text = "ENTER A VALID EMAIL";
                checkCorr.Background = Brushes.White;
                checkCorr.Foreground = Brushes.Red;
            }
            else
            {
                usernGot = username.Text;
                passGot = password.Password.ToString();
                nameGot = name.Text;
                phoneGot = phone.Text;
                emailGot = email.Text;
                dateGot = DateTime.Now.Date.ToString("yyyy-MM-dd");

                connectionString = "server=localhost;username=root;password=19621741;database=tsp_proekt";
                conn = new MySql.Data.MySqlClient.MySqlConnection(connectionString);

                string query = "INSERT INTO users(username, password, approvaldate, usertypes_usertypeid) VALUES(?un, ?pw, ?apdt, ?ustp)";
                string query2 = "INSERT INTO userinfos(users_userid, name, phone, email) VALUES(?usrid, ?nm, ?phn, ?ml)";
                string query3 = "INSERT INTO forms(creationdate, users_userid) VALUES(?crdt, ?usid)";
                string getId = "SELECT userid FROM users WHERE username=@usnm2 AND password=@pswd2";

                MySqlCommand cmd = new MySqlCommand(query, conn);
                MySqlCommand cmd2 = new MySqlCommand(query2, conn);
                MySqlCommand cmd3 = new MySqlCommand(query3, conn);
                MySqlCommand getIdCmd = new MySqlCommand(getId, conn);

                conn.Open();

                cmd.Parameters.AddWithValue("?un", usernGot);
                cmd.Parameters.AddWithValue("?pw", passGot);
                cmd.Parameters.AddWithValue("?apdt", dateGot);
                cmd.Parameters.AddWithValue("?ustp", 2);

                cmd.ExecuteNonQuery();

                getIdCmd.Parameters.AddWithValue("@usnm2", usernGot);
                getIdCmd.Parameters.AddWithValue("@pswd2", passGot);

                var getIdGot = getIdCmd.ExecuteScalar();

                cmd2.Parameters.AddWithValue("?usrid", getIdGot);
                cmd2.Parameters.AddWithValue("?nm", nameGot);
                cmd2.Parameters.AddWithValue("?phn", phoneGot);
                cmd2.Parameters.AddWithValue("?ml", emailGot);

                cmd2.ExecuteNonQuery();

                cmd3.Parameters.AddWithValue("?crdt", dateGot);
                cmd3.Parameters.AddWithValue("?usid", getIdGot);

                cmd3.ExecuteNonQuery();

                conn.Close();

                checkCorr.Text = "OPERATOR WITH ID: " + getIdGot + " CREATED SUCCESSFULLY";
                checkCorr.Background = Brushes.White;
                checkCorr.Foreground = Brushes.Green;

                log.Info("Operator created successfully!");
            }
        }
    }
}
