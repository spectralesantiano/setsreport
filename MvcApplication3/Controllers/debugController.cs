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

namespace SETSReport.Controllers
{
    public class debugController : Controller
    {
        //
        // GET: /debug/

        public ActionResult Index()
        {
            //return View();
            return PartialView("_DocumentViewer1Partial", MainReport);
        }

        //
        // GET: /debug/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /debug/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /debug/Create

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
        // GET: /debug/Edit/5

        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /debug/Edit/5

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
        // GET: /debug/Delete/5

        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /debug/Delete/5

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

        SETSReport.Reports.debugrpt MainReport = new SETSReport.Reports.debugrpt();

        [HttpPost]
        public ActionResult DocumentViewerPartial()
        {
            return PartialView("_DocumentViewer1Partial", MainReport);
        }


    }
}
