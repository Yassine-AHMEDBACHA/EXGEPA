using CORESI.Data;
using CORESI.DataAccess.Core;
using CORESI.IoC;
using EXGEPA.HotelElDjazair.Reports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EXGEPA.HotelElDjazair
{
    public class AquisitionReportManager
    {
        public static AquisitionReports GetReport()
        {
            var parameterProvider = ServiceLocator.Resolve<IParameterProvider>();
            var report = new Reports.AquisitionReports();
            report.companyName.Text = parameterProvider.GetValue<string>("CompanyName");
            report.departementName.Text = "DGM";
            report.reportTitle.Text = "Etat des aquisitions";



            return report;
        }

    }
}
