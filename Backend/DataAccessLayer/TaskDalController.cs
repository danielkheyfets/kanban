using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    public class TaskDalController : DalController
    {
        private const string TaskTableName = "Tasks";
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        public TaskDalController() : base(TaskTableName)
        {

        }
        ///<summary>This method returns a list of all the tasks in the DB.</summary>
        public List<TaskDL> SelectAllMessages()
        {
            List<TaskDL> result = Select().Cast<TaskDL>().ToList(); //cast to TaskDL
            return result;
        }
        ///<summary>This method returns a list of all the tasks in the same column by email and column ordinal.</summary>
        public List<TaskDL> SelectAllTaskByKey(string email, int columnOrdinal)
        {

            List<TaskDL> result = SelectByKey(email, columnOrdinal).Cast<TaskDL>().ToList();
            return result;

        }



        ///<summary>This method saves a new task in the DB.</summary>
        public bool Insert(TaskDL task)
        {

            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command = new SQLiteCommand(null, connection);
                int res = -1;
                try
                {
                    connection.Open();
                    command.CommandText = $"INSERT INTO {TaskTableName} ({DalObject.EmailColumnName} ,{TaskDL.taskIDColumnName} ,{TaskDL.TitleColumnName} ,{TaskDL.DescColumnName} ,{TaskDL.ColumnOrdColumnName} ,{TaskDL.CreationDColumnName} ,{TaskDL.DueDColumnName}, {TaskDL.HostColumnName}) " +
                        $"VALUES (@emailC,@taskID,@titleC,@DescC,@ColOrd,@criationD,@Duedate,@host);";
                    SQLiteParameter email = new SQLiteParameter(@"emailC", task.Email);
                    SQLiteParameter taskid = new SQLiteParameter(@"taskID", task.TaskID);
                    SQLiteParameter title = new SQLiteParameter(@"titleC", task.Title);
                    SQLiteParameter description = new SQLiteParameter(@"DescC", task.Description);
                    SQLiteParameter Columnordinal = new SQLiteParameter(@"ColOrd", task.ColOrdinal);
                    SQLiteParameter criationDate = new SQLiteParameter(@"criationD", task.CreationD);
                    SQLiteParameter Duedate = new SQLiteParameter(@"Duedate", task.DueD);
                    SQLiteParameter hostEmail = new SQLiteParameter(@"host", task.BoardHost);
                    command.Parameters.Add(email);
                    command.Parameters.Add(taskid);
                    command.Parameters.Add(title);
                    command.Parameters.Add(description);
                    command.Parameters.Add(Columnordinal);
                    command.Parameters.Add(criationDate);
                    command.Parameters.Add(Duedate);
                    command.Parameters.Add(hostEmail);

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



        protected override DalObject ToDalObject(SQLiteDataReader reader) // convert to dall obj
        {
            if (!reader.IsDBNull(3))
                return new TaskDL(reader.GetString(0), reader.GetInt32(1), reader.GetString(2), reader.GetString(3), (int)reader.GetInt32(4), DateTime.Parse(reader.GetString(5)), DateTime.Parse(reader.GetString(6)), reader.GetString(7));
            return new TaskDL(reader.GetString(0), reader.GetInt32(1), reader.GetString(2), null, (int)reader.GetInt32(4), DateTime.Parse(reader.GetString(5)), DateTime.Parse(reader.GetString(6)), reader.GetString(7));

        }
    }
}




