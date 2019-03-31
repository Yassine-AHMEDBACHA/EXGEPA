namespace EXGEPA.Label.Core.Reports
{
    public partial class LabelItem5025 : DevExpress.XtraReports.UI.XtraReport
    {
        public LabelItem5025(string companyName, string logoPath = null)
        {
            InitializeComponent();
            this.companyNameLabel.Text = companyName;
            this.Logo.ImageUrl = logoPath;
        }

    }
}
