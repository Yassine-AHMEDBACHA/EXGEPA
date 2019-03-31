using CORESI.Data;
using CORESI.IoC;
using CORESI.Tools;
using CORESI.Tools.DateTimeTools;
using EXGEPA.Depreciations.Core;
using EXGEPA.Model;
using EXGEPA.Report.Commun;
using EXGEPA.Report.Controls;
using System.Linq;

namespace EXGEPA.Report.InvestismentRecap
{
    public class RecapByGeneralAccountPreparator : ReportPreparator
    {
        private readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public RecapByGeneralAccountPreparator() : base()
        { }

        public ReportWrapperViewModel GetReportWrapper()
        {
            var reportWrapper = new ReportWrapperViewModel("Recap general d'investissement");
            using (var scooplogger = new ScoopLogger("Loading Data", this.logger, false))
            {
                var AllGeneralAccount = ServiceLocator.Resolve<IDataProvider<GeneralAccount>>().SelectAll();
                var recapRows = ServiceLocator.Resolve<IDataProvider<GeneralAccount>>().SelectAll().Select(ga => new RecapRow(ga)).ToList();




                var currentPeriod = AccountingPeriodsService.SelectAll().FirstOrDefault(x => !x.Approved);
                this.RepositoryDataProvider.Refresh();
                scooplogger.Snap("Refreshing repository");

                scooplogger.Snap("getting min value");
                var Items = this.ItemService.SelectAll().Where(item => item.GeneralAccount.GeneralAccountType.Type == EGeneralAccountType.Investment && item.AquisitionDate <= currentPeriod.EndDate).ToList();

                var Calculator = new MonthelyCalculator(new AccountingPeriodHelper());
                scooplogger.Snap("Loading Items");
                RepositoryDataProvider.BindItemFields(Items);
                var groups = Items.Where(item => item.GeneralAccount.GeneralAccountType.Type == EGeneralAccountType.Investment).GroupBy(x => x.GeneralAccount);
                Calculator.GetDepriciation(Items, currentPeriod.StartDate, currentPeriod.EndDate);
                foreach (var group in groups)
                {
                    var recapRow = recapRows.First(row => row.GeneralAccount.Id == group.Key.Id);
                    recapRow.aquisitionAmount = group.Where(x => x.AquisitionDate.Between(currentPeriod.StartDate, currentPeriod.EndDate)).Sum(item => item.Amount);
                    recapRow.PreviousDepreciation = group.Sum(x => x.Depreciations.Sum(d => d.PreviousDepreciation));
                    recapRow.Depreciation = group.Sum(x => x.Depreciations.Sum(d => d.Annuity));
                    recapRow.InitialAmount = group.Where(x => x.AquisitionDate < currentPeriod.StartDate).Sum(item => item.Amount);
                    recapRow.OutputAmount = group.Where(x => x.OutputCertificate != null && x.OutputCertificate.Date.Between(currentPeriod.StartDate, currentPeriod.EndDate)).Sum(item => item.Amount);
                }
                scooplogger.Snap("Grouping Items");



                var report = new InvestismentRecapByGeneralAcount();
                report.Periode.Text = currentPeriod.Key;

                var companyName = ParameterProvider.GetValue<string>("CompanyName");
                var logo = @"C:\SQLIMMO\Images\logo.jpg";
                report.CompanyName.Text = companyName;
                report.Logo.ImageUrl = logo;

                reportWrapper.DocumentSource = report;

                reportWrapper.DocumentSource.DataSource = recapRows;



            }
            reportWrapper.DocumentSource.CreateDocument();
            return reportWrapper;
        }
    }
}
