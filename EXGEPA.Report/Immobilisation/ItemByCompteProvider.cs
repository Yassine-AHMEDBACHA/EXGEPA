using CORESI.IoC;
using CORESI.WPF;
using EXGEPA.Core.Interfaces;
using EXGEPA.Model;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;

namespace EXGEPA.Report.Immobilisation
{
    [Export(typeof(IItemByCompteProvider))]
    public class ItemByCompteProvider : IItemByCompteProvider
    {
        protected IUIMessage UIMessage { get; set; }

        public ItemByCompteProvider()
        {
            this.UIMessage = ServiceLocator.GetPriorizedInstance<IUIMessage>();
        }

        public void PrintImmobilisationSheet(IEnumerable<Item> items, string title = null)
        {
            this.Print<ImmobilisationSheet>(items, title);
        }

        public void PrintImmobilisationByAccount(IEnumerable<Item> items, string title = null, string filtreCreteria = null)
        {
            this.Print<Investment.InvestmentByAccount>(items, title, filtreCreteria);
        }

        public void PrintRecapByAccount(IEnumerable<Item> items, string title = null, string filtreCreteria = null)
        {
            this.Print<Investment.RecapByInvestmentAccount>(items, title, filtreCreteria);
        }

        private void Print<TReport>(IEnumerable<Item> items, string title = null, string filtreCreteria = null) where TReport : DevExpress.XtraReports.UI.XtraReport, new()
        {
            if (items != null)
            {
                var AccountingPeriodsService = ServiceLocator.Resolve<CORESI.Data.IDataProvider<AccountingPeriod>>();
                var currentPeriod = AccountingPeriodsService.SelectAll().FirstOrDefault(x => !x.Approved);
                var parameterProvider = ServiceLocator.Resolve<CORESI.Data.IParameterProvider>();
                var companyName = parameterProvider.GetValue<string>("CompanyName");
                var header = parameterProvider.GetValue<string>("DepartmentName");

                var subHeader = parameterProvider.GetValue<string>("DirectionName");
                var logo = Path.Combine(parameterProvider.GetValue("PicturesDirectory", @"C:\SQLIMMO\Images"), parameterProvider.GetValue("LogoFileName", "logo.jpg"));
                dynamic report = new TReport();
                if (title != null)
                {
                    report.SheetTitle.Text = title;
                }
                report.DataSource = items.Where(x => x.GeneralAccount.GeneralAccountType.Type == EGeneralAccountType.Investment).ToList();
                report.CompanyName.Text = companyName;
                report.Header.Text = header;
                //report.FilterCriteria.Text =  filtreCreteria.IsValidData() ?  $"Critaire de selection : { filtreCreteria}" : null;
                report.SubHeader.Text = subHeader;
                report.Logo.ImageUrl = logo;
                report.Periode.Text = currentPeriod.Key;
                report.CreateDocument();
                var page = CORESI.Report.Controls.ReportViewModel.GetModulePage(report.SheetTitle.Text, report);
                var uIService = ServiceLocator.Resolve<IUIService>();
                uIService.AddPage(page);
            }
            else
            {
                UIMessage.Information("Acune fiche à imprimer !");
            }
        }
    }
}
