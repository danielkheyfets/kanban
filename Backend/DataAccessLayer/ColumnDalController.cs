using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    class ColumnDalController : DalController
    {
        private const string ColumnTableName = "Columns";
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        public ColumnDalController() : base(ColumnTableName)
        {
        }

        protected override DalObject ToDalObject(SQLiteDataReader reader)
        {
            ColumnDL result = new ColumnDL(reader.GetString(0), reader.GetInt32(1), reader.GetString(2), reader.GetInt32(3), reader.GetInt32(4));
            return result;

        }
        ///<summary>This method returns a list of all the columns in the DB.</summary>
        public List<ColumnDL> SelectAllColumns()
        {
            List<ColumnDL> result = Select().Cast<ColumnDL>().ToList(); //cast to ColumnDL.
            return result;
        }

        public List<ColumnDL> SelectAllColumnsByUser(string email)
        {
            List<ColumnDL> result = SelectByKey(email).Cast<ColumnDL>().ToList(); //cast to ColumnDL.
            return result;
        }

        ///<summary>This method saves a new column in the DB.</summary>

        public bool Insert(ColumnDL column)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command = new SQLiteCommand(null, connection);
                int res = -1;
                try
                {
                    connection.Open();
                    command.CommandText = $"INSERT INTO {ColumnTableName} ({DalObject.EmailColumnName} ,{ColumnDL.ColumnOrdinalColumnName} ,{ColumnDL.ColumnNameColumnName} ,{ColumnDL.MaxValueColumnName} ,{ColumnDL.CurrNumTasksColumnName}) " +
                        $"VALUES (@emailC,@OrdinalC,@NameC,@MaxValueC,@CurrNumC);";
                    SQLiteParameter email = new SQLiteParameter(@"emailC", column.Email);
                    SQLiteParameter Ordinal = new SQLiteParameter(@"OrdinalC", column.ColumnOrdinal);
                    SQLiteParameter Name = new SQLiteParameter(@"NameC", column.ColumnName);
                    SQLiteParameter MaxValue = new SQLiteParameter(@"MaxValueC", column.MaxTasks);
                    SQLiteParameter CurrNum = new SQLiteParameter(@"CurrNumC", column.CurrNOTasks);

                    command.Parameters.Add(email);
                    command.Parameters.Add(Ordinal);
                    command.Parameters.Add(Name);
                    command.Parameters.Add(MaxValue);
                    command.Parameters.Add(CurrNum);

                    command.Prepare();

                    res = command.ExecuteNonQuery();
                }
                catch (SQLiteException ex)
                {
                    if (ex.ErrorCode != 19)
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
