using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicTacToe.Model;

namespace TicTacToe.Interfaces
{
    public enum FieldState { ThereIsEmptySpace, NoSpaceLeft, ThereIsAMatch }
    interface IField : ICloneable
    {
        public FieldState FieldState { get; }
        public Char[,] FieldMap { get; }
        public abstract bool AddMove(Coordinate coordinate, char symbol);
        public abstract void UpdateFieldState();
        public abstract bool IsEmptySpaceLeft();
        public abstract bool IsCellEmpty(Coordinate coordinate);
    }
}