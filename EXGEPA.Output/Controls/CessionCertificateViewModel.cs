﻿// <copyright file="CessionCertificateViewModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace EXGEPA.Output.Controls
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using CORESI.Data;
    using CORESI.IoC;
    using CORESI.Tools.Collections;
    using CORESI.WPF.Core;
    using CORESI.WPF.Core.Interfaces;
    using EXGEPA.Core.Interfaces;
    using EXGEPA.Depreciations.Core;
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
            var types = this.analyticalAccountTypeService.SelectAll();
            var internalType = types.FirstOrDefault(x => x.Key.ToLowerInvariant() == "external");
            var list = this.analyticalAccountService.SelectAll(types);
            this.ListOfTakers = new ObservableCollection<NamedKeyRow>(list.Where(x => x.AnalyticalAccountType == internalType));
            this.AddNewGroup().AddCommand("Contenu du PV", IconProvider.GreaterThan, this.DisplayPvContent);
        }

        private bool IsMatchingSelectedRowId(Item item)
        {
            return item.OutputCertificate?.Id == this.SelectedRow?.Id;
        }

        private void DisplayPvContent()
        {
            var title = this.ParameterProvider.TryGet("CessionCertificateReportTitle", "Fiche de cession");
            this.UIItemService.DisplayItems(this.IsMatchingSelectedRowId, $"Contenu du PV de cession {this.SelectedRow?.Key}", (items) =>
            {
                items.ForEach(x =>
                {
                        x.Json = $"{this.SelectedRow.Tag ?? string.Empty}, Libellé : {this.SelectedRow.Json ?? string.Empty}";
                });

                this.SetTotalDepreciationInTag(items);
                var reports = ServiceLocator.Resolve<IImmobilisationSheetProvider>();
                reports.PrintOutputSheet(items, true, title);
            });
        }

        private void SetTotalDepreciationInTag(IEnumerable<Item> items)
        {
            var deprectiationTypeDaily = this.ParameterProvider.TryGet("IsDefaultDepreciationTypeDaily", false);
            ICalculator calculator = new MonthelyCalculator(null);
            if (deprectiationTypeDaily)
            {
                calculator = new DailyCalculator();
            }

            items.ForEach(x =>
            {
                var endDate = x.OutputCertificate?.Date ?? x.LimiteDate;
                x.Tag = calculator.GetDepriciations(x, x.AquisitionDate, endDate).LastOrDefault();
            });
        }
    }
}
