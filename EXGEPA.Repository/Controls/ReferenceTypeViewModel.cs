// <copyright file="ReferenceTypeViewModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace EXGEPA.Repository.Controls
{
    using CORESI.WPF.Controls;
    using CORESI.WPF.Core.Interfaces;
    using EXGEPA.Model;

    public class ReferenceTypeViewModel : GenericEditableViewModel<ReferenceType>
    {
        public ReferenceTypeViewModel(IExportableGrid exportableView)
            : base(exportableView)
        {
            this.Caption = "Lists de familles de réfèrences";
        }
    }
}