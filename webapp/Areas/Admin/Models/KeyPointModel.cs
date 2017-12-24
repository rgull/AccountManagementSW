using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmartAdminMvc.Areas.Admin.Models
{
    public class KeyPointModel
    {
        public int id { get; set; }
        public string name { get; set; }
        public int companyId { get; set; }
        public int categoryId { get; set; }
        public decimal percentage { get; set; }
    }
    public class addKeyPoint
    {
        public int id { get; set; }
        public string name { get; set; }
        public decimal percentage { get; set; }
        public int companyId { get; set; }
        public DateTime createDate { get; set; }
        public DateTime updateDate { get; set; }
        public Boolean isActive { get; set; }
        public Boolean isnetProfitKey { get; set; }
        public Boolean isBusinessDevKey { get; set; }
        public List<CategoryValueselcet> CategoryValueSelected { get; set; }
        public List<CategorywithtypeList> CategorywithList { get; set; }
    }
}