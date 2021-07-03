using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Media;
using CORESI.Data;
using CORESI.Data.Tools;
using CORESI.IoC;
using CORESI.Tools;
using CORESI.Tools.Collections;
using CORESI.WPF.Controls;
using CORESI.WPF.Core;
using CORESI.WPF.Core.Interfaces;
using CORESI.WPF.Model;
using EXGEPA.Core.Interfaces;
using EXGEPA.Model;

namespace EXGEPA.Invoice.Controls
{
    public class InvoiceViewModel : GenericEditableViewModel<Model.Invoice>
    {
        private readonly bool shouldIncludeInputSheetInInvoice;
        private CheckedRibbonButton validationButton;

        public InvoiceViewModel(IExportableGrid view) : base(view)
        {
            this.Caption = "List des factures";
            this.ParametreProvider = ServiceLocator.Resolve<IParameterProvider>();
            this.UIItemService = ServiceLocator.Resolve<IUIItemService>();
            DoubleClicAction = ItemAttribution;
            this.AddNewGroup().AddCommand("Items", IconProvider.GreaterThan, ItemAttribution);
            this.validationButton = this.AddNewGroup().AddCommand<CheckedRibbonButton>("Validée", IconProvider.Task);
            validationButton.Action = ValidateInvoice;
            this.shouldIncludeInputSheetInInvoice = this.ParameterProvider.TryGet("ShouldIncludeInputSheetInInvoice", true);
            this.AddNewGroup().AddCommand("Borecep", IconProvider.Reading, () => this.StartBackGroundAction(() => ExternalProcess.StartProcess("borecep.exe")));
            this.Selection.CollectionChanged += InvoiceViewModel_SelectionChanged;
        }

        private void InvoiceViewModel_SelectionChanged(object sender, EventArgs e)
        {
            this.InitValidationButton();
        }

        private void InitValidationButton()
        {
            if (this.SelectedRow == null)
            {
                this.validationButton.IsEnabled = false;
                return;
            }
            this.validationButton.IsEnabled = true;
            if (this.SelectedRow.IsValidated)
            {
                this.validationButton.Action = DevalidateInvoice;
                this.validationButton.IsChecked = true;
            }
            else
            {
                this.validationButton.Action = ValidateInvoice;
                this.validationButton.IsChecked = false;
            }
        }

        private void ValidateInvoice()
        {
            if (this.SelectedRow != null)
            {
                this.UIMessage.ConfirmeAndTryDoAction(Logger,
                    $"Etes vous sûr de vouloir valider la facture N°{this.SelectedRow.Key}",
                    () => this.ValidateInvoice(this.SelectedRow),
                    false, this.InitValidationButton);
            }
        }

        private void DevalidateInvoice()
        {
            if (this.SelectedRow != null)
            {
                this.UIMessage.ConfirmeAndTryDoAction(Logger,
                    $"Etes vous sûr de vouloir dévalider la facture N°{this.SelectedRow.Key}",
                    () =>
                    {
                        this.SelectedRow.IsValidated = false;
                        this.DBservice.Update(this.SelectedRow);
                        var itemDBService = ServiceLocator.Resolve<IDataProvider<Item>>();
                        Parallel.ForEach(itemDBService.All.Where(x => x?.Invoice.EqualsTo(this.SelectedRow) == true && x.IsLocked), item =>
                          {
                              item.IsLocked = false;
                              itemDBService.Update(item);
                          });

                        this.RefreshView(this.SelectedRow);
                    },
                    false, this.InitValidationButton);
            }
        }

        protected IParameterProvider ParametreProvider { get; }

        protected IUIItemService UIItemService { get; }

        private ObservableCollection<GeneralAccount> _ListOfGeneralAccount;

        public ObservableCollection<GeneralAccount> ListOfGeneralAccount
        {
            get => _ListOfGeneralAccount;
            set
            {
                _ListOfGeneralAccount = value;
                RaisePropertyChanged("ListOfGeneralAccount");
            }
        }

        private ObservableCollection<Currency> _ListOfCurrencies;

        public ObservableCollection<Currency> ListOfCurrencies
        {
            get => _ListOfCurrencies;
            set
            {
                _ListOfCurrencies = value;
                RaisePropertyChanged("ListOfCurrencies");
            }
        }

        private ObservableCollection<OrderDocument> _OrderDocuments;
        public ObservableCollection<OrderDocument> OrderDocuments
        {
            get => _OrderDocuments;
            set
            {
                _OrderDocuments = value;
                RaisePropertyChanged("OrderDocuments");
            }
        }

        private ObservableCollection<Provider> _ListOfProvider;

        public ObservableCollection<Provider> ListOfProvider
        {
            get => _ListOfProvider;
            set
            {
                _ListOfProvider = value;
                RaisePropertyChanged("ListOfProvider");
            }
        }

        private ObservableCollection<InputSheet> _ListOfInputSheet;

        public ObservableCollection<InputSheet> ListOfInputSheet
        {
            get => _ListOfInputSheet;
            set
            {
                _ListOfInputSheet = value;
                RaisePropertyChanged("ListOfInputSheet");
            }
        }

        private ObservableCollection<Project> _ListOfProject;

        public ObservableCollection<Project> ListOfProject
        {
            get => _ListOfProject;
            set
            {
                _ListOfProject = value;
                RaisePropertyChanged("ListOfProject");
            }
        }

        public override void AddItem()
        {
            Currency localCurrency = ListOfCurrencies.FirstOrDefault(x => x.Key == ParametreProvider.GetValue("LocalCurrency", "DZD"));
            this.ConcernedRow = new Model.Invoice() { Date = DateTime.Today, Key = this.GetTemporaryKey(), Currency = localCurrency };
            this.ValidateCommand = new Command(() =>
            {
                this.UIMessage.TryDoAction(Logger, () =>
                {
                    string message = "Veuillez completer les informations suivantes SVP : ";
                    bool error = false;

                    if (ConcernedRow.Key.IsNotValid())
                    {
                        message += "\n\t-Numéro de facture";
                        error = true;
                    }

                    if (ConcernedRow.Provider == null)
                    {
                        message += "\n\t-Fournisseur";
                        error = true;
                    }

                    if (this.shouldIncludeInputSheetInInvoice && ConcernedRow.InputSheet == null)
                    {
                        message += "\n\t-Fiche d'entrée";
                        error = true;
                    }

                    if (ConcernedRow.Amount == 0)
                    {
                        message += "\n\t-montant";
                        error = true;
                    }
                    if (ConcernedRow.Date.Year < 1950)
                    {
                        message += "\n\t-Date facture";
                        error = true;
                    }
                    if (error)
                    {
                        this.UIMessage.Error(message);
                        return;
                    }

                    if (localCurrency?.Id == ConcernedRow.Currency?.Id && ConcernedRow.Forex != 1)
                    {
                        this.UIMessage.Error("le taux de change doit etre egale à 1 si c'est la devise local");
                        return;
                    }

                    DBservice.Add(ConcernedRow);
                    this.ListOfRows.Add(ConcernedRow);
                    this.DisplayDetail = false;
                });
            });
            RaisePropertyChanged("ValidateCommand");
            this.DisplayDetail = true;
        }

        public void ItemAttribution()
        {
            Model.Invoice invoice = this.SelectedRow;
            if (invoice == null)
                return;

            var options = new ItemAttributionOptions
            {
                PageCaption = "Facture N°:" + invoice.Key,
                SetConfirmationMessage = "Etes vous sûr de vouloir affecter ces articles à la facture N° " + invoice.Key,
                ResetConfirmationMessage = "Etes vous sûr de vouloir retirer ces articles de la facture N° " + invoice.Key,
                RightPanelCaption = "Contenu de la facture N° " + invoice.Key,
                Tester = (item) => item.Invoice?.Id == invoice.Id,
                Setter = (item) => Setter(item, invoice),
                Resetter = (item) => item.Invoice = null,
                Categorie = new Categorie("Contenu Facture", Colors.AliceBlue)
            };
            //Group Group = new Group();
            //CheckedRibbonButton button = Group.AddCommand<CheckedRibbonButton>("Validée", IconProvider.Task);
            //button.Action = () =>
            //{
            //    //this.ValidateInvoice(invoice);
            //    //button.IsChecked = invoice.IsValidated;
            //    this.UIMessage.ConfirmeAndTryDoAction(Logger,
            //            $"Etes vous sûr de vouloir valider la facture N°{invoice.Key}",
            //            () => this.ValidateInvoice(invoice),
            //            false, () => button.IsChecked = invoice?.IsValidated ?? false);
            //};
            //options.Groups.Add(Group);
            UIItemService.ShowItemAttribution(options);
        }

        private void Setter(Item item, Model.Invoice invoice)
        {
            item.Invoice = invoice;
            item.InputSheet = invoice.InputSheet;
            item.Provider = invoice.Provider;
        }

        public void ValidateInvoice(Model.Invoice invoice)
        {
            var invoiceItems = ServiceLocator.Resolve<IDataProvider<Item>>().SelectAll().Where(x => x.Invoice?.Id == invoice.Id).ToList();
            var totalItemAmount = invoiceItems.Sum(x => x.Amount);
            bool isValidated = totalItemAmount == invoice.Amount && invoiceItems.All(x => x.IsLocked);
            if (invoice.IsValidated != isValidated)
            {
                invoice.IsValidated = isValidated;
                this.DBservice.Update(invoice);
                this.RefreshView(invoice);
            }

            if (!invoice.IsValidated)
            {
                this.UIMessage.Error("Le montant de la facture est différent du total du montant des articles ou tous les articles ne sont pas validés");
            }
        }

        public override void InitData()
        {
            StartBackGroundAction(() =>
                        {
                            IDataProvider<GeneralAccount> generalAccountService = ServiceLocator.Resolve<IDataProvider<GeneralAccount>>();
                            IDataProvider<Provider> providerService = ServiceLocator.Resolve<IDataProvider<Provider>>();
                            IDataProvider<InputSheet> inputSheetService = ServiceLocator.Resolve<IDataProvider<InputSheet>>();
                            IDataProvider<Currency> currencyService = ServiceLocator.Resolve<IDataProvider<Currency>>();
                            IDataProvider<Project> projectService = ServiceLocator.Resolve<IDataProvider<Project>>();
                            IDataProvider<OrderDocument> _OrderDocumentService = ServiceLocator.Resolve<IDataProvider<OrderDocument>>();
                            ListOfGeneralAccount = new ObservableCollection<GeneralAccount>(generalAccountService.SelectAll().Where(g => g.GeneralAccountType.Type == EGeneralAccountType.Investment));
                            ListOfProvider = new ObservableCollection<Provider>(providerService.SelectAll());
                            ListOfInputSheet = new ObservableCollection<InputSheet>(inputSheetService.SelectAll());
                            this.OrderDocuments = new ObservableCollection<OrderDocument>(_OrderDocumentService.SelectAll());
                            this.ListOfCurrencies = new ObservableCollection<Currency>(currencyService.SelectAll());
                            this.ListOfProject = new ObservableCollection<Project>(projectService.SelectAll());
                            this.ListOfRows = new ObservableCollection<Model.Invoice>(this.DBservice.SelectAll());
                            foreach (Model.Invoice invoice in this.ListOfRows)
                            {
                                invoice.InputSheet = ListOfInputSheet.FirstOrDefault(inputSheet => inputSheet.Id == invoice.InputSheet?.Id);
                                invoice.Provider = ListOfProvider.FirstOrDefault(x => x.Id == invoice.Provider?.Id);
                                invoice.Currency = this.ListOfCurrencies.FirstOrDefault(x => x.Id == invoice.Currency?.Id);
                                invoice.Project = this.ListOfProject.FirstOrDefault(x => x.Id == invoice.Project?.Id);
                                invoice.OrderDocument = this.OrderDocuments.FirstOrDefault(x => x.Id == invoice.OrderDocument?.Id);
                                invoice.HoldbackGeneralAccount = this.ListOfGeneralAccount.FirstOrDefault(x => x.Id == invoice.HoldbackGeneralAccount?.Id);
                                invoice.TvaGeneralAccount = this.ListOfGeneralAccount.FirstOrDefault(x => x.Id == invoice.TvaGeneralAccount?.Id);
                            }
                        });
        }
    }
}