namespace EXGEPA.Items.Controls
{
    using System;
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Linq;
    using CORESI.Data;
    using CORESI.Data.Tools;
    using CORESI.IoC;
    using CORESI.Security;
    using CORESI.Tools.Collections;
    using CORESI.WPF.Controls;
    using CORESI.WPF.Core;
    using CORESI.WPF.Core.Framework;
    using CORESI.WPF.Model;
    using EXGEPA.Core.Interfaces;
    using EXGEPA.Depreciations.Core;
    using EXGEPA.Model;

    public abstract class ItemViewModelBase : PageViewModel
    {
        //public ItemExtendedProperties itemExtendedProperties;

        protected Action manageChargeAccount;

        private readonly ICalculator monthelyCalculator;

        private readonly ICalculator dailyCalculator;
        protected readonly RightManager rightManager;

        public ItemViewModelBase()
        {
            ServiceLocator.Resolve(out this.rightManager);
            this.IsLockActivated = this.rightManager.HasAccess($"{nameof(Item)}-FullUpdate");
            this.ParameterProvider = ServiceLocator.Resolve<IParameterProvider>();
            this.ItemService = ServiceLocator.Resolve<IDataProvider<Item>>();
            this.KeyGenerator = ServiceLocator.GetDefault<IKeyGenerator<Item>>();
            this.RepositoryDataProvider = ServiceLocator.Resolve<IRepositoryDataProvider>();
            this.PicturesDirectory = ParameterProvider.GetValue("PicturesDirectory", @"C:\SQLIMMO\Images");
            this.monthelyCalculator = new MonthelyCalculator(null);
            this.dailyCalculator = new DailyCalculator();
            this.MinAmount = ParameterProvider.GetValue("ItemInvestismentMinAmount", 30000);
            this.KeyLength = ParameterProvider.GetValue<int>("ItemKeyLength");
            this.AddNewGroup().AddCommand("Refresh", IconProvider.Refresh, this.RefreshView);
            this.OldCodeCaption = this.ParameterProvider.TryGet("OldCodeCaption", "IMMO");
        }

        public IRepositoryDataProvider RepositoryDataProvider { get; }

        public bool IsLockActivated { get; set; }

        protected IParameterProvider ParameterProvider { get; }

        protected IDataProvider<Item> ItemService { get; }

        protected IKeyGenerator<Item> KeyGenerator { get; }

        protected int KeyLength { get; }



        private void RefreshView()
        {
            this.RepositoryDataProvider.Refresh();
            this.BindFields();
            this.RaisePropertyChanged(string.Empty);
        }

        protected void BindFields()
        {
            RepositoryDataProvider.BindProperties(ConcernedItem);
            SetAccoutToDisplay();
        }

        public virtual void SetAccoutToDisplay()
        {
            var filter = this.Amount >= this.MinAmount ? EGeneralAccountType.Investment : EGeneralAccountType.Charge;
            this.RepositoryDataProvider.WaitTillDataReady();
            var list = this.RepositoryDataProvider.AllGeneralAccounts
                ?.Where(x => x.GeneralAccountType.Type == filter).ToList();

            if (this.GeneralAccount != null && !list.Contains(this.GeneralAccount))
            {
                list.Add(this.GeneralAccount);
            }
            var account = this.GeneralAccount;
            this.ListOfGeneralAccount = list.ToObservable();
            this.GeneralAccount = account;
        }

        public virtual void UpdateDescription(Reference reference)
        {
            this.SmallDescription = this.Description = reference?.Caption ?? string.Empty;
        }

        public string OldCodeCaption { get; set; }

        public string VehicleNumber
        {
            get => ConcernedItem.ExtendedProperties?.VehicleNumber;

            set
            {
                ConcernedItem.ExtendedProperties.VehicleNumber = value;
                RaisePropertyChanged();
            }
        }

        public Reference Reference
        {
            get => this.ConcernedItem.Reference;
            set
            {
                this.UpdateKey(value);
                this.UpdateDescription(value);
                this.UpdateImage(value);
                this.UpdateGeneralAccount(value);
                this.ConcernedItem.Reference = value;
                RaisePropertyChanged("Reference");
            }
        }

        private void UpdateImage(Reference value)
        {
            if (value?.ImagePath.IsValid() == true)
            {
                this.ImagePath = Path.Combine(this.PicturesDirectory, value.ImagePath);
            }
        }

        private void UpdateKey(Reference value)
        {
            this.Key = value == null ? string.Empty : KeyGenerator.GenerateKey(value, this.KeyLength);
        }

        public virtual void UpdateGeneralAccount(Reference reference)
        {
            if (reference != null)
            {
                this.GeneralAccount = this.Amount < this.MinAmount ? reference.ChargeAccount : reference.InvestmentAccount;
            }

            this.SetAccoutToDisplay();
        }

        #region Settings

        public Item ConcernedItem { get; set; }
        protected string PicturesDirectory { get; set; }
        public Item InitialItem { get; set; }
        public bool ExcludedFromInventory
        {
            get => ConcernedItem.ExcludedFromInventory;
            set
            {
                ConcernedItem.ExcludedFromInventory = value;
                RaisePropertyChanged();
            }
        }
        public decimal MinAmount { get; set; }

        private ObservableCollection<Depreciation> _ListOfDailyDepreciation;
        public ObservableCollection<Depreciation> ListOfDailyDepreciation
        {
            get => _ListOfDailyDepreciation;
            set
            {
                _ListOfDailyDepreciation = value;
                RaisePropertyChanged();
            }
        }

        private ObservableCollection<Depreciation> _ListOfMonthelyDepreciation;

        public ObservableCollection<Depreciation> ListOfMonthelyDepreciation
        {
            get => _ListOfMonthelyDepreciation;
            set
            {
                _ListOfMonthelyDepreciation = value;
                RaisePropertyChanged();
            }
        }

        public decimal PreviousDepreciation
        {
            get => this.ConcernedItem.PreviousDepreciation;
            set
            {
                this.ConcernedItem.PreviousDepreciation = value;
                this.UpdateDepreciationBase();
                RaisePropertyChanged();
            }
        }

        public GeneralAccount GeneralAccount
        {
            get => ConcernedItem.GeneralAccount;
            set
            {
                this.UpdateFiscaleRate(value);
                this.ConcernedItem.GeneralAccount = value;
                RaisePropertyChanged();
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
            get => ConcernedItem.ItemState;
            set
            {
                ConcernedItem.ItemState = value;
                RaisePropertyChanged();
            }

        }

        private ComboBoxRibbon<int> _Quantity;

        public ComboBoxRibbon<int> Quantity
        {
            get => _Quantity;
            set
            {
                _Quantity = value;
                RaisePropertyChanged();
            }
        }

        public Office Office
        {
            get => ConcernedItem.Office;
            set
            {
                ConcernedItem.Office = value;
                if (value != null)
                {
                    this.AnalyticalAccount = ConcernedItem.Office.AnalyticalAccount;
                }

                RaisePropertyChanged("Office");
            }
        }

        public Provider Provider
        {
            get => ConcernedItem.Provider;
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
                {
                    if (string.IsNullOrEmpty(this.Reference?.ImagePath))
                    {
                        return null;
                    }

                    return Path.Combine(PicturesDirectory, this.Reference.ImagePath);

                }

                return Path.Combine(PicturesDirectory, ConcernedItem.ImagePath);
            }
            set
            {
                var target = string.Empty;
                if (string.IsNullOrEmpty(value))
                {
                    string s = this.ImagePath;
                    _SavePicture = () => { DeleteImage(s); };
                    target = null;
                }
                else
                {
                    target = $"Item_{this.ConcernedItem.Key}{Path.GetExtension(value)}";
                    _SavePicture = () => { CopyPicture(value, target); };
                }

                ConcernedItem.ImagePath = target;
                RaisePropertyChanged("ImagePath");
            }
        }


        private bool _IsOldItem;
        public bool IsOldItem
        {
            get => _IsOldItem;
            set
            {
                _IsOldItem = value;
                RaisePropertyChanged();
            }
        }

        private void CopyPicture(string sourcePath, string target)
        {
            if (!Directory.Exists(PicturesDirectory))
                Directory.CreateDirectory(PicturesDirectory);

            target = Path.Combine(this.PicturesDirectory, target);
            File.Copy(sourcePath, target, true);
        }

        private void DeleteImage(string path)
        {
            if (this.RepositoryDataProvider.AllItems.All(x => !path.Contains(x.ImagePath)))
            {
                File.Delete(path);
            }
        }

        public void UpdatePreviouseDepreciationDate(DateTime limiteDate)
        {
            if (PreviouseDepreciationDate < AquisitionDate || PreviouseDepreciationDate >= limiteDate)
            {
                PreviouseDepreciationDate = AquisitionDate;
            }
        }

        public decimal FiscaleRate
        {
            get => ConcernedItem.FiscalRate;
            set
            {
                this.ConcernedItem.FiscalRate = value;
                this.UpdateLimiteDate(value);
                RaisePropertyChanged("FiscaleRate");
            }
        }

        public void UpdateLimiteDate(decimal rate)
        {
            var limiteDate = DepriciationHelper.GetLimiteDate(rate, AquisitionDate);
            UpdatePreviouseDepreciationDate(limiteDate);
            this.LimiteDate = limiteDate;
        }

        public string Comment
        {
            get => ConcernedItem.Comment;
            set
            {
                ConcernedItem.Comment = value;
                RaisePropertyChanged("Comment");
            }
        }
        public string OldCode
        {
            get => ConcernedItem.OldCode;
            set
            {
                ConcernedItem.OldCode = value;
                RaisePropertyChanged("OldCode");
            }
        }

        public Item Owner
        {
            get => ConcernedItem.Owner;
            set
            {
                ConcernedItem.Owner = value;
                RaisePropertyChanged("Owner");
            }
        }

        public decimal DepreciationBase
        {
            get => ConcernedItem.DepreciationBase;
            set
            {
                ConcernedItem.DepreciationBase = value;
                this.UpdateGeneralAccount(this.Reference);
                RaisePropertyChanged("DepreciationBase");
            }
        }
        public decimal Amount
        {
            get => ConcernedItem.Amount;
            set
            {
                ConcernedItem.Amount = value;
                this.UpdateAmountHT(value);
                RaisePropertyChanged("Amount");
            }
        }

        private void UpdateAmountHT(decimal amount)
        {
            decimal amountHt = amount;
            if (this.Tva != null)
            {
                amountHt = amount - (amount * this.Tva.Rate / 100);
            }

            this.TvaAmount = amount - amountHt;
            this.AmountHT = amountHt;
        }

        public DateTime AquisitionDate
        {
            get => ConcernedItem.AquisitionDate;
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
            get => ConcernedItem.StartServiceDate;
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
            get => ConcernedItem.IsTvaDepreciatible;
            set
            {
                ConcernedItem.IsTvaDepreciatible = value;
                this.UpdateAmountHT(this.Amount);
                RaisePropertyChanged("IsTvaDepreciatible");
            }
        }
        public Tva Tva
        {
            get => ConcernedItem.Tva;
            set
            {
                ConcernedItem.Tva = value;
                this.UpdateAmountHT(this.Amount);
                RaisePropertyChanged("Tva");
            }
        }
        public decimal AmountHT
        {
            get => ConcernedItem.AmountHT;
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
            get => ConcernedItem.TvaAmount;
            set
            {
                ConcernedItem.TvaAmount = value;
                RaisePropertyChanged("TvaAmount");
            }
        }
        public DateTime LimiteDate
        {
            get => ConcernedItem.LimiteDate;
            set
            {
                this.ConcernedItem.LimiteDate = value;
                this.UpdateDepreciations();
                this.RaisePropertyChanged("LimiteDate");
            }
        }

        protected void UpdateDepreciations()
        {
            if (ConcernedItem != null && AquisitionDate <= LimiteDate)
            {
                this.ListOfMonthelyDepreciation = this.monthelyCalculator.GetDepriciations(this.ConcernedItem, this.AquisitionDate, this.LimiteDate)
                    .ToObservable();
                this.ListOfDailyDepreciation = this.dailyCalculator.GetDepriciations(this.ConcernedItem, this.AquisitionDate, this.LimiteDate)
                    .ToObservable();
            }
            else
            {
                this.ListOfMonthelyDepreciation?.Clear();
                this.ListOfDailyDepreciation?.Clear();
            }

        }

        public string Key
        {
            get => ConcernedItem.Key;

            set
            {
                ConcernedItem.Key = value;
                RaisePropertyChanged("Key");
            }
        }

        public bool IsAccountingInformationLocked => (this.Invoice?.IsValidated) ?? false;

        public Invoice Invoice
        {
            get => ConcernedItem.Invoice;
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

        public DateTime PreviouseDepreciationDate
        {
            get => ConcernedItem.ExtendedProperties.PreviouseDepreciationDate;
            set
            {
                ConcernedItem.ExtendedProperties.PreviouseDepreciationDate = value;
                UpdateDepreciations();
                RaisePropertyChanged();
            }
        }

        public Person Person
        {
            get => ConcernedItem.Person;
            set
            {
                ConcernedItem.Person = value;
                RaisePropertyChanged("Person");
            }
        }
        public TransferOrder TransferOrder
        {
            get => ConcernedItem.TransferOrder;
            set
            {
                ConcernedItem.TransferOrder = value;
                if (value != null)
                {
                    this.ConcernedItem.Invoice = null;
                    this.ConcernedItem.InputSheet = null;
                }

                RaisePropertyChanged("Invoice");
                RaisePropertyChanged("TransferOrder");
                RaisePropertyChanged("InputSheet");
            }
        }
        public InputSheet InputSheet
        {
            get => ConcernedItem.InputSheet;
            set
            {
                ConcernedItem.InputSheet = value;
                RaisePropertyChanged("InputSheet");
            }
        }
        public ReceiveOrder ReceiveOrder
        {
            get => ConcernedItem.ReceiveOrder;
            set
            {
                ConcernedItem.ReceiveOrder = value;
                RaisePropertyChanged("ReceiveOrder");
            }
        }
        public AnalyticalAccount AnalyticalAccount
        {
            get => ConcernedItem.AnalyticalAccount;
            set
            {
                ConcernedItem.AnalyticalAccount = value;
                RaisePropertyChanged();
            }
        }

        public string Description
        {
            get => ConcernedItem.Description;
            set
            {
                ConcernedItem.Description = value;
                RaisePropertyChanged("Description");
            }
        }

        public string SmallDescription
        {
            get => ConcernedItem.SmallDescription;
            set
            {
                ConcernedItem.SmallDescription = value;
                RaisePropertyChanged("SmallDescription");
            }
        }

        public string Brand
        {
            get => ConcernedItem.Brand;
            set
            {
                ConcernedItem.Brand = value;
                RaisePropertyChanged("Brand");
            }
        }
        public string Model
        {
            get => ConcernedItem.Model;
            set
            {
                ConcernedItem.Model = value;
                RaisePropertyChanged("Model");
            }
        }
        public string SerialNumber
        {
            get => ConcernedItem.SerialNumber;
            set
            {
                ConcernedItem.SerialNumber = value;
                RaisePropertyChanged("SerialNumber");
            }
        }
        public bool PrintLabel
        {
            get => ConcernedItem.PrintLabel;
            set
            {
                ConcernedItem.PrintLabel = value;
                RaisePropertyChanged("PrintLabel");
            }
        }
        public int ElementCount
        {
            get => ConcernedItem.ElementCount;
            set
            {
                ConcernedItem.ElementCount = value;
                RaisePropertyChanged("ElementCount");
            }
        }
        public DateTime OfficeAssignmentStartDate
        {
            get => ConcernedItem.OfficeAssignmentStartDate;
            set
            {
                ConcernedItem.OfficeAssignmentStartDate = value;
                RaisePropertyChanged("OfficeAssignmentStartDate");
            }
        }
        public DateTime UserAssignmentStartDate
        {
            get => ConcernedItem.UserAssignmentStartDate;
            set
            {
                ConcernedItem.UserAssignmentStartDate = value;
                RaisePropertyChanged("UserAssignmentStartDate");
            }
        }

        private bool isBaseDepreciationReadOnly;

        public bool IsBaseDepreciationReadOnly
        {
            get => isBaseDepreciationReadOnly;
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
            get => _ListOfGeneralAccount;
            set
            {
                _ListOfGeneralAccount = value; RaisePropertyChanged("ListOfGeneralAccount");
            }
        }

        public bool _IsKeyReadOnly;
        public bool IsKeyReadOnly
        {
            get => _IsKeyReadOnly;
            set
            {
                _IsKeyReadOnly = value;
                RaisePropertyChanged("IsKeyReadOnly");
            }
        }

        #endregion


    }
}
