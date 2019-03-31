using CORESI.Data;
using CORESI.IoC;
using CORESI.WPF.Controls;
using CORESI.WPF.Core.Interfaces;
using EXGEPA.Model;
using System.Collections.ObjectModel;
using System.Linq;

namespace EXGEPA.Repository.Controls
{
    public class AnalyticalAccountViewModel : GenericEditableViewModel<AnalyticalAccount>
    {
        private IDataProvider<AnalyticalAccountType> analyticalAccountTypeService;

        private ObservableCollection<AnalyticalAccountType> _ListOfAnalyticalAccountType;
        public ObservableCollection<AnalyticalAccountType> ListOfAnalyticalAccountType
        {
            get { return _ListOfAnalyticalAccountType; }
            set
            {
                _ListOfAnalyticalAccountType = value;
                RaisePropertyChanged("ListOfAnalyticalAccountType");
            }
        }

        public AnalyticalAccountViewModel(IExportableGrid exportableView) : base(exportableView)
        {
            ServiceLocator.Resolve(out this.analyticalAccountTypeService);
            this.Caption = "Lists de comptes analytiques";
        }



        public override void InitData()
        {
            StartBackGroundAction(() =>
            {
                this.ListOfRows = new ObservableCollection<AnalyticalAccount>(this.DBservice.SelectAll());
                ListOfAnalyticalAccountType = new ObservableCollection<AnalyticalAccountType>(analyticalAccountTypeService.SelectAll());
                foreach (var item in ListOfRows)
                {
                    item.AnalyticalAccountType = ListOfAnalyticalAccountType.Single(x => x.Id == item.AnalyticalAccountType.Id);
                }
            });

        }
    }
}