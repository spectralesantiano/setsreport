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



namespace MvcApplication3.Controllers.ReportPS
{
    public class rptIndiTestResultController : Controller
    {
        //
        // GET: /rptINdiTestResutl/

        public ActionResult Index() 
        {
           // return RedirectToAction("Index", "Home");
           Session["amount"] = Request["txtAmount"].ToString();
           Session["selected"] = Request["txtselected"].ToString();
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
            _da.Fill(_dt);
            ViewBag.SubjectCat = ToSelectList(_dt, "SubjCategoryID", "SubjCategoryName");

            return PartialView();
           //return View();
        }

        public ActionResult SelectionList(string rankid)
        {
            string constr = ConfigurationManager.ConnectionStrings["dropdownconn"].ToString();
            SqlConnection _con = new SqlConnection(constr);

            SqlDataAdapter _da = new SqlDataAdapter("SELECT *, 0 IsSelected, CONCAT(LastFirstMiddle, ' - ', FORMAT(DateTaken, 'dd-MMM-yyyy hh:mm tt', 'en-us'), ' - ', TestNameDate) DisplayField FROM view_FullExamineeResults where positionid=" + rankid, constr);
            DataTable _dt = new DataTable();
             _da.Fill(_dt);
             ViewBag.selectionlist = ToSelectList(_dt, "ActualTestID", "DisplayField");
            return PartialView("~/Views/ReportMain/SelectionList.cshtml");
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

        MvcApplication3.Reports.XtraReport2 report = new MvcApplication3.Reports.XtraReport2();

        [HttpPost]
        public ActionResult DocumentViewerPartial()
        {
            //MvcApplication3.Reports.XtraReport2 report = new MvcApplication3.Reports.XtraReport2();
            DevExpress.XtraReports.UI.XRLabel lbl = ((DevExpress.XtraReports.UI.XRLabel)report.FindControl("xrlabel1", true));
           // lbl.Text = Session["amount"].ToString();
            //report.DataSource = "SELECT * FROM [SETS].[dbo].[view_FullExamineeResultsWithQuestions] ";// where actualtestid in(" + Session["selected"] + ") ORDER BY LastFirstMiddle ASC"; //WHERE ActualTestID IN ({0}) {1}ORDER BY LastFirstMiddle ASC";

            string constr = ConfigurationManager.ConnectionStrings["dropdownconn"].ToString();
            SqlConnection _con = new SqlConnection(constr);

            //SqlDataAdapter _da = new SqlDataAdapter("SELECT * FROM [SETS].[dbo].[view_FullExamineeResultsWithQuestions] where actualtestid in(" + Session["selected"] + ") ORDER BY LastFirstMiddle ASC", _con);
            SqlDataAdapter _da = new SqlDataAdapter("SELECT * FROM [SETS].[dbo].[view_FullExamineeResultsWithQuestions] where actualtestid in(" + Request["txtselected"].ToString() + ") ORDER BY LastFirstMiddle ASC", _con);
            
            DataSet ds = new DataSet();
            _da.Fill(ds);
            report.DataMember = ds.Tables[0].TableName;
            report.DataSource = ds;

            //return PartialView("~/Views/rptIndiTestResult/_DocumentViewer1Partial.cshtml", report);
            //return PartialView("~/Views/ReportMain/ReportFilters/_DocumentViewerPartial.cshtml", report);
            //return PartialView("_DocumentViewer1Partial", report);
            return PartialView("_DocumentViewer1Partial", report);
        }

        public ActionResult DocumentViewerPartialExport()
        {
            return DocumentViewerExtension.ExportTo(report, Request);
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
