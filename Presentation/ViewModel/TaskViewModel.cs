using Presentation.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.ViewModel
{
    class TaskViewModel : NotifiableObject
    {
        private UserModel user;
        private BoardModel board;

        private string _title = "";
        public string Title
        {
            get
            {
                return _title;
            }
            set
            {
                _title = value;
                RaisePropertyChanged("Title");
            }
        }

        private string _description = "";
        public string Description
        {
            get
            {
                return _description;
            }
            set
            {
                _description = value;
                RaisePropertyChanged("Description");
            }
        }


        private DateTime _dueDate=DateTime.Now;
        public DateTime DueDate
        {
            get => _dueDate;
            set
            {
                _dueDate = value;
                RaisePropertyChanged("DueDate");
            }
        }



        private string _message;
        public string Message
        {
            get => _message;
            set
            {
                this._message = value;
                RaisePropertyChanged("Message");
            }
        }

        private bool _enableForward = false;
        public bool EnableForward
        {
            get => _enableForward;
            private set
            {
                _enableForward = value;
                RaisePropertyChanged("EnableForward");
            }
        }

        public TaskViewModel(UserModel user, BoardModel board)
        {
            this.user = user;
            this.board = board;
        }

        public bool AddTask()
        {
            Message = "";
            try
            {
                TaskModel task = user.Controller.AddTask(user.Email, this.Title, this.Description, this.DueDate);
                board.refresh();
                return true;

            }
            catch (Exception e)
            {
                Message = e.Message;
                return false;

            }
        }


    }
}
