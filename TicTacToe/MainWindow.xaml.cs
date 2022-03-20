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
        }

        /// <summary>
        /// How many AI vs AI games should compute at once
        /// </summary>
        private int AiIteration = 10;

        private IGame game;

        private const int CelldSize = 100;
        public enum LineType { GridLine, XLine }
        private Ai ai1;
        private Ai ai2;

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

        private bool PlayerMove() {
            if (game.Field.AddMove(GetMousePosition(), game.CurrentPlayer.symbol))
            {
                DrawPlayerClick(game.CurrentPlayer);
                game.NextPlayer();
                return true;
            }
            return false;
        }

        private void AiMove(Ai ai) {
            /*Coordinate move = ai.GetMoveByLogicType((Game)game);
            game.Field.AddMove(move, ai.symbol);*/
            DrawAiMove(ai.Act((Game)game), ai.symbol);
            game.NextPlayer();
        }

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
            // Save results to the database.
            DatabaseContextFactory databaseFactory = new DatabaseContextFactory();
            using var databaseContext = databaseFactory.CreateDbContext(new string[0]);

            DatabaseCommands database = new DatabaseCommands();
            database.AddEntry((Game)game);
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

        private void LetAiPlay()
        {
            int ai1Win = 0;
            int ai2Win = 0;
            int tie = 0;
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
                    //Thread.Sleep(200);
                    isAi1 = !isAi1;
                    if (game.isEnded())
                    {
                        break;
                    }
                }
                PostWinCondition();
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
                            $"Played ties: {tie}");
        }

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

        private void Query(object sender, RoutedEventArgs e)
        {
            var queryWindow = new QueryWindow();
            queryWindow.ShowDialog();
        }

    }
}
