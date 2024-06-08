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
    /// Interaction logic for ColumnEditVIEW.xaml
    /// </summary>
    public partial class ColumnEditVIEW : Window
    {
        public ColumnEditViewModel viewModel;

        public ColumnEditVIEW(UserModel user, BoardModel board, int columnOrdinal)
        {
            InitializeComponent();
            this.viewModel = new ColumnEditViewModel(user, board, columnOrdinal);
            this.DataContext = viewModel;
        }


        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void NameSave_Click(object sender, RoutedEventArgs e)
        {
            if (viewModel.ChangeName())
                this.Close();
        }

        private void LimitSave_Click(object sender, RoutedEventArgs e)
        {
            if (viewModel.LimitTasks())
                this.Close();
        }
    }
}
