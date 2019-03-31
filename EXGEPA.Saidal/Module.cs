// <copyright file="Module.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace EXGEPA.Saidal
{
    using CORESI.WPF.Core;
    using CORESI.WPF.Model;

    public sealed class Module : AModule
    {
        public override int Priority => 50;

        public override void AddGroups()
        {
            var saidalGroup = new Group();
            saidalGroup.AddCommand("Interface", IconProvider.PageSetup, this.AddPage<Controls.InterfaceView, Controls.InterfaceViewModel>);
            this.UIService.AddGroupToHomePage(saidalGroup);
        }
    }
}
