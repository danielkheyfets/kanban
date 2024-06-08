using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.BusinessLayer
{
    interface IUserController
    {
        ///<summary>This method returns a User by his email.</summary>
        UserBL GetUser(String email);

        ///<summary>This method register a new acoount to the KanbanApp.</summary>
        ///<param name="email"> the email adress that the user is using.</param>
        ///<param name="password"> the user's password.</param>
        ///<param name="nickname"> the user's nickname.</param>
        void Register(String email, String password, String nickname);

        ///<summary>This method let a user to log into his account in the KanbanApp.</summary>
        ///<param name="email"> the email adress that the user is using.</param>
        ///<param name="password"> the user's password.</param>
        void Login(String email, String password);

        ///<summary>This method logs out a user from the system.</summary>
        void Logout(String email);

        ///<summary>This method checks if the user is using the Kanban board at the moment.</summary>
        bool IsLogin();

        ///<summary>This method will upload the data in the JSON files to the RAM.</summary>
        void Import();
    }
}

