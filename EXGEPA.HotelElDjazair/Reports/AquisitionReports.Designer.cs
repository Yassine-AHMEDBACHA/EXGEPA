namespace EXGEPA.HotelElDjazair.Reports
{
    partial class AquisitionReports
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.Detail = new DevExpress.XtraReports.UI.DetailBand();
            this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
            this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
            this.HeaderPanel = new DevExpress.XtraReports.UI.XRPanel();
            this.reportTitle = new DevExpress.XtraReports.UI.XRLabel();
            this.departementName = new DevExpress.XtraReports.UI.XRLabel();
            this.companyName = new DevExpress.XtraReports.UI.XRLabel();
            this.reportDate = new DevExpress.XtraReports.UI.XRLabel();
            this.xrPictureBox1 = new DevExpress.XtraReports.UI.XRPictureBox();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // Detail
            // 
            this.Detail.HeightF = 100F;
            this.Detail.Name = "Detail";
            this.Detail.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.Detail.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // TopMargin
            // 
            this.TopMargin.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.HeaderPanel});
            this.TopMargin.HeightF = 256F;
            this.TopMargin.Name = "TopMargin";
            this.TopMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.TopMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // BottomMargin
            // 
            this.BottomMargin.HeightF = 100F;
            this.BottomMargin.Name = "BottomMargin";
            this.BottomMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.BottomMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // HeaderPanel
            // 
            this.HeaderPanel.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.reportTitle,
            this.departementName,
            this.companyName,
            this.reportDate,
            this.xrPictureBox1});
            this.HeaderPanel.LocationFloat = new DevExpress.Utils.PointFloat(9.999974F, 25F);
            this.HeaderPanel.Name = "HeaderPanel";
            this.HeaderPanel.SizeF = new System.Drawing.SizeF(800F, 163.8333F);
            // 
            // reportTitle
            // 
            this.reportTitle.LocationFloat = new DevExpress.Utils.PointFloat(188.5417F, 115.625F);
            this.reportTitle.Name = "reportTitle";
            this.reportTitle.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.reportTitle.SizeF = new System.Drawing.SizeF(496.8751F, 34.375F);
            this.reportTitle.StylePriority.UseTextAlignment = false;
            this.reportTitle.Text = "reportTitle";
            this.reportTitle.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            // 
            // departementName
            // 
            this.departementName.LocationFloat = new DevExpress.Utils.PointFloat(148.9583F, 48.95833F);
            this.departementName.Name = "departementName";
            this.departementName.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.departementName.SizeF = new System.Drawing.SizeF(364.5833F, 26.04166F);
            this.departementName.Text = "departementName";
            // 
            // companyName
            // 
            this.companyName.LocationFloat = new DevExpress.Utils.PointFloat(148.9583F, 12.5F);
            this.companyName.Name = "companyName";
            this.companyName.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.companyName.SizeF = new System.Drawing.SizeF(364.5833F, 36.45833F);
            this.companyName.Text = "companyName";
            // 
            // reportDate
            // 
            this.reportDate.CanGrow = false;
            this.reportDate.LocationFloat = new DevExpress.Utils.PointFloat(660.8334F, 10.00001F);
            this.reportDate.Name = "reportDate";
            this.reportDate.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.reportDate.SizeF = new System.Drawing.SizeF(129.1667F, 23F);
            this.reportDate.StylePriority.UseTextAlignment = false;
            this.reportDate.Text = "01/01/2016";
            this.reportDate.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.reportDate.WordWrap = false;
            // 
            // xrPictureBox1
            // 
            this.xrPictureBox1.LocationFloat = new DevExpress.Utils.PointFloat(12.5F, 12.5F);
            this.xrPictureBox1.Name = "xrPictureBox1";
            this.xrPictureBox1.SizeF = new System.Drawing.SizeF(125F, 122.9167F);
            // 
            // AquisitionReports
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.TopMargin,
            this.BottomMargin});
            this.Margins = new System.Drawing.Printing.Margins(10, 14, 256, 100);
            this.Version = "15.2";
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }

        #endregion

        private DevExpress.XtraReports.UI.DetailBand Detail;
        private DevExpress.XtraReports.UI.TopMarginBand TopMargin;
        private DevExpress.XtraReports.UI.BottomMarginBand BottomMargin;
        private DevExpress.XtraReports.UI.XRPanel HeaderPanel;
        public DevExpress.XtraReports.UI.XRLabel reportTitle;
        public DevExpress.XtraReports.UI.XRLabel departementName;
        public DevExpress.XtraReports.UI.XRLabel companyName;
        public DevExpress.XtraReports.UI.XRLabel reportDate;
        public DevExpress.XtraReports.UI.XRPictureBox xrPictureBox1;
    }
}
