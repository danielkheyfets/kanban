using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.BusinessLayer.UserPackage
{
    public interface IUserFactory
    {
        UserBL createUser(string email, string password, string nickname, string host);
    }
}
