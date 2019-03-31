// <copyright file="ReceiveOrderViewModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace EXGEPA.Repository.Controls
{
    using CORESI.WPF.Controls;
    using CORESI.WPF.Core.Interfaces;
    using EXGEPA.Model;

    public class ReceiveOrderViewModel : GenericEditableViewModel<ReceiveOrder>
    {
        public ReceiveOrderViewModel(IExportableGrid exportableView)
            : base(exportableView)
        {
            this.Caption = "Liste de bons de recéption";
        }
    }
}