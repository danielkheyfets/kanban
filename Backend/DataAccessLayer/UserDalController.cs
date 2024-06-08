using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    public class UserDalController : DalController
    {
        private const string UserTableName = "Users";
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        public UserDalController() : base(UserTableName)
        {

        }
        ///<summary>This method returns a list of all the users in the DB.</summary>
        public List<UserDl> SelectAllUsers()
        {
            List<UserDl> result = Select().Cast<UserDl>().ToList();
            return result;
        }
        ///<summary>This method saves a new user in the DB.</summary>
        public bool Insert(UserDl user)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                int res = -1;
                SQLiteCommand command = new SQLiteCommand(null, connection);
                try
                {
                    connection.Open();
                    command.CommandText = $"INSERT INTO {UserTableName} ({DalObject.EmailColumnName} ,{UserDl.PasswordColumnName} ,{UserDl.NicknameColumnName} ,{UserDl.HostColumnName}) " +
                        $"VALUES (@emailVal,@passVal,@nickVal,@hostemailVal);";

                    SQLiteParameter emailParam = new SQLiteParameter(@"emailVal", user.Email);
                    SQLiteParameter passParam = new SQLiteParameter(@"passVal", user.Password);
                    SQLiteParameter nicknnameParam = new SQLiteParameter(@"nickVal", user.Nickname);
                    SQLiteParameter hostemailParam = new SQLiteParameter(@"hostemailVal", user.EmailHost);

                    command.Parameters.Add(emailParam);
                    command.Parameters.Add(passParam);
                    command.Parameters.Add(nicknnameParam);
                    command.Parameters.Add(hostemailParam);
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

        protected override DalObject ToDalObject(SQLiteDataReader reader)
        {
            UserDl result = new UserDl(reader.GetString(0), reader.GetString(1), reader.GetString(2), reader.GetString(3));
            return result;

        }
    }
}