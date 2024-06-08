using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.ViewModel
{
    class RegisterViewModel : NotifiableObject
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

        private string _nickName;
        public string Nickname
        {
            get => _nickName;
            set
            {
                this._nickName = value;
                RaisePropertyChanged("Nickname");
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

        private string _emailHost;
        public string Host
        {
            get => _emailHost;
            set
            {
                this._emailHost = value;
                RaisePropertyChanged("Host");
            }
        }

        public RegisterViewModel(BackendController controller)
        {
            this.Controller = controller;
        }
        public bool Register(bool isHostRegister)
        {
            if (isHostRegister)
            {
                try
                {
                    Controller.Register(_email, _password, _nickName, _emailHost);
                    return true;

                }
                catch (Exception e)
                {
                    Message = e.Message;
                    return false;
                }
            }
            else
            {
                Message = "";
                try
                {
                    Controller.Register(_email, _password, _nickName);
                    return true;
                }
                catch (Exception e)
                {
                    Message = e.Message;
                    return false;
                }
            }
        }
    }
}
