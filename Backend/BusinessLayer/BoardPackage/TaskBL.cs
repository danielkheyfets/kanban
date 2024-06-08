using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Text;
using System.Threading.Tasks;
using IntroSE.Kanban.Backend.DataAccessLayer;

namespace IntroSE.Kanban.Backend.BusinessLayer
{
    public class TaskBL : ITask
    {
        private int taskID;
        private DateTime creationDate;
        private string title;
        private string description;
        private DateTime dueDate;
        private int position;
        private string emailAssignee;
        private string hostEmail;
        private const int MAX_LENGTH_OF_DESCRIPTION = 300;
        private const int MAX_LENGTH_OF_TITLE = 50;

        ///<summary> TaskbL constructer.</summary> 
        public TaskBL(string userEmail, string title, string description, DateTime dueDate, int taskID, int position, string _hostEmail)
        {
            DateTime dateAndTime = DateTime.Now;
            this.creationDate = dateAndTime; // only the date without specific time.
            this.title = title;
            this.description = description;
            this.dueDate = dueDate;
            //   this.userEmail = userEmail;
            this.emailAssignee = userEmail;
            this.hostEmail = _hostEmail;
            this.position = position;

           
            this.taskID = taskID;
           

        }

        ///<summary> TaskbL constructer from DalObject.</summary> 
        public TaskBL(string userEmail, string title, string description, DateTime cDate, DateTime dueDate, int taskID, int position, string _hostEmail)
        {
            DateTime dateAndTime = DateTime.Now;
            this.creationDate = cDate;
            this.title = title;
            this.description = description;
            this.dueDate = dueDate;
            //   this.userEmail = userEmail;
            this.emailAssignee = userEmail;
            this.taskID = taskID;
            this.position = position;
            this.hostEmail = _hostEmail;
            
            
        }

        public void insertToData() // save the task to the db
        {
            TaskDL task = ToDalObject();
            task.Insert();
            
        }


        public void deleteFromData() //delete all tasks from the db
        {
            TaskDL task = ToDalObject();
            task.delete();
        }



        public void setPosition(int newPosition) // set new position to the task
        {


            TaskDL taskdl = this.ToDalObject();
            taskdl.ColOrdinal = newPosition;
            this.position = newPosition;

        }


        public int getPosition()
        {
            return position;
        }


        ///<summary>This method change the title of the task with a new title.</summary>
        ///<param name="newTitle"> the new title for the task.</param>
        public void SetTitle(string newTitle)
        {
            if (newTitle == null || title == "")
            {
                throw new ArgumentException("the title is null or empty");
            }
            if (title.Length > MAX_LENGTH_OF_TITLE)
                throw new ArgumentException("the title is too long");


            TaskDL taskdl = ToDalObject();
            taskdl.Title = newTitle;
            this.title = newTitle;

        }



        ///<summary>This method return the title of the task .</summary>
        public string GetTitle()
        {
            return this.title;
        }




        ///<summary>This method change the description of the task with a new description.</summary>
        ///<param name="newDesc"> the new description for the task.</param>
        public void SetDescription(string newDesc)
        {
            if (newDesc != null && newDesc.Length > MAX_LENGTH_OF_DESCRIPTION)
            {
                throw new ArgumentException("the description is too long");
            }

            TaskDL taskdl = ToDalObject();
            taskdl.Description = newDesc;
            this.description = newDesc;

        }




        ///<summary>This method return the description of the task .</summary>
        public string GetDescription()
        {
            return this.description;
        }



        ///<summary>This method change the due date of the task with a new due date.</summary>
        ///<param name="newDate"> the new due date for the task.</param>
        public void SetDueDate(DateTime newDate)
        {
            if (newDate == null)
                throw new ArgumentException("the due date is null");
            DateTime thisDate = DateTime.Now;
            if (thisDate.CompareTo(newDate) > 0)
            {
                throw new ArgumentException("The new date is already passed");
            }

            TaskDL taskdl = ToDalObject();
            taskdl.DueD = newDate;
            this.dueDate = newDate;

        }



        ///<summary>This method return the due date of the task .</summary>
        public DateTime GetDueDate()
        {
            return this.dueDate;
        }




        ///<summary>This method return the task ID.</summary>
        public int GetID()
        {
            return this.taskID;
        }

        ///<summary>This method return the email if the user.</summary>
        //public string GetUserEmail()
        //{
        //    return this.userEmail;
        //}

        ///<summary>This method return the creation date of the task.</summary>
        public DateTime GetCreationDate()
        {
            return this.creationDate;
        }

        public TaskDL ToDalObject()
        {
            TaskDL task = new TaskDL(this.emailAssignee ,this.taskID,this.title,this.description,this.position,this.creationDate,this.dueDate, this.hostEmail);
            return task;
        }

        public string GetEmailAssignee()
        {
            return this.emailAssignee;
        }

        public void SetEmailAssignee(string emailAssignee)
        {
            TaskDL task = ToDalObject();
            task.Email = emailAssignee;
            this.emailAssignee = emailAssignee;

        }

    }
}
