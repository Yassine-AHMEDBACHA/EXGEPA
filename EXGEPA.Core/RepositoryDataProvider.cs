namespace EXGEPA.Core
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel.Composition;
    using System.Linq;
    using System.Threading;
    using CORESI.Data;
    using CORESI.DataAccess.Core.Tools;
    using CORESI.IoC;
    using CORESI.Tools;
    using CORESI.Tools.Collections;
    using CORESI.WPF;
    using EXGEPA.Core.Interfaces;
    using EXGEPA.Model;

    [Export(typeof(IRepositoryDataProvider)), PartCreationPolicy(CreationPolicy.NonShared)]

    public sealed class RepositoryDataProvider : UiNotifier, IRepositoryDataProvider
    {
        private readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly ManualResetEventSlim manualResetEventSlim;

        public RepositoryDataProvider()
        {
            this.manualResetEventSlim = new ManualResetEventSlim();
            ServiceLocator.ComposeParts(this);
            this.UIMessage = ServiceLocator.GetPriorizedInstance<IUIMessage>();
            this.Refresh();
        }

        [Import(typeof(IDataProvider<Person>))]
        IDataProvider<Person> PersonService { get; set; }

        [Import(typeof(IDataProvider<Provider>))]
        IDataProvider<Provider> ProviderService { get; set; }

        [Import(typeof(IDataProvider<ReceiveOrder>))]
        IDataProvider<ReceiveOrder> ReceiveOrderService { get; set; }

        [Import(typeof(IDataProvider<ItemState>))]
        IDataProvider<ItemState> ItemStateService { get; set; }

        [Import(typeof(IDataProvider<InputSheet>))]
        IDataProvider<InputSheet> InputSheetService { get; set; }

        [Import(typeof(IDataProvider<Tva>))]
        IDataProvider<Tva> TvaService { get; set; }

        [Import(typeof(IDataProvider<Reference>))]
        IDataProvider<Reference> ReferenceService { get; set; }

        [Import(typeof(IDataProvider<GeneralAccount>))]
        IDataProvider<GeneralAccount> GeneralAccountService { get; set; }

        [Import(typeof(IDataProvider<AccountingPeriod>))]
        IDataProvider<AccountingPeriod> AccountingPeriodService { get; set; }

        [Import(typeof(IDataProvider<ReformeCertificate>))]
        IDataProvider<ReformeCertificate> ReformeCertificateService { get; set; }

        [Import(typeof(IDataProvider<TransferOrder>))]
        IDataProvider<TransferOrder> TransferOrderService { get; set; }

        [Import(typeof(IDataProvider<OutputCertificate>))]
        IDataProvider<OutputCertificate> OutputCertificateService { get; set; }

        [Import(typeof(IDataProvider<AnalyticalAccount>))]
        IDataProvider<AnalyticalAccount> AnalyticalAccountService { get; set; }

        [Import(typeof(IDataProvider<Invoice>))]
        IDataProvider<Invoice> InvoiceService { get; set; }

        [Import(typeof(IDataProvider<Office>))]
        IDataProvider<Office> OfficeService { get; set; }

        [Import(typeof(IDataProvider<OrderDocument>))]
        IDataProvider<OrderDocument> OrderDocumentService { get; set; }

        [Import(typeof(IDataProvider<ProposeToReformCertificate>))]
        IDataProvider<ProposeToReformCertificate> ProposeToReformCertificateService { get; set; }

        [Import(typeof(IDataProvider<ReferenceType>))]
        IDataProvider<ReferenceType> ReferenceTypeService { get; set; }

        [Import(typeof(IDataProvider<Item>))]
        IDataProvider<Item> ItemService { get; set; }

        [Import(typeof(IDataProvider<GeneralAccountType>))]
        IDataProvider<GeneralAccountType> GeneralAccountTypeService { get; set; }

        public void Refresh()
        {
            this.manualResetEventSlim.Reset();
            this.UIMessage.TryDoActionAsync(logger, loadData, manualResetEventSlim.Set);
        }

        public void WaitTillDataReady()
        {
            this.manualResetEventSlim.Wait();
        }


        #region Data Providers

        public ObservableCollection<ItemState> AllStats { get; set; }

        public ObservableCollection<ProposeToReformCertificate> ListOfProposeToReformCertificate { get; set; }

        public ObservableCollection<Tva> ListOfTva { get; set; }

        public ObservableCollection<ReformeCertificate> ListOfReformeCertificate { get; set; }

        public ObservableCollection<OutputCertificate> ListOfOutputCertificate { get; set; }

        public ObservableCollection<GeneralAccountType> AllGeneralAccountTypes { get; set; }

        public ObservableCollection<GeneralAccount> AllGeneralAccounts { get; set; }

        public ObservableCollection<Reference> ListOfReference { get; set; }

        public ObservableCollection<ReferenceType> ListOfReferenceType { get; set; }

        public ObservableCollection<Invoice> AllInvoices { get; set; }

        public ObservableCollection<TransferOrder> AllTransferOrders { get; set; }

        public ObservableCollection<Provider> ListOfProvider { get; set; }

        public ObservableCollection<OrderDocument> OrderDocuments { get; set; }

        public ObservableCollection<AnalyticalAccount> ListOfAnalyticalAccount { get; set; }

        public ObservableCollection<Office> ListOfOffice { get; set; }

        public ObservableCollection<Depreciation> ListOfDepreciation { get; set; }

        public ObservableCollection<InputSheet> ListOfInputSheet { get; set; }

        public ObservableCollection<ReceiveOrder> ListOfReceiveOrder { get; set; }

        public ObservableCollection<Person> ListOfPerson { get; set; }

        public ObservableCollection<AccountingPeriod> ListOfAccountingPeriod { get; set; }

        #endregion

        public void loadData()
        {
            FillRepositories();
            RaisePropertyChanged(string.Empty);
        }

        public void FillRepositories()
        {
            this.manualResetEventSlim.Reset();
            this.FillItems();
            this.AllGeneralAccountTypes = this.GeneralAccountTypeService.All
                .ToObservable();
            this.AllStats = this.ItemStateService.All.ToObservable();
            this.ListOfProposeToReformCertificate = this.ProposeToReformCertificateService.All.ToObservable();
            this.ListOfAccountingPeriod = this.AccountingPeriodService.All.ToObservable();
            this.ListOfReformeCertificate = ReformeCertificateService.All.ToObservable();
            this.ListOfOutputCertificate = OutputCertificateService.All.ToObservable();
            this.FillPersons();
            this.FillAnalyticalAccounts();
            this.AllTransferOrders = TransferOrderService.All
                .ApplyOnAll(x => x.SetProperties(this.ListOfAnalyticalAccount))
                .ToObservable();

            this.FillGeneralAccounts();

            this.ListOfReferenceType = ReferenceTypeService.All.ToObservable();
            this.FillOrderDocuments();

            this.ListOfReference = ReferenceService.All
                .ApplyOnAll(x =>
                {
                    x.SetProperties(this.GeneralAccountById);
                    x.SetProperties(this.ListOfReferenceType);
                }).ToObservable();

            this.References = ListOfReference.ToDictionary(x => x.Id);
            this.FillProvider();

            this.FillInputSheets();
            this.ListOfReceiveOrder = this.ReceiveOrderService.All.ToObservable();

            this.AllInvoices = this.InvoiceService.All.ToObservable();
            this.Invoices = AllInvoices.ApplyOnAll(invoice =>
            {
                invoice.SetProperties(this.ProviderById);
                invoice.SetProperties(this.InputSheetById);
                invoice.SetProperties(this.GeneralAccountById);
                invoice.SetProperties(this.OrderDocumentById);
            }).ToDictionary(x => x.Id);

            ListOfTva = this.TvaService.All.ToObservable();
            this.FillOffices();
        }

        private void FillOrderDocuments()
        {
            var orderDocuments = OrderDocumentService.All;
            this.OrderDocuments = orderDocuments.ToObservable();
            this.OrderDocumentById = orderDocuments.ToDictionary(x => x.Id);
        }

        private void FillInputSheets()
        {
            var inputSheets = this.InputSheetService.All;
            this.ListOfInputSheet = inputSheets.ToObservable();
            this.InputSheetById = inputSheets.ToDictionary(x => x.Id);
        }

        private void FillProvider()
        {
            var providers = this.ProviderService.All;

            this.ListOfProvider = providers.ToObservable();
            this.ProviderById = providers.ToDictionary(x => x.Id);
        }

        private void FillOffices()
        {
            this.ListOfOffice = OfficeService.All
                .ApplyOnAll(x => x.SetProperties(this.AnalyticalAccountById))
                .ToObservable();

            this.Offices = ListOfOffice.ToDictionary(x => x.Id);
        }

        private void FillAnalyticalAccounts()
        {
            var analyticalAccounts = AnalyticalAccountService.All;
            this.ListOfAnalyticalAccount = analyticalAccounts.ToObservable();
            this.AnalyticalAccountById = analyticalAccounts.ToDictionary(x => x.Id);
        }

        private void FillGeneralAccounts()
        {
            this.AllGeneralAccounts = GeneralAccountService.SelectAll()
                            .ApplyOnAll(x => x.SetProperties(this.AllGeneralAccountTypes))
                            .ToObservable();

            this.GeneralAccountById = this.AllGeneralAccounts.ToDictionary(x => x.Id);
        }

        private void FillItems()
        {
            var items = this.ItemService.All;
            this.AllItems = items.ToObservable();
            this.ItemById = items.ToDictionary(x => x.Id);
        }

        private void FillPersons()
        {
            var persons = this.PersonService.All;
            this.ListOfPerson = persons.ToObservable();
            this.PersonById = persons.ToDictionary(x => x.Id);
        }

        public Dictionary<int, Invoice> Invoices { get; set; }

        public Dictionary<int, Reference> References { get; set; }

        public Dictionary<int, Office> Offices { get; set; }

        public IUIMessage UIMessage { get; set; }

        public ObservableCollection<Item> AllItems { get; set; }

        public Dictionary<int, Item> ItemById { get; set; }

        public Dictionary<int, Provider> ProviderById { get; set; }

        public Dictionary<int, GeneralAccount> GeneralAccountById { get; set; }

        public Dictionary<int, AnalyticalAccount> AnalyticalAccountById { get; set; }

        public Dictionary<int, Person> PersonById { get; set; }

        public Dictionary<int, InputSheet> InputSheetById { get; set; }

        public Dictionary<int, OrderDocument> OrderDocumentById { get; set; }

        public void BindPropertyAndSetExtended(Item item)
        {
            item.SetExtendedProperties();
            this.BindProperties(item);
        }

        public void BindProperties(Item item)
        {
            manualResetEventSlim.Wait();
            item.SetProperties(this.ListOfProposeToReformCertificate);
            item.SetProperties(this.AnalyticalAccountById);
            item.SetProperties(this.GeneralAccountById);
            item.Map(this.References);
            item.Map(this.Invoices);
            item.SetProperties(this.ItemById);
            item.SetProperties(this.PersonById);
            item.SetProperties(this.ProviderById);
            item.SetProperties(this.InputSheetById);
            item.SetProperties(this.ListOfReceiveOrder);
            item.SetProperties(this.AllTransferOrders);
            item.SetProperties(this.Offices);
            item.SetProperties(this.ListOfReformeCertificate);
            item.SetProperties(this.ListOfOutputCertificate);
            item.SetProperties(this.ListOfAccountingPeriod);
            item.SetProperties(this.AllStats);
        }

        public void MapAllItems()
        {
            manualResetEventSlim.Wait();
            this.BindProperties(this.AllItems);
        }

        public void BindProperties(IList<Item> items)
        {
            items.ParallelForEach(item => this.BindProperties(item));
        }

        public void BindPropertyAndSetExtended(IList<Item> items)
        {
            items.ParallelForEach(item => this.BindPropertyAndSetExtended(item));
        }
    }
}
