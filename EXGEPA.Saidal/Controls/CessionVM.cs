namespace EXGEPA.Saidal.Controls
{
    using CORESI.WPF.Controls;
    using EXGEPA.Model;
    using EXGEPA.Saidal.Core;

    public class CessionVM : OutputVMBase
    {
        public CessionVM(ExportableView exportableView)
            : base(exportableView, "Cessions", Model.OutputType.Cession)
        {
            this.Serializer = new CessionSerializer(this.RepositoryDataProvider);
        }

        public override Serializer<OutputCertificate> Serializer { get; }
    }
}
