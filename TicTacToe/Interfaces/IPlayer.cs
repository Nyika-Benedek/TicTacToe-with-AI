using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToe.Interfaces
{
    /// <summary>
    /// This interface contain all nescesarry atributes of <see cref="Player"/>
    /// </summary>
    public interface IPlayer
    {
        public String name { get; }

        public char symbol { get; }

    }
}
