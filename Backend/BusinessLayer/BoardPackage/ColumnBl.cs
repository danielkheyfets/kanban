using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntroSE.Kanban.Backend.DataAccessLayer;

namespace IntroSE.Kanban.Backend.BusinessLayer
{
    public class columnBL : Icolumn
    {

        internal int columnOrdinal;
        internal Dictionary<int, ITask> tasksDictionary;
        internal int maxvalue;
        internal String userEmail;
        private const int DEFAULT_MAX_VALUE = 100;
        internal int CurrentNumOfTasks;
        internal string Name;


        ///<summary> constructor</summary>
        ///<param name="maxvalue"> max value of tasks in the column.</param>
        ///<param name="columnOrdinal"> The id of the cloumn .</param>
        ///<param name="columnOrdinal"> The id of the cloumn .</param>

        public columnBL(int columnOrdinal, int maxvalue, String userEmail, int CurrentNumOfTasks, string Name)
        {
            this.columnOrdinal = columnOrdinal;
            tasksDictionary = new Dictionary<int, ITask>();
            this.maxvalue = maxvalue;
            this.userEmail = userEmail;
            this.CurrentNumOfTasks = CurrentNumOfTasks;
            this.Name = Name;
            save();


        }

        // empty cunstrctor for unitTest
        public columnBL()
        {
            this.columnOrdinal = 0;
            tasksDictionary = new Dictionary<int, ITask>();
            this.maxvalue = DEFAULT_MAX_VALUE;
            this.userEmail = "unitTest";
            this.CurrentNumOfTasks = 0;
            this.Name = "mock";
        }


        ///<summary>  copy constructor</summary>
        ///<param name="otherColumn"> other column .</param>
        public columnBL(columnBL otherColumn)
        {
            Dictionary<int, ITask> otherTasks = otherColumn.getAllTasks();
            this.tasksDictionary = new Dictionary<int, ITask>(otherTasks);
            this.columnOrdinal = otherColumn.getcolumnOrdinal();
            this.maxvalue = otherColumn.getMaxValue();
            this.userEmail = otherColumn.getUserEmail();
            this.CurrentNumOfTasks = otherColumn.getCurrentNumOfTasks();
            this.Name = otherColumn.Name;
            save();


        }


        ///<summary>constructor in case the max value of task is not specified</summary>
        public columnBL(int columnOrdinal, String userEmail, int CurrentNumOfTasks, string Name)
        {
            this.columnOrdinal = columnOrdinal;
            tasksDictionary = new Dictionary<int, ITask>();
            this.maxvalue = DEFAULT_MAX_VALUE;
            this.userEmail = userEmail;
            this.CurrentNumOfTasks = CurrentNumOfTasks;
            this.Name = Name;
            save();

        }

        ///<summary>constructor from DalObject</summary>
        public columnBL(String email, int columnNum, int _maxValue, Dictionary<int, ITask> tasks, string Name)
        {
            columnOrdinal = columnNum;
            tasksDictionary = tasks;
            maxvalue = _maxValue;
            userEmail = email;
            CurrentNumOfTasks = tasks.Count();
            this.Name = Name;
            save();

        }


        public virtual void save() // save the column to the db
        {
            toDalObject().Insert();
        }

        public virtual void addTasksToDictionary(Dictionary<int, ITask> otherDictionary) // add task to this column that aready exist in the board 
            //use this function after moving columns
        {
            if (maxvalue != -1 && otherDictionary.Count() + tasksDictionary.Count() > this.maxvalue) 
            {
                throw new ArgumentException("the amount of tasks is bigger than max value");
            }

            foreach (TaskBL taskkey in otherDictionary.Values)// increase the courent num of tasks in this colum
            {
                setCurrentNumOfTasks(this.CurrentNumOfTasks + 1); 
                taskkey.setPosition(columnOrdinal);
                tasksDictionary.Add(taskkey.GetID(), otherDictionary[taskkey.GetID()]);
            }
        }



        public void addTask(string email, string title, string description, DateTime dueDate, int taskId)
        {
            if (reachedMax())
            {
                throw new Exception("The amount of tasks is greater than the maximum value");
            }
            if (email == null || dueDate == null)
                throw new ArgumentException("the string is null");
            ITask task = new TaskBL(email, title, description, dueDate, taskId, columnOrdinal, userEmail);
            toDalObject().CurrNOTasks = CurrentNumOfTasks + 1;
            tasksDictionary.Add(taskId, task);
            task.insertToData();
            this.setCurrentNumOfTasks(this.getCurrentNumOfTasks() + 1); //add +1 to the current num of tasks
        }



        public void addTask(ITask task)
        {
            if (reachedMax()) //  if CurrentNumOfTasks>maxvalue 
            {
                throw new Exception("The amount of tasks is greater than the maximum value");
            }
            task.setPosition(columnOrdinal);
            toDalObject().CurrNOTasks = CurrentNumOfTasks + 1;
            tasksDictionary.Add(task.GetID(), task);
            this.setCurrentNumOfTasks(this.getCurrentNumOfTasks() + 1);//add +1 to the current num of tasks
        }


        public void AssignTask(string email, int columnOrdinal, int taskId, string emailAssignee)
        {
            if (!email.Equals(emailAssignee)) //no need to change
                getTask(taskId).SetEmailAssignee(emailAssignee);
        }


        public void DeleteTask(string email, int taskId)
        {
            ITask task = getTask(taskId);
            if (!task.GetEmailAssignee().Equals(email)) { throw new ArgumentException("only the task assignee can delete the task"); }
            task.deleteFromData();
            tasksDictionary.Remove(taskId); //remove task from the dictionary
            setCurrentNumOfTasks(this.CurrentNumOfTasks - 1); // reduce the current number of tasks
        }


        public void addTaskFromImport(ITask task) 
        { 
            tasksDictionary.Add(task.GetID(), task);
        }


        public virtual void setPositionOfAllTasks(int newPosition) //set all the task position to a new position
        {
            foreach (int taskkey in tasksDictionary.Keys)
            {
                tasksDictionary[taskkey].setPosition(newPosition);
            }
        }


        public bool hasTask(int taskID)
        {
            bool isContain = tasksDictionary.ContainsKey(taskID); // check the dictionary by the id ,the id is represent the key in the Dictionary
            return isContain;
        }



        public bool reachedMax() // check if the amount of tasks is equals too max of tasks
        {
            return maxvalue != -1 && CurrentNumOfTasks == maxvalue;
        }

        public void setcolORD(int x)
        {
            columnOrdinal = x;
        }

        public bool removeTask(int taskID)
        {

            bool isRemoved = tasksDictionary.Remove(taskID); //return true if the task removed from the colum
            if (isRemoved)
            {
                // create new func that update the DB. (delte task)
                
                this.setCurrentNumOfTasks(this.getCurrentNumOfTasks() - 1);
                
                
            }
            return isRemoved;
        }


        public virtual void removeFromeData()
        {
            toDalObject().delete();
        }

        public bool setMax(int newMax) // set the max number of tasks in this column
        {
            if (newMax <= 0)
                throw new Exception("the new value of max task is negative");
            if (CurrentNumOfTasks > newMax)
                throw new Exception("the new max value is smaller then the current number of tasks"); //if the new max value is smaller then the current num throw exeption
            toDalObject().MaxTasks = newMax;
            this.maxvalue = newMax;
            return true;
        }


        public virtual void deleteFromData()  // delate all the data from the db 
        {
            toDalObject().delete();

        }

        public ITask getTask(int taskID)
        {
            if (!hasTask(taskID))
                throw new Exception("the column is not conatains this task"); //if the task is not in this column throw exeption
            return tasksDictionary[taskID];
        }




        public virtual Dictionary<int, ITask> getAllTasks()
        {
            return tasksDictionary;
        }



        public int getcolumnOrdinal() 
        {
            return this.columnOrdinal;
        }

        public int getMaxValue()
        {
            return this.maxvalue;
        }

        public String getUserEmail()
        {
            return this.userEmail;
        }



        public int getCurrentNumOfTasks()
        {
            return this.CurrentNumOfTasks;
        }


        public void setCurrentNumOfTasks(int numOfTasks)
        {
            toDalObject().CurrNOTasks = numOfTasks;
            this.CurrentNumOfTasks = numOfTasks;
        }


        public ColumnDL toDalObject()  // convert the column to db column
         {   
             return new ColumnDL(userEmail,columnOrdinal,Name, maxvalue, CurrentNumOfTasks);
         }
         

        public virtual void setColumnOrdinal(int columnOrdinal)
        {
            toDalObject().ColumnOrdinal = columnOrdinal;
            this.columnOrdinal = columnOrdinal;
        }

        public void SetColumnName(string newName)
        {
            toDalObject().ColumnName = newName;
            this.Name = newName;
        }

        public string getColumnName()
        {
            return Name;
        }
       

    }


}
