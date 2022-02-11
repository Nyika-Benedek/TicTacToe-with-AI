using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToe.Interfaces
{
    interface IField
    {
        public Char[,] FieldMap { get; }

        public abstract Char Match();
    }
}
