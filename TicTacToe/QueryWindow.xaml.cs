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
        /// <summary>
        /// Create a connection with the database.
        /// </summary>
        DatabaseCommands database = new DatabaseCommands();

        public QueryWindow()
        {
            InitializeComponent();


            RefreshDatagrid();
        }

        /// <summary>
        /// Refill the datagrid with all entry from the database.
        /// </summary>
        public void RefreshDatagrid() 
        {
            QueryGrid.Items.Clear();
            


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

        /// <summary>
        /// Refill the datagrid from the datagrid filter by name
        /// </summary>
        /// <param name="sender">The interacted object.</param>
        /// <param name="e">Data of the mouse related event.</param>
        private void FilterByName(object sender, RoutedEventArgs e)
        {
            QueryGrid.Items.Clear();

            foreach (var entry in database.GetAll())
            {
                if (entry.player1.Contains(FilterText.Text) || entry.player2.Contains(FilterText.Text))
                {
                    QueryGrid.Items.Add(entry);
                }
            }
        }

        /// <summary>
        /// Delete all the stored data from the databse.
        /// </summary>
        /// <param name="sender">The interacted object.</param>
        /// <param name="e">Data of the mouse related event.</param>
        private void DropDb(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Are you sure, you want to delete all entry?", "Delete Database", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
            {
                return;
            }
            else
            {
                foreach (var entry in database.GetAll())
                {
                    database.DeleteById(entry.id);
                }
                RefreshDatagrid();
            }
        }

        /// <summary>
        /// Clear placeholder on focus.
        /// </summary>
        /// <param name="sender">The interacted object.</param>
        /// <param name="e">Data of the mouse related event.</param>
        private void ClearFilterText(object sender, RoutedEventArgs e)
        {
            if (FilterText.Text == "Filter by name")
            {
                FilterText.Text = "";
            }
        }

        /// <summary>
        /// Rewrite placeholder if it was empty on focus lose.
        /// </summary>
        /// <param name="sender">The interacted object.</param>
        /// <param name="e">Data of the mouse related event.</param>
        private void FillFilterText(object sender, RoutedEventArgs e)
        {
            if (FilterText.Text == "")
            {
                FilterText.Text = "Filter by name";
            }
        }

        /// <summary>
        /// This function called, when the filter has been deleted.
        /// Refreshing the datagrid content withoit the filters.
        /// </summary>
        /// <param name="sender">The interacted object.</param>
        /// <param name="e">Data of the mouse related event.</param>
        private void ResetQuery(object sender, RoutedEventArgs e)
        {
            RefreshDatagrid();
        }
    }
}
