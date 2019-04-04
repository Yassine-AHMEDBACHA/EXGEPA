using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CORESI.WPF
{
    public class UiNotifier : INotifyPropertyChanged
    {
        public void RaisePropertyChanged([CallerMemberName] string caller = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(caller));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
