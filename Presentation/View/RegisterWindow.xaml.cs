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
    /// Interaction logic for RegisterWindow.xaml
    /// </summary>
    public partial class RegisterWindow : Window
    {
        private RegisterViewModel registerViewModel;

        public RegisterWindow(BackendController controller)
        {
            InitializeComponent();
            this.DataContext = new RegisterViewModel(controller);
            this.registerViewModel = (RegisterViewModel)DataContext;
        }


        private void GoBack_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }
        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            HostTextBox.IsEnabled = true;
            HostTextBox.Focus();
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            HostTextBox.IsEnabled = false;
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
                bool isRegistred = registerViewModel.Register(myCheckBox.IsChecked.Value);
                if (isRegistred)
                {
                    MainWindow mainWindow = new MainWindow(registerViewModel.Controller);
                    mainWindow.Show();
                    this.Close();
                }
            
        }
    }
}
