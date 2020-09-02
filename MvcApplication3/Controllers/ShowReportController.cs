using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcApplication3.Controllers
{
    public class ShowReportController : Controller
    {
        //
        // GET: /ShowReport/

        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /ShowReport/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /ShowReport/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /ShowReport/Create

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
        // GET: /ShowReport/Edit/5

        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /ShowReport/Edit/5

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
        // GET: /ShowReport/Delete/5

        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /ShowReport/Delete/5

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
