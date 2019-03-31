namespace EXGEPA.Label.Core.Reports
{
    public partial class LabelOffice6030 : DevExpress.XtraReports.UI.XtraReport
    {
        public LabelOffice6030(string companyName, string logoPath = null)
        {
            InitializeComponent();
            this.companyNameLabel.Text = companyName;
            this.Logo.ImageUrl = logoPath;
        }

    }
}
