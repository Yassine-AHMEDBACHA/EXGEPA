// <copyright file="DestructionCertificateViewModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace EXGEPA.Output.Controls
{
    using CORESI.WPF.Core.Interfaces;
    using EXGEPA.Model;

    public class DestructionCertificateViewModel : OutputViewModel
    {
        public DestructionCertificateViewModel(IExportableGrid exportableView)
            : base(OutputType.Destruction, exportableView)
        {
            this.Caption = "Liste de PV de destruction";
        }
    }
}
