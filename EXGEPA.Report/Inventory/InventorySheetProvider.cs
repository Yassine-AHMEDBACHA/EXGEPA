using CORESI.IoC;
using CORESI.WPF;
using EXGEPA.Core.Interfaces;
using EXGEPA.Model;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using CORESI.Data;

namespace EXGEPA.Report.Inventory
{
    public class InventorySheetProvider : IInventorySheetProvider
    {
        private readonly IUIMessage uIMessage;

        private readonly IParameterProvider parameterProvider;

        public InventorySheetProvider()
        {
            ServiceLocator.GetDefault(out this.uIMessage);
            ServiceLocator.Resolve(out this.parameterProvider);
        }

        public void PrintInventorySheet(IList<Item> items, bool isTheorical = true)
        {
            if (items == null || items.Count == 0)
            {
                this.uIMessage.Information("Aucune fiche à imprimer !");
                return;
            }

            items = items.OrderBy(x => x.Key).ToList();
            var AccountingPeriodsService = ServiceLocator.Resolve<CORESI.Data.IDataProvider<AccountingPeriod>>();
            var currentPeriod = AccountingPeriodsService.SelectAll().FirstOrDefault(x => !x.Approved);
            var report = new InventorySheet();
            report.SheetTitle.Text += isTheorical ? "Théorique" : "Physique";
            report.SubHeader.Text = this.parameterProvider.GetValue("DirectionName", "");
            report.Header.Text = parameterProvider.GetStringValue("DepartmentName");
            report.CompanyName.Text = parameterProvider.GetStringValue("CompanyName");
            report.oldCodeLabel.Text = this.parameterProvider.TryGet("OldCodeCaption", "IMMO");
            report.Logo.ImageUrl = Path.Combine(parameterProvider.GetValue("PicturesDirectory", @"C:\SQLIMMO\Images"), parameterProvider.GetValue("LogoFileName", "logo.jpg"));
            report.DataSource = items;
            report.Periode.Text = currentPeriod.Key;
            report.CreateDocument();
            var page = CORESI.Report.Controls.ReportViewModel.GetModulePage(report.SheetTitle.Text, report);
            IUIService uIService = ServiceLocator.Resolve<IUIService>();
            uIService.AddPage(page);
        }
    }
}
