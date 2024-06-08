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
    /// Interaction logic for TaskView.xaml
    /// </summary>
    public partial class TaskView : Window
    {
        TaskViewModel taskViewModel;
        // int boardID;

        public TaskView(UserModel userModel, BoardModel boardModel)
        {
            InitializeComponent();
            this.taskViewModel = new TaskViewModel(userModel, boardModel);
            this.DataContext = taskViewModel;
        }

        public void Save_click(object sender, RoutedEventArgs e)
        {
            if (taskViewModel.AddTask())
            {
                this.Close();
            }
        }

        public void Cancel_click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }



    }
}
