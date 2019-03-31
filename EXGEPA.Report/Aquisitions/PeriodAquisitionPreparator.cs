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
            var reportWrapper = new ReportWrapperViewModel("Etat des immobilisations acquises");
            using (var scooplogger = new ScoopLogger("Loading Data", this.logger, false))
            {
                var currentPeriod = AccountingPeriodsService.SelectAll().FirstOrDefault(x => !x.Approved);
                var items = this.LoadItem(onlyInvestissment).Where(x => x.AquisitionDate.Between(currentPeriod.StartDate, currentPeriod.EndDate, true)).ToList();
                var report = new PeriodAquisition();
                report.Periode.Text = currentPeriod.Key;
                var companyName = ParameterProvider.GetValue<string>("CompanyName");
                var logo = @"C:\SQLIMMO\Images\logo.jpg";
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
            var items = this.ItemService.SelectAll();
            if (onlyInvestissment)
            {
                items = items.Where(x => x.GeneralAccount.GeneralAccountType.Type == EGeneralAccountType.Investment).ToList();
            }
            else
            {
                items = items.Where(x => x.GeneralAccount.GeneralAccountType.Type == EGeneralAccountType.Charge).ToList();
            }
            this.RepositoryDataProvider.BindItemFields(items);
            return items;
        }
    }
}
