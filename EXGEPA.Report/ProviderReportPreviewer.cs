using CORESI.Data;
using CORESI.IoC;
using CORESI.WPF.Core;
using CORESI.WPF.Model;
using System.IO;

namespace EXGEPA.Report
{
    public class ProviderReportPreviewer : AReportPreviwer<Model.Provider>
    {
        public override Group GetGroupForReportBottons()
        {
            var group = new Group();
            group.AddCommand("Etat de fournisseurs", IconProvider.Reading, this.PrintProvidersSheet);
            return group;
        }

        public void PrintProvidersSheet()
        {
            var service = ServiceLocator.Resolve<IDataProvider<Model.Provider>>();
            var data = service.SelectAll();
            if (data == null)
            {
                this.UIMessage.Information("Aucun fournisseur !");
                return;
            }

            var parameterProvider = ServiceLocator.Resolve<CORESI.Data.IParameterProvider>();
            var companyName = parameterProvider.GetValue<string>("CompanyName");
            var logo = Path.Combine(parameterProvider.GetValue("PicturesDirectory", @"C:\SQLIMMO\Images"), parameterProvider.GetValue("LogoFileName", "logo.jpg"));
            dynamic report = new ProviderReport();
            report.SheetTitle.Text = "Liste des Fournisseurs";
            report.DataSource = data;
            report.CompanyName.Text = companyName;
            report.Logo.ImageUrl = logo;
            report.CreateDocument();
            var page = CORESI.Report.Controls.ReportViewModel.GetModulePage(report.SheetTitle.Text, report);
            this.UIService.AddPage(page);
        }
    }
}
