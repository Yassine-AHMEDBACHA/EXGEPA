using EXGEPA.Model;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace EXGEPA.Core.Interfaces
{
    public interface IRepositoryDataProvider
    {
        ObservableCollection<Office> ListOfOffice { get; set; }

        ObservableCollection<ItemState> AllStats { get; set; }

        ObservableCollection<AccountingPeriod> ListOfAccountingPeriod { get; set; }

        ObservableCollection<GeneralAccount> AllGeneralAccounts { get; set; }

        ObservableCollection<Item> AllItems { get; set; }

        Dictionary<int, Item> ItemById { get; set; }

        ObservableCollection<Invoice> AllInvoices { get; set; }

        ObservableCollection<TransferOrder> AllTransferOrders { get; set; }

        ObservableCollection<OutputCertificate> ListOfOutputCertificate { get; set; }

        ObservableCollection<AnalyticalAccount> ListOfAnalyticalAccount { get; }

        void BindProperties(Item item);

        void BindProperties(IList<Item> Items);

        void BindPropertyAndSetExtended(Item item);

        void BindPropertyAndSetExtended(IList<Item> Items);

        void MapAllItems();

        void Refresh();

        void WaitTillDataReady();
    }
}
