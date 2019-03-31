// <copyright file="Module.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace EXGEPA.Repository
{
    using CORESI.WPF.Core;
    using CORESI.WPF.Model;
    using EXGEPA.Repository.Controls;

    public sealed class Module : AModule
    {
        public override int Priority
        {
            get { return 0; }
        }

        public override void AddGroups()
        {
            // var referenceGroup = new Group();
            // referenceGroup.AddCommand("References", IconProvider.Reference, this.AddPage<ReferenceView, ReferenceViewModel>);
            // UIService.AddGroupToHomePage(referenceGroup);
            Group repository = new Group("Référentiel");
            repository.AddCommand("Cpts généraux", IconProvider.GeneralAccountSmall, this.AddPage<GeneralAccountView, GeneralAccountViewModel>, true);
            repository.AddCommand("Cpts analytiques", IconProvider.AnalyticalAccountSmall, this.AddPage<AnalyticalAccountView, AnalyticalAccountViewModel>, true);
            repository.AddCommand("Fournisseurs", IconProvider.ProviderSmall, this.AddPage<ProviderView, ProviderViewModel>, true);
            repository.AddCommand("Recéptions", IconProvider.ReceiveOrderSmall, this.AddPage<ReceiveOrderView, ReceiveOrderViewModel>, true);
            repository.AddCommand("Transfert", IconProvider.TransferOrderSmall, this.AddPage<TransferOrderView, TransferOrderViewModel>, true);
            repository.AddCommand("F.Entrées", IconProvider.InputSheetSmall, this.AddPage<InputSheetView, InputSheetViewModel>, true);
            repository.AddCommand("Personnel", IconProvider.TeamSmall, this.AddPage<PersonView, PersonViewModel>, true);
            repository.AddCommand("Devises", IconProvider.CurrencySmall, this.AddPage<CurrencyView, CurrencyViewModel>, true);
            repository.AddCommand("TVA", IconProvider.PercentageSmall, this.AddPage<TvaView, TvaViewModel>, true);
            repository.AddCommand("References", IconProvider.ReferenceSmall, this.AddPage<ReferenceView, ReferenceViewModel>, true);
            repository.AddCommand("familles", IconProvider.Reference, this.AddPage<ReferenceTypeView, ReferenceTypeViewModel>, true);
            repository.AddCommand("Projets", IconProvider.Properties, this.AddPage<ProjectView, ProjectViewModel>, true);

            repository.AddCommand("Pcs base", IconProvider.PropertiesSmall, this.AddPage<OrderDocumentView, OrderDocumentViewModel>, true);

            this.UIService.AddGroupToHomePage(repository);
            Logger.Info("loading Repository Module done");
        }
    }
}
