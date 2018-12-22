using System.IO;
using CORESI.Data;
using CORESI.IoC;
using CORESI.WPF.Core;
using CORESI.WPF.Model;
using EXGEPA.Core;
using EXGEPA.Model;
using EXGEPA.Report;
using EXGEPA.Report.AnalyticalAccounts;

namespace EXGEPA.Repository.Report.AnalyticalAccounts
{
    public class AnalyticalAccountReportPreviewer : AReportPreviwer<AnalyticalAccount>
    {
        public override Group GetGroupForReportBottons()
        {
            var group = new Group();
            group.AddCommand("Etat des comptes analytiques", IconProvider.Reading, this.PrintSheet);
            return group;
        }

        public void PrintSheet()
        {
            var service = ServiceLocator.Resolve<IDataProvider<AnalyticalAccount>>();
            var data = service.SelectAll();
            if (data == null)
            {
                this.UIMessage.Information("Aucun Compte analytique !");
                return;
            }
            var parameterProvider = ServiceLocator.Resolve<CORESI.Data.IParameterProvider>();
            var companyName = parameterProvider.GetValue<string>("CompanyName");
            var logo = Path.Combine(parameterProvider.GetValue("PicturesDirectory", @"C:\SQLIMMO\Images"), parameterProvider.GetValue("LogoFileName", "logo.jpg"));
            var report = new AnalyticalAccountSheet();
            report.SheetTitle.Text = "Liste des Comptes Analytiques";
            report.DataSource = data;
            report.CompanyName.Text = companyName;
            report.Logo.ImageUrl = logo;
            report.CreateDocument();
            var page = CORESI.Report.Controls.ReportViewModel.GetModulePage(report.SheetTitle.Text, report);

            this.UIService.AddPage(page);

        }
    }
}
