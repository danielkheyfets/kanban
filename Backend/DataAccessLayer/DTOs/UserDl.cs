using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    public class UserDl : DalObject
    {
        public const string PasswordColumnName = "Password";
        public const string NicknameColumnName = "Nickname";
        public const string HostColumnName = "AssigneeEmail";


        private string _email;
        public string Email { get => _email; set { _email = value; } }

        private string _password;
        public string Password { get => _password; set { _password = value; } }

        private string _nickname;
        public string Nickname { get => _nickname; set { _nickname = value; } }

        private string _emailHost;
        public string EmailHost { get => _emailHost; set { _emailHost = value; } }

        public UserDl(string email, string password, string nickname, string emailHost) : base(new UserDalController())
        {
            _email = email;
            _password = password;
            _nickname = nickname;
            _emailHost = emailHost;
        }

        public UserDl() : base(new UserDalController())
        {

        }

        public void Insert()
        {
            ((UserDalController)_controller).Insert(this);
        }

        public List<UserDl> Select()
        {
            return ((UserDalController)_controller).SelectAllUsers();
        }

        public void Delete()
        {
            ((UserDalController)_controller).DeleteData();
        }
    }
}
