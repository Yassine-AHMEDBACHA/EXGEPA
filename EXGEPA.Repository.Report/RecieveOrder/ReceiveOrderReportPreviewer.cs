using System.IO;
using System.Linq;
using CORESI.IoC;
using CORESI.WPF.Core;
using CORESI.WPF.Model;
using EXGEPA.Model;
using EXGEPA.Report;
using EXGEPA.Report.RecieveOrder;

namespace EXGEPA.Repository.Report
{
    public class ReceiveOrderReportPreviewer : AReportPreviwer<ReceiveOrder>
    {
        public override Group GetGroupForReportBottons()
        {
            var group = new Group();
            group.AddCommand("Etat de bons de receptions", IconProvider.Reading, this.PrintSheet);
            return group;
        }

        public void PrintSheet()
        {
            var data = this.GetDataToDisplay();
            if (data == null)
            {
                return;
            }

            var AccountingPeriodsService = ServiceLocator.Resolve<CORESI.Data.IDataProvider<AccountingPeriod>>();
            var currentPeriod = AccountingPeriodsService.SelectAll().FirstOrDefault(x => !x.Approved);
            var parameterProvider = ServiceLocator.Resolve<CORESI.Data.IParameterProvider>();
            var companyName = parameterProvider.GetValue<string>("CompanyName");
            var logo = Path.Combine(parameterProvider.GetValue("PicturesDirectory", @"C:\SQLIMMO\Images"), parameterProvider.GetValue("LogoFileName", "logo.jpg")); ;
            var report = new RecieveOrderSheet();
            report.SheetTitle.Text = "Liste des Réceptions";
            report.DataSource = data;
            report.CompanyName.Text = companyName;
            report.Logo.ImageUrl = logo;
            report.CreateDocument();
            var page = CORESI.Report.Controls.ReportViewModel.GetModulePage(report.SheetTitle.Text, report);
            this.UIService.AddPage(page);
        }
    }
}
