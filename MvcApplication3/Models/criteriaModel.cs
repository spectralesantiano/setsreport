﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SETSReport.Models
{
    public class criteriaModel
    {
        public string key { get; set; }
        public object value { get; set; }  
    }

    public class listCriteria
    {
        public List<criteriaModel> criteriaModel { get; set; }
    }
}