using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SmartAdminMvc.Areas.Admin.Models
{
    public class AdminModel
    {
    }

    //public class tblAdmin
    //{
    //    public int id { get; set; }
    //    public string name { get; set; }
    //    public string email { get; set; }
    //    public string password { get; set; }
    //    public DateTime createDate { get; set; }
    //    public DateTime updateDate { get; set; }
    //    public Boolean isActive { get; set; }           
    //}

    /// <summary>
    /// Admin Login check get set method
    /// </summary>
    public class adminLoginModel
    {
        public string email { get; set; }
        public string password { get; set; }

    }

    /// <summary>
    /// get set method for admin user.
    /// </summary>
    public class Addadminuser
    {
        public int id { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public DateTime createDate { get; set; }
        public DateTime updateDate { get; set; }
        public Boolean isActive { get; set; }

    }

    /// <summary>
    /// get set method for admin user.
    /// </summary>
    public class Editadminuser
    {
        public int id { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public DateTime createDate { get; set; }
        public DateTime updateDate { get; set; }
        public Boolean isActive { get; set; }

    }
}