// <copyright file="DisparitionCertificateViewModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace EXGEPA.Output.Controls
{
    using CORESI.WPF.Core.Interfaces;
    using EXGEPA.Model;

    public class DisparitionCertificateViewModel : OutputViewModel
    {
        public DisparitionCertificateViewModel(IExportableGrid exportableView)
            : base(OutputType.Disparition, exportableView)
        {
            this.Caption = "Liste de PV de disparition";
        }
    }
}
