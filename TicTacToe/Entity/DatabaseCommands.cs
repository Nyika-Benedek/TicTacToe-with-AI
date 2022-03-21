using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
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
            //try
            //{
                using var connection = databaseFactory.CreateDbContext(new string[0]);
                {
                    connection.database.Add(new DatabaseStructure(game));
                    connection.SaveChanges();
                }
            //}
            //catch (Exception)
            //{
                //MessageBox.Show("There is no available database in this device!");
            //}
        }

        /// <summary>
        /// Give the whole content of the dtbase.
        /// </summary>
        /// <returns>every entries in <see cref="List{DatabaseStructure}"/></returns>
        public List<DatabaseStructure> GetAll() {
            try
            {
                using var connection = databaseFactory.CreateDbContext(new string[0]);
                {
                    return connection.database.ToList();
                }
            }
            catch (Exception)
            {
                MessageBox.Show("There is no available database in this device!");
            }
            return new List<DatabaseStructure>(0);
        }

        /// <summary>
        /// Delete a single entry from the database by ID.
        /// </summary>
        /// <param name="id">The ID of the requested entry</param>
        public void DeleteById(int id) {
            try
            {
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
            catch (Exception)
            {
                MessageBox.Show("There is no available database in this device!");
            }
        }

        /// <summary>
        /// Update an entry by ID.
        /// </summary>
        /// <param name="id">The ID of the requested entry</param>
        public void UpdateById(int id) {
            try
            {
                using var connection = databaseFactory.CreateDbContext(new string[0]);
                {
                    var entry = connection.database.Find(id);
                    entry.date = DateTime.Now;

                    connection.Update(entry);
                    connection.SaveChanges();
                }
            }
            catch (Exception)
            {
                MessageBox.Show("There is no available database in this device!");
            }
        }
    }
}
