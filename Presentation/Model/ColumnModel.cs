using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Model
{
    public class ColumnModel : NotifiableModelObject
    {
        public ObservableCollection<TaskModel> Tasks { get; set; }
        private string Email;
        private int _id;
        public int Id
        {
            get => _id;
            set
            {
                this._id = value;
                RaisePropertyChanged("Id");
            }
        }

        public void sortByDueDate()
        {
            List<TaskModel> listtasks = new List<TaskModel>(Tasks.ToList());
            listtasks.Sort((x, y) => x.DueDate.CompareTo(y.DueDate));
            Tasks.Clear();
            foreach (TaskModel task in listtasks)
            {
                Tasks.Add(task);

            }

        }

        private TaskModel _SelectedTask;
        public TaskModel SelectedTask
        {
            get
            {
                return _SelectedTask;
            }
            set
            {
                _SelectedTask = value;
                RaisePropertyChanged("SelectedTask");
            }
        }

        public int _SelecteIndex;
        public int SelecteIndex
        {
            get
            {
                return _SelecteIndex;
            }
            set
            {
                _SelecteIndex = value;
                RaisePropertyChanged("SelecteIndex");
            }
        }

        private string _name;
        public string Name
        {
            get => _name;
            set
            {
                this._name = value;
                RaisePropertyChanged("Name");
            }
        }
        private int _limit;
        public int Limit
        {
            get => _limit;
            set
            {
                this._limit = value;
                RaisePropertyChanged("Limit");
            }
        }


        public ColumnModel(BackendController controller, int id, string name, int limit, string email, ObservableCollection<TaskModel> tasks) : base(controller)
        {
            _id = id;
            _name = name;
            Email = email;
            _limit = limit;
            Tasks = tasks;

        }

        public ColumnModel(BackendController controller, ColumnModel col, string email) : this(controller, col.Id, col.Name, col.Limit, email, controller.GetColumn(email, col.Id).Tasks) { }

    }
}
