using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using DevExpress.DataAccess.Sql;
using DevExpress.DataAccess.ConnectionParameters;
using DevExpress.XtraReports.UI;
using DevExpress.XtraReports.Configuration;

namespace testwebdocviewer.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";

            Reports.XtraReport1 report = new Reports.XtraReport1();
            string constr = ConfigurationManager.ConnectionStrings["dropdownconn"].ToString();
            SqlConnection _con = new SqlConnection(constr);

            SqlDataAdapter _da = new SqlDataAdapter("SELECT * FROM [SETS].[dbo].[view_FullExamineeResultsWithQuestions]  ORDER BY LastFirstMiddle ASC", _con);//where actualtestid in(" + Session["selected"] + ") ORDER BY LastFirstMiddle ASC", _con);

            DataSet ds = new DataSet();
            _da.Fill(ds);
            report.DataMember = ds.Tables[0].TableName;
            report.DataSource = ds;
           // return PartialView("_DocumentViewer1Partial", report);

            //return PartialView(new Reports.XtraReport1());
            return PartialView(report);
            //return View();
            //return PartialView();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

    }
}
