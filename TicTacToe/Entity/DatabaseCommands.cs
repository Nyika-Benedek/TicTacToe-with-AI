using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicTacToe.Models;

namespace TicTacToe.Entity
{
    /// <summary>
    /// This class contains all the command, which can be used on the database.
    /// </summary>
    class DatabaseCommands
    {
        DatabaseContextFactory databaseFactory = new DatabaseContextFactory();

        /// <summary>
        /// Add new entry to the database.
        /// </summary>
        /// <param name="game"></param>
        public void AddEntry(Game game) {
            using var connection = databaseFactory.CreateDbContext(new string[0]); {
                connection.database.Add(new DatabaseStructure(game));
                connection.SaveChanges();
            }
        }

        /// <summary>
        /// Give the whole content of the dtbase.
        /// </summary>
        /// <returns>every entries in <see cref="List{DatabaseStructure}"/></returns>
        public List<DatabaseStructure> GetAll() {
            using var connection = databaseFactory.CreateDbContext(new string[0]);
            {
                return connection.database.ToList();
            }
        }

        /// <summary>
        /// Delete a single entry from the database by ID.
        /// </summary>
        /// <param name="id">The ID of the requested entry</param>
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

        /// <summary>
        /// Update an entry by ID.
        /// </summary>
        /// <param name="id">The ID of the requested entry</param>
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
