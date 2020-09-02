using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcApplication3.Models
{
    public class PrintSelection
    {
        [Key]
        public int rptID { get; set; }
        public string rptName { get; set; }

        //public class selectedrptID
        public IEnumerable<SelectListItem> selectedrptID
        {
            get;
            set;
        }    
        
    }

}