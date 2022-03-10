using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicTacToe.Interfaces;

namespace TicTacToe.Models
{
    class Game : IGame
    {
        public List<IPlayer> Players { get; private set; } = new List<IPlayer>(2);

        public IPlayer CurrentPlayer { get; private set; }

        public IField Field { get; private set; }

        public IPlayer Winner { get; set; }

        public int Turn { get; private set; } = 0;
        public GameState GameState { get; set; } = GameState.None;
        public GameType GameType { get; set; }
        public FieldState FieldState { get; set; } = FieldState.ThereIsEmptySpace;

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
            this.GameState = GameState.Finneshed;
            Winner = this.CurrentPlayer;
        }

        public IPlayer NextPlayer()
        {
            playerIndex++;
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
            GameState = GameState.OnGoing;
            this.CurrentPlayer = Players[0];
            Field = new Field();
            Winner = null;
        }
    }
}
