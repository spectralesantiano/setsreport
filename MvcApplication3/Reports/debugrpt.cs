using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;

namespace SETSReport.Reports
{
    public partial class debugrpt : DevExpress.XtraReports.UI.XtraReport
    {
        public debugrpt()
        {
            InitializeComponent();
        }

        private void Detail_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            xrPictureBox1.ImageUrl = "https://sets-online.azurewebsites.net/media-files/positionerfinal.jpg";
        }

    }
}
