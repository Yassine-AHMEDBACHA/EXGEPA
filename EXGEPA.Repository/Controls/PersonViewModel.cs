// <copyright file="PersonViewModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace EXGEPA.Repository.Controls
{
    using CORESI.WPF.Controls;
    using CORESI.WPF.Core.Interfaces;
    using EXGEPA.Model;

    public class PersonViewModel : GenericEditableViewModel<Person>
    {
        public PersonViewModel(IExportableGrid exportableView)
            : base(exportableView)
        {
            this.Caption = "Liste de Personnel";
        }
    }
}