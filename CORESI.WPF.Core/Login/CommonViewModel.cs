using CORESI.Security;
using CORESI.WPF.Core.Framework;
using System;
using System.Windows.Input;

namespace CORESI.WPF.Core.Login
{
    public abstract class CommonViewModel : PageViewModel
    {
        private bool badInformations;

        private bool topMost;

        protected ILoginManager<IOperator> LoginManager { get; set; }

        public ICommand ValidateCommand { get; protected internal set; }

        protected Action CloseWindow { get; set; }

        protected void TryCloseWindow() => CloseWindow?.Invoke();

        public bool BadInformations
        {
            get => badInformations;
            set
            {
                badInformations = value;
                RaisePropertyChanged(nameof(this.BadInformations));
            }
        }


        public bool TopMost
        {
            get => topMost;
            set
            {
                topMost = value;
                RaisePropertyChanged(nameof(this.TopMost));
            }
        }
    }
}
