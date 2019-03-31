using CORESI.WPF.Controls;
using CORESI.WPF.Core.Interfaces;
using EXGEPA.Model;

namespace EXGEPA.Repository.Controls
{
    public class InputSheetViewModel : GenericEditableViewModel<InputSheet>
    {
        public InputSheetViewModel(IExportableGrid exportableView) : base(exportableView)
        {
            this.Caption = "Liste de fiches d'entrée";
        }
    }
}