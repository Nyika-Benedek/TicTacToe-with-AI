using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToe.Entity
{
    public class DatabaseStructure
    {
        public int id { get; set; }
        public DateTime date { get; set; }


        public DatabaseStructure()
        {
            this.date = DateTime.Now;
        }

    }
}
