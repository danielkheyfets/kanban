﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.BusinessLayer.UserPackage
{
    public class AdminUserFactory : IUserFactory
    {
        public UserBL createUser(string email, string password, string nickname, string host)
        {
            if (!email.Equals(host))
            {
                throw new ArgumentException("the email must be same as host email");
            }
            return new UserBL(email, password, nickname);
        }
    }
}
