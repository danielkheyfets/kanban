using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.BusinessLayer
{
    public class LoginData
    {
        private Boolean isLogin; //if a user is now online
        private String OnlineUser; // which user is online
        private string OnlineHost; // the host email of the online user


        public LoginData() //the dafault is no user online
        {
            this.isLogin = false;
            this.OnlineUser = "";
            this.OnlineHost = "";
        }
      
        public String getOnlineUser()
        {
            return OnlineUser;
        }

        public bool getIsLogin()
        {
            return this.isLogin;
        }

        public void setIslogin(bool isLogin)
        {
            this.isLogin=isLogin;
        }

        public void setOnlineUser(String OnlineUser)
        {
            this.OnlineUser = OnlineUser;
        }

        public string GetOnlineHost()
        {
            return this.OnlineHost;
        }

        public void SetOnlineHost(string emailHost)
        {
            this.OnlineHost = emailHost;
        }

    }
}
