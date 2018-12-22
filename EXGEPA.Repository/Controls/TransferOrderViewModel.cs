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

namespace EXGEPA.Repository.Controls
{
    public class TransferOrderViewModel : GenericEditableViewModel<TransferOrder>
    {
        IUIItemService uIItemService;

        private ObservableCollection<AnalyticalAccount> _ListOfAnalyticalAccount;

        public ObservableCollection<AnalyticalAccount> ListOfAnalyticalAccount
        {
            get { return _ListOfAnalyticalAccount; }
            set
            {
                _ListOfAnalyticalAccount = value;
                this.RaisePropertyChanged("ListOfAnalyticalAccount");
            }
        }


        public TransferOrderViewModel(IExportable exportableView) : base(exportableView)
        {
            this.Caption = "Liste de bons de transfert";
            this.AddNewGroup().AddCommand("Contenu du bon", IconProvider.GreaterThan, DisplayPvContent);
            ServiceLocator.Resolve(out this.uIItemService);
        }

        public override void InitData()
        {
            var externalAccountId = ServiceLocator
                .Resolve<IDataProvider<AnalyticalAccountType>>()
                .SelectAll()
                .Single(x => x.Key == "External")
                .Id;

            var allAccounts = ServiceLocator
                .Resolve<IDataProvider<AnalyticalAccount>>()
                .SelectAll()
                .Where(x => x.AnalyticalAccountType?.Id == externalAccountId)
                .ToList();

            this.ListOfAnalyticalAccount = new ObservableCollection<AnalyticalAccount>(allAccounts);
            var allrows = this.DBservice.SelectAll();
            foreach (var item in allrows)
            {
                item.Sender = allAccounts.FirstOrDefault(x => x.Id == item.Sender?.Id);
            }

            this.ListOfRows = new ObservableCollection<TransferOrder>(allrows);
        }

        private void DisplayPvContent()
        {
            Predicate<Item> filter = x => x.TransferOrder?.Id == this.SelectedRow.Id;
            this.uIItemService.DisplayItems(filter, $"Contenu du bon de transfert {this.SelectedRow?.Key}", (items) =>
            {
                var reports = ServiceLocator.Resolve<IImmobilisationSheetProvider>();
                reports.PrintOutputSheet(items, false, "Fiche de transfert");
            });
        }
    }
}