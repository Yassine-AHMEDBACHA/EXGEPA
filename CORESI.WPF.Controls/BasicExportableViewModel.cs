using CORESI.WPF.Core;
using CORESI.WPF.Model;

namespace CORESI.WPF.Controls
{
    public abstract class BasicExportableViewModel<T> : GenericExportableViewModel<T>
    {
        public BasicExportableViewModel(ExportableView view)
            : base()
        {
            Group group = this.AddNewGroup("Outils");
            group.AddCommand("Refresh", IconProvider.Refresh, this.InitData);
            group.AddCommand("Filtre", IconProvider.Filtre, () => this.ShowColumnFilter = !ShowColumnFilter);
            group.AddCommand("Groupement", IconProvider.GroupRows, () => this.ShowGroupPanel = !ShowGroupPanel);

            if (EnableTotalSumary)
            {
                group.AddCommand<CheckedRibbonButton>("Totaux", IconProvider.Summary, () => this.ShowTotalSummary = !ShowTotalSummary).IsChecked = ShowTotalSummary;
            }
            group.AddCommand<CheckedRibbonButton>("Largeur automatique", IconProvider.FloatindWidth, () => this.AutoWidth = !AutoWidth).IsChecked = AutoWidth;
            group.AddCommand<CheckedRibbonButton>("System ID", IconProvider.FindByID, () => this.ShowSysIdColumn = !ShowSysIdColumn);
        }
        public abstract void InitData();
    }
}
