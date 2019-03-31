// <copyright file="ProjectViewModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace EXGEPA.Repository.Controls
{
    using CORESI.WPF.Controls;
    using CORESI.WPF.Core.Interfaces;
    using EXGEPA.Model;

    public class ProjectViewModel : GenericEditableViewModel<Project>
    {
        public ProjectViewModel(IExportableGrid exportableView)
            : base(exportableView)
        {
            this.Caption = "Lists de projets";
        }
    }
}