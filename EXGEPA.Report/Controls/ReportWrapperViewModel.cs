﻿using DevExpress.XtraReports.UI;

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
