using CORESI.WPF.Controls;
using CORESI.WPF.Core.Interfaces;
using EXGEPA.Model;

namespace EXGEPA.Repository.Controls
{
    public class ProjectViewModel :  GenericEditableViewModel<Project>
    {
        public ProjectViewModel(IExportableGrid exportableView) : base(exportableView)
        {
            this.Caption = "Lists de projets";
        }
    }
}