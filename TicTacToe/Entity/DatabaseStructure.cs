using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicTacToe.Models;

namespace TicTacToe.Entity
{
    public class DatabaseStructure
    {
        public int id { get; set; }
        public string player1 { get; set; }
        public string player2 { get; set; }
        public string winner { get; set; }
        public int turn { get; set; }
        public DateTime date { get; set; }

        public DatabaseStructure()
        {
            this.date = DateTime.Now;
        }

        public DatabaseStructure(Game game)
        {
            this.player1 = game.Players[0].name;
            this.player2 = game.Players[1].name;
            this.turn = game.Turn;
            this.date = DateTime.Now;

            this.winner = game.Winner is null ? "It was a tie." : game.Winner.name;
        }

    }
}
