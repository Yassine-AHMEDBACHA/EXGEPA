namespace EXGEPA.Saidal.Controls
{
    using System.Linq;
    using CORESI.Data.Tools;
    using CORESI.Tools;
    using CORESI.Tools.Collections;
    using CORESI.WPF.Core.Interfaces;
    using EXGEPA.Model;
    using EXGEPA.Saidal.Core;

    public class TransferOrderVM : InterfaceVMBase<TransferOrder>
    {
        public TransferOrderVM(IExportableGrid exportableView)
            : base(exportableView, "Bons de transfert")
        {
            this.AutoWidth = true;
            this.Serializer = new TransferOrderSerializer();
        }

        public override Serializer<TransferOrder> Serializer { get; }

        public override void InitData()
        {
            this.StartBackGroundAction(() =>
            {
                using (var scoopLogger = new ScoopLogger("Loading transfert orders data ...", this.Logger, true))
                {
                    scoopLogger.Snap("Laoding...");
                    this.RepositoryDataProvider.Refresh();
                    this.RepositoryDataProvider.MapAllItems();
                    this.ListOfRows = this.RepositoryDataProvider.AllTransferOrders
                    .Where(this.IsToDisplay)
                    .ToObservable();
                }
            });
        }

        protected override bool IsToDisplay(TransferOrder instance)
        {
            if (!instance.Date.IsBetween(this.StartDateEditRibbon.Date, this.EndDateEditRibbon.Date))
            {
                return false;
            }

            if (instance.Tag is bool value)
            {
                return !value;
            }

            return true;
        }
    }
}
