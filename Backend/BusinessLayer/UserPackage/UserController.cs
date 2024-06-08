using IntroSE.Kanban.Backend.BusinessLayer.UserPackage;
using IntroSE.Kanban.Backend.DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.BusinessLayer
{
    public class UserController
    {
        private Dictionary<string, UserBL> Users;
        private LoginData loginData;
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        ///constructerDa
        public UserController(LoginData loginData)
        {
            Users = new Dictionary<string, UserBL>();
            this.loginData = loginData;

        }
        ///<summary>This method returns a dictionary that contains all the users in the system.</summary>
        public Dictionary<string, UserBL> getusersDictionary()
        {
            return Users;
        }

        ///<summary>This method returns a User by his email.</summary>
        public UserBL GetUser(String email)
        {
            if (email == null)
            {
                throw new ArgumentException("the email is null");
            }
            email = email.ToLower();

            if (email == null)
                throw new ArgumentException("the string is null");
            if (Users.ContainsKey(email))// if the user exists in the system
                return Users[email];
            else
                throw new KeyNotFoundException("This email is not registred");
        }



        public void Register(String email, String password, String nickname, String emailHost, IUserFactory userFactory)
        {
            if (nickname == null || password == null || email == null || emailHost == null)// non of the values can be null
            {
                throw new ArgumentException("invalid input");
            }
            if (nickname.Equals(""))// user must have a nickname
            {
                throw new ArgumentException("the nickname is empty");
            }
            email = email.ToLower();
            emailHost = emailHost.ToLower();

            if (Users.ContainsKey(email)) //can't register if the email already exists in the system
            {
                throw new ArgumentException("the email is already used");
            }

            if (!email.Equals(emailHost) & !Users.ContainsKey(emailHost)) // if the board doesnt exist
            {
                throw new ArgumentException("this host deson't have a board");
            }

            UserBL addUser = userFactory.createUser(email, password, nickname, emailHost);

            addUser.inserToData();
            Users.Add(email, addUser);
            log.Info("New user has register with the email: " + email);
        }


        public void Login(String email, String password)
        {
            if (email == null || password == null)
            {
                throw new ArgumentException("the email or the password is null");
            }
            email = email.ToLower();
            if (IsLogin())
            {
                throw new ArgumentException("another user is logged in now");
            }
            if (Users.ContainsKey(email))
            {
                //UserBL hostUser = Users[hostemail]; //fix the password
                UserBL currUser = Users[email];
                if (!currUser.GetPassword().Equals(password))
                    throw new ArgumentException("password is not correct");
                setIslogin(currUser.Login(email, currUser.GetPassword())); //to declare a user is online
                if (IsLogin())
                {
                    setOnlineUser(currUser.GetEmail());//change the email of the user thats online to the current user
                    loginData.SetOnlineHost(currUser.GetHostEmail());
                    log.Info("The user: " + email + " has logged in");
                }
            }
            else
            {
                throw new ArgumentException("email is not in the system");
            }
        }


        ///<summary>This method returns the email of the current online user.</summary>
        public String getOnlineUser()
        {
            return loginData.getOnlineUser();
        }
        ///<summary>This method is for knowing if a user is logged in.</summary>
        public void setIslogin(bool isLogin)
        {
            loginData.setIslogin(isLogin);
        }
        ///<summary>This method is for update which user is now logged in.</summary>

        public void setOnlineUser(String OnlineUser)
        {
            if (OnlineUser == null)
                throw new ArgumentException("the string is null");
            loginData.setOnlineUser(OnlineUser);
        }



        public void Logout(String email)
        {
            if (email == null)
            {
                throw new ArgumentException("the string is null");
            }
            email = email.ToLower();

            if (email.Equals(getOnlineUser()) && IsLogin())
            {
                setIslogin(false);
                setOnlineUser("");
                loginData.SetOnlineHost("");
                log.Info("The user: " + email + " has loged out");
            }
            else if (IsLogin())//if another user is logged in and not the one that's trying to logout
            {
                throw new ArgumentException("the email is no correct");
            }
            else
            {
                throw new ArgumentException("the user is already logged out");
            }
        }

        ///<summary>This method checks if a user is logged in.</summary>
        public bool IsLogin()
        {
            return loginData.getIsLogin();
        }


        public string GetHostEmail(string email)
        {
            if (email == null)
            {
                throw new ArgumentException("the email is null");
            }
            email = email.ToLower();
            if (Users.ContainsKey(email))
            {
                UserBL currUser = Users[email];
                return currUser.GetHostEmail();
            }
            else
            {
                throw new ArgumentException("email is not in the system");
            }
        }


        ///<summary>This method loads all the users from the DB.</summary>
        public void Import()
        {


            Dictionary<String, UserBL> diOfUserBl = MakeDictionary();
            if (diOfUserBl != null)
            {
                Users = diOfUserBl;
                log.Debug("UserController used Import()");
            }

        }

        ///<summary>This method uploads all the users from the DB to the users dictionary.</summary>
        private Dictionary<string, UserBL> MakeDictionary()
        {
            Dictionary<string, UserBL> userBlDictionary = new Dictionary<string, UserBL>();
            UserDl userFromImport = new UserDl();
            List<UserDl> allUsers = userFromImport.Select();
            if (allUsers == null)
                return null;
            foreach (UserDl user in allUsers)//change the objects from dal objects to business objects
            {
                UserBL userBL = new UserBL(user.Email, user.Password, user.Nickname, user.EmailHost);
                userBlDictionary.Add(user.Email, userBL);
            }
            return userBlDictionary;
        }
        ///<summary>This method deletes all the users from the DB.</summary>
        public void Delete()
        {
            new UserDl().Delete();
            Users = new Dictionary<string, UserBL>();
        }
    }
}
