using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicTacToe.Model;

namespace TicTacToe.Interfaces
{
    /// <summary>
    /// Represents the 3 state type.
    /// </summary>
    public enum FieldState { ThereIsEmptySpace, NoSpaceLeft, ThereIsAMatch }

    /// <summary>
    /// Collection of Field class attributes.
    /// </summary>
    public interface IField : ICloneable
    {
        public FieldState FieldState { get; }
        public Char[,] FieldMap { get; }

        /// <summary>
        /// Add a move to the filed.
        /// </summary>
        /// <param name="coordinate">choosed <see cref="Coordinate"/></param>
        /// <param name="symbol">symbol of choice</param>
        /// <returns>True, if it was successful, false otherwise</returns>
        public abstract bool AddMove(Coordinate coordinate, char symbol);

        /// <summary>
        /// Updates the <see cref="IField.FieldState"/>
        /// </summary>
        public abstract void UpdateFieldState();

        /// <summary>
        /// Checks if is there any empty space left.
        /// </summary>
        /// <returns>True, if there is any empty space left, false otherwise</returns>
        public abstract bool IsEmptySpaceLeft();

        /// <summary>
        /// Checks if the given coordinate is empty.
        /// </summary>
        /// <param name="coordinate">Coordinate to check</param>
        /// <returns>True, if its empty, false otherwise</returns>
        public abstract bool IsCellEmpty(Coordinate coordinate);
    }
}