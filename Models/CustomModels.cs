using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ScholarPortal.Models
{
    public class CustomModels
    {

    }

    public class ErrorLogSessionData
    {
        public decimal LoginID { get; set; }
        public string PageUrl { get; set; }
        public string UserIP { get; set; }
        public string Browser { get; set; }
    }
}