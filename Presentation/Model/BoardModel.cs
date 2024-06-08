using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Model
{
    public class BoardModel : NotifiableModelObject
    {

        public ObservableCollection<ColumnModel> Columns { get; set; }
        private string Email;


        public BoardModel(BackendController controller, string email) : base(controller)
        {
            this.Email = email;
            Columns = Controller.GetBoard(email).Columns;
        }

        public BoardModel(BackendController controller, string email, ObservableCollection<ColumnModel> columns) : base(controller)
        {
            this.Email = email;
            Columns = columns;
        }


        public void SortByDueDate()
        {

            foreach (ColumnModel col in Columns)
            {
                col.sortByDueDate();
            }

        }

        public void RemoveColumn(ColumnModel toRemove)
        {
            Controller.RemoveColumn(this.Email, toRemove.Id);
            refresh(Controller.GetBoard(this.Email).Columns);
        }

        public void RemoveTask(TaskModel toRemove)
        {

            Controller.DeleteTask(this.Email, toRemove.ColOrdinal, toRemove.Id);
            refresh(Controller.GetBoard(this.Email).Columns);
        }

        public void FilterBy(string filter)
        {

            ObservableCollection<ColumnModel> filterColumns = new ObservableCollection<ColumnModel>();

            foreach (ColumnModel col in Columns)
            {
                ObservableCollection<TaskModel> tasksFilred = new ObservableCollection<TaskModel>();


                foreach (TaskModel taskModel in col.Tasks)
                {
                    if (taskModel.Description.Contains(filter) || taskModel.Title.Contains(filter))

                    {

                        tasksFilred.Add(taskModel);
                    }
                }
                ColumnModel column = new ColumnModel(this.Controller, col.Id, col.Name, col.Limit, Email, tasksFilred);
                filterColumns.Add(column);
            }
            refresh(filterColumns);
        }



        public void MoveRight(ColumnModel column)
        {
            ColumnModel moved = Controller.MoveColumnRight(this.Email, column.Id);
            refresh(Controller.GetBoard(this.Email).Columns);

        }

        //refresh all the columns in the board view
        private void refresh(ObservableCollection<ColumnModel> ColumnsRefresh)
        {
            Columns.Clear();
            foreach (ColumnModel column in ColumnsRefresh)
            {
                Columns.Add(column);
            }

        }

        public void refresh()
        {
            refresh(Controller.GetBoard(Email).Columns);
        }

        public void MoveLeft(ColumnModel column)
        {
            ColumnModel moved = Controller.MoveColumnLeft(this.Email, column.Id);
            refresh(Controller.GetBoard(this.Email).Columns);
        }


        public void advance(TaskModel task)
        {
            Controller.AdvanceTask(this.Email, task.ColOrdinal, task.Id);
            refresh(Controller.GetBoard(this.Email).Columns);
        }


        public void ArrangeColumns()
        {
            int counter = 0;
            foreach (ColumnModel col in Columns)
            {
                col.Id = counter;
                counter++;
            }
        }

    }
}