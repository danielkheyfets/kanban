using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.BusinessLayer
{
    public interface Icolumn
    {
        void addTask(String email, String title, String description, DateTime dueDate, int taskId);

        void addTask(ITask task);

        bool reachedMax();

        bool hasTask(int taskID);

        bool setMax(int newMax);

        ITask getTask(int taskID);

        bool removeTask(int taskID);

        Dictionary<int, ITask> getAllTasks();

    }
}
