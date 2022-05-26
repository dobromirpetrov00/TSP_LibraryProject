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

namespace TSP_Proekt
{
    /// <summary>
    /// Interaction logic for OperatorLogin.xaml
    /// </summary>

    public partial class OperatorLogin : Window
    {
        public OperatorLogin()
        {
            InitializeComponent();
        }

        MySql.Data.MySqlClient.MySqlConnection conn;
        string connectionString;
        private String usernameGot;
        private String passwordGot;

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MainWindow f = new MainWindow();
            f.Show();
            Close();
        }

        private void OperatorLoginButton(object sender, RoutedEventArgs e)
        {
            usernameGot = username.Text;
            passwordGot = password.Password.ToString();

            connectionString = "server=localhost;username=root;password=19621741;database=tsp_proekt";
            conn = new MySql.Data.MySqlClient.MySqlConnection(connectionString);
            string query = "SELECT username, password FROM users WHERE username=@un AND password=@pw AND usertypes_usertypeid=@ustp";

            MySqlCommand cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@un", usernameGot);
            cmd.Parameters.AddWithValue("@pw", passwordGot);
            cmd.Parameters.AddWithValue("@ustp", 2);
            conn.Open();

            var result = cmd.ExecuteScalar();
            
            conn.Close();

            if (result != null)
            {
                OperatorMenu f = new OperatorMenu();
                f.Show();
                f.Title = "Operator " + usernameGot;
                Close();
            }
            else
            {
                MessageBox.Show("Please enter a valid username and password!","Wrong username or password!");
                username.Clear();
                password.Clear();
            }
        }
    }
}
