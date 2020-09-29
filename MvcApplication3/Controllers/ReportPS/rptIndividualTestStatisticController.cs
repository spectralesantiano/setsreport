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
    public class rptIndividualTestStatisticController : Controller
    {
        //
        // GET: /rptIndividualTestStatistic/

        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /rptIndividualTestStatistic/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /rptIndividualTestStatistic/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /rptIndividualTestStatistic/Create

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
        // GET: /rptIndividualTestStatistic/Edit/5

        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /rptIndividualTestStatistic/Edit/5

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
        // GET: /rptIndividualTestStatistic/Delete/5

        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /rptIndividualTestStatistic/Delete/5

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
            string constr = ConfigurationManager.ConnectionStrings["dropdownconn"].ToString();
            SqlConnection _con = new SqlConnection(constr);
            SqlDataAdapter _da = new SqlDataAdapter("Select * From [tbladmrank]", constr);
            DataTable _dt = new DataTable();
            _da.Fill(_dt);
            ViewBag.RankList = Util.ToSelectList(_dt, "PositionID", "Abbrv");

            _da = new SqlDataAdapter("Select * From [tblAdmSubjectCategory]", constr);
            _dt.Clear();
            _dt.Columns.Clear();
            _da.Fill(_dt);
            ViewBag.SubjectCat = Util.ToSelectList(_dt, "SubjCategoryID", "SubjCategoryName");

            _da = new SqlDataAdapter(Controllers.GlobalVar.TestNameQuery, constr);
            _dt.Clear();
            _dt.Columns.Clear();
            _da.Fill(_dt);
            ViewBag.TestName = Util.ToSelectList(_dt, "TestName", "TestName");

            _da = new SqlDataAdapter(Controllers.GlobalVar.NationalityQuery, constr);
            _dt.Clear();
            _dt.Columns.Clear();
            _da.Fill(_dt);
            ViewBag.Nationality = Util.ToSelectList(_dt, "PKey", "Nat");

            _da = new SqlDataAdapter(Controllers.GlobalVar.CompanyNameQuery, constr);
            _dt.Clear();
            _dt.Columns.Clear();
            _da.Fill(_dt);
            ViewBag.CompanyName = Util.ToSelectList(_dt, "CompanyName", "CompanyName");

            var list = new SelectList(new[] 
            {
                new { cbeSortBy = "LName", Text = "Name" },
                new { cbeSortBy = "DateTaken", Text = "Date" },
            }, "cbeSortBy", "Text", 1);
            ViewBag.sortitems = list;


            return PartialView();
            //return View();
        }

        public ActionResult SelectionList(string criteria, string sortbyname, string sortby)
        {

            string filterCriteria = ApplyCriteria(criteria);
            if (filterCriteria != "")
            {
                filterCriteria = " where " + filterCriteria;
            }

            string constr = ConfigurationManager.ConnectionStrings["dropdownconn"].ToString();
            SqlConnection _con = new SqlConnection(constr);

            String sql = "SELECT *, 0 IsSelected, CONCAT(LastFirstMiddle, ' - ', FORMAT(DateTaken, 'dd-MMM-yyyy hh:mm tt', 'en-us'), ' - ', TestNameDate) DisplayField FROM view_FullExamineeResults " + filterCriteria;

            sql += " order by " + sortbyname + " " + sortby;

            SqlDataAdapter _da = new SqlDataAdapter(sql, constr);
            DataTable _dt = new DataTable();
            _da.Fill(_dt);

            //// ----- required
            // ViewBag.selectionlist = ToSelectList(_dt, "ActualTestID", "DisplayField");
            // return PartialView("~/Views/ReportMain/SelectionList.cshtml"); //--- REQUIRED View to redirect to ---//

            //this does not work. gets null 
            //SelectList slist = ToSelectList(_dt, "actualtestid", "displayfield");
            //return RedirectToAction("showSelectionList", "ReportMain", new { dt = _dt}); //slist });

            //// ----- required : to send the selectionlist ----///////////////
            TempData["SelecionLIst"] = _dt; //get the value in the reportmain/showSelectionlist , should be datatable
            return RedirectToAction("showSelectionList", "ReportMain", new { fieldname = "DisplayField", fieldvalue = "ActualTestID" });
        }

        public JsonResult GetSubjects(string SubjCategoryID)
        {
            string constr = ConfigurationManager.ConnectionStrings["dropdownconn"].ToString();
            SqlConnection _con = new SqlConnection(constr);

            SqlDataAdapter _da = new SqlDataAdapter("SELECT * from tblAdmSubject where SubjCategoryID =" + SubjCategoryID, constr);
            DataTable _dt = new DataTable();
            _da.Fill(_dt);
            //ViewBag.SubjectList = ToSelectList(_dt, "SubjectID", "SubjectName");
            var list = Util.ToSelectList(_dt, "SubjectID", "SubjectName");
            return Json(list);
            //return View();
        }

        SETSReport.Reports.rptIndividualTestStatistic MainReport = new SETSReport.Reports.rptIndividualTestStatistic();

        [HttpPost]
        public ActionResult DocumentViewerPartial()
        {
            string selectedIDs = Request["txtselected"].ToString();
            string conditions = "";

            if (Request["AnswerFilter"].ToString() != "2") // "" in sets desktop..empty space causes error in view
            {
                conditions = String.Format("AND Answer {0} UserAns ", ((Request["AnswerFilter"].ToString()) == "0") ? "!=" : "=");
            }

            string sql = String.Format("SELECT LastFirstMiddle, RefID, CompanyName, RankName, DateTaken, TestName, TestStatusName, TimeLimit, TimeTakenSec, ActualTestID, SubjectName, Level2MarkMin, Level3MarkMin, Average, TestScore, TotalPercent, " +
                "COUNT(CASE WHEN Difficulty = 'SYSMANAGEMENT' THEN 1 ELSE NULL END) TManagement, " +
                "COUNT(CASE WHEN Difficulty = 'SYSOPERATIONAL' THEN 1 ELSE NULL END) TOperational, " +
                "COUNT(CASE WHEN Difficulty = 'SYSSUPPORT' THEN 1 ELSE NULL END) TSupport, " +
                "COUNT(CASE WHEN IsCorrect = 1 THEN 1 ELSE NULL END) UserScore, COUNT(Answer) TotalScore " +
            "FROM view_FullExamineeResultsWithQuestions " +
            "WHERE ActualTestID IN ({0}) " +
            "GROUP BY LastFirstMiddle, RefID, CompanyName, RankName, DateTaken, TestName, TestStatusName, TimeLimit, TimeTakenSec, ActualTestID, SubjectName, Level2MarkMin, Level3MarkMin, Average, TestScore, TotalPercent " +
            "ORDER BY LastFirstMiddle ASC", selectedIDs, conditions);


            string constr = ConfigurationManager.ConnectionStrings["dropdownconn"].ToString();
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

            GroupField item = new GroupField();
            item.FieldName = "ActualTestID";
            item.SortOrder = XRColumnSortOrder.Ascending;
            MainReport.ActualTestHeader.GroupFields.Add(item);
            MainReport.SubjectName.DataBindings.Add("Text", null, "ActualTestID");

            DataTable dt = ds.Tables[0];
            for (int i = 0; i <= dt.Columns.Count - 1; i++)
            {
                XRControl cell = MainReport.FindControl(dt.Columns[i].ColumnName, true);
                if (cell != null)
                {
                    cell.DataBindings.Add("Text", null, dt.Columns[i].ColumnName);
                }
            }

            MainReport.TotalQuestions.DataBindings.Add("Text", null, "TotalScore");
            MainReport.txtTotalQuestion.DataBindings.Add("Text", null, "TotalScore");

            return PartialView("_DocumentViewer1Partial", MainReport);
        }

        private String ApplyCriteria(string AllCriteria)
        {
            string searchText = "";
            AllCriteria.Trim(new Char[] { '\'' });
            string filterlist = AllCriteria.Trim(new Char[] { '\'' });  // "{\"FName\":\"ghdfd\",\"LName\":\"dfdf\",\"PositionID\":\"SYSR1E\",\"TestName\":\"2nd Officer Test for cargo ships\",\"DateTaken\":\"09/10/2020\",\"ToDate\":\"09/11/2020\",\"AnswerFilter\":\"2\",\"Nat\":\"SYSCNAX\",\"CompanyName\":\"Spectral Technologies. Inc.\"}";
            var filter = JsonConvert.DeserializeObject<dynamic>(filterlist);

            string namem = "";
            var valuen = "";
            foreach (var record in filter)
            {
                namem = record.Name;
                valuen = record.Value;
                System.Diagnostics.Debug.WriteLine(namem);
                System.Diagnostics.Debug.WriteLine(valuen);

                if (valuen != null && valuen != "")
                {
                    searchText = (searchText != "") ? searchText += " AND " : "";
                    switch (namem)
                    {
                        case "FName":
                            searchText += String.Format("{0} LIKE '%{1}%'", namem, valuen);
                            break;
                        case "LName":
                            searchText += String.Format("{0} LIKE '%{1}%'", namem, valuen);
                            break;
                        case "DateTaken":
                            searchText += String.Format("{0} >= '{1}'", namem, valuen);
                            break;
                        case "ToDate":
                            DateTime toDate  = Convert.ToDateTime(valuen);
                            toDate = toDate.AddDays(1);
                            searchText += String.Format("DateTaken <= '{0}'", toDate.ToString("dd-MMM-yyyy"));
                            break;
                        case "PositionID":
                            searchText += String.Format("{0} = '{1}'", namem, valuen);
                            break;
                        case "TestName":
                            searchText += String.Format("{0} LIKE '%{1}%'", namem, valuen);
                            break;
                        case "Nat":
                            searchText += String.Format("{0} = '{1}'", namem, valuen);
                            break;
                        case "CompanyName":
                            searchText += String.Format("{0} = '{1}'", namem, valuen);
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

        public ActionResult DocumentViewerPartialExport()
        {
            return DocumentViewerExtension.ExportTo(MainReport, Request);
        }



    }
}
