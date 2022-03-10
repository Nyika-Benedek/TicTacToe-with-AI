using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicTacToe.Model;

namespace TicTacToe.Interfaces
{
    public enum FieldState { ThereIsEmptySpace, NoSpaceLeft, ThereIsAMatch }
    interface IField
    {
        public FieldState FieldState { get; }
        public Char[,] FieldMap { get; }
        public abstract void AddMove(Coordinate coordinate, char symbol);
        public abstract void UpdateFieldState();
        public abstract bool isEmptySpaceLeft();
    }
}
