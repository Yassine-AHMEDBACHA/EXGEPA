// <copyright file="TransferOrderViewModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace EXGEPA.Repository.Controls
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using CORESI.Data;
    using CORESI.IoC;
    using CORESI.Tools.Collections;
    using CORESI.WPF.Controls;
    using CORESI.WPF.Core;
    using CORESI.WPF.Core.Interfaces;
    using EXGEPA.Core.Interfaces;
    using EXGEPA.Depreciations.Core;
    using EXGEPA.Model;

    public class TransferOrderViewModel : GenericEditableViewModel<TransferOrder>
    {
        private readonly IUIItemService uIItemService;

        private ObservableCollection<AnalyticalAccount> listOfAnalyticalAccount;

        public TransferOrderViewModel(IExportableGrid exportableView)
            : base(exportableView, false)
        {
            this.Caption = "Liste de bons de transfert";
            this.AddNewGroup().AddCommand("Contenu du bon", IconProvider.GreaterThan, this.DisplayPvContent);
            ServiceLocator.Resolve(out this.uIItemService);
            this.AnalyticalAccountService = ServiceLocator.Resolve<IDataProvider<AnalyticalAccount>>();
            this.InitData();
        }

        public ObservableCollection<AnalyticalAccount> ListOfAnalyticalAccount
        {
            get => this.listOfAnalyticalAccount;
            set
            {
                this.listOfAnalyticalAccount = value;
                this.RaisePropertyChanged(nameof(this.ListOfAnalyticalAccount));
            }
        }

        protected IDataProvider<AnalyticalAccount> AnalyticalAccountService { get; }

        public override void InitData()
        {
            var externalAccountId = ServiceLocator
                .Resolve<IDataProvider<AnalyticalAccountType>>()
                .SelectAll()
                .Single(x => x.Key == "External")
                .Id;

            var allAccounts = this.AnalyticalAccountService.SelectAll();

            this.ListOfAnalyticalAccount = new ObservableCollection<AnalyticalAccount>(allAccounts.Where(x => x.AnalyticalAccountType?.Id == externalAccountId));
            var allrows = this.DBservice.SelectAll().ApplyOnAll(item => item.Sender = allAccounts.FirstOrDefault(x => x.Id == item.Sender?.Id));
            this.ListOfRows = new ObservableCollection<TransferOrder>(allrows);
        }

        private bool IsMatchingSelectedRowId(Item item)
        {
            return item.TransferOrder?.Id == this.SelectedRow?.Id;
        }

        private void DisplayPvContent()
        {
            var title = this.ParameterProvider.TryGet("TransferOrderReportTitle", "Fiche de transfert");
            this.uIItemService.DisplayItems(this.IsMatchingSelectedRowId, $"Contenu du bon de transfert {this.SelectedRow?.Key}", (items) =>
            {
                var reports = ServiceLocator.Resolve<IImmobilisationSheetProvider>();
                this.SetTotalDepreciationInTag(items);
                reports.PrintOutputSheet(items, false, title);
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