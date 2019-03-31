using CORESI.WPF;
using CORESI.WPF.Controls;
using CORESI.WPF.Model;
using EXGEPA.Model;

namespace EXGEPA.Items.Report
{
    public class ItemReportProvider : IReportPreviwer<Item>
    {
        public Group GetGroupForReportBottons()
        {
            Group group = new Group("Editions");
            group.AddCommand<RibbonButton>();
            return group;
        }

        public void SetPreviewReportGroup(IPageSetter pageSetter)
        {

        }
    }
}
