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
    /// Interaction logic for TaskInfo.xaml
    /// </summary>
    public partial class TaskInfo : Window
    {
        public TaskInfoViewModel viewModel;

        public TaskInfo(TaskModel task,UserModel userModel)
        {
            InitializeComponent();
            this.viewModel = new TaskInfoViewModel(task,userModel);
            this.DataContext = viewModel;
        }


        private void EditTask_Click(object sender, RoutedEventArgs e)
        {
            viewModel.EditTask();
            this.Close();
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
