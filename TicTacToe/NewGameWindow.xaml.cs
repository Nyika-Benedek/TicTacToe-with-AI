using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using TicTacToe.Interfaces;

namespace TicTacToe
{
    /// <summary>
    /// Interaction logic for NewGameWindow.xaml
    /// </summary>
    public partial class NewGameWindow : Window
    {
        public NewGameWindow()
        {
            InitializeComponent();
        }

        public string Player1Name { get; private set; }
        public string Player2Name { get; private set; }

        /// <summary>
        /// How many games should the 2 ai play continuously.
        /// </summary>
        public int XGames { get; private set; }
        public GameType gameType { get; private set; }

        /// <summary>
        /// Gather all data from this window before closing it.
        /// </summary>
        /// <param name="sender">The interacted object.</param>
        /// <param name="e">Data of the mouse related event.</param>
        private void GiveNames(object sender, RoutedEventArgs e)
        {
            var nameRegex = new Regex(@"[^a-zA-Z0-9\s]");
            var intRegex = new Regex(@"[^0-9\s]");
            if (nameRegex.IsMatch(Player1.Text) || string.IsNullOrWhiteSpace(Player1.Text))
            {
                MessageBox.Show("Player1 cannot be whitespace or contain special characters");
                return;
            }

            if (nameRegex.IsMatch(Player2.Text) || string.IsNullOrWhiteSpace(Player2.Text))
            {
                MessageBox.Show("Player2 cannot be whitespace or contain special characters");
                return;
            }

            if (AIvsAIButton.IsChecked == true && (intRegex.IsMatch(AutoRunXGames.Text) || string.IsNullOrWhiteSpace(AutoRunXGames.Text)))
            {
                MessageBox.Show("Please give a positive integer in auto run!");
                return;
            }

            if (PvsPButton.IsChecked == false && PvsAIButton.IsChecked == false && AIvsAIButton.IsChecked == false)
            {
                MessageBox.Show("Plase select a game mode");
                return;
            }

            Player1Name = Player1.Text;
            Player2Name = Player2.Text;
            if (PvsPButton.IsChecked == true)
            {
                gameType = GameType.PvP;
            }
            else if (PvsAIButton.IsChecked == true)
            {
                gameType = GameType.PvAI;
            }
            else
            {
                gameType = GameType.AIvAI;
            }

            try
            {
                XGames = int.Parse(AutoRunXGames.Text);
            }
            catch (FormatException)
            {

                XGames = 10;
            }
            Close();
        }

        /// <summary>
        /// Enable or disable the window's other elements to access the only necessary functions.
        /// </summary>
        /// <param name="sender">The interacted object.</param>
        /// <param name="e">Data of the mouse related event.</param>
        private void PlayerVsPlayer(object sender, RoutedEventArgs e)
        {
            Player1.IsEnabled = true;
            Player2.IsEnabled = true;

            PvsAIButton.IsChecked = false;
            AIvsAIButton.IsChecked = false;
        }

        /// <summary>
        /// Enable or disable the window's other elements to access the only necessary functions.
        /// </summary>
        /// <param name="sender">The interacted object.</param>
        /// <param name="e">Data of the mouse related event.</param>
        private void PlayerVsAI(object sender, RoutedEventArgs e)
        {
            Player1.IsEnabled = true;
            Player2.IsEnabled = false;
            Player2.Text = "Player2";

            AIvsAIButton.IsChecked = false;
            PvsPButton.IsChecked = false;
        }

        /// <summary>
        /// Enable or disable the window's other elements to access the only necessary functions.
        /// </summary>
        /// <param name="sender">The interacted object.</param>
        /// <param name="e">Data of the mouse related event.</param>
        private void AIVsAI(object sender, RoutedEventArgs e)
        {
            Player1.IsEnabled = false;
            Player1.Text = "Player1";
            Player2.IsEnabled = false;
            Player2.Text = "Player2";

            PvsAIButton.IsChecked = false;
            PvsPButton.IsChecked = false;
        }
    }
}
