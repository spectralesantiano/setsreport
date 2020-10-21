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
    public class rptAdmDetailedQStatsController : Controller
    {
        //
        // GET: /rptAdmDetailedQStats/

        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /rptAdmDetailedQStats/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /rptAdmDetailedQStats/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /rptAdmDetailedQStats/Create

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
        // GET: /rptAdmDetailedQStats/Edit/5

        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /rptAdmDetailedQStats/Edit/5

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
        // GET: /rptAdmDetailedQStats/Delete/5

        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /rptAdmDetailedQStats/Delete/5

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
            DataTable _dt = new DataTable();

            SqlDataAdapter _da = new SqlDataAdapter(GlobalVar.TestNameQuery, constr);
            _dt.Clear();
            _dt.Columns.Clear();
            _da.Fill(_dt);
            ViewBag.TestName = Util.ToSelectList(_dt, "TestName", "TestName");

            _da = new SqlDataAdapter(GlobalVar.SubjNameQuery, constr);
            _dt.Clear();
            _dt.Columns.Clear();
            _da.Fill(_dt);
            ViewBag.SubjectName = Util.ToSelectList(_dt, "SubjectName", "SubjectName");

            var list = new SelectList(new[] 
            {
                new { cbeSortBy = "SubjectName", Text = "Subject Name" },
                new { cbeSortBy = "NoOfExaminees", Text = "No. of Users Tested" },
                new { cbeSortBy = "PercentRight", Text = "Percent of Correct" },
            }, "cbeSortBy", "Text", 1);
            ViewBag.sortitems = list;

            return PartialView();
        }

        public ActionResult SelectionList(string criteria, string sortbyname, string sortby)
        {

            string filterCriteria = ApplyCriteria(criteria);
            if (filterCriteria != "")
            {
                filterCriteria = " where " + filterCriteria;
            }

            string constr = ConfigurationManager.ConnectionStrings["dbconn"].ToString();
            SqlConnection _con = new SqlConnection(constr);

            String sql = string.Format("SELECT DISTINCT 0 IsSelected, SubjectName, dbo.GetSubjectTestNames(SubjectID) TestName FROM view_FullExamineeResultsWithQuestions {0} ORDER BY SubjectName ASC " , filterCriteria);

            //sql += " order by " + sortbyname + " " + sortby;

            SqlDataAdapter _da = new SqlDataAdapter(sql, constr);
            DataTable _dt = new DataTable();
            _da.Fill(_dt);

            //// ----- required : to send the selectionlist ----///////////////
            TempData["SelecionLIst"] = _dt; //get the value in the reportmain/showSelectionlist , should be datatable
            return RedirectToAction("showSelectionList", "ReportMain", new { fieldname = "SubjectName", fieldvalue = "SubjectName" });
        }

        SETSReport.Reports.rptAdmDetailedQStats MainReport = new SETSReport.Reports.rptAdmDetailedQStats();

        [HttpPost]
        public ActionResult DocumentViewerPartial()
        {
            string selectedIDs = Request["txtselected"].ToString();
            string conditions = String.Format("AND PercentRight >= {0} AND PercentRight <= {1}", Request["Min"], Request["Max"]);

            string sql = String.Format("SELECT * FROM view_QuestionStatistics WHERE SubjectName IN ({0}) {1} ORDER BY {2} {3}", selectedIDs, conditions, Request["cbeSortBy"], Request["rgSortOrder"]);

            string constr = ConfigurationManager.ConnectionStrings["dbconn"].ToString();
            SqlConnection _con = new SqlConnection(constr);
            SqlDataAdapter _da = new SqlDataAdapter(sql, _con);

            DataSet ds = new DataSet();
            _da.Fill(ds);
            MainReport.DataMember = ds.Tables[0].TableName;
            MainReport.DataSource = ds;

            MainReport.txtCompanyName.Text = Util.GetConfig("COMPANY_NAME");
            MainReport.txtRptTitle.Text = Util.GetConfig("APP_ABBRV") + " " + MainReport.txtRptTitle.Text;

            MainReport.txtPrintDate.Text = "Print Date: " + DateTime.Now.ToString("dd-MMM-yyyy hh:mm tt");
            MainReport.txtCompanyName.Text = Util.GetConfig("COMPANY_NAME");
            MainReport.pbLogo.ImageUrl = Util.GetReportLogoPath() ;
            MainReport.txtSortedBy.Text = "Sorted by " + Request["SortedByText"];
            MainReport.txtRange.Text = String.Format("% Right ranges from {0}.00 % to {1}.00 %", Request["Min"], Request["Max"]);

            DataTable dt = ds.Tables[0];
            for (int i = 0; i <= dt.Columns.Count - 1; i++)
            {
                XRControl cell = MainReport.FindControl(dt.Columns[i].ColumnName, true);
                if (cell != null)
                {
                    cell.DataBindings.Add("Text", null, dt.Columns[i].ColumnName);
                }
            }

           MainReport.PercentRight.EvaluateBinding += PercentageFormatter;
           MainReport.PercentWrong.EvaluateBinding += PercentageFormatter;
           MainReport.PercentSkipped.EvaluateBinding +=  PercentageFormatter;

            return PartialView("_DocumentViewer1Partial", MainReport);
        }

        private void PercentageFormatter(Object sender ,BindingEventArgs e){
            if (e.Value.ToString().Contains(".")) {
                e.Value = String.Format("{0:0.00}", e.Value);
                e.Value += "%";
            }
         }

        private String ApplyCriteria(string AllCriteria)
        {
            string searchText = "";
            //AllCriteria.Trim(new Char[] { '\'' });
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
                        case "TestName":
                            searchText += String.Format("{0} LIKE '%{1}%'", namem, valuen);
                            break;
                        default:
                            searchText += String.Format("{0} = '{1}'", namem, valuen);
                            break;
                    }
                }
            }

            return searchText;
        }

    }
}
