using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using DevExpress.DataAccess.Sql;
using DevExpress.DataAccess.ConnectionParameters;
using System.Web.Mvc;
using SETSReport.Controllers;

namespace SETSReport.Controllers
{
    public static class Util
    {
         // public bool IsNotEmpty(object val ) {
         //     return  val != DBNull.Value &&  val != null &&  !string.IsNullOrEmpty(val.ToString()) &&  !string.IsNullOrWhiteSpace(val.ToString());
         //}

          public static string GetConfig(string cCode)
          {
              string defaultvalue="";

              string constr = ConfigurationManager.ConnectionStrings["dbconn"].ToString();
              SqlConnection sqlcon = new SqlConnection(constr);

              try{
                  sqlcon.Open();
                   SqlCommand sqlcmd = new SqlCommand("SELECT [TextValue] FROM SETS.dbo.tblconfig  WHERE Code='" + cCode + "'", sqlcon);
                  object tmpval;
                  tmpval = sqlcmd.ExecuteScalar();
                  if (tmpval == System.DBNull.Value)
                  {
                    defaultvalue = "";
                  }
                  else{
                    defaultvalue = tmpval.ToString();
                  }
                  sqlcmd.Dispose();
                  sqlcon.Close();
              }
              catch (InvalidCastException e){
                  if( sqlcon.State != ConnectionState.Closed) {
                      sqlcon.Close();
                  }
              }
             
              return defaultvalue;
          }


          public static SelectList ToSelectList(DataTable table, string valueField, string textField)
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

          public static string GetReportLogoPath()
          {
              string path = "";


              path = "~/Content/images/" + ConfigurationManager.AppSettings["LogoFileName"];
              return path;
          }

          public static string GetMEDIA_PATH()
          {
              string path = "";


              path = "~/" + ConfigurationManager.AppSettings["MediaFolderName"];
              return path;
          }

    }
}