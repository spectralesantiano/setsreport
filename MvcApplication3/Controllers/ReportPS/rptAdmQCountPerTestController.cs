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
    public class rptAdmQCountPerTestController : Controller
    {
        //
        // GET: /rptAdmQCountPerTest/

        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /rptAdmQCountPerTest/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /rptAdmQCountPerTest/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /rptAdmQCountPerTest/Create

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
        // GET: /rptAdmQCountPerTest/Edit/5

        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /rptAdmQCountPerTest/Edit/5

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
        // GET: /rptAdmQCountPerTest/Delete/5

        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /rptAdmQCountPerTest/Delete/5

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
            SqlDataAdapter _da = new SqlDataAdapter(Controllers.GlobalVar.TestGroupQuery, constr);
            DataTable _dt = new DataTable();
            _da.Fill(_dt);

            ViewBag.TestGroup = Util.ToSelectList(_dt, "TestGroupName", "TestGroupName");

            var list = new SelectList(new[] 
            {
                new { value = "SubjectName", Text = "Subject Name" },
                new { value = "COUNT(q.QuestionID)", Text = "Total No. of Questions" },
            }, "value", "Text", 1);
            ViewBag.sortitems = list;

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

            String sql = "SELECT DISTINCT t.TestID, TestName, dbo.GetTestGroupNames(t.TestID) TestGroupNames, 0 IsSelected FROM tblAdmTestTemplates t INNER JOIN tblAdmTestTemplateSubjects ts ON t.TestID = ts.TestID WHERE t.IsActive=1 AND ts.IsActive=1";


            sql = "select * from (" + sql + ") tb " + filterCriteria;
            sql += " order by TestName " + sortby ;

            SqlDataAdapter _da = new SqlDataAdapter(sql, constr);
            DataTable _dt = new DataTable();
            _da.Fill(_dt);

            //// ----- required : to send the selectionlist ----///////////////
            TempData["SelecionLIst"] = _dt; //get the value in the reportmain/showSelectionlist , should be datatable
            return RedirectToAction("showSelectionList", "ReportMain", new { fieldname = "TestName", fieldvalue = "TestID" });

        }

        SETSReport.Reports.rptAdmQCountPerTest MainReport = new SETSReport.Reports.rptAdmQCountPerTest();
        SETSReport.Reports.rptSubRevisionDate RevisionDates = new SETSReport.Reports.rptSubRevisionDate();

        DataTable RevisionDatesList = new DataTable() ;
        Boolean ShowRevDates = false;

        [HttpPost]
        public ActionResult DocumentViewerPartial()
        {

            MainReport.GroupHeader.BeforePrint += MainReport_BeforePrint;

            string selectedIDs = Request["txtselected"].ToString();
            string conditions = "";

            string sql = String.Format("SELECT * FROM view_ListOfTests WHERE TestID IN ({0}) ", selectedIDs);

            string constr = ConfigurationManager.ConnectionStrings["dbconn"].ToString();
            SqlConnection _con = new SqlConnection(constr);

            SqlDataAdapter _da = new SqlDataAdapter(sql, _con);

            DataSet ds = new DataSet();
            _da.Fill(ds);
            MainReport.DataMember = ds.Tables[0].TableName;
            MainReport.DataSource = ds;

            MainReport.TestName.DataBindings.Add("Text", null, MainReport.TestName.Name);
            MainReport.txtTotalTQuestion.DataBindings.Add("Text", null, MainReport.TQuestions.Name);

            MainReport.txtPrintDate.Text = "Print Date: " + DateTime.Now.ToString("dd-MMM-yyyy hh:mm tt");
            MainReport.txtCompanyName.Text = Util.GetConfig("COMPANY_NAME");
            MainReport.pbLogo.ImageUrl = Util.GetReportLogoPath();
            MainReport.txtRptTitle.Text = Util.GetConfig("APP_ABBRV") + " " + MainReport.txtRptTitle.Text;

            GroupField item = new GroupField();
            item.FieldName = "TestName";
            item.SortOrder = Request["rgSortOrder"] == "Asc" ? XRColumnSortOrder.Ascending : XRColumnSortOrder.Descending;
            MainReport.GroupHeader.GroupFields.Add(item);

            MainReport.Detail.SortFields.Add(new GroupField(Request["SortReportBy"], Request["rgSortReportOrder"] == "Asc" ? XRColumnSortOrder.Ascending : XRColumnSortOrder.Descending));

            DataTable dt = ds.Tables[0];
            for (int i = 0; i <= dt.Columns.Count - 1; i++)
            {
                XRControl cell = MainReport.FindControl(dt.Columns[i].ColumnName, true);
                if (cell != null)
                {
                    cell.DataBindings.Add("Text", null, dt.Columns[i].ColumnName);
                    continue ;

                }

                cell = MainReport.SubTable.FindControl(dt.Columns[i].ColumnName, true);
               if (cell != null)
               {
                    cell.DataBindings.Add("Text", null, dt.Columns[i].ColumnName);
                    cell.EvaluateBinding += FormatZeroValues;
               }

            }

            ShowRevDates = Convert.ToBoolean(Request["ShowRevDates"]);

            _da = new SqlDataAdapter("SELECT OrigID, FORMAT(DateCreated, 'dd-MMM-yyyy hh:mm tt', 'en-us') DateCreated FROM tblAdmTestTemplates WHERe IsActive<>1 ORDER BY DateCreated DESC", constr);
            _da.Fill(RevisionDatesList);

            RevisionDates.DateCreated.DataBindings.Add("Text", null, "DateCreated");
            MainReport.subRevisionDates.ReportSource = RevisionDates;

            return PartialView("_DocumentViewer1Partial", MainReport);
        }


        private void FormatZeroValues(object sender, BindingEventArgs e)
        {

            int num;
            bool success = Int32.TryParse(Convert.ToString(e.Value), out num);
            if (success)
                if (num == 0)
                    e.Value = "";

        }

        private void MainReport_BeforePrint(Object sender  , System.Drawing.Printing.PrintEventArgs e){
            DataRow[] rows = RevisionDatesList.Select("OrigID='" + MainReport.GetCurrentColumnValue("OrigID") + "'");
            if (rows.Length > 0) {
                RevisionDates.DataSource = rows.CopyToDataTable();
                MainReport.subRevisionDates.Visible = ShowRevDates;
            }
            else{
                RevisionDates.DataSource = null;
                MainReport.subRevisionDates.Visible = false;
            }
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
                        case "TestGroupNames":
                            searchText += String.Format("{0} LIKE '%{1}%'", namem, valuen);
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
