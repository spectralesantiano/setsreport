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
    public class rptPrintTestCertificateController : Controller
    {
        //
        // GET: /rptPrintTestCertificate/

        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /rptPrintTestCertificate/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /rptPrintTestCertificate/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /rptPrintTestCertificate/Create

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
        // GET: /rptPrintTestCertificate/Edit/5

        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /rptPrintTestCertificate/Edit/5

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
        // GET: /rptPrintTestCertificate/Delete/5

        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /rptPrintTestCertificate/Delete/5

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

        SETSReport.Reports.rptPrintTestCertificate MainReport = new SETSReport.Reports.rptPrintTestCertificate();


        [HttpPost]
        public ActionResult DocumentViewerPartial()
        {
             string selectedIDs = Request["txtselected"].ToString();
             string conditions = "";

          string sql = String.Format("SELECT FullName, RankName, FORMAT(DateTaken, 'MMMM dd, yyyy', 'en-us') DateTaken, TestName, UserScore, TotalScore, " +
                                    "CONCAT('with a rating of ', CONVERT(DECIMAL(10,2), CAST((CAST(UserScore AS FLOAT) / TotalScore) * 100 AS FLOAT)), '%') ScoreRating " +
                                "FROM view_FullExamineeResults WHERE ActualTestID IN ({0}) " +
                                "ORDER BY Fullname ASC", selectedIDs);

        string constr = ConfigurationManager.ConnectionStrings["dbconn"].ToString();
        SqlConnection _con = new SqlConnection(constr);
        SqlDataAdapter _da = new SqlDataAdapter(sql, _con);
            
        DataSet ds = new DataSet();
        _da.Fill(ds);
        MainReport.DataMember = ds.Tables[0].TableName; 
        MainReport.DataSource = ds;
   
        MainReport.txtCompanyName.Text = Util.GetConfig("COMPANY_NAME");
        MainReport.txtRptTitleLong.Text = Util.GetConfig("APP_NAME").ToUpper();
        MainReport.txtRptTitle.Text = "(" + Util.GetConfig("APP_ABBRV") + ")";

        if (Request["Signatory"] != null && Request["Signatory"] != "")
        {
            MainReport.txtSigName.Text = Request["Signatory"].Split(new string[] { " - " }, StringSplitOptions.None)[0];
            MainReport.txtJobTitle.Text = Request["Signatory"].Split(new string[] { " - " }, StringSplitOptions.None)[1];
        }

        MainReport.pbLogo.ImageUrl = Util.GetReportLogoPath();

        MainReport.Position.DataBindings.Add("Text", null, "RankName");
        MainReport.FullName.DataBindings.Add("Text", null, "Fullname");
        MainReport.DateTaken.DataBindings.Add("Html", null, "DateTaken");
        MainReport.TestName.DataBindings.Add("Text", null, "TestName");
        MainReport.Score.DataBindings.Add("Text", null, "ScoreRating");

        MainReport.DateTaken.EvaluateBinding += FormatDate;

            return PartialView("_DocumentViewer1Partial", MainReport);
        }

        public ActionResult viewFilter()
        {
            string constr = ConfigurationManager.ConnectionStrings["dbconn"].ToString();
            SqlConnection _con = new SqlConnection(constr);
            DataTable _dt = new DataTable();

            SqlDataAdapter _da = new SqlDataAdapter(GlobalVar.CompanyNameQuery, constr);
            _dt.Clear();
            _dt.Columns.Clear();
            _da.Fill(_dt);
            ViewBag.CompanyName = Util.ToSelectList(_dt, "CompanyName", "CompanyName");

            _da = new SqlDataAdapter(GlobalVar.SignatoryQuery, constr);
            _dt.Clear();
            _dt.Columns.Clear();
            _da.Fill(_dt);
            ViewBag.Signatory = Util.ToSelectList(_dt, "DisplayValue", "DisplayValue");

            var list = new SelectList(new[] 
            {
                new { cbeSortBy = "LName", Text = "Name" },
                new { cbeSortBy = "DateTaken", Text = "Date" },
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

            String sql = "SELECT *, 0 IsSelected, CONCAT(LastFirstMiddle, ' - ', FORMAT(DateTaken, 'dd-MMM-yyyy hh:mm tt', 'en-us'), ' - ', TestNameDate) DisplayField FROM view_FullExamineeResults " + filterCriteria;

            sql += " order by " + sortbyname + " " + sortby;

            SqlDataAdapter _da = new SqlDataAdapter(sql, constr);
            DataTable _dt = new DataTable();
            _da.Fill(_dt);

            //// ----- required : to send the selectionlist ----///////////////
            TempData["SelecionLIst"] = _dt; //get the value in the reportmain/showSelectionlist , should be datatable
            return RedirectToAction("showSelectionList", "ReportMain", new { fieldname = "DisplayField", fieldvalue = "ActualTestID" });
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
                        case "CompanyName":
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

         private void FormatDate(Object sender,BindingEventArgs e ){
            try {
                if(e.Value != null) {
                    e.Value = String.Format("<div style='font-size:25px;font-family:Times New Roman;'><center><i>has on <b>{0}</b></i></center></dib>", e.Value);
                }
            }
            catch(Exception ex ){
               
          }
         }


    }
}
