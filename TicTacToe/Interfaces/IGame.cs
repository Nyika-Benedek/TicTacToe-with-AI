using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToe.Interfaces
{
    public enum GameState { None, OnGoing, Finneshed }
    enum GameType { PvP, PvAI, AIvAI }
    interface IGame
    {
        public GameState GameState { get; set; }
        public GameType GameType { get; set; }
        public List<IPlayer> Players { get; }
        public IPlayer CurrentPlayer { get; }

        public IField Field { get; }

        public IPlayer Winner { get; set; }

        public int Turn { get; }


        abstract void Start();

        abstract bool isEnded();

        public abstract void AddPlayer(IPlayer player);

        public abstract IPlayer NextPlayer();

    }
}
