using System.IO;
using System.Linq;
using CORESI.IoC;
using CORESI.WPF.Core;
using CORESI.WPF.Model;
using EXGEPA.Model;
using EXGEPA.Report;
using EXGEPA.Report.InputSheet;

namespace EXGEPA.Repository.Report
{
    public class InputSheetReportPreviwer : AReportPreviwer<InputSheet>
    {
        public override Group GetGroupForReportBottons()
        {
            var group = new Group();
            group.AddCommand("Etat de fiches d'entrées", IconProvider.Reading, this.PrintInputSheet);
            return group;
        }

        public void PrintInputSheet()
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
            var logo = Path.Combine(parameterProvider.GetValue("PicturesDirectory", @"C:\SQLIMMO\Images"), parameterProvider.GetValue("LogoFileName", "logo.jpg"));
            var report = new InputSheetReport();
            report.SheetTitle.Text = "Liste des Entrées";
            report.DataSource = data;
            report.CompanyName.Text = companyName;
            report.Logo.ImageUrl = logo;
            report.CreateDocument();
            var page = CORESI.Report.Controls.ReportViewModel.GetModulePage(report.SheetTitle.Text, report);

            this.UIService.AddPage(page);

        }
    }
}
