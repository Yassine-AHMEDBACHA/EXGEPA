using CORESI.IoC;
using CORESI.WPF;
using CORESI.WPF.Core;
using CORESI.WPF.Model;
using DevExpress.Mvvm;
using EXGEPA.Invoice.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;

namespace EXGEPA.Invoice
{

    public sealed class Module : AModule
    {
        public override int Priority
        {
            get { return 1; }
        }

        public RibbonButton GetHomePageRibbonButton()
        {
            return new RibbonButton()
            {
                Caption = "Factures",
                LargeGlyph = IconProvider.Invoice,

                Action = () =>
                {
                    var view = new InvoiceView();
                    var invoiceViewModel = new InvoiceViewModel(view);
                    var page = new Page(invoiceViewModel, view, true);
                    this.uIService.AddPage(page);
                    invoiceViewModel.InitData();
                }
            };
        }

        public override void AddGroups()
        {
            logger.Info("Start loading Invoice Module...");
            var invoiceGroup = new Group();
            invoiceGroup.Commands.Add(this.GetHomePageRibbonButton());
            logger.Info("Adding Invoice Home buttons to Ribbon");
            uIService.AddGroupToHomePage(invoiceGroup);
            logger.Info("loading Invoice Module done");
        }
    }
}
