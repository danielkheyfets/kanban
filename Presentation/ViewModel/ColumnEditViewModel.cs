using Presentation.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.ViewModel
{
    public class ColumnEditViewModel : NotifiableObject
    {
        public UserModel user;
        public BoardModel board;
        private String _name;
        public String Name
        {
            get
            {
                return this._name;
            }
            set
            {
                _name = value;
                RaisePropertyChanged("Name");

            }
        }

        private string _limit;
        public string Limit
        {
            get
            {
                return _limit;
            }
            set
            {
                _limit = value;
                RaisePropertyChanged("Limit");
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

        private int _columnOrdinal;
        public int Col
        {
            get => _columnOrdinal;
            set
            {
                this._columnOrdinal = value;
                RaisePropertyChanged("Col");
            }
        }

        public ColumnEditViewModel(UserModel user, BoardModel board, int columnOrdinal)
        {
            this.user = user;
            this.board = board;
            _columnOrdinal = columnOrdinal;
        }

        public bool LimitTasks()
        {
            Message = "";
            try
            {
                user.Controller.LimitColumnTasks(user.Email, _columnOrdinal, Int32.Parse(Limit));
                board.Columns.ElementAt(_columnOrdinal).Limit = Int32.Parse(Limit);
                return true;
            }
            catch (Exception e)
            {
                Message = e.Message;
                return false;
            }
        }

        public bool ChangeName()
        {
            Message = "";
            try
            {
                user.Controller.ChangeColumnName(user.Email, _columnOrdinal, Name);
                board.Columns.ElementAt(_columnOrdinal).Name = Name;
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
