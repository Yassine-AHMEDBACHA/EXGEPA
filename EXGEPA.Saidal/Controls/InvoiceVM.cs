namespace EXGEPA.Saidal.Controls
{
    using System.Linq;
    using CORESI.Data.Tools;
    using CORESI.Tools;
    using CORESI.Tools.Collections;
    using CORESI.WPF.Core.Interfaces;
    using EXGEPA.Model;
    using EXGEPA.Saidal.Core;

    public class InvoiceVM : InterfaceVMBase<Invoice>
    {
        private readonly bool displayOnlyValidatedInvoice;

        public InvoiceVM(IExportableGrid exportableView)
            : base(exportableView, "Factures")
        {
            this.displayOnlyValidatedInvoice = this.ParameterProvider.TryGet("InterfaceDisplayOnlyValidatedInvoice", true);
        }

        public override Serializer<Invoice> Serializer => new InvoiceSerializer();

        public override void InitData()
        {
            this.StartBackGroundAction(() =>
            {
                using (var scoopLogger = new ScoopLogger("Loading Data", this.Logger, true))
                {
                    scoopLogger.Snap("Loading Data ");
                    this.RepositoryDataProvider.Refresh();
                    this.RepositoryDataProvider.MapAllItems();
                    this.ListOfRows = this.RepositoryDataProvider.AllInvoices
                    .Where(this.IsToDisplay)
                    .ToObservable();
                }
            });
        }

        protected override bool IsToDisplay(Invoice invoice)
        {
            if (!invoice.Date.IsBetween(this.StartDateEditRibbon.Date, this.EndDateEditRibbon.Date))
            {
                return false;
            }

            if (invoice is null)
            {
                return false;
            }

            if (this.displayOnlyValidatedInvoice && !invoice.IsValidated)
            {
                return false;
            }

            return invoice.Items.Any();
        }
    }
}