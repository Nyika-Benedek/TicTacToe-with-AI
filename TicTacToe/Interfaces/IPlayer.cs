using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToe.Interfaces
{
    interface IPlayer
    {
        public String name { get; }

        public char symbol { get; }

    }
}
