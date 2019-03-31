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
            this?.CanExecuteChanged(this, new EventArgs());
            return true;
        }

        public void Execute(object parameter) => Action?.Invoke((Page)parameter);
    }
}