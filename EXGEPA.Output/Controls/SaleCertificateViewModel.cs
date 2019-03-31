// <copyright file="SaleCertificateViewModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace EXGEPA.Output.Controls
{
    using System.Collections.ObjectModel;
    using CORESI.Data;
    using CORESI.IoC;
    using CORESI.WPF.Core.Interfaces;
    using EXGEPA.Model;

    public class SaleCertificateViewModel : OutputViewModel
    {
        private readonly IDataProvider<Provider> providerService;

        public SaleCertificateViewModel(IExportableGrid exportableView)
            : base(OutputType.Vente, exportableView)
        {
            this.Caption = "Liste de PV de vente";
            this.TakerVisibility = System.Windows.Visibility.Visible;
            this.TakerOptionVisibilty = System.Windows.Visibility.Visible;
            this.TakerFieldName = "Client";
            ServiceLocator.Resolve(out this.providerService);
            this.ListOfTakers = new ObservableCollection<NamedKeyRow>(this.providerService.SelectAll());
        }
    }
}
