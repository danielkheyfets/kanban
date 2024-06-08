using IntroSE.Kanban.Backend.BusinessLayer;
using IntroSE.Kanban.Backend.BusinessLayer.UserPackage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.ServiceLayer
{
    public class UserService
    {
        private UserController userController;
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public UserService(LoginData loginData)
        {
            userController = new UserController(loginData);
            LoadData();
        }

        public Response LoadData()
        {
            try
            {
                userController.Import();
            }
            catch (Exception e)
            {
                log.Error("LoadData(userService) has failed : " + e.Message);
                return new Response(e.Message);
            }
            return new Response();
        }

        public Response DeleteData()
        {
            try
            {
                userController.Delete();
            }
            catch (Exception e)
            {
                log.Error("Delete Data (userService) has failed : " + e.Message);
                return new Response(e.Message);
            }
            return new Response();
        }


        public Response Register(string email, string password, string nickname)
        {
            try
            {
                userController.Register(email, password, nickname, email, new AdminUserFactory());
            }
            catch (Exception e)
            {
                log.Error("Register has failed : " + e.Message);
                return new Response(e.Message);
            }
            return new Response();
        }


        public Response Register(string email, string password, string nickname, string emailHost)
        {
            try
            {
                userController.Register(email, password, nickname, emailHost, new NormalUserFactory());
            }
            catch (Exception e)
            {
                log.Error("Registration failed : " + e.Message);
                return new Response(e.Message);
            }
            return new Response();
        }


        public Response<User> Login(string email, string password)
        {
            String nickname;
            try
            {
                nickname = userController.GetUser(email).GetNickname();
                userController.Login(email, password);

            }
            catch (Exception e)
            {
                log.Error("Login has failed : " + e.Message);
                return new Response<User>(e.Message);
            }
            User user = new User(email, nickname);
            return new Response<User>(user);
        }

        public Response Logout(string email)
        {
            try
            {
                userController.Logout(email);
            }
            catch (Exception e)
            {
                log.Error("Logout has failed : " + e.Message);
                return new Response(e.Message);
            }
            return new Response();
        }

        public string GetHostEmail(string email)
        {
            try
            {
                return userController.GetHostEmail(email);
            }
            catch (Exception e)
            {
                log.Error("Failed to get the email of the Board's host : " + e.Message);
                return null;
            }
        }
    }
}
