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

        // set filter data
        public ActionResult viewFilter()
        {
            string constr = ConfigurationManager.ConnectionStrings["dbconn"].ToString();
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

            //_da = new SqlDataAdapter(Controllers.GlobalVar.SiteNameQuery, constr);
            _da = new SqlDataAdapter(Controllers.GlobalVar.SiteNameQuery, constr);
            _dt.Clear();
            _dt.Columns.Clear();
            _da.Fill(_dt);
            ViewBag.CompanyName = Util.ToSelectList(_dt, "SiteName", "SiteName");

            var list = new SelectList(new [] 
            {
                new { cbeSortBy = "LName", Text = "Name" },
                new { cbeSortBy = "DateTaken", Text = "Date" },
            }, "cbeSortBy", "Text", 1);
            ViewBag.sortitems = list;


            return PartialView();
           //return View();
        }

        // this where filter data is used to filter what will appear in the selection list
        public ActionResult SelectionList(string criteria, string sortbyname ,string sortby)
        {

            string filterCriteria = ApplyCriteria(criteria);
            if (filterCriteria != "")
            {
                filterCriteria = " where " + filterCriteria;
            }

            string constr = ConfigurationManager.ConnectionStrings["dbconn"].ToString();
            SqlConnection _con = new SqlConnection(constr);

            // sql come from db [view_ReportGroups].SelctionSource
            String sql = "SELECT *, 0 IsSelected, CONCAT(LastFirstMiddle, ' - ', FORMAT(DateTaken, 'dd-MMM-yyyy hh:mm tt', 'en-us'), ' - ', TestNameDate) DisplayField FROM (select * from view_FullExamineeResults   " + (GlobalVar.SiteID==""?"": " where " + GlobalVar.SiteID) + " ) vfe " + filterCriteria;

            sql += " order by " + sortbyname + " " + sortby;

            SqlDataAdapter _da = new SqlDataAdapter(sql, constr);
            DataTable _dt = new DataTable();
             _da.Fill(_dt);

            //// ----- required
            // ViewBag.selectionlist = Util.ToSelectList(_dt, "ActualTestID", "DisplayField");
            // return PartialView("~/Views/ReportMain/SelectionList.cshtml"); //--- REQUIRED View to redirect to ---//

            //this does not work. gets null 
            //SelectList slist = Util.ToSelectList(_dt, "actualtestid", "displayfield");
             //return RedirectToAction("showSelectionList", "ReportMain", new { dt = _dt}); //slist });

             //// ----- required : to send the selectionlist ----///////////////
             TempData["SelecionLIst"] = _dt; //get the value in the reportmain/showSelectionlist , should be datatable
             return RedirectToAction("showSelectionList", "ReportMain", new { fieldname = "DisplayField", fieldvalue = "ActualTestID" }); 
        }

        public JsonResult GetSubjects(string SubjCategoryID)
        {
            string constr = ConfigurationManager.ConnectionStrings["dbconn"].ToString();
            SqlConnection _con = new SqlConnection(constr);

            SqlDataAdapter _da = new SqlDataAdapter("SELECT * from tblAdmSubject where SubjCategoryID =" + SubjCategoryID, constr);
            DataTable _dt = new DataTable();
            _da.Fill(_dt);
            //ViewBag.SubjectList = Util.ToSelectList(_dt, "SubjectID", "SubjectName");
            var list = Util.ToSelectList(_dt, "SubjectID", "SubjectName");
            return Json(list);
            //return View();
        }

        //SETSReport.Reports.XtraReport2 report = new SETSReport.Reports.XtraReport2();
        SETSReport.Reports.rptIndiTestResult MainReport = new SETSReport.Reports.rptIndiTestResult();

        [HttpPost]
        public ActionResult DocumentViewerPartial()
        {
            //SETSReport.Reports.XtraReport2 report = new SETSReport.Reports.XtraReport2();
            //DevExpress.XtraReports.UI.XRLabel lbl = ((DevExpress.XtraReports.UI.XRLabel)report.FindControl("xrlabel1", true));
           // lbl.Text = Session["amount"].ToString();
            //report.DataSource = "SELECT * FROM [SETS].[dbo].[view_FullExamineeResultsWithQuestions] ";// where actualtestid in(" + Session["selected"] + ") ORDER BY LastFirstMiddle ASC"; //WHERE ActualTestID IN ({0}) {1}ORDER BY LastFirstMiddle ASC";

            if (!Util.isSessionValid())
            {
                return View("noReferer");
            }

            string selectedIDs = Request["txtselected"].ToString();
             string conditions = "";

             if (Request["AnswerFilter"].ToString() != "2") // "" in sets desktop..empty space causes error in view
             {
                 conditions = String.Format("AND Answer {0} UserAns ", ((Request["AnswerFilter"].ToString()) == "0") ? "!=": "=");
             }

            //sql from report class. which from desktop sets

             string sql = String.Format("SELECT * FROM view_FullExamineeResultsWithQuestions  where " + (GlobalVar.SiteID==""?"":GlobalVar.SiteID + " and ")  + " ActualTestID IN ({0}) {1}ORDER BY LastFirstMiddle ASC", selectedIDs, conditions);


            string constr = ConfigurationManager.ConnectionStrings["dbconn"].ToString();
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

            //--- !Declaration of controls in report's designer.cs should all be public change from private
            ViewBag.selection = Request["teFirstName"];
            MainReport.txtPrintDate.Text = "Print Date: "   + DateTime.Now.ToString("dd-MMM-yyyy hh:mm tt");
            MainReport.txtCompanyName.Text = Util.GetConfig("COMPANY_NAME");
            MainReport.pbLogo.ImageUrl = Util.GetReportLogoPath();
            MainReport.txtRptTitle.Text = Util.GetConfig("APP_ABBRV") + " " + MainReport.txtRptTitle.Text;
            
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
            //AllCriteria.Trim(new Char[] { '\'' });
            string filterlist = AllCriteria.Trim(new Char[] { '\'' });  // "{\"FName\":\"ghdfd\",\"LName\":\"dfdf\",\"PositionID\":\"SYSR1E\",\"TestName\":\"2nd Officer Test for cargo ships\",\"DateTaken\":\"09/10/2020\",\"ToDate\":\"09/11/2020\",\"AnswerFilter\":\"2\",\"Nat\":\"SYSCNAX\",\"CompanyName\":\"Spectral Technologies. Inc.\"}";
            var filter = JsonConvert.DeserializeObject<dynamic>(filterlist);

            string namem = "";
            var valuen="";
            foreach (var record in filter)
            {
                namem = record.Name;
                valuen = record.Value;
                //System.Diagnostics.Debug.WriteLine(namem);
                //System.Diagnostics.Debug.WriteLine(valuen);

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
                                //DateTime frDate  = Convert.ToDateTime(valuen);
                                //searchText += String.Format("{0} >= '{1}'", namem, frDate);
                                searchText += String.Format("{0} >= '{1}'", namem, valuen);
                                break;
                            case "ToDate":
                                DateTime toDate  = Convert.ToDateTime(valuen);
                                toDate = toDate.AddDays(1);
                                //searchText += String.Format("DateTaken <= '{0}'", toDate);
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
                            case "SiteName":
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


        //[NonAction]
        //public SelectList ToSelectList(DataTable table, string valueField, string textField)
        //{
        //    List<SelectListItem> list = new List<SelectListItem>();

        //    foreach (DataRow row in table.Rows)
        //    {
        //        list.Add(new SelectListItem()
        //        {
        //            Text = row[textField].ToString(),
        //            Value = row[valueField].ToString()
        //        });
        //    }

        //    return new SelectList(list, "Value", "Text");
        //}
    }
}
