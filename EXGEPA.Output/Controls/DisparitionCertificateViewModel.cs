using CORESI.WPF.Core.Interfaces;
using EXGEPA.Model;

namespace EXGEPA.Output.Controls
{
    class DisparitionCertificateViewModel : OutputViewModel
    {
        public DisparitionCertificateViewModel(IExportableGrid exportableView) : base(OutputType.Disparition, exportableView)
        {
            this.Caption = "Liste de PV de disparition";
        }
    }
}
