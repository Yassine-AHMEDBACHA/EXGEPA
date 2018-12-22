namespace EXGEPA.Label.Core.Reports
{
    partial class LabelItem5025
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
            this.components = new System.ComponentModel.Container();
            DevExpress.XtraPrinting.BarCode.Code128Generator code128Generator1 = new DevExpress.XtraPrinting.BarCode.Code128Generator();
            this.Detail = new DevExpress.XtraReports.UI.DetailBand();
            this.xrPanel1 = new DevExpress.XtraReports.UI.XRPanel();
            this.PrefixCode = new DevExpress.XtraReports.UI.XRLabel();
            this.Logo = new DevExpress.XtraReports.UI.XRPictureBox();
            this.SuffixCode = new DevExpress.XtraReports.UI.XRLabel();
            this.companyNameLabel = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel3 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrBarCode1 = new DevExpress.XtraReports.UI.XRBarCode();
            this.Code = new DevExpress.XtraReports.UI.XRLabel();
            this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
            this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
            this.objectDataSource1 = new DevExpress.DataAccess.ObjectBinding.ObjectDataSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.objectDataSource1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // Detail
            // 
            this.Detail.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrPanel1});
            this.Detail.Dpi = 254F;
            this.Detail.HeightF = 249F;
            this.Detail.Name = "Detail";
            this.Detail.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 254F);
            this.Detail.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // xrPanel1
            // 
            this.xrPanel1.CanGrow = false;
            this.xrPanel1.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.PrefixCode,
            this.Logo,
            this.SuffixCode,
            this.companyNameLabel,
            this.xrLabel3,
            this.xrBarCode1,
            this.Code});
            this.xrPanel1.Dpi = 254F;
            this.xrPanel1.LocationFloat = new DevExpress.Utils.PointFloat(4.000004F, 6.000004F);
            this.xrPanel1.Name = "xrPanel1";
            this.xrPanel1.SizeF = new System.Drawing.SizeF(491F, 240F);
            // 
            // PrefixCode
            // 
            this.PrefixCode.BackColor = System.Drawing.Color.Transparent;
            this.PrefixCode.CanGrow = false;
            this.PrefixCode.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "PrefixCode")});
            this.PrefixCode.Dpi = 254F;
            this.PrefixCode.Font = new System.Drawing.Font("Times New Roman", 8F, System.Drawing.FontStyle.Bold);
            this.PrefixCode.LocationFloat = new DevExpress.Utils.PointFloat(13.00001F, 195.8154F);
            this.PrefixCode.Name = "PrefixCode";
            this.PrefixCode.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.PrefixCode.SizeF = new System.Drawing.SizeF(85.66196F, 40.18465F);
            this.PrefixCode.StylePriority.UseBackColor = false;
            this.PrefixCode.StylePriority.UseFont = false;
            this.PrefixCode.StylePriority.UseTextAlignment = false;
            this.PrefixCode.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            this.PrefixCode.WordWrap = false;
            // 
            // Logo
            // 
            this.Logo.Dpi = 254F;
            this.Logo.LocationFloat = new DevExpress.Utils.PointFloat(5F, 5F);
            this.Logo.Name = "Logo";
            this.Logo.SizeF = new System.Drawing.SizeF(100F, 100F);
            this.Logo.Sizing = DevExpress.XtraPrinting.ImageSizeMode.StretchImage;
            // 
            // SuffixCode
            // 
            this.SuffixCode.BackColor = System.Drawing.Color.Transparent;
            this.SuffixCode.CanGrow = false;
            this.SuffixCode.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "SuffixCode")});
            this.SuffixCode.Dpi = 254F;
            this.SuffixCode.Font = new System.Drawing.Font("Times New Roman", 8F, System.Drawing.FontStyle.Bold);
            this.SuffixCode.LocationFloat = new DevExpress.Utils.PointFloat(400.338F, 195.8154F);
            this.SuffixCode.Name = "SuffixCode";
            this.SuffixCode.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.SuffixCode.SizeF = new System.Drawing.SizeF(85.66196F, 40.18465F);
            this.SuffixCode.StylePriority.UseBackColor = false;
            this.SuffixCode.StylePriority.UseFont = false;
            this.SuffixCode.StylePriority.UseTextAlignment = false;
            this.SuffixCode.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            this.SuffixCode.WordWrap = false;
            // 
            // companyNameLabel
            // 
            this.companyNameLabel.CanGrow = false;
            this.companyNameLabel.Dpi = 254F;
            this.companyNameLabel.Font = new System.Drawing.Font("Times New Roman", 12F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.companyNameLabel.LocationFloat = new DevExpress.Utils.PointFloat(112.7112F, 4.658444F);
            this.companyNameLabel.Name = "companyNameLabel";
            this.companyNameLabel.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.companyNameLabel.SizeF = new System.Drawing.SizeF(376.2888F, 56.8638F);
            this.companyNameLabel.StylePriority.UseFont = false;
            this.companyNameLabel.StylePriority.UseTextAlignment = false;
            this.companyNameLabel.Text = "SARL CORESI";
            this.companyNameLabel.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // xrLabel3
            // 
            this.xrLabel3.CanGrow = false;
            this.xrLabel3.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Caption")});
            this.xrLabel3.Dpi = 254F;
            this.xrLabel3.Font = new System.Drawing.Font("Times New Roman", 8F, System.Drawing.FontStyle.Bold);
            this.xrLabel3.LocationFloat = new DevExpress.Utils.PointFloat(112.7112F, 66.52224F);
            this.xrLabel3.Name = "xrLabel3";
            this.xrLabel3.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrLabel3.SizeF = new System.Drawing.SizeF(376.2888F, 42.13621F);
            this.xrLabel3.StylePriority.UseFont = false;
            this.xrLabel3.StylePriority.UseTextAlignment = false;
            this.xrLabel3.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            // 
            // xrBarCode1
            // 
            this.xrBarCode1.AutoModule = true;
            this.xrBarCode1.BarCodeOrientation = DevExpress.XtraPrinting.BarCode.BarCodeOrientation.UpsideDown;
            this.xrBarCode1.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "CodeBare")});
            this.xrBarCode1.Dpi = 254F;
            this.xrBarCode1.LocationFloat = new DevExpress.Utils.PointFloat(25.00001F, 110.6584F);
            this.xrBarCode1.Module = 3F;
            this.xrBarCode1.Name = "xrBarCode1";
            this.xrBarCode1.Padding = new DevExpress.XtraPrinting.PaddingInfo(25, 25, 0, 0, 254F);
            this.xrBarCode1.ShowText = false;
            this.xrBarCode1.SizeF = new System.Drawing.SizeF(446F, 85.15695F);
            this.xrBarCode1.StylePriority.UseTextAlignment = false;
            this.xrBarCode1.Symbology = code128Generator1;
            this.xrBarCode1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomCenter;
            // 
            // Code
            // 
            this.Code.CanGrow = false;
            this.Code.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Code")});
            this.Code.Dpi = 254F;
            this.Code.Font = new System.Drawing.Font("Times New Roman", 8F, System.Drawing.FontStyle.Bold);
            this.Code.LocationFloat = new DevExpress.Utils.PointFloat(108.6866F, 195.8154F);
            this.Code.Name = "Code";
            this.Code.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.Code.SizeF = new System.Drawing.SizeF(281.6268F, 40.18462F);
            this.Code.StylePriority.UseFont = false;
            this.Code.StylePriority.UseTextAlignment = false;
            this.Code.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            // 
            // TopMargin
            // 
            this.TopMargin.Dpi = 254F;
            this.TopMargin.HeightF = 0F;
            this.TopMargin.Name = "TopMargin";
            this.TopMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 254F);
            this.TopMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // BottomMargin
            // 
            this.BottomMargin.Dpi = 254F;
            this.BottomMargin.HeightF = 0F;
            this.BottomMargin.Name = "BottomMargin";
            this.BottomMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 254F);
            this.BottomMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // objectDataSource1
            // 
            this.objectDataSource1.DataSource = typeof(EXGEPA.Label.Core.Model.ItemLabel);
            this.objectDataSource1.Name = "objectDataSource1";
            // 
            // LabelItem5025
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.TopMargin,
            this.BottomMargin});
            this.ComponentStorage.AddRange(new System.ComponentModel.IComponent[] {
            this.objectDataSource1});
            this.DataSource = this.objectDataSource1;
            this.Dpi = 254F;
            this.Margins = new System.Drawing.Printing.Margins(0, 0, 0, 0);
            this.PageHeight = 250;
            this.PageWidth = 500;
            this.PaperKind = System.Drawing.Printing.PaperKind.Custom;
            this.PaperName = "Label5025";
            this.ReportUnit = DevExpress.XtraReports.UI.ReportUnit.TenthsOfAMillimeter;
            this.SnapGridSize = 25F;
            this.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleJustify;
            this.Version = "16.1";
            ((System.ComponentModel.ISupportInitialize)(this.objectDataSource1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }

        #endregion

        private DevExpress.XtraReports.UI.DetailBand Detail;
        private DevExpress.XtraReports.UI.TopMarginBand TopMargin;
        private DevExpress.XtraReports.UI.BottomMarginBand BottomMargin;
        private DevExpress.XtraReports.UI.XRPanel xrPanel1;
        private DevExpress.XtraReports.UI.XRLabel companyNameLabel;
        private DevExpress.XtraReports.UI.XRLabel xrLabel3;
        private DevExpress.XtraReports.UI.XRBarCode xrBarCode1;
        private DevExpress.XtraReports.UI.XRLabel Code;
        private DevExpress.DataAccess.ObjectBinding.ObjectDataSource objectDataSource1;
        private DevExpress.XtraReports.UI.XRLabel SuffixCode;
        public DevExpress.XtraReports.UI.XRPictureBox Logo;
        private DevExpress.XtraReports.UI.XRLabel PrefixCode;
    }
}
