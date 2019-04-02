﻿namespace EXGEPA.Items.Controls
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Linq;
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
        public ItemExtendedProperties itemExtendedProperties;

        private string oldCodeCaption;

        public ItemViewModelBase()
        {
            this.ParameterProvider =  ServiceLocator.Resolve<IParameterProvider>();
            this.ItemService = ServiceLocator.Resolve<IDataProvider<Item>>();
            this.KeyGenerator = ServiceLocator.GetDefault<IKeyGenerator<Item>>();
            this.PicturesDirectory = ParameterProvider.GetValue("PicturesDirectory", @"C:\SQLIMMO\Images");
            AccountingPeriodHelper accountingPeriodHelper = new AccountingPeriodHelper(loadHistory: true);
            MonthelyCalculator = new MonthelyCalculator(accountingPeriodHelper);
            DailyCalculator = new DailyCalculator(accountingPeriodHelper);
            this.MinAmount = ParameterProvider.GetValue("ItemInvestismentMinAmount", 30000);
            this.KeyLength = ParameterProvider.GetValue<int>("ItemKeyLength");
            this.AddNewGroup().AddCommand("Refresh", IconProvider.Refresh, this.BindFields);
        }

        protected IParameterProvider ParameterProvider { get; }

        protected IDataProvider<Item> ItemService { get; }

        protected IKeyGenerator<Item> KeyGenerator { get; }

        protected int KeyLength { get; }

        internal void BindFields()
        {
            IDataProvider<GeneralAccountType> generalAccountTypeService = ServiceLocator.Resolve<IDataProvider<GeneralAccountType>>();
            IList<GeneralAccountType> Types = generalAccountTypeService.SelectAll();
            RepositoryDataProvider = ServiceLocator.Resolve<IRepositoryDataProvider>();
            RepositoryDataProvider.Refresh();
            RepositoryDataProvider.BindItemFields(ConcernedItem);
            foreach (GeneralAccount item in RepositoryDataProvider.ListOfGeneralAccount)
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

        
        public ICalculator MonthelyCalculator { get; set; }
        public ICalculator DailyCalculator { get; set; }

        public string OldCodeCaption
        {
            get => this.oldCodeCaption;
            set
            {
                this.oldCodeCaption = value;
                RaisePropertyChanged(nameof(this.OldCodeCaption));
            }
        }


        public string VehicleNumber
        {
            get => this.itemExtendedProperties?.VehicleNumber;
            set
            {
                this.itemExtendedProperties.VehicleNumber = value;
                RaisePropertyChanged(VehicleNumber);
            }
        }



        public Reference Reference
        {
            get => this.ConcernedItem.Reference;
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

        private IRepositoryDataProvider _RepositoryDataProvider;
        public IRepositoryDataProvider RepositoryDataProvider
        {
            get => _RepositoryDataProvider;
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
            get => ConcernedItem.ExcludedFromInventory;
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
            get => _ListOfDailyDepreciation;
            set
            {
                _ListOfDailyDepreciation = value;
                RaisePropertyChanged("ListOfDailyDepreciation");
            }
        }
        private ObservableCollection<Depreciation> _ListOfMonthelyDepreciation;
        public ObservableCollection<Depreciation> ListOfMonthelyDepreciation
        {
            get => _ListOfMonthelyDepreciation;
            set
            {
                _ListOfMonthelyDepreciation = value;
                RaisePropertyChanged("ListOfMonthelyDepreciation");
            }
        }


        public decimal PreviousDepreciation
        {
            get => this.ConcernedItem.PreviousDepreciation;
            set
            {
                this.ConcernedItem.PreviousDepreciation = value;
                this.UpdateDepreciationBase();
                RaisePropertyChanged("PreviousDepreciation");
            }
        }

        public GeneralAccount GeneralAccount
        {
            get => ConcernedItem.GeneralAccount;
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
            get => ConcernedItem.ItemState;
            set
            {
                ConcernedItem.ItemState = value;
                RaisePropertyChanged("ItemState");
            }

        }
        private ComboBoxRibbon<int> _Quantity;
        public ComboBoxRibbon<int> Quantity
        {
            get => _Quantity;
            set
            {
                _Quantity = value;
                RaisePropertyChanged("Quantity");
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
                    AnalyticalAccount = ConcernedItem.Office.AnalyticalAccount;
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
                    return null;
                else
                    return PicturesDirectory + ConcernedItem.ImagePath;
            }
            set
            {
                string target = "I" + this.ConcernedItem.Id.ToString() + Path.GetExtension(value);
                if (string.IsNullOrEmpty(value))
                {
                    string s = this.ImagePath;
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
            get => _IsOldItem;
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
            this.LimiteDate = DepriciationHelper.GetLimiteDate(rate, this.AquisitionDate);
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
            decimal depreciationBase = this.IsTvaDepreciatible ? this.Amount : this.AmountHT;

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
            get => ConcernedItem.Key;

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
                    ConcernedItem.Invoice = null;
                RaisePropertyChanged("Invoice");
                RaisePropertyChanged("TransferOrder");
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
                RaisePropertyChanged("AnalyticalAccount");
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