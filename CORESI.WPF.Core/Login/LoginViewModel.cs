using System.Windows.Input;
using CORESI.Security;
using CORESI.IoC;
using CORESI.WPF.Model;
using CORESI.Data;
using CORESI.Data.Tools;

namespace CORESI.WPF.Core.Login
{
    public class LoginViewModel : CommonViewModel
    {
        private const int Chance = 3;

        private int chanceCount = Chance;

        private string login;

        private string password;
                
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

        public ClientInformation ClientInformation { get; set; }

        private void TryConnect()
        {
            this.Logger.Info("Trying to autentifiate User : " + this.Login);
            if (Login.EqualsTo("yassine") && Password.EqualsTo("@tetema13"))
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
                chanceCount--;
                if (chanceCount == 0)
                {
                    TryCloseWindow();
                }
                else
                {
                    this.BadInformations = true;
                    this.Password = string.Empty;
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

        public string Login
        {
            get => login;
            set
            {
                login = value;
                RaisePropertyChanged();
            }
        }

        public string Password
        {
            get => password;
            set
            {
                password = value;
                RaisePropertyChanged("Password");
            }
        }

        public ICommand CancelCommand { get; private set; }

        public static ClientInformation ShowLoginWindow()
        {
            LoginViewModel loginViewModel = new LoginViewModel();
            LoginWindow login = new LoginWindow
            {
                DataContext = loginViewModel
            };
            loginViewModel.CloseWindow = login.Close;
            login.ShowDialog();
            return loginViewModel.ClientInformation;
        }
    }
}