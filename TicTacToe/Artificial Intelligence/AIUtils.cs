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
        /// <summary>
        /// Generate a random Coordinate.
        /// </summary>
        /// <returns>A random <see cref="Coordinate"/></returns>
        public static Coordinate GetRandomCoordinate() {
            Random random = new Random();

            return new Coordinate(random.Next(0, 3), random.Next(0, 3));
        }

        /// <summary>
        /// Give a list of possible move(only direct moves), it will chose the worst(like it was the enemy)
        /// </summary>
        /// <param name="childs"><see cref="List{(Coordinate, int)}"/></param>
        /// <returns>Worst move <see cref="List{(Coordinate, int)}"/></returns>
        public static (Coordinate, int) GetMinOfChilds(List<(Coordinate, int)> childs) {
            if (childs.Count == 0)
            {
                throw new NullReferenceException("There is no element ins childs!");
            }

            if (childs.Count == 1)
            {
                return childs[0];
            }

            int min = 0;
            for (int i = 1; i < childs.Count; i++)
            {
                if (childs[min].Item2 > childs[i].Item2)
                {
                    min = i;
                }
            }
            return childs[min];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="childs"></param>
        /// <returns></returns>
        public static (Coordinate, int) GetMaxOfChilds(List<(Coordinate, int)> childs)
        {
            if (childs.Count == 0)
            {
                throw new NullReferenceException("There is no element ins childs!");
            }

            if (childs.Count == 1)
            {
                return childs[0];
            }

            int max = 0;
            for (int i = 1; i < childs.Count; i++)
            {
                if (childs[max].Item2 < childs[i].Item2)
                {
                    max = i;
                }
            }
            return childs[max];
        }
    }
}
