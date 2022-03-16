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
        public Game gameToSimulate { get; private set; }
        public int helpedPlayer { get; private set; }
        public AiLogicType LogicType { get; private set; }


        public Ai(string name, char symbol, AiLogicType logicType, IGame game) :base(name, symbol) {
            this.LogicType = logicType;
            this.helpedPlayer = game.Players.Count-1;
            this.gameToSimulate = (Game)game;
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
                return MinMax(game).Item1;
            }
            else
            {
                return new Coordinate(-1, -1);
            }
        }

        public (Coordinate, int) MinMax(Game game, int score = 0)
        {
            Coordinate bestMove = new Coordinate(-1, -1);

            game.Field.UpdateFieldState();
            if (game.Field.FieldState == Interfaces.FieldState.ThereIsAMatch)
            {
                game.NextPlayer();
                game.EndGame();
                // If it wins
                if (game.Winner == game.Players[helpedPlayer])
                {
                    return (bestMove, 100);
                }
                else
                {
                    return (bestMove, -100);
                }                
            }

            if (game.Field.FieldState == Interfaces.FieldState.NoSpaceLeft)
            {
                // If tie
                return (bestMove, 0);
            }

            int bestScore = int.MinValue;
            // createing possible fields
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    Coordinate coordinate = new Coordinate(i, j);
                    if (game.Field.IsCellEmpty(coordinate))
                    {
                        game.Field.AddMove(coordinate, game.CurrentPlayer.symbol);
                        game.NextPlayer();

                        (Coordinate, int) child = MinMax(game, --score);
                        if (bestScore < child.Item2)
                        {
                            bestMove = child.Item1;
                            bestScore = child.Item2;
                        }
                    }
                }
            }
            return (bestMove, bestScore);
        }

        public void Act(Game game) {
            game.Field.AddMove(GetMoveByLogicType(game), this.symbol);
        }
    }
}
