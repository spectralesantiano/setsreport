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
                 // if (tmpval == System.DBNull.Value)
                  if (tmpval == null)
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


          public static bool isSessionValid()
          {
                  string constr = ConfigurationManager.ConnectionStrings["dbconn"].ToString();
                    SqlConnection _con = new SqlConnection(constr);
                    DataTable _dt = new DataTable();

                    SqlDataAdapter _da = new SqlDataAdapter("Select *,getdate() as serverDate from tblWebSession where UniqueID='" + GlobalVar.UniqueID + "' and IPAddress ='" + GlobalVar.UserIP + "'", constr);
              
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
                                    return false;
                                }
                                else
                                {
                                    return true;
                                }
                
                        }
                        else
                        {
                            return false;
                        }
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

          public static string GetMEDIA_PATH_ONLINE()
          {
              string path = "";

              path = ConfigurationManager.AppSettings["MediaFolderNameOnline"];
              char last = path[path.Length - 1];
              if (!last.Equals( "/") )
              {
                  path = path + "/";
              }
              return path;
          }

    }
}