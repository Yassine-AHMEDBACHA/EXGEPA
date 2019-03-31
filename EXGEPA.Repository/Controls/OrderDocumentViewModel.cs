using System.Collections.ObjectModel;
using System.Linq;
using CORESI.Data;
using CORESI.IoC;
using CORESI.WPF.Controls;
using CORESI.WPF.Core.Interfaces;
using EXGEPA.Model;

namespace EXGEPA.Repository.Controls
{
    public class OrderDocumentViewModel : GenericEditableViewModel<OrderDocument>
    {
        private IDataProvider<OrderDocumentType> _OrderDocumentTypeTypeService;

        public OrderDocumentViewModel(IExportableGrid exportableView) : base(exportableView)
        {
            this.Caption = "Liste de documents de base";
            ServiceLocator.Resolve(out this._OrderDocumentTypeTypeService);
        }

        private ObservableCollection<OrderDocumentType> _ListOfOrderDocumentType;

        public ObservableCollection<OrderDocumentType> ListOfOrderDocumentType
        {
            get { return _ListOfOrderDocumentType; }
            set
            {
                _ListOfOrderDocumentType = value;
                RaisePropertyChanged("ListOfOrderDocumentType");
            }
        }

        public override void InitData()
        {
            StartBackGroundAction(() =>
            {
                this.ListOfRows = new ObservableCollection<OrderDocument>(this.DBservice.SelectAll());
                ListOfOrderDocumentType = new ObservableCollection<OrderDocumentType>(_OrderDocumentTypeTypeService.SelectAll());
                foreach (var item in ListOfRows)
                {
                    item.OrderDocumentType = ListOfOrderDocumentType.Single(x => x.Id == item.OrderDocumentType.Id);
                }
            });
        }

        public override void AddItem()
        {
            base.AddItem();
            this.ConcernedRow.OrderDocumentType = ListOfOrderDocumentType.FirstOrDefault();
            this.RaisePropertyChanged();
        }



    }
}
