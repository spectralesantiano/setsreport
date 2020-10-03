using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Drawing.Printing;
using DevExpress.Web.Mvc;
using SETSReport.Models;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using DevExpress.DataAccess.Sql;
using DevExpress.DataAccess.ConnectionParameters;
using DevExpress.XtraReports.UI;
using DevExpress.XtraReports.Configuration;
using SETSReport.Controllers;
using Newtonsoft.Json;
using SETSReport.Reports;

namespace SETSReport.Reports
{
    public partial class rptAvgScoringPerNatDummy : DevExpress.XtraReports.UI.XtraReport
    {
        public rptAvgScoringPerNatDummy()
        {
            InitializeComponent();
            this.AfterPrint += rptDummy_AfterPrint;
        }

        private void rptDummy_AfterPrint(object sender, EventArgs e)
        {

         
        }

    }
}
