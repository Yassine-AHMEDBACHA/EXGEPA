using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using CORESI.Data;
using CORESI.IoC;
using CORESI.WPF.Model;
using EXGEPA.Core.Interfaces;
using EXGEPA.Model;
using EXGEPA.Report;

namespace EXGEPA.Repository.Report
{
    public class GeneralAccountReportProvider : ReportProvider<GeneralAccount>
    {


        public override void PrintSheet()
        {
            var data = this.GetDataToDisplay();
            if (data == null)
            {
                return;
            }
            var parameterProvider = ServiceLocator.Resolve<CORESI.Data.IParameterProvider>();
            var report = new NamedKeyReport();
            report.DocumentTitle.Text = "Liste des Comptes Généraux";
            report.DataSource = data;
            report.CompanyName.Text = parameterProvider.GetStringValue("CompanyName"); ;
            report.Logo.ImageUrl = Path.Combine(parameterProvider.GetValue("PicturesDirectory", @"C:\SQLIMMO\Images"), parameterProvider.GetValue("LogoFileName", "logo.jpg"));
            report.CreateDocument();
            var page = CORESI.Report.Controls.ReportViewModel.GetModulePage(report.DocumentTitle.Text, report);
            UIService.AddPage(page);
        }

    }
}
