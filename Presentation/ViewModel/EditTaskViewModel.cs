using Presentation.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.ViewModel
{
    public class EditTaskViewModel : NotifiableObject
    {
        TaskModel taskModel;
        UserModel userModel;
        public EditTaskViewModel(TaskModel taskModel, UserModel userModel)
        {
            this.taskModel = taskModel;
            this.userModel = userModel;
            DueDate = DateTime.Now;


        }

        private string _titleTask = "";
        public string Title
        {
            get => _titleTask;
            set
            {
                _titleTask = value;
                RaisePropertyChanged("Title");
            }
        }



        private string _assigneeEmail = "";
        public string AssigneeEmail
        {
            get => _assigneeEmail;
            set
            {
                _assigneeEmail = value;
                RaisePropertyChanged("AssigneeEmail");
            }
        }

        private DateTime _dueDate;
        public DateTime DueDate
        {
            get => _dueDate;
            set
            {
                _dueDate = value;
                RaisePropertyChanged("DueDate");
            }
        }


        private string _description = "";
        public string Description
        {
            get => _description;
            set
            {
                _description = value;
                RaisePropertyChanged("Description");
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


        public bool setDescription()
        {
            Message = "";
            try
            {
                taskModel.Description = Description;
                return true;
            }
            catch (Exception e)
            {
                Message = e.Message;
                return false;
            }
        }

        public bool setTitle()
        {
            Message = "";
            try
            {
                taskModel.Title = Title;
                return true;
            }
            catch (Exception e)
            {
                Message = e.Message;
                return false;
            }
        }


        public bool setDueDate()
        {
            Message = "";
            try
            {
                taskModel.DueDate = DueDate;
                return true;
            }
            catch (Exception e)
            {
                Message = e.Message;
                return false;
            }
        }
        public bool Assign()
        {
            Message = "";
            try
            {
                taskModel.EmailAssignee = AssigneeEmail;
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
