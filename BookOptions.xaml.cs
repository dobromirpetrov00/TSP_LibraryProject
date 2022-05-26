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
    /// Interaction logic for BookOptions.xaml
    /// </summary>
    public partial class BookOptions : Window
    {
        public BookOptions()
        {
            InitializeComponent();
        }

        private void GoBackButton(object sender, RoutedEventArgs e)
        {
            AdminMenu f = new AdminMenu();
            f.Show();
            Close();
        }

        private void AddBookButton(object sender, RoutedEventArgs e)
        {
            AddBook f = new AddBook();
            f.Show();
            Close();
        }

        private void ScrapBookButton(object sender, RoutedEventArgs e)
        {
            ScrapBook f = new ScrapBook();
            f.Show();
            Close();
        }

        private void ArchiveBookButton(object sender, RoutedEventArgs e)
        {
            ArchiveBook f = new ArchiveBook();
            f.Show();
            Close();
        }

        private void ArchiveLaterButton(object sender, RoutedEventArgs e)
        {
            ArchiveLater f = new ArchiveLater();
            f.Show();
            Close();
        }
    }
}
