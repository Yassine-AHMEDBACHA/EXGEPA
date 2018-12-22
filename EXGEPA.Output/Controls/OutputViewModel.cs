using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using CORESI.Data;
using CORESI.WPF.Core.Interfaces;
using EXGEPA.Model;

namespace EXGEPA.Output.Controls
{
    public class OutputViewModel : CommunOutputViewModel
    {
        public OutputViewModel(OutputType outputType, IExportable exportableView) : base(outputType, exportableView)
        { }

        private ObservableCollection<NamedKeyRow> _ListOfTakers;
        public ObservableCollection<NamedKeyRow> ListOfTakers
        {
            get { return _ListOfTakers; }
            set
            {
                _ListOfTakers = value;
                RaisePropertyChanged("ListOfTakers");
            }
        }

        private Visibility _TakerVisibility;

        public Visibility TakerVisibility
        {
            get { return _TakerVisibility; }
            set
            {
                _TakerVisibility = value;
                RaisePropertyChanged("TakerVisibility");
            }
        }

        private string _TakerFieldName;
        public string TakerFieldName
        {
            get { return _TakerFieldName; }
            set
            {
                _TakerFieldName = value;
                RaisePropertyChanged("TakerFieldName");
            }
        }

        private Visibility _TakerOptionVisibilty;

        public Visibility TakerOptionVisibilty
        {
            get { return _TakerOptionVisibilty; }
            set
            {
                _TakerOptionVisibilty = value;
                RaisePropertyChanged("TakerOptionVisibilty");
            }
        }

        public NamedKeyRow Taker
        {
            get { return this.GetTacker(this.ConcernedRow?.Tag.ToString()); }
            set
            {
                this.ConcernedRow.Tag = value.Key;
                RaisePropertyChanged("Taker");
            }
        }

        public virtual NamedKeyRow GetTacker(string key)
        {
            return this.ListOfTakers?.FirstOrDefault(x => x.Key == key);
        }

        public decimal SaleAmount
        {
            get
            {
                decimal amount;
                decimal.TryParse(this.ConcernedRow?.Json, out amount);
                return amount;   
            }
            set
            {
                this.ConcernedRow.Json = value.ToString();
            }
        }

    }
}