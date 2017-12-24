using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmartAdminMvc.Areas.Admin.Models
{
    public class CompanyModel
    {
    }
    public class addCompany
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
        public List<CategoryValueselcet> CategoryValueSelected { get; set; }
        public List<CategorywithtypeList> CategorywithList { get; set; }
        public Boolean isActive { get; set; }
        public Boolean isDeleted { get; set; }
    }
    public class EditCompany
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
        public List<CategoryValueselcet> CategoryValueSelected { get; set; }
        public List<CategorywithtypeList> CategorywithList { get; set; }
        public Boolean isActive { get; set; }
        public Boolean isDeleted { get; set; }
    }

    public class CategoryValueselcet
    {
        public int id { get; set; }
        public bool selected { get; set; }
        public string name { get; set; }

    }
    public class CategorywithtypeList
    {
        public int budgetTypeId { get; set; }
        public string budgetTypeName { get; set; }
        public int categoryId { get; set; }
        public string categoryName { get; set; }
        public List<CategoryValueselcet> CategoryValueList { get; set; }
    }
}