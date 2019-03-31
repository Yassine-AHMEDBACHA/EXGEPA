// <copyright file="TransferOrderViewModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace EXGEPA.Repository.Controls
{
    using System;
    using System.Collections.ObjectModel;
    using System.Linq;
    using CORESI.Data;
    using CORESI.IoC;
    using CORESI.WPF.Controls;
    using CORESI.WPF.Core;
    using CORESI.WPF.Core.Interfaces;
    using EXGEPA.Core.Interfaces;
    using EXGEPA.Model;

    public class TransferOrderViewModel : GenericEditableViewModel<TransferOrder>
    {
        private readonly IUIItemService uIItemService;

        private ObservableCollection<AnalyticalAccount> listOfAnalyticalAccount;

        public TransferOrderViewModel(IExportableGrid exportableView)
            : base(exportableView)
        {
            this.Caption = "Liste de bons de transfert";
            this.AddNewGroup().AddCommand("Contenu du bon", IconProvider.GreaterThan, this.DisplayPvContent);
            ServiceLocator.Resolve(out this.uIItemService);
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

        public override void InitData()
        {
            int externalAccountId = ServiceLocator
                .Resolve<IDataProvider<AnalyticalAccountType>>()
                .SelectAll()
                .Single(x => x.Key == "External")
                .Id;

            System.Collections.Generic.List<AnalyticalAccount> allAccounts = ServiceLocator
                .Resolve<IDataProvider<AnalyticalAccount>>()
                .SelectAll()
                .Where(x => x.AnalyticalAccountType?.Id == externalAccountId)
                .ToList();

            this.ListOfAnalyticalAccount = new ObservableCollection<AnalyticalAccount>(allAccounts);
            System.Collections.Generic.IList<TransferOrder> allrows = this.DBservice.SelectAll();
            foreach (TransferOrder item in allrows)
            {
                item.Sender = allAccounts.FirstOrDefault(x => x.Id == item.Sender?.Id);
            }

            this.ListOfRows = new ObservableCollection<TransferOrder>(allrows);
        }

        private void DisplayPvContent()
        {
            this.uIItemService.DisplayItems(
            x => x.TransferOrder?.Id == this.SelectedRow.Id,
             $"Contenu du bon de transfert {this.SelectedRow?.Key}",
             (items) =>
            {
                IImmobilisationSheetProvider reports = ServiceLocator.Resolve<IImmobilisationSheetProvider>();
                reports.PrintOutputSheet(items, false, "Fiche de transfert");
            });
        }
    }
}