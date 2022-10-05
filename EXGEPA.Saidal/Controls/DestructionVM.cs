namespace EXGEPA.Saidal.Controls
{
    using CORESI.WPF.Controls;
    using EXGEPA.Model;
    using EXGEPA.Saidal.Core;

    public class DestructionVM : OutputVMBase
    {
        public DestructionVM(ExportableView exportableView)
            : base(exportableView, "Destructions", OutputType.Destruction)
        {
            this.Serializer = new DestructionSerializer(this.RepositoryDataProvider);
        }

        public override Serializer<OutputCertificate> Serializer { get; }
    }
}