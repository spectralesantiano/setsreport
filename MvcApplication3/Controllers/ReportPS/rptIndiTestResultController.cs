using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DevExpress.Web.Mvc;
using MvcApplication3.Models;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using DevExpress.DataAccess.Sql;
using DevExpress.DataAccess.ConnectionParameters;
using DevExpress.XtraReports.UI;
using DevExpress.XtraReports.Configuration;
using MvcApplication3.Controllers;
using Newtonsoft.Json;


namespace MvcApplication3.Controllers.ReportPS
{
    public class rptIndiTestResultController : Controller
    {
        //
        // GET: /rptINdiTestResutl/

        public ActionResult Index() 
        {
           // return RedirectToAction("Index", "Home");
           //Session["amount"] = Request["txtAmount"].ToString();  //old not needed
           //Session["selected"] = Request["txtselected"].ToString(); //old not needed
            return View();
        }

        //
        // GET: /rptINdiTestResutl/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /rptINdiTestResutl/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /rptINdiTestResutl/Create

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
        // GET: /rptINdiTestResutl/Edit/5

        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /rptINdiTestResutl/Edit/5

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
        // GET: /rptINdiTestResutl/Delete/5

        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /rptINdiTestResutl/Delete/5

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
            ViewBag.RankList = ToSelectList(_dt, "PositionID", "Abbrv");

            _da = new SqlDataAdapter("Select * From [tblAdmSubjectCategory]", constr);
            _dt.Clear();
            _dt.Columns.Clear();
            _da.Fill(_dt);
            ViewBag.SubjectCat = ToSelectList(_dt, "SubjCategoryID", "SubjCategoryName");

            _da = new SqlDataAdapter(Controllers.GlobalVar.TestNameQuery, constr);
            _dt.Clear();
            _dt.Columns.Clear();
            _da.Fill(_dt);
            ViewBag.TestName = ToSelectList(_dt, "TestName", "TestName");

            _da = new SqlDataAdapter(Controllers.GlobalVar.NationalityQuery, constr);
            _dt.Clear();
            _dt.Columns.Clear();
            _da.Fill(_dt);
            ViewBag.Nationality = ToSelectList(_dt, "PKey", "Nat");

            _da = new SqlDataAdapter(Controllers.GlobalVar.CompanyNameQuery, constr);
            _dt.Clear();
            _dt.Columns.Clear();
            _da.Fill(_dt);
            ViewBag.CompanyName = ToSelectList(_dt, "CompanyName", "CompanyName");

            var list = new SelectList(new [] 
            {
                new { cbeSortBy = "1", Text = "Name" },
                new { cbeSortBy = "2", Text = "Date" },
            }, "cbeSortBy", "Text", 1);
            ViewBag.sortitems = list;


            return PartialView();
           //return View();
        }

        public ActionResult SelectionList(string criteria)
        {

            string filterCriteria = ApplyCriteria(criteria);

            string constr = ConfigurationManager.ConnectionStrings["dropdownconn"].ToString();
            SqlConnection _con = new SqlConnection(constr);

            SqlDataAdapter _da = new SqlDataAdapter("SELECT *, 0 IsSelected, CONCAT(LastFirstMiddle, ' - ', FORMAT(DateTaken, 'dd-MMM-yyyy hh:mm tt', 'en-us'), ' - ', TestNameDate) DisplayField FROM view_FullExamineeResults where " + filterCriteria, constr);
            DataTable _dt = new DataTable();
             _da.Fill(_dt);
             ViewBag.selectionlist = ToSelectList(_dt, "ActualTestID", "DisplayField");
            return PartialView("~/Views/ReportMain/SelectionList.cshtml"); //--- REQUIRED View to redirect to ---//
        }

        public JsonResult GetSubjects(string SubjCategoryID)
        {
            string constr = ConfigurationManager.ConnectionStrings["dropdownconn"].ToString();
            SqlConnection _con = new SqlConnection(constr);

            SqlDataAdapter _da = new SqlDataAdapter("SELECT * from tblAdmSubject where SubjCategoryID =" + SubjCategoryID, constr);
            DataTable _dt = new DataTable();
            _da.Fill(_dt);
            //ViewBag.SubjectList = ToSelectList(_dt, "SubjectID", "SubjectName");
            var list = ToSelectList(_dt, "SubjectID", "SubjectName");
            return Json(list);
            //return View();
        }

        //MvcApplication3.Reports.XtraReport2 report = new MvcApplication3.Reports.XtraReport2();
        MvcApplication3.Reports.rptIndiTestResult MainReport = new MvcApplication3.Reports.rptIndiTestResult();

        [HttpPost]
        public ActionResult DocumentViewerPartial()
        {
            //MvcApplication3.Reports.XtraReport2 report = new MvcApplication3.Reports.XtraReport2();
            //DevExpress.XtraReports.UI.XRLabel lbl = ((DevExpress.XtraReports.UI.XRLabel)report.FindControl("xrlabel1", true));
           // lbl.Text = Session["amount"].ToString();
            //report.DataSource = "SELECT * FROM [SETS].[dbo].[view_FullExamineeResultsWithQuestions] ";// where actualtestid in(" + Session["selected"] + ") ORDER BY LastFirstMiddle ASC"; //WHERE ActualTestID IN ({0}) {1}ORDER BY LastFirstMiddle ASC";
             string selectedIDs = Request["txtselected"].ToString();
             string conditions = "";

             if (Request["AnswerFilter"].ToString() != "2") // "" in sets desktop..empty space causes error in view
             {
                 conditions = String.Format("AND Answer {0} UserAns ", ((Request["AnswerFilter"].ToString()) == "0") ? "!=": "=");
             }

             string sql = String.Format("SELECT * FROM view_FullExamineeResultsWithQuestions WHERE ActualTestID IN ({0}) {1}ORDER BY LastFirstMiddle ASC", selectedIDs, conditions);


            string constr = ConfigurationManager.ConnectionStrings["dropdownconn"].ToString();
            SqlConnection _con = new SqlConnection(constr);

            //SqlDataAdapter _da = new SqlDataAdapter("SELECT * FROM [SETS].[dbo].[view_FullExamineeResultsWithQuestions] where actualtestid in(" + Session["selected"] + ") ORDER BY LastFirstMiddle ASC", _con);
            //SqlDataAdapter _da = new SqlDataAdapter("SELECT * FROM [SETS].[dbo].[view_FullExamineeResultsWithQuestions] where actualtestid in(" + Request["txtselected"].ToString() + ") ORDER BY LastFirstMiddle ASC", _con);
            SqlDataAdapter _da = new SqlDataAdapter(sql, _con);
            
            DataSet ds = new DataSet();
            _da.Fill(ds);
            MainReport.DataMember = ds.Tables[0].TableName;
            MainReport.DataSource = ds;

            //return PartialView("~/Views/rptIndiTestResult/_DocumentViewer1Partial.cshtml", report);
            //return PartialView("~/Views/ReportMain/ReportFilters/_DocumentViewerPartial.cshtml", report);
            //return PartialView("_DocumentViewer1Partial", report);
            ViewBag.selection = Request["teFirstName"];
            MainReport.FindControl("txtPrintDate", true).Text = "Print Date: "   + DateTime.Now.ToString("dd-MMM-yyyy hh:mm tt");

            
            GroupField item = new GroupField();
            item.FieldName = "ActualTestID";
            item.SortOrder = XRColumnSortOrder.Ascending;
            MainReport.ActualTestHeader.GroupFields.Add(item);
            MainReport.SubjectName.DataBindings.Add("Text", null, "ActualTestID");

            item = new GroupField();
            item.FieldName = "SubjectName";
            item.SortOrder = XRColumnSortOrder.Ascending;
            MainReport.SubjectNameHeader.GroupFields.Add(item);
            MainReport.SubjectName.DataBindings.Add("Text", null, "SubjectName");

            DataTable dt =ds.Tables[0];
             for(int i  = 0 ;i <= dt.Columns.Count - 1;i++){
                XRControl cell   = MainReport.FindControl(dt.Columns[i].ColumnName,true);
                if(cell != null) {
                    cell.DataBindings.Add("Text", null, dt.Columns[i].ColumnName);
                }
             }

            return PartialView("_DocumentViewer1Partial", MainReport);
        }

        private String ApplyCriteria(string AllCriteria)
        {  
            string searchText = "";
            AllCriteria.Trim(new Char[] { '\'' });
            string filterlist = AllCriteria.Trim(new Char[] { '\'' });  // "{\"FName\":\"ghdfd\",\"LName\":\"dfdf\",\"PositionID\":\"SYSR1E\",\"TestName\":\"2nd Officer Test for cargo ships\",\"DateTaken\":\"09/10/2020\",\"ToDate\":\"09/11/2020\",\"AnswerFilter\":\"2\",\"Nat\":\"SYSCNAX\",\"CompanyName\":\"Spectral Technologies. Inc.\"}";
            var filter = JsonConvert.DeserializeObject<dynamic>(filterlist);

            string namem = "";
            var valuen="";
            foreach (var record in filter)
            {
                namem = record.Name;
                valuen = record.Value;
                System.Diagnostics.Debug.WriteLine(namem);
                System.Diagnostics.Debug.WriteLine(valuen);

                if (valuen != null && valuen != "")
                {
                     searchText = (searchText !="")? searchText += " AND ": "";
                        switch (namem) {
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
                                searchText += String.Format("DateTaken <= '{0}'", toDate);
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


        [NonAction]
        public SelectList ToSelectList(DataTable table, string valueField, string textField)
        {
            List<SelectListItem> list = new List<SelectListItem>();

            foreach (DataRow row in table.Rows)
            {
                list.Add(new SelectListItem()
                {
                    Text = row[textField].ToString(),
                    Value = row[valueField].ToString()
                });
            }

            return new SelectList(list, "Value", "Text");
        }
    }
}
