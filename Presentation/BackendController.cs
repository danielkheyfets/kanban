using IntroSE.Kanban.Backend.ServiceLayer;
using Presentation.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task = IntroSE.Kanban.Backend.ServiceLayer.Task;

namespace Presentation
{
    public class BackendController
    {
        private Service Service { get; set; }
        public BackendController(Service service)
        {
            this.Service = service;
        }



        public Response LoadData()
        {
            throw new NotImplementedException();
        }


        ///<summary>Remove all persistent data.</summary>
        public Response DeleteData()
        {
            throw new NotImplementedException();
        }



        public void Register(string email, string password, string nickname)
        {
            Response res = Service.Register(email, password, nickname);
            if (res.ErrorOccured)
            {
                throw new Exception(res.ErrorMessage);
            }

        }



        public void Register(string email, string password, string nickname, string emailHost)
        {
            Response res = Service.Register(email, password, nickname, emailHost);
            if (res.ErrorOccured)
            {
                throw new Exception(res.ErrorMessage);
            }
        }


        public void AssignTask(string email, int columnOrdinal, int taskId, string emailAssignee)
        {
            Response res = Service.AssignTask(email, columnOrdinal, taskId, emailAssignee);
            if (res.ErrorOccured)
                throw new Exception(res.ErrorMessage);
        }


        public void DeleteTask(string email, int columnOrdinal, int taskId)
        {
            Response res = Service.DeleteTask(email, columnOrdinal, taskId);
            if (res.ErrorOccured)
                throw new Exception(res.ErrorMessage);
        }


        public UserModel Login(string email, string password)
        {
            Response<User> user = Service.Login(email, password);
            if (user.ErrorOccured)
            {
                throw new Exception(user.ErrorMessage);
            }
            return new UserModel(this, email, user.Value.Nickname);
        }




        public void Logout(string email)
        {
            Response res = Service.Logout(email);
            if (res.ErrorOccured)
            {
                throw new Exception(res.ErrorMessage);
            }
        }


        public BoardModel GetBoard(string email)
        {
            Response<Board> board = Service.GetBoard(email);
            if (board.ErrorOccured)
                throw new Exception(board.ErrorMessage);
            ObservableCollection<ColumnModel> columns = new ObservableCollection<ColumnModel>();
            for (int i = 0; i < board.Value.ColumnsNames.Count; i++)
            {
                columns.Add(new ColumnModel(this, this.GetColumn(email, i), email));

            }

            return new BoardModel(this, email, columns);
        }



        public void LimitColumnTasks(string email, int columnOrdinal, int limit)
        {
            Response res = Service.LimitColumnTasks(email, columnOrdinal, limit);
            if (res.ErrorOccured)
            {
                throw new Exception(res.ErrorMessage);
            }
        }

        public void ChangeColumnName(string email, int columnOrdinal, string newName)
        {
            Response res = Service.ChangeColumnName(email, columnOrdinal, newName);
            if (res.ErrorOccured)
            {
                throw new Exception(res.ErrorMessage);
            }
        }


        public TaskModel AddTask(string email, string title, string description, DateTime dueDate)
        {
            Response<Task> task = Service.AddTask(email, title, description, dueDate);
            if (task.ErrorOccured)
                throw new Exception(task.ErrorMessage);
            return new TaskModel(this, task.Value.Id, task.Value.CreationTime, dueDate, title, description, email, 0, email);
        }


        public void UpdateTaskDueDate(string email, int columnOrdinal, int taskId, DateTime dueDate)
        {
            Response res = Service.UpdateTaskDueDate(email, columnOrdinal, taskId, dueDate);
            if (res.ErrorOccured)
                throw new Exception(res.ErrorMessage);
        }


        public void UpdateTaskTitle(string email, int columnOrdinal, int taskId, string title)
        {
            Response res = Service.UpdateTaskTitle(email, columnOrdinal, taskId, title);
            if (res.ErrorOccured)
                throw new Exception(res.ErrorMessage);
        }


        public void UpdateTaskDescription(string email, int columnOrdinal, int taskId, string description)
        {
            Response res = Service.UpdateTaskDescription(email, columnOrdinal, taskId, description);

            if (res.ErrorOccured)
                throw new Exception(res.ErrorMessage);
        }


        public void AdvanceTask(string email, int columnOrdinal, int taskId)
        {
            Response res = Service.AdvanceTask(email, columnOrdinal, taskId);
            if (res.ErrorOccured)
                throw new Exception(res.ErrorMessage);
        }



        public Response<Column> GetColumn(string email, string columnName)
        {
            throw new NotImplementedException();
        }


        public ColumnModel GetColumn(string email, int columnOrdinal)
        {
            Response<Column> column = Service.GetColumn(email, columnOrdinal);
            ObservableCollection<TaskModel> tasks = new ObservableCollection<TaskModel>();
            foreach (Task task in column.Value.Tasks)
            {
                tasks.Add(new TaskModel(this, task.Id, task.CreationTime, task.DueDate, task.Title, task.Description, task.emailAssignee, columnOrdinal, email));
            }
            return new ColumnModel(this, columnOrdinal, column.Value.Name, column.Value.Limit, email, tasks);
        }


        public void RemoveColumn(string email, int columnOrdinal)
        {
            Response res = Service.RemoveColumn(email, columnOrdinal);
            if (res.ErrorOccured)
                throw new Exception(res.ErrorMessage);
        }


        public ColumnModel AddColumn(string email, int columnOrdinal, string Name)
        {
            Response<Column> column = Service.AddColumn(email, columnOrdinal, Name);
            if (column.ErrorOccured)
                throw new Exception(column.ErrorMessage);
            return new ColumnModel(this, columnOrdinal, column.Value.Name, column.Value.Limit, email, GetTasks(column.Value.Tasks, columnOrdinal, email));
        }


        public ColumnModel MoveColumnRight(string email, int columnOrdinal)
        {
            Response<Column> column = Service.MoveColumnRight(email, columnOrdinal);;
            if (column.ErrorOccured)
                throw new Exception(column.ErrorMessage);
            return new ColumnModel(this, columnOrdinal + 1, column.Value.Name, column.Value.Limit, email, GetTasks(column.Value.Tasks, columnOrdinal, email));

        }


        public ColumnModel MoveColumnLeft(string email, int columnOrdinal)
        {
            Response<Column> column = Service.MoveColumnLeft(email, columnOrdinal);
            if (column.ErrorOccured)
                throw new Exception(column.ErrorMessage);
            return new ColumnModel(this, columnOrdinal - 1, column.Value.Name, column.Value.Limit, email, GetTasks(column.Value.Tasks, columnOrdinal, email));
        }

        private ObservableCollection<TaskModel> GetTasks(IReadOnlyCollection<Task> serviceTasks, int columnOrdinal, string email)
        {
            ObservableCollection<TaskModel> tasks = new ObservableCollection<TaskModel>();
            foreach (Task task in serviceTasks)
            {

                tasks.Add(new TaskModel(this, task.Id, task.CreationTime, task.DueDate, task.Title, task.Description, task.emailAssignee, columnOrdinal, email));
            }
            return tasks;
        }
    }
}
