using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicTacToe.Model;
using TicTacToe.Models;

namespace TicTacToe.AI
{
    /// <summary>
    /// This class contains static methotds to help the AI.
    /// </summary>
    class AIUtils
    {
        public static Coordinate GetRandomCoordinate() {
            Random random = new Random();

            return new Coordinate(random.Next(0, 3), random.Next(0, 3));
        }

        
    }
}
