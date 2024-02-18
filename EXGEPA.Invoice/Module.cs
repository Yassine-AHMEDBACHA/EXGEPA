using CORESI.WPF.Core;
using CORESI.WPF.Model;
using EXGEPA.Invoice.Controls;

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
                    InvoiceView view = new InvoiceView();
                    InvoiceViewModel invoiceViewModel = new InvoiceViewModel(view);
                    Page page = new Page(invoiceViewModel, view, true);
                    this.UIService.AddPage(page);
                    invoiceViewModel.InitData();
                }
            };
        }

        public override void AddGroups()
        {
            Logger.Info("Start loading Invoice Module...");
            var invoiceGroup = new Group();
            invoiceGroup.Commands.Add(this.GetHomePageRibbonButton());
            Logger.Info("Adding Invoice Home buttons to Ribbon");
            UIService.AddGroupToHomePage(invoiceGroup);
            Logger.Info("loading Invoice Module done");
        }
    }
}
