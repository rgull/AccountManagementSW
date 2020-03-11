using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmartAdminMvc.Areas.Admin.Models
{
    public class Pages
    {
        public int id { get; set; }
        public int titleid { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public bool status { get; set; }
        public string metaTitle { get; set; }
        public string metaDescription { get; set; }
    }
}