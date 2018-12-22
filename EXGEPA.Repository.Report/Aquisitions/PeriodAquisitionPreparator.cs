using CORESI.Tools;
using CORESI.Tools.DateTimeTools;
using EXGEPA.Report.Commun;
using EXGEPA.Report.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EXGEPA.Report.Aquisitions
{
    public class PeriodAquisitionPreparator : ReportPreparator
    {
        private readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public PeriodAquisitionPreparator() : base()
        { }


        public ReportWrapperViewModel GetReportWrapper()
        {
            var reportWrapper = new ReportWrapperViewModel("Etat des immobilisations acquises");
            using (var scooplogger = new ScoopLogger("Loading Data", this.logger, false))
            {
                var currentPeriod = AccountingPeriodsService.SelectAll().FirstOrDefault(x => !x.Approved);
                var items = this.ItemService.SelectAll().Where(x => x.AquisitionDate.Between(currentPeriod.StartDate, currentPeriod.EndDate, true)).ToList();
                this.RepositoryDataProvider.BindItemFields(items);
                var report = new PeriodAquisition();
                report.Periode.Text = currentPeriod.Code;
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
    }
}
