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
        private readonly IParameterProvider parameterProvider;

        public Module()
        {
            this.parameterProvider = ServiceLocator.Resolve<IParameterProvider>();
        }

        public override int Priority => 50;

        public override void AddGroups()
        {
            this.SetParameters();
            var saidalGroup = new Group("Interface");
            saidalGroup.AddCommand("Aquisitions", this.AddPage<Controls.InvoiceView, Controls.InvoiceVM>, true);
            saidalGroup.AddCommand("Bons de Transfert", this.AddPage<Controls.TransferOrderView, Controls.TransferOrderVM>, true);
            saidalGroup.AddCommand("Cessions", this.AddPage<Controls.OutputView, Controls.CessionVM>, true);
            saidalGroup.AddCommand("Vente", this.AddPage<Controls.OutputView, Controls.SaleVM>, true);
            saidalGroup.AddCommand("Destruction", this.AddPage<Controls.OutputView, Controls.DestructionVM>, true);
            if (this.parameterProvider.TryGet<bool>("EnableDotationsInterface", false))
            {
                saidalGroup.AddCommand("Dotations", this.AddPage<Controls.InvoiceView, Controls.InvoiceVM>, true);
            }

            this.UIService.AddGroupToHomePage(saidalGroup);
        }

        private void SetParameters()
        {
            this.parameterProvider.TryAdd("InterfaceOutputDirectory", @"C:\SQLIMMO\");
            this.parameterProvider.TryAdd("InterfaceFileExtension", ".csv");
            this.parameterProvider.TryAdd("InterfaceSerializerSeparator", ",");
            this.parameterProvider.TryAdd("InterfaceAdditionalCharacter", " ");
            this.parameterProvider.TryAdd("InterfaceFileNamePattern", "Dump");
            this.parameterProvider.TryAdd("InterfaceFileNamePatternForCession", "CessionDump");
            this.parameterProvider.TryAdd("InterfaceFileNamePatternForDestruction", "DestructionDump");
            this.parameterProvider.TryAdd("InterfaceFileNamePatternForDotation", "DotationDump");
            this.parameterProvider.TryAdd("InterfaceFileNamePatternForTransferOrder", "TransfertDump");
            this.parameterProvider.TryAdd("InterfaceFileNamePatternForSale", "SalesDump");
        }
    }
}
