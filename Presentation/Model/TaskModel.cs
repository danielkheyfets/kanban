using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Presentation.Model
{
    public class TaskModel : NotifiableModelObject
    {
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

        private DateTime _creationTime;
        public DateTime CreationDate
        {
            get => _creationTime;
            set
            {
                this._creationTime = value;
                RaisePropertyChanged("CreationDate");

            }
        }

        private string email;
        public SolidColorBrush BackgroundColor
        {
            get
            {
                if (DateTime.Compare(DateTime.Now,DueDate) > 0)
                {
                    return new SolidColorBrush(Colors.Red);
                }

                else
                {
                    double double_minutesfull = DueDate.Subtract(CreationDate).TotalMinutes;
                    double double_minutesNow = DateTime.Now.Subtract(CreationDate).TotalMinutes;
                    if (double_minutesfull * 0.75 < double_minutesNow)
                    {

                        return new SolidColorBrush(Colors.Orange);

                    }
                    return new SolidColorBrush(Colors.White);
                }
            }
        }


        public SolidColorBrush BorderColor
        {
            get
            {
                return new SolidColorBrush(EmailAssignee.Contains(email) ? Colors.Blue : Colors.Black);
            }
        }

        private DateTime _dueDate;
        public DateTime DueDate
        {
            get => _dueDate;
            set
            {
                Controller.UpdateTaskDueDate(_emailAssignee, _colOrdinal, _id, value);
                this._dueDate = value;
                RaisePropertyChanged("DueDate");
            }
        }

        private string _title;
        public string Title
        {
            get => _title;
            set
            {
                Controller.UpdateTaskTitle(_emailAssignee, _colOrdinal, _id, value); 
                this._title = value;

                RaisePropertyChanged("Title");
            }
        }
        private string _description;
        public string Description
        {
            get => _description;
            set
            {
                Controller.UpdateTaskDescription(_emailAssignee, _colOrdinal, _id, value);
                this._description = value;

                RaisePropertyChanged("Description");
            }
        }
        private string _emailAssignee;
        public string EmailAssignee
        {
            get => _emailAssignee;
            set
            {
                if (_emailAssignee != null)
                    Controller.AssignTask(_emailAssignee, _colOrdinal, _id, value);
                this._emailAssignee = value;
                RaisePropertyChanged("EmailAssignee");
            }
        }


        private int _colOrdinal;
        public int ColOrdinal
        {
            get => _colOrdinal;
            set
            {
                this._colOrdinal = value;
                RaisePropertyChanged("EmailAssignee");
            }
        }

        public TaskModel(BackendController controller, int id, DateTime creation, DateTime due, string title, string description, string emailAssignee, int columnOrdinal, string email) : base(controller)
        {
            this.email = email;
            _emailAssignee = emailAssignee;
            _id = id;
            _colOrdinal = columnOrdinal;          
            _dueDate = due;
            _creationTime = creation;
            _title = title;
            _description = description;
        }

    }
}
