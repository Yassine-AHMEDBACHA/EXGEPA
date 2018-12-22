using System;
using System.Windows.Input;
using CORESI.WPF.Model;

namespace CORESI.WPF.Core.Shell
{
    internal class DelegateCommand<T> : ICommand
    {
        private Action<Page> Action;

        public DelegateCommand(Action<Page> switchView)
        {
            this.Action = switchView;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            if (Action != null)
                Action((Page)parameter);
        }
    }
}