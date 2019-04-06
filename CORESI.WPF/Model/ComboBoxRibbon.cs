using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using CORESI.Tools.Collections;

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
            get => _EditValue;
            set
            {
                _EditValue = value;
                RaisePropertyChanged("EditValue");
            }
        }

        private ObservableCollection<T> _ItemsSource;

        public ObservableCollection<T> ItemsSource
        {
            get => _ItemsSource;
            set
            {
                _ItemsSource = value;
                RaisePropertyChanged("ItemsSource");
            }
        }

        public void SetSource(IEnumerable<T> collection)
        {
            ItemsSource = collection.ToObservable();
            EditValue = ItemsSource.FirstOrDefault();
        }


    }
}
