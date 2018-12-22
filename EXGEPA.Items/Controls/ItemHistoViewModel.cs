using CORESI.WPF.Controls;
using CORESI.WPF.Core.Interfaces;
using EXGEPA.Model;

namespace EXGEPA.Items.Controls
{
    public class ItemHistoViewModel : GenericEditableViewModel<Item>
    {
        public int Id { get; set; }
        public ItemHistoViewModel(IExportable view, int id)
        {
            this.Id = id;
            this.AutoWidth = false;
            HideAddButton = true;
            HideEditButton = true;
            HideDeleteButton = true;
            this.EnableTotalSumary = true;
            this.DoubleClicAction = this.EditItem;
            this.Caption = "Historique de mouvements";
            InitilizeRibbonGroup(view);
            this.InitData();
        }

        public override void InitData()
        {
            this.ListOfRows = new System.Collections.ObjectModel.ObservableCollection<Item>(this.DBservice.GetHistoric(this.Id));
        }
    }
}
