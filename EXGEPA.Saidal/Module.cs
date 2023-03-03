// <copyright file="Module.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace EXGEPA.Saidal
{
    using CORESI.Data;
    using CORESI.IoC;
    using CORESI.WPF.Core;
    using CORESI.WPF.Model;

    public sealed class Module : AModule
    {
        public override int Priority => 50;

        public override void AddGroups()
        {
            var saidalGroup = new Group("Interface");
            saidalGroup.AddCommand("Aquisitions", this.AddPage<Controls.InvoiceView, Controls.InvoiceVM>, true);
            saidalGroup.AddCommand("Bons de Transfert", this.AddPage<Controls.TransferOrderView, Controls.TransferOrderVM>, true);
            saidalGroup.AddCommand("Cessions", this.AddPage<Controls.OutputView, Controls.CessionVM>, true);
            saidalGroup.AddCommand("Vente", this.AddPage<Controls.OutputView, Controls.SaleVM>, true);
            saidalGroup.AddCommand("Destruction", this.AddPage<Controls.OutputView, Controls.DestructionVM>, true);
            ServiceLocator.Resolve<IParameterProvider>(out IParameterProvider parameterProvider);
            if (parameterProvider.GetAndSetIfMissing<bool>("EnableDotationsInterface", false))
            {
                saidalGroup.AddCommand("Dotations", this.AddPage<Controls.InvoiceView, Controls.InvoiceVM>, true);
            }

            this.UIService.AddGroupToHomePage(saidalGroup);
        }
    }
}
