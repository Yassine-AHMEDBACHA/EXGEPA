using CORESI.Data;
using CORESI.IoC;
using CORESI.WPF;
using CORESI.WPF.Core;
using CORESI.WPF.Model;

namespace EXGEPA.HotelElDjazair
{

    public class Module// : IModule
    {
        readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        IUIService UIService { get; set; }

        public Module()
        {
            logger.Debug("start composing HotelElDjazair module...");
            this.UIService = ServiceLocator.Resolve<IUIService>();
            logger.Info("Module HotelElDjazair Ready");
        }


        public int Priority
        {
            get { return 100; }
        }

        public void LoadModule()
        {
            Group hotelElDjazair = new Group("Hotel El Djazair");

            hotelElDjazair.Commands.Add(GetHomePageRibbonButton());
            UIService.AddGroupToHomePage(hotelElDjazair);
        }

        private RibbonButton GetHomePageRibbonButton()
        {
            return new RibbonButton()
            {
                Caption = "Editions",
                LargeGlyph = IconProvider.MoreFunctions,
                Action = () =>
                {
                    Reports.AquisitionReports report = new Reports.AquisitionReports();
                    IParameterProvider parameterProvider = ServiceLocator.Resolve<IParameterProvider>();
                    report.companyName.Text = parameterProvider.GetValue<string>("CompanyName");
                    report.departementName.Text = "DGM";
                    report.reportTitle.Text = "Etat des aquisitions";
                    report.CreateDocument();
                    UIService.AddPage(CORESI.Report.Controls.ReportViewModel.GetModulePage("Etat des aquisition", report));
                }
            };
        }
    }
}
