using Presentation.Model;
using Presentation.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Interaction logic for 
    /// addColumnView.xaml
    /// </summary>
    public partial class AddColumnView : Window
    {
        public ColumnViewModel viewModel;

        public AddColumnView(UserModel user, BoardModel board)
        {
            InitializeComponent();
            this.viewModel = new ColumnViewModel(user, board);
            this.DataContext = viewModel;

        }

        public void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (viewModel.AddColumn())
                this.Close();

        }
    }
}

