using System.Collections.Generic;
using System.IO;
using System.Linq;
using CORESI.Data;
using CORESI.IoC;
using CORESI.WPF;
using DevExpress.XtraReports.UI;
using EXGEPA.Core.Interfaces;
using EXGEPA.Model;

namespace EXGEPA.Report.Immobilisation
{
    public class ImmobilisationSheetProvider : IImmobilisationSheetProvider
    {
        public ImmobilisationSheetProvider()
        {
            this.UIMessage = ServiceLocator.GetPriorizedInstance<IUIMessage>();
            this.ParameterProvider = ServiceLocator.Resolve<IParameterProvider>();
            this.AccountingPeriodsService = ServiceLocator.Resolve<IDataProvider<AccountingPeriod>>();
            this.UIService = ServiceLocator.Resolve<IUIService>();
        }

        protected IUIMessage UIMessage { get; }

        protected IParameterProvider ParameterProvider { get; }

        protected IDataProvider<AccountingPeriod> AccountingPeriodsService { get; }

        protected IUIService UIService { get; }

        public void PrintImmobilisationSheet(IEnumerable<Depreciation> Depreciations, string title = null)
        {
            if (Depreciations != null)
            {
                var resport = new ImmobilisationSheet();
                resport.SheetTitle.Text = title ?? "Carte d'immobilisation";
                resport.DataSource = Depreciations;
                resport.CompanyName.Text = this.ParameterProvider.GetValue<string>("CompanyName");
                resport.Logo.ImageUrl = this.GetLogoPath();
                resport.Periode.Text = this.GetEffectiveDateText();
                DispalyReport(resport, resport.SheetTitle.Text);
            }
            else
            {
                UIMessage.Information("Aucune fiche à imprimer !");
            }
        }

        public void PrintExploitationStartupSheet(IEnumerable<Item> items, string title = null)
        {
            if (items != null)
            {
                ExploitationStartupSheet resport = new ExploitationStartupSheet();
                resport.SheetTitle.Text = "Fiche de mise en exploitation des investissements";
                resport.DataSource = items;
                resport.CompanyName.Text = this.ParameterProvider.GetValue<string>("CompanyName");
                resport.Logo.ImageUrl = this.GetLogoPath();
                resport.Periode.Text = this.GetEffectiveDateText();
                this.DispalyReport(resport, resport.SheetTitle.Text);
            }
            else
            {
                UIMessage.Information("Aucune fiche à imprimer !");
            }
        }

        private string GetLogoPath()
        {
            return Path.Combine(this.ParameterProvider.GetValue("PicturesDirectory", @"C:\SQLIMMO\Images"), this.ParameterProvider.GetValue("LogoFileName", "logo.jpg"));
        }

        public void PrintOutputSheet(IEnumerable<Item> items, bool isCession, string title = null)
        {
            if (items?.Count() > 0)
            {
                var rpt = new Outputs.CessionSheet();
                this.SetExpressions(isCession, rpt);
                rpt.SheetTitle.Text = title ?? "Fiche de sortie";
                rpt.DataSource = items;
                rpt.CompanyName.Text = this.ParameterProvider.GetValue<string>("CompanyName");
                rpt.Logo.ImageUrl = this.GetLogoPath();
                rpt.Periode.Text = this.GetEffectiveDateText();
                this.DispalyReport(rpt, rpt.SheetTitle.Text);
            }
            else
            {
                UIMessage.Information("Aucune fiche à imprimer !");
            }
        }

        private void SetExpressions(bool isCession, Outputs.CessionSheet rpt)
        {
            if (isCession)
            {
                rpt.NPV.ExpressionBindings[0].Expression = "Concat(\'N° du PV cession :\',[OutputCertificate].[Key])";
                rpt.DatePV.ExpressionBindings[0].Expression = "Concat('Date du PV :',FormatString('{0:dd/MM/yyyy}',[OutputCertificate].[Date]))";
                rpt.SenderUnit.ExpressionBindings[0].Expression = "Concat(\'Unité expéditrice :\',[Office].[Level].[Building].[Levels].[Building].[Site].[Region].[Caption])";
                rpt.RecieverUnit.ExpressionBindings[0].Expression = "Concat(\'Unité réceptrice :\',[OutputCertificate].[Tag])";
                rpt.reportLabel.Text = this.ParameterProvider.TryGet("CessionCertificateReportLabel", "IMP(PR.G.DPMG/01)");
            }
            else
            {
                rpt.NPV.ExpressionBindings[0].Expression = "Concat('N° du PV de transfert :',[TransferOrder].[Key])";
                rpt.DatePV.ExpressionBindings[0].Expression = "Concat('Date du PV :',FormatString('{0:dd/MM/yyyy}',[TransferOrder].[Date]))";
                rpt.SenderUnit.ExpressionBindings[0].Expression = "Concat(\'Unité expéditrice :\',[TransferOrder].[Sender].[Key])";
                rpt.RecieverUnit.ExpressionBindings[0].Expression = "Concat(\'Unité réceptrice :\',[Office].[Level].[Building].[Levels].[Building].[Site].[Region].[Caption])";
                rpt.reportLabel.Text = this.ParameterProvider.TryGet("TransferOrderReportLabel", "Transfert Order");
            }
        }

        private void DispalyReport(XtraReport resport, string sheetTitle)
        {
            resport.CreateDocument();
            var page = CORESI.Report.Controls.ReportViewModel.GetModulePage(sheetTitle, resport);
            this.UIService.AddPage(page);
        }

        private string GetEffectiveDateText()
        {
            var currentPeriod = AccountingPeriodsService.SelectAll().FirstOrDefault(x => !x.Approved);
            var effectiveDate = "Date Effet :" + currentPeriod.EndDate.ToString("dd/MM/yyyy");
            return effectiveDate;
        }
    }
}