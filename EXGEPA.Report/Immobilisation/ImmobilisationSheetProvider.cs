﻿using System.Collections.Generic;
using System.IO;
using System.Linq;
using CORESI.IoC;
using CORESI.WPF;
using EXGEPA.Core.Interfaces;
using EXGEPA.Model;

namespace EXGEPA.Report.Immobilisation
{
    public class ImmobilisationSheetProvider : IImmobilisationSheetProvider
    {
        protected IUIMessage UIMessage { get; set; }
        public ImmobilisationSheetProvider()
        {
            this.UIMessage = ServiceLocator.GetPriorizedInstance<IUIMessage>();
        }

        public void PrintImmobilisationSheet(IEnumerable<Depreciation> Depreciations, string title = "")
        {
            if (Depreciations != null)
            {
                var AccountingPeriodsService = ServiceLocator.Resolve<CORESI.Data.IDataProvider<AccountingPeriod>>();
                var currentPeriod = AccountingPeriodsService.SelectAll().FirstOrDefault(x => !x.Approved);
                var parameterProvider = ServiceLocator.Resolve<CORESI.Data.IParameterProvider>();
                var companyName = parameterProvider.GetValue<string>("CompanyName");
                var logo = Path.Combine(parameterProvider.GetValue("PicturesDirectory", @"C:\SQLIMMO\Images"), parameterProvider.GetValue("LogoFileName", "logo.jpg"));
                var resport = new ImmobilisationSheet();
                resport.SheetTitle.Text = "Carte d'immobilisation";
                resport.DataSource = Depreciations;
                resport.CompanyName.Text = companyName;
                resport.Logo.ImageUrl = logo;
                resport.Periode.Text = "Date Effet :" + currentPeriod.EndDate.ToString("dd/MM/yyyy");
                //.Code;
                resport.CreateDocument();
                var page = CORESI.Report.Controls.ReportViewModel.GetModulePage(resport.SheetTitle.Text, resport);
                var uIService = ServiceLocator.Resolve<IUIService>();
                uIService.AddPage(page);
            }
            else
            {
                UIMessage.Information("Aucune fiche à imprimer !");
            }
        }

        public void PrintExploitationStartupSheet(IEnumerable<Item> items, string title = "")
        {
            if (items != null)
            {
                var AccountingPeriodsService = ServiceLocator.Resolve<CORESI.Data.IDataProvider<AccountingPeriod>>();
                var currentPeriod = AccountingPeriodsService.SelectAll().FirstOrDefault(x => !x.Approved);
                var parameterProvider = ServiceLocator.Resolve<CORESI.Data.IParameterProvider>();
                var companyName = parameterProvider.GetValue<string>("CompanyName");
                var logo = Path.Combine(parameterProvider.GetValue("PicturesDirectory", @"C:\SQLIMMO\Images"), parameterProvider.GetValue("LogoFileName", "logo.jpg"));
                var resport = new ExploitationStartupSheet();
                resport.SheetTitle.Text = "Fiche de mise en exploitation des investissements";
                resport.DataSource = items;
                resport.CompanyName.Text = companyName;
                resport.Logo.ImageUrl = logo;
                resport.Periode.Text = "Date Effet :" + currentPeriod.EndDate.ToString("dd/MM/yyyy");
                //.Code;
                resport.CreateDocument();
                var page = CORESI.Report.Controls.ReportViewModel.GetModulePage(resport.SheetTitle.Text, resport);
                var uIService = ServiceLocator.Resolve<IUIService>();
                uIService.AddPage(page);
            }
            else
            {
                UIMessage.Information("Aucune fiche à imprimer !");
            }
        }

        public void PrintOutputSheet(IEnumerable<Item> items, bool isCession, string title = null)
        {
            if (items?.Count() > 0)
            {
                var AccountingPeriodsService = ServiceLocator.Resolve<CORESI.Data.IDataProvider<AccountingPeriod>>();
                var currentPeriod = AccountingPeriodsService.SelectAll().FirstOrDefault(x => !x.Approved);
                var parameterProvider = ServiceLocator.Resolve<CORESI.Data.IParameterProvider>();
                var companyName = parameterProvider.GetValue<string>("CompanyName");
                var logo = Path.Combine(parameterProvider.GetValue("PicturesDirectory", @"C:\SQLIMMO\Images"), parameterProvider.GetValue("LogoFileName", "logo.jpg"));
                var rpt = new Outputs.OutputSheet();
                NewMethod(isCession, rpt);
                rpt.SheetTitle.Text = title ?? "Fiche de sortie";
                rpt.DataSource = items;
                rpt.CompanyName.Text = companyName;
                rpt.Logo.ImageUrl = logo;
                rpt.Periode.Text = "Date Effet :" + currentPeriod.EndDate.ToString("dd/MM/yyyy");
                //.Code;
                rpt.CreateDocument();
                var page = CORESI.Report.Controls.ReportViewModel.GetModulePage(rpt.SheetTitle.Text, rpt);
                var uIService = ServiceLocator.Resolve<IUIService>();
                uIService.AddPage(page);
            }
            else
            {
                UIMessage.Information("Aucune fiche à imprimer !");
            }
        }

        private static void NewMethod(bool isCession, Outputs.OutputSheet rpt)
        {
            if (isCession)
            {
                rpt.NPV.ExpressionBindings[0].Expression = "Concat(\'N° du PV cession :\',[OutputCertificate].[Key])";
                rpt.DatePV.ExpressionBindings[0].Expression = "Concat('Date du PV :',FormatString('{0:dd/MM/yyyy}',[OutputCertificate].[Date]))";
                rpt.SenderUnit.ExpressionBindings[0].Expression = "Concat(\'Unité expéditrice :\',[Office].[Level].[Building].[Levels].[Building].[Site].[Region].[Caption])";
                rpt.RecieverUnit.ExpressionBindings[0].Expression = "Concat(\'Unité réceptrice :\',[OutputCertificate].[Tag])";
            }
            else
            {
                rpt.NPV.ExpressionBindings[0].Expression = "Concat('N° du PV de transfert :',[TransferOrder].[Key])";
                rpt.DatePV.ExpressionBindings[0].Expression = "Concat('Date du PV :',FormatString('{0:dd/MM/yyyy}',[TransferOrder].[Date]))";
                rpt.SenderUnit.ExpressionBindings[0].Expression = "Concat(\'Unité expéditrice :\',[TransferOrder].[Sender].[Key])";
                rpt.RecieverUnit.ExpressionBindings[0].Expression = "Concat(\'Unité réceptrice :\',[Office].[Level].[Building].[Levels].[Building].[Site].[Region].[Caption])";
            }
        }
    }
}