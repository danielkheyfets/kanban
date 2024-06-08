using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    public class ColumnDL : DalObject
    {
        public const string ColumnOrdinalColumnName = "ID";
        public const string ColumnNameColumnName = "ColumnName";
        public const string MaxValueColumnName = "MaxValue";
        public const string CurrNumTasksColumnName = "CurrNumOfTasks";

        private string _Email;
        public string Email { get { return _Email; } set { _Email = value; } }
        private int _columnOrdinal;
        public int ColumnOrdinal { get => _columnOrdinal; set { bool res = _controller.Update(_Email, _columnOrdinal, ColumnOrdinalColumnName, value.ToString());  ; _columnOrdinal = value; } }
        private string _columnName;
        public string ColumnName { get => _columnName; set { _columnName = value; bool res = _controller.Update(_Email, _columnOrdinal, ColumnNameColumnName, value); } }
        private int _maxValue;
        public int MaxTasks { get => _maxValue; set { _maxValue = value; bool res = _controller.Update(_Email, _columnOrdinal, MaxValueColumnName, value.ToString()); } }
        private int _currNumTasks;
        public int CurrNOTasks { get => _currNumTasks; set { _currNumTasks = value; bool res = _controller.Update(_Email, _columnOrdinal.ToString(), CurrNumTasksColumnName, value);  } }


        public ColumnDL(string _email, int ordinal, string colName, int maxVal, int currNumOfTasks) : base(new ColumnDalController())
        {
            _Email = _email;
            _columnOrdinal = ordinal;
            _columnName = colName;
            _maxValue = maxVal;
            _currNumTasks = currNumOfTasks;
        }

        public ColumnDL() : base(new ColumnDalController())
        {
            _Email = null;
            _columnOrdinal = 0;
            _columnName = null;
            _maxValue = 0;
            _currNumTasks = 0;
        }


        public void Insert()
        {
            ((ColumnDalController)_controller).Insert(this);
        }

        public void delete()
        {
            _controller.Delete(this._Email, this.ColumnOrdinal);
        }

        public List<ColumnDL> select(string email)
        {
            return ((ColumnDalController)_controller).SelectAllColumnsByUser(email);
        }

        public void DeleteAllData()
        {
            ((ColumnDalController)_controller).DeleteData();
        }

    }
}
