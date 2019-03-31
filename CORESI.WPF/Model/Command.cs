using CORESI.IoC;
using System;
using System.Windows.Input;

namespace CORESI.WPF.Model
{
    public class Command : ICommand
    {
        private static readonly IButtonRights ButtonRights = ServiceLocator.GetDefault<IButtonRights>();

        Action Action { get; set; }

        private bool canExecuteAction;

        public string Ability { get; set; }
        bool FullAccess { get; set; }
        public Command(Action action, bool fullAccess)
        {
            this.Action = action;
            this.FullAccess = fullAccess;
        }
        public Command(Action action, string ability)
        {
            this.Action = action;
            this.Ability = ability;
        }

        public Command(Action action) : this(action, false)
        {
            this.Action = action;
        }

        public bool CanExecute(object parameter)
        {
            if (FullAccess)
                return false;

            if (this.canExecuteAction != ButtonRights.CanDoAction(Ability))
            {
                this.canExecuteAction = !this.canExecuteAction;
                this.CanExecuteChanged?.Invoke(this, new EventArgs());
            }

            return this.canExecuteAction;
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            Action();
        }

        public void SetAbility<T>(string operation)
        {
            this.Ability = $"{typeof(T).Name}-{operation}";
        }
    }
}
