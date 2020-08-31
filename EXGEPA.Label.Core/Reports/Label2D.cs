namespace EXGEPA.Label.Core.Reports
{
    public partial class Label2D : DevExpress.XtraReports.UI.XtraReport
    {
        public Label2D(string companyName)
        {
            InitializeComponent();
            this.companyNameLabel.Text = companyName;
        }

    }
}
