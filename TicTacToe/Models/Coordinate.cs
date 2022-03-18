using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToe.Model
{
    public class Coordinate
    {

        public int X { get; set; }

        public int Y { get; set; }

        public Coordinate(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Coordinate);
        }

        public bool Equals(Coordinate other)
        {
            return !(other is null)
                && this.X == other.X
                && this.Y == other.Y;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(this.X, this.Y);
        }

        public static bool operator ==(Coordinate left, Coordinate right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Coordinate left, Coordinate right)
        {
            return !(left == right);
        }
        public override string ToString()
        {
            return "X" + this.X + "Y" + this.Y;
        }

        /// <summary>
        /// If the coordinate's each value is [0, 2]
        /// </summary>
        /// <returns>True, if its within the interval, false otherwise.</returns>
        public bool IsValid() {
            bool inclusive = false;
            return inclusive
                    ?   0 <= this.X && this.X <= 2 &&
                        0 <= this.Y && this.Y <= 2
                    : true;
        }
    }
}
