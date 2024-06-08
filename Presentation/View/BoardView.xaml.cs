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
    /// Interaction logic for BoardView.xaml
    /// </summary>
    public partial class BoardView : Window
    {
        public BoardViewModel viewModel;

        public BoardView(UserModel u)
        {
            InitializeComponent();
            this.viewModel = new BoardViewModel(u);
            this.DataContext = viewModel;
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            viewModel.Logout();
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }


        private void AddTask_Click(object sender, RoutedEventArgs e)
        {
            TaskView taskWindow = new TaskView(viewModel.user, viewModel.Board);
            taskWindow.Show();
            viewModel.EnableForward = false;


        }

        private void AddColumn_Click(object sender, RoutedEventArgs e)
        {
            AddColumnView columnWindow = new AddColumnView(viewModel.user, this.viewModel.Board);
            columnWindow.Show();
            viewModel.EnableForward = false;

        }

        private void TaskInfo_Click(object sender, RoutedEventArgs e)
        {
            viewModel.TaskInfo();
            viewModel.EnableForward = false;
            list.SelectedIndex = -1;
        }


        private void EditColumn_Click(object sender, RoutedEventArgs e)
        {
            viewModel.EditColumn();
            list.SelectedIndex = -1;
        }


        private void DeleteTask_Click(object sender, RoutedEventArgs e)
        {
            viewModel.DeleteTask();
        }

        private void DeleteColumn_Click(object sender, RoutedEventArgs e)
        {
            viewModel.RemoveColumn();
            viewModel.EnableForward = false;
            list.SelectedIndex = -1;
        }

        private void MoveRight_Click(object sender, RoutedEventArgs e)
        {
            viewModel.MoveRight();
            viewModel.EnableForward = false;
            list.SelectedIndex = -1;
        }

        private void MoveLeft_Click(object sender, RoutedEventArgs e)
        {
            viewModel.MoveLeft();
            viewModel.EnableForward = false;
            list.SelectedIndex = -1;
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            viewModel.UpdateMyTask();
        }

        //private void ListBox_SelectionChangedColumn(object sender, SelectionChangedEventArgs e)
        //{
        //    if(list.SelectedIndex != -1 && != -1)
        //    {

        //    }
        //}

        private void OnContainerFocused(object sender, RoutedEventArgs e)
        {
            (sender as ListBoxItem).IsSelected = true;
        }

        private void ListBox_Unselected(object sender, RoutedEventArgs e)
        {
            viewModel.Description = "";
        }

        private void Advance_Click(object sender, RoutedEventArgs e)
        {
            viewModel.Advance();
            list.SelectedIndex = -1;
        }

        private void SerchButton_Click(object sender, RoutedEventArgs e)
        {
            viewModel.filterBy();
        }

        private void StopSerchButton_Click(object sender, RoutedEventArgs e)
        {
            viewModel.StopFilterBy();
        }

        private void SortByDueDate(object sender, RoutedEventArgs e)
        {
            viewModel.SortByDueDate();
        }

    }
}
