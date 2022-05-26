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
    /// Interaction logic for ReferencesOperator.xaml
    /// </summary>
    public partial class ReferencesOperator : Window
    {
        public ReferencesOperator()
        {
            InitializeComponent();
        }

        private void BookInfoButton(object sender, RoutedEventArgs e)
        {
            BookInfoOper f = new BookInfoOper();
            f.Show();
            Close();
        }

        private void UserInfoButton(object sender, RoutedEventArgs e)
        {
            UserInfoOper f = new UserInfoOper();
            f.Show();
            Close();
        }

        private void SubmittedFormsButton(object sender, RoutedEventArgs e)
        {
            SubmittedFormsOper f = new SubmittedFormsOper();
            f.Show();
            Close();
        }

        private void LendBookInfoButton(object sender, RoutedEventArgs e)
        {
            LendBookInfo f = new LendBookInfo();
            f.Show();
            Close();
        }

        private void GoBackButton(object sender, RoutedEventArgs e)
        {
            OperatorMenu f = new OperatorMenu();
            f.Show();
            Close();
        }
    }
}
