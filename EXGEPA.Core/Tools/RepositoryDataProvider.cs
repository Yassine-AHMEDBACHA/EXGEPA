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

        public ObservableCollection<Invoice> ListOfInvoice { get; set; }

        public ObservableCollection<TransferOrder> ListOfTransferOrder { get; set; }

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
            this.AllItems = this.ItemService.All.ToObservable();
            this.AllGeneralAccountTypes = this.GeneralAccountTypeService.All
                .ToObservable();
            this.AllStats = this.ItemStateService.All.ToObservable();
            this.ListOfProposeToReformCertificate = this.ProposeToReformCertificateService.All.ToObservable();
            this.ListOfAccountingPeriod = this.AccountingPeriodService.All.ToObservable();
            this.ListOfReformeCertificate = ReformeCertificateService.All.ToObservable();
            this.ListOfOutputCertificate = OutputCertificateService.All.ToObservable();
            this.ListOfPerson = PersonService.All.ToObservable();
            this.ListOfAnalyticalAccount = AnalyticalAccountService.All.ToObservable();
            this.ListOfTransferOrder = TransferOrderService.All
                .ApplyOnAll(x => x.SetProperties(this.ListOfAnalyticalAccount))
                .ToObservable();

            this.AllGeneralAccounts = GeneralAccountService.All
                .ApplyOnAll(x => x.SetProperties(this.AllGeneralAccountTypes))
                .ToObservable();

            this.ListOfReferenceType = ReferenceTypeService.All.ToObservable();
            OrderDocuments = OrderDocumentService.All.ToObservable();

            this.ListOfReference = ReferenceService.All
                .ApplyOnAll(x => x.SetProperties(this.AllGeneralAccounts))
                .ApplyOnAll(x => x.SetProperties(this.ListOfReferenceType))
                .ToObservable();

            References = ListOfReference.ToDictionary(x => x.Id);
            ListOfProvider = this.ProviderService.All.ToObservable();
            ListOfInvoice = this.InvoiceService.All.ToObservable();
            ListOfInputSheet = this.InputSheetService.All.ToObservable();
            ListOfReceiveOrder = this.ReceiveOrderService.All.ToObservable();
            this.Invoices = ListOfInvoice.ApplyOnAll(invoice =>
            {
                invoice.SetProperties(this.ListOfProvider);
                invoice.SetProperties(this.ListOfInputSheet);
                invoice.SetProperties(this.AllGeneralAccounts);
                invoice.SetProperties(this.OrderDocuments);
            }).ToDictionary(x => x.Id);

            ListOfTva = this.TvaService.All.ToObservable();
            ListOfOffice = OfficeService.All.ToObservable();
            ListOfOffice.ParallelForEach(office => office.SetProperties(this.ListOfAnalyticalAccount));
            Offices = ListOfOffice.ToDictionary(x => x.Id);
        }

        public Dictionary<int, Invoice> Invoices { get; set; }

        public Dictionary<int, Reference> References { get; set; }

        public Dictionary<int, Office> Offices { get; set; }

        public IUIMessage UIMessage { get; set; }

        public ObservableCollection<Item> AllItems { get; set; }

        public void BindItemFields(Item item)
        {
            manualResetEventSlim.Wait();
            item.SetProperties(this.ListOfProposeToReformCertificate);
            item.SetProperties(this.ListOfAnalyticalAccount);
            item.SetProperties(this.AllGeneralAccounts);
            item.SetProperties(this.References);
            item.SetProperties(this.Invoices);
            item.SetProperties(this.AllItems);
            item.SetProperties(this.ListOfPerson);
            item.SetProperties(this.ListOfProvider);
            item.SetProperties(this.ListOfInputSheet);
            item.SetProperties(this.ListOfReceiveOrder);
            item.SetProperties(this.ListOfTransferOrder);
            item.SetProperties(Offices);
            item.SetProperties(this.ListOfReformeCertificate);
            item.SetProperties(this.ListOfOutputCertificate);
            item.SetProperties(this.ListOfAccountingPeriod);
            item.SetProperties(this.AllStats);
        }

        public void BindItemFields(IList<Item> items)
        {
            items.ParallelForEach(item => this.BindItemFields(item));
        }
    }
}
