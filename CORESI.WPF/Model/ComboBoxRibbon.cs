using System.Collections.ObjectModel;

namespace CORESI.WPF.Model
{
    public class ComboBoxRibbon<T> : EditSettingRibbon
    {



        public ComboBoxRibbon(string caption = "")
            : base(caption)
        {
            this.ItemsSource = new ObservableCollection<T>();
        }

        private T _EditValue;

        public T EditValue
        {
            get { return _EditValue; }
            set
            {
                _EditValue = value;
                RaisePropertyChanged("EditValue");
            }
        }

        private ObservableCollection<T> _ItemsSource;

        public ObservableCollection<T> ItemsSource
        {
            get { return _ItemsSource; }
            set
            {
                _ItemsSource = value;
                RaisePropertyChanged("ItemsSource");
            }
        }




    }
}
