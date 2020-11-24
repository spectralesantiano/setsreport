using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
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

namespace SETSReport.Controllers.ReportPS
{
    public class rptAvgScoringPerNatController : Controller
    {
        //
        // GET: /rptAvgScoringPerNat/
        string maySiteID = "";

        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /rptAvgScoringPerNat/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /rptAvgScoringPerNat/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /rptAvgScoringPerNat/Create

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
        // GET: /rptAvgScoringPerNat/Edit/5

        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /rptAvgScoringPerNat/Edit/5

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
        // GET: /rptAvgScoringPerNat/Delete/5

        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /rptAvgScoringPerNat/Delete/5

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
            SqlDataAdapter _da = new SqlDataAdapter(Controllers.GlobalVar.SiteNameQuery, constr);
            DataTable _dt = new DataTable();
            _da.Fill(_dt);

            ViewBag.CompanyName = Util.ToSelectList(_dt, "SiteID", "SiteName");

            _da = new SqlDataAdapter(Controllers.GlobalVar.NationalityQuery, constr);
            _dt.Clear();
            _dt.Columns.Clear();
            _da.Fill(_dt);
            ViewBag.Nationality = Util.ToSelectList(_dt, "PKey", "Nat");

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

            if (maySiteID != "")
            {
                maySiteID = " where SiteID = '" + maySiteID + "'";
            }
            else
            {
                maySiteID = (GlobalVar.SiteID == "" ? "" : " where " + GlobalVar.SiteID);
            }

            //String sql = "SELECT DISTINCT 0 IsSelected, TestNameDate, TestName, DateCreated, t.* FROM view_ExamineeResults r INNER JOIN view_TestScoreStatistics t ON t.TestID=r.TestID AND r.CompanyName=t.CompanyName";
            String sql = "SELECT DISTINCT 0 IsSelected, TestNameDate, TestName, DateCreated, t.* FROM " +
                      "(select view_ExamineeResults.*,tblExaminee.SiteID from view_ExamineeResults left join tblExaminee on view_ExamineeResults.ExamineeID = tblExaminee.ExamineeID   " + maySiteID + " ) r " +
                      "INNER JOIN view_TestScoreStatistics t ON t.TestID=r.TestID AND r.CompanyName=t.CompanyName";


            sql = "select * from (" + sql + ") tb " + filterCriteria;
            sql += " order by TestName " + sortby + ", DateCreated DESC ";

            SqlDataAdapter _da = new SqlDataAdapter(sql, constr);
            DataTable _dt = new DataTable();
            _da.Fill(_dt);

            //// ----- required : to send the selectionlist ----///////////////
            TempData["SelecionLIst"] = _dt; //get the value in the reportmain/showSelectionlist , should be datatable
            return RedirectToAction("showSelectionList", "ReportMain", new { fieldname = "TestNameDate", fieldvalue = "TestID" });

        }

        rptAvgScoringPerNatDummy MainReport = new rptAvgScoringPerNatDummy();
        string selectedIDs;
        string rgSortReportOrder;
        string luenat;
        string CompanyName;

        [HttpPost]
        public ActionResult DocumentViewerPartial()
        {
             selectedIDs = Request["txtselected"].ToString();

            //XtraReport MainReport = new XtraReport();
            MainReport.DisplayName = "Average Scoring per Nationality";
            MainReport.Bookmark = "Average Scoring per Nationality";
            MainReport.Landscape = true;
            MainReport.Margins = new Margins(50, 50, 50, 50);
            MainReport.PageHeight = 827;
            MainReport.PageWidth = 1169;
            MainReport.PaperKind = PaperKind.A4;
            MainReport.CreateDocument();
            MainReport.PrintingSystem.ContinuousPageNumbering = true;

            rgSortReportOrder = Request["rgSortReportOrder"];
            luenat = Request["lueNat"];
            //CompanyName = Request["CompanyName"];

            MainReport.AfterPrint += rptDummy_afterPrint_method; //addhandler

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
                        case "Nat":
                            searchText += String.Format("{0} = '{1}'", namem, valuen);
                            break;
                        case "SiteName":
                            //searchText += String.Format("{0} = '{1}'", namem, valuen);
                            maySiteID = valuen;
                            break;
                        default:
                            searchText += String.Format("{0} = '{1}'", namem, valuen);
                            break;
                    }
                }
                //   End If
            }

            return searchText;
        }

        public void rptDummy_afterPrint_method(object sender, EventArgs e)
        {
            string comName = Util.GetConfig("COMPANY_NAME");
            string logoPath = Util.GetReportLogoPath();
            string dateNow = DateTime.Now.ToString("dd-MMM-yyyy hh:mm tt");

            string constr = ConfigurationManager.ConnectionStrings["dbconn"].ToString();
            SqlConnection _con = new SqlConnection(constr);

            if (maySiteID != "")
            {
                maySiteID = " where SiteID = '" + maySiteID + "'";
            }
            else
            {
                maySiteID = (GlobalVar.SiteID == "" ? "" : " where " + GlobalVar.SiteID);
            }

            //String selectedsql = "SELECT DISTINCT 0 IsSelected, TestNameDate, TestName, DateCreated, t.* FROM view_ExamineeResults r INNER JOIN view_TestScoreStatistics t ON t.TestID=r.TestID AND r.CompanyName=t.CompanyName";
            String selectedsql = "SELECT DISTINCT 0 IsSelected, TestNameDate, TestName, DateCreated, t.* FROM " +
                       "(select view_ExamineeResults.*,tblExaminee.SiteID from view_ExamineeResults left join tblExaminee on view_ExamineeResults.ExamineeID = tblExaminee.ExamineeID   " + maySiteID + " ) r " +
                       "INNER JOIN view_TestScoreStatistics t ON t.TestID=r.TestID AND r.CompanyName=t.CompanyName";

            selectedsql = "select * from (" + selectedsql + ") tb where TestID in (" + selectedIDs + ")";
            selectedsql += " order by TestName " + rgSortReportOrder + ", DateCreated DESC ";

            SqlDataAdapter _da = new SqlDataAdapter(selectedsql, constr);
            DataTable _dt = new DataTable();
            _da.Fill(_dt);

            DataTable dt = _dt;

            string selectedNat = luenat != null ? " WHERE a.Pkey IN ('" + luenat.ToString().Replace(",", "','") + "')" : "";

            foreach (DataRow row in dt.Rows)
            {
                rptAvgScoringPerNat tempReport = new rptAvgScoringPerNat();
                string testID = row["TestID"].ToString();

                foreach (var item in new Dictionary<string, string>() { { "Lowest", "MIN(MinScore) / MIN(MinTotal)" },
                                                                        { "Average", "AVG(AvgScore) / AVG(AvgTotal)" },
                                                                        { "Highest", "MAX(MaxScore) / MAX(MaxTotal)" } })
                {
                    //string sql = String.Format("SELECT CASE WHEN NoOfTimesTaken IS NULL THEN a.Nat ELSE CONCAT(a.Nat, ' (', NoOfTimesTaken, ')') END Argument, " +
                    //       "ROUND(({0}) * 100, 2) Value " +
                    //       "FROM view_AllTestNationality a " +
                    //       "LEFT JOIN view_TestScoreStatisticsPerNat b ON b.NatID = a.PKey AND TestID = '{1}' AND CompanyName = '{2}' {3}" +
                    //       "GROUP BY a.Nat, NoOfTimesTaken", item.Value, testID, CompanyName, selectedNat);

                    string sql = String.Format("SELECT CASE WHEN NoOfTimesTaken IS NULL THEN a.Nat ELSE CONCAT(a.Nat, ' (', NoOfTimesTaken, ')') END Argument, " +
                          "ROUND(({0}) * 100, 2) Value " +
                          "FROM (select * from view_AllTestNationality   " + maySiteID + " ) a " +
                          "LEFT JOIN view_TestScoreStatisticsPerNat b ON b.NatID = a.PKey AND TestID = '{1}' {2}" +
                          "GROUP BY a.Nat, NoOfTimesTaken", item.Value, testID, selectedNat);

                    _da = new SqlDataAdapter(sql, _con);
                    DataSet ds = new DataSet();
                    _da.Fill(ds);
          
                    //tempReport.SetChartDataSource(item.Key,ds.Tables[0]);
                    tempReport.MainChart.DataMember = ds.Tables[0].TableName;
                    tempReport.MainChart.Series[item.Key].DataSource = ds;
                }

                tempReport.txtCompanyName.Text = comName;
                tempReport.pbLogo.ImageUrl = logoPath;
                tempReport.txtPrintDate.Text = dateNow;
                tempReport.TestName.Text = row["TestNameDate"].ToString();
                tempReport.NoOfTimesTaken.Text = row["NoOfTimesTaken"].ToString();
                tempReport.Lowest.Text = row["Lowest"].ToString();
                tempReport.Highest.Text = row["Highest"].ToString();
                tempReport.Average.Text = row["Average"].ToString();
                tempReport.SiteName.Text = row["CompanyName"].ToString();
                tempReport.txtRptTitle.Text = Util.GetConfig("APP_ABBRV") + " " + tempReport.txtRptTitle.Text;
                tempReport.CreateDocument();

                //MainReport.Pages.AddRange(tempReport.Pages);
                // MainReport = tempReport;
                rptAvgScoringPerNatDummy s = sender as rptAvgScoringPerNatDummy;
                s.Pages.AddRange(tempReport.Pages);  

            }
        }



    }
}
