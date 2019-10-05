using CORESI.Tools.Collections;
using CORESI.WPF.Controls;
using CORESI.WPF.Core.Interfaces;
using EXGEPA.Model;
using System.Linq;

namespace EXGEPA.Items.Controls
{
    public class ItemHistoViewModel : GenericEditableViewModel<Item>
    {
        public int Id { get; set; }

        public ItemHistoViewModel(IExportableGrid view, int id)
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
            this.ListOfRows = this.DBservice.GetHistoric(this.Id)
                .GroupBy(x=>x.Office)
                .FirstOrDefault()
                .ToObservable();
        }
    }
}
