using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
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

namespace SETSReport.Controllers.ReportPS
{
    public class rptAvgScoringController : Controller
    {
        //
        // GET: /rptAvgScoring/

        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /rptAvgScoring/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /rptAvgScoring/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /rptAvgScoring/Create

        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /rptAvgScoring/Edit/5

        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /rptAvgScoring/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /rptAvgScoring/Delete/5

        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /rptAvgScoring/Delete/5

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult viewFilter()
        {
            string constr = ConfigurationManager.ConnectionStrings["dbconn"].ToString();
            SqlConnection _con = new SqlConnection(constr);
            SqlDataAdapter _da = new SqlDataAdapter(Controllers.GlobalVar.CompanyNameQuery, constr);
            DataTable _dt = new DataTable();
            _da.Fill(_dt);

            ViewBag.CompanyName = Util.ToSelectList(_dt, "CompanyName", "CompanyName");

            //ViewBag.SortReportBy = new List<object>(new object[] { new { Display = "Test Name", Value = "TestNameDate" }, new { Display = "Number of times taken", Value = "NoOfTimesTaken" }, new { Display = "Lowest Score", Value = "MinPercent" }, new { Display = "Highest Score", Value = "MaxPercent" }, new { Display = "Average Score", Value = "AvgPercent" } });
            var list = new SelectList(new[] 
            {
               new { Text = "Test Name", SortReportBy = "TestNameDate" },
               new { Text = "Number of times taken", SortReportBy = "NoOfTimesTaken" },
               new { Text = "Lowest Score", SortReportBy = "MinPercent" },
               new { Text = "Highest Score", SortReportBy = "MaxPercent" },
               new { Text = "Average Score", SortReportBy = "AvgPercent" },
            }, "SortReportBy", "Text", 1);
            ViewBag.SortReportBy = list;

            return PartialView();
        }

        public ActionResult SelectionList(string criteria, string sortby)
        {
            string filterCriteria = ApplyCriteria(criteria); ;

            if (filterCriteria != "")
            {
                filterCriteria = " where " + filterCriteria;
            }

            string constr = ConfigurationManager.ConnectionStrings["dbconn"].ToString();
            SqlConnection _con = new SqlConnection(constr);

            String sql = "SELECT DISTINCT 0 IsSelected, TestNameDate, TestName, DateCreated, t.* FROM view_ExamineeResults r INNER JOIN view_TestScoreStatistics t ON t.TestID=r.TestID AND r.CompanyName=t.CompanyName";
           

             sql = "select * from (" + sql + ") tb " + filterCriteria;
             sql += " order by TestName " + sortby + ", DateCreated DESC ";

            SqlDataAdapter _da = new SqlDataAdapter(sql, constr);
            DataTable _dt = new DataTable();
            _da.Fill(_dt);

            //// ----- required : to send the selectionlist ----///////////////
            TempData["SelecionLIst"] = _dt; //get the value in the reportmain/showSelectionlist , should be datatable
            return RedirectToAction("showSelectionList", "ReportMain", new { fieldname = "TestNameDate", fieldvalue = "TestID" });

        }

        SETSReport.Reports.rptAvgScoring MainReport = new SETSReport.Reports.rptAvgScoring();

        [HttpPost]
        public ActionResult DocumentViewerPartial()
        {
            string selectedIDs = Request["txtselected"].ToString();
            string conditions = "";

            conditions = " AND t.CompanyName = '" + Request["CompanyName"] + "' " ;

            //string sql = String.Format("SELECT DISTINCT TestNameDate, TestName, DateCreated, t.* " +
            //    "FROM view_TestScoreStatistics t " +
            //    "INNER JOIN view_ExamineeResults er ON er.TestID=t.TestID AND er.CompanyName = t.CompanyName " +
            //    "WHERE t.TestID IN ({0}) {1}" +
            //    "ORDER BY {2}", selectedIDs, conditions, Request["SortReportBy"] + " " + Request["rgSortReportOrder"]);

            string sql = String.Format("SELECT DISTINCT TestNameDate, TestName, DateCreated, t.* " +
                "FROM view_TestScoreStatistics t " +
                "INNER JOIN " +
                "(select view_ExamineeResults.*,tblExaminee.SiteID from view_ExamineeResults left join tblExaminee on view_ExamineeResults.ExamineeID = tblExaminee.ExamineeID where siteid ='" + GlobalVar.SiteID + "') " +
                " er ON er.TestID=t.TestID AND er.CompanyName = t.CompanyName " +
                "WHERE t.TestID IN ({0}) {1}" +
                "ORDER BY {2}", selectedIDs, conditions, Request["SortReportBy"] + " " + Request["rgSortReportOrder"]);

            string constr = ConfigurationManager.ConnectionStrings["dbconn"].ToString();
            SqlConnection _con = new SqlConnection(constr);

            SqlDataAdapter _da = new SqlDataAdapter(sql, _con);

            DataSet ds = new DataSet();
            _da.Fill(ds);
            MainReport.DataMember = ds.Tables[0].TableName;
            MainReport.DataSource = ds;

            MainReport.txtPrintDate.Text =  DateTime.Now.ToString("dd-MMM-yyyy hh:mm tt");
            MainReport.txtCompanyName.Text = Util.GetConfig("COMPANY_NAME");
            MainReport.pbLogo.ImageUrl = Util.GetReportLogoPath();
            MainReport.txtRptTitle.Text = Util.GetConfig("APP_ABBRV") + " " + MainReport.txtRptTitle.Text;

            MainReport.lblTakenFrom.Text = String.Format(MainReport.lblTakenFrom.Text, Request["CompanyName"]);

            DataTable dt = ds.Tables[0];
            for (int i = 0; i <= dt.Columns.Count - 1; i++)
            {
                XRControl cell = MainReport.FindControl(dt.Columns[i].ColumnName, true);
                if (cell != null)
                {
                    cell.DataBindings.Add("Text", null, dt.Columns[i].ColumnName);
                }
            }

            return PartialView("_DocumentViewer1Partial", MainReport);
        }

        private String ApplyCriteria(string AllCriteria)
        {
            string searchText = "";
            string filterlist = AllCriteria.Trim(new Char[] { '\'' });  // "{\"FName\":\"ghdfd\",\"LName\":\"dfdf\",\"PositionID\":\"SYSR1E\",\"TestName\":\"2nd Officer Test for cargo ships\",\"DateTaken\":\"09/10/2020\",\"ToDate\":\"09/11/2020\",\"AnswerFilter\":\"2\",\"Nat\":\"SYSCNAX\",\"CompanyName\":\"Spectral Technologies. Inc.\"}";
            var filter = JsonConvert.DeserializeObject<dynamic>(filterlist);

            string namem = "";
            var valuen = "";
            foreach (var record in filter)
            {
                namem = record.Name;
                valuen = record.Value;

                if (valuen != null && valuen != "")
                {
                    searchText = (searchText != "") ? searchText += " AND " : "";
                    switch (namem)
                    {
                        //case "FName":
                        //    searchText += String.Format("{0} LIKE '%{1}%'", namem, valuen);
                        //    break;
                        //case "LName":
                        //    searchText += String.Format("{0} LIKE '%{1}%'", namem, valuen);
                        //    break;
                        //case "DateTaken":
                        //    searchText += String.Format("{0} >= '{1}'", namem, valuen);
                        //    break;
                        //case "ToDate":
                        //    DateTime toDate = Convert.ToDateTime(valuen);
                        //    toDate = toDate.AddDays(1);
                        //    searchText += String.Format("DateTaken <= '{0}'", toDate.ToString("dd-MMM-yyyy"));
                        //    break;
                        //case "PositionID":
                        //    searchText += String.Format("{0} = '{1}'", namem, valuen);
                        //    break;
                        case "TestName":
                            searchText += String.Format("{0} = '{1}'", namem, valuen);
                            break;
                        //case "Nat":
                        //    searchText += String.Format("{0} = '{1}'", namem, valuen);
                        //    break;
                        //case "CompanyName":
                        //    searchText += String.Format("{0} = '{1}'", namem, valuen);
                        //    break;
                        default:
                            searchText += String.Format("{0} = '{1}'", namem, valuen);
                            break;
                    }
                }
                //   End If
            }

            return searchText;
        }
    }
}
