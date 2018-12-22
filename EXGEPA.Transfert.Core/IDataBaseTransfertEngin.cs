using System.Collections.Generic;
using EXGEPA.Model;

namespace EXGEPA.Transfert.Core
{
    public interface IDataBaseTransfertEngin
    {
        List<ReferenceType> LoadReferenceType();
        List<AccountingPeriod> LoadAccountingPeriods();
        List<OrderDocumentType> LoadOrderDocumentTypes();
        List<AnalyticalAccountType> LoadAnalyticalAccountTypes();
        List<AnalyticalAccount> LoadAnalyticalAccount();
        List<Assignment> LoadAssignament();
        List<Building> LoadBuildings();
        List<GeneralAccount> LoadGeneralAccount();
        List<InputSheet> LoadInputSheet();
        List<Invoice> LoadInvoices();
        List<Item> LoadItem();
        List<Level> LoadLevels();
        List<Office> LoadOffices();
        List<OutputCertificate> LoadOutputCertificate();
        List<Person> LoadPeron();
        List<Provider> LoadProviders();
        List<ReceiveOrder> LoadReceiveOrder();
        List<Reference> LoadReferences();
        List<ReformeCertificate> LoadReformsCertificate();
        List<Region> LoadRegion();
        List<ItemState> LoadStates();
        List<TransferOrder> loadTransferOrder();
        List<InventoryRow> loadInventory();
        List<Site> LoadSites();
        List<GeneralAccountType> ListOfGeneralAccountType();
        string GetCompanyName();
    }
}