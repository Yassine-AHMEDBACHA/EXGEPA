using CORESI.WPF.Controls;
using CORESI.WPF.Core.Interfaces;
using EXGEPA.Model;

namespace EXGEPA.Repository.Controls
{
    public class ReferenceTypeViewModel : GenericEditableViewModel<ReferenceType>
    {
        public ReferenceTypeViewModel(IExportable exportableView) : base(exportableView)
        {
            this.Caption = "Lists de familles de réfèrences";
        }
    }
}