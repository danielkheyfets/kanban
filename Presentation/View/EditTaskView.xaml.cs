using Presentation.Model;
using Presentation.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Presentation.View
{
    /// <summary>
    /// Interaction logic for EditTaskView.xaml
    /// </summary>
    public partial class EditTaskView : Window
    {
        EditTaskViewModel viewModel;
        public EditTaskView(TaskModel taskModel, UserModel user)
        {
            InitializeComponent();
            this.viewModel = new EditTaskViewModel(taskModel, user);
            this.DataContext = viewModel;
        }
        private void DescriptionClick(object sender, EventArgs e)
        {

            bool x = viewModel.setDescription();
            if (x == true)
            {
                this.Close();
            }

        }
        private void titleClick(object sender, EventArgs e)
        {

            bool x = viewModel.setTitle();
            if (x == true)
            {
                this.Close();
            }

        }
        private void AssignClick(object sender, EventArgs e)
        {

            bool x = viewModel.Assign();
            if (x == true)
            {
                this.Close();
            }

        }

        private void DueDateClick(object sender, EventArgs e)
        {

            bool x = viewModel.setDueDate();
            if (x == true)
            {
                this.Close();
            }

        }
        public void Cancel_click(object sender, RoutedEventArgs e) { this.Close(); }

    }
}
