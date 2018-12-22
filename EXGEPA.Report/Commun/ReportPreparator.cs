using CORESI.Data;
using CORESI.IoC;
using DevExpress.XtraReports.UI;
using EXGEPA.Core.Interfaces;
using EXGEPA.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EXGEPA.Report.Commun
{
    public abstract class ReportPreparator
    {
        protected IParameterProvider ParameterProvider { get; set; }
        protected IDataProvider<Item> ItemService { get; set; }
        protected IRepositoryDataProvider RepositoryDataProvider { get; set; }
        protected IDataProvider<AccountingPeriod> AccountingPeriodsService { get; set; }
        public ReportPreparator()
        {
            this.AccountingPeriodsService = ServiceLocator.Resolve<IDataProvider<AccountingPeriod>>();
            ParameterProvider = ServiceLocator.Resolve<IParameterProvider>();
            ItemService = ServiceLocator.Resolve<IDataProvider<Item>>();
            RepositoryDataProvider = ServiceLocator.Resolve<IRepositoryDataProvider>();
        }

        
    }
}
