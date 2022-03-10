using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicTacToe.Interfaces;
using TicTacToe.Model;

namespace TicTacToe.Models
{
    public class Field : IField
    {
        public FieldState FieldState { get; private set; } = FieldState.ThereIsEmptySpace;
        public Char[,] FieldMap { get; private set; } = new Char[3,3];

        public void AddMove(Coordinate coordinate, Char symbol) {
            this.FieldMap[coordinate.X, coordinate.Y] = symbol;
        }
        public Field()
        {
            Char[,] FieldMap = new char[3, 3] { { ' ', ' ', ' ' }, { ' ', ' ', ' ' }, { ' ', ' ', ' ' }};
        }

        /// <summary>
        /// Build a Field by a given map.
        /// </summary>
        /// <param name="map"><see cref="String"/> map of the given field</param>
        public Field(String map)
        {
            if (map.Length != 9)
            {
                throw new InvalidMapException();
            }
            else
            {
                Char[,] FieldMap = new char[3, 3] { { ' ', ' ', ' ' }, { ' ', ' ', ' ' }, { ' ', ' ', ' ' } };
                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        FieldMap[i, j] = map[i + j];
                    }
                }
            }
        }

        public bool isEmptySpaceLeft() {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (FieldMap[i, j] == '\0')
                    {
                        return true;
                    }
                }
            }
            return false;
        }


        public void UpdateFieldState() {
            for (int i = 0; i < 3; i++)
            {
                // check rows
                if (FieldMap[i, 0] == FieldMap[i, 1] && FieldMap[i, 1] == FieldMap[i, 2] && FieldMap[i, 1] != '\0')
                {
                    this.FieldState = FieldState.ThereIsAMatch;
                    return;
                }

                // check collumns
                if (FieldMap[0, i] == FieldMap[1, i] && FieldMap[1, i] == FieldMap[2, i] && FieldMap[1, i] != '\0')
                {
                    this.FieldState = FieldState.ThereIsAMatch;
                    return;
                }
            }

            // check diagonals
            if (FieldMap[0, 0] == FieldMap[1, 1] && FieldMap[1, 1] == FieldMap[2, 2] && FieldMap[1, 1] != '\0')
            {
                this.FieldState = FieldState.ThereIsAMatch;
                return;
            }

            if (FieldMap[2, 0] == FieldMap[1, 1] && FieldMap[1, 1] == FieldMap[0, 2] && FieldMap[1, 1] != '\0')
            {
                this.FieldState = FieldState.ThereIsAMatch;
                return;
            }

            if (isEmptySpaceLeft())
            {
                this.FieldState = FieldState.ThereIsEmptySpace;
                return;
            }

            this.FieldState = FieldState.NoSpaceLeft;
        }
    }

    [Serializable]
    public class InvalidMapException : Exception
    {
        public InvalidMapException()
        { }

        public InvalidMapException(string message)
            : base(message)
        { }

        public InvalidMapException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
