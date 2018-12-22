using CORESI.Security;
using CORESI.WPF.Core.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace CORESI.WPF.Core.Login
{
    public abstract class CommonViewModel : PageViewModel
    {
        
        protected ILoginManager<IOperator> LoginManager;
        public ICommand ValidateCommand { get; protected internal set; }
        protected Action CloseWindow { get; set; }
        protected void TryCloseWindow()
        {
            if (CloseWindow != null)
            {
                CloseWindow();
            }
        }
        private bool _BadInformations;
        public bool BadInformations
        {
            get { return _BadInformations; }
            set
            {
                _BadInformations = value;
                RaisePropertyChanged("BadInformations");
            }
        }

        private bool _TopMost;
        public bool TopMost
        {
            get
            {
                return _TopMost;
            }
            set
            {
                _TopMost = value;
                RaisePropertyChanged("TopMost");
            }
        }
    }
}
