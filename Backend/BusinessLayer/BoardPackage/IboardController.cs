using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.BusinessLayer
{
    public interface IboardController
    {
        ///<summary>This method add a new task to the board .</summary>

        int addTask(String email, String title, String description, DateTime dueDate);

        ///<summary>This method advanse task by id.</summary>

        void AdvanceTask(string email, int columnOrdinal, int taskId);

        ///<summary>this method limit the max value in the column.</summary>

        void LimitColumnTasks(string email, int columnOrdinal, int limit, string emailHost);

        ///<summary>this method update the title of the task.</summary>

        void UpdateTaskTitle(string email, int columnOrdinal, int taskId, string title);

        ///<summary>this method update the description of the task.</summary>

        void UpdateTaskDescription(string email, int columnOrdinal, int taskId, string description);

        ///<summary>this method update the due date of the task.</summary>

        void UpdateTaskDueDate(string email, int columnOrdinal, int taskId, DateTime dueDate);

        ///<summary>this method import from the data.</summary>

        void Import();

        ///<summary>this method return column by column ordinal.</summary>

        columnBL getColumn(string email, int columnOrdinal);

        ///<summary>this method return column by column name.</summary>

        columnBL getColumnByName(string email, string columnName);

        ///<summary>this method return board by user email.</summary>

        boardBL getBoard(string email);

        ///<summary>this method move column left.</summary>

        columnBL MoveColumnLeft(string email, int columnOrdinal, string emailHost);

        ///<summary>this method move column right.</summary>

        columnBL MoveColumnRight(string email, int columnOrdinal, string emailHost);

        ///<summary>this method add column.</summary>

        columnBL AddColumn(string email, int columnOrdinal, string Name, string emailHost);

        ///<summary>this method remove column.</summary>

        void RemoveColumn(string email, int columnOrdinal, string emailHost);

    }
}
