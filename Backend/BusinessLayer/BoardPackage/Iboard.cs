using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.BusinessLayer
{
    public interface Iboard
    {
        ///<summary>This method return the email of the use.</summary>
        string GetUserEmail();


        ///<summary>This method advance the correct task to the next colunm .</summary>
        ///<param name="columnOrdinal">The current column id where the task is located .</param>
        ///<param name="takID">taskID.</param>
        void Advance(int columnOrdinal, int takID, string email);


        ///<summary>this method return column by id .</summary>
        ///<param name="columnOrdinal">colum id .</param>

        //columnBL GetColumn(int columnOrdinal);

        void addTask(String email, String title, String description, DateTime dueDate, int taskId);

    }
}
