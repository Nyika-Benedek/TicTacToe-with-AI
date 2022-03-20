using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToe.Interfaces
{
    /// <summary>
    /// This enum class represents the state of the game.
    /// </summary>
    public enum GameState { None, OnGoing, Finneshed }

    /// <summary>
    /// This class represents the type of the game.
    /// </summary>
    public enum GameType { PvP, PvAI, AIvAI }

    /// <summary>
    /// This interface collect all the atribute, which a game should have.
    /// </summary>
    interface IGame : ICloneable
    {
        public GameState GameState { get; set; }
        public GameType GameType { get; set; }
        public List<IPlayer> Players { get; }
        public IPlayer CurrentPlayer { get; }

        public IField Field { get; }

        public IPlayer Winner { get; set; }

        public int Turn { get; }

        /// <summary>
        /// Start the inicialized game.
        /// </summary>
        abstract void Start();

        /// <summary>
        /// Checks if the current game is at it's end.
        /// </summary>
        /// <returns>True, if ended, false otherwise</returns>
        abstract bool isEnded();

        /// <summary>
        /// Add a new player to the game.
        /// </summary>
        /// <param name="player">player name</param>
        public abstract void AddPlayer(IPlayer player);

        /// <summary>
        /// Gives the controll to the next player.
        /// </summary>
        /// <returns>The next player</returns>
        public abstract IPlayer NextPlayer();

        /// <summary>
        /// Close the game progress.
        /// </summary>
        public abstract void EndGame();

        /// <summary>
        /// Resets some of the atributes, if the AIs would play more games in a row.
        /// </summary>
        void Restart();
    }
}
