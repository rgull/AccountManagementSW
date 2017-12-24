using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SmartAdminMvc.Models
{
    public class CompanyParameterModel
    {
    }

    public class ViewCompanyParameter
    {
        public int Id { get; set; }
        public string name { get; set; }
        public string Categoryname { get; set; }
        public int budgetTypeId { get; set; }
        public int parentId { get; set; }
        public string BudgetTypeName { get; set; }
        public DateTime createDate { get; set; }
        public Boolean isActive { get; set; }
    }

    public class CompanyParameter
    {
        public int Id { get; set; }
        public string name { get; set; }
        public int budgetTypeId { get; set; }
        public int parentId { get; set; }
        public DateTime createDate { get; set; }
        public Boolean isActive { get; set; }
        public List<SelectListItem> BudgetTypeList { get; set; }
        public List<SelectListItem> MaincategoryList { get; set; }
        public List<SelectListItem> Parameterslist { get; set; }
    }
}