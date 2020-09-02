using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcApplication3.Reports;
using DevExpress.Web.Mvc;

namespace MvcApplication3.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";

            return View();
           
        }

        public ActionResult View1()
        {
            ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";

            return View();

        }

        public ActionResult Partial1()
        {
            ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";

            return View();

        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }


        //below codes were autoadded when adding documentviewer -------------
        MvcApplication3.Reports.XtraReport2 report = new MvcApplication3.Reports.XtraReport2();

        public ActionResult DocumentViewerPartial()
        {
            return PartialView("_DocumentViewerPartial", report);
        }

        public ActionResult DocumentViewerPartialExport()
        {
            return DocumentViewerExtension.ExportTo(report, Request);
        }
        ///----------------
    }
}
