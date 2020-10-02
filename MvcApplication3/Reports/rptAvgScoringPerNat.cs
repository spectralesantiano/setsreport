using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Data;
using DevExpress.XtraReports.UI;

namespace SETSReport.Reports
{
    public partial class rptAvgScoringPerNat : DevExpress.XtraReports.UI.XtraReport
    {
        public rptAvgScoringPerNat()
        {
            InitializeComponent();
        }

        public void SetChartDataSource(string name , DataTable table )
        {
            MainChart.Series[name].DataSource = table;
        } 
    }
}
