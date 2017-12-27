using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SmartAdminMvc.Models;
using System.Web.Security;
using SmartAdminMvc.BL;
using System.Reflection;
using System.IO;
using System.Data.SqlClient;

namespace SmartAdminMvc.Controllers
{
    public class CompanyReportController : Controller
    {
        // GET: CompanyReport
        public ActionResult Index()
        {
            return View();
        }
        // [HttpGet]

        public ActionResult CompanyBudgetReport(string reportdate = "", string Month = "", string Year = "")
        {
            var model = new List<CompanyReportModel>();
            CompanyReportBL objreport = new CompanyReportBL();
            if (Session["CompanyUser"] != null)
            {
                //if (Request.IsAjaxRequest())
                //{
                if (Month != "" && Year != "")
                {
                    string dateforreport = Month + "/01/" + Year;
                    DateTime dat = DateTime.ParseExact(dateforreport.ToString(), "MM/dd/yyyy", null);
                    model = getData(dat);
                }
                if (reportdate != "")
                {
                    ViewBag.reportdate = reportdate;
                    DateTime dat = DateTime.ParseExact(reportdate.ToString(), "MM/dd/yyyy", null);
                    model = getData(dat);
                    // return PartialView("CompanyReportList", model);
                }
                // }
                //CompanyReportModel obj = new CompanyReportModel();                
                return View(model);
            }
            return RedirectToAction("Companylogin", "Companylogin");
        }

        //public ActionResult YearlyBudgetReport(string Year = "")
        //{
        //    var model = new List<YearlyReportModel>();
        //    CompanyReportBL objreport = new CompanyReportBL();
        //    if (Session["CompanyUser"] != null)
        //    {
        //        //if (Request.IsAjaxRequest())
        //        //{
        //        if (Year != "")
        //        {
        //            string dateforreport = "12/01/" + Year; // 12-01 is teken bcoz last week on yesr is needed in Sp
        //            DateTime dat = DateTime.ParseExact(dateforreport.ToString(), "MM/dd/yyyy", null);
        //            model = getYearlyData(dat);
        //        }
        //        // }
        //        //CompanyReportModel obj = new CompanyReportModel();                
        //        return View(model);
        //    }
        //    return RedirectToAction("Companylogin", "Companylogin");
        //}
        public ActionResult YearlyBudgetReport(string Year = "")
        {
            var model = new YearlyReportModel();
            model.YearlyItems = new List<YearlyItem>();
            CompanyReportBL objreport = new CompanyReportBL();
            if (Session["CompanyUser"] != null)
            {
                //if (Request.IsAjaxRequest())
                //{
                if (Year != "")
                {
                    string dateforreport = "01/01/" + Year; // 12-01 is teken bcoz last week on yesr is needed in Sp
                    DateTime dat = DateTime.ParseExact(dateforreport.ToString(), "MM/dd/yyyy", null);
                    model.YearlyItems = getKeyPointDataYearly(dat);
                    var yearlyBudget = getYearlyData(dat);
                    model.CarCount = GetyearlyCarCount(dat);
                    if (yearlyBudget != null)
                    {
                        model.Income = yearlyBudget.Where(yb=>yb.categoryId==3).Sum(x => x.RealBudgetTotal);
                        if (model.Income != 0)
                            model.AverageTicketValue = model.Income / model.CarCount;
                    }
                    
                }
                // }
                //CompanyReportModel obj = new CompanyReportModel();                
                return View(model);
            }
            return RedirectToAction("Companylogin", "Companylogin");
        }
        public List<CompanyReportModel> getData(DateTime reportdate)
        {
            DateTime repoerdate = reportdate;
            var report_List = new List<CompanyReportModel>();
            CompanyReportBL objreport = new CompanyReportBL();
            if (Session["CompanyUser"] != null)
            {
                int companyId = Convert.ToInt32(Session["CompanyId"].ToString());
                CompanyReportModel updatemodel = new CompanyReportModel();

                using (managementsoftwaredbEntities context = new managementsoftwaredbEntities())
                {
                    var qry = (from db in context.tblRealBudgets
                               join ct in context.tblCategories on db.categoryId equals ct.id
                               join mc in context.tblCategories on ct.parentId equals mc.id
                               join ex in context.tblExpectedBudgetLines on db.categoryId equals ex.categoryId
                               where db.companyId == companyId
                               where db.date.Month == repoerdate.Month
                               where db.date.Year == repoerdate.Year
                               select new reportList
                               {
                                   categoryId = mc.id,
                                   CategoryName = mc.name,
                                   BudgetTypeId = mc.budgetTypeId
                                   // RealBudgetTotal = db.budget,
                                   // BudgetAmountTotal = ex.budget,
                                   // BudgetDate = db.date

                               });
                    var obj = (from db in context.tblRealBudgets
                               join ct in context.tblCategories on db.categoryId equals ct.id
                               join mc in context.tblCategories on ct.parentId equals mc.id
                               join ex in context.tblExpectedBudgetLines on db.categoryId equals ex.categoryId
                               where db.companyId == companyId
                               where db.date.Month == repoerdate.Month
                               where db.date.Year == repoerdate.Year
                               select new reportList
                               {
                                   categoryId = mc.id,
                                   CategoryName = mc.name,
                                   BudgetTypeId = mc.budgetTypeId
                                   // RealBudgetTotal = db.budget,
                                   // BudgetAmountTotal = ex.budget,
                                   // BudgetDate = db.date

                               }).Distinct().OrderBy(x=>x.categoryId).ToList();

                    if (obj.Any())
                    {
                        double TOTALRealBudget = 0;
                        double TOTALBudgetAmount = 0;
                        decimal TotalDiscount = 0;
                        for (int i = 0; i < obj.Count; i++)
                        {
                            var Company_Report_item = new CompanyReportModel();
                            Company_Report_item.categoryId = obj[i].categoryId;
                            Company_Report_item.CategoryName = obj[i].CategoryName;
                            Company_Report_item.BudgetDate = obj[i].BudgetDate;
                            Company_Report_item.BudgetTypeId = obj[i].BudgetTypeId;
                            Company_Report_item.CarCounts = objreport.MyBudgetCarCount(Convert.ToInt32(Session["CompanyId"].ToString()), reportdate);
                            Company_Report_item.CategoryParameterslist = objreport.CategoryParameterslistfetch(obj[i].categoryId, reportdate, Convert.ToInt32(Session["CompanyId"].ToString()));
                            // Company_Report_item.weekAmountlist = objreport.weekAmountlistfetch(obj[i].categoryId, obj[i].BudgetDate, Convert.ToInt32(Session["CompanyId"].ToString()));
                            Company_Report_item.weekAmountlist = objreport.weekLybudgetlistfetch(obj[i].categoryId, reportdate, Convert.ToInt32(Session["CompanyId"].ToString()));
                            Company_Report_item.weeklycarcount = objreport.weekLycarcountlistfetch(reportdate, Convert.ToInt32(Session["CompanyId"].ToString()));
                            if (obj[i].categoryId == Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["BUDGET2EXPENSES"].ToString()))
                            {
                                Company_Report_item.IncomVScostofsalelist = objreport.IncomVScostofsalelistfetch(obj[i].categoryId, reportdate, Convert.ToInt32(Session["CompanyId"].ToString()));
                            }
                            if (obj[i].categoryId == Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["UTILITIESEXPENSES"].ToString()))
                            {
                                Company_Report_item.GrossProfitweekly = objreport.grosprofitlistfetch(obj[i].categoryId, Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["BUDGET3EXPENSES"].ToString()), Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["BUDGET2EXPENSES"].ToString()), reportdate, Convert.ToInt32(Session["CompanyId"].ToString()));
                            }
                            report_List.Add(Company_Report_item);
                        }


                        if (report_List != null && report_List.Count() > 0)
                        {

                            CompanyReportModel IncomeCategory = report_List.Where(x => x.categoryId == 3).FirstOrDefault();
                            if (IncomeCategory != null)
                            {
                                IncomeCategory.TotalDiscount = 0;
                                weeklyListofbudget weeklyIncomelist = null;
                                if (IncomeCategory.weekAmountlist != null && IncomeCategory.weekAmountlist.Count() > 0)
                                {
                                    weeklyIncomelist = IncomeCategory.weekAmountlist[0];
                                }

                                #region categoryparamlist
                                //get discount amount
                                CategoryParalist DiscountCat = IncomeCategory.CategoryParameterslist.Where(x => x.id == 6).FirstOrDefault();
                                if (DiscountCat != null)
                                {
                                    CategoryParalist IncomeCat = IncomeCategory.CategoryParameterslist.Where(x => x.id == 4).FirstOrDefault();

                                    if (IncomeCat != null && IncomeCat.weeklyParaList != null)
                                    {
                                        for (int i = 0; i < IncomeCat.weeklyParaList.Count(); i++)
                                        {
                                            if (DiscountCat.weeklyParaList != null && DiscountCat.weeklyParaList.Count() >= i)
                                            {
                                                weeklyParaListofbudget weeklyDiscount = DiscountCat.weeklyParaList[i];
                                                weeklyParaListofbudget weeklyincome = IncomeCat.weeklyParaList[i];
                                                weeklyincome.mybugetweek1 = weeklyincome.mybugetweek1 - Math.Abs(weeklyDiscount.mybugetweek1);
                                                if (weeklyincome.bugetLineweek1 != 0)
                                                    weeklyincome.percentage1 = (weeklyincome.mybugetweek1 - weeklyincome.bugetLineweek1) / weeklyincome.bugetLineweek1 * 100;
                                                weeklyincome.mybugetweek2 = weeklyincome.mybugetweek2 - Math.Abs(weeklyDiscount.mybugetweek2);
                                                if (weeklyincome.bugetLineweek2 != 0)
                                                    weeklyincome.percentage2 = (weeklyincome.mybugetweek2 - weeklyincome.bugetLineweek2) / weeklyincome.bugetLineweek2 * 100;
                                                weeklyincome.mybugetweek3 = weeklyincome.mybugetweek3 - Math.Abs(weeklyDiscount.mybugetweek3);
                                                if (weeklyincome.bugetLineweek3 != 0)
                                                    weeklyincome.percentage3 = (weeklyincome.mybugetweek3 - weeklyincome.bugetLineweek3) / weeklyincome.bugetLineweek3 * 100;
                                                weeklyincome.mybugetweek4 = weeklyincome.mybugetweek4 - Math.Abs(weeklyDiscount.mybugetweek4);
                                                if (weeklyincome.bugetLineweek4 != 0)
                                                    weeklyincome.percentage4 = (weeklyincome.mybugetweek4 - weeklyincome.bugetLineweek4) / weeklyincome.bugetLineweek4 * 100;
                                                weeklyincome.mybugetweek5 = weeklyincome.mybugetweek5 - Math.Abs(weeklyDiscount.mybugetweek5);
                                                if (weeklyincome.bugetLineweek5 != 0)
                                                    weeklyincome.percentage5 = (weeklyincome.mybugetweek5 - weeklyincome.bugetLineweek5) / weeklyincome.bugetLineweek5 * 100;
                                                weeklyincome.mybugetweek6 = weeklyincome.mybugetweek6 - Math.Abs(weeklyDiscount.mybugetweek6);
                                                if (weeklyincome.bugetLineweek6 != 0)
                                                    weeklyincome.percentage6 = (weeklyincome.mybugetweek6 - weeklyincome.bugetLineweek6) / weeklyincome.bugetLineweek6 * 100;

                                                weeklyincome.TotalmybugetMonth = weeklyincome.TotalmybugetMonth - Math.Abs(weeklyDiscount.TotalmybugetMonth);
                                                if (weeklyincome.TotalbugetLineMonth != 0)
                                                    weeklyincome.TotalpercentageMonth = (weeklyincome.TotalmybugetMonth - weeklyincome.TotalbugetLineMonth) / weeklyincome.TotalbugetLineMonth * 100;
                                                if (weeklyIncomelist != null)
                                                {
                                                    weeklyIncomelist.mybugetweek1 = weeklyIncomelist.mybugetweek1 - Math.Abs(weeklyDiscount.mybugetweek1);
                                                    if (weeklyIncomelist.bugetLineweek1 != 0)
                                                        weeklyIncomelist.percentage1 = (weeklyIncomelist.mybugetweek1 - weeklyIncomelist.bugetLineweek1) / weeklyIncomelist.bugetLineweek1 * 100;
                                                    weeklyIncomelist.mybugetweek2 = weeklyIncomelist.mybugetweek2 - Math.Abs(weeklyDiscount.mybugetweek2);
                                                    if (weeklyIncomelist.bugetLineweek2 != 0)
                                                        weeklyIncomelist.percentage2 = (weeklyIncomelist.mybugetweek2 - weeklyIncomelist.bugetLineweek2) / weeklyIncomelist.bugetLineweek2 * 100;
                                                    weeklyIncomelist.mybugetweek3 = weeklyIncomelist.mybugetweek3 - Math.Abs(weeklyDiscount.mybugetweek3);
                                                    if (weeklyIncomelist.bugetLineweek3 != 0)
                                                        weeklyIncomelist.percentage3 = (weeklyIncomelist.mybugetweek3 - weeklyIncomelist.bugetLineweek3) / weeklyIncomelist.bugetLineweek3 * 100;
                                                    weeklyIncomelist.mybugetweek4 = weeklyIncomelist.mybugetweek4 - Math.Abs(weeklyDiscount.mybugetweek4);
                                                    if (weeklyIncomelist.bugetLineweek4 != 0)
                                                        weeklyIncomelist.percentage4 = (weeklyIncomelist.mybugetweek4 - weeklyIncomelist.bugetLineweek4) / weeklyIncomelist.bugetLineweek4 * 100;
                                                    weeklyIncomelist.mybugetweek5 = weeklyIncomelist.mybugetweek5 - Math.Abs(weeklyDiscount.mybugetweek5);
                                                    if (weeklyIncomelist.bugetLineweek5 != 0)
                                                        weeklyIncomelist.percentage5 = (weeklyIncomelist.mybugetweek5 - weeklyIncomelist.bugetLineweek5) / weeklyIncomelist.bugetLineweek5 * 100;
                                                    weeklyIncomelist.mybugetweek6 = weeklyIncomelist.mybugetweek6 - Math.Abs(weeklyDiscount.mybugetweek6);
                                                    if (weeklyIncomelist.bugetLineweek6 != 0)
                                                        weeklyIncomelist.percentage6 = (weeklyIncomelist.mybugetweek6 - weeklyIncomelist.bugetLineweek6) / weeklyIncomelist.bugetLineweek6 * 100;

                                                    weeklyIncomelist.TotalmybugetofMonth = weeklyIncomelist.TotalmybugetofMonth - Math.Abs(weeklyDiscount.TotalmybugetMonth);
                                                    if (weeklyIncomelist.TotalbugetLineofMonth != 0)
                                                        weeklyIncomelist.TotalpercentageofMonth = (weeklyIncomelist.TotalmybugetofMonth - weeklyIncomelist.TotalbugetLineofMonth) / weeklyIncomelist.TotalbugetLineofMonth * 100;
                                                }
                                            }
                                        }
                                    }
                                    #region "discount-negate"
                                    if (DiscountCat.weeklyParaList != null && DiscountCat.weeklyParaList.Count() > 0)
                                    {
                                        foreach (weeklyParaListofbudget discountWeek in DiscountCat.weeklyParaList)
                                        {
                                            if (discountWeek.bugetLineweek1 != null)
                                                discountWeek.bugetLineweek1 = Math.Abs((decimal)discountWeek.bugetLineweek1) * -1;
                                            if (discountWeek.bugetLineweek2 != null)
                                                discountWeek.bugetLineweek2 = Math.Abs((decimal)discountWeek.bugetLineweek2) * -1;
                                            if (discountWeek.bugetLineweek3 != null)
                                                discountWeek.bugetLineweek3 = Math.Abs((decimal)discountWeek.bugetLineweek3) * -1;
                                            if (discountWeek.bugetLineweek4 != null)
                                                discountWeek.bugetLineweek4 = Math.Abs((decimal)discountWeek.bugetLineweek4) * -1;
                                            if (discountWeek.bugetLineweek5 != null)
                                                discountWeek.bugetLineweek5 = Math.Abs((decimal)discountWeek.bugetLineweek5) * -1;
                                            if (discountWeek.bugetLineweek6 != null)
                                                discountWeek.bugetLineweek6 = Math.Abs((decimal)discountWeek.bugetLineweek6) * -1;



                                            discountWeek.mybugetweek1 = Math.Abs(discountWeek.mybugetweek1) * -1;
                                            discountWeek.mybugetweek2 = Math.Abs(discountWeek.mybugetweek2) * -1;
                                            discountWeek.mybugetweek3 = Math.Abs(discountWeek.mybugetweek3) * -1;
                                            discountWeek.mybugetweek4 = Math.Abs(discountWeek.mybugetweek4) * -1;
                                            discountWeek.mybugetweek5 = Math.Abs(discountWeek.mybugetweek5) * -1;
                                            discountWeek.mybugetweek6 = Math.Abs(discountWeek.mybugetweek6) * -1;

                                            if (discountWeek.bugetLineweek1 != null && discountWeek.bugetLineweek1 != 0)
                                                discountWeek.percentage1 = (Math.Abs(discountWeek.mybugetweek1) - Math.Abs((decimal)discountWeek.bugetLineweek1)) / Math.Abs((decimal)discountWeek.bugetLineweek1) * 100;
                                            if (discountWeek.bugetLineweek2 != null && discountWeek.bugetLineweek2 != 0)
                                                discountWeek.percentage2 = (Math.Abs(discountWeek.mybugetweek2) - Math.Abs((decimal)discountWeek.bugetLineweek2)) / Math.Abs((decimal)discountWeek.bugetLineweek2) * 100;
                                            if (discountWeek.bugetLineweek3 != null && discountWeek.bugetLineweek3 != 0)
                                                discountWeek.percentage3 = (Math.Abs(discountWeek.mybugetweek3) - Math.Abs((decimal)discountWeek.bugetLineweek3)) / Math.Abs((decimal)discountWeek.bugetLineweek3) * 100;
                                            if (discountWeek.bugetLineweek4 != null && discountWeek.bugetLineweek4 != 0)
                                                discountWeek.percentage4 = (Math.Abs(discountWeek.mybugetweek4) - Math.Abs((decimal)discountWeek.bugetLineweek4)) / Math.Abs((decimal)discountWeek.bugetLineweek4) * 100;
                                            if (discountWeek.bugetLineweek5 != null && discountWeek.bugetLineweek5 != 0)
                                                discountWeek.percentage5 = (Math.Abs(discountWeek.mybugetweek5) - Math.Abs((decimal)discountWeek.bugetLineweek5)) / Math.Abs((decimal)discountWeek.bugetLineweek5) * 100;
                                            if (discountWeek.bugetLineweek6 != null && discountWeek.bugetLineweek6 != 0)
                                                discountWeek.percentage6 = (Math.Abs(discountWeek.mybugetweek6) - Math.Abs((decimal)discountWeek.bugetLineweek6)) / Math.Abs((decimal)discountWeek.bugetLineweek6) * 100;


                                            if (discountWeek.TotalbugetLineMonth != null)
                                                discountWeek.TotalbugetLineMonth = Math.Abs((decimal)discountWeek.TotalbugetLineMonth) * -1;

                                            discountWeek.TotalmybugetMonth = Math.Abs(discountWeek.TotalmybugetMonth) * -1;

                                            if (discountWeek.TotalbugetLineMonth != null && discountWeek.TotalbugetLineMonth != 0)
                                                discountWeek.TotalpercentageMonth = (Math.Abs(discountWeek.TotalmybugetMonth) - Math.Abs((decimal)discountWeek.TotalbugetLineMonth)) / Math.Abs((decimal)discountWeek.TotalbugetLineMonth) * 100;
                                            IncomeCategory.TotalDiscount = Math.Abs(discountWeek.TotalmybugetMonth);
                                        }
                                    }
                                }
                                #endregion

                                #endregion

                                #region

                                #endregion

                                //get income
                                //CompanyReportModel IncomeDetails = IncomeCategory.weekAmountlist.Where(x=>x.)
                                //if (IncomeDetails != null)
                                //{
                                //    if (IncomeDetails.weekAmountlist != null)
                                //    {
                                //        for (int i = 0; i < IncomeDetails.weekAmountlist.Count(); i++)
                                //        {
                                //            if (DiscountDetails.weekAmountlist != null && DiscountDetails.weekAmountlist.Count() >= i)
                                //            {
                                //                weeklyListofbudget weeklyDiscount = DiscountDetails.weekAmountlist[i];
                                //                weeklyListofbudget weeklyincome = IncomeDetails.weekAmountlist[i];
                                //                weeklyincome.mybugetweek1 = weeklyincome.mybugetweek1 - Math.Abs(weeklyDiscount.mybugetweek1);
                                //                weeklyincome.mybugetweek2 = weeklyincome.mybugetweek2 - Math.Abs(weeklyDiscount.mybugetweek2);
                                //                weeklyincome.mybugetweek3 = weeklyincome.mybugetweek3 - Math.Abs(weeklyDiscount.mybugetweek3);
                                //                weeklyincome.mybugetweek4 = weeklyincome.mybugetweek4 - Math.Abs(weeklyDiscount.mybugetweek4);
                                //                weeklyincome.mybugetweek5 = weeklyincome.mybugetweek5 - Math.Abs(weeklyDiscount.mybugetweek5);
                                //                weeklyincome.mybugetweek6 = weeklyincome.mybugetweek6 - Math.Abs(weeklyDiscount.mybugetweek6);
                                //            }
                                //        }
                                //    }

                                //}
                            }


                        }

                        return report_List;
                    }

                }

            }


            return report_List;
        }
        public List<YearlyBudgetOld1> getYearlyData(DateTime reportdate)
        {
            DateTime repoerdate = reportdate;
            var report_List = new List<YearlyBudgetOld1>();
            CompanyReportBL objreport = new CompanyReportBL();
            if (Session["CompanyUser"] != null)
            {
                int companyId = Convert.ToInt32(Session["CompanyId"].ToString());
                YearlyBudgetOld1 updatemodel = new YearlyBudgetOld1();

                using (managementsoftwaredbEntities context = new managementsoftwaredbEntities())
                {
                    //var obj = (from db in context.tblRealBudgets
                    //           join ct in context.tblCategories on db.categoryId equals ct.id
                    //           join mc in context.tblCategories on ct.parentId equals mc.id
                    //           join ex in context.tblExpectedBudgetLines on db.categoryId equals ex.categoryId
                    //           where db.companyId == companyId
                    //           where db.date.Year == repoerdate.Year
                    //           select new reportList
                    //           {
                    //               categoryId = mc.id,
                    //               CategoryName = mc.name,
                    //               // RealBudgetTotal = db.budget,
                    //               // BudgetAmountTotal = ex.budget,
                    //               // BudgetDate = db.date
                    //           }).Distinct().ToList();
                    List<object> parameters = new List<object>();
                    //parameters.Add(new SqlParameter("categoryid", catId));
                    parameters.Add(new SqlParameter("companyId", companyId));
                    parameters.Add(new SqlParameter("date", repoerdate));
                    var obj = context.Database.SqlQuery<YearlyBudgetOld1>("sp_YearlyReport @companyId , @date", parameters.ToArray()).ToList();
                    // return list;

                    if (obj.Any())
                    {
                        double TOTALRealBudget = 0;
                        double TOTALBudgetAmount = 0;
                        for (int i = 0; i < obj.Count; i++)
                        {
                            var Company_Report_item = new YearlyBudgetOld1();
                            Company_Report_item.categoryId = obj[i].categoryId;
                            Company_Report_item.budgettypeId = obj[i].budgettypeId;
                            Company_Report_item.CategoryName = obj[i].CategoryName;
                            Company_Report_item.BudgetAmountTotal = obj[i].BudgetAmountTotal;
                            Company_Report_item.RealBudgetTotal = obj[i].RealBudgetTotal;
                            // Company_Report_item.BudgetDate = obj[i].BudgetDate;
                            report_List.Add(Company_Report_item);
                        }

                        return report_List;
                    }

                }

            }
            return report_List;
        }
        public Decimal getMonthlyData(DateTime reportdate)
        {
            DateTime repoerdate = reportdate;
            decimal TotalMonthlyBudget = 0;
            var report_List = new List<YearlyBudgetOld1>();
            CompanyReportBL objreport = new CompanyReportBL();
            if (Session["CompanyUser"] != null)
            {
                int companyId = Convert.ToInt32(Session["CompanyId"].ToString());
                

                using (managementsoftwaredbEntities context = new managementsoftwaredbEntities())
                {
                    
                    List<object> parameters = new List<object>();
                    //parameters.Add(new SqlParameter("categoryid", catId));
                    parameters.Add(new SqlParameter("companyId", companyId));
                    parameters.Add(new SqlParameter("date", repoerdate));
                    
                    var obj = context.Database.SqlQuery<decimal>("sp_MonthlyReport @companyId , @date", parameters.ToArray()).ToList();
                    // return list;

                    if (obj.Any())
                    {                       
                        TotalMonthlyBudget = obj[0];
                        return TotalMonthlyBudget;
                    }

                }

            }
            return TotalMonthlyBudget;
        }
        public int GetyearlyCarCount(DateTime reportDate)
        {
            int carCount = 0;
            if (Session["CompanyUser"] != null)
            {
                int companyId = Convert.ToInt32(Session["CompanyId"].ToString());

                CompanyReportBL objreport = new CompanyReportBL();
                using (managementsoftwaredbEntities context = new managementsoftwaredbEntities())
                {
                    carCount=  objreport.MyBudgetYearlyCarCount(Convert.ToInt32(Session["CompanyId"].ToString()), reportDate);
                }
            }
            return carCount;
        }
        public ActionResult KeyPointIndicators(string reportdate = "", string Month = "", string Year = "")
        {
            var model = new KeyPointIndicatorModel();
            model.KeyPointIndicators = new List<KeyPointIndicators>();
            string dateforreport = "";
            CompanyReportBL objreport = new CompanyReportBL();
            if (Session["CompanyUser"] != null)
            {
                //if (Request.IsAjaxRequest())
                //{
                if (Month != "" && Year != "")
                {
                    dateforreport = Month + "/01/" + Year;
                    DateTime dat = DateTime.ParseExact(dateforreport.ToString(), "MM/dd/yyyy", null);
                    model.KeyPointIndicators = getKeyPointData(dat);
                }
                if (reportdate != "")
                {
                    ViewBag.reportdate = reportdate;
                    dateforreport = reportdate;
                    DateTime dat = DateTime.ParseExact(reportdate.ToString(), "MM/dd/yyyy", null);
                    model.KeyPointIndicators = getKeyPointData(dat);
                    // return PartialView("CompanyReportList", model);
                }
                //RGK Add Total Income Data
                if (dateforreport != "")
                {
                    var monthlyBudget = getMonthlyData(DateTime.ParseExact(dateforreport.ToString(), "MM/dd/yyyy", null));
                    model.CarCount = GetyearlyCarCount(DateTime.ParseExact(dateforreport.ToString(), "MM/dd/yyyy", null));
                    if (monthlyBudget > 0)
                    {
                        model.Income = monthlyBudget;                        
                    }
                }
                

                // }
                //CompanyReportModel obj = new CompanyReportModel();                
                return View(model);
            }
            return RedirectToAction("Companylogin", "Companylogin");
        }

        public List<KeyPointIndicators> getKeyPointData(DateTime reportdate)
        {
            DateTime repoerdate = reportdate;
            var report_List = new List<KeyPointIndicators>();
            CompanyReportBL objreport = new CompanyReportBL();
            if (Session["CompanyUser"] != null)
            {
                int companyId = Convert.ToInt32(Session["CompanyId"].ToString());
                KeyPointIndicators updatemodel = new KeyPointIndicators();

                using (managementsoftwaredbEntities context = new managementsoftwaredbEntities())
                {
                    //var obj = (from db in context.tblRealBudgets
                    //           join ct in context.tblCategories on db.categoryId equals ct.id
                    //           join mc in context.tblCategories on ct.parentId equals mc.id
                    //           join ex in context.tblExpectedBudgetLines on db.categoryId equals ex.categoryId
                    //           where db.companyId == companyId
                    //           where db.date.Year == repoerdate.Year
                    //           select new reportList
                    //           {
                    //               categoryId = mc.id,
                    //               CategoryName = mc.name,
                    //               // RealBudgetTotal = db.budget,
                    //               // BudgetAmountTotal = ex.budget,
                    //               // BudgetDate = db.date
                    //           }).Distinct().ToList();
                    List<object> parameters = new List<object>();
                    //parameters.Add(new SqlParameter("categoryid", catId));
                    parameters.Add(new SqlParameter("companyId", companyId));
                    parameters.Add(new SqlParameter("date", repoerdate));
                    var obj = context.Database.SqlQuery<KeyPointIndicators>("sp_KeyPointGroup @companyId , @date", parameters.ToArray()).ToList();
                    // return list;

                    if (obj.Any())
                    {
                        double TOTALRealBudget = 0;
                        double TOTALBudgetAmount = 0;
                        for (int i = 0; i < obj.Count; i++)
                        {
                            var Company_Report_item = new KeyPointIndicators();
                            Company_Report_item.keyPointid = obj[i].keyPointid;
                            Company_Report_item.groupName = obj[i].groupName;
                            Company_Report_item.percentage = obj[i].percentage;
                            Company_Report_item.KeyPointTotal = obj[i].KeyPointTotal;
                            Company_Report_item.Budget1TotalIncome = obj[i].Budget1TotalIncome;
                            Company_Report_item.Budget1GrossIncome = obj[i].Budget1GrossIncome;
                            Company_Report_item.isnetProfitKey = obj[i].isnetProfitKey;
                            Company_Report_item.isBusinessDevKey = obj[i].isBusinessDevKey;
                            // Company_Report_item.BudgetDate = obj[i].BudgetDate;
                            report_List.Add(Company_Report_item);
                        }

                        return report_List;
                    }

                }

            }
            return report_List;
        }
        public List<YearlyItem> getKeyPointDataYearly(DateTime reportdate)
        {
            DateTime repoerdate = reportdate;
            var report_List = new List<YearlyItem>();
            CompanyReportBL objreport = new CompanyReportBL();
            if (Session["CompanyUser"] != null)
            {
                int companyId = Convert.ToInt32(Session["CompanyId"].ToString());
                YearlyReportModel updatemodel = new YearlyReportModel();

                using (managementsoftwaredbEntities context = new managementsoftwaredbEntities())
                {
                    //var obj = (from db in context.tblRealBudgets
                    //           join ct in context.tblCategories on db.categoryId equals ct.id
                    //           join mc in context.tblCategories on ct.parentId equals mc.id
                    //           join ex in context.tblExpectedBudgetLines on db.categoryId equals ex.categoryId
                    //           where db.companyId == companyId
                    //           where db.date.Year == repoerdate.Year
                    //           select new reportList
                    //           {
                    //               categoryId = mc.id,
                    //               CategoryName = mc.name,
                    //               // RealBudgetTotal = db.budget,
                    //               // BudgetAmountTotal = ex.budget,
                    //               // BudgetDate = db.date
                    //           }).Distinct().ToList();
                    List<object> parameters = new List<object>();
                    //parameters.Add(new SqlParameter("categoryid", catId));
                    parameters.Add(new SqlParameter("companyId", companyId));
                    parameters.Add(new SqlParameter("date", repoerdate));
                    var obj = context.Database.SqlQuery<KeyPointIndicators>("sp_KeyPointGroup_yearly @companyId , @date", parameters.ToArray()).ToList();
                    // return list;

                    if (obj.Any())
                    {
                        double TOTALRealBudget = 0;
                        double TOTALBudgetAmount = 0;
                        for (int i = 0; i < obj.Count; i++)
                        {
                            var Company_Report_item = new YearlyItem();
                            Company_Report_item.keyPointid = obj[i].keyPointid;
                            Company_Report_item.groupName = obj[i].groupName;
                            Company_Report_item.percentage = obj[i].percentage;
                            Company_Report_item.KeyPointTotal = obj[i].KeyPointTotal;
                            Company_Report_item.Budget1TotalIncome = obj[i].Budget1TotalIncome;
                            Company_Report_item.Budget1GrossIncome = obj[i].Budget1GrossIncome;
                            Company_Report_item.isnetProfitKey = obj[i].isnetProfitKey;
                            Company_Report_item.isBusinessDevKey = obj[i].isBusinessDevKey;
                            // Company_Report_item.BudgetDate = obj[i].BudgetDate;
                            report_List.Add(Company_Report_item);
                        }

                        return report_List;
                    }

                }

            }
            return report_List;
        }
        //[HttpPost]
        //[AllowAnonymous]
        //[ValidateAntiForgeryToken]
        //public ActionResult CompanyBudgetReport(CompanyReportModel model)
        //{
        //    DateTime repoerdate = model.BudgetDate;
        //    CompanyReportBL objreport = new CompanyReportBL();
        //    if (Session["CompanyUser"] != null)
        //    {
        //        int companyId = Convert.ToInt32(Session["CompanyId"].ToString());
        //        CompanyReportModel updatemodel = new CompanyReportModel();
        //        var report_List = new List<CompanyReportModel>();
        //        using (managementsoftwaredbEntities context = new managementsoftwaredbEntities())
        //        {
        //            var obj = (from db in context.tblRealBudgets
        //                       join ct in context.tblCategories on db.categoryId equals ct.id
        //                       join mc in context.tblCategories on ct.parentId equals mc.id
        //                       join ex in context.tblExpectedBudgetLines on db.categoryId equals ex.categoryId
        //                       where db.companyId == companyId
        //                       where db.date.Month == repoerdate.Month
        //                       where db.date.Year == repoerdate.Year
        //                       select new reportList
        //                       {
        //                          categoryId=db.categoryId,
        //                          CategoryName=mc.name,
        //                          RealBudgetTotal= db.budget,
        //                          BudgetAmountTotal = ex.budget,
        //                          BudgetDate =db.date

        //                       }).Distinct().ToList();

        //            if (obj.Any())
        //            {
        //                double TOTALRealBudget = 0;
        //                double TOTALBudgetAmount = 0;
        //                for (int i = 0; i < obj.Count; i++)
        //                {
        //                    var Company_Report_item = new CompanyReportModel();
        //                    Company_Report_item.categoryId = obj[i].categoryId;
        //                    Company_Report_item.CategoryName = obj[i].CategoryName;
        //                    Company_Report_item.BudgetDate = obj[i].BudgetDate;
        //                    Company_Report_item.weekAmountlist = objreport.weekAmountlistfetch(obj[i].categoryId, obj[i].BudgetDate, Convert.ToInt32(Session["CompanyId"].ToString()));
        //                    report_List.Add(Company_Report_item);
        //                }

        //                return View(report_List);
        //            }

        //        }

        //    }
        //    return View();
        //}
    }
}