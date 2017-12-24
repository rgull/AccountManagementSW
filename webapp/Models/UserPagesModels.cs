using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmartAdminMvc.Models
{
    public class UserPagesModels
    {
            public int id { get; set; }
            public int titleid { get; set; }
            public string name { get; set; }
            public string description { get; set; }
            public bool status { get; set; }
            public List<PagesList> ListOfPages { get; set; }
    }

    public class PagesList
    {
        public int id { get; set; }
        public string name { get; set; }
    }
}