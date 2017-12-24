using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SmartAdminMvc.Models;
using System.Linq.Dynamic;
using System.Web.Mvc;
using System.Data.SqlClient;

namespace SmartAdminMvc.BL
{
    public class CompanyReportBL
    {
        public List<weeklyBugetList> weekAmountlistfetch(int catId,DateTime BudgetDate, int companyId)
        {
            try
            {
                var weekly_Buget_List = new List<weeklyBugetList>();
                using (managementsoftwaredbEntities context = new managementsoftwaredbEntities())
                {
                    //var obj = (from db in context.tblRealBudgets
                    //           join ex in context.tblExpectedBudgetLines on db.categoryId equals ex.categoryId
                    //           where db.companyId == companyId
                    //           where db.categoryId == catId
                    //           where db.date.Month == BudgetDate.Month
                    //           where db.date.Year == BudgetDate.Year
                    //           select new reportList
                    //           {
                    //               categoryId = db.categoryId,
                    //               RealBudgetTotal = db.budget,
                    //               BudgetAmountTotal = ex.budget,
                    //               BudgetDate = db.date

                    //           }).Distinct().ToList();
                    List<object> parameters = new List<object>();
                    parameters.Add(new SqlParameter("categoryid", catId));
                    parameters.Add(new SqlParameter("companyId", companyId));
                    parameters.Add(new SqlParameter("date", BudgetDate));
                    var obj = context.Database.SqlQuery<weeklyBugetList>("sp_ReportByweeks @categoryid , @companyId , @date", parameters.ToArray()).ToList();
                   // return list;
                    if (obj.Any())
                    {

                        for (int i = 0; i < obj.Count; i++)
                        {
                            var weeklyBugetList_item = new weeklyBugetList();
                            weeklyBugetList_item.weeklyRealBudget = obj[i].weeklyRealBudget;
                            weeklyBugetList_item.weeklyBudgetAmount = obj[i].weeklyBudgetAmount;
                            weeklyBugetList_item.weekNo = obj[i].weekNo;
                            weekly_Buget_List.Add(weeklyBugetList_item);
                        }
                    }
                }
                return weekly_Buget_List;
            }
            catch (Exception e)
            {
                throw e;
            }

        }

        public List<weeklyListofbudget> weekLybudgetlistfetch(int catId, DateTime BudgetDate, int companyId)
        {
            try
            {
                var weekly_Buget_List = new List<weeklyListofbudget>();
                using (managementsoftwaredbEntities context = new managementsoftwaredbEntities())
                {
                    //var obj = (from db in context.tblRealBudgets
                    //           join ex in context.tblExpectedBudgetLines on db.categoryId equals ex.categoryId
                    //           where db.companyId == companyId
                    //           where db.categoryId == catId
                    //           where db.date.Month == BudgetDate.Month
                    //           where db.date.Year == BudgetDate.Year
                    //           select new reportList
                    //           {
                    //               categoryId = db.categoryId,
                    //               RealBudgetTotal = db.budget,
                    //               BudgetAmountTotal = ex.budget,
                    //               BudgetDate = db.date

                    //           }).Distinct().ToList();
                    List<object> parameters = new List<object>();
                    parameters.Add(new SqlParameter("categoryid", catId));
                    parameters.Add(new SqlParameter("companyId", companyId));
                    parameters.Add(new SqlParameter("date", BudgetDate));
                    //var obj = context.Database.SqlQuery<weeklyListofbudget>("sp_ReportByweeks @categoryid , @companyId , @date", parameters.ToArray()).ToList();
                    var obj = context.Database.SqlQuery<weeklyListofbudget>("sp_ReportByweeksWithfilter @categoryid , @companyId , @date", parameters.ToArray()).ToList();
                    // return list;
                    if (obj.Any())
                    {

                        for (int i = 0; i < obj.Count; i++)
                        {
                            var weeklyBugetList_item = new weeklyListofbudget();
                            weeklyBugetList_item.mybugetweek1 = obj[i].mybugetweek1;
                            weeklyBugetList_item.bugetLineweek1 = obj[i].bugetLineweek1;
                            weeklyBugetList_item.mybugetweek2 = obj[i].mybugetweek2;
                            weeklyBugetList_item.bugetLineweek2 = obj[i].bugetLineweek2;
                            weeklyBugetList_item.mybugetweek3 = obj[i].mybugetweek3;
                            weeklyBugetList_item.bugetLineweek3 = obj[i].bugetLineweek3;
                            weeklyBugetList_item.mybugetweek4 = obj[i].mybugetweek4;
                            weeklyBugetList_item.bugetLineweek4 = obj[i].bugetLineweek4;
                            weeklyBugetList_item.mybugetweek5 = obj[i].mybugetweek5;
                            weeklyBugetList_item.bugetLineweek5 = obj[i].bugetLineweek5;
                            weeklyBugetList_item.mybugetweek6 = obj[i].mybugetweek6;
                            weeklyBugetList_item.bugetLineweek6 = obj[i].bugetLineweek6;
                            weeklyBugetList_item.TotalmybugetofMonth = obj[i].TotalmybugetofMonth;
                            weeklyBugetList_item.TotalbugetLineofMonth = obj[i].TotalbugetLineofMonth;
                            weeklyBugetList_item.percentage1 = obj[i].percentage1;
                            weeklyBugetList_item.percentage2 = obj[i].percentage2;
                            weeklyBugetList_item.percentage3 = obj[i].percentage3;
                            weeklyBugetList_item.percentage4 = obj[i].percentage4;
                            weeklyBugetList_item.percentage5 = obj[i].percentage5;
                            weeklyBugetList_item.percentage6 = obj[i].percentage6;
                            weeklyBugetList_item.TotalpercentageofMonth = obj[i].TotalpercentageofMonth;
                            weeklyBugetList_item.BudgetTypeId = obj[i].BudgetTypeId;
                            weeklyBugetList_item.totalweekdays = obj[i].totalweekdays;
                            weekly_Buget_List.Add(weeklyBugetList_item);
                        }
                    }
                }
                return weekly_Buget_List;
            }
            catch (Exception e)
            {
                throw e;
            }

        }

        public List<weeklycarcounts> weekLycarcountlistfetch(DateTime BudgetDate, int companyId)
        {
            try
            {
                var weekly_cars_List = new List<weeklycarcounts>();
                using (managementsoftwaredbEntities context = new managementsoftwaredbEntities())
                {
                    //var obj = (from db in context.tblRealBudgets
                    //           join ex in context.tblExpectedBudgetLines on db.categoryId equals ex.categoryId
                    //           where db.companyId == companyId
                    //           where db.categoryId == catId
                    //           where db.date.Month == BudgetDate.Month
                    //           where db.date.Year == BudgetDate.Year
                    //           select new reportList
                    //           {
                    //               categoryId = db.categoryId,
                    //               RealBudgetTotal = db.budget,
                    //               BudgetAmountTotal = ex.budget,
                    //               BudgetDate = db.date

                    //           }).Distinct().ToList();
                    List<object> parameters = new List<object>();
                    parameters.Add(new SqlParameter("companyId", companyId));
                    parameters.Add(new SqlParameter("date", BudgetDate));
                    var obj = context.Database.SqlQuery<weeklycarcounts>("sp_weekly_carCounts @companyId , @date", parameters.ToArray()).ToList();
                    // return list;
                    if (obj.Any())
                    {

                        for (int i = 0; i < obj.Count; i++)
                        {
                            var weeklyCarList_item = new weeklycarcounts();
                            weeklyCarList_item.carsinWeek_1 = obj[i].carsinWeek_1;
                            weeklyCarList_item.carsinWeek_2 = obj[i].carsinWeek_2;
                            weeklyCarList_item.carsinWeek_3 = obj[i].carsinWeek_3;
                            weeklyCarList_item.carsinWeek_4 = obj[i].carsinWeek_4;
                            weeklyCarList_item.carsinWeek_5 = obj[i].carsinWeek_5;
                            weeklyCarList_item.carsinWeek_6 = obj[i].carsinWeek_6;
                            weeklyCarList_item.totalweekdays = obj[i].totalweekdays;
                            weekly_cars_List.Add(weeklyCarList_item);
                        }
                    }
                }
                return weekly_cars_List;
            }
            catch (Exception e)
            {
                throw e;
            }

        }
        public List<weeklyListofbudget> IncomVScostofsalelistfetch(int catId, DateTime BudgetDate, int companyId)
        {
            try
            {
                var weekly_Buget_List = new List<weeklyListofbudget>();
                using (managementsoftwaredbEntities context = new managementsoftwaredbEntities())
                {
                    //var obj = (from db in context.tblRealBudgets
                    //           join ex in context.tblExpectedBudgetLines on db.categoryId equals ex.categoryId
                    //           where db.companyId == companyId
                    //           where db.categoryId == catId
                    //           where db.date.Month == BudgetDate.Month
                    //           where db.date.Year == BudgetDate.Year
                    //           select new reportList
                    //           {
                    //               categoryId = db.categoryId,
                    //               RealBudgetTotal = db.budget,
                    //               BudgetAmountTotal = ex.budget,
                    //               BudgetDate = db.date

                    //           }).Distinct().ToList();
                    List<object> parameters = new List<object>();
                    parameters.Add(new SqlParameter("categoryid", catId));
                    parameters.Add(new SqlParameter("companyId", companyId));
                    parameters.Add(new SqlParameter("date", BudgetDate));
                    var obj = context.Database.SqlQuery<weeklyListofbudget>("sp_IncomVScostofsale @categoryid , @companyId , @date", parameters.ToArray()).ToList();
                    // return list;
                    if (obj.Any())
                    {

                        for (int i = 0; i < obj.Count; i++)
                        {
                            var weeklyBugetList_item = new weeklyListofbudget();
                            weeklyBugetList_item.mybugetweek1 = obj[i].mybugetweek1;
                            weeklyBugetList_item.bugetLineweek1 = obj[i].bugetLineweek1;
                            weeklyBugetList_item.mybugetweek2 = obj[i].mybugetweek2;
                            weeklyBugetList_item.bugetLineweek2 = obj[i].bugetLineweek2;
                            weeklyBugetList_item.mybugetweek3 = obj[i].mybugetweek3;
                            weeklyBugetList_item.bugetLineweek3 = obj[i].bugetLineweek3;
                            weeklyBugetList_item.mybugetweek4 = obj[i].mybugetweek4;
                            weeklyBugetList_item.bugetLineweek4 = obj[i].bugetLineweek4;
                            weeklyBugetList_item.mybugetweek5 = obj[i].mybugetweek5;
                            weeklyBugetList_item.bugetLineweek5 = obj[i].bugetLineweek5;
                            weeklyBugetList_item.mybugetweek6 = obj[i].mybugetweek6;
                            weeklyBugetList_item.bugetLineweek6 = obj[i].bugetLineweek6;
                            weeklyBugetList_item.TotalmybugetofMonth = obj[i].TotalmybugetofMonth;
                            weeklyBugetList_item.TotalbugetLineofMonth = obj[i].TotalbugetLineofMonth;
                            weeklyBugetList_item.percentage1 = obj[i].percentage1;
                            weeklyBugetList_item.percentage2 = obj[i].percentage2;
                            weeklyBugetList_item.percentage3 = obj[i].percentage3;
                            weeklyBugetList_item.percentage4 = obj[i].percentage4;
                            weeklyBugetList_item.percentage5 = obj[i].percentage5;
                            weeklyBugetList_item.percentage6 = obj[i].percentage6;
                            weeklyBugetList_item.TotalpercentageofMonth = obj[i].TotalpercentageofMonth;
                            weeklyBugetList_item.BudgetTypeId = obj[i].BudgetTypeId;
                            weeklyBugetList_item.totalweekdays = obj[i].totalweekdays;
                            weekly_Buget_List.Add(weeklyBugetList_item);
                        }
                    }
                }
                return weekly_Buget_List;
            }
            catch (Exception e)
            {
                throw e;
            }

        }
        public List<weeklyListofbudget> grosprofitlistfetch(int UTILITIEScatId, int BUDGET3catId, int BUDGET2catId, DateTime BudgetDate, int companyId)
        {
            try
            {
                var weekly_Buget_List = new List<weeklyListofbudget>();
                using (managementsoftwaredbEntities context = new managementsoftwaredbEntities())
                {
                    List<object> parameters = new List<object>();
                    parameters.Add(new SqlParameter("UTILITIEScatId", UTILITIEScatId));
                    parameters.Add(new SqlParameter("BUDGET3catId", BUDGET3catId));
                    parameters.Add(new SqlParameter("BUDGET2catId", BUDGET2catId));
                    parameters.Add(new SqlParameter("companyId", companyId));
                    parameters.Add(new SqlParameter("date", BudgetDate));
                    var obj = context.Database.SqlQuery<weeklyListofbudget>("sp_WeeklyGP @UTILITIEScatId ,@BUDGET3catId ,@BUDGET2catId , @companyId , @date", parameters.ToArray()).ToList();
                    // return list;
                    if (obj.Any())
                    {

                        for (int i = 0; i < obj.Count; i++)
                        {
                            var GPweekly_item = new weeklyListofbudget();
                            GPweekly_item.mybugetweek1 = obj[i].mybugetweek1;
                            GPweekly_item.bugetLineweek1 = obj[i].bugetLineweek1;
                            GPweekly_item.mybugetweek2 = obj[i].mybugetweek2;
                            GPweekly_item.bugetLineweek2 = obj[i].bugetLineweek2;
                            GPweekly_item.mybugetweek3 = obj[i].mybugetweek3;
                            GPweekly_item.bugetLineweek3 = obj[i].bugetLineweek3;
                            GPweekly_item.mybugetweek4 = obj[i].mybugetweek4;
                            GPweekly_item.bugetLineweek4 = obj[i].bugetLineweek4;
                            GPweekly_item.mybugetweek5 = obj[i].mybugetweek5;
                            GPweekly_item.bugetLineweek5 = obj[i].bugetLineweek5;
                            GPweekly_item.mybugetweek6 = obj[i].mybugetweek6;
                            GPweekly_item.bugetLineweek6 = obj[i].bugetLineweek6;
                            GPweekly_item.TotalmybugetofMonth = obj[i].TotalmybugetofMonth;
                            GPweekly_item.TotalbugetLineofMonth = obj[i].TotalbugetLineofMonth;
                            GPweekly_item.percentage1 = obj[i].percentage1;
                            GPweekly_item.percentage2 = obj[i].percentage2;
                            GPweekly_item.percentage3 = obj[i].percentage3;
                            GPweekly_item.percentage4 = obj[i].percentage4;
                            GPweekly_item.percentage5 = obj[i].percentage5;
                            GPweekly_item.percentage6 = obj[i].percentage6;
                            GPweekly_item.TotalpercentageofMonth = obj[i].TotalpercentageofMonth;
                            GPweekly_item.BudgetTypeId = obj[i].BudgetTypeId;
                            GPweekly_item.totalweekdays = obj[i].totalweekdays;
                            weekly_Buget_List.Add(GPweekly_item);
                        }
                    }
                }
                return weekly_Buget_List;
            }
            catch (Exception e)
            {
                throw e;
            }

        }
        public List<CategoryParalist> CategoryParameterslistfetch(int catId, DateTime BudgetDate, int companyId)
        {
            try
            {
                var Para_Budget_list = new List<CategoryParalist>();
                using (managementsoftwaredbEntities context = new managementsoftwaredbEntities())
                {
                    //var obj = (from db in context.tblRealBudgets
                    //           join ex in context.tblExpectedBudgetLines on db.categoryId equals ex.categoryId
                    //           where db.companyId == companyId
                    //           where db.categoryId == catId
                    //           where db.date.Month == BudgetDate.Month
                    //           where db.date.Year == BudgetDate.Year
                    //           select new reportList
                    //           {
                    //               categoryId = db.categoryId,
                    //               RealBudgetTotal = db.budget,
                    //               BudgetAmountTotal = ex.budget,
                    //               BudgetDate = db.date

                    //           }).Distinct().ToList();
                    List<object> parameters = new List<object>();
                    parameters.Add(new SqlParameter("categoryid", catId));
                    parameters.Add(new SqlParameter("companyId", companyId));
                    parameters.Add(new SqlParameter("date", BudgetDate));
                    var obj = context.Database.SqlQuery<CategoryParalist>("sp_ParametersforReport @categoryid , @companyId , @date", parameters.ToArray()).ToList();
                    // return list;
                    if (obj.Any())
                    {

                        for (int i = 0; i < obj.Count; i++)
                        {
                            var Para_BudgetList_item = new CategoryParalist();
                            Para_BudgetList_item.id = obj[i].id;
                            Para_BudgetList_item.paraName = obj[i].paraName;
                            Para_BudgetList_item.paraBudgetAmount = obj[i].paraBudgetAmount;
                            Para_BudgetList_item.paraRealBudget = obj[i].paraRealBudget;
                            Para_BudgetList_item.weeklyParaList = weeklyParameterslistfetch(obj[i].id, BudgetDate, companyId);
                            Para_Budget_list.Add(Para_BudgetList_item);
                        }
                    }
                }
                return Para_Budget_list;
            }
            catch (Exception e)
            {
                throw e;
            }

        }
        public List<weeklyParaListofbudget> weeklyParameterslistfetch(int catId, DateTime BudgetDate, int companyId)
        {
            try
            {
                var weekly_Para_Buget_List = new List<weeklyParaListofbudget>();
                using (managementsoftwaredbEntities context = new managementsoftwaredbEntities())
                {
                    //var obj = (from db in context.tblRealBudgets
                    //           join ex in context.tblExpectedBudgetLines on db.categoryId equals ex.categoryId
                    //           where db.companyId == companyId
                    //           where db.categoryId == catId
                    //           where db.date.Month == BudgetDate.Month
                    //           where db.date.Year == BudgetDate.Year
                    //           select new reportList
                    //           {
                    //               categoryId = db.categoryId,
                    //               RealBudgetTotal = db.budget,
                    //               BudgetAmountTotal = ex.budget,
                    //               BudgetDate = db.date

                    //           }).Distinct().ToList();
                    List<object> parameters = new List<object>();
                    parameters.Add(new SqlParameter("categoryid", catId));
                    parameters.Add(new SqlParameter("companyId", companyId));
                    parameters.Add(new SqlParameter("date", BudgetDate));
                    //var obj = context.Database.SqlQuery<weeklyParaListofbudget>("sp_ParametersReportByweeks @categoryid , @companyId , @date", parameters.ToArray()).ToList();
                    var obj = context.Database.SqlQuery<weeklyParaListofbudget>("sp_ParametersReportByweeksWithfilter @categoryid , @companyId , @date", parameters.ToArray()).ToList();
                    // return list;
                    if (obj.Any())
                    {
                        for (int i = 0; i < obj.Count; i++)
                        {
                            var weeklyparaBugetList_item = new weeklyParaListofbudget();
                            weeklyparaBugetList_item.mybugetweek1 = obj[i].mybugetweek1;
                            weeklyparaBugetList_item.bugetLineweek1 = obj[i].bugetLineweek1;
                            weeklyparaBugetList_item.mybugetweek2 = obj[i].mybugetweek2;
                            weeklyparaBugetList_item.bugetLineweek2 = obj[i].bugetLineweek2;
                            weeklyparaBugetList_item.mybugetweek3 = obj[i].mybugetweek3;
                            weeklyparaBugetList_item.bugetLineweek3 = obj[i].bugetLineweek3;
                            weeklyparaBugetList_item.mybugetweek4 = obj[i].mybugetweek4;
                            weeklyparaBugetList_item.bugetLineweek4 = obj[i].bugetLineweek4;
                            weeklyparaBugetList_item.mybugetweek5 = obj[i].mybugetweek5;
                            weeklyparaBugetList_item.bugetLineweek5 = obj[i].bugetLineweek5;
                            weeklyparaBugetList_item.mybugetweek6 = obj[i].mybugetweek6;
                            weeklyparaBugetList_item.bugetLineweek6 = obj[i].bugetLineweek6;
                            weeklyparaBugetList_item.TotalmybugetMonth = obj[i].TotalmybugetMonth;
                            weeklyparaBugetList_item.TotalbugetLineMonth = obj[i].TotalbugetLineMonth;
                            weeklyparaBugetList_item.percentage1 = obj[i].percentage1;
                            weeklyparaBugetList_item.percentage2 = obj[i].percentage2;
                            weeklyparaBugetList_item.percentage3 = obj[i].percentage3;
                            weeklyparaBugetList_item.percentage4 = obj[i].percentage4;
                            weeklyparaBugetList_item.percentage5 = obj[i].percentage5;
                            weeklyparaBugetList_item.percentage6 = obj[i].percentage6;
                            weeklyparaBugetList_item.TotalpercentageMonth = obj[i].TotalpercentageMonth;
                            weeklyparaBugetList_item.BudgetTypeId = obj[i].BudgetTypeId;
                            weeklyparaBugetList_item.totalweekdays = obj[i].totalweekdays;
                            weekly_Para_Buget_List.Add(weeklyparaBugetList_item);
                        }
                    }
                }
                return weekly_Para_Buget_List;
            }
            catch (Exception e)
            {
                throw e;
            }

        }
        public int MyBudgetCarCount(int CompanyId, DateTime date)
        {
            try
            {
                int CarCounts = 0;
                using (managementsoftwaredbEntities context = new managementsoftwaredbEntities())
                {
                    List<object> parameters = new List<object>();
                    parameters.Add(new SqlParameter("companyId", CompanyId));
                    parameters.Add(new SqlParameter("date", date));
                    var obj = context.Database.SqlQuery<CarcountModel>("sp_carCountsofMonth  @companyId , @date", parameters.ToArray()).ToList();
                    if (obj.Any())
                    {
                        CarCounts = Convert.ToInt32(obj[0].CarCounts);
                    }
                }
                return CarCounts;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally { }
        }
        public int MyBudgetYearlyCarCount(int CompanyId, DateTime date)
        {
            try
            {
                int CarCounts = 0;
                using (managementsoftwaredbEntities context = new managementsoftwaredbEntities())
                {
                    List<object> parameters = new List<object>();
                    parameters.Add(new SqlParameter("companyId", CompanyId));
                    parameters.Add(new SqlParameter("date", date));
                    var obj = context.Database.SqlQuery<CarcountModel>("sp_carCountsofYear  @companyId , @date", parameters.ToArray()).ToList();
                    if (obj.Any())
                    {
                        CarCounts = Convert.ToInt32(obj[0].CarCounts);
                    }
                }
                return CarCounts;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally { }
        }
    }
}