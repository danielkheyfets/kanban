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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Presentation.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainViewModel viewModel;

        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = new MainViewModel();
            this.viewModel = (MainViewModel)DataContext;
        }
        public MainWindow(BackendController controller)
        {
            InitializeComponent();
            this.DataContext = new MainViewModel(controller);
            this.viewModel = (MainViewModel)DataContext;
        }


        private void Register_Click(object sender, RoutedEventArgs e)
        {
            RegisterWindow registerWindow = new RegisterWindow(viewModel.Controller);
            registerWindow.Show();
            this.Close();

        }

        private void Login_click(object sender, RoutedEventArgs e)
        {
            UserModel user = viewModel.Login();
            if (user != null)
            {
                BoardView boardView = new BoardView(user);
                boardView.Show();
                this.Close();
            }
        }

    }
}
