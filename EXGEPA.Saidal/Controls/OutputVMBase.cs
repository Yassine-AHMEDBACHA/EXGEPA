namespace EXGEPA.Saidal.Controls
{
    using System.Linq;
    using CORESI.Data.Tools;
    using CORESI.Tools;
    using CORESI.Tools.Collections;
    using CORESI.WPF.Controls;
    using EXGEPA.Model;

    public abstract class OutputVMBase : InterfaceVMBase<OutputCertificate>
    {
        public OutputVMBase(ExportableView exportableView, string entityName, OutputType outputType)
            : base(exportableView, entityName)
        {
            this.OutputType = outputType;
            this.AutoWidth = true;
        }

        public OutputType OutputType { get; }

        public override void InitData()
        {
            this.StartBackGroundAction(() =>
            {
                using (var scoopLogger = new ScoopLogger("Loading Data", this.Logger, true))
                {
                    scoopLogger.Snap("Loading Data ");
                    this.RepositoryDataProvider.Refresh();
                    this.RepositoryDataProvider.MapAllItems();
                    this.ListOfRows = this.RepositoryDataProvider.ListOfOutputCertificate
                    .Where(this.IsToDisplay)
                    .ToObservable();
                }
            });
        }

        protected override bool IsToDisplay(OutputCertificate instance)
        {
            if (instance.OutputType != this.OutputType)
            {
                return false;
            }

            if (!instance.Date.IsBetween(this.StartDateEditRibbon.Date, this.EndDateEditRibbon.Date))
            {
                return false;
            }

            if (bool.TryParse(instance.Caption, out bool value))
            {
                return !value;
            }

            return true;
        }
    }
}
