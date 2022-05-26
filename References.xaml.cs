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

namespace TSP_Proekt
{
    /// <summary>
    /// Interaction logic for References.xaml
    /// </summary>
    public partial class References : Window
    {
        public References()
        {
            InitializeComponent();
        }

        private void GoBackButton(object sender, RoutedEventArgs e)
        {
            AdminMenu f = new AdminMenu();
            f.Show();
            Close();
        }

        private void SubmittedFormsButton(object sender, RoutedEventArgs e)
        {
            SubmittedForms f = new SubmittedForms();
            f.Show();
            Close();
        }

        private void BookInfoButton(object sender, RoutedEventArgs e)
        {
            BookInfo f = new BookInfo();
            f.Show();
            Close();
        }

        private void UserInfoButton(object sender, RoutedEventArgs e)
        {
            UserInfo f = new UserInfo();
            f.Show();
            Close();
        }
    }
}
