using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using DevExpress.XtraPrinting;
using SETSReport.Controllers;

namespace SETSReport.Reports
{
    public partial class rptIndiTestResult_wAnsOptionsAndPics : DevExpress.XtraReports.UI.XtraReport
    {
        private string DefaultMediaPath = "";

        public rptIndiTestResult_wAnsOptionsAndPics()
        {
            InitializeComponent();
            //DefaultMediaPath = Util.GetConfig("MEDIA_PATH");
            DefaultMediaPath = Util.GetMEDIA_PATH_ONLINE();
        }

        private void ClearText(params XRTableCell[] cells)
        {
            foreach (var cell in cells)
                cell.Text = "";
        }


        private void ClearBorders(params XRTableCell[] cells)
        {
            foreach (var cell in cells)
                cell.Borders = BorderSide.None;
        }

        private void ShowAllBoarders(XRTableCell cell)
        {
            cell.Borders = (BorderSide)(((BorderSide.Left | BorderSide.Top) | BorderSide.Right) | BorderSide.Bottom);
        }


        // ' 
        // ' GENERATED METHODS
        // '
        private void ActualTestHeader_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            if (RowCount > 0)
                txtTimeTaken.Text = TimeSpan.FromSeconds(System.Convert.ToDouble(GetCurrentColumnValue("TimeTakenSec"))).ToString(@"hh\:mm\:ss") + " (hr:min:sec)";
        }

        private void Detail_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            if (RowCount > 0)
            {
                txtMark.Text = (GetCurrentColumnValue("IsCorrect").ToString() == "1") ? "✔" : "✘";
                txtMark.ForeColor = (GetCurrentColumnValue("IsCorrect").ToString() == "1") ? Color.Green : Color.Red;

                ClearText(lblOpt1, lblOpt2, lblOpt3, lblOpt4);

                switch (GetCurrentColumnValue("Answer").ToString())
                {
                    case "1":
                        lblOpt1.Text = ">";
                        break;
                    case "2":
                        lblOpt2.Text = ">";
                        break;
                    case "3":
                        lblOpt3.Text = ">";
                        break;
                    case "4":
                        lblOpt4.Text = ">";
                        break;
                }

                ClearBorders(lblOption1, lblOption2, lblOption3, lblOption4);

                switch (GetCurrentColumnValue("UserAns").ToString())
                {
                    case "1": ShowAllBoarders(lblOption1); break;
                    case "2": ShowAllBoarders(lblOption2); break;
                    case "3": ShowAllBoarders(lblOption3); break;
                    case "4": ShowAllBoarders(lblOption4); break;
                }

                 pbQuestionImg.ImageUrl = "";
                 pbQuestionImg.Visible = false;
                 if (Convert.ToInt32(GetCurrentColumnValue("SupportDocType")) == (int)SETSReportFunction.MediaType.Picture ||
                        Convert.ToInt32(GetCurrentColumnValue("SupportDocType")) == (int)SETSReportFunction.MediaType.PictureAudio ) {

                    var doc = GetCurrentColumnValue("SupportDoc").ToString().Split(';', '|')[0];

                    //pbQuestionImg.ImageUrl = IO.Path.Combine(DefaultMediaPath, doc);
                    pbQuestionImg.ImageUrl = DefaultMediaPath + doc;
                    pbQuestionImg.Visible = true;
                 }

                IsContested.WidthF = Convert.ToBoolean(GetCurrentColumnValue("IsContested")) ? ContestedIcon.WidthF : 0;
            }
        }

        private void DateTaken_EvaluateBinding(object sender, BindingEventArgs e)
        {
            try
            {
                if (e.Value != null)
                {
                    e.Value = Convert.ToDateTime(e.Value).ToString("dd-MMM-yyyy hh:mm tt");
                }
            }
            catch (Exception ex) { }
        }

    }
}
