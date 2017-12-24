using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SmartAdminMvc.Models
{
    public class CompanyReportModel
    {
        public int categoryId { get; set; }
        public int BudgetTypeId { get; set; }
        public int CarCounts { get; set; }
        public string CategoryName { get; set; }
        public List<CategoryParalist> CategoryParameterslist { get; set; }
        public decimal BudgetAmountTotal { get; set; }
        public decimal RealBudgetTotal { get; set; }
        public decimal TotalDiscount { get; set; }
        //public List<weeklyBugetList> weekAmountlist { get; set; }
        public List<weeklyListofbudget> weekAmountlist { get; set; }
        public List<weeklyListofbudget> IncomVScostofsalelist { get; set; }
        public List<weeklyListofbudget> GrossProfitweekly { get; set; }
        public List<weeklycarcounts> weeklycarcount { get; set; }
        public DateTime BudgetDate { get; set; }
    }
    public class CarcountModel
    {
        public int Id { get; set; }
        public int CarCounts { get; set; }
        public DateTime Date { get; set; }

    }

    public class YearlyBudgetOld1
    {
        public int categoryId { get; set; }
        public int budgettypeId { get; set; }
        public string CategoryName { get; set; }
        public decimal BudgetAmountTotal { get; set; }
        public decimal RealBudgetTotal { get; set; }
        public DateTime BudgetDate { get; set; }
    }

    public class YearlyReportModel
    {
        public List<YearlyItem> YearlyItems;
        public int CarCount;
        public decimal Income;
        public decimal AverageTicketValue;
    }

    //RGK 24-12-17
    public class KeyPointIndicatorModel
    {
        public List<KeyPointIndicators> KeyPointIndicators;
        public int CarCount;
        public decimal Income;
        public decimal AverageTicketValue;
    }

    public class YearlyItem
    {
        public int keyPointid { get; set; }
        public string groupName { get; set; }
        public decimal percentage { get; set; }
        public decimal KeyPointTotal { get; set; }
        public decimal Budget1TotalIncome { get; set; }
        public decimal Budget1GrossIncome { get; set; }
        public Boolean isnetProfitKey { get; set; }
        public Boolean isBusinessDevKey { get; set; }
    }
    public class weeklyBugetList
    {
        public int weekNo { get; set; }
        public decimal weeklyBudgetAmount { get; set; }
        public decimal weeklyRealBudget { get; set; }

    }

    public class weeklyListofbudget
    {
        public decimal mybugetweek1 { get; set; }
        [DisplayFormat(DataFormatString = "{0:F2}", ApplyFormatInEditMode = true)]
        public decimal? bugetLineweek1 { get; set; }
        public decimal mybugetweek2 { get; set; }
        [DisplayFormat(DataFormatString = "{0:F2}", ApplyFormatInEditMode = true)]
        public decimal? bugetLineweek2 { get; set; }
        public decimal mybugetweek3 { get; set; }
        [DisplayFormat(DataFormatString = "{0:F2}", ApplyFormatInEditMode = true)]
        public decimal? bugetLineweek3 { get; set; }
        public decimal mybugetweek4 { get; set; }
        [DisplayFormat(DataFormatString = "{0:F2}", ApplyFormatInEditMode = true)]
        public decimal? bugetLineweek4 { get; set; }
        public decimal mybugetweek5 { get; set; }
        [DisplayFormat(DataFormatString = "{0:F2}", ApplyFormatInEditMode = true)]
        public decimal? bugetLineweek5 { get; set; }
        public decimal mybugetweek6 { get; set; }
        [DisplayFormat(DataFormatString = "{0:F2}", ApplyFormatInEditMode = true)]
        public decimal? bugetLineweek6 { get; set; }
        public decimal TotalmybugetofMonth { get; set; }
        [DisplayFormat(DataFormatString = "{0:F2}", ApplyFormatInEditMode = true)]
        public decimal? TotalbugetLineofMonth { get; set; }
        public decimal? percentage1 { get; set; }
        public decimal? percentage2 { get; set; }
        public decimal? percentage3 { get; set; }
        public decimal? percentage4 { get; set; }
        public decimal? percentage5 { get; set; }
        public decimal? percentage6 { get; set; }
        public decimal? TotalpercentageofMonth { get; set; }
        public int BudgetTypeId { get; set; }
        public int totalweekdays { get; set; }

    }

    public class weeklycarcounts
    {
        public int carsinWeek_1 { get; set; }
        public int carsinWeek_2 { get; set; }
        public int carsinWeek_3 { get; set; }
        public int carsinWeek_4 { get; set; }
        public int carsinWeek_5 { get; set; }
        public int carsinWeek_6 { get; set; }
        public int totalweekdays { get; set; }
    }
    public class reportList
    {
        public int categoryId { get; set; }
        public int BudgetTypeId { get; set; }
        public string CategoryName { get; set; }
        public decimal BudgetAmountTotal { get; set; }
        public decimal RealBudgetTotal { get; set; }
        public DateTime BudgetDate { get; set; }

    }

    public class CategoryParalist
    {
        public int id { get; set; }
        public string paraName { get; set; }
        public decimal paraBudgetAmount { get; set; }
        public decimal paraRealBudget { get; set; }
        public List<weeklyParaListofbudget> weeklyParaList { get; set; }

    }
    public class weeklyParaListofbudget
    {
        public decimal mybugetweek1 { get; set; }
        [DisplayFormat(DataFormatString = "{0:F2}", ApplyFormatInEditMode = true)]
        public decimal? bugetLineweek1 { get; set; }
        public decimal mybugetweek2 { get; set; }
        [DisplayFormat(DataFormatString = "{0:F2}", ApplyFormatInEditMode = true)]
        public decimal? bugetLineweek2 { get; set; }
        public decimal mybugetweek3 { get; set; }
        [DisplayFormat(DataFormatString = "{0:F2}", ApplyFormatInEditMode = true)]
        public decimal? bugetLineweek3 { get; set; }
        public decimal mybugetweek4 { get; set; }
        [DisplayFormat(DataFormatString = "{0:F2}", ApplyFormatInEditMode = true)]
        public decimal? bugetLineweek4 { get; set; }
        public decimal mybugetweek5 { get; set; }
        [DisplayFormat(DataFormatString = "{0:F2}", ApplyFormatInEditMode = true)]
        public decimal? bugetLineweek5 { get; set; }
        public decimal mybugetweek6 { get; set; }
        [DisplayFormat(DataFormatString = "{0:F2}", ApplyFormatInEditMode = true)]
        public decimal? bugetLineweek6 { get; set; }
        public decimal TotalmybugetMonth { get; set; }
        [DisplayFormat(DataFormatString = "{0:F2}", ApplyFormatInEditMode = true)]
        public decimal? TotalbugetLineMonth { get; set; }
        public decimal? percentage1 { get; set; }
        public decimal? percentage2 { get; set; }
        public decimal? percentage3 { get; set; }
        public decimal? percentage4 { get; set; }
        public decimal? percentage5 { get; set; }
        public decimal? percentage6 { get; set; }
        public decimal? TotalpercentageMonth { get; set; }
        public int BudgetTypeId { get; set; }
        public int totalweekdays { get; set; }

    }
    public class todisplayreportList
    {
        public List<CompanyReportModel> CompanyReportModel { get; set; }
    }

    public class KeyPointIndicators
    {
        public int keyPointid { get; set; }
        public string groupName { get; set; }
        public decimal percentage { get; set; }
        public decimal KeyPointTotal { get; set; }
        public decimal Budget1TotalIncome { get; set; }
        public decimal Budget1GrossIncome { get; set; }
        public Boolean isnetProfitKey { get; set; }
        public Boolean isBusinessDevKey { get; set; }
    }
}