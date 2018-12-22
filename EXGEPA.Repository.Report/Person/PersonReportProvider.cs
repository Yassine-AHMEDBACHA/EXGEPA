using System.IO;
using System.Linq;
using CORESI.Data;
using CORESI.IoC;
using CORESI.WPF;
using EXGEPA.Model;
using EXGEPA.Report.Person;

namespace EXGEPA.Report
{
    public class PersonSheetProvider : ReportProvider<Model.Person>
    {
        public override void PrintSheet()
        {
            var data = this.GetDataToDisplay();
            if (data == null)
            {
                return;
            }

            var AccountingPeriodsService = ServiceLocator.Resolve<CORESI.Data.IDataProvider<AccountingPeriod>>();
                var currentPeriod = AccountingPeriodsService.SelectAll().FirstOrDefault(x => !x.Approved);
                var parameterProvider = ServiceLocator.Resolve<IParameterProvider>();
                var companyName = parameterProvider.GetValue<string>("CompanyName");
                var logo = Path.Combine(parameterProvider.GetValue("PicturesDirectory", @"C:\SQLIMMO\Images"), parameterProvider.GetValue("LogoFileName", "logo.jpg"));
            var report = new PersonSheet();
                report.SheetTitle.Text = "Liste du Personnel";
                report.DataSource = data;
                report.CompanyName.Text = companyName;
                report.Logo.ImageUrl = logo;
                report.CreateDocument();
                var page = CORESI.Report.Controls.ReportViewModel.GetModulePage(report.SheetTitle.Text, report);
                var uIService = ServiceLocator.Resolve<IUIService>();
                uIService.AddPage(page);
         
        }
        
    }
}
