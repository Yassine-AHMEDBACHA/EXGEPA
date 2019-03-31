// <copyright file="CurrencyViewModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace EXGEPA.Repository.Controls
{
    using CORESI.WPF.Controls;
    using CORESI.WPF.Core.Interfaces;
    using EXGEPA.Model;

    public class CurrencyViewModel : GenericEditableViewModel<Currency>
    {
        public CurrencyViewModel(IExportableGrid exportableView)
            : base(exportableView)
        {
            this.Caption = "Lists de devises";
        }

        public override string GetTemporaryKey()
        {
            return "DEV";
        }
    }
}