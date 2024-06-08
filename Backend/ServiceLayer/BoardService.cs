using IntroSE.Kanban.Backend.BusinessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace IntroSE.Kanban.Backend.ServiceLayer
{
    public class BoardService
    {
        private boardController boardController;
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public BoardService(LoginData loginData)
        {
            this.boardController = new boardController(loginData);
            LoadData();
        }

        public Response LoadData()
        {
            try
            {
               boardController.Import();
            }
            catch (Exception e)
            {
                log.Error("LoadData(BoardService) has failed : " + e.Message);
                return new Response(e.Message);
            }
            return new Response();
        }


        public Response DeleteData()
        {
            try
            {
                boardController.DeleteAllData();
            }
            catch (Exception e)
            {
                log.Error("Delete Data (boardService) has failed : " + e.Message);
                return new Response(e.Message);
            }
            return new Response();
        }


        public Response AssignTask(string email, string emailHostOfAssignee, int columnOrdinal, int taskId, string emailAssignee)
        {
            try
            {
                boardController.AssignTask(email, emailHostOfAssignee, columnOrdinal, taskId, emailAssignee);
            }
            catch (Exception e)
            {
                log.Error("Failed to Assign Task : " + e.Message);
                return new Response(e.Message);
            }
            return new Response();
        }

        public Response<Task> AddTask(string email, string title, string description, DateTime dueDate)
        {
            try
            {
                DateTime Cdate = DateTime.Today;
                int TaskID = boardController.addTask(email, title, description, dueDate);
                Task taskService = new Task(TaskID, Cdate, dueDate, title, description, email);
                return new Response<Task>(taskService);
            }
            catch (Exception e)
            {
                log.Error("Add Task has failed : " + e.Message);
                return new Response<Task>(e.Message);
            }
        }

        


        public Response DeleteTask(string email, int columnOrdinal, int taskId)
        {
            try
            {
                boardController.DeleteTask(email, columnOrdinal, taskId);
                return new Response();
            }
            catch (Exception e)
            {
                log.Error("Delete Task has failed : " + e.Message);
                return new Response<Task>(e.Message);
            }
        }

        public Response ChangeColumnName(string email, int columnOrdinal, string newName, string emailHost)
        {
            try
            {
                boardController.ChangeColumnName(email, columnOrdinal, newName, emailHost);
                return new Response();
            }
            catch (Exception e)
            {
                log.Error("failed to change the column name: " + e.Message);
                return new Response<Task>(e.Message);
            }
        }



        public Response AdvanceTask(string email, int columnOrdinal, int taskId)
        {
            try
            {
                boardController.AdvanceTask(email, columnOrdinal, taskId);
            }
            catch (Exception e)
            {
                log.Error("Advance Task has failed : " + e.Message);
                return new Response(e.Message);
            }
            return new Response();
        }

        public Response LimitColumnTasks(string email, int columnOrdinal, int limit, string emailHost)
        {
            try
            {
                boardController.LimitColumnTasks(email, columnOrdinal, limit, emailHost);
            }
            catch (Exception e)
            {
                log.Error("Limit column Task has failed : " + e.Message);
                return new Response(e.Message);
            }
            return new Response();
        }

        public Response<Board> GetBoard(string email)
        {
                try
                {
                    boardBL board = boardController.getBoard(email);
                    List<string> columnsAsService = new List<string>();
                    List<columnBL> columnName = board.GetBoard();
                    foreach (columnBL tempColumn in columnName)
                    {
                    string name = tempColumn.getColumnName();
                    columnsAsService.Add(name);
                    }
                    IReadOnlyCollection<string> columnsNames = columnsAsService.AsReadOnly();
                    Board boardService = new Board(columnsNames, email);
                    return new Response<Board>(boardService);
                }
                catch (Exception e)
                {
                    log.Error("GetBoard has fail: " + e.Message);
                    return new Response<Board>(e.Message);
                }  
        }


        public Response UpdateTaskTitle(string email, int columnOrdinal, int taskId, string title)
        {
            try
            {
                boardController.UpdateTaskTitle(email, columnOrdinal, taskId, title);
            }
            catch (Exception e)
            {
                log.Error("Update Task Title has failed : " + e.Message);
                return new Response(e.Message);
            }
            return new Response();
        }

        public Response UpdateTaskDescription(string email, int columnOrdinal, int taskId, string description)
        {
            try
            {
                boardController.UpdateTaskDescription(email, columnOrdinal, taskId, description);
            }
            catch (Exception e)
            {
                log.Error("Update Task description has failed : " + e.Message);
                return new Response(e.Message);
            }
            return new Response();
        }

        public Response UpdateTaskDueDate(string email, int columnOrdinal, int taskId, DateTime dueDate)
        {
            try
            {
                boardController.UpdateTaskDueDate(email, columnOrdinal, taskId, dueDate);
            }
            catch (Exception e)
            {
                log.Error("Update Task due date has failed : " + e.Message);
                return new Response(e.Message);
            }
            return new Response();
        }

        public Response<Column> GetColumn(string email, string columnName)
        {
            try
            {
                Column colSL = GetColumnPrivate(email, -1, columnName);
                return new Response<Column>(colSL);
            }

            catch (Exception e)
            {
                log.Error("GetColumn has failed : " + e.Message);
                return new Response<Column>(e.Message);
            }
        }

        public Response<Column> GetColumn(string email, int columnOrdinal)
        {
            try
            {
                Column colSL = GetColumnPrivate(email, columnOrdinal,"-1");
                return new Response<Column>(colSL);
            }
            catch (Exception e)
            {
                log.Error("GetColumn has failed : " + e.Message);
                return new Response<Column>(e.Message);
            }
        }

        private Column GetColumnPrivate(string email, int columnOrdinal, string columnName)
        {
            columnBL colBL;
            if (columnName.Equals("-1")) // made column by columnOrdinal.
            {
                colBL = boardController.getColumn(email, columnOrdinal);
            }
            else // made column by columnName.
            {
                colBL = boardController.getColumnByName(email, columnName);
            }
            string nameColumn = colBL.getColumnName();
            List<Task> tempList = new List<Task>();
            foreach (ITask tempTask in colBL.getAllTasks().Values)
            {
                Task task1 = new Task(tempTask.GetID(), tempTask.GetCreationDate(), tempTask.GetDueDate(), tempTask.GetTitle(), tempTask.GetDescription(), tempTask.GetEmailAssignee());
                tempList.Add(task1);
            }
            IReadOnlyCollection<Task> tasksCollectionReadOnly = tempList.AsReadOnly();
            Column colSL = new Column(tasksCollectionReadOnly, nameColumn, colBL.getMaxValue());
            return colSL;
        }

        public Response RemoveColumn(string email, int columnOrdinal, string emailHost)
        {
            try
            {
                boardController.RemoveColumn(email, columnOrdinal, emailHost);
            }
            catch (Exception e)
            {
                log.Error("Remove Column has failed: " + e.Message);
                return new Response(e.Message);
            }
            return new Response();
        }

        public Response<Column> AddColumn(string email, int columnOrdinal, string Name, string emailHost)
        {
            Column colSL;
            try
            {
                columnBL colBL = boardController.AddColumn(email, columnOrdinal, Name, emailHost);
                string nameColumn = colBL.getColumnName();
                List<Task> tempList = new List<Task>();
                foreach (ITask tempTask in colBL.getAllTasks().Values)
                {
                    Task task1 = new Task(tempTask.GetID(), tempTask.GetCreationDate(), tempTask.GetDueDate(), tempTask.GetTitle(), tempTask.GetDescription(), tempTask.GetEmailAssignee());
                    tempList.Add(task1);
                }
                IReadOnlyCollection<Task> tasksCollectionReadOnly = tempList.AsReadOnly();
                colSL = new Column(tasksCollectionReadOnly, nameColumn, colBL.getMaxValue());
                return new Response<Column>(colSL);
            }
            catch (Exception e)
            {
                log.Error("Add Column has failed: " + e.Message);
                return new Response<Column>(e.Message);
            }
        }


        public Response<Column> MoveColumnRight(string email, int columnOrdinal, string emailHost)
        {
            Column colSL;
            try
            {
                columnBL colBL = boardController.MoveColumnRight(email, columnOrdinal, emailHost);
                string nameColumn = colBL.getColumnName();
                List<Task> tempList = new List<Task>();
                foreach (ITask tempTask in colBL.getAllTasks().Values)
                {
                    Task task1 = new Task(tempTask.GetID(), tempTask.GetCreationDate(), tempTask.GetDueDate(), tempTask.GetTitle(), tempTask.GetDescription(), tempTask.GetEmailAssignee());
                    tempList.Add(task1);
                }
                IReadOnlyCollection<Task> tasksCollectionReadOnly = tempList.AsReadOnly();
                colSL = new Column(tasksCollectionReadOnly, nameColumn, colBL.getMaxValue());
                return new Response<Column>(colSL);
            }
            catch (Exception e)
            {
                log.Error("Move column Right has failed: " + e.Message);
                return new Response<Column>(e.Message);
            }
            
        }

        public Response<Column> MoveColumnLeft(string email, int columnOrdinal, string emailHost)
        {
            Column colSL;
            try
            {
                columnBL colBL = boardController.MoveColumnLeft(email, columnOrdinal, emailHost);
                string nameColumn = colBL.getColumnName();
                List<Task> tempList = new List<Task>();
                foreach (ITask tempTask in colBL.getAllTasks().Values)
                {
                    Task task1 = new Task(tempTask.GetID(), tempTask.GetCreationDate(), tempTask.GetDueDate(), tempTask.GetTitle(), tempTask.GetDescription(), tempTask.GetEmailAssignee());
                    tempList.Add(task1);
                }
                IReadOnlyCollection<Task> tasksCollectionReadOnly = tempList.AsReadOnly();
                colSL = new Column(tasksCollectionReadOnly, nameColumn, colBL.getMaxValue());
                return new Response<Column>(colSL);
            }
            catch (Exception e)
            {
                log.Error("Move Column left has failed: " + e.Message);
                return new Response<Column>(e.Message);
            }
        }

       

        public void CreateBoard(string email)
        {
            boardController.CreateBoard(email);
        }

    }
}
