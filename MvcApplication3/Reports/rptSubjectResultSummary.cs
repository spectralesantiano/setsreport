using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;

namespace SETSReport.Reports
{
    public partial class rptSubjectResultSummary : DevExpress.XtraReports.UI.XtraReport
    {
        public rptSubjectResultSummary()
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

    }
}
