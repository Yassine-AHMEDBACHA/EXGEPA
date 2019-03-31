using CORESI.IoC;
using CORESI.WPF;
using EXGEPA.Core.Interfaces;
using EXGEPA.Model;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace EXGEPA.Report.Inventory
{
    public class InventorySheetProvider : IInventorySheetProvider
    {
        public InventorySheetProvider()
        {
            ServiceLocator.GetDefault(out this.uIMessage);
        }

        public IUIMessage uIMessage;

        public void PrintInventorySheet(IList<Item> items, bool isTheorical = true)
        {
            if (items == null || items.Count == 0)
            {
                this.uIMessage.Information("Acune fiche à imprimer !");
                return;
            }
            items = items.OrderBy(x => x.Key).ToList();
            CORESI.Data.IDataProvider<AccountingPeriod> AccountingPeriodsService = ServiceLocator.Resolve<CORESI.Data.IDataProvider<AccountingPeriod>>();
            AccountingPeriod currentPeriod = AccountingPeriodsService.SelectAll().FirstOrDefault(x => !x.Approved);
            CORESI.Data.IParameterProvider parameterProvider = ServiceLocator.Resolve<CORESI.Data.IParameterProvider>();
            InventorySheet report = new InventorySheet();
            report.SheetTitle.Text += isTheorical ? "Théorique" : "Physique";
            report.SubHeader.Text = parameterProvider.GetValue("DirectionName", "");
            report.Header.Text = parameterProvider.GetStringValue("DepartmentName");
            report.CompanyName.Text = parameterProvider.GetStringValue("CompanyName");
            report.Logo.ImageUrl = Path.Combine(parameterProvider.GetValue("PicturesDirectory", @"C:\SQLIMMO\Images"), parameterProvider.GetValue("LogoFileName", "logo.jpg"));
            report.DataSource = items;
            report.Periode.Text = currentPeriod.Key;
            report.CreateDocument();
            CORESI.WPF.Model.Page page = CORESI.Report.Controls.ReportViewModel.GetModulePage(report.SheetTitle.Text, report);
            IUIService uIService = ServiceLocator.Resolve<IUIService>();
            uIService.AddPage(page);
        }
    }
}
