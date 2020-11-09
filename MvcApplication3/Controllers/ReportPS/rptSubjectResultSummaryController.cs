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
    public class rptSubjectResultSummaryController : Controller
    {
        //
        // GET: /rptSubjectResultSummary/

        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /rptSubjectResultSummary/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /rptSubjectResultSummary/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /rptSubjectResultSummary/Create

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
        // GET: /rptSubjectResultSummary/Edit/5

        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /rptSubjectResultSummary/Edit/5

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
        // GET: /rptSubjectResultSummary/Delete/5

        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /rptSubjectResultSummary/Delete/5

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
            SqlDataAdapter _da = new SqlDataAdapter("Select * From [tbladmrank]", constr);
            DataTable _dt = new DataTable();
            _da.Fill(_dt);
            ViewBag.RankList = Util.ToSelectList(_dt, "PositionID", "Abbrv");

            _da = new SqlDataAdapter(Controllers.GlobalVar.TestNameQuery, constr);
            _dt.Clear();
            _dt.Columns.Clear();
            _da.Fill(_dt);
            ViewBag.TestName = Util.ToSelectList(_dt, "TestName", "TestName");

            _da = new SqlDataAdapter(Controllers.GlobalVar.VesselTypeQuery, constr);
            _dt.Clear();
            _dt.Columns.Clear();
            _da.Fill(_dt);
            ViewBag.VesselType = Util.ToSelectList(_dt, "VesselTypeID", "VesselType");

            _da = new SqlDataAdapter(Controllers.GlobalVar.CompanyNameQuery, constr);
            _dt.Clear();
            _dt.Columns.Clear();
            _da.Fill(_dt);
            ViewBag.CompanyName = Util.ToSelectList(_dt, "CompanyName", "CompanyName");

            var list = new SelectList(new[] 
            {
               new { Text = "Rank", SortReportBy = "RankName" },
               new { Text = "Examinee Name", SortReportBy = "LastFirstMiddle" },
               new { Text = "Date Taken", SortReportBy = "DateTaken" },
               new { Text = "Test Name", SortReportBy = "TestName" },
               new { Text = "Score", SortReportBy = "TotalPercent" },
            }, "SortReportBy", "Text", 1);
            ViewBag.SortReportBy = list;

            return PartialView();
            //return View();
        }

        public ActionResult SelectionList(string criteria, string sortbyname, string sortby)
        {

            //string filterCriteria = ApplyCriteria(criteria);
            //if (filterCriteria != "")
            //{
            //    filterCriteria = " where " + filterCriteria;
            //}

            string constr = ConfigurationManager.ConnectionStrings["dbconn"].ToString();
            SqlConnection _con = new SqlConnection(constr);

            //String sql = "SELECT DISTINCT SubjectName, s.SubjectID, 0 IsSelected FROM tblAdmSubject s INNER JOIN tblAdmTestTemplateSubjects tts ON s.SubjectID = tts.SubjectID INNER JOIN tblAdmTestTemplates tt ON tt.TestID = tts.TestID INNER JOIN tblActualTest at ON at.TestID = tt.TestID ";
            String sql = "SELECT DISTINCT SubjectName, s.SubjectID, 0 IsSelected FROM tblAdmSubject s INNER JOIN tblAdmTestTemplateSubjects tts ON s.SubjectID = tts.SubjectID INNER JOIN tblAdmTestTemplates tt ON tt.TestID = tts.TestID INNER JOIN " +
                "(select tblActualTest.*,tblexaminee.SiteID from tblActualTest inner join tblexaminee on tblactualtest.examineeid = tblexaminee.examineeid where Siteid = '" + GlobalVar.SiteID  + "') " +
                " at ON at.TestID = tt.TestID ";


            sql = "select * from (" + sql + ") tb "; //+ filterCriteria;
            sql += " order by SubjectName " + sortby ;

            SqlDataAdapter _da = new SqlDataAdapter(sql, constr);
            DataTable _dt = new DataTable();
            _da.Fill(_dt);

            //// ----- required : to send the selectionlist ----///////////////
            TempData["SelecionLIst"] = _dt; //get the value in the reportmain/showSelectionlist , should be datatable
            return RedirectToAction("showSelectionList", "ReportMain", new { fieldname = "SubjectName", fieldvalue = "SubjectID" });
        }

        SETSReport.Reports.rptSubjectResultSummary MainReport = new SETSReport.Reports.rptSubjectResultSummary();

        [HttpPost]
        public ActionResult DocumentViewerPartial()
        {
            string selectedIDs = Request["txtselected"].ToString();

            IDictionary<string, string> previewcriteria = new Dictionary<string, string>();
            previewcriteria.Add("PositionID", Request["PositionID"].ToString());
            previewcriteria.Add("VesselTypeID"        ,  Request[  "VesselTypeID" ] .ToString()   );
            previewcriteria.Add("TestName"            ,  Request[  "TestName" ]     .ToString()   );
            previewcriteria.Add("CompanyName"         , Request["CompanyName"].ToString());
            previewcriteria.Add("FromDate"            ,  Request[  "deFromDate" ]     .ToString()   );
            previewcriteria.Add("ToDate"              ,  Request[  "deToDate" ]       .ToString()   );
           

            string filterCriteria = ApplyCriteria(previewcriteria);

            //string sql = String.Format(
            //"SELECT *, CAST((CAST(UserScore AS FLOAT) / TotalScore) * 100 AS FLOAT) TotalPercent, " +
            //    "CONCAT(CONVERT(DECIMAL(10,2), CAST((CAST(UserScore AS FLOAT) / TotalScore) * 100 AS FLOAT)), ' % (', UserScore, '/', TotalScore, ')') SubjectScore " +
            //"FROM (" +
            //    "SELECT SubjectID, LastFirstMiddle, PositionID, RankName, DateTaken, TestID, TestName, TestStatusName, SubjectName, CompanyName, COUNT(CASE WHEN IsCorrect = 1 THEN 1 ELSE NULL END) UserScore, COUNT(Answer) TotalScore " +
            //    "FROM view_FullExamineeResultsWithQuestions " +
            //    "GROUP BY SubjectID, LastFirstMiddle, PositionID, RankName, DateTaken, TestID, TestName, TestStatusName, SubjectName, CompanyName" +
            //    ") T WHERE SubjectID IN ({0}) {1}", selectedIDs, filterCriteria !=""? " And " + filterCriteria:""); //SubjectName to SubjectID...
            
            string sql = String.Format(
            "SELECT *, CAST((CAST(UserScore AS FLOAT) / TotalScore) * 100 AS FLOAT) TotalPercent, " +
                "CONCAT(CONVERT(DECIMAL(10,2), CAST((CAST(UserScore AS FLOAT) / TotalScore) * 100 AS FLOAT)), ' % (', UserScore, '/', TotalScore, ')') SubjectScore " +
            "FROM (" +
                "SELECT SubjectID, LastFirstMiddle, PositionID, RankName, DateTaken, TestID, TestName, TestStatusName, SubjectName, CompanyName, COUNT(CASE WHEN IsCorrect = 1 THEN 1 ELSE NULL END) UserScore, COUNT(Answer) TotalScore " +
                "FROM (select * from view_FullExamineeResultsWithQuestions where SiteID ='"+ GlobalVar.SiteID +"') vfe " +
                "GROUP BY SubjectID, LastFirstMiddle, PositionID, RankName, DateTaken, TestID, TestName, TestStatusName, SubjectName, CompanyName" +
                ") T WHERE SubjectID IN ({0}) {1}", selectedIDs, filterCriteria != "" ? " And " + filterCriteria : ""); //SubjectName to SubjectID...

            string constr = ConfigurationManager.ConnectionStrings["dbconn"].ToString();
            SqlConnection _con = new SqlConnection(constr);

            SqlDataAdapter _da = new SqlDataAdapter(sql, _con);

            DataSet ds = new DataSet();
            _da.Fill(ds);
            MainReport.DataMember = ds.Tables[0].TableName;
            MainReport.DataSource = ds;

            ViewBag.selection = Request["teFirstName"];
            MainReport.txtPrintDate.Text = "Print Date: " + DateTime.Now.ToString("dd-MMM-yyyy hh:mm tt");
            MainReport.txtCompanyName.Text = Util.GetConfig("COMPANY_NAME");
            MainReport.pbLogo.ImageUrl = Util.GetReportLogoPath();
            MainReport.txtRptTitle.Text = Util.GetConfig("APP_ABBRV") + " " + MainReport.txtRptTitle.Text;

            MainReport.SubjectHeader.GroupFields.Add(new GroupField("SubjectName", Request["rgSortOrder"] == "ASC" ? XRColumnSortOrder.Ascending : XRColumnSortOrder.Descending));
            MainReport.Detail.SortFields.Add(new GroupField(Request["SortReportBy"], Request["rptSortdOrder"] ==  "ASC" ? XRColumnSortOrder.Ascending : XRColumnSortOrder.Descending));

            MainReport.SubjectName.DataBindings.Add("Text", null, "SubjectName");
            MainReport.txtAvgScore.DataBindings.Add("Text", null, "TotalPercent");
            MainReport.lblAvgScore.DataBindings.Add("Text", null, "TestName");

            DataTable dt = ds.Tables[0];
            for (int i = 0; i <= dt.Columns.Count - 1; i++)
            {
                XRControl cell = MainReport.FindControl(dt.Columns[i].ColumnName, true);
                if (cell != null)
                {
                    cell.DataBindings.Add("Text", null, dt.Columns[i].ColumnName);
                }
            }

            MainReport.txtAvgScore.DataBindings.Add("Text", null, "TotalPercent");

            return PartialView("_DocumentViewer1Partial", MainReport);
        }

        private String ApplyCriteria(IDictionary<string, string> AllCriteria)
        {
            
            string searchText = "";

            //if (AllCriteria != "")
            //{

            //string filterlist = AllCriteria.Trim(new Char[] { '\'' });  // "{\"FName\":\"ghdfd\",\"LName\":\"dfdf\",\"PositionID\":\"SYSR1E\",\"TestName\":\"2nd Officer Test for cargo ships\",\"DateTaken\":\"09/10/2020\",\"ToDate\":\"09/11/2020\",\"AnswerFilter\":\"2\",\"Nat\":\"SYSCNAX\",\"CompanyName\":\"Spectral Technologies. Inc.\"}";
            //    var filter = JsonConvert.DeserializeObject<dynamic>(filterlist);

                string namem = "";
                var valuen = "";
                foreach (var criteria in AllCriteria)
                {
                    namem = criteria.Key;
                    valuen = criteria.Value;
                    System.Diagnostics.Debug.WriteLine(namem);
                    System.Diagnostics.Debug.WriteLine(valuen);

                    if (valuen != null && valuen != "")
                    {
                        searchText = (searchText != "") ? searchText += " AND " : "";
                        switch (namem)
                        {
                            case "FromDate":
                                searchText += String.Format("{0} >= '{1}'", "DateTaken", valuen);
                                break;
                            case "ToDate":
                                DateTime toDate = Convert.ToDateTime(valuen);
                                toDate = toDate.AddDays(1);
                                searchText += String.Format("DateTaken <= '{0}'", toDate.ToString("dd-MMM-yyyy"));
                                break;
                            case "VesselTypeID":
                                searchText += String.Format("{0} LIKE '%{1}%'", "[dbo].[GetVesselTypes](TestID)", valuen);
                                break;
                            default:
                                searchText += String.Format("{0} = '{1}'", namem, valuen);
                                break;
                        }
                    }
                    //   End If
                  //}
              }


            return searchText;
        }

        public ActionResult DocumentViewerPartialExport()
        {
            return DocumentViewerExtension.ExportTo(MainReport, Request);
        }



    }
}
