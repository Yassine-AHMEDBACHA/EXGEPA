using System;
using System.Linq;
using CORESI.Data;
using CORESI.IoC;
using CORESI.WPF.Core;
using CORESI.WPF.Core.Interfaces;
using CORESI.WPF.Model;

namespace CORESI.WPF.Controls
{
    public class GenericExportableViewModel<T> : BasicGridViewModel<T>
    {
        internal protected IExportable exportableView;
        public int KeyLength { get; set; }
        public GenericExportableViewModel(IExportable view = null) : base()
        {
            this.exportableView = view;
            if (view != null)
            {
                this.SetExportGroup(view);
            }
           
            ServiceLocator.Resolve<IReportPreviwer<T>>()?.SetPreviewReportGroup(this);
        }

        public string DisplayedFilter => exportableView.DisplayedFilter;



        protected virtual void SetExportGroup(IExportable view, bool AllSmall = false)
        {
            var group = this.AddNewGroup("Export");
            if (AllSmall)

                AddSmallButtons(view, group);
            else
                AddButtons(view, group);
            foreach (var item in group.Commands.OfType<RibbonButton>())
            {
                item.SetAbility<T>("Export");
            }
        }

        private void AddSmallButtons(IExportable view, Group group)
        {
            group.AddCommand("Excel", IconProvider.XlsxSmall, () => view.ExportExcel(this.Caption), true);
            group.AddCommand("PDF", IconProvider.PDFSmall, () => view.ExportPDF(this.Caption), true);
            group.AddCommand("Imprimer", IconProvider.PrintSmall, () => view.Print(this.Caption), true);
        }
        private void AddButtons(IExportable view, Group group)
        {
            group.AddCommand("Excel", IconProvider.Xlsx, () => view.ExportExcel(this.Caption));
            group.AddCommand("PDF", IconProvider.PDF, () => view.ExportPDF(this.Caption));
            group.AddCommand("Imprimer", IconProvider.Print, () => view.Print(this.Caption));
        }
    }
}
