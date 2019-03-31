using CORESI.Data;
using CORESI.IoC;
using EXGEPA.HotelElDjazair.Reports;

namespace EXGEPA.HotelElDjazair
{
    public class AquisitionReportManager
    {
        public static AquisitionReports GetReport()
        {
            IParameterProvider parameterProvider = ServiceLocator.Resolve<IParameterProvider>();
            AquisitionReports report = new Reports.AquisitionReports();
            report.companyName.Text = parameterProvider.GetValue<string>("CompanyName");
            report.departementName.Text = "DGM";
            report.reportTitle.Text = "Etat des aquisitions";



            return report;
        }

    }
}
