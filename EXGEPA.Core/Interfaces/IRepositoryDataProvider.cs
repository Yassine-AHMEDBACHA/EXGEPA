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

        void BindItemFields(Item item);

        void BindItemFields(IList<Item> Items);

        void Refresh();
    }
}
