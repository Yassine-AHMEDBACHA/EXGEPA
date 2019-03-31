// <copyright file="GeneralAccountViewModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace EXGEPA.Repository.Controls
{
    using System.Collections.ObjectModel;
    using System.Linq;
    using CORESI.Data;
    using CORESI.IoC;
    using CORESI.WPF.Controls;
    using CORESI.WPF.Core.Interfaces;
    using EXGEPA.Model;

    public class GeneralAccountViewModel : GenericEditableViewModel<GeneralAccount>
    {
        private ObservableCollection<GeneralAccount> listOfEndowmentAccount;
        private bool isInvestmentAccount;
        private ObservableCollection<GeneralAccount> listOfDeprciationAccount;
        private ObservableCollection<GeneralAccountType> listOfGeneralAccountType;

        public GeneralAccountViewModel(IExportableGrid exportableView)
            : base(exportableView)
        {
            this.Caption = "Liste de comptes généraux";
            this.IsInvestmentAccount = true;
        }

        public bool IsInvestmentAccount
        {
            get => this.isInvestmentAccount;
            set
            {
                this.isInvestmentAccount = value;
                this.RaisePropertyChanged("IsInvestmentAccount");
            }
        }

        public GeneralAccountType GeneralAccountType
        {
            get => this.ConcernedRow?.GeneralAccountType;
            set
            {
                this.ConcernedRow.GeneralAccountType = value;
                this.IsInvestmentAccount = value.Type == EGeneralAccountType.Investment;
                if (value.Type == EGeneralAccountType.Charge)
                {
                    this.ConcernedRow.Rate = 0;
                    this.ConcernedRow.Children = null;
                }

                this.RaisePropertyChanged(string.Empty);
            }
        }

        public ObservableCollection<GeneralAccount> ListOfDeprciationAccount
        {
            get => this.listOfDeprciationAccount;
            set
            {
                this.listOfDeprciationAccount = value;
                this.RaisePropertyChanged("ListOfDeprciationAccount");
            }
        }

        public ObservableCollection<GeneralAccount> ListOfEndowmentAccount
        {
            get => this.listOfEndowmentAccount;
            set
            {
                this.listOfEndowmentAccount = value;
                this.RaisePropertyChanged("ListOfEndowmentAccount");
            }
        }

        public ObservableCollection<GeneralAccountType> ListOfGeneralAccountType
        {
            get => this.listOfGeneralAccountType;
            set
            {
                this.listOfGeneralAccountType = value;
                this.RaisePropertyChanged("ListOfGeneralAccountType");
            }
        }

        public override void InitData()
        {
            this.StartBackGroundAction(() =>
            {
                System.Collections.Generic.IList<GeneralAccount> allAccount = this.DBservice.SelectAll(true);
                System.Collections.Generic.IList<GeneralAccountType> list = ServiceLocator.Resolve<IDataProvider<GeneralAccountType>>().SelectAll();
                foreach (GeneralAccount item in allAccount)
                {
                    item.GeneralAccountType = list.FirstOrDefault(x => x.Id == item.GeneralAccountType?.Id);
                }

                System.Collections.Generic.List<GeneralAccount> itemTodisplay = allAccount.Where(x => x.GeneralAccountType.Type == EGeneralAccountType.Investment || x.GeneralAccountType.Type == EGeneralAccountType.Charge).ToList();
                System.Collections.Generic.List<GeneralAccount> deprciationAccount = allAccount.Where(x => x.GeneralAccountType.Type == EGeneralAccountType.Depreciation).ToList();
                System.Collections.Generic.List<GeneralAccount> endowmentAccount = allAccount.Where(x => x.GeneralAccountType.Type == EGeneralAccountType.Endowment).ToList();
                this.ListOfRows = new ObservableCollection<GeneralAccount>(itemTodisplay);
                this.ListOfGeneralAccountType = new ObservableCollection<GeneralAccountType>(list.Where(x => x.Type == EGeneralAccountType.Investment || x.Type == EGeneralAccountType.Charge));
                this.ListOfDeprciationAccount = new ObservableCollection<GeneralAccount>(deprciationAccount);
                this.ListOfEndowmentAccount = new ObservableCollection<GeneralAccount>(endowmentAccount);
            });
        }

        public override void AddItem()
        {
            base.AddItem();
            this.RaisePropertyChanged("GeneralAccountType");
        }

        public override void EditItem()
        {
            base.EditItem();
            this.GeneralAccountType = this.ListOfGeneralAccountType.FirstOrDefault(x => x.Id == this.GeneralAccountType?.Id);
            this.IsInvestmentAccount = this.ConcernedRow.GeneralAccountType.Type.HasFlag(EGeneralAccountType.Investment);
            this.RaisePropertyChanged("GeneralAccountType");
        }
    }
}