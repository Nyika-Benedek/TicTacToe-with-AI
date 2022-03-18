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
    public enum AiLogicType { Random, MinMax };
    class Ai : Player
    {
        private static int MAX_DEPTH = 20;
        public int helpedPlayer { get; private set; }
        public AiLogicType LogicType { get; private set; }


        public Ai(string name, char symbol, AiLogicType logicType, IGame game) :base(name, symbol) {
            this.LogicType = logicType;
            this.helpedPlayer = game.Players.Count;
        }

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

        public Coordinate Act(Game game) {
            Coordinate move = GetMoveByLogicType(game);
            game.Field.AddMove(move, this.symbol);
            return move;
        }
    }
}
