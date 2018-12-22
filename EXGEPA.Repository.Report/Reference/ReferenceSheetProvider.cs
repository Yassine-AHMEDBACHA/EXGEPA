using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CORESI.Data;
using CORESI.IoC;
using CORESI.WPF.Model;
using EXGEPA.Model;

namespace EXGEPA.Report.Reference
{
    public class ReferenceSheetProvider : ReportProvider<Model.Reference>
    {
        public override IEnumerable<Model.Reference> GetDataToDisplay()
        {
            var data = base.GetDataToDisplay();
            if (data == null)
                return null;
            var listOfGeneralAccount = ServiceLocator.Resolve<IDataProvider<GeneralAccount>>().SelectAll();
            var listOfReferenceType = ServiceLocator.Resolve<IDataProvider<ReferenceType>>().SelectAll();
            Parallel.ForEach(data, x =>
            {
                if (x.ChargeAccount != null)
                {
                    x.ChargeAccount = listOfGeneralAccount.FirstOrDefault(g => g.Id == x.ChargeAccount.Id);
                }
                if (x.InvestmentAccount != null)
                {
                    x.InvestmentAccount = listOfGeneralAccount.FirstOrDefault(g => g.Id == x.InvestmentAccount.Id);
                }
               
                if (x.ReferenceType != null)
                {
                    x.ReferenceType = listOfReferenceType.FirstOrDefault(rt => rt.Id == x.ReferenceType.Id);
                }
            });

            return data;
        }

        public override void PrintSheet()
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
            var report = new ReferenceSheet();
            report.SheetTitle.Text = "Liste des Références";
            report.DataSource = data;
            report.CompanyName.Text = companyName;
            report.Logo.ImageUrl = logo;
            report.CreateDocument();
            var page = CORESI.Report.Controls.ReportViewModel.GetModulePage(report.SheetTitle.Text, report);
            this.UIService.AddPage(page);
        }

        public override List<SimpleItem> GetAdditionalReportCommand()
        {
            return new List<SimpleItem> { this.ShowRepositoryreport() };
        }

        private RibbonButton ShowRepositoryreport()
        {
            return new RibbonButton("Articles par compte")
            {
                Action = ShowItems
            };
        }

        void ShowItems()
        {
            var data = base.GetDataToDisplay();
            if (data == null)
            {
                return;
            }
            var items = ServiceLocator.Resolve<IDataProvider<Item>>().SelectAll().GroupBy(x => x.Reference.Id);
            foreach (var item in items)
            {
                var group = data.FirstOrDefault(x => x.Id == item.Key);
                    group.Items = item.ToList();
            }
            
            var parameterProvider = ServiceLocator.Resolve<IParameterProvider>();
            var report = new NamedKeyRepositoryReport();
            report.SheetTitle.Text = "Liste des articles par reference";
            report.DataSource = data;
            report.CompanyName.Text = parameterProvider.GetStringValue("CompanyName"); ;
            report.Logo.ImageUrl = Path.Combine(parameterProvider.GetValue("PicturesDirectory", @"C:\SQLIMMO\Images"), parameterProvider.GetValue("LogoFileName", "logo.jpg"));
            report.CreateDocument();
            var page = CORESI.Report.Controls.ReportViewModel.GetModulePage(report.SheetTitle.Text, report);
            UIService.AddPage(page);
        }
    }
}
