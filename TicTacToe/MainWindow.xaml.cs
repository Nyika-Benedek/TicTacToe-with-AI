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
using TicTacToe.Interfaces;
using TicTacToe.Model;
using TicTacToe.Models;

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

        private const int CelldSize = 100;
        public enum LineType { GridLine, XLine }

        private void DrawLine(int x1, int x2, int y1, int y2, LineType color) {
            var myLine = new Line();
            if (color == LineType.GridLine)
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

        private void DrawX(Coordinate coordinate)
        {
            int x = coordinate.X;
            int y = coordinate.Y;
            //cell's right left corner
            x = x * 100;
            y = y * 100;
            DrawLine(x, x + CelldSize, y, y + CelldSize, LineType.XLine);
            DrawLine(x, x+CelldSize, y + CelldSize, y, LineType.XLine);
        }

        private void DrawCirle(Coordinate coordinate)
        {
            Ellipse myEllipse = new Ellipse();

            // Describes the brush's color using RGB values.
            // Each value has a range of 0-255.
            myEllipse.StrokeThickness = 2;
            myEllipse.Stroke = Brushes.White;

            // Set the width and height of the Ellipse.
            myEllipse.Width = CelldSize;
            myEllipse.Height = CelldSize;

            Canvas.SetLeft(myEllipse, coordinate.X * CelldSize);
            Canvas.SetTop(myEllipse, coordinate.Y * CelldSize);

            // Add the Ellipse to the StackPanel.
            canvas.Children.Add(myEllipse);
        }

        private void DrawNewField() {
            canvas.Children.Clear();
            // Vertical Lines
            DrawLine(100, 100, 0, 300, LineType.GridLine);
            DrawLine(200, 200, 0, 300, LineType.GridLine);

            // Horizontal Lines
            DrawLine(0, 300, 100, 100, LineType.GridLine);
            DrawLine(0, 300, 200, 200, LineType.GridLine);
        }

        private Coordinate GetMousePosition() {
            int x = (int)(Mouse.GetPosition(canvas).X / CelldSize);
            int y = (int)(Mouse.GetPosition(canvas).Y / CelldSize);
            return new Coordinate(x, y);
        }

        private void CanvasClick(object sender, MouseButtonEventArgs e)
        {
            
            DrawX(GetMousePosition());
            DrawCirle(GetMousePosition());
            
        }

        private void NewGame(object sender, RoutedEventArgs e)
        {
            var newGameWindow = new NewGameWindow();
            newGameWindow.ShowDialog();
            DrawNewField();

            IGame game = new Game();
            game.AddPlayer(new Player(newGameWindow.Player1Name));
            game.AddPlayer(new Player(newGameWindow.Player2Name));
        }

        private void Query(object sender, RoutedEventArgs e)
        {

        }

    }
}
