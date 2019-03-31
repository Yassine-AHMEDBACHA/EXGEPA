using CORESI.IoC;
using CORESI.WPF;
using CORESI.WPF.Core;
using CORESI.WPF.Model;

namespace EXGEPA.Report
{
    class Module //: IModule
    {
        private readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        IUIService uIService;
        IUIMessage uIMessage;

        public Module()
        {
            ServiceLocator.Resolve(out this.uIService);
            ServiceLocator.Resolve(out this.uIMessage);
        }

        public int Priority
        {
            get { return 8; }
        }

        public void LoadModule()
        {
            Group reportGroup = new Group("Editions");
            reportGroup.AddCommand("Investissments", IconProvider.Article, () =>
                  {
                      Controls.ReportViewModel reportViewModel = new Controls.ReportViewModel("Editions investissements");
                      Controls.ReportView reportView = new Controls.ReportView();
                      Page page = new Page(reportViewModel, reportView, true);
                      this.uIService.AddPage(page);
                  });
            reportGroup.AddCommand("Charges", IconProvider.Article, () =>
            {
                Controls.EditionViewModel editionViewModel = new Controls.EditionViewModel("Editions charges");
                Controls.EditionView editionView = new Controls.EditionView();
                Page page = new Page(editionViewModel, editionView, true);
                this.uIService.AddPage(page);
            });
            this.uIService.AddGroupToHomePage(reportGroup);
        }
    }
}
