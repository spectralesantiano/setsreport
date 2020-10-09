namespace SETSReport.Reports
{
    partial class rptSubRevisionDate
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
            DevExpress.XtraReports.UI.XRSummary xrSummary1 = new DevExpress.XtraReports.UI.XRSummary();
            this.XrLabel1 = new DevExpress.XtraReports.UI.XRLabel();
            this.XrTableCell1 = new DevExpress.XtraReports.UI.XRTableCell();
            this.XrLabel2 = new DevExpress.XtraReports.UI.XRLabel();
            this.ReportHeader = new DevExpress.XtraReports.UI.ReportHeaderBand();
            this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
            this.ReportFooter = new DevExpress.XtraReports.UI.ReportFooterBand();
            this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
            this.XrTableRow1 = new DevExpress.XtraReports.UI.XRTableRow();
            this.DateCreated = new DevExpress.XtraReports.UI.XRTableCell();
            this.XrTable1 = new DevExpress.XtraReports.UI.XRTable();
            this.Detail = new DevExpress.XtraReports.UI.DetailBand();
            ((System.ComponentModel.ISupportInitialize)(this.XrTable1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // XrLabel1
            // 
            this.XrLabel1.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.XrLabel1.LocationFloat = new DevExpress.Utils.PointFloat(22F, 6.999997F);
            this.XrLabel1.Name = "XrLabel1";
            this.XrLabel1.Padding = new DevExpress.XtraPrinting.PaddingInfo(7, 2, 0, 0, 100F);
            this.XrLabel1.SizeF = new System.Drawing.SizeF(100.696F, 15F);
            this.XrLabel1.StylePriority.UseFont = false;
            this.XrLabel1.StylePriority.UsePadding = false;
            this.XrLabel1.StylePriority.UseTextAlignment = false;
            this.XrLabel1.Text = "Revision Dates";
            this.XrLabel1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomLeft;
            // 
            // XrTableCell1
            // 
            this.XrTableCell1.Font = new System.Drawing.Font("Arial", 8F);
            this.XrTableCell1.Name = "XrTableCell1";
            this.XrTableCell1.StylePriority.UseFont = false;
            this.XrTableCell1.StylePriority.UsePadding = false;
            this.XrTableCell1.StylePriority.UseTextAlignment = false;
            xrSummary1.FormatString = "{0:#,#}.";
            xrSummary1.Func = DevExpress.XtraReports.UI.SummaryFunc.RecordNumber;
            xrSummary1.Running = DevExpress.XtraReports.UI.SummaryRunning.Report;
            this.XrTableCell1.Summary = xrSummary1;
            this.XrTableCell1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.XrTableCell1.Weight = 0.020560745317891042D;
            // 
            // XrLabel2
            // 
            this.XrLabel2.Borders = DevExpress.XtraPrinting.BorderSide.Top;
            this.XrLabel2.BorderWidth = 2F;
            this.XrLabel2.LocationFloat = new DevExpress.Utils.PointFloat(0.08398113F, 9.999997F);
            this.XrLabel2.Name = "XrLabel2";
            this.XrLabel2.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.XrLabel2.SizeF = new System.Drawing.SizeF(1069.916F, 5F);
            this.XrLabel2.StylePriority.UseBorders = false;
            this.XrLabel2.StylePriority.UseBorderWidth = false;
            this.XrLabel2.StylePriority.UsePadding = false;
            // 
            // ReportHeader
            // 
            this.ReportHeader.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.XrLabel1});
            this.ReportHeader.HeightF = 22F;
            this.ReportHeader.Name = "ReportHeader";
            // 
            // TopMargin
            // 
            this.TopMargin.HeightF = 36F;
            this.TopMargin.Name = "TopMargin";
            this.TopMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.TopMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // ReportFooter
            // 
            this.ReportFooter.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.XrLabel2});
            this.ReportFooter.HeightF = 15F;
            this.ReportFooter.Name = "ReportFooter";
            // 
            // BottomMargin
            // 
            this.BottomMargin.HeightF = 55F;
            this.BottomMargin.Name = "BottomMargin";
            this.BottomMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.BottomMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // XrTableRow1
            // 
            this.XrTableRow1.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.XrTableCell1,
            this.DateCreated});
            this.XrTableRow1.Name = "XrTableRow1";
            this.XrTableRow1.Weight = 0.5842519635088097D;
            // 
            // DateCreated
            // 
            this.DateCreated.Font = new System.Drawing.Font("Arial", 8F);
            this.DateCreated.Name = "DateCreated";
            this.DateCreated.Padding = new DevExpress.XtraPrinting.PaddingInfo(7, 0, 0, 0, 100F);
            this.DateCreated.StylePriority.UseFont = false;
            this.DateCreated.StylePriority.UsePadding = false;
            this.DateCreated.Weight = 0.97943925468210891D;
            // 
            // XrTable1
            // 
            this.XrTable1.Font = new System.Drawing.Font("Arial", 9F);
            this.XrTable1.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.XrTable1.Name = "XrTable1";
            this.XrTable1.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.XrTableRow1});
            this.XrTable1.SizeF = new System.Drawing.SizeF(1070F, 12F);
            this.XrTable1.StylePriority.UseFont = false;
            this.XrTable1.StylePriority.UseTextAlignment = false;
            this.XrTable1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // Detail
            // 
            this.Detail.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.XrTable1});
            this.Detail.HeightF = 12F;
            this.Detail.Name = "Detail";
            this.Detail.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.Detail.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // rptSubRevisionDate
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.TopMargin,
            this.BottomMargin,
            this.ReportHeader,
            this.ReportFooter});
            this.Landscape = true;
            this.Margins = new System.Drawing.Printing.Margins(49, 50, 36, 55);
            this.PageHeight = 827;
            this.PageWidth = 1169;
            this.PaperKind = System.Drawing.Printing.PaperKind.A4;
            this.ScriptLanguage = DevExpress.XtraReports.ScriptLanguage.VisualBasic;
            this.StyleSheetPath = "";
            this.Version = "15.2";
            ((System.ComponentModel.ISupportInitialize)(this.XrTable1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }

        #endregion

        public DevExpress.XtraReports.UI.XRLabel XrLabel1;
        public DevExpress.XtraReports.UI.XRTableCell XrTableCell1;
        public DevExpress.XtraReports.UI.XRLabel XrLabel2;
        public DevExpress.XtraReports.UI.ReportHeaderBand ReportHeader;
        public DevExpress.XtraReports.UI.TopMarginBand TopMargin;
        public DevExpress.XtraReports.UI.ReportFooterBand ReportFooter;
        public DevExpress.XtraReports.UI.BottomMarginBand BottomMargin;
        public DevExpress.XtraReports.UI.XRTableRow XrTableRow1;
        public DevExpress.XtraReports.UI.XRTableCell DateCreated;
        public DevExpress.XtraReports.UI.XRTable XrTable1;
        public DevExpress.XtraReports.UI.DetailBand Detail;

    }
}
