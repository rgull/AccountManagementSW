using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SmartAdminMvc.Areas.Admin.Models
{
    public class BudgetAmountModel
    {
    }
    public class adminCompanyAccountModel
    {
        public int id { get; set; }
        public int budgetTypeId { get; set; }
        public string budgetType { get; set; }
        public int categoryId { get; set; }
        public string category { get; set; }
        public int companyId { get; set; }
        public string company { get; set; }
        public DateTime date { get; set; }
        public int reportType { get; set; }
        public string reportName { get; set; }
        public decimal budget { get; set; }
        public DateTime createDate { get; set; }
        public DateTime updateDate { get; set; }
        public bool isActive { get; set; }
    }

    public class adminAddCompnayBudgetLine
    {
        public int id { get; set; }
        public int budgetTypeId { get; set; }
        public int SelectedBusgetTypeId { get; set; }
        public int categoryId { get; set; }
        public int categoryValueId { get; set; }
        public int companyId { get; set; }
        public DateTime date { get; set; }
        public string selectedDate { get; set; }
        public int reportType { get; set; }
        public int SelectedreportTypeId { get; set; }
        public decimal budget { get; set; }
        public DateTime createDate { get; set; }
        public DateTime updateDate { get; set; }
        public bool isActive { get; set; }
        public bool isAddOnceInMonth { get; set; }
        public List<adminBudgetTypeList> adminBudgetTypeList { get; set; }
        public List<SelectListItem> MaincategoryList { get; set; }
        public List<SelectListItem> categoryValueList { get; set; }
        public List<adminReportTypeList> adminReportTypeList { get; set; }
    }

    public class DefaultAddCompnayBudgetLine
    {
        public int id { get; set; }
        public int budgetTypeId { get; set; }
        public int SelectedBusgetTypeId { get; set; }
        public int categoryId { get; set; }
        public int categoryValueId { get; set; }
        public int companyId { get; set; }
        public DateTime date { get; set; }
        public string selectedDate { get; set; }
        public int reportType { get; set; }
        public int SelectedreportTypeId { get; set; }
        public decimal budget { get; set; }
        public DateTime createDate { get; set; }
        public DateTime updateDate { get; set; }
        public bool isActive { get; set; }
        public bool isAddOnceInMonth { get; set; }
        public List<adminBudgetTypeList> adminBudgetTypeList { get; set; }
        public List<SelectListItem> MaincategoryList { get; set; }
        public List<SelectListItem> categoryValueList { get; set; }
        public List<adminReportTypeList> adminReportTypeList { get; set; }
    }
    public class adminBudgetTypeList
    {
        public int id { get; set; }
        public string name { get; set; }
        public bool Budgetselected { get; set; }

    }
    public class adminCategoryList
    {
        public int id { get; set; }
        public string name { get; set; }

    }
    public class adminReportTypeList
    {
        public int id { get; set; }
        public string name { get; set; }
        public bool Reportselected { get; set; }

    }
}