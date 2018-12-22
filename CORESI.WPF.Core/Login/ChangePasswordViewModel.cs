using System;
using DevExpress.Mvvm;
using CORESI.WPF.Core.Framework;
using System.Windows.Input;
using CORESI.IoC;
using CORESI.Security;
using CORESI.WPF.Model;

namespace CORESI.WPF.Core.Login
{
    public class ChangePasswordViewModel : CommonViewModel
    {
        IUIMessage UIMessage { get; set; }
        public string Login { get; set; }
        public string OldPassword { get; set; }
        public ChangePasswordViewModel(string login,string oldPassword)
        {
            logger.Debug("Loading ChangePasswordViewModel ...");
            this.UIMessage = ServiceLocator.GetPriorizedInstance<IUIMessage>();
            this.Login = login;
            this.OldPassword = oldPassword;
            this.LoginManager = ServiceLocator.Resolve<ILoginManager<IOperator>>();
            ValidateCommand = new Command(TryUpdatePassword);
            logger.Debug("ChangePasswordViewModel ready");
        }

        private void TryUpdatePassword()
        {
            UIMessage.TryDoAction(logger, () =>
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
            get { return _Password; }
            set
            {
                _Password = value;
                RaisePropertyChanged("Password");
            }
        }

        private string _Confirmation;
        public string Confirmation
        {
            get { return _Confirmation; }
            set
            {
                _Confirmation = value;
                RaisePropertyChanged("Confirmation");
            }
        }
                
        public ICommand ValidateCommand { get; private set; }
        

        public static void ShowChangePasswordViewModel(string login, string oldPassword)
        {
            logger.Info("Loading loggin window");
            var changePasswordViewModel = new ChangePasswordViewModel(login,oldPassword);
            var changePasswordView = new ChangePasswordView();
            changePasswordView.DataContext = changePasswordViewModel;
            changePasswordViewModel.CloseWindow = changePasswordView.Close;
            changePasswordView.ShowDialog();
        }
    }
}