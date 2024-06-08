using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    public class BoardDalController : DalController
    {
        private const string BoardTableName = "Boards";
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        public BoardDalController() : base(BoardTableName)
        {

        }
        ///<summary>This method returns a list of all the boards in the DB.</summary>
        public List<BoardDL> SelectAllBoards()
        {
            List<BoardDL> result = Select().Cast<BoardDL>().ToList();
            return result;
        }

        protected override DalObject ToDalObject(SQLiteDataReader reader)
        {
            BoardDL result = new BoardDL(reader.GetString(0), Int32.Parse(reader.GetString(1)));
            return result;

        }

        ///<summary>This method saves a new board in the DB.</summary>

        public bool Insert(BoardDL board)
        {

            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command = new SQLiteCommand(null, connection);
                int res = -1;
                try
                {
                    connection.Open();
                    command.CommandText = $"INSERT INTO {BoardTableName} ({BoardDL.EmailColumnName} ,{BoardDL.NextTaskID}) " +
                        $"VALUES (@emailC,@nextId);";
                    SQLiteParameter email = new SQLiteParameter(@"emailC", board.Email);
                    SQLiteParameter taskid = new SQLiteParameter(@"nextId", board.NextID);
                    command.Parameters.Add(email);
                    command.Parameters.Add(taskid);
                    command.Prepare();
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
                return res > 0;
            }
        }
    }
}
