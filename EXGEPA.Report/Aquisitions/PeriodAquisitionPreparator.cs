using CORESI.Tools;
using CORESI.Tools.DateTimeTools;
using EXGEPA.Model;
using EXGEPA.Report.Commun;
using EXGEPA.Report.Controls;
using System.Collections.Generic;
using System.Linq;

namespace EXGEPA.Report.Aquisitions
{
    public class PeriodAquisitionPreparator : ReportPreparator
    {
        private readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        public PeriodAquisitionPreparator() : base()
        { }


        public ReportWrapperViewModel GetReportWrapper(bool onlyInvestissment = true)
        {
            ReportWrapperViewModel reportWrapper = new ReportWrapperViewModel("Etat des immobilisations acquises");
            using (ScoopLogger scooplogger = new ScoopLogger("Loading Data", this.logger, false))
            {
                AccountingPeriod currentPeriod = AccountingPeriodsService.SelectAll().FirstOrDefault(x => !x.Approved);
                List<Item> items = this.LoadItem(onlyInvestissment).Where(x => x.AquisitionDate.Between(currentPeriod.StartDate, currentPeriod.EndDate)).ToList();
                PeriodAquisition report = new PeriodAquisition();
                report.Periode.Text = currentPeriod.Key;
                string companyName = ParameterProvider.GetValue<string>("CompanyName");
                string logo = @"C:\SQLIMMO\Images\logo.jpg";
                report.CompanyName.Text = companyName;
                report.Logo.ImageUrl = logo;
                reportWrapper.DocumentSource = report;
                reportWrapper.DocumentSource.DataSource = items;
            }
            reportWrapper.DocumentSource.CreateDocument();
            return reportWrapper;
        }

        private IList<Item> LoadItem(bool onlyInvestissment = true)
        {
            IList<Item> items = this.ItemService.SelectAll();
            if (onlyInvestissment)
            {
                items = items.Where(x => x.GeneralAccount.GeneralAccountType.Type == EGeneralAccountType.Investment).ToList();
            }
            else
            {
                items = items.Where(x => x.GeneralAccount.GeneralAccountType.Type == EGeneralAccountType.Charge).ToList();
            }
            this.RepositoryDataProvider.BindProperties(items);
            return items;
        }
    }
}
