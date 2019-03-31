using System.Windows.Input;
using CORESI.IoC;
using CORESI.Security;
using CORESI.WPF.Model;

namespace CORESI.WPF.Core.Login
{
    public class ChangePasswordViewModel : CommonViewModel
    {
        public string Login { get; set; }

        public string OldPassword { get; set; }

        public ChangePasswordViewModel(string login, string oldPassword)
        {
            this.Logger.Debug("Loading ChangePasswordViewModel ...");
            this.Login = login;
            this.OldPassword = oldPassword;
            this.LoginManager = ServiceLocator.Resolve<ILoginManager<IOperator>>();
            ValidateCommand = new Command(TryUpdatePassword);
            this.Logger.Debug("ChangePasswordViewModel ready");
        }

        private void TryUpdatePassword()
        {
            UIMessage.TryDoAction(this.Logger, () =>
            {
                BadInformations = false;

                if (Password == OldPassword || Password != Confirmation)
                {
                    BadInformations = true;
                }
                else
                {
                    this.LoginManager.UpdatePassword(Login, Password);
                    TryCloseWindow();
                }
            });
        }

        private string _Password;
        public string Password
        {
            get => _Password;
            set
            {
                _Password = value;
                RaisePropertyChanged("Password");
            }
        }

        private string _Confirmation;
        public string Confirmation
        {
            get => _Confirmation;
            set
            {
                _Confirmation = value;
                RaisePropertyChanged("Confirmation");
            }
        }

        public static void ShowChangePasswordViewModel(string login, string oldPassword)
        {
            ChangePasswordViewModel changePasswordViewModel = new ChangePasswordViewModel(login, oldPassword);
            ChangePasswordView changePasswordView = new ChangePasswordView
            {
                DataContext = changePasswordViewModel
            };
            changePasswordViewModel.CloseWindow = changePasswordView.Close;
            changePasswordView.ShowDialog();
        }
    }
}