namespace EXGEPA.Saidal.Controls
{
    using CORESI.WPF.Controls;
    using EXGEPA.Model;
    using EXGEPA.Saidal.Core;

    public class SaleVM : OutputVMBase
    {
        public SaleVM(ExportableView exportableView)
            : base(exportableView, "Vente", Model.OutputType.Vente)
        {
            this.Serializer = new SaleSerializer(this.RepositoryDataProvider);
        }

        public override Serializer<OutputCertificate> Serializer { get; }
    }
}
