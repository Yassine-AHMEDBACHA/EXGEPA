using CORESI.Data;
using CORESI.IoC;
using CORESI.WPF;
using EXGEPA.Core.Interfaces;
using EXGEPA.Model;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace EXGEPA.Core
{
    [Export(typeof(IRepositoryDataProvider)), PartCreationPolicy(CreationPolicy.NonShared)]
    
    public class RepositoryDataProvider : UiNotifier, IRepositoryDataProvider
    {
        static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

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

        ManualResetEventSlim _ManualResetEventSlim;
        public RepositoryDataProvider()
        {
            ServiceLocator.ComposeParts(this);
            this.UIMessage = ServiceLocator.GetPriorizedInstance<IUIMessage>();
            Refresh();
        }

        public void Refresh()
        {
            _ManualResetEventSlim = new ManualResetEventSlim();
            UIMessage.TryDoActionAsync(logger, loadData, _ManualResetEventSlim.Set);
        }

        #region Data Providers

        private ObservableCollection<ItemState> _ListOfStats;

        public ObservableCollection<ItemState> ListOfStats
        {
            get { return _ListOfStats; }
            set
            {
                _ListOfStats = value;
                RaisePropertyChanged("ListOfStats");
            }
        }

        private ObservableCollection<ProposeToReformCertificate> _ListOfProposeToReformCertificate;

        public ObservableCollection<ProposeToReformCertificate> ListOfProposeToReformCertificate
        {
            get { return _ListOfProposeToReformCertificate; }
            set
            {
                _ListOfProposeToReformCertificate = value;
                RaisePropertyChanged("ListOfProposeToReformCertificate");
            }
        }



        private ObservableCollection<Tva> _ListOfTva;
        public ObservableCollection<Tva> ListOfTva
        {
            get { return _ListOfTva; }
            set
            {
                _ListOfTva = value;
                RaisePropertyChanged("ListOfTva");
            }
        }

        private ObservableCollection<ReformeCertificate> _ListOfReformeCertificate;
        public ObservableCollection<ReformeCertificate> ListOfReformeCertificate
        {
            get { return _ListOfReformeCertificate; }
            set { _ListOfReformeCertificate = value; }
        }

        private ObservableCollection<OutputCertificate> _ListOfOutputCertificate;
        public ObservableCollection<OutputCertificate> ListOfOutputCertificate
        {
            get { return _ListOfOutputCertificate; }
            set { _ListOfOutputCertificate = value; }
        }

        private ObservableCollection<GeneralAccount> _ListOfGeneralAccount;
        public ObservableCollection<GeneralAccount> ListOfGeneralAccount
        {
            get { return _ListOfGeneralAccount; }
            set { _ListOfGeneralAccount = value; }
        }

        private ObservableCollection<Reference> _ListOfReference;
        public ObservableCollection<Reference> ListOfReference
        {
            get { return _ListOfReference; }
            set { _ListOfReference = value; }
        }

        private ObservableCollection<ReferenceType> _ListOfReferenceType;
        public ObservableCollection<ReferenceType> ListOfReferenceType
        {
            get { return _ListOfReferenceType; }
            set { _ListOfReferenceType = value; }
        }

        private ObservableCollection<Invoice> _ListOfInvoice;
        public ObservableCollection<Invoice> ListOfInvoice
        {
            get { return _ListOfInvoice; }
            set { _ListOfInvoice = value; }
        }

        private ObservableCollection<TransferOrder> _ListOfTransferOrder;
        public ObservableCollection<TransferOrder> ListOfTransferOrder
        {
            get { return _ListOfTransferOrder; }
            set { _ListOfTransferOrder = value; }
        }



        private ObservableCollection<Provider> _ListOfProvider;
        public ObservableCollection<Provider> ListOfProvider
        {
            get { return _ListOfProvider; }
            set { _ListOfProvider = value; }
        }

        private ObservableCollection<OrderDocument> _OrderDocuments;
        public ObservableCollection<OrderDocument> OrderDocuments
        {
            get { return _OrderDocuments; }
            set { _OrderDocuments = value; }
        }

        private ObservableCollection<AnalyticalAccount> _ListOfAnalyticalAccount;
        public ObservableCollection<AnalyticalAccount> ListOfAnalyticalAccount
        {
            get { return _ListOfAnalyticalAccount; }
            set { _ListOfAnalyticalAccount = value; }
        }

        private ObservableCollection<Office> _ListOfOffice;
        public ObservableCollection<Office> ListOfOffice
        {
            get { return _ListOfOffice; }
            set { _ListOfOffice = value; }
        }

        private ObservableCollection<Depreciation> _ListOfDepreciation;
        public ObservableCollection<Depreciation> ListOfDepreciation
        {
            get { return _ListOfDepreciation; }
            set { _ListOfDepreciation = value; }
        }

        private ObservableCollection<InputSheet> _ListOfInputSheet;

        public ObservableCollection<InputSheet> ListOfInputSheet
        {
            get { return _ListOfInputSheet; }
            set { _ListOfInputSheet = value; }
        }

        private ObservableCollection<ReceiveOrder> _ListOfReceiveOrder;

        public ObservableCollection<ReceiveOrder> ListOfReceiveOrder
        {
            get { return _ListOfReceiveOrder; }
            set { _ListOfReceiveOrder = value; }
        }

        private ObservableCollection<Person> _ListOfPerson;

        public ObservableCollection<Person> ListOfPerson
        {
            get { return _ListOfPerson; }
            set
            {
                _ListOfPerson = value;

            }
        }

        private ObservableCollection<AccountingPeriod> _ListOfAccountingPeriod;

        public ObservableCollection<AccountingPeriod> ListOfAccountingPeriod
        {
            get { return _ListOfAccountingPeriod; }
            set
            {
                _ListOfAccountingPeriod = value;

            }
        }

        #endregion

        public void loadData()
        {
            FillRepositories();
            RaisePropertyChanged("");
        }

        public void FillRepositories()
        {
            _ManualResetEventSlim.Reset();
            ListOfStats = new ObservableCollection<ItemState>(ItemStateService.SelectAll());
            ListOfProposeToReformCertificate = new ObservableCollection<ProposeToReformCertificate>(ProposeToReformCertificateService.SelectAll());
            ListOfAccountingPeriod = new ObservableCollection<AccountingPeriod>(AccountingPeriodService.SelectAll());
            ListOfReformeCertificate = new ObservableCollection<ReformeCertificate>(ReformeCertificateService.SelectAll());
            ListOfOutputCertificate = new ObservableCollection<OutputCertificate>(OutputCertificateService.SelectAll());
            ListOfPerson = new ObservableCollection<Person>(PersonService.SelectAll());
            ListOfTransferOrder = new ObservableCollection<EXGEPA.Model.TransferOrder>(TransferOrderService.SelectAll());
            ListOfGeneralAccount = new ObservableCollection<GeneralAccount>(GeneralAccountService.SelectAll());
            this.ListOfReferenceType = new ObservableCollection<ReferenceType>(ReferenceTypeService.SelectAll());
            ListOfReference = new ObservableCollection<Reference>(ReferenceService.SelectAll());
            OrderDocuments = new ObservableCollection<OrderDocument>(OrderDocumentService.SelectAll());
            ListOfReference.ToList().ForEach(reference =>
            {
                reference.ChargeAccount = ListOfGeneralAccount.FirstOrDefault(x => x.Id == reference.ChargeAccount?.Id);
                reference.InvestmentAccount = ListOfGeneralAccount.FirstOrDefault(x => x.Id == reference.InvestmentAccount?.Id);
                reference.ReferenceType = ListOfReferenceType.FirstOrDefault(x => x.Id == reference.ReferenceType?.Id);
            });
            References = ListOfReference.ToDictionary(x => x.Id);
            ListOfProvider = new ObservableCollection<Provider>(ProviderService.SelectAll());
            ListOfInvoice = new ObservableCollection<Invoice>(InvoiceService.SelectAll());
            ListOfInputSheet = new ObservableCollection<InputSheet>(InputSheetService.SelectAll());
            ListOfReceiveOrder = new ObservableCollection<ReceiveOrder>(ReceiveOrderService.SelectAll());
            ListOfInvoice.ToList().ForEach(invoice =>
            {
                invoice.Provider = ListOfProvider.FirstOrDefault(provider => provider.Id == invoice.Provider?.Id);
                invoice.InputSheet = ListOfInputSheet.FirstOrDefault(inputSheet => inputSheet.Id == invoice.InputSheet?.Id);
                invoice.HoldbackGeneralAccount = ListOfGeneralAccount.FirstOrDefault(x => x.Id == invoice.HoldbackGeneralAccount?.Id);
                invoice.TvaGeneralAccount = ListOfGeneralAccount.FirstOrDefault(x => x.Id == invoice.TvaGeneralAccount?.Id);
                
                invoice.OrderDocument = OrderDocuments.FirstOrDefault(x => x.Id == invoice.OrderDocument?.Id);
            });
            Invoices = ListOfInvoice.ToDictionary(x => x.Id);
            ListOfAnalyticalAccount = new ObservableCollection<AnalyticalAccount>(AnalyticalAccountService.SelectAll());
            ListOfDepreciation = new ObservableCollection<Depreciation>();
            ListOfTva = new ObservableCollection<Tva>(TvaService.SelectAll());
            var ListOfOffices = OfficeService.SelectAll();
            ListOfOffice = new ObservableCollection<Office>(ListOfOffices);
            Parallel.ForEach(ListOfOffices, office =>
            {
                var x = office.AnalyticalAccount?.Id;
                office.AnalyticalAccount = ListOfAnalyticalAccount.FirstOrDefault(analyticalAccount => analyticalAccount.Id == office.AnalyticalAccount?.Id);
            });
            Offices = ListOfOffices.ToDictionary(x => x.Id);
        }

        public Dictionary<int, Invoice> Invoices { get; set; }
        public Dictionary<int, Reference> References { get; set; }
        public Dictionary<int, Office> Offices { get; set; }
        IUIMessage UIMessage { get; set; }

        public void BindItemFields(Item item)
        {
            _ManualResetEventSlim.Wait();
            item.ProposeToReformCertificate = ListOfProposeToReformCertificate.FirstOrDefault(certificate => certificate.Id == item.ProposeToReformCertificate?.Id);
            item.AnalyticalAccount = ListOfAnalyticalAccount.FirstOrDefault(account => account.Id == item.AnalyticalAccount?.Id);
            item.GeneralAccount = ListOfGeneralAccount.FirstOrDefault(account => account.Id == item.GeneralAccount?.Id);
            if (item.Reference != null)
            {
                Reference reference;
                References.TryGetValue(item.Reference.Id, out reference);
                item.Reference = reference;
            }

            if (item.Invoice != null)
            {
                Invoice invoice;
                Invoices.TryGetValue(item.Invoice.Id, out invoice);
                item.Invoice = invoice;
            }

            item.Person = ListOfPerson.FirstOrDefault(person => person.Id == item.Person?.Id);

            item.Provider = ListOfProvider.FirstOrDefault(provider => provider.Id == item.Provider?.Id);

            item.InputSheet = ListOfInputSheet.FirstOrDefault(inputSheet => inputSheet.Id == item.InputSheet?.Id);

            item.ReceiveOrder = ListOfReceiveOrder.FirstOrDefault(receiveOrder => receiveOrder.Id == item.ReceiveOrder?.Id);

            item.TransferOrder = ListOfTransferOrder.FirstOrDefault(transferOrder => transferOrder.Id == item.TransferOrder?.Id);
            if (item.Office != null)
            {
                Office office;
                Offices.TryGetValue(item.Office.Id, out office);
                item.Office = office;
            }

            item.ReformeCertificate = ListOfReformeCertificate.FirstOrDefault(reformeCertificate => reformeCertificate.Id == item.ReformeCertificate?.Id);
            
            item.OutputCertificate = ListOfOutputCertificate.FirstOrDefault(outputCertificate => outputCertificate.Id == item.OutputCertificate?.Id);
            
            item.AccountingPeriod = ListOfAccountingPeriod.FirstOrDefault(accountingPeriod => accountingPeriod.Id == item.AccountingPeriod?.Id);
            
            item.ItemState = ListOfStats.FirstOrDefault(state => state.Id == item.ItemState?.Id);
        }

        public void BindItemFields(IList<Item> items)
        {
            Parallel.ForEach(items, item =>
             {
                 this.BindItemFields(item);
             });
        }
    }
}
