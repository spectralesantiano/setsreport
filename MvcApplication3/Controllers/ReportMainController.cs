﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SETSReport.Models;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;


namespace SETSReport.Controllers
{
    public class ReportMainController : Controller
    {
        internal enum DurationTypes { 
            Second,
            Minute,
            Hour,
            Day,
            Month,
            Year
        }

        public ActionResult showSelectionList(string fieldname,string fieldvalue)
        {

            DataTable _dt = TempData["SelecionLIst"] as DataTable; //ToSelectList(dt, "actualtestid", "displayfield");
            ViewBag.selectionlist = ToSelectList(_dt, fieldvalue, fieldname); 
            return PartialView("~/Views/ReportMain/SelectionList.cshtml"); 
        }

        [ChildActionOnly]
        public ActionResult ShowRptList()
        {
            string constr = ConfigurationManager.ConnectionStrings["dbconn"].ToString();
            SqlConnection _con = new SqlConnection(constr);
            //SqlDataAdapter _da = new SqlDataAdapter("Select * From [view_ReportGroups]", constr);
            DataTable _dt = new DataTable();
            //_da.Fill(_dt);
            //ViewBag.ReportList = ToSelectList(_dt, "ObjectID", "Caption");

            SqlDataAdapter _da = new SqlDataAdapter("Select GroupName, ObjectID,Caption From [view_ReportGroups] where RowType ='REPORT' order by groupsortcode asc, SortCode ASC", constr);
            _dt.Clear();
            _dt.Columns.Clear();
            _da.Fill(_dt);
            ViewBag.rptlistdt = _dt;
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
        public ActionResult Index(string id)
        {
            List<SelectListItem> dummylist = new List<SelectListItem>();
            dummylist.Add(new SelectListItem()
            {
                Text = "- No Data -",
                Value = ""
            });

            ViewBag.selectionlist = new SelectList(dummylist, "Value", "Text");
            ViewBag.logopath = Util.GetReportLogoPath().Replace("~", ""); //string "~" are not processed inside <img src>

            //--- code to check referer
            //string frURI = ConfigurationManager.AppSettings["SetRefererURI"] == null ? "" : ConfigurationManager.AppSettings["SetRefererURI"].ToString();
            //string referURI; //= Request.UrlReferrer.AbsoluteUri == null ? "" : Request.UrlReferrer.AbsoluteUri.ToString();
            
            //if (Request.UrlReferrer == null)
            //{ 
            //    referURI = "";
            //}
            //else {
            //    referURI = Request.UrlReferrer.AbsoluteUri.ToString();
            //}

            //if (frURI != "")
            //{
            //    if (referURI.Contains(frURI))
            //    {
            //        return View();
            //    }
            //    else
            //    {
            //       return View("noReferer");
            //    }
            //}
            //else
            //{
            //    return View();
            //}

            if (ConfigurationManager.AppSettings["CheckParam"] == "1")
            {
                string constr = ConfigurationManager.ConnectionStrings["dbconn"].ToString();
                SqlConnection _con = new SqlConnection(constr);
                //SqlDataAdapter _da = new SqlDataAdapter("Select * From [view_ReportGroups]", constr);
                DataTable _dt = new DataTable();
                //_da.Fill(_dt);
                //ViewBag.ReportList = ToSelectList(_dt, "ObjectID", "Caption");

                SqlDataAdapter _da = new SqlDataAdapter("Select *,getdate() as serverDate from tblWebSession where UniqueID='" + id + "' and IPAddress ='" + GetIp() + "'", constr);
                //_dt.Clear();
                //_dt.Columns.Clear();
                _da.Fill(_dt);

                if (_dt.Rows.Count > 0 ){

                            DateTime sdate = (DateTime)_dt.Rows[0]["serverDate"];
                            DateTime logdate = (DateTime)_dt.Rows[0]["DateLoggedIn"];
                            int validityt = Convert.ToInt32(_dt.Rows[0]["ValidityType"]);
                            DateTime newdate;

                            switch (validityt)
                            {
                                case 0:
                                        newdate = logdate.AddSeconds((int)_dt.Rows[0]["Validity"]); break;
                                case 1:
                                        newdate = logdate.AddMinutes((int)_dt.Rows[0]["Validity"]); break;
                                case 2:
                                        newdate = logdate.AddHours((int)_dt.Rows[0]["Validity"]); break;
                                case 3:
                                        newdate = logdate.AddDays((int)_dt.Rows[0]["Validity"]); break;
                                case 4:
                                        newdate = logdate.AddMonths((int)_dt.Rows[0]["Validity"]); break;
                                case 5:
                                        newdate = logdate.AddYears((int)_dt.Rows[0]["Validity"]); break;
                                default: newdate = sdate; break;
                            }

                            if (newdate < sdate)
                            {
                                return View("noReferer");
                            }
                            else
                            {
                                //Response.Write(GetIp());
                                return View();
                            }
                
                        }
                        else
                        {
                            return View("noReferer");
                        }
                }
                else
                {
                   return View();
                }
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

        public string GetIp()  
            {  
            string ip = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];  
            if (string.IsNullOrEmpty(ip))  
            {  
               ip = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];  
            }  
            return ip;  
            }  


    }
}
