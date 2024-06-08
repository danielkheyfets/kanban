using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    public abstract class DalController
    {
        protected readonly string _connectionString;
        private readonly string _tableName;
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        public DalController(string tableName)
        {
            string path = Path.GetFullPath(Path.Combine(Path.GetFullPath(Directory.GetCurrentDirectory()), "Kanban.db"));
            this._connectionString = $"Data Source={path}; Version=3;";
            if (!File.Exists(Directory.GetCurrentDirectory() + @"\Kanban.db"))
            {
                System.Data.SQLite.SQLiteConnection.CreateFile("Kanban.db");
                createDB();
            }
            this._tableName = tableName;
        }
        public void createDB()
        {

            string createTableQuery = @"CREATE TABLE if not EXISTS [Columns] ([Email] TEXT NOT NULL,[ID] INTEGER NOT NULL,[ColumnName] TEXT
             NOT NULL,[MaxValue] INTEGER NOT NULL,[CurrNumOfTasks] INTEGER NOT NULL,PRIMARY KEY([Email],[ID]));  CREATE TABLE if not EXISTS [Boards] ([Email]	TEXT NOT NULL,
	        [NextTaskId]	TEXT NOT NULL, PRIMARY KEY([Email]));CREATE TABLE if not EXISTS [Tasks] ([Email]	TEXT NOT NULL , [ID]	INTEGER NOT NULL, [Title]	TEXT NOT NULL, [Description]	TEXT,[ColumnOrdinal]	INTEGER NOT NULL,
	          [CreationDate]	TEXT NOT NULL, [DueDate]	TEXT NOT NULL, [HostEmail]	    TEXT NOT NULL, PRIMARY KEY([Email],[ID])); CREATE  TABLE if not EXISTS [Users] (
               [Email]	TEXT NOT NULL UNIQUE, [Password]	TEXT NOT NULL, [Nickname]	TEXT NOT NULL, 	[AssigneeEmail]	TEXT NOT NULL, PRIMARY KEY([Email]));";

            // Create the file which will be hosting our database
            using (System.Data.SQLite.SQLiteConnection con = new System.Data.SQLite.SQLiteConnection(_connectionString))
            {
                using (System.Data.SQLite.SQLiteCommand com = new System.Data.SQLite.SQLiteCommand(con))
                {
                    try
                    {
                        con.Open();                             // Open the connection to the database
                        com.CommandText = createTableQuery;     // Set CommandText to our query that will create the table
                        com.ExecuteNonQuery();                  // Execute the query

                    }
                    catch (SQLiteException ex)
                    {
                        log.Info(ex.Message);
                    }
                    finally
                    {
                        con.Close();        // Close the connection to the database

                    }
                }
            }
        }
        ///<summary>This method returns a list that contains all the Objects from a specific table in the DB.</summary>
        protected List<DalObject> Select()
        {
            List<DalObject> results = new List<DalObject>();
            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command = new SQLiteCommand(null, connection);
                command.CommandText = $"SELECT * FROM {_tableName};";
                SQLiteDataReader dataReader = null;
                try
                {
                    connection.Open();
                    dataReader = command.ExecuteReader();

                    while (dataReader.Read())
                    {
                        results.Add(ToDalObject(dataReader));

                    }
                }
                catch (SQLiteException ex)
                {
                    log.Info(ex.Message);
                }
                finally
                {
                    if (dataReader != null)
                    {
                        dataReader.Close();
                    }

                    command.Dispose();
                    connection.Close();
                }
            }
            return results;
        }









        ///<summary>This method returns a list that contains all the objects from a specific table by email.</summary>
        protected List<DalObject> SelectByKey(string email)
        {
            List<DalObject> results = new List<DalObject>();
            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command = new SQLiteCommand(null, connection);
                command.CommandText = $"SELECT * FROM {_tableName} WHERE Email=@email;"; // check in tests
                SQLiteDataReader dataReader = null;
                try
                {
                    command.Parameters.Add(new SQLiteParameter(@"email", email));
                    connection.Open();
                    dataReader = command.ExecuteReader();

                    while (dataReader.Read())
                    {
                        results.Add(ToDalObject(dataReader));

                    }
                }
                catch (SQLiteException ex)
                {
                    log.Info(ex.Message);
                }
                finally
                {
                    if (dataReader != null)
                    {
                        dataReader.Close();
                    }

                    command.Dispose();
                    connection.Close();
                }
            }
            return results;
        }

        ///<summary>This method returns a list that contains all the objects from a specific table by email and ID.</summary>
        protected List<DalObject> SelectByKey(string email, int ColumnOrdinaL)
        {
            List<DalObject> results = new List<DalObject>();
            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command = new SQLiteCommand(null, connection);
                command.CommandText = $"SELECT * FROM {_tableName} WHERE HostEmail=@email AND ColumnOrdinal=@ColumnOrdinaL ;"; 
                SQLiteDataReader dataReader = null;
                try
                {
                    command.Parameters.Add(new SQLiteParameter(@"email", email));
                    command.Parameters.Add(new SQLiteParameter(@"ColumnOrdinaL", ColumnOrdinaL));
                    connection.Open();
                    dataReader = command.ExecuteReader();

                    while (dataReader.Read())
                    {
                        results.Add(ToDalObject(dataReader));

                    }
                }
                catch (SQLiteException ex)
                {
                    log.Info(ex.Message);
                }
                finally
                {
                    if (dataReader != null)
                    {
                        dataReader.Close();
                    }

                    command.Dispose();
                    connection.Close();
                }
            }
            return results;
        }


        //Delete all persisted data
        public bool DeleteData()
        {
            int res = -1;

            using (var connection = new SQLiteConnection(_connectionString))
            {
                var command = new SQLiteCommand
                {

                    Connection = connection,
                    CommandText = $"DELETE FROM {_tableName}"
                };
                try
                {
                    connection.Open();
                    res = command.ExecuteNonQuery();
                }
                catch (SQLiteException ex)
                {
                    log.Info(ex.Message);
                }
                finally
                {
                    command.Dispose();
                    connection.Close();
                }

            }
            return res > 0;
        }

        ///<summary>This method deletes a specific Task or Column from the Tables by email and ID.</summary>

        public bool Delete(string email, int ID)// delete by id and email
        {

            int res = -1;
            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command = new SQLiteCommand(null, connection);
                try
                {
                    connection.Open();
                    command.CommandText = $"DELETE FROM {_tableName} WHERE ID=@ID AND Email=@email;";
                    command.Parameters.Add(new SQLiteParameter(@"ID", ID));
                    command.Parameters.Add(new SQLiteParameter(@"Email", email));
                    res = command.ExecuteNonQuery();
                }
                catch (SQLiteException ex)
                {
                    log.Info(ex.Message);
                }
                finally
                {
                    command.Dispose();
                    connection.Close();
                }
            }
            return res > 0;
        }




        ///<summary>This method updates the Task or Column information in the DB.</summary>

        public bool Update(string email, int ID, string wereToChange, string value)//update for column and task
        {
            int res = -1;
            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command = new SQLiteCommand(null, connection);

                try
                {
                    connection.Open();
                    command.CommandText = $"UPDATE {_tableName} SET {wereToChange}=@value WHERE (ID=@IDover AND Email=@email)";
                    command.Parameters.Add(new SQLiteParameter(@"value", value));
                    command.Parameters.Add(new SQLiteParameter(@"IDover", ID));
                    command.Parameters.Add(new SQLiteParameter(@"email", email));

                    res = command.ExecuteNonQuery();
                }
                catch (SQLiteException ex)
                {
                    log.Info(ex.Message);
                }
                finally
                {
                    command.Dispose();
                    connection.Close();
                }
            }
            return res > 0;
        }

        ///<summary>This method updates the board information in the DB.</summary>

        public bool Update(string email, string wereToChange, string value)//update boards
        {
            int res = -1;
            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command = new SQLiteCommand(null, connection);

                try
                {
                    connection.Open();
                    command.CommandText = $"UPDATE {_tableName} SET {wereToChange}=@{wereToChange} WHERE (Email=@email);";
                    command.Parameters.Add(new SQLiteParameter(wereToChange, value));
                    command.Parameters.Add(new SQLiteParameter(@"Email", email));
                    res = command.ExecuteNonQuery();
                }
                catch (SQLiteException ex)
                {
                    log.Info(ex.Message);
                }
            }
            return res > 0;
        }


        ///<summary>This method updates the board information in the DB.</summary>

        public bool Update(string email, string ID, string wereToChange, int value)//update for column 
        {
            int res = -1;
            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command = new SQLiteCommand(null, connection);

                try
                {
                    connection.Open();
                    command.CommandText = $"UPDATE {_tableName} SET {wereToChange}=@{wereToChange} WHERE (ID=@ID AND Email=@email)";
                    command.Parameters.Add(new SQLiteParameter(wereToChange, value));
                    command.Parameters.Add(new SQLiteParameter(@"ID", ID));
                    command.Parameters.Add(new SQLiteParameter(@"Email", email));
                    res = command.ExecuteNonQuery();
                }
                catch (SQLiteException ex)
                {
                    log.Info(ex.Message);
                }
                finally
                {
                    command.Dispose();
                    connection.Close();
                }
            }
            return res > 0;
        }




        protected abstract DalObject ToDalObject(SQLiteDataReader reader);

    }
}