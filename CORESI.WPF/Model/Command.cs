using CORESI.IoC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace CORESI.WPF.Model
{
    public class Command : ICommand
    {
        static IButtonRights ButtonRights = ServiceLocator.GetDefault<IButtonRights>();

        Action Action { get; set; }

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
            var canExecute = ButtonRights.CanDoAction(Ability);
            return canExecute;
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
