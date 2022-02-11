using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicTacToe.Interfaces;

namespace TicTacToe.Models
{
    class Player : IPlayer
    {
        public string name { get; private set; }

        public Player(String name)
        {
            this.name = name;
        }
    }
}
