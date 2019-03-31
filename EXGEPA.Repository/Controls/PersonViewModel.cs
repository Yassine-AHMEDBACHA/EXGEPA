using CORESI.WPF.Controls;
using CORESI.WPF.Core.Interfaces;
using EXGEPA.Model;

namespace EXGEPA.Repository.Controls
{
    public class PersonViewModel : GenericEditableViewModel<Person>
    {
        public PersonViewModel(IExportableGrid exportableView) : base(exportableView)
        {
            this.Caption = "Liste de Personnel";
        }
    }
}