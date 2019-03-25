using CORESI.WPF.Controls;
using CORESI.WPF.Core.Interfaces;
using EXGEPA.Model;

namespace EXGEPA.Repository.Controls
{
    public class ProviderViewModel : GenericEditableViewModel<Provider>
    {
        public ProviderViewModel(IExportable exportableView) : base(exportableView)
        {
            this.Caption = "Liste de fournisseurs";
        }

    }
}