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
    /// Interaction logic for OperatorMenu.xaml
    /// </summary>
    public partial class OperatorMenu : Window
    {
        public OperatorMenu()
        {
            InitializeComponent();
        }

        private void GoBackButton(object sender, RoutedEventArgs e)
        {
            OperatorLogin f = new OperatorLogin();
            f.Show();
            Close();
        }

        private void CreateReaderButton(object sender, RoutedEventArgs e)
        {
            CreateReader f = new CreateReader();
            f.Show();
            Close();
        }

        private void SignOutReaderButton(object sender, RoutedEventArgs e)
        {
            SignOutReader f = new SignOutReader();
            f.Show();
            Close();
        }

        private void GiveBookButton(object sender, RoutedEventArgs e)
        {
            GiveBook f = new GiveBook();
            f.Show();
            Close();
        }

        private void GetBackBookButton(object sender, RoutedEventArgs e)
        {
            GetBackBook f = new GetBackBook();
            f.Show();
            Close();
        }

        private void ReferencesButton(object sender, RoutedEventArgs e)
        {
            ReferencesOperator f = new ReferencesOperator();
            f.Show();
            Close();
        }
    }
}
