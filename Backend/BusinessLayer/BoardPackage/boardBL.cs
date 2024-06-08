using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using IntroSE.Kanban.Backend.DataAccessLayer;

namespace IntroSE.Kanban.Backend.BusinessLayer
{
    public class boardBL : Iboard
    {
        internal List<columnBL> ListColumn;
        private String hostEmail;
        private int nextInd;
        private const int _MINIMUM = 0;
        private const int MAX_LENGTH_OF_COLUMN_NAME = 15;
        private const int MIN_LENGTH_OF_COLUMN_NAME = 1;



        ///<summary>  this constructor initializes all fields by default</summary>
        ///<param name="userEmail"> the email adress of the user .</param>
        public boardBL(String userEmail, int _nextInd)
        {
            this.hostEmail = userEmail;
            this.nextInd = _nextInd;
            ListColumn = new List<columnBL>();
            columnBL column1 = new columnBL(0, userEmail, 0, "backlog");
         //   column1.save();  
            columnBL column2 = new columnBL(1, userEmail, 0, "in progress");
         //   column2.save();
            columnBL column3 = new columnBL(2, userEmail, 0, "done");
         //   column3.save();
            ListColumn.Add(column1);
            ListColumn.Add(column2);
            ListColumn.Add(column3);
            save();

        }

        //constructor for mock - unitTest
        public boardBL()
        {
            this.hostEmail = "unitTest";
            ListColumn = new List<columnBL>();
            this.nextInd = 0;
            columnBL column1 = new columnBL();
            columnBL column2 = new columnBL();
            columnBL column3 = new columnBL();
            ListColumn.Add(column1);
            ListColumn.Add(column2);
            ListColumn.Add(column3);

        }

        ///<summary>  this constructor is for loadData only</summary>
        ///<param name="userEmail"> the email adress of the user .</param>
        public boardBL(String userEmail, int _nextInd, List<columnBL> ListCol)
        {
            this.hostEmail = userEmail;
            this.nextInd = _nextInd;
            ListColumn = ListCol;

        }



        ///<param name="column0">column with ordinal value 0 .</param>
        ///<param name="column1">column with ordinal value 1 .</param>
        ///<param name="column2"> column with ordinal value 2 .</param>
        ///<param name="userEmail"> the email adress of the user .</param>

        public boardBL(String userEmail, columnBL column0, columnBL column1, columnBL column2, int _nextInd)
        {
            ListColumn = new List<columnBL>();
            ListColumn.Add(new columnBL(column0));
            ListColumn.Add(new columnBL(column1));
            ListColumn.Add(new columnBL(column2));
            this.hostEmail = userEmail;
            this.nextInd = _nextInd;
            save();
        }


        public List<columnBL> getListOfColumns()
        {
            return ListColumn;
        }


        public void Advance(int columnOrdinal, int taskID, string email)
        {
            if (columnOrdinal == ListColumn.Count - 1) // if the column is "done" column throw exeption
            {
                throw new Exception("you cannot advance tasks from this column");
            }
            ITask task = GetColumn(columnOrdinal).getTask(taskID);
            if (!task.GetEmailAssignee().Equals(email)) { throw new ArgumentException("only the task assignee can advance the task "); }
            ListColumn.ElementAt<columnBL>(columnOrdinal).removeTask(taskID);
            ListColumn.ElementAt<columnBL>(columnOrdinal + 1).addTask(task);
        }


        public void AssignTask(string email, int columnOrdinal, int taskId, string emailAssignee)
        {
            GetColumn(columnOrdinal).AssignTask(email, columnOrdinal, taskId, emailAssignee);

        }


        public void DeleteTask(string email, int columnOrdinal, int taskId)
        {
            GetColumn(columnOrdinal).DeleteTask(email, taskId);
        }


        public void ChangeColumnName(string email, int columnOrdinal, string newName)
        {
            if (!email.Equals(this.hostEmail))
                throw new ArgumentException("only the board creator can change the column name");
            if (newName.Length > MAX_LENGTH_OF_COLUMN_NAME)
                throw new ArgumentException("the name is too long");
            GetColumn(columnOrdinal).SetColumnName(newName);

        }


        public columnBL GetColumn(int columnOrdinal)
        {
            if (columnOrdinal > ListColumn.Count - 1 || columnOrdinal < _MINIMUM) 
            {
                throw new ArgumentException("the calumn ordinal is incorrect");
            }
            return ListColumn.ElementAt<columnBL>(columnOrdinal); // get the column by column ordinal from the dictionary
        }


        public columnBL GetColumnByName(string columnName)
        {
            int i = -1;
            if (columnName == null) // add more tests
            {
                throw new ArgumentException("the column name is worng");
            }
            foreach (columnBL tempColumn in ListColumn)
            {
                string check = tempColumn.getColumnName();
                if (check.Equals(columnName)) {
                     i = tempColumn.getcolumnOrdinal();
                }
            }
            if (i == -1) //the column is not in the dictionary
            {
                 throw new ArgumentException("column did not found by name");
            }
            return ListColumn.ElementAt<columnBL>(i);
        }


        public bool nameIsValid(string name)
        {
            if (name == null | name.Length < MIN_LENGTH_OF_COLUMN_NAME | name.Length >MAX_LENGTH_OF_COLUMN_NAME) //if the lenth of the name is biger then 15 or smaller the 1
                return false;
            return true;
        }


        public columnBL AddColumn(string email, int columnOrdinal, string Name, columnBL columnNew)
        {
            if (!nameIsValid(Name))
            {
                throw new ArgumentException("the Name value is not correct");
            }

            if (columnOrdinal > ListColumn.Count() || columnOrdinal < _MINIMUM)
            {
                throw new ArgumentException("the columnOrdinal is not correct");
            }
            if (isAlreadyComtainThisName(Name))
            {
                throw new ArgumentException("this column name is already exist");
            }
            columnBL column;
            if (columnNew != null)
            {
                column = columnNew;
                
            }
            else
                column = new columnBL(columnOrdinal, email, 0, Name);
                
            ListColumn.Insert(columnOrdinal, column);
            for (int i = ListColumn.Count() - 1; i >= columnOrdinal + 1; i--) //increase the column ordinal +1 forom the new column we insert
            {
                columnBL col = ListColumn.ElementAt<columnBL>(i);
                col.setColumnOrdinal(col.getcolumnOrdinal() + 1);// increase the column ordinal
                col.setPositionOfAllTasks(col.getcolumnOrdinal());// set the column ordinal to all tasks in the column
            }
            column.save();
            return column;


        }


        public virtual void AddColumn(columnBL column, int columOrdinal) // for the move column function
        {

            ListColumn.Insert(columOrdinal, column);
            for (int i = ListColumn.Count() - 1; i >= columOrdinal + 1; i--) //increase the column ordinal +1 forom the new column we insert
            {
                columnBL col = ListColumn.ElementAt<columnBL>(i);
                col.setColumnOrdinal(col.getcolumnOrdinal() + 1);// increase the column ordinal
                col.setPositionOfAllTasks(col.getcolumnOrdinal());// set the column ordinal to all tasks in the column
            }
        }
        


     public virtual columnBL MoveColumnRight(int columnOrdinal)
        {
            if (columnOrdinal > ListColumn.Count() || columnOrdinal < _MINIMUM)
            {
                throw new ArgumentException("the columnOrdinal is not correct");
            }

            if (columnOrdinal == ListColumn.Count() - 1)
            {
                throw new ArgumentException("this column is the last you canot move it to right");
            }

            columnBL col = ListColumn.ElementAt<columnBL>(columnOrdinal);
            col.deleteFromData(); 
            removeForMove(columnOrdinal);   //remove from the dictionary
            AddColumn(col, columnOrdinal + 1); 
            col.setcolORD(columnOrdinal + 1); // increase the column ordinal
            col.setPositionOfAllTasks(columnOrdinal + 1);
            col.save();
            return col;
        }



        public void removeForMove(int columnOrdinal) //// for the move column function
        {
          
            ListColumn.RemoveAt(columnOrdinal);
            for (int i = columnOrdinal; i < ListColumn.Count(); i++)
            {
                columnBL col = ListColumn.ElementAt<columnBL>(i);
                int j = col.getcolumnOrdinal() -1;
                col.setColumnOrdinal(j);
                col.setPositionOfAllTasks(col.getcolumnOrdinal());
            }
        }




        public columnBL MoveColumnLeft(int columnOrdinal)
        {
            if (columnOrdinal > ListColumn.Count() || columnOrdinal < _MINIMUM)
            {
                throw new ArgumentException("the columnOrdinal is not correct");
            }

            if (columnOrdinal == _MINIMUM)
            {
                throw new ArgumentException("this column is the first you canot move it to left");
            }

            columnBL col = ListColumn.ElementAt<columnBL>(columnOrdinal);
            col.deleteFromData();
            removeForMove(columnOrdinal);
            AddColumn(col, columnOrdinal - 1);
            col.setcolORD(columnOrdinal - 1); // decrease the column ordinal
            col.setPositionOfAllTasks(columnOrdinal - 1); // set the column ordinal to all tasks in the column
            col.save();
            return col;
        }



        public void RemoveColumn(int columnOrdinal)
        {

            if (columnOrdinal >= ListColumn.Count() || columnOrdinal < _MINIMUM)
            {
                throw new ArgumentException("the columnOrdinal is not correct");
            }
            if (ListColumn.Count() == 2)
            {
                throw new ArgumentException("you cannot delete another column,the board have 2 columns ");
            }
            columnBL column = ListColumn.ElementAt<columnBL>(columnOrdinal);
            if (columnOrdinal == 0)
            {
                columnBL rightcolumn = ListColumn.ElementAt<columnBL>(1);
                if (/*rightcolumn.maxvalue != -1 &&*/ column.getAllTasks().Count() + rightcolumn.tasksDictionary.Count() > rightcolumn.maxvalue)
                {
                    throw new ArgumentException("the amount of tasks is bigger than max value");
                }
                rightcolumn.addTasksToDictionary(column.getAllTasks());
            }
            else
            {
                columnBL rightcolumn = ListColumn.ElementAt<columnBL>(columnOrdinal - 1);
                if (/*rightcolumn.maxvalue != -1 &&*/ column.getAllTasks().Count() + rightcolumn.tasksDictionary.Count() > rightcolumn.maxvalue) 
                {
                    throw new ArgumentException("the amount of tasks is bigger than max value");
                }
                rightcolumn.addTasksToDictionary(column.getAllTasks());
            }
            ListColumn.RemoveAt(columnOrdinal);
            column.removeFromeData();
            for (int i = columnOrdinal; i < ListColumn.Count(); i++) // decrease the column ordinal of the columns
            {
                columnBL col = ListColumn.ElementAt<columnBL>(i);
                col.setColumnOrdinal(col.getcolumnOrdinal() - 1);// decrease the column ordinal
                col.setPositionOfAllTasks(col.getcolumnOrdinal());// set the column ordinal to all tasks in the column
            }
        }




        private bool isAlreadyComtainThisName(string name) // check if this name is already exist
        {
            foreach (columnBL col in ListColumn)
            {
                if (col.getColumnName().Equals(name))
                {
                    return true;
                }
            }
            return false;
        }



        public string GetUserEmail()
        {
            return this.hostEmail;
        }



        public List<columnBL> GetBoard()
        {
            List<columnBL> arrColumnsBoard = this.ListColumn;
            return arrColumnsBoard;
        }



        public void addTask(string email, string title, string description, DateTime dueDate, int taskId)
        {
            this.GetColumn(0).addTask(email, title, description, dueDate, taskId);
            setNextID();
        }



        public int GetNextID()
        {
            return nextInd;
        }

        public void setNextID()
        {
            toDalObject().NextID = this.nextInd + 1;
            this.nextInd++;

        }

        public void save()
        {
            BoardDL board = toDalObject();
            board.Insert();
        }
        
        public BoardDL toDalObject()
        {
            BoardDL newBoard = new BoardDL(this.hostEmail, this.GetNextID());
            return newBoard;
        }

        
    }
}
