using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;

namespace MvcApplication3.Reports
{
    public partial class rptIndiTestResult : DevExpress.XtraReports.UI.XtraReport
    {
        public rptIndiTestResult()
        {
            InitializeComponent();
        }

        private void ActualTestHeader_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
           if(RowCount > 0) {
               //txtTimeTaken.Text = string.Format("hh:mm:ss", TimeSpan.FromSeconds(double.Parse(GetCurrentColumnValue("TimeTakenSec").ToString()))) + " (hr:min:sec)";
               txtTimeTaken.Text = TimeSpan.FromSeconds(double.Parse(GetCurrentColumnValue("TimeTakenSec").ToString())) + " (hr:min:sec)";
           }
        }

        private void Detail_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
             if (RowCount > 0 ){
                 txtMark.Text = (GetCurrentColumnValue("IsCorrect").ToString() == "1")? "✔":"✘";
                 txtMark.ForeColor = (GetCurrentColumnValue("IsCorrect").ToString() == "1") ? Color.Green: Color.Red;

                 IsContested.WidthF = (Convert.ToBoolean(GetCurrentColumnValue("IsContested")))? ContestedIcon.WidthF: 0;
             }
        }

        private void DateTaken_EvaluateBinding(object sender, BindingEventArgs e)
        {
             try{ 
                 if (e.Value != null) {
                     e.Value = Convert.ToDateTime(e.Value).ToString("dd-MMM-yyyy hh:mm tt");
                 }
             }
               catch (Exception ex){}
        }

    }
}
