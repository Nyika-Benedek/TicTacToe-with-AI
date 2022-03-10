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
        public List<IPlayer> Players { get; private set; }

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
            Char matchedChar = Field.Match();
            if (matchedChar == ' ')
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public IPlayer NextPlayer()
        {
            playerIndex++;
            return CurrentPlayer = Players[playerIndex % 2];
        }

        public void Start()
        {
            GameState = GameState.OnGoing;

            Field = new Field();
        }
    }
}
