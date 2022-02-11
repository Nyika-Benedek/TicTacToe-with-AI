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
using System.Windows.Navigation;
using System.Windows.Shapes;
using TicTacToe.Entity;

namespace TicTacToe
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            /*
            DatabaseContextFactory databaseFactory = new DatabaseContextFactory();
            using var databaseContext = databaseFactory.CreateDbContext(new string[0]);

            // adding entry to database
            DatabaseStructure entry = new DatabaseStructure();
            databaseContext.database.Add(entry);
            databaseContext.SaveChanges();
            MessageBox.Show($"id:{entry.id} > Entry added!");

            // deleting entry by id
            databaseContext.Remove( databaseContext.database.Find(1));
            databaseContext.SaveChanges();
            


            DatabaseCommands database = new DatabaseCommands();

            database.AddEntry();
            database.DeleteById(2);
            database.UpdateById(5);

            foreach (var entry in database.GetAll()) {
                dataGrid.Items.Add(entry);
            }
            */

            DrawNewField();
        }

        public enum Color { GridLine, XLine }

        private void DrawLine(int x1, int x2, int y1, int y2, Color color) {
            var myLine = new Line();
            if (color == Color.GridLine)
            {
                myLine.Stroke = System.Windows.Media.Brushes.Green;
            }
            else
            {
                myLine.Stroke = System.Windows.Media.Brushes.White;
            }
            
            myLine.X1 = x1;
            myLine.X2 = x2;
            myLine.Y1 = y1;
            myLine.Y2 = y2;
            myLine.HorizontalAlignment = HorizontalAlignment.Left;
            myLine.VerticalAlignment = VerticalAlignment.Center;
            myLine.StrokeThickness = 2;
            canvas.Children.Add(myLine);
        }

        private void DrawNewField() {
            canvas.Children.Clear();
            // Vertical Lines
            DrawLine(100, 100, 0, 300, Color.GridLine);
            DrawLine(200, 200, 0, 300, Color.GridLine);

            // Horizontal Lines
            DrawLine(0, 300, 100, 100, Color.GridLine);
            DrawLine(0, 300, 200, 200, Color.GridLine);
        }

        private void NewGame(object sender, RoutedEventArgs e)
        {

        }

        private void Query(object sender, RoutedEventArgs e)
        {

        }

        private void CanvasClick(object sender, MouseButtonEventArgs e)
        {

        }
    }
}
