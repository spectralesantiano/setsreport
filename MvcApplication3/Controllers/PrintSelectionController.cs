using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcApplication3.Controllers
{
    public class PrintSelectionController : Controller
    {
        //
        // GET: /PrintSelection/

        public ActionResult Index()
        {
            string rpts = "jkljkljk";
            ViewBag.rptsname = rpts;

            var rptlist = new List<MvcApplication3.Models.PrintSelection>
            {
                new MvcApplication3.Models.PrintSelection() { rptID = 1, rptName="ako"},
                new MvcApplication3.Models.PrintSelection() { rptID = 2, rptName="ikaw"}
            };

            ViewData["myreports"] = rptlist;
            
           
            
            return View(); //new MvcApplication3.Models.PrintSelection());
        }


        //
        // GET: /PrintSelection/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /PrintSelection/Create

        public ActionResult Create()
        {

            return View();
        }

        //
        // POST: /PrintSelection/Create

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
        // GET: /PrintSelection/Edit/5

        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /PrintSelection/Edit/5

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
        // GET: /PrintSelection/Delete/5

        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /PrintSelection/Delete/5

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
