// <copyright file="AnalyticalAccountViewModel.cs" company="PlaceholderCompany">
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

    public class AnalyticalAccountViewModel : GenericEditableViewModel<AnalyticalAccount>
    {
        private readonly IDataProvider<AnalyticalAccountType> analyticalAccountTypeService;

        private ObservableCollection<AnalyticalAccountType> listOfAnalyticalAccountType;

        public AnalyticalAccountViewModel(IExportableGrid exportableView)
            : base(exportableView)
        {
            ServiceLocator.Resolve(out this.analyticalAccountTypeService);
            this.Caption = "Lists de comptes analytiques";
        }

        public ObservableCollection<AnalyticalAccountType> ListOfAnalyticalAccountType
        {
            get => this.listOfAnalyticalAccountType;
            set
            {
                this.listOfAnalyticalAccountType = value;
                this.RaisePropertyChanged("ListOfAnalyticalAccountType");
            }
        }

        public override void InitData()
        {
            this.StartBackGroundAction(() =>
            {
                this.ListOfRows = new ObservableCollection<AnalyticalAccount>(this.DBservice.SelectAll());
                this.ListOfAnalyticalAccountType = new ObservableCollection<AnalyticalAccountType>(this.analyticalAccountTypeService.SelectAll());
                foreach (AnalyticalAccount item in this.ListOfRows)
                {
                    item.AnalyticalAccountType = this.ListOfAnalyticalAccountType.Single(x => x.Id == item.AnalyticalAccountType.Id);
                }
            });
        }
    }
}