using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;

namespace SETSReport.Reports
{
    public partial class rptTestResultSummaryForm : DevExpress.XtraReports.UI.XtraReport
    {
        public rptTestResultSummaryForm()
        {
            InitializeComponent();
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

        private void Detail_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            if (RowCount > 0)
            {
                //txtTimeTaken.Text = string.Format("hh:mm:ss", TimeSpan.FromSeconds(double.Parse(GetCurrentColumnValue("TimeTakenSec").ToString()))) + " (hr:min:sec)";
                txtTimeTaken.Text = TimeSpan.FromSeconds(double.Parse(GetCurrentColumnValue("TimeTakenSec").ToString())) + "";
                txtTimeLimit.Text = TimeSpan.FromMinutes(double.Parse(GetCurrentColumnValue("TimeLimit").ToString())) + "";
            }
        }

       
    }
}
