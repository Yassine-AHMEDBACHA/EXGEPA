﻿using System.IO;
using System.Linq;
using CORESI.IoC;
using CORESI.WPF;
using EXGEPA.Model;

namespace EXGEPA.Report.Tva
{
    public class TvaSheetProvider : ReportProvider<Model.Tva>
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
            var parameterProvider = ServiceLocator.Resolve<CORESI.Data.IParameterProvider>();
            var companyName = parameterProvider.GetValue<string>("CompanyName");
            var logo = Path.Combine(parameterProvider.GetValue("PicturesDirectory", @"C:\SQLIMMO\Images"), parameterProvider.GetValue("LogoFileName", "logo.jpg"));
            var report = new TvaSheet();
            report.SheetTitle.Text = "Liste des Références";
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
