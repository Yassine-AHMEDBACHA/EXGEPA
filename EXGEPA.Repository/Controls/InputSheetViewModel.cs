// <copyright file="InputSheetViewModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace EXGEPA.Repository.Controls
{
    using CORESI.WPF.Controls;
    using CORESI.WPF.Core.Interfaces;
    using EXGEPA.Model;

    public class InputSheetViewModel : GenericEditableViewModel<InputSheet>
    {
        public InputSheetViewModel(IExportableGrid exportableView)
            : base(exportableView)
        {
            this.Caption = "Liste de fiches d'entrée";
        }
    }
}