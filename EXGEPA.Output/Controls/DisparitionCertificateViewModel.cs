using CORESI.WPF.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using EXGEPA.Model;
using System.Text;

namespace EXGEPA.Output.Controls
{
    class DisparitionCertificateViewModel : OutputViewModel
    {
        public DisparitionCertificateViewModel(IExportable exportableView) : base(OutputType.Disparition, exportableView)
        {
            this.Caption = "Liste de PV de disparition";
        }
    }
}
