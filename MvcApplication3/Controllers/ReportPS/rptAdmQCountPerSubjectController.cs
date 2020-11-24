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
    public class rptAdmQCountPerSubjectController : Controller
    {
        //
        // GET: /rptAdmQCountPerSubject/

        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /rptAdmQCountPerSubject/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /rptAdmQCountPerSubject/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /rptAdmQCountPerSubject/Create

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
        // GET: /rptAdmQCountPerSubject/Edit/5

        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /rptAdmQCountPerSubject/Edit/5

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
        // GET: /rptAdmQCountPerSubject/Delete/5

        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /rptAdmQCountPerSubject/Delete/5

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

            SqlDataAdapter _da = new SqlDataAdapter(GlobalVar.SubjCategoryQuery, constr);
            _dt.Clear();
            _dt.Columns.Clear();
            _da.Fill(_dt);
            ViewBag.SubjCategory = Util.ToSelectList(_dt, "SubjCategoryID", "SubjCategoryName");

            _da = new SqlDataAdapter(GlobalVar.SubjGroupQuery, constr);
            _dt.Clear();
            _dt.Columns.Clear();
            _da.Fill(_dt);
            ViewBag.SubjGroup = Util.ToSelectList(_dt, "SubjGroupID", "SubjGroupName");

            _da = new SqlDataAdapter(GlobalVar.SubjLevelQuery, constr);
            _dt.Clear();
            _dt.Columns.Clear();
            _da.Fill(_dt);
            ViewBag.SubjLevel = Util.ToSelectList(_dt, "SubjLevelID", "SubjLevelName");

            var list = new SelectList(new[] 
            {
                new { value = "SubjectName", Text = "Subject Name" },
                new { value = "COUNT(q.QuestionID)", Text = "Total No. of Questions" },
            }, "value", "Text", 1);
            ViewBag.sortitems = list;

            return PartialView();
        }

        public ActionResult SelectionList(string criteria, string sortbyname, string sortby)
        {

            string filterCriteria = ApplyCriteria(criteria);
            if (filterCriteria != "")
            {
                filterCriteria = " and " + filterCriteria;
            }

            string constr = ConfigurationManager.ConnectionStrings["dbconn"].ToString();
            SqlConnection _con = new SqlConnection(constr);

            String sql = string.Format("SELECT s.SubjectID, SubjectName, 0 IsSelected, s.SubjCategoryID, s.SubjGroupID, s.SubjLevelID FROM tblAdmQuestionSubject qs INNER JOIN tblAdmQuestions q ON q.QuestionID = qs.QuestionID INNER JOIN tblAdmSubject s ON qs.SubjectID = s.SubjectID WHERE s.IsActive=1 {0} GROUP BY s.SubjectID, s.SubjectName, s.SubjCategoryID, s.SubjGroupID, s.SubjLevelID " ,filterCriteria);

            //sql = "select * from (" + sql + ") tb " + filterCriteria;
            sql += " order by " + sortbyname + " " + sortby;

            SqlDataAdapter _da = new SqlDataAdapter(sql, constr);
            DataTable _dt = new DataTable();
            _da.Fill(_dt);

            //// ----- required : to send the selectionlist ----///////////////
            TempData["SelecionLIst"] = _dt; //get the value in the reportmain/showSelectionlist , should be datatable
            return RedirectToAction("showSelectionList", "ReportMain", new { fieldname = "SubjectName", fieldvalue = "SubjectID" });
        }


        SETSReport.Reports.rptAdmQCountPerSubject MainReport = new SETSReport.Reports.rptAdmQCountPerSubject();

        [HttpPost]
        public ActionResult DocumentViewerPartial()
        {
            string selectedIDs = Request["txtselected"].ToString();
            string sortby = Request["cbeSortBy"] == "COUNT(q.QuestionID)" ? "TotalNumberOfQuestions" : Request["cbeSortBy"];

            string sql = String.Format("SELECT *, TotalNumberOfQuestions - BlockedQuestions AvailableQuestions FROM view_ListOfSubjects WHERE SubjectID IN ({0}) ORDER BY {1} {2}", selectedIDs, sortby, Request["rgSortOrder"]);

            string constr = ConfigurationManager.ConnectionStrings["dbconn"].ToString();
            SqlConnection _con = new SqlConnection(constr);
            SqlDataAdapter _da = new SqlDataAdapter(sql, _con);

            DataSet ds = new DataSet();
            _da.Fill(ds);
            MainReport.DataMember = ds.Tables[0].TableName;
            MainReport.DataSource = ds;

            string sortbytext = Request["cbeSortBy"] == "COUNT(q.QuestionID)" ? "Total Number Of Questions" : "Subject Name";
            string sortbyorder = Request["rgSortOrder"] == "Asc" ? "(Ascending)" : "(Descending)";

            MainReport.txtSortedBy.Text = "Sorted by: " + sortbytext;
            MainReport.txtOrderBy.Text =  sortbyorder ;

            string category = Request["SubjCategoryName"];
            string group = Request["SubjGroupName"];
            string level = Request["SubjLevelName"];
            MainReport.txtSubjCategory.Text = "Category: " + category != null? category: "All";
            MainReport.txtSubjGroup.Text = "Group: " + group !=null? group: "All";
            MainReport.txtSubjLevel.Text = "Level: " + level != null ?  level: "All";

            MainReport.txtRptTitle.Text = Util.GetConfig("APP_ABBRV") + " " + MainReport.txtRptTitle.Text;

            MainReport.txtPrintDate.Text = DateTime.Now.ToString("dd-MMM-yyyy hh:mm tt");
            MainReport.pbLogo.ImageUrl = Util.GetReportLogoPath();

            DataTable dt = ds.Tables[0];
            for (int i = 0; i <= dt.Columns.Count - 1; i++)
            {
                XRControl cell = MainReport.FindControl(dt.Columns[i].ColumnName, true);
                if (cell != null)
                {
                    cell.DataBindings.Add("Text", null, dt.Columns[i].ColumnName);
                    cell.EvaluateBinding += FormatZeroValues;
                }
            }

            MainReport.txtTotalSupport.DataBindings.Add("Text", null, "Support");             
            MainReport.txtTotalOperational.DataBindings.Add("Text", null, "Operational");          
            MainReport.txtTotalManagement.DataBindings.Add("Text", null, "Management");     
            MainReport.txtTotalQuestion.DataBindings.Add("Text", null, "TotalNumberOfQuestions");

            return PartialView("_DocumentViewer1Partial", MainReport);
        }

         private void FormatZeroValues(object sender, BindingEventArgs e ){
             
             int num;
             bool success = Int32.TryParse(Convert.ToString(e.Value), out num);
             if (success)
                 if (num == 0)
                     e.Value = "";
             
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
                System.Diagnostics.Debug.WriteLine(namem);
                System.Diagnostics.Debug.WriteLine(valuen);

                if (valuen != null && valuen != "")
                {
                    searchText = (searchText != "") ? searchText += " AND " : "";
                    switch (namem)
                    {
                        case "SiteName":
                            searchText += String.Format("{0} = '{1}'", namem, valuen);
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
