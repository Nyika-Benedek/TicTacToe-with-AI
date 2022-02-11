using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToe.Entity
{
    class DatabaseCommands
    {
        DatabaseContextFactory databaseFactory = new DatabaseContextFactory();

        public void AddEntry() {
            using var connection = databaseFactory.CreateDbContext(new string[0]); {
                connection.database.Add(new DatabaseStructure());
                connection.SaveChanges();
            }
        }

        public List<DatabaseStructure> GetAll() {
            using var connection = databaseFactory.CreateDbContext(new string[0]);
            {
                return connection.database.ToList();
            }
        }

        public void DeleteById(int id) {
            using var connection = databaseFactory.CreateDbContext(new string[0]);
            {
                var entry = connection.database.Find(id);
                if (!(entry is null))
                {
                    connection.Remove(connection.database.Find(id));
                    connection.SaveChanges();
                }
            }
        }

        public void UpdateById(int id) {
            using var connection = databaseFactory.CreateDbContext(new string[0]);
            {
                var entry = connection.database.Find(id);
                entry.date = DateTime.Now;

                connection.Update(entry);
                connection.SaveChanges();
            }
        }
    }
}
