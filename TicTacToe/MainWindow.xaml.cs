using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
using TicTacToe.AI;
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


            DatabaseContextFactory databaseFactory = new DatabaseContextFactory();

            using var connection = databaseFactory.CreateDbContext(new string[0]);
            {

                if (connection.Database.EnsureCreated())
                {
                    connection.Database.EnsureDeleted();
                    connection.Database.Migrate();
                    MessageBox.Show("No database was found in this device!" + '\n' + "Creating database...", "Database wizard");
                }
                   

            }
        }

        /// <summary>
        /// How many AI vs AI games should compute at once.
        /// </summary>
        private int AiIteration = 10;

        private IGame game;

        /// <summary>
        /// A cell's size in pixels.
        /// </summary>
        private const int CellSize = 100;

        /// <summary>
        /// Which type of line should be drawn on the canvas.
        /// </summary>
        public enum LineType { GridLine, XLine }
        private Ai ai1;
        private Ai ai2;

        /// <summary>
        /// Draw a single line on the canvas from the starting coordinate to the ending coordinate with a given color type.
        /// </summary>
        /// <param name="x1">Starting X coordinate.</param>
        /// <param name="x2">Ending X coordinate.</param>
        /// <param name="y1">Starting Y coordinate.</param>
        /// <param name="y2">Ending Y coordinate.</param>
        /// <param name="color"><see cref="LineType"/> of the drawn object.</param>
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

        /// <summary>
        /// Draw X into the given coordinate.
        /// </summary>
        /// <param name="coordinate">Where to draw the X sign.</param>
        private void DrawX(Coordinate coordinate)
        {
            int x = coordinate.X;
            int y = coordinate.Y;
            //cell's right left corner
            x = x * CellSize;
            y = y * CellSize;
            DrawLine(x, x + CellSize, y, y + CellSize, LineType.XLine);
            DrawLine(x, x + CellSize, y + CellSize, y, LineType.XLine);
        }

        /// <summary>
        /// Draw a circle into the given coordinate.
        /// </summary>
        /// <param name="coordinate">Where to draw the circle.</param>
        private void DrawCirle(Coordinate coordinate)
        {
            Ellipse myEllipse = new Ellipse();

            // Describes the brush's color using RGB values.
            // Each value has a range of 0-255.
            myEllipse.StrokeThickness = 2;
            myEllipse.Stroke = Brushes.White;

            // Set the width and height of the Ellipse.
            myEllipse.Width = CellSize;
            myEllipse.Height = CellSize;

            Canvas.SetLeft(myEllipse, coordinate.X * CellSize);
            Canvas.SetTop(myEllipse, coordinate.Y * CellSize);

            // Add the Ellipse to the StackPanel.
            canvas.Children.Add(myEllipse);
        }

        /// <summary>
        /// Draw a new field grid.
        /// </summary>
        private void DrawNewField() {
            canvas.Children.Clear();
            // Vertical Lines
            DrawLine(100, 100, 0, 300, LineType.GridLine);
            DrawLine(200, 200, 0, 300, LineType.GridLine);

            // Horizontal Lines
            DrawLine(0, 300, 100, 100, LineType.GridLine);
            DrawLine(0, 300, 200, 200, LineType.GridLine);
        }

        /// <summary>
        /// Gets the coordinate, which cell the mouse in.
        /// </summary>
        /// <returns><see cref="Coordinate"/> of which cell is the mouse in.</returns>
        private Coordinate GetMousePosition() {
            int x = (int)(Mouse.GetPosition(canvas).X / CellSize);
            int y = (int)(Mouse.GetPosition(canvas).Y / CellSize);
            return new Coordinate(x, y);
        }

        /// <summary>
        /// Draw the actual player's symbol into the mouse position.
        /// </summary>
        /// <param name="player"></param>
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

        /// <summary>
        /// Draw the AI choosed move.
        /// </summary>
        /// <param name="coordinate">Where to draw the AI's symbol.</param>
        /// <param name="symbol">Which symbol the AI is with.</param>
        private void DrawAiMove(Coordinate coordinate, Char symbol) {
            if (symbol == 'X')
            {
                DrawX(coordinate);
            }
            else
            {
                DrawCirle(coordinate);
            }
        }

        /// <summary>
        /// Calls every method, to execute the player's move.
        /// </summary>
        /// <returns>True, if it was a valid move, False if it wasn't(no changes aplied on the game either)</returns>
        private bool PlayerMove() {
            if (game.Field.AddMove(GetMousePosition(), game.CurrentPlayer.symbol))
            {
                DrawPlayerClick(game.CurrentPlayer);
                game.NextPlayer();
                return true;
            }
            return false;
        }

        /// <summary>
        /// Calls every method to execute the AI's move.
        /// </summary>
        /// <param name="ai">The AI we use as a player</param>
        private void AiMove(Ai ai) {
            /*Coordinate move = ai.GetMoveByLogicType((Game)game);
            game.Field.AddMove(move, ai.symbol);*/
            DrawAiMove(ai.Act((Game)game), ai.symbol);
            game.NextPlayer();
        }

        /// <summary>
        /// Execute every necessary action, after the game is at end state.
        /// </summary>
        public void PostWinCondition() {
            game.EndGame();
            if (game.GameType != GameType.AIvAI)
            {
                if (game.Field.FieldState == FieldState.NoSpaceLeft)
                {
                    MessageBox.Show("Unfortunately, this is a tie.");
                }
                else
                {
                    MessageBox.Show($"Congratulation {game.Winner.name}, you win!");
                }
            }
            try
            {
                DatabaseCommands database = new DatabaseCommands();
                database.AddEntry((Game)game);
            }
            catch (Exception)
            {
                MessageBox.Show("There is no Database in this device!");
            }
            
        }

        /// <summary>
        /// This method is called, when the user clicked the canvas.
        /// Calls every method to make the correct actions based on the <see cref="GameType"/>.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                
                return;
            }

            if (game.GameType == GameType.PvAI)
            {
                if (PlayerMove())
                {
                    if (game.isEnded())
                    {
                        PostWinCondition();
                        return;
                    }
                    else
                    {
                        AiMove(ai2);
                        if (game.isEnded())
                        {
                            PostWinCondition();
                            return;
                        }
                    }
                }
                return;
            }
            if (game.GameType == GameType.AIvAI) {
                LetAiPlay();
            }
        }

        /// <summary>
        /// Makes 2 AI playe with eachother <see cref="AiIteration"/> times.
        /// </summary>
        private void LetAiPlay()
        {
            int ai1Win = 0;
            int ai2Win = 0;
            int tie = 0;
            List<Game> games = new List<Game>();
            for (int i = 0; i < AiIteration; i++)
            {
                bool isAi1 = true;
                while (!game.isEnded())
                {
                    if (isAi1)
                    {
                        AiMove(ai1);
                    }
                    else
                    {
                        AiMove(ai2);
                    }

                    isAi1 = !isAi1;
                    if (game.isEnded())
                    {
                        break;
                    }
                }
                game.EndGame();
                games.Add((Game)game.Clone());
                if (game.Winner == ai1)
                {
                    ai1Win++;
                }
                else if(game.Winner == ai2)
                {
                    ai2Win++;
                }
                else
                {
                    tie++;
                }
                game.Restart();
                DrawNewField();
            }
            MessageBox.Show($"{game.Players[0].name} has won {ai1Win}/{AiIteration}" + '\n' +
                            $"{game.Players[1].name} has won {ai2Win}/{AiIteration}" + '\n' +
                            $"Played ties: {tie}",
                            "Summary of simulated matches");
            PostGames(games);
        }

        /// <summary>
        /// Post a <see cref="List{Game}"/> into the database at once.
        /// </summary>
        /// <param name="games">List of <see cref="Game"/>s</param>
        private void PostGames(List<Game> games)
        {
            try
            {
                DatabaseCommands database = new DatabaseCommands();
                foreach (var game in games)
                {
                    database.AddEntry(game);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("There is no Database!");
            }
        }

        /// <summary>
        /// Initialize everything to start a new <see cref="Game"/>.
        /// </summary>
        /// <param name="sender">The interacted object.</param>
        /// <param name="e">Data of the mouse related event.</param>
        private void NewGame(object sender, RoutedEventArgs e)
        {
            var newGameWindow = new NewGameWindow();
            newGameWindow.ShowDialog();
            DrawNewField();


            game = new Game();
            game.GameType = newGameWindow.gameType;
            if (game.GameType == GameType.PvP)
            {
                game.AddPlayer(new Player(newGameWindow.Player1Name, 'X'));
                game.AddPlayer(new Player(newGameWindow.Player2Name, 'O'));
            }

            if (game.GameType == GameType.PvAI)
            {
                game.AddPlayer(new Player(newGameWindow.Player1Name, 'X'));

                var aiOptionsWindow = new AiOptionsWindow();
                aiOptionsWindow.ShowDialog();

                if (aiOptionsWindow.aiLogicType == AiLogicType.Random)
                {
                    ai2 = new Ai("Player2(AI) - Random", 'O', AiLogicType.Random, game);
                    game.AddPlayer(ai2);
                }

                if (aiOptionsWindow.aiLogicType == AiLogicType.MinMax)
                {
                    ai2 = new Ai("Player2(AI) - MiniMax", 'O', AiLogicType.MinMax, game);
                    game.AddPlayer(ai2);
                }

                
            }

            if (game.GameType == GameType.AIvAI)
            {
                var aiVsAiOptionsWindow = new AiVsAiOptionsWindow();
                aiVsAiOptionsWindow.ShowDialog();

                // ai1 setup
                if (aiVsAiOptionsWindow.ai1LogicType == AiLogicType.Random)
                {
                    ai1 = new Ai("Player1(AI) - Random", 'X', AiLogicType.Random, game);
                    game.AddPlayer(ai1);
                }

                if (aiVsAiOptionsWindow.ai1LogicType == AiLogicType.MinMax)
                {
                    ai1 = new Ai("Player1(AI) - MiniMax", 'X', AiLogicType.MinMax, game);
                    game.AddPlayer(ai1);
                }

                // ai2 setup
                if (aiVsAiOptionsWindow.ai2LogicType == AiLogicType.Random)
                {
                    ai2 = new Ai("Player2(AI) - Random", 'O', AiLogicType.Random, game);
                    game.AddPlayer(ai2);
                }

                if (aiVsAiOptionsWindow.ai2LogicType == AiLogicType.MinMax)
                {
                    ai2 = new Ai("Player2(AI) - Minimax", 'O', AiLogicType.MinMax, game);
                    game.AddPlayer(ai2);
                }

            }

            game.GameType = newGameWindow.gameType;
            AiIteration = newGameWindow.XGames;

            game.Start();

            if (game.GameType == GameType.AIvAI)
            {
                MessageBox.Show("We are ready! \rPlease click on the playing field to start.");
            }
        }

        /// <summary>
        /// Opens a query window where we can access the stored information from the previously played game.
        /// </summary>
        /// <param name="sender">The interacted object.</param>
        /// <param name="e">Data of the mouse related event.</param>
        private void Query(object sender, RoutedEventArgs e)
        {
            var queryWindow = new QueryWindow();
            queryWindow.ShowDialog();
        }

    }
}
