using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    public class TaskDL : DalObject
    {
        public const string taskIDColumnName = "ID";
        public const string TitleColumnName = "Title";
        public const string DescColumnName = "Description";
        public const string ColumnOrdColumnName = "ColumnOrdinal";
        public const string CreationDColumnName = "CreationDate";
        public const string DueDColumnName = "DueDate";
        public const string HostColumnName = "HostEmail";

        private string _Email;
        public string Email { get { return _Email; } set { _controller.Update(_Email, TaskID, EmailColumnName, value); _Email = value;  } }
        private string _title;
        public string Title { get => _title; set { _title = value; bool res = _controller.Update(_Email, TaskID, TitleColumnName, value);  } }
        private string _desc;
        public string Description { get => _desc; set { _desc = value; bool res = _controller.Update(_Email, TaskID, DescColumnName, value); } }
        private int _colOrdinal;
        public int ColOrdinal { get => _colOrdinal; set { _colOrdinal = value; bool res = _controller.Update(_Email, TaskID, ColumnOrdColumnName, value.ToString()); } }
        private DateTime _creationD;
        public DateTime CreationD { get => _creationD; set { _creationD = value; bool res = _controller.Update(_Email, TaskID, CreationDColumnName, value.ToString());  } }
        private DateTime _dueD;
        public DateTime DueD { get => _dueD; set { _dueD = value; bool res = _controller.Update(_Email, TaskID, DueDColumnName, value.ToString());  } }
        private int _taskID;
        public int TaskID { get => _taskID; set { _taskID = value; } }
        private string _boardHost;
        public string BoardHost { get => _boardHost; set { _title = value; bool res = _controller.Update(_Email, TaskID, HostColumnName, value); } }

        public TaskDL(string _email, int id, string title, string desc, int colOrdinal, DateTime cDate, DateTime dDate, string hostEmail) : base(new TaskDalController())
        {
            _Email = _email;
            _taskID = id;
            _title = title;
            _desc = desc;
            _colOrdinal = colOrdinal;
            _creationD = cDate;
            _dueD = dDate;
            _boardHost = hostEmail;

        }

        public TaskDL() : base(new TaskDalController())
        {
            _Email = null;
            _taskID = 0;
            _title = null;
            _desc = null;
            _colOrdinal = 0;
            _creationD = DateTime.Now;
            _dueD = DateTime.Now;

        }

        public void Insert()
        {
            ((TaskDalController)_controller).Insert(this);
        }

        public void delete()
        {
            _controller.Delete(this._Email, this.TaskID);
        }

        public List<TaskDL> select(string email, int columnOrdinal)
        {
            return ((TaskDalController)_controller).SelectAllTaskByKey(email, columnOrdinal);
        }

        public void DeleteAllData()
        {
            ((TaskDalController)_controller).DeleteData();
        }

    }
}