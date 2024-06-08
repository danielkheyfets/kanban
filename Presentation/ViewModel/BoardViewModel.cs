using Presentation.Model;
using Presentation.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Presentation.ViewModel
{
    public class BoardViewModel : NotifiableObject
    {
        public UserModel user;

        public BoardModel Board { get; private set; }
        public string Title { get; private set; }

        private ColumnModel _selectedColumn;

        public ColumnModel SelectedColumn
        {
            get
            {
                return _selectedColumn;
            }
            set
            {
                _selectedColumn = value;
                RaisePropertyChanged("SelectedColumn");
            }
        }


        public bool _enableForward;
        public bool EnableForward
        {
            get => _enableForward;
            set
            {
                _enableForward = value;
                RaisePropertyChanged("EnableForward");
            }
        }

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


        private bool _isEnableFiltreBot = true;
        public bool isEnableFiltreBot
        {
            get => _isEnableFiltreBot;
            set
            {
                if (value == false)
                {
                    _isEnableFiltreBot = value;
                    isEnableStopFiltreBot = true;
                }
                else
                {
                    _isEnableFiltreBot = value;

                }
                RaisePropertyChanged("isEnableFiltreBot");
            }
        }



        private bool _isEnableStopFiltreBot = false;
        public bool isEnableStopFiltreBot
        {
            get => _isEnableStopFiltreBot;
            set
            {

                if (value == false)
                {
                    isEnableFiltreBot = true;
                    _isEnableStopFiltreBot = value;
                }
                else
                {
                    _isEnableStopFiltreBot = value;

                }
                RaisePropertyChanged("isEnableStopFiltreBot");
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



        private string _SearchTerm;
        public string SearchTerm
        {
            get => _SearchTerm;
            set
            {
                this._SearchTerm = value;
                RaisePropertyChanged("SearchTerm");
            }
        }


        public void filterBy()
        {
            Message = "";
            if (SearchTerm == null || SearchTerm.Equals(""))
            {
                Message = "please insert the filter";
            }
            else
            {
                Board.FilterBy(SearchTerm);
                isEnableFiltreBot = false;
            }
        }


        public void StopFilterBy()
        {
            SearchTerm = "";
            Board.refresh();
            isEnableStopFiltreBot = false;
        }



        internal bool Logout()
        {
            Message = "";
            try
            {
                user.Controller.Logout(user.Email);
                return true;
            }
            catch (Exception e)
            {
                Message = e.Message;
                return false;
            }
        }


        public BoardViewModel(UserModel user)
        {

            this.user = user;
            Title = "Welcome " + user.Nickname; 
            Board = user.GetBoard();
        }

        public void DeleteTask()
        {
            if (isEnableStopFiltreBot)
            {
                Message = "stop the filter";
            }
            else
            {
                if (SelectedColumn == null || SelectedColumn.SelectedTask == null)
                {
                    Message = "you need to select task";
                }
                else
                {
                    Message = "";
                    try
                    {
                        Board.RemoveTask(SelectedColumn.SelectedTask);
                        setAllempty();

                    }
                    catch (Exception e)
                    {
                        Message = e.Message;
                    }
                }
            }
        }

        private void setAllempty()
        {
            Description = "";
            TitleTask = "";
            DueDate = "";
            AssigneeEmail = "";
            CreationDate = "";

        }

        public void RemoveColumn()
        {
            if (isEnableStopFiltreBot)
            {
                Message = "stop the filter";
            }
            else
            {
                if (SelectedColumn == null)
                {
                    Message = "you need to select a column";
                }
                else
                {
                    Message = "";
                    try
                    {
                        Board.RemoveColumn(SelectedColumn);
                    }
                    catch (Exception e)
                    {
                        Message = e.Message;
                    }
                }
            }
        }

        public void TaskInfo()
        {
            Message = "";
            if (SelectedColumn == null || SelectedColumn.SelectedTask == null)
            {
                Message = "please select a task";
            }
            else
            {
                TaskInfo taskInfo = new TaskInfo(SelectedColumn.SelectedTask, user);
                taskInfo.Show();
                setAllempty();
                SelectedColumn.SelecteIndex = -1;

            }
        }

        public void EditColumn()
        {
            if (SelectedColumn == null)
            {
                Message = "please select a column";
            }
            else
            {

                ColumnEditVIEW columnWindow = new ColumnEditVIEW(user, Board, SelectedColumn.Id);
                columnWindow.Show();
            }
        }
        public void UpdateMyTask()
        {
            if (SelectedColumn != null && SelectedColumn.SelectedTask != null)
            {
                Description = SelectedColumn.SelectedTask.Description;
                TitleTask = SelectedColumn.SelectedTask.Title;
                DueDate = SelectedColumn.SelectedTask.DueDate.ToString();
                AssigneeEmail = SelectedColumn.SelectedTask.EmailAssignee;
                CreationDate = SelectedColumn.SelectedTask.CreationDate.ToString();
            }
            else
            {
                setAllempty();
            }
        }

        public void MoveRight()
        {
            if (isEnableStopFiltreBot)
            {
                Message = "stop the filter";
            }
            {
                if (SelectedColumn == null)
                {
                    Message = "please select a column to move";
                }
                else
                {
                    Message = "";
                    try
                    {
                        Board.MoveRight(SelectedColumn);
                    }
                    catch (Exception e)
                    {
                        Message = e.Message;
                    }
                }
            }
        }

        public void Advance()
        {
            if (isEnableStopFiltreBot)
            {
                Message = "stop the filter";
            }
            else
            {
                if (SelectedColumn == null || SelectedColumn.SelectedTask == null)
                {
                    Message = "please select a task to advance";
                }
                else
                {
                    Message = "";
                    try
                    {
                        Board.advance(SelectedColumn.SelectedTask);
                    }
                    catch (Exception e)
                    {
                        Message = e.Message;
                    }
                }
            }
        }

        public void SortByDueDate()
        {
            Board.SortByDueDate();
        }



        public void MoveLeft()
        {
            if (isEnableStopFiltreBot)
            {
                Message = "stop the filter";
            }
            else
            {
                if (SelectedColumn == null)
                {
                    Message = "please select a column to move";
                }
                else
                {
                    Message = "";
                    try
                    {
                        Board.MoveLeft(SelectedColumn);
                    }
                    catch (Exception e)
                    {
                        Message = e.Message;
                    }
                }
            }
        }

    }
}
