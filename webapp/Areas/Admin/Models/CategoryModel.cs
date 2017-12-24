using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SmartAdminMvc.Areas.Admin.Models
{
    public class CategoryModel
    {
    }

    public class Category
    {
        public int Id { get; set; }
        [AllowHtml]
        [RegularExpression(@"[^<>]*", ErrorMessage = "Invalid entry")]
        public string name { get; set; }
        public int budgetTypeId { get; set; }
        public int parentId { get; set; }
        public DateTime createDate { get; set; }
        public DateTime updateDate { get; set; }
        public Boolean isActive { get; set; }
        public List<SelectListItem> BudgetTypeList { get; set; }
    }

    public class ViewCategory
    {
        public int Id { get; set; }
        public string name { get; set; }
        public string Categoryname { get; set; }
        public int budgetTypeId { get; set; }
        public int parentId { get; set; }
        public string BudgetTypeName { get; set; }
        public DateTime createDate { get; set; }
        public DateTime updateDate { get; set; }
        public Boolean isActive { get; set; }
    }

    public class CategoryValue
    {
        public int Id { get; set; }
        public string name { get; set; }
        public int budgetTypeId { get; set; }
        public int parentId { get; set; }
        public DateTime createDate { get; set; }
        public DateTime updateDate { get; set; }
        public Boolean isActive { get; set; }
        public List<SelectListItem> BudgetTypeList { get; set; }
        public List<SelectListItem> MaincategoryList { get; set; }
    }



}