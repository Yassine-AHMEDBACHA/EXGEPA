using System.ComponentModel;

namespace CORESI.WPF
{
    public class UiNotifier : INotifyPropertyChanged
    {
        public void RaisePropertyChanged(string propertyName="")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
