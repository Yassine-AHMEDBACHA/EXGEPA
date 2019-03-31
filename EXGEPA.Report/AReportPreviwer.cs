using System.Collections.Generic;
using CORESI.Data;
using CORESI.IoC;
using CORESI.WPF;
using CORESI.WPF.Controls;
using CORESI.WPF.Model;

namespace EXGEPA.Report
{
    public abstract class AReportPreviwer<T> : IReportPreviwer<T> where T : IRowId
    {
        protected static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        protected IUIService UIService { get; set; }

        protected IUIMessage UIMessage { get; set; }

        public AReportPreviwer()
        {
            this.UIService = ServiceLocator.Resolve<IUIService>();

            this.UIMessage = ServiceLocator.GetPriorizedInstance<IUIMessage>();
        }

        public abstract Group GetGroupForReportBottons();

        public void SetPreviewReportGroup(IPageSetter pageSetter)
        {
            Group group = this.GetGroupForReportBottons();
            pageSetter.Groups.Add(group);
        }

        public virtual IEnumerable<T> GetDataToDisplay()
        {
            IList<T> data = ServiceLocator.Resolve<IDataProvider<T>>()?.SelectAll();
            if (data == null || data.Count == 0)
            {
                this.UIMessage.Information("Aucun ligne à afficher !");
                return null;
            }

            return data;
        }
    }
}
