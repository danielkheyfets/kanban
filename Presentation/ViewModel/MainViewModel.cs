using IntroSE.Kanban.Backend.ServiceLayer;
using Presentation.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.ViewModel
{
   public class MainViewModel : NotifiableObject
    {
        public BackendController Controller { get; private set; }

        private string _email;
        public string Email
        {
            get => _email;
            set
            {
                this._email = value;
                RaisePropertyChanged("Email");
            }
        }

        private string _password;
        public string Password
        {
            get => _password;
            set
            {
                this._password = value;
                RaisePropertyChanged("Password");
            }
        }
        private string _message;
        public string Message
        {
            get => _message;
            set
            {
                this._message = value;
                RaisePropertyChanged("Message");
            }
        }

        public MainViewModel()
        {
            this.Controller = new BackendController(new Service());
        }

        public MainViewModel(BackendController controller)
        {
            this.Controller = controller;
        }

        public UserModel Login()
        {
            Message = "";
            try
            {
                return Controller.Login(_email, _password);
            }
            catch (Exception e)
            {
                Message = e.Message;
                return null;
            }
        }

    }
}
