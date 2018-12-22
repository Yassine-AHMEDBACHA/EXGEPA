using DevExpress.XtraReports.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EXGEPA.Report.Controls
{
   public class ReportWrapperViewModel
    {
        public string Title { get; set; }

        public ReportWrapperViewModel(string title)
        {
            this.Title = title;
        }

        public XtraReport DocumentSource { get; set; }
    }
}
