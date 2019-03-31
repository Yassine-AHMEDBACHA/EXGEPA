using CORESI.WPF.Controls;
using CORESI.WPF.Core.Interfaces;
using EXGEPA.Model;

namespace EXGEPA.Repository.Controls
{
    public class CurrencyViewModel : GenericEditableViewModel<Currency>
    {
        public CurrencyViewModel(IExportableGrid exportableView) : base(exportableView)
        {
            this.Caption = "Lists de devises";
        }

        public override string GetTemporaryKey()
        {
            return "DEV";
        }
    }
}