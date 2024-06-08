using Presentation.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.ViewModel
{
    public class ColumnViewModel : NotifiableObject
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

        private string _columnOrdinal;
        public string ColumnOrdinal
        {
            get
            {
                return _columnOrdinal;
            }
            set
            {
                _columnOrdinal = value;
                RaisePropertyChanged("ColumnOrdinal");
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


        public ColumnViewModel(UserModel user, BoardModel board)
        {
            this.user = user;
            this.board = board;
        }


        public bool AddColumn()
        {
            if (_columnOrdinal == null || _name == null)
            {
                Message = "please Fill all fields";
                return false;
            }
            else
            {
                Message = "";
                try
                {
                    ColumnModel column = user.Controller.AddColumn(user.Email, Int32.Parse(ColumnOrdinal), Name);
                    board.Columns.Insert(Int32.Parse(ColumnOrdinal), column);
                    board.ArrangeColumns();
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
}
