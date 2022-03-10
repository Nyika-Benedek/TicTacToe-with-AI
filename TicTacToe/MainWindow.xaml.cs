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

        private int AiIteration = 0;

        private IGame game;

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
            DrawLine(x, x + CelldSize, y + CelldSize, y, LineType.XLine);
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

        private void DrawPlayerClick(IPlayer player) {
            if (player.symbol == 'X')
            {
                DrawX(GetMousePosition());
            }
            else
            {
                DrawCirle(GetMousePosition());
            }
        }

        private void PlayerMove() {
            DrawPlayerClick(game.CurrentPlayer);
            game.Field.AddMove(GetMousePosition(), game.CurrentPlayer.symbol);
        }

        public void PostWinCondition() {
            game.EndGame();
            if (game.Field.FieldState == FieldState.NoSpaceLeft)
            {
                MessageBox.Show("Unfortunately, this is a tie.");
            }
            else
            {
                MessageBox.Show($"Congratulation {game.Winner.name}, you win!");
            }
            // TODO: DATABSE UPDATE
        }

        private void CanvasClick(object sender, MouseButtonEventArgs e)
        {
            if (game is null || game.GameState == GameState.None || game.GameState == GameState.Finneshed)
            {
                return;
            }

            if (game.GameType == GameType.PvP)
            {
                PlayerMove();
                if (game.isEnded())
                {
                    PostWinCondition();
                }
                game.NextPlayer();
            }

            if (game.GameType == GameType.PvAI)
            {
                PlayerMove();
                if (game.isEnded())
                {
                    PostWinCondition();
                }
                else
                {
                    game.NextPlayer();
                    // TODO: AI act
                    if (game.isEnded())
                    {
                        PostWinCondition();
                    }
                }
            }

            LetAiPlay();
        }

        private void LetAiPlay()
        {
            for (int i = 0; i < AiIteration; i++)
            {
                while (!game.isEnded())
                {
                    // TODO: AI-1 act
                    if (game.isEnded())
                    {
                        break;
                    }
                    // TODO: AI-2 act
                }
                PostWinCondition();
            }
        }

        private void NewGame(object sender, RoutedEventArgs e)
        {
            var newGameWindow = new NewGameWindow();
            newGameWindow.ShowDialog();
            DrawNewField();

            game = new Game();
            game.AddPlayer(new Player(newGameWindow.Player1Name, 'X'));
            game.AddPlayer(new Player(newGameWindow.Player2Name, 'O'));
            game.GameType = newGameWindow.gameType;
            AiIteration = newGameWindow.XGames;

            game.Start();

            if (game.GameType == GameType.AIvAI)
            {
                MessageBox.Show("We are ready! \rPlease click on the playing field to start.");
            }
        }

        private void Query(object sender, RoutedEventArgs e)
        {

        }

    }
}
