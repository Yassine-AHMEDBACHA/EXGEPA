namespace EXGEPA.Items.Controls
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using CORESI.Data;
    using CORESI.IoC;
    using CORESI.WPF.Controls;
    using CORESI.WPF.Core;
    using CORESI.WPF.Core.Framework;
    using CORESI.WPF.Model;
    using EXGEPA.Core.Interfaces;
    using EXGEPA.Depreciations.Core;
    using EXGEPA.Model;


    public abstract class ItemViewModelBase : PageViewModel
    {
        protected readonly IParameterProvider parameterProvider;

        protected readonly IKeyGenerator<Item> keyGenerator;

        public ItemExtendedProperties itemExtendedProperties;

        protected int keyLength { get; set; }
        public ItemViewModelBase()
        {
            ServiceLocator.Resolve(out this.itemService);
            ServiceLocator.Resolve(out this.parameterProvider);
            ServiceLocator.GetDefault(out this.keyGenerator);
            this.PicturesDirectory = parameterProvider.GetValue("PicturesDirectory", @"C:\SQLIMMO\Images");
            var accountingPeriodHelper = new AccountingPeriodHelper(loadHistory: true);
            MonthelyCalculator = new MonthelyCalculator(accountingPeriodHelper);
            DailyCalculator = new DailyCalculator(accountingPeriodHelper);
            this.MinAmount = parameterProvider.GetValue("ItemInvestismentMinAmount", 30000);
            this.keyLength = parameterProvider.GetValue<int>("ItemKeyLength");
            this.AddNewGroup().AddCommand("Refresh", IconProvider.Refresh, this.BindFields);
            Task.Factory.StartNew(() => this.ListOfItems = new ObservableCollection<Item>(this.itemService.SelectAll()));

        }

        internal void BindFields()
        {
            var generalAccountTypeService = ServiceLocator.Resolve<IDataProvider<GeneralAccountType>>();
            var Types = generalAccountTypeService.SelectAll();
            RepositoryDataProvider = ServiceLocator.Resolve<IRepositoryDataProvider>();
            RepositoryDataProvider.Refresh();
            RepositoryDataProvider.BindItemFields(ConcernedItem);
            foreach (var item in RepositoryDataProvider.ListOfGeneralAccount)
            {
                item.GeneralAccountType = Types.FirstOrDefault(x => x.Id == item.GeneralAccountType.Id);
            }

            SetAccoutToDisplay();
            RaisePropertyChanged();
        }

        public virtual void SetAccoutToDisplay()
        {
            IEnumerable<GeneralAccount> accounts;
            if (this.Amount >= this.MinAmount)
            {
                accounts = RepositoryDataProvider?.ListOfGeneralAccount.Where(x => x.GeneralAccountType.Type == EGeneralAccountType.Investment);
            }
            else
            {
                accounts = RepositoryDataProvider?.ListOfGeneralAccount.Where(x => x.GeneralAccountType.Type == EGeneralAccountType.Charge);
            }
            this.ListOfGeneralAccount = new ObservableCollection<GeneralAccount>(accounts ?? Enumerable.Empty<GeneralAccount>());
        }

        protected Action ManageChargeAccount;

        public virtual void UpdateDescription(Reference reference)
        {
            this.SmallDescription = this.Description = reference?.Caption ?? string.Empty;
        }

        protected readonly IDataProvider<Item> itemService;
        public ICalculator MonthelyCalculator { get; set; }
        public ICalculator DailyCalculator { get; set; }

        public string VehicleNumber
        {
            get
            {
                return this.itemExtendedProperties?.VehicleNumber;
            }
            set
            {
                this.itemExtendedProperties.VehicleNumber = value;
                RaisePropertyChanged(VehicleNumber);
            }
        }



        public Reference Reference
        {
            get
            {
                return this.ConcernedItem.Reference;
            }
            set
            {
                this.UpdateKey(value);
                this.UpdateDescription(value);
                this.UpdateGeneralAccount(value);
                this.ConcernedItem.Reference = value;
                RaisePropertyChanged("Reference");
            }
        }

        private void UpdateKey(Reference value)
        {
            this.Key = value == null ? string.Empty : keyGenerator.GenerateKey(value, this.keyLength);
        }

        public virtual void UpdateGeneralAccount(Reference reference)
        {
            if (reference != null)
            {
                this.GeneralAccount = this.Amount < this.MinAmount ? reference.ChargeAccount : reference.InvestmentAccount;
            }
            this.SetAccoutToDisplay();
        }

        private IRepositoryDataProvider _RepositoryDataProvider;
        public IRepositoryDataProvider RepositoryDataProvider
        {
            get { return _RepositoryDataProvider; }
            set
            {
                _RepositoryDataProvider = value;
                RaisePropertyChanged("RepositoryDataProvider");
            }
        }

        #region Settings
        public Item ConcernedItem { get; set; }
        protected string PicturesDirectory { get; set; }
        public Item InitialItem { get; set; }
        public bool ExcludedFromInventory
        {
            get { return ConcernedItem.ExcludedFromInventory; }
            set
            {
                ConcernedItem.ExcludedFromInventory = value;
                RaisePropertyChanged("ExcludedFromInventory");
            }
        }
        public decimal MinAmount { get; set; }

        private ObservableCollection<Depreciation> _ListOfDailyDepreciation;
        public ObservableCollection<Depreciation> ListOfDailyDepreciation
        {
            get { return _ListOfDailyDepreciation; }
            set
            {
                _ListOfDailyDepreciation = value;
                RaisePropertyChanged("ListOfDailyDepreciation");
            }
        }
        private ObservableCollection<Depreciation> _ListOfMonthelyDepreciation;
        public ObservableCollection<Depreciation> ListOfMonthelyDepreciation
        {
            get { return _ListOfMonthelyDepreciation; }
            set
            {
                _ListOfMonthelyDepreciation = value;
                RaisePropertyChanged("ListOfMonthelyDepreciation");
            }
        }


        public decimal PreviousDepreciation
        {
            get
            {
                return this.ConcernedItem.PreviousDepreciation;
            }
            set
            {
                this.ConcernedItem.PreviousDepreciation = value;
                this.UpdateDepreciationBase();
                RaisePropertyChanged("PreviousDepreciation");
            }
        }

        public GeneralAccount GeneralAccount
        {
            get
            {
                return ConcernedItem.GeneralAccount;
            }
            set
            {
                this.UpdateFiscaleRate(value);
                this.ConcernedItem.GeneralAccount = value;
                RaisePropertyChanged("GeneralAccount");
            }
        }

        public virtual void UpdateFiscaleRate(GeneralAccount generalAccount)
        {
            if (generalAccount != null)
            {
                this.FiscaleRate = generalAccount.Rate;
            }
        }


        public ItemState ItemState
        {
            get { return ConcernedItem.ItemState; }
            set
            {
                ConcernedItem.ItemState = value;
                RaisePropertyChanged("ItemState");
            }

        }
        private ComboBoxRibbon<int> _Quantity;
        public ComboBoxRibbon<int> Quantity
        {
            get { return _Quantity; }
            set
            {
                _Quantity = value;
                RaisePropertyChanged("Quantity");
            }

        }



        public Office Office
        {

            get { return ConcernedItem.Office; }
            set
            {
                ConcernedItem.Office = value;
                if (value != null)
                {
                    AnalyticalAccount = ConcernedItem.Office.AnalyticalAccount;
                }

                RaisePropertyChanged("Office");
            }
        }

        public Provider Provider
        {
            get { return ConcernedItem.Provider; }
            set
            {
                ConcernedItem.Provider = value;
                RaisePropertyChanged("Provider");
            }
        }

        internal Action _SavePicture;
        public string ImagePath
        {
            get
            {
                if (string.IsNullOrEmpty(ConcernedItem?.ImagePath))
                    return null;
                else
                    return PicturesDirectory + ConcernedItem.ImagePath;
            }
            set
            {
                string target = "I" + this.ConcernedItem.Id.ToString() + Path.GetExtension(value);
                if (string.IsNullOrEmpty(value))
                {
                    var s = this.ImagePath;
                    _SavePicture = () => { DeleteImage(s); };
                    target = null;
                }
                else
                {

                    _SavePicture = () => { CopyPicture(value, target); };
                }
                ConcernedItem.ImagePath = target;
                RaisePropertyChanged("ImagePath");
            }
        }


        private bool _IsOldItem;
        public bool IsOldItem
        {
            get { return _IsOldItem; }
            set
            {
                _IsOldItem = value;
                RaisePropertyChanged("IsOldItem");
            }
        }

        private void CopyPicture(string sourcePath, string target)
        {
            if (!Directory.Exists(PicturesDirectory))
                Directory.CreateDirectory(PicturesDirectory);
            File.Copy(sourcePath, PicturesDirectory + target, true);
        }
        private void DeleteImage(string path)
        {
            File.Delete(path);
        }



        public decimal FiscaleRate
        {
            get { return ConcernedItem.FiscalRate; }
            set
            {
                this.ConcernedItem.FiscalRate = value;
                this.UpdateLimiteDate(value);
                RaisePropertyChanged("FiscaleRate");
            }
        }

        public void UpdateLimiteDate(decimal rate)
        {
            this.LimiteDate = DepriciationHelper.GetLimiteDate(rate, this.AquisitionDate);
        }

        public string Comment
        {
            get { return ConcernedItem.Comment; }
            set
            {
                ConcernedItem.Comment = value;
                RaisePropertyChanged("Comment");
            }
        }
        public string OldCode
        {
            get { return ConcernedItem.OldCode; }
            set
            {
                ConcernedItem.OldCode = value;
                RaisePropertyChanged("OldCode");
            }
        }

        public Item Owner
        {
            get { return ConcernedItem.Owner; }
            set
            {
                ConcernedItem.Owner = value;
                RaisePropertyChanged("Owner");
            }
        }

        private ObservableCollection<Item> _ListOfItems;
        public ObservableCollection<Item> ListOfItems
        {
            get { return _ListOfItems; }
            set
            {
                _ListOfItems = value;
                RaisePropertyChanged("ListOfItems");
            }
        }

        public decimal DepreciationBase
        {
            get { return ConcernedItem.DepreciationBase; }
            set
            {
                ConcernedItem.DepreciationBase = value;
                this.UpdateGeneralAccount(this.Reference);
                RaisePropertyChanged("DepreciationBase");
            }
        }
        public decimal Amount
        {
            get { return ConcernedItem.Amount; }
            set
            {
                ConcernedItem.Amount = value;
                this.UpdateAmountHT(value);
                RaisePropertyChanged("Amount");
            }
        }

        private void UpdateAmountHT(decimal amount)
        {
            var amountHt = amount;
            if (this.Tva != null)
            {
                amountHt = amount - (amount * this.Tva.Rate / 100);
            }

            this.TvaAmount = amount - amountHt;
            this.AmountHT = amountHt;
        }

        public DateTime AquisitionDate
        {
            get
            {
                return ConcernedItem.AquisitionDate;
            }
            set
            {
                ConcernedItem.AquisitionDate = value;
                this.UpdateStartServiceDate();
                RaisePropertyChanged("AquisitionDate");
            }
        }

        private void UpdateStartServiceDate()
        {
            if (this.StartServiceDate < this.AquisitionDate)
            {
                this.StartServiceDate = this.AquisitionDate;
            }
            this.StartServiceDate = this.StartServiceDate;
        }

        public DateTime StartServiceDate
        {
            get { return ConcernedItem.StartServiceDate; }
            set
            {
                ConcernedItem.StartServiceDate = value;
                this.UpdateOfficeAffectationDate();
                this.UpdateUserAssignmentStartDate();
                this.UpdateLimiteDate(this.FiscaleRate);
                RaisePropertyChanged("StartServiceDate");
            }
        }

        private void UpdateOfficeAffectationDate()
        {
            if (this.StartServiceDate > this.OfficeAssignmentStartDate)
            {
                this.OfficeAssignmentStartDate = this.StartServiceDate;
            }
        }

        private void UpdateUserAssignmentStartDate()
        {
            if (this.StartServiceDate > this.UserAssignmentStartDate)
            {
                this.UserAssignmentStartDate = this.StartServiceDate;
            }
        }

        public bool IsTvaDepreciatible
        {
            get { return ConcernedItem.IsTvaDepreciatible; }
            set
            {
                ConcernedItem.IsTvaDepreciatible = value;
                this.UpdateAmountHT(this.Amount);
                RaisePropertyChanged("IsTvaDepreciatible");
            }
        }
        public Tva Tva
        {
            get { return ConcernedItem.Tva; }
            set
            {
                ConcernedItem.Tva = value;
                this.UpdateAmountHT(this.Amount);
                RaisePropertyChanged("Tva");
            }
        }
        public decimal AmountHT
        {
            get { return ConcernedItem.AmountHT; }
            set
            {
                ConcernedItem.AmountHT = value;
                this.UpdateDepreciationBase();
                RaisePropertyChanged("AmountHT");
            }
        }

        private void UpdateDepreciationBase()
        {
            var depreciationBase = this.IsTvaDepreciatible ? this.Amount : this.AmountHT;

            this.DepreciationBase = depreciationBase > this.PreviousDepreciation ? (depreciationBase - this.PreviousDepreciation) : depreciationBase;
        }

        public decimal TvaAmount
        {
            get { return ConcernedItem.TvaAmount; }
            set
            {
                ConcernedItem.TvaAmount = value;
                RaisePropertyChanged("TvaAmount");
            }
        }
        public DateTime LimiteDate
        {
            get { return ConcernedItem.LimiteDate; }
            set
            {
                this.ConcernedItem.LimiteDate = value;
                this.UpdateDepreciations();
                this.RaisePropertyChanged("LimiteDate");
            }
        }

        private void UpdateDepreciations()
        {
            if (this.ConcernedItem != null)
            {
                this.ListOfMonthelyDepreciation = new ObservableCollection<Depreciation>(MonthelyCalculator.GetDepriciations(this.ConcernedItem, this.AquisitionDate, this.LimiteDate));
                this.ListOfDailyDepreciation = new ObservableCollection<Depreciation>(DailyCalculator.GetDepriciations(this.ConcernedItem, this.AquisitionDate, this.LimiteDate));
            }
        }

        public string Key
        {
            get { return ConcernedItem.Key; }

            set
            {
                ConcernedItem.Key = value;
                RaisePropertyChanged("Key");
            }
        }

        public bool IsAccountingInformationLocked
        {
            get
            {
                return (this.Invoice?.IsValidated) ?? false;
            }
        }

        public Invoice Invoice
        {
            get { return ConcernedItem.Invoice; }
            set
            {
                ConcernedItem.Invoice = value;
                if (value != null)
                {
                    ConcernedItem.TransferOrder = null;
                    Provider = ConcernedItem.Invoice.Provider;
                    InputSheet = ConcernedItem.Invoice.InputSheet;
                }
                else
                {
                    Provider = null;
                }
                RaisePropertyChanged("Invoice");
                RaisePropertyChanged("TransferOrder");
            }
        }
        public Person Person
        {
            get { return ConcernedItem.Person; }
            set
            {
                ConcernedItem.Person = value;
                RaisePropertyChanged("Person");
            }
        }
        public TransferOrder TransferOrder
        {
            get { return ConcernedItem.TransferOrder; }
            set
            {
                ConcernedItem.TransferOrder = value;
                if (value != null)
                    ConcernedItem.Invoice = null;
                RaisePropertyChanged("Invoice");
                RaisePropertyChanged("TransferOrder");
            }
        }
        public InputSheet InputSheet
        {
            get { return ConcernedItem.InputSheet; }
            set
            {
                ConcernedItem.InputSheet = value;
                RaisePropertyChanged("InputSheet");
            }
        }
        public ReceiveOrder ReceiveOrder
        {
            get { return ConcernedItem.ReceiveOrder; }
            set
            {
                ConcernedItem.ReceiveOrder = value;
                RaisePropertyChanged("ReceiveOrder");
            }
        }
        public AnalyticalAccount AnalyticalAccount
        {
            get { return ConcernedItem.AnalyticalAccount; }
            set
            {
                ConcernedItem.AnalyticalAccount = value;
                RaisePropertyChanged("AnalyticalAccount");
            }
        }

        public string Description
        {
            get
            {
                return ConcernedItem.Description;
            }
            set
            {
                ConcernedItem.Description = value;
                RaisePropertyChanged("Description");
            }
        }

        public string SmallDescription
        {
            get
            {
                return ConcernedItem.SmallDescription;
            }
            set
            {
                ConcernedItem.SmallDescription = value;
                RaisePropertyChanged("SmallDescription");
            }
        }

        public string Brand
        {
            get
            {
                return ConcernedItem.Brand;
            }
            set
            {
                ConcernedItem.Brand = value;
                RaisePropertyChanged("Brand");
            }
        }
        public string Model
        {
            get
            {
                return ConcernedItem.Model;
            }
            set
            {
                ConcernedItem.Model = value;
                RaisePropertyChanged("Model");
            }
        }
        public string SerialNumber
        {
            get
            {
                return ConcernedItem.SerialNumber;
            }
            set
            {
                ConcernedItem.SerialNumber = value;
                RaisePropertyChanged("SerialNumber");
            }
        }
        public bool PrintLabel
        {
            get { return ConcernedItem.PrintLabel; }
            set
            {
                ConcernedItem.PrintLabel = value;
                RaisePropertyChanged("PrintLabel");
            }
        }
        public int ElementCount
        {
            get
            {
                return ConcernedItem.ElementCount;
            }
            set
            {
                ConcernedItem.ElementCount = value;
                RaisePropertyChanged("ElementCount");
            }
        }
        public DateTime OfficeAssignmentStartDate
        {
            get
            {
                return ConcernedItem.OfficeAssignmentStartDate;
            }
            set
            {
                ConcernedItem.OfficeAssignmentStartDate = value;
                RaisePropertyChanged("OfficeAssignmentStartDate");
            }
        }
        public DateTime UserAssignmentStartDate
        {
            get
            {
                return ConcernedItem.UserAssignmentStartDate;
            }
            set
            {
                ConcernedItem.UserAssignmentStartDate = value;
                RaisePropertyChanged("UserAssignmentStartDate");
            }
        }

        private bool isBaseDepreciationReadOnly;

        public bool IsBaseDepreciationReadOnly
        {
            get { return isBaseDepreciationReadOnly; }
            set
            {
                isBaseDepreciationReadOnly = value;
                RaisePropertyChanged("IsBaseDepreciationReadOnly");
            }
        }
        protected void Notify(Item item)
        {
            ItemGridViewModel.NotifyUpdate?.Invoke(this, item);
        }

        private ObservableCollection<GeneralAccount> _ListOfGeneralAccount;

        public ObservableCollection<GeneralAccount> ListOfGeneralAccount
        {
            get
            {
                return _ListOfGeneralAccount;
            }
            set
            {
                _ListOfGeneralAccount = value; RaisePropertyChanged("ListOfGeneralAccount");
            }
        }

        public bool _IsKeyReadOnly;
        public bool IsKeyReadOnly
        {
            get
            {
                return _IsKeyReadOnly;
            }
            set
            {
                _IsKeyReadOnly = value;
                RaisePropertyChanged("IsKeyReadOnly");
            }
        }

        #endregion
    }
}
