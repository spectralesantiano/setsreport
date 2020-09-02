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
    public class dropdownController : Controller
    {

        [HttpGet]
        public ActionResult ddEntry()
        {
            string constr = ConfigurationManager.ConnectionStrings["dropdownconn"].ToString();
            SqlConnection _con = new SqlConnection(constr);
            SqlDataAdapter _da = new SqlDataAdapter("Select * From [view_ReportGroups]", constr);
            DataTable _dt = new DataTable();
            _da.Fill(_dt);
            ViewBag.ReportList = ToSelectList(_dt, "ObjectID", "Caption");

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

        [HttpPost]
        public ActionResult showReport(string ObjectID)
        {
            if (ObjectID != null)
            {
                return RedirectToAction("Index", ObjectID);

            }
            return View();
        }

        //
        // GET: /dropdown/

        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /dropdown/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /dropdown/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /dropdown/Create

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
        // GET: /dropdown/Edit/5

        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /dropdown/Edit/5

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
        // GET: /dropdown/Delete/5

        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /dropdown/Delete/5

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
