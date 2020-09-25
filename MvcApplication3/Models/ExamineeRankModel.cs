using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace SETSReport.Models
{
    public class ExamineeRankModel
    {
      
            public string PositionID { get; set; }
            public string Name { get; set; }
       

            public string SubjectID { get; set; }
            public string SubjectName { get; set; }
        

        //public IEnumerable<Examinee> Examinee { get; set; }  
        public IEnumerable<SelectListItem> Ranks { get; set; }
        public IEnumerable<SelectListItem> Subjects { get; set; }  

    }

   
}