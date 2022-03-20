using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicTacToe.Interfaces;

namespace TicTacToe.Models
{
    /// <summary>
    /// This class represents the Players as an object.
    /// </summary>
    public class Player : IPlayer
    {
        public string name { get; private set; }

        public Char symbol { get; private set; }

        public Player(String name, Char symbol)
        {
            this.name = name;
            this.symbol = symbol;
        }
    }
}
