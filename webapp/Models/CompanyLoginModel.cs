using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmartAdminMvc.Models
{
    public class CompanyLoginModel
    {
    }
    public class Company
    {
        public int id { get; set; }
        public string name { get; set; }
        public string emailId { get; set; }
        public string password { get; set; }
        public string contactName { get; set; }
        public string contactNo { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string country { get; set; }
        public string address { get; set; }
        public DateTime createDate { get; set; }
        public DateTime updateDate { get; set; }
        public Boolean isActive { get; set; }
        public Boolean isDeleted { get; set; }
    }

    public class CompanyLogin
    {
        public string emailId { get; set; }
        public string password { get; set; }
        public List<PagesList> ListOfPages { get; set; }
    }
}