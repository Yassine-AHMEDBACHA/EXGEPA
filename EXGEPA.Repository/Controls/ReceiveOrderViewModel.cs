using CORESI.WPF.Controls;
using CORESI.WPF.Core.Interfaces;
using EXGEPA.Model;

namespace EXGEPA.Repository.Controls
{
    public class ReceiveOrderViewModel : GenericEditableViewModel<ReceiveOrder>
    {
        public ReceiveOrderViewModel(IExportable exportableView) : base(exportableView)
        {
            this.Caption = "Liste de bons de recéption";
        }
    }
}