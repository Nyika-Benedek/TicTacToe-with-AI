using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using TicTacToe.Entity;

namespace TicTacToe
{
    /// <summary>
    /// Interaction logic for QueryWindow.xaml
    /// </summary>
    public partial class QueryWindow : Window
    {
        DatabaseContextFactory databaseFactory = new DatabaseContextFactory();

        public QueryWindow()
        {
            InitializeComponent();


            RefreshDatagrid();
        }

        public void RefreshDatagrid() 
        {
            QueryGrid.Items.Clear();
            using var databaseContext = databaseFactory.CreateDbContext(new string[0]);
            DatabaseCommands database = new DatabaseCommands();


            /*
             * example how to use this:
             * database.AddEntry();
             * database.DeleteById(2);
             * database.UpdateById(5);
            */

            foreach (var entry in database.GetAll())
            {
                QueryGrid.Items.Add(entry);
            }
        }

        private void FilterByName(object sender, RoutedEventArgs e)
        {
            QueryGrid.Items.Clear();

            using var databaseContext = databaseFactory.CreateDbContext(new string[0]);
            DatabaseCommands database = new DatabaseCommands();
            foreach (var entry in database.GetAll())
            {
                if (entry.player1.Contains(FilterText.Text) || entry.player2.Contains(FilterText.Text))
                {
                    QueryGrid.Items.Add(entry);
                }
            }
        }

        private void DropDb(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Are you sure, you want to delete all entry?", "Delete Database", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
            {
                return;
            }
            else
            {
                using var databaseContext = databaseFactory.CreateDbContext(new string[0]);
                DatabaseCommands database = new DatabaseCommands();
                foreach (var entry in database.GetAll())
                {
                    database.DeleteById(entry.id);
                }
                RefreshDatagrid();
            }
        }

        private void ClearFilterText(object sender, RoutedEventArgs e)
        {
            if (FilterText.Text == "Filter by name")
            {
                FilterText.Text = "";
            }
        }

        private void FillFilterText(object sender, RoutedEventArgs e)
        {
            if (FilterText.Text == "")
            {
                FilterText.Text = "Filter by name";
            }
        }

        private void ResetQuery(object sender, RoutedEventArgs e)
        {
            RefreshDatagrid();
        }
    }
}
