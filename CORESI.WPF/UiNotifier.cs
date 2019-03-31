using System.ComponentModel;

namespace CORESI.WPF
{
    public class UiNotifier : INotifyPropertyChanged
    {
        public void RaisePropertyChanged(string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
