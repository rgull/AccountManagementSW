using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmartAdminMvc.Areas.Admin.Models
{
    public class ReportTypeModel
    {
    }
    public class AddReportType
    {
        public int id { get; set; }
        public string name { get; set; }
        public int rank { get; set; }
        public DateTime createDate { get; set; }
        public Boolean isActive { get; set; }
    }
    public class EditReportType
    {
        public int id { get; set; }
        public string name { get; set; }
        public int rank { get; set; }
        public DateTime createDate { get; set; }
        public Boolean isActive { get; set; }
    }
}