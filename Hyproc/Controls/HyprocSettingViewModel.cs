using CORESI.Data;
using CORESI.IoC;
using CORESI.WPF;
using CORESI.WPF.Model;
using System;

namespace Hyproc.Controls
{
    public class HyprocSettingViewModel : UiNotifier
    {
        public IParameterProvider ParameterProvider { get; set; }

        public HyprocSettingViewModel()
        {
            ParameterProvider = ServiceLocator.Resolve<IParameterProvider>();
            this.SyncPath = ParameterProvider.GetValue("SyncShare", Environment.CurrentDirectory) + @"\";
            this.Prefix = ParameterProvider.GetValue("prefix", "ADM");
            this.Validate = new Command(this.Save);
        }

        public void Save()
        {
            ParameterProvider.TrySetOrAdd("SyncShare", this.SyncPath);
            ParameterProvider.TrySetOrAdd("prefix", this.Prefix);
            if (this.CloseWindow != null)
                this.CloseWindow();
        }

        public Command Validate { get; set; }

        private string _SyncPath;

        public string SyncPath
        {
            get { return _SyncPath; }
            set
            {
                _SyncPath = value;
                RaisePropertyChanged("SyncPath");
            }
        }


        private string _Prefix;

        public string Prefix
        {
            get { return _Prefix; }
            set
            {
                _Prefix = value;
                RaisePropertyChanged("Prefix");
            }
        }

        public Action CloseWindow { get; set; }
    }
}
