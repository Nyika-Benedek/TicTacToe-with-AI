using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicTacToe.Interfaces;

namespace TicTacToe.Models
{
    /// <summary>
    /// Thi class represents the game as an object.
    /// </summary>
    public class Game : IGame
    {
        public List<IPlayer> Players { get; private set; } = new List<IPlayer>(2);

        public IPlayer CurrentPlayer { get; private set; }

        public IField Field { get; private set; }

        public IPlayer Winner { get; set; }

        public int Turn { get; private set; } = 0;
        public GameState GameState { get; set; } = GameState.None;
        public GameType GameType { get; set; }

        private int playerIndex = -1;

        public void AddPlayer(IPlayer player)
        {
            if (Players.Count > 2)
            {
                throw new OverflowException();
            }
            Players.Add(player);
        }

        public bool isEnded()
        {
            Field.UpdateFieldState();
            if (Field.FieldState == FieldState.ThereIsAMatch || Field.FieldState == FieldState.NoSpaceLeft)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void EndGame() {
            // set back turn and player to the correct value
            this.NextPlayer();
            Turn -= 2;

            this.GameState = GameState.Finneshed;
            if (this.Field.FieldState == FieldState.ThereIsAMatch)
            {
                Winner = this.CurrentPlayer;
            }
            else
            {
                Winner = null;
            }
        }

        public IPlayer NextPlayer()
        {
            playerIndex++;
            this.Turn++;
            return CurrentPlayer = Players[playerIndex % 2];
        }

        public void Start()
        {
            GameState = GameState.OnGoing;
            this.NextPlayer();
            Field = new Field();
        }

        public void Restart()
        {
            this.Turn = 0;
            GameState = GameState.OnGoing;
            playerIndex = 0;
            this.CurrentPlayer = Players[0];
            Field = new Field();
            Winner = null;
        }

        /// <summary>
        /// Make a deepcopy of itself.
        /// </summary>
        /// <returns>Deep copy of this object</returns>
        public object Clone()
        {
            Game clone = new Game();

            foreach (var player in this.Players)
            {
                clone.AddPlayer(player);
            }

            clone.CurrentPlayer = this.CurrentPlayer;
            clone.Field = (Field)this.Field.Clone();
            clone.Winner = this.Winner;
            clone.Turn = this.Turn;
            clone.GameState = this.GameState;
            clone.GameType = this.GameType;
            clone.playerIndex = this.playerIndex;

            return clone;
        }
    }
}
