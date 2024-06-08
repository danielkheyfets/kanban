using IntroSE.Kanban.Backend.DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.BusinessLayer
{
    public class boardController : IboardController
    {
        private Dictionary<String, boardBL> boards;
        private const int MAX_LENGTH_OF_TITLE = 50;

        public int _NEXT_TASK_ID = 0;
        private LoginData LoginData;
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public boardController(LoginData loginData) // constructor
        {
            boards = new Dictionary<String, boardBL>();
            this.LoginData = loginData;
        }



        public void DeleteAllData() // constructor
        {
            new BoardDL().DeleteAllData();
            new ColumnDL().DeleteAllData();
            new TaskDL().DeleteAllData();
            boards = new Dictionary<string, boardBL>();
            _NEXT_TASK_ID = 0;
           
        }

        public void CreateBoard(string email)
        {
            string lowEmail = email.ToLower();
            boards.Add(lowEmail, new boardBL(lowEmail, 0));
        }

        public columnBL getColumn(string email, int columnOrdinal) // get column by email and column ordinal
        {
            email = email.ToLower();
            return getBoard(email).GetColumn(columnOrdinal);
        }


        public columnBL getColumnByName(string email, string columnName) // get column by name
        {
            email = email.ToLower();
            return getBoard(email).GetColumnByName(columnName);
        }


        public int getNextId() 
        {
            return _NEXT_TASK_ID;
        }

        public void setNextId(int nextId) // set the next task id of this board
        {
            if (nextId < 0)
            {
                throw new ArgumentException("The new id is negative");
            }
            this._NEXT_TASK_ID = nextId;
        }

        public String getOnlineUser() // get the corrent online user
        {
            return LoginData.getOnlineUser();
        }


        public bool IsLogin() //return if some user is login to the kanban 
        {
            return LoginData.getIsLogin();
        }




        public columnBL MoveColumnLeft(string email, int columnOrdinal, string emailHost) // move column to the left identifi by columordinal
        {
            if (IsLogin() && getOnlineUser().Equals(email))
            {
                if (!email.Equals(emailHost)) { throw new ArgumentException("Only the board host can move column"); }
                boardBL board = getBoard(email);
                columnBL column = board.MoveColumnLeft(columnOrdinal);
                log.Info("The column moved left");
                return column;
            }
            else
                throw new ArgumentException("This user is not Online");
        }


        public columnBL MoveColumnRight(string email, int columnOrdinal, string emailHost)// move column to the right identifi by columordinal
        {
            if (IsLogin() && getOnlineUser().Equals(email))
            {
                if (!email.Equals(emailHost)) { throw new ArgumentException("Only the board host can move column"); }
                boardBL board = getBoard(email);
                columnBL column = board.MoveColumnRight(columnOrdinal);
                log.Info("The column moved right");
                return column;

            }
            else
                throw new ArgumentException("This user is not Online");
        }


        public columnBL AddColumn(string email, int columnOrdinal, string Name, string emailHost)// add new column to the board 
        {

            if (IsLogin() && getOnlineUser().Equals(email))
            {
                if (!emailHost.Equals(email)) { throw new ArgumentException("Only the board host can add column"); }
                boardBL board = getBoard(email);
                columnBL columnBL = board.AddColumn(email, columnOrdinal, Name, null); //null for giving the added column
                log.Info("new column has been added");
                return columnBL;               
            }
            else
                throw new ArgumentException("This user is not Online");
        }



        public void RemoveColumn(string email, int columnOrdinal, string emailHost)// remove the column from the kanban
        {
            if (IsLogin() && getOnlineUser().Equals(email))
            {
                if (!email.Equals(emailHost)) { throw new ArgumentException("Only the board host can remove column"); }
                boardBL board = getBoard(email);
                board.RemoveColumn(columnOrdinal);
                log.Info("column removed frome the board");


            }
            else
                throw new ArgumentException("This user is not Online");
        }


        public int addTask(string email, string title, string description, DateTime dueDate)// add new task to the kanban 
        {
            if (title == null || title.Equals(""))
            {
                throw new ArgumentException("title value not correct");
            }
            if (title.Length > 50)
                throw new ArgumentException("the title is too long");

            if (description != null && description.Length > 300)
            {
                throw new ArgumentException("the description is too long");

            }
            if (DateTime.Now.CompareTo(dueDate) > 0)
            {
                throw new ArgumentException("the due date is not correct");
            }
            email = email.ToLower();
            if (IsLogin() && getOnlineUser().Equals(email))
            {
                string hostEmail = LoginData.GetOnlineHost();
                boardBL board = GetBoardOfHost(hostEmail);
                int taskID = board.GetNextID();
                board.addTask(email, title, description, dueDate, taskID); //ad the task to the board
                setNextId(getNextId() + 1); // increse the next task id +1
                log.Info("new Task was added");
                return taskID;
            }
            else
            {
                throw new ArgumentException("This user is not Online");
            }
        }


        public void AdvanceTask(string email, int columnOrdinal, int taskId) //advance task to the next column
        {
            if (email == null)
                throw new ArgumentException("the email is null");
            email = email.ToLower();

            if (IsLogin() && getOnlineUser().Equals(email))
            {
                boardBL board = getBoard(email);
                board.Advance(columnOrdinal, taskId,email);
                log.Info("Task: " + taskId + " has advanced to the next column");
            }
            else
            {
                throw new ArgumentException("This user is not Online");
            }
        }

       

        public void Import()// import all the boards from the db call the makeDictionary function
        {
            
            Dictionary<String, boardBL> dicBoards = MakeDictionary();
            if (dicBoards != null)
            {
                boards = dicBoards;
                log.Debug("The system use Import()");
            }

            else
            {
                boards = new Dictionary<string, boardBL>();
            }
            
        }

        
        
        private Dictionary<String, boardBL> MakeDictionary() // import all the data fro the db an convert it to dictionary its privat function that the import function use
        {
            Dictionary<string, boardBL> outputDictionary = new Dictionary<string, boardBL>();
            foreach (BoardDL boardFromImport in (new BoardDL().select()))
            {
                // strat to create board for the user
                BoardDL tempBoard = boardFromImport;
                List<columnBL> ColumnsBList = new List<columnBL>();
                List<ColumnDL> ColumnsDL = new ColumnDL().select(tempBoard.Email); 
                foreach (ColumnDL tempColumn in ColumnsDL)
                {
                    columnBL columnBlNew = new columnBL(tempColumn.ColumnOrdinal, tempColumn.MaxTasks, tempBoard.Email, tempColumn.CurrNOTasks, tempColumn.ColumnName);
                    foreach (TaskDL taskFromImport in (new TaskDL().select(tempBoard.Email, tempColumn.ColumnOrdinal)))
                    {
                        TaskDL tempTask = taskFromImport;
                        TaskBL newTask = new TaskBL(tempTask.Email , tempTask.Title, tempTask.Description, tempTask.CreationD, tempTask.DueD, tempTask.TaskID, tempTask.ColOrdinal, tempTask.BoardHost);
                        columnBlNew.addTaskFromImport(newTask);
                    }
                    ColumnsBList.Add(columnBlNew);
                }
                boardBL boardForUser = new boardBL(tempBoard.Email, tempBoard.NextID, ColumnsBList);
                // finish to create the user's board and add the board to the dictionary.
                outputDictionary.Add(tempBoard.Email,boardForUser);
            }
            // finish to creat to ALL USERS boards from DB.
            log.Debug("all data from DB is uplodad on Business objects");
            return outputDictionary;
        }
    



        public void LimitColumnTasks(string email, int columnOrdinal, int limit, string emailHost)
        {
            if (email == null)
                throw new ArgumentException("the email is null");
            email = email.ToLower();

            if (IsLogin() && getOnlineUser().Equals(email))
            {
                if (!email.Equals(emailHost)) { throw new ArgumentException("Only the board host can limit the number of tasks"); }
                boardBL board = getBoard(email);
                columnBL column = board.GetColumn(columnOrdinal);
                column.setMax(limit);
                log.Info("Column: " + columnOrdinal + " at user board: " + email + " has been limited to max " + limit + " tasks in column");
            }
            else
            {
                throw new ArgumentException("This user is not Online");
            }
        }



        public void UpdateTaskDescription(string email, int columnOrdinal, int taskId, string description)
        {
            if (email == null)
                throw new ArgumentException("the email is null");
            email = email.ToLower();
            boardBL board = getBoard(email);
            if (columnOrdinal == board.getListOfColumns().Count - 1)
            {
                throw new ArgumentException("task that in the done last cannot be edited");
            }

            if (IsLogin() && getOnlineUser().Equals(email))
            {
                columnBL column = board.GetColumn(columnOrdinal);
                ITask task = column.getTask(taskId);
                if (!task.GetEmailAssignee().Equals(email)) { throw new ArgumentException("only the task assignee can update task description"); }
                task.SetDescription(description);
                log.Info("Task: " + taskId + " at user board: " + email + " has new description: " + description);
            }
            else
            {
                throw new ArgumentException("This user is not Online");
            }
        }



        public void UpdateTaskDueDate(string email, int columnOrdinal, int taskId, DateTime dueDate)
        {
            if (email == null)
                throw new ArgumentException("the email is null");
            email = email.ToLower();

            boardBL board = getBoard(email);
            if (columnOrdinal == board.getListOfColumns().Count - 1)
            {
                throw new ArgumentException("task that in the last coulmn cannot be edited");
            }

            if (IsLogin() && getOnlineUser().Equals(email))
            {
                columnBL column = board.GetColumn(columnOrdinal);
                ITask task = column.getTask(taskId);
                if (!task.GetEmailAssignee().Equals(email)) { throw new ArgumentException("only the task assignee can update due date"); }
                task.SetDueDate(dueDate);
                log.Info("Task: " + taskId + " at user board: " + email + " has new due date: " + dueDate);
            }
            else
            {
                throw new ArgumentException("This user is not Online");
            }
        }



        public void UpdateTaskTitle(string email, int columnOrdinal, int taskId, string title)
        {
            if (title == null || title.Equals("")||title.Length>MAX_LENGTH_OF_TITLE)
            {
                throw new ArgumentException("title value not correct");
            }
            if (email == null)
                throw new ArgumentException("th e email is null");
            email = email.ToLower();
            boardBL board = getBoard(email);

            if (columnOrdinal == board.getListOfColumns().Count - 1)
            {
                throw new ArgumentException("task that in the last coulmn cannot be edited");
            }


            if (IsLogin() && getOnlineUser().Equals(email))
            {
                columnBL column = board.GetColumn(columnOrdinal);
                ITask task = column.getTask(taskId);
                if (!task.GetEmailAssignee().Equals(email)) { throw new ArgumentException("only the task assignee can update task title "); }
                task.SetTitle(title);
                log.Info("Task: " + taskId + " at user board: " + email + " has new title: " + title);
            }
            else
            {
                throw new ArgumentException("This user is not Online");
            }
        }


        public boardBL getBoard(string email)
        {
            if (email == null)
            {
                throw new ArgumentException("the email is null");
            }
            email = email.ToLower();
            string hostEmail = LoginData.GetOnlineHost();
            if (IsLogin() && (getOnlineUser().Equals(email) || hostEmail.Equals(email)))
            {
                bool isContain = boards.ContainsKey(hostEmail);
                if (isContain)
                {
                    boards.TryGetValue(hostEmail, out boardBL boardReturn);
                    _NEXT_TASK_ID = boardReturn.GetNextID();
                    return boardReturn;
                }
                else
                {
                    boardBL boardReturn = new boardBL(email, 0);
                    boards.Add(email, boardReturn);
                    boardReturn.save();
                    return boardReturn;

                }
            }
            else
                throw new ArgumentException("This user is not Online");
        }

        public void AssignTask(string email, string emailHostOfAssignee, int columnOrdinal, int taskId, string emailAssignee)
        {
            boardBL board = getBoard(email);
            if (columnOrdinal == board.getListOfColumns().Count - 1)
            {
                throw new ArgumentException("task that in the last coulmn cannot be edited");
            }
            if (email == null)
                throw new ArgumentException("the email is null");
            email = email.ToLower();
            if (IsLogin() && getOnlineUser().Equals(email))
            {
                string hostEmail = LoginData.GetOnlineHost();
                if (!hostEmail.Equals(emailHostOfAssignee))
                    throw new ArgumentException("the assignee isn't a member of your board");
                board = GetBoardOfHost(hostEmail);
                emailAssignee = emailAssignee.ToLower();
                board.AssignTask(email, columnOrdinal, taskId, emailAssignee);
                log.Info("Task: " + taskId + " has been assigned to: "+ emailAssignee);
            }
            else
            {
                throw new ArgumentException("This user is not Online");
            }
        }


        private boardBL GetBoardOfHost(string Hostemail)
        {
         
            if (boards.ContainsKey(Hostemail))
            {
                boards.TryGetValue(Hostemail, out boardBL boardReturn);
                _NEXT_TASK_ID = boardReturn.GetNextID();
                return boardReturn;
            }
            else         
                throw new ArgumentException("this user doesn't have a board");
        }
       

        public void DeleteTask(string email, int columnOrdinal, int taskId)
        {
            if (email == null)
                throw new ArgumentException("the email is null");
            email = email.ToLower();
            if (IsLogin() && getOnlineUser().Equals(email))
            {
                boardBL board = getBoard(email);
                board.DeleteTask(email, columnOrdinal, taskId);
                log.Info("Task: " + taskId + " has been deleted");
            }
            else
            {
                throw new ArgumentException("This user is not Online");
            }
        }

        public void ChangeColumnName(string email, int columnOrdinal, string newName, string emailHost)
        {
            if (email == null)
                throw new ArgumentException("the email is null");
            email = email.ToLower();
            if (IsLogin() && getOnlineUser().Equals(email))
            {
                if (!email.Equals(emailHost)) { throw new ArgumentException("Only the board host can change the column name"); }
                boardBL board = getBoard(email);
                board.ChangeColumnName(email, columnOrdinal, newName);
                log.Info("Column: " + columnOrdinal + " updeted name to: "+ newName);
            }
            else
            {
                throw new ArgumentException("This user is not Online");
            }
        }

    }
}