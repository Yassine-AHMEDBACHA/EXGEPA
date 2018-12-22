using CORESI.IoC;
using CORESI.WPF;
using CORESI.WPF.Core;
using CORESI.WPF.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
            var reportGroup = new Group("Editions");
            reportGroup.AddCommand("Investissments", IconProvider.Article, () =>
                  {
                      var reportViewModel = new Controls.ReportViewModel("Editions investissements");
                      var reportView = new Controls.ReportView();
                      var page = new Page(reportViewModel, reportView, true);
                      this.uIService.AddPage(page);
                  });
            reportGroup.AddCommand("Charges", IconProvider.Article, () =>
            {
                var editionViewModel = new Controls.EditionViewModel("Editions charges");
                var editionView = new Controls.EditionView();
                var page = new Page(editionViewModel, editionView, true);
                this.uIService.AddPage(page);
            });
            this.uIService.AddGroupToHomePage(reportGroup);
        }
    }
}
