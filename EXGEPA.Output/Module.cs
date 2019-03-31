// <copyright file="Module.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace EXGEPA.Output
{
    using CORESI.WPF.Core;
    using CORESI.WPF.Model;
    using EXGEPA.Output.Controls;

    public sealed class Module : AModule
    {
        public override int Priority
        {
            get { return 5; }
        }

        public override void AddGroups()
        {
            var outputGroup = new Group("Sorties");
            outputGroup.AddCommand("Proposés à la reforme", IconProvider.Reforme, this.AddPage<OutputView, ProposeToReformViewModel>, true);
            outputGroup.AddCommand("Reforme", IconProvider.Reforme, this.AddPage<OutputView, ReformeCertificateViewModel>, true);
            outputGroup.AddCommand("Vente", IconProvider.Download, this.AddPage<OutputView, SaleCertificateViewModel>, true);
            outputGroup.AddCommand("Cession", IconProvider.CessionSmall, this.AddPage<OutputView, CessionCertificateViewModel>, true);
            outputGroup.AddCommand("Disparition", IconProvider.DisparitionSmall, this.AddPage<OutputView, DisparitionCertificateViewModel>, true);
            outputGroup.AddCommand("Destruction", IconProvider.DestructionSmall, this.AddPage<OutputView, DestructionCertificateViewModel>, true);
            this.UIService.AddGroupToHomePage(outputGroup);
        }
    }
}
