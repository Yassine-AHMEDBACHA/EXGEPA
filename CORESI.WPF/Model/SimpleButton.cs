using System;

namespace CORESI.WPF.Model
{
    public class SimpleButton : SimpleItem
    {
        public SimpleButton(string caption = "") : base(caption)
        {
            IsEnabled = true;
            this.CommandAction = new Command(ExecuteAction);
        }

        private void ExecuteAction()
        {
            if (this.Action != null)
                Action();
        }

        private bool _IsEnabled;
        public bool IsEnabled
        {
            get => _IsEnabled;
            set
            {
                _IsEnabled = value;
                RaisePropertyChanged("IsEnabled");
            }
        }


        public Command CommandAction { get; protected set; }
        public Action Action { get; set; }

        public void SetAbility<T>(string operation)
        {
            this.SetAbility($"{typeof(T).Name}-{operation}");
        }

        public void SetAbility<T>()
        {
            this.SetAbility<T>(this.Caption);
        }

        public void SetAbility(string ability)
        {
            this.CommandAction.Ability = ability;
        }
    }
}
