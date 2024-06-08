using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    public class BoardDL : DalObject
    {
        public const string NextTaskID = "NextTaskId";

        private int _nexID;
        public int NextID { get { return _nexID; } set { _nexID = value; bool res = _controller.Update(_Email, NextTaskID, value.ToString());  } }

        private string _Email;
        public string Email { get { return _Email; } set { _Email = value; } }

        public BoardDL(string email, int nextID) : base(new BoardDalController())
        {
            _nexID = nextID;
            _Email = email;
        }
        public BoardDL() : base(new BoardDalController())
        {
        }

        public void Insert()
        {

            ((BoardDalController)_controller).Insert(this);
        }

        public List<BoardDL> select()
        {
            return ((BoardDalController)_controller).SelectAllBoards();
        }

        public void DeleteAllData()
        {
            ((BoardDalController)_controller).DeleteData();
        }

    }
}
