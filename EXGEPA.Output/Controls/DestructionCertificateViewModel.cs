using CORESI.WPF.Core.Interfaces;
using EXGEPA.Model;

namespace EXGEPA.Output.Controls
{
    class DestructionCertificateViewModel : OutputViewModel
    {
        public DestructionCertificateViewModel(IExportable exportableView) : base(OutputType.Destruction, exportableView)
        {
            this.Caption = "Liste de PV de destruction";
        }
    } 
}
