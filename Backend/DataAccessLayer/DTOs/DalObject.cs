
using System;

namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    public class DalObject
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public const string EmailColumnName = "Email";
        protected DalController _controller;

       // public string email { get; set; } = "";

        public DalObject()
        {

        }
        public DalObject(DalController controller)
        {
            _controller = controller;
        }



    }
}