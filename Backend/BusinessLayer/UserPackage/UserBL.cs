using IntroSE.Kanban.Backend.DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.BusinessLayer
{
    public class UserBL
    {
        private String email;
        private String password;
        private String nickname;
        private string emailHost;
        private UserDl dalUser;

        //empty constructor
        public UserBL()
        {
            this.email = "";
            this.password = "";
            this.nickname = "";
            this.emailHost = "";
        }

        //constructor
        public UserBL(String email, String password, String nickname)
        {
            if (CheckEmail(email) && CheckPassword(password))
            {
                this.email = email;
                this.password = password;
                this.nickname = nickname;
                this.emailHost = email;

            }
        }

        public UserBL(string email, string password, string nickname, string host)
        {
            if (CheckEmail(email) && CheckEmail(host) && CheckPassword(password))
            {
                this.email = email;
                this.password = password;
                this.nickname = nickname;
                this.emailHost = host;
            }
        }

        public void inserToData()
        {
            dalUser = toDalObject();
            dalUser.Insert();
        }

        public String GetPassword()
        {
            return this.password;
        }

        public void SetPassword(String password)
        {
            this.password = password;
        }

        public String GetEmail()
        {
            return this.email;

        }

        public void SetEmail(String email)
        {
            this.email = email;
        }

        public String GetNickname()
        {
            return this.nickname;
        }

        public void SetNickname(String nickname)
        {
            this.nickname = nickname;
        }

        public string GetHostEmail()
        {
            return this.emailHost;
        }

        public bool Login(String email, String password)
        {
            if (!this.email.Equals(email) || !this.password.Equals(password))
                throw new ArgumentException("The email or the password is not correct");

            return this.email.Equals(email) && this.password.Equals(password);
        }


        public UserDl toDalObject()
        {
            UserDl user = new UserDl(this.email, this.password, this.nickname, this.emailHost);
            return user;
        }

        ///<summary>This method checks if the email adress represent a valid email</summary>
        private bool CheckEmail(String email)
        {
            if (email == null)
                throw new ArgumentException("the email is null");


            string pattern = @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z";

            //check first string
            if (!Regex.IsMatch(email, pattern))
            {
                throw new ArgumentException("Not Valid email adress");
            }

            else
                return true;
        }

        ///<summary>This method checks if the password is valid</summary>
        private bool CheckPassword(String password)
        {
            if (password == null)
                throw new ArgumentException("the password is null");
            if (password.Length < 5 | password.Length > 25)
                throw new ArgumentException("not Valid password");
            if (!(password.Any(char.IsDigit) && password.Any(char.IsUpper) && password.Any(char.IsLower)))
                throw new ArgumentException("not Valid password");
            return true;
        }
    }
}
