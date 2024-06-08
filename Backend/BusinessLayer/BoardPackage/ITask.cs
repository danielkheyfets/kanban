using IntroSE.Kanban.Backend.DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace IntroSE.Kanban.Backend.BusinessLayer
{
    public interface ITask
    {
        ///<summary>This method change the title of the task with a new title.</summary>
        ///<param name="newTitle"> the new title for the task.</param>
        void SetTitle(string newTitle);

        ///<summary>This method return the title of the task .</summary>
        string GetTitle();

        ///<summary>This method change the description of the task with a new description.</summary>
        ///<param name="newDesc"> the new description for the task.</param>
        void SetDescription(string newDesc);

        ///<summary>This method return the description of the task .</summary>
        string GetDescription();

        ///<summary>This method change the due date of the task with a new due date.</summary>
        ///<param name="newDate"> the new due date for the task.</param>
        void SetDueDate(DateTime newDate);

        ///<summary>This method return the due date of the task .</summary>
        DateTime GetDueDate();


        ///<summary>This method return the task ID.</summary>
        int GetID();

        ///<summary>This method return the email if the user.</summary>
        //   string GetUserEmail();

        ///<summary>This method return the assignee email of the task.</summary>
        string GetEmailAssignee();

        ///<summary>This method changes the assignee email of the task.</summary>
        void SetEmailAssignee(string emailAssignee);

        ///<summary>This method return the creation date of the task.</summary>
        DateTime GetCreationDate();

        void setPosition(int newPosition);


        int getPosition();

        void deleteFromData();

        void insertToData();

        TaskDL ToDalObject();
    }
}