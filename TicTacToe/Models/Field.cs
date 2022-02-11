using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicTacToe.Interfaces;

namespace TicTacToe.Models
{
    public class Field : IField
    {
        public Char[,] FieldMap { get; private set; }


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

        /// <summary>
        /// if there are 3 symboles matched horisontally/vertically/diagonally, than returns the matched symbol.
        /// </summary>
        /// <returns>if there any, then return the matched symbol, ' ' otherwise.</returns>
        public Char Match() {
            for (int i = 0; i < 3; i++)
            {
                // check rows
                if (FieldMap[i, 0] == FieldMap[i, 1] && FieldMap[i, 1] == FieldMap[i, 2])
                {
                    return FieldMap[i, 0];
                }

                // check collumns
                if (FieldMap[0, i] == FieldMap[1, i] && FieldMap[1, i] == FieldMap[2, i])
                {
                    return FieldMap[0, i];
                }
            }

            // check diagonals
            if (FieldMap[0, 0] == FieldMap[1, 1] && FieldMap[1, 1] == FieldMap[2, 2])
            {
                return FieldMap[0, 0];
            }

            if (FieldMap[2, 0] == FieldMap[1, 1] && FieldMap[1, 1] == FieldMap[2, 0])
            {
                return FieldMap[2, 0];
            }
            return ' ';
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
