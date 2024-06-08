using Presentation.Model;
using Presentation.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.ViewModel
{
    public class TaskInfoViewModel : NotifiableObject
    {
       private TaskModel task;
        private UserModel user;



    
        private string _titleTask = "";
        public string TitleTask
        {
            get => _titleTask;
            set
            {
                _titleTask = value;
                RaisePropertyChanged("TitleTask");
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

        private string _dueDate = "";
        public string DueDate
        {
            get => _dueDate;
            set
            {
                _dueDate = value;
                RaisePropertyChanged("DueDate");
            }
        }

        private string _creationDate = "";
        public string CreationDate
        {
            get => _creationDate;
            set
            {
                _creationDate = value;
                RaisePropertyChanged("CreationDate");
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


        public void EditTask()
        {
            EditTaskView editTaskWindow = new EditTaskView(task, user);
            editTaskWindow.Show();
        }



        public TaskInfoViewModel(TaskModel task,UserModel user)
        {
            this.user = user;
            this.task = task;
            refresh();
        }



        public void refresh()
        {
            Description = task.Description;
            TitleTask = task.Title;
            DueDate = task.DueDate.ToString();
            AssigneeEmail = task.EmailAssignee;
            CreationDate = task.CreationDate.ToString();
        }




    }
}
