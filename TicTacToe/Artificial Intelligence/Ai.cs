using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicTacToe.Interfaces;
using TicTacToe.Model;
using TicTacToe.Models;

namespace TicTacToe.AI
{
    /// <summary>
    /// Which type of thinking of the AI is using.
    /// </summary>
    public enum AiLogicType { Random, MinMax };

    /// <summary>
    /// This class is represents the player which is extended with the AI's properties.
    /// </summary>
    class Ai : Player
    {
        /// <summary>
        /// The maximum steps it will think ahead.
        /// </summary>
        private static int MAX_DEPTH = 20;
        public int helpedPlayer { get; private set; }
        public AiLogicType LogicType { get; private set; }

        /// <summary>
        /// Constructor of AI based on player class.
        /// </summary>
        /// <param name="name">name of AI</param>
        /// <param name="symbol">symbol of AI</param>
        /// <param name="logicType">Logical thinking type of AI</param>
        /// <param name="game">The game this AI will thin ahead</param>
        public Ai(string name, char symbol, AiLogicType logicType, IGame game) :base(name, symbol) {
            this.LogicType = logicType;
            this.helpedPlayer = game.Players.Count;
        }

        /// <summary>
        /// Give a game into this and it calls the AI's correct behivaour to act.
        /// </summary>
        /// <param name="game">Game the move will be taken.</param>
        /// <returns>A <see cref="Coordinate"/> where the AI will act</returns>
        public Coordinate GetMoveByLogicType(Game game) {
            if (this.LogicType == AiLogicType.Random)
            {
                while (true)
                {
                    Coordinate coordinate = AIUtils.GetRandomCoordinate();
                    if (game.Field.IsCellEmpty(coordinate))
                    {
                        return coordinate;
                    }
                }
            }

            if (this.LogicType == AiLogicType.MinMax)
            {
                return MiniMax(game).Item1;
            }
            else
            {
                return new Coordinate(-1, -1);
            }
        }

        /// <summary>
        /// Calculates all possibility untill reach the depth limit by the MiniMax algorithm.
        /// </summary>
        /// <param name="game">Game to simulate</param>
        /// <param name="move">The move how did we get here from the parent null if it has no parent</param>
        /// <param name="score">The Score we got from the parent, 0 if it has no parent</param>
        /// <param name="isMaximising">Is it the maximising or minimising layer</param>
        /// <param name="depth">How deep we are currently</param>
        /// <returns></returns>
        public (Coordinate, int) MiniMax(Game game, Coordinate move = null, int score = 0, bool isMaximising = true, int depth = 0)
        {
            if (depth == MAX_DEPTH)
            {
                return (move, 0);
            }

            Game gameCopy = (Game)game.Clone();
            if (move is not null)
            {
                gameCopy.Field.AddMove(move, gameCopy.CurrentPlayer.symbol);
                gameCopy.NextPlayer();
            }

            gameCopy.Field.UpdateFieldState();
            if (gameCopy.Field.FieldState == Interfaces.FieldState.ThereIsAMatch)
            {
                gameCopy.EndGame();
                // If it wins
                if (gameCopy.Winner == gameCopy.Players[helpedPlayer])
                {
                    return (move, 100);
                }
                else
                {
                    return (move, -100);
                }
            }

            Coordinate coordinate = null;
            List<(Coordinate, int)> childs = new List<(Coordinate, int)>();
            bool thereIsSpaceLeft = false;
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    coordinate = new Coordinate(i, j);

                    if (gameCopy.Field.IsCellEmpty(coordinate))
                    {
                        thereIsSpaceLeft = true;
                        (Coordinate, int) child = MiniMax(game: gameCopy, move: coordinate, score: score-1, isMaximising: !isMaximising, depth: depth + 1);
                        child.Item2 += score;
                        childs.Add(child);
                        
                    }
                }
            }

            if (!thereIsSpaceLeft)
            {
                return (move, 0);
            }

            if (isMaximising)
            {
                (Coordinate, int) max = AIUtils.GetMaxOfChilds(childs);
                if (move is null)
                {
                    return max;
                }
                else
                {
                    return (move, max.Item2);
                }
            }
            else
            {
                (Coordinate, int) min = AIUtils.GetMinOfChilds(childs);
                if (move is null)
                {
                    return min;
                }
                else
                {
                    return (move, min.Item2);
                }
            }
        }

        /// <summary>
        /// Call the AI to act and aply its move.
        /// </summary>
        /// <param name="game">Game the AI plays</param>
        /// <returns>move of the AI</returns>
        public Coordinate Act(Game game) {
            Coordinate move = GetMoveByLogicType(game);
            game.Field.AddMove(move, this.symbol);
            return move;
        }
    }
}
