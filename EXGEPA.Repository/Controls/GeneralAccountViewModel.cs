using System.Collections.ObjectModel;
using System.Linq;
using CORESI.Data;
using CORESI.IoC;
using CORESI.WPF.Controls;
using CORESI.WPF.Core.Interfaces;
using EXGEPA.Model;

namespace EXGEPA.Repository.Controls
{
    public class GeneralAccountViewModel : GenericEditableViewModel<GeneralAccount>
    {
        public GeneralAccountViewModel(IExportable exportableView) : base(exportableView)
        {
            this.Caption = "Liste de comptes généraux";
            this.IsInvestmentAccount = true;
        }

        private bool isInvestmentAccount;
        public bool IsInvestmentAccount
        {
            get { return isInvestmentAccount; }
            set
            {
                isInvestmentAccount = value;
                this.RaisePropertyChanged("IsInvestmentAccount");
            }
        }

        public GeneralAccountType GeneralAccountType
        {
            get { return this.ConcernedRow?.GeneralAccountType; }
            set
            {
                this.ConcernedRow.GeneralAccountType = value;
                this.IsInvestmentAccount = value.Type == EGeneralAccountType.Investment;
                if (value.Type == EGeneralAccountType.Charge)
                {
                    this.ConcernedRow.Rate = 0;
                    this.ConcernedRow.Children = null;
                }

                this.RaisePropertyChanged("");
            }
        }


        private ObservableCollection<GeneralAccount> _ListOfDeprciationAccount;
        public ObservableCollection<GeneralAccount> ListOfDeprciationAccount
        {
            get
            {
                return _ListOfDeprciationAccount;
            }
            set
            {
                _ListOfDeprciationAccount = value;
                RaisePropertyChanged("ListOfDeprciationAccount");
            }
        }

        private ObservableCollection<GeneralAccount> _ListOfEndowmentAccount;
        public ObservableCollection<GeneralAccount> ListOfEndowmentAccount
        {
            get
            {
                return _ListOfEndowmentAccount;
            }
            set
            {
                _ListOfEndowmentAccount = value;
                RaisePropertyChanged("ListOfEndowmentAccount");
            }
        }



        private ObservableCollection<GeneralAccountType> _ListOfGeneralAccountType;

        public ObservableCollection<GeneralAccountType> ListOfGeneralAccountType
        {
            get
            {
                return _ListOfGeneralAccountType;
            }
            set
            {
                _ListOfGeneralAccountType = value;
                RaisePropertyChanged("ListOfGeneralAccountType");
            }
        }

        public override void InitData()
        {
            StartBackGroundAction(() =>
            {
                var allAccount = this.DBservice.SelectAll(true);
                var list = ServiceLocator.Resolve<IDataProvider<GeneralAccountType>>().SelectAll();
                foreach (var item in allAccount)
                {
                    item.GeneralAccountType = list.FirstOrDefault(x => x.Id == item.GeneralAccountType?.Id);
                }

                var itemTodisplay = allAccount.Where(x => x.GeneralAccountType.Type == EGeneralAccountType.Investment || x.GeneralAccountType.Type == EGeneralAccountType.Charge).ToList();
                var deprciationAccount = allAccount.Where(x => x.GeneralAccountType.Type == EGeneralAccountType.Depreciation).ToList();
                var endowmentAccount = allAccount.Where(x => x.GeneralAccountType.Type == EGeneralAccountType.Endowment).ToList();
                this.ListOfRows = new ObservableCollection<GeneralAccount>(itemTodisplay);
                this.ListOfGeneralAccountType = new ObservableCollection<GeneralAccountType>(list.Where(x => x.Type == EGeneralAccountType.Investment || x.Type == EGeneralAccountType.Charge));
                this.ListOfDeprciationAccount = new ObservableCollection<GeneralAccount>(deprciationAccount);
                this.ListOfEndowmentAccount = new ObservableCollection<GeneralAccount>(endowmentAccount);
            });
        }

        public override void AddItem()
        {
            base.AddItem();
            RaisePropertyChanged("GeneralAccountType");
        }

        public override void EditItem()
        {
            base.EditItem();
            this.GeneralAccountType = this.ListOfGeneralAccountType.FirstOrDefault(x => x.Id == this.GeneralAccountType?.Id);
            this.IsInvestmentAccount = this.ConcernedRow.GeneralAccountType.Type.HasFlag(EGeneralAccountType.Investment);
            RaisePropertyChanged("GeneralAccountType");
        }



    }
}