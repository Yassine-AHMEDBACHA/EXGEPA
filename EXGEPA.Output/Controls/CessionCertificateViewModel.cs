// <copyright file="CessionCertificateViewModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace EXGEPA.Output.Controls
{
    using System;
    using System.Collections.ObjectModel;
    using System.Linq;
    using CORESI.Data;
    using CORESI.IoC;
    using CORESI.WPF.Core;
    using CORESI.WPF.Core.Interfaces;
    using EXGEPA.Core.Interfaces;
    using EXGEPA.Model;

    public class CessionCertificateViewModel : OutputViewModel
    {
        private readonly IDataProvider<AnalyticalAccount> analyticalAccountService;

        private readonly IDataProvider<AnalyticalAccountType> analyticalAccountTypeService;

        public CessionCertificateViewModel(IExportableGrid exportableView)
            : base(OutputType.Cession, exportableView)
        {
            this.Caption = "Liste de PV de cession";
            ServiceLocator.Resolve(out this.analyticalAccountService);
            ServiceLocator.Resolve(out this.analyticalAccountTypeService);
            this.TakerVisibility = System.Windows.Visibility.Visible;
            this.TakerFieldName = "Unité réceptrice";
            this.TakerOptionVisibilty = System.Windows.Visibility.Collapsed;
            System.Collections.Generic.IList<AnalyticalAccountType> types = this.analyticalAccountTypeService.SelectAll();
            AnalyticalAccountType internalType = types.FirstOrDefault(x => x.Key.ToLowerInvariant() == "external");
            System.Collections.Generic.IList<AnalyticalAccount> list = this.analyticalAccountService.SelectAll(types);
            this.ListOfTakers = new ObservableCollection<NamedKeyRow>(list.Where(x => x.AnalyticalAccountType == internalType));
            this.AddNewGroup().AddCommand("Contenu du PV", IconProvider.GreaterThan, this.DisplayPvContent);
        }

        private bool IsMatchingSelectedRowId(Item item)
        {
            return item.OutputCertificate?.Id == this.SelectedRow?.Id;
        }

        private void DisplayPvContent()
        {
            var title = this.ParameterProvider.GetAndSetIfMissing("CessionCertificateReportTitle", "Fiche de cession");
            this.UIItemService.DisplayItems(this.IsMatchingSelectedRowId, $"Contenu du PV de cession {this.SelectedRow?.Key}", (items) =>
            {
                IImmobilisationSheetProvider reports = ServiceLocator.Resolve<IImmobilisationSheetProvider>();
                reports.PrintOutputSheet(items, true, title);
            });
        }
    }
}
