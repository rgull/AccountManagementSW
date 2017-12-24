using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SmartAdminMvc.Models
{
    public class CompanyAccountModel
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

    public class AddCompnayBudgetLine
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
        public List<BudgetTypeList> BudgetTypeList { get; set; }
        public List<SelectListItem> MaincategoryList { get; set; }
        public List<SelectListItem> categoryValueList { get; set; }
        public List<ReportTypeList> ReportTypeList { get; set; }
    }

    public class BudgetTypeList
    {
        public int id { get; set; }
        public string name { get; set; }
        public bool Budgetselected { get; set; }

    }
    public class CategoryList
    {
        public int id { get; set; }
        public string name { get; set; }

    }
    public class ReportTypeList
    {
        public int id { get; set; }
        public string name { get; set; }
        public bool Reportselected { get; set; }

    }
    public class Mybudget
    {
        //public int budgetTypeId { get; set; }
        //public string budgetTypeName { get; set; }
        //public int categoryId { get; set; }
        //public string categoryName { get; set; }
        //public List<MyParameterList> MyValueParameterList { get; set; }
        public DateTime date { get; set; }
        public int carCountId { get; set; }
        public int carCounts { get; set; }
        public int companyId { get; set; }
        public string selectedDate { get; set; }
        public int budgetTypeId { get; set; }
        public int SelectedBudgetTypeId { get; set; }
        public List<MybudgetFullList> MyValuebudgetFullList { get; set; }
        public List<ReportTypeList> ReportTypeList { get; set; }
        public List<BudgetTypeList> BudgetTypeList { get; set; }

    }
    public class MyParameterList
    {
        public int id { get; set; }
        public int MyBudgetid { get; set; }
        public string name { get; set; }
        public Decimal budget { get; set; }
        public int reportType { get; set; }
        public int SelectedreportTypeId { get; set; }
        public bool isAddOnceInMonth { get; set; }
        public int isUpdated { get; set; }
        public int budgetTypeId { get; set; }
        public bool IsDisabled { get; set; }



    }
    public class MybudgetFullList
    {
        public int budgetTypeId { get; set; }
        public string budgetTypeName { get; set; }
        public int categoryId { get; set; }
        public string categoryName { get; set; }
        public List<MyParameterList> MyValueParameterList { get; set; }
        public DateTime date { get; set; }

    }

    //public class Mybudget
    //{
    //    public List<myBudgetTypeList> myBudgetTypeList { get; set; }
    //    public DateTime date { get; set; }
    //}
    //public class myBudgetTypeList
    //{
    //    public int id { get; set; }
    //    public string name { get; set; }
    //    public List<MyCategoryList> MyCategoryList { get; set; }
    //}
    //public class MyCategoryList
    //{
    //    public int id { get; set; }
    //    public string name { get; set; }
    //    public List<MyParameterList> MyParameterList { get; set; }

    //}
   

}