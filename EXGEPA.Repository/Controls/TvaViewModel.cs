using CORESI.WPF.Controls;
using CORESI.WPF.Core.Interfaces;
using EXGEPA.Model;

namespace EXGEPA.Repository.Controls
{
    public class TvaViewModel : GenericEditableViewModel<Tva>
    {
        public TvaViewModel(IExportable exportableView) : base(exportableView)
        {
            this.Caption = "Liste de TVA";
        }
    }
}