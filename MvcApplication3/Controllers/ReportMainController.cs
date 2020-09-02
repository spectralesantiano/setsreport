using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcApplication3.Models;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;


namespace MvcApplication3.Controllers
{
    public class ReportMainController : Controller
    {

        [ChildActionOnly]
        public ActionResult ShowRptList()
        {
            string constr = ConfigurationManager.ConnectionStrings["dropdownconn"].ToString();
            SqlConnection _con = new SqlConnection(constr);
            SqlDataAdapter _da = new SqlDataAdapter("Select * From [view_ReportGroups]", constr);
            DataTable _dt = new DataTable();
            _da.Fill(_dt);
            ViewBag.ReportList = ToSelectList(_dt, "ObjectID", "Caption");

            return PartialView();
        }

        public ActionResult ShowRptFilter(string ObjectID)
        {
            if (ObjectID != null)
            {
                try
                {
                    //return PartialView(ObjectID); //("ShowRptFilter"); // to be replace 
                    
                    //var ee = "ReportFilters/" + ObjectID.Replace("'","");
                    var ee = ObjectID.Replace("'", ""); //+"/viewFilter";
                    //if (ViewExists(ee))
                    //if (ViewExists("/" + ee + "/viewFilter/"))
                    //{
                        //return PartialView(ee); //rptIndiTestResult");
                    return RedirectToAction("viewFilter", ee);
                   // return RedirectToAction("Index", ee);
                        //{
                        //    return PartialView("ReportFilters/NoReportFilter");
                        //}
                    //}
                    //else
                    //{
                    //    return PartialView("ReportFilters/NoReportFilter");
                    //}
                    
                }
                catch
                {
                    return PartialView("ReportFilters/NoReportFilter");
                }
            }
            return View();
           
        }

        public ActionResult ShowNoRptFilter()
        {
            return PartialView("ReportFilters/NoReportFilter");
        }

        private bool ViewExists(string name)
        {
            ViewEngineResult result = ViewEngines.Engines.FindView(ControllerContext, name, null);
            return (result.View != null);
        }
        
        //
        // GET: /ReportMain/

        public ActionResult Index()
        {
            return View();
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

        //
        // GET: /ReportMain/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /ReportMain/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /ReportMain/Create

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
        // GET: /ReportMain/Edit/5

        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /ReportMain/Edit/5

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
        // GET: /ReportMain/Delete/5

        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /ReportMain/Delete/5

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

    }
}
