using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcApplication3.Controllers
{
    public class Util
    {
          public bool IsNotEmpty(object val ) {
              return  val != DBNull.Value &&  val != null &&  !string.IsNullOrEmpty(val.ToString()) &&  !string.IsNullOrWhiteSpace(val.ToString());
         }
    }
}