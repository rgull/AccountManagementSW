using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SmartAdminMvc.Models;
using System.Linq.Dynamic;
using System.Web.Mvc;

namespace SmartAdminMvc.BL
{
    public class CompanyAccountBL
    {
        public List<SelectListItem> Categoryfetch(int tid)
        {
            try
            {
                var CategoryList = new List<SelectListItem>();
                using (managementsoftwaredbEntities context = new managementsoftwaredbEntities())
                {
                    //    var obj = (from db in context.tblCategories
                    //               join ty in context.tblBudgetTypes on db.budgetTypeId  equals ty.id
                    //               where db.budgetTypeId == tid where db.isActive == true
                    var obj = (from db in context.tblCategories
                               where db.budgetTypeId == tid
                               && db.parentId == 0
                               && db.isActive == true
                               select new
                               {
                                   db.id,
                                   db.name
                               }).ToList();
                    if (obj.Any())
                    {

                        for (int i = 0; i < obj.Count; i++)
                        {
                            var budget_Type_item = new SelectListItem();
                            budget_Type_item.Text = obj[i].name;
                            budget_Type_item.Value = obj[i].id.ToString();
                            CategoryList.Add(budget_Type_item);
                        }
                    }
                }
                return CategoryList;
            }
            catch (Exception e)
            {
                throw e;
            }

        }

        /// <summary>
        ///  Bind dropdown of Budget Type in add category page.
        /// </summary>
        public List<BudgetTypeList> BudgetTypefetch()
        {
            try
            {
                // var budget_TypeList = new List<SelectListItem>();
                var budget_TypeList = new List<BudgetTypeList>();
                using (managementsoftwaredbEntities context = new managementsoftwaredbEntities())
                {
                    var obj = (from db in context.tblBudgetTypes
                               where db.isActive == true
                               select db).ToList();
                    if (obj.Any())
                    {

                        for (int i = 0; i < obj.Count; i++)
                        {
                            //var budget_Type_item = new SelectListItem();
                            //budget_Type_item.Text = obj[i].name;
                            //budget_Type_item.Value = obj[i].id.ToString();
                            //budget_TypeList.Add(budget_Type_item);
                            var budget_Type_item = new BudgetTypeList();
                            budget_Type_item.name = obj[i].name;
                            budget_Type_item.id = obj[i].id;
                            budget_TypeList.Add(budget_Type_item);
                        }
                    }
                }
                return budget_TypeList;
            }
            catch (Exception e)
            {
                throw e;
            }

        }

        public List<MybudgetFullList> MybudgetLine(int CompanyId)
        {
            try
            {
                // var budget_TypeList = new List<SelectListItem>();
                var mybudget_List = new List<MybudgetFullList>();
                using (managementsoftwaredbEntities context = new managementsoftwaredbEntities())
                {
                    var obj = (from db in context.tblExpectedBudgetLines
                               join cc in context.tblcompanyCategories on db.categoryId equals cc.categoryId
                               join bt in context.tblBudgetTypes on db.budgetTypeId equals bt.id
                               join ct in context.tblCategories on db.categoryId equals ct.id
                               join mct in context.tblCategories on ct.parentId equals mct.id
                               where db.companyId == CompanyId
                               && db.isActive == true
                               //where cc.companyId == CompanyId
                               //where cc.isActive == true
                               select new
                               {
                                   budgetTypeId = db.budgetTypeId,
                                   budgetTypeName = bt.name,
                                   categoryId = mct.id,
                                   categoryName = mct.name
                               }).Distinct().ToList();
                    if (obj.Any())
                    {

                        for (int i = 0; i < obj.Count; i++)
                        {
                            //var budget_Type_item = new SelectListItem();
                            //budget_Type_item.Text = obj[i].name;
                            //budget_Type_item.Value = obj[i].id.ToString();
                            //budget_TypeList.Add(budget_Type_item);
                            var mybudget_item = new MybudgetFullList();
                            mybudget_item.budgetTypeId = obj[i].budgetTypeId;
                            mybudget_item.budgetTypeName = obj[i].budgetTypeName;
                            mybudget_item.categoryId = obj[i].categoryId;
                            mybudget_item.categoryName = obj[i].categoryName.ToString();
                            mybudget_item.MyValueParameterList = MyCategoryValueFetch(obj[i].categoryId, CompanyId);
                            mybudget_List.Add(mybudget_item);
                        }
                    }
                }
                return mybudget_List;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<MybudgetFullList> MybudgetLineWithFilter(int CompanyId, DateTime date)
        {
            try
            {
                // var budget_TypeList = new List<SelectListItem>();
                var mybudget_List = new List<MybudgetFullList>();
                using (managementsoftwaredbEntities context = new managementsoftwaredbEntities())
                {
                    
                    var obj = (from db in context.tblExpectedBudgetLines
                               join cc in context.tblcompanyCategories on db.categoryId equals cc.categoryId
                               join bt in context.tblBudgetTypes on db.budgetTypeId equals bt.id
                               join ct in context.tblCategories on db.categoryId equals ct.id
                               join mct in context.tblCategories on ct.parentId equals mct.id
                               where (db.date <= date && db.reportType < 3) || (db.reportType == 3 && db.date.Year == date.Year)
                               && db.companyId == CompanyId
                               && db.isActive == true
                               select new
                               {
                                   budgetTypeId = db.budgetTypeId,
                                   budgetTypeName = bt.name,
                                   categoryId = mct.id,
                                   categoryName = mct.name
                               }).Distinct().OrderBy("categoryId").ToList();



                    //where cc.companyId == CompanyId
                    //where cc.isActive == true

                    if (obj.Any())
                    {

                        for (int i = 0; i < obj.Count; i++)
                        {
                            //var budget_Type_item = new SelectListItem();
                            //budget_Type_item.Text = obj[i].name;
                            //budget_Type_item.Value = obj[i].id.ToString();
                            //budget_TypeList.Add(budget_Type_item);
                            var mybudget_item = new MybudgetFullList();
                            mybudget_item.budgetTypeId = obj[i].budgetTypeId;
                            mybudget_item.budgetTypeName = obj[i].budgetTypeName;
                            mybudget_item.categoryId = obj[i].categoryId;
                            mybudget_item.categoryName = obj[i].categoryName.ToString();
                            mybudget_item.MyValueParameterList = MyCategoryValueFetchWithfilter(obj[i].categoryId, CompanyId, date);
                            mybudget_List.Add(mybudget_item);
                        }
                    }
                }
                return mybudget_List;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// RGK Added newmethodtofetchall heads with previous values
        /// </summary>
        /// <param name="CompanyId"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        public List<MybudgetFullList> MybudgetLinesWithFilter(int CompanyId, DateTime date)
        {
            try
            {
                // var budget_TypeList = new List<SelectListItem>();
                var mybudget_List = new List<MybudgetFullList>();
                using (managementsoftwaredbEntities context = new managementsoftwaredbEntities())
                {
                    // Old Code 11-06-17
                    //var obj = (from db in context.tblExpectedBudgetLines
                    //           join cc in context.tblcompanyCategories on db.categoryId equals cc.categoryId
                    //           join bt in context.tblBudgetTypes on db.budgetTypeId equals bt.id
                    //           join ct in context.tblCategories on db.categoryId equals ct.id
                    //           join mct in context.tblCategories on ct.parentId equals mct.id
                    //           where (db.date <= date && db.reportType < 3) || (db.reportType == 3 && db.date.Year == date.Year)
                    //           && db.companyId == CompanyId
                    //           && db.isActive == true                               
                    //           select new
                    //           {
                    //               budgetTypeId = db.budgetTypeId,
                    //               budgetTypeName = bt.name,
                    //               categoryId = mct.id,
                    //               categoryName = mct.name
                    //           }).Distinct().OrderBy("categoryId").ToList();

                    var obj = (from
                             //db in context.tblExpectedBudgetLines
                             ct in context.tblCategories
                               join mct in context.tblCategories on ct.parentId equals mct.id
                               join cc in context.tblcompanyCategories on ct.id equals cc.categoryId
                               join bt in context.tblBudgetTypes on ct.budgetTypeId equals bt.id
                               where cc.companyId == CompanyId
                               && ct.isActive == true
                               select new
                               {
                                   budgetTypeId = ct.budgetTypeId,
                                   budgetTypeName = bt.name,
                                   categoryId = mct.id,
                                   categoryName = mct.name
                               }).Distinct().OrderBy("categoryId").ToList();
                    //where cc.companyId == CompanyId
                    //where cc.isActive == true

                    if (obj.Any())
                    {

                        for (int i = 0; i < obj.Count; i++)
                        {
                            //var budget_Type_item = new SelectListItem();
                            //budget_Type_item.Text = obj[i].name;
                            //budget_Type_item.Value = obj[i].id.ToString();
                            //budget_TypeList.Add(budget_Type_item);
                            var mybudget_item = new MybudgetFullList();
                            mybudget_item.budgetTypeId = obj[i].budgetTypeId;
                            mybudget_item.budgetTypeName = obj[i].budgetTypeName;
                            mybudget_item.categoryId = obj[i].categoryId;
                            mybudget_item.categoryName = obj[i].categoryName.ToString();
                            mybudget_item.MyValueParameterList = MyCategoryValuesFetchWithfilter(obj[i].categoryId, CompanyId, date);
                            mybudget_List.Add(mybudget_item);
                        }
                    }
                }
                return mybudget_List;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<MybudgetFullList> EditMybudgetLine(int CompanyId, DateTime date)
        {
            try
            {
                // var budget_TypeList = new List<SelectListItem>();
                var mybudget_List = new List<MybudgetFullList>();
                using (managementsoftwaredbEntities context = new managementsoftwaredbEntities())
                {
                    var obj = (from db in context.tblExpectedBudgetLines
                               join bt in context.tblBudgetTypes on db.budgetTypeId equals bt.id
                               join ct in context.tblCategories on db.categoryId equals ct.id
                               join mct in context.tblCategories on ct.parentId equals mct.id
                               where db.companyId == CompanyId
                               // where db.date == date
                               where db.isActive == true
                               select new
                               {
                                   budgetTypeId = db.budgetTypeId,
                                   budgetTypeName = bt.name,
                                   categoryId = mct.id,
                                   categoryName = mct.name
                               }).Distinct().ToList();
                    if (obj.Any())
                    {

                        for (int i = 0; i < obj.Count; i++)
                        {
                            //var budget_Type_item = new SelectListItem();
                            //budget_Type_item.Text = obj[i].name;
                            //budget_Type_item.Value = obj[i].id.ToString();
                            //budget_TypeList.Add(budget_Type_item);
                            var mybudget_item = new MybudgetFullList();
                            mybudget_item.budgetTypeId = obj[i].budgetTypeId;
                            mybudget_item.budgetTypeName = obj[i].budgetTypeName;
                            mybudget_item.categoryId = obj[i].categoryId;
                            mybudget_item.categoryName = obj[i].categoryName.ToString();
                            mybudget_item.MyValueParameterList = EditMyCategoryValueFetch(obj[i].categoryId, CompanyId, date);
                            mybudget_List.Add(mybudget_item);
                        }
                    }
                }
                return mybudget_List;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public List<MyParameterList> MyCategoryValueFetch(int cid, int CompanyId)
        {

            try
            {
                var categoryTypeList = new List<MyParameterList>();
                // var categoryTypeList = new List<CategoryList>();
                using (managementsoftwaredbEntities context = new managementsoftwaredbEntities())
                {
                    //var obj = (from db in context.tblCategories where db.parentId == cid
                    //           where db.isActive == true
                    //           select db).ToList();
                    var obj = (from cc in context.tblExpectedBudgetLines
                               join cct in context.tblcompanyCategories on cc.categoryId equals cct.categoryId
                               join ty in context.tblCategories on cc.categoryId equals ty.id
                               join mc in context.tblCategories on ty.parentId equals mc.id
                               where ty.parentId == cid
                               //where cc.companyId == CompanyId
                               //where cc.isActive == true
                               where cct.companyId == CompanyId
                               where cc.companyId == CompanyId
                               where cct.isActive == true
                               select new
                               {
                                   ty.id,
                                   ty.name
                               }).Distinct().ToList();

                    if (obj.Any())
                    {

                        for (int i = 0; i < obj.Count; i++)
                        {
                            var mycatTypeList = new MyParameterList();
                            mycatTypeList.name = obj[i].name;
                            mycatTypeList.id = obj[i].id;
                            if (checkOnceInmonth(obj[i].id, CompanyId) > 0)
                            {
                                //if (HaveentryInOnceInmonth(obj[i].id, CompanyId) == 0)
                                //{
                                categoryTypeList.Add(mycatTypeList);
                                // }
                            }
                            else
                            {
                                categoryTypeList.Add(mycatTypeList);
                            }
                            //var catTypeList = new CategoryList();
                            //catTypeList.name = obj[i].name;
                            //catTypeList.id = obj[i].id;
                            //categoryTypeList.Add(catTypeList);
                        }
                    }
                }
                return categoryTypeList;
            }
            catch (Exception e)
            {
                throw e;
            }

        }
        public List<MyParameterList> MyCategoryValueFetchWithfilter(int cid, int CompanyId, DateTime date)
        {

            try
            {
                var categoryTypeList = new List<MyParameterList>();
                // var categoryTypeList = new List<CategoryList>();
                using (managementsoftwaredbEntities context = new managementsoftwaredbEntities())
                {
                    //var obj = (from db in context.tblCategories where db.parentId == cid
                    //           where db.isActive == true
                    //           select db).ToList();



                    var obj = (from cc in context.tblExpectedBudgetLines
                               join cct in context.tblcompanyCategories on cc.categoryId equals cct.categoryId
                               join ty in context.tblCategories on cc.categoryId equals ty.id
                               //join mc in context.tblCategories on ty.parentId equals mc.id
                               where ty.parentId == cid
                               //where cc.companyId == CompanyId
                               //where cc.isActive == true
                               && cct.companyId == CompanyId
                               && cc.companyId == CompanyId
                               && ((cc.date <= date && cc.reportType < 3) || (cc.reportType == 3 && cc.date.Year == date.Year))
                               && cct.isActive == true
                               select new
                               {
                                   ty.id,
                                   ty.name,
                               }).Distinct().ToList();



                    if (obj.Any())
                    {

                        for (int i = 0; i < obj.Count; i++)
                        {
                            var mycatTypeList = new MyParameterList();
                            mycatTypeList.name = obj[i].name;
                            mycatTypeList.id = obj[i].id;
                           
                            if (checkOnceInmonth(obj[i].id, CompanyId,date) > 0)
                            {
                                decimal lastWeekValue = HaveentryInOnceInmonth(obj[i].id, CompanyId, date);
                                if (lastWeekValue == 0)
                                {
                                    mycatTypeList.IsDisabled = false;
                                }
                                else
                                {
                                    mycatTypeList.IsDisabled = true;
                                    mycatTypeList.budget = lastWeekValue;
                                }
                                categoryTypeList.Add(mycatTypeList);
                            }
                            else
                            {
                                categoryTypeList.Add(mycatTypeList);
                            }
                            //var catTypeList = new CategoryList();
                            //catTypeList.name = obj[i].name;
                            //catTypeList.id = obj[i].id;
                            //categoryTypeList.Add(catTypeList);
                        }
                    }
                }
                return categoryTypeList;
            }
            catch (Exception e)
            {
                throw e;
            }

        }
        /// <summary>
        /// RGK Added NewMethod to collect all headers with previous values
        /// </summary>
        /// <param name="cid"></param>
        /// <param name="CompanyId"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        public List<MyParameterList> MyCategoryValuesFetchWithfilter(int cid, int CompanyId, DateTime date)
        {

            try
            {
                var categoryTypeList = new List<MyParameterList>();
                // var categoryTypeList = new List<CategoryList>();
                using (managementsoftwaredbEntities context = new managementsoftwaredbEntities())
                {
                    //var obj = (from db in context.tblCategories where db.parentId == cid
                    //           where db.isActive == true
                    //           select db).ToList();
                    var qry = (from cc in context.tblCategories
                               join cct in context.tblcompanyCategories on cc.id equals cct.categoryId
                               join ebl in context.tblExpectedBudgetLines on cc.id equals ebl.categoryId                                
                               into category
                               from budgetCategory in category.DefaultIfEmpty()
                               where cc.parentId == cid
                               && cct.companyId == CompanyId
                               && cc.isActive == true
                               && (budgetCategory == null || ((budgetCategory.date <= date && budgetCategory.reportType < 3) || (budgetCategory.reportType == 3 && budgetCategory.date.Year == date.Year)))
                               select new
                               {
                                   cc.id,
                                   cc.name,
                                   budget = budgetCategory == null ? 0 : budgetCategory.budget,
                                   reportType = budgetCategory == null ? 1 : budgetCategory.reportType,
                                   isAddOnceInMonth = budgetCategory == null ? false : budgetCategory.isAddOnceInMonth,
                                   cc.budgetTypeId
                               });
                    //var obj = (from cc in context.tblExpectedBudgetLines
                    //           join cct in context.tblcompanyCategories on cc.categoryId equals cct.categoryId
                    //           join ty in context.tblCategories on cc.categoryId equals ty.id
                    //           join mc in context.tblCategories on ty.parentId equals mc.id
                    //           where ty.parentId == cid
                    //           //where cc.companyId == CompanyId
                    //           //where cc.isActive == true
                    //           && cct.companyId == CompanyId
                    //           && cc.companyId == CompanyId
                    //           //&& (cc.date <= date && cc.reportType < 3) || (cc.reportType == 3 && cc.date.Year == date.Year)
                    //           && ((cc.date <= date && cc.reportType < 3) || (cc.reportType == 3 && cc.date.Year == date.Year))
                    //           && cct.isActive == true
                    //           select new
                    //           {
                    //               ty.id,
                    //               ty.name,
                    //               cc.budget,
                    //               cc.reportType,
                    //               cc.isAddOnceInMonth,
                    //               cc.budgetTypeId
                    //           }).Distinct().ToList();

                    //var obj = (from cc in context.tblCategories
                    //           join cct in context.tblcompanyCategories on cc.id equals cct.categoryId
                    //           join ebl in context.tblExpectedBudgetLines on cc.id equals ebl.categoryId
                    //           into category
                    //           from budgetCategory in category.DefaultIfEmpty()
                    //           where cc.parentId == cid                             
                    //           && cct.companyId == CompanyId
                    //           && cc.isActive == true
                    //           && (budgetCategory == null || ((budgetCategory.date <= date && budgetCategory.reportType < 3) || (budgetCategory.reportType == 3 && budgetCategory.date.Year == date.Year)))
                    //           select new
                    //           {
                    //               cc.id,
                    //               cc.name,
                    //               budget = budgetCategory == null ? 0 : budgetCategory.budget,
                    //               reportType = budgetCategory == null ? 1 : budgetCategory.reportType,
                    //               isAddOnceInMonth = budgetCategory == null ? false : budgetCategory.isAddOnceInMonth,                                  
                    //               cc.budgetTypeId
                    //           }).Distinct().ToList();

                    var obj = (from cc in context.tblCategories
                               join cct in context.tblcompanyCategories on cc.id equals cct.categoryId                               
                               where cc.parentId == cid
                               && cct.companyId == CompanyId
                               && cc.isActive == true                               
                               select new
                               {
                                   cc.id,
                                   cc.name,                                  
                                   cc.budgetTypeId
                               }).Distinct().ToList();
                    foreach (var item in obj)
                    {
                        var lastBudget = (from cc in context.tblExpectedBudgetLines
                                          where cc.categoryId == item.id
                                          select new
                                          {
                                              cc.budget,
                                              cc.reportType,
                                              cc.isAddOnceInMonth,
                                              cc.createDate

                                          }).OrderByDescending(x=>x.createDate).FirstOrDefault();

                        var mycatTypeList = new MyParameterList();
                        mycatTypeList.name = item.name;
                        mycatTypeList.id = item.id;
                        mycatTypeList.budgetTypeId = item.budgetTypeId;
                        if (lastBudget != null)
                        {
                            mycatTypeList.budget = lastBudget.budget;
                            mycatTypeList.reportType = lastBudget.reportType;
                            mycatTypeList.isAddOnceInMonth = lastBudget.isAddOnceInMonth.HasValue ? lastBudget.isAddOnceInMonth.HasValue : false;
                            mycatTypeList.SelectedreportTypeId = lastBudget.reportType;
                        }
                        else
                        {
                            mycatTypeList.budget = 0;
                            mycatTypeList.reportType = 1;
                            mycatTypeList.isAddOnceInMonth = false;
                            mycatTypeList.SelectedreportTypeId = 1;
                        }
                        categoryTypeList.Add(mycatTypeList);
                        //if (checkOnceInmonth(item.id, CompanyId, date) > 0)
                        //{
                        //    if (HaveentryInOnceInmonth(item.id, CompanyId, date) == 0)
                        //    {
                        //        categoryTypeList.Add(mycatTypeList);
                        //    }
                        //}
                        //else
                        //{
                        //    categoryTypeList.Add(mycatTypeList);
                        //}
                    }

                    //if (obj.Any())
                    //{

                    //    for (int i = 0; i < obj.Count; i++)
                    //    {
                    //        var mycatTypeList = new MyParameterList();
                    //        mycatTypeList.name = obj[i].name;
                    //        mycatTypeList.id = obj[i].id;
                    //        mycatTypeList.budget = obj[i].budget;
                    //        mycatTypeList.reportType = obj[i].reportType;
                    //        mycatTypeList.budgetTypeId = obj[i].budgetTypeId;
                    //        mycatTypeList.isAddOnceInMonth = obj[i].isAddOnceInMonth.HasValue? obj[i].isAddOnceInMonth.HasValue:false;
                    //        mycatTypeList.SelectedreportTypeId = obj[i].reportType;
                    //        if (checkOnceInmonth(obj[i].id, CompanyId, date) > 0)
                    //        {
                    //            if (HaveentryInOnceInmonth(obj[i].id, CompanyId, date) == 0)
                    //            {
                    //                categoryTypeList.Add(mycatTypeList);
                    //            }
                    //        }
                    //        else
                    //        {
                    //            categoryTypeList.Add(mycatTypeList);
                    //        }
                    //        //var catTypeList = new CategoryList();
                    //        //catTypeList.name = obj[i].name;
                    //        //catTypeList.id = obj[i].id;
                    //        //categoryTypeList.Add(catTypeList);
                    //    }
                    //}
                }
                return categoryTypeList;
            }
            catch (Exception e)
            {
                throw e;
            }

        }

        public int checkOnceInmonth(int CategoryId, int CompanyId,DateTime date)
        {
            try
            {
                using (managementsoftwaredbEntities context = new managementsoftwaredbEntities())
                {
                    //check for username isExit or Not
                    //var ut = context.tblExpectedBudgetLines.Where(x => x.companyId == CompanyId && x.categoryId == CategoryId && x.isAddOnceInMonth == true).SingleOrDefault();
                    //if (ut != null)
                    //{
                    //    return ut.id;
                    //}
                    //else
                    //{
                    //    return 0;
                    //}
                    var ut = (from db in context.tblExpectedBudgetLines
                              where db.companyId == CompanyId
                              && db.categoryId == CategoryId
                              && (db.date <= date && db.reportType < 3)
                              && db.isAddOnceInMonth == true
                              select db).OrderByDescending(x => x.date).FirstOrDefault();

                    if(ut==null)
                         ut = (from db in context.tblExpectedBudgetLines
                                  where db.companyId == CompanyId
                                  && db.categoryId == CategoryId
                                  && (db.reportType == 3 && db.date.Year == date.Year)
                               && db.isAddOnceInMonth == true
                                  select db).OrderByDescending(x => x.date).FirstOrDefault();
                    if (ut!=null)
                    {
                        return 1;
                    }
                    else
                    {
                        return 0;
                    }

                }
            }
            catch (Exception)
            {
                return 0;
            }
        }
        public int checkOnceInmonth(int CategoryId, int CompanyId)
        {
            try
            {
                using (managementsoftwaredbEntities context = new managementsoftwaredbEntities())
                {
                    //check for username isExit or Not
                    //var ut = context.tblExpectedBudgetLines.Where(x => x.companyId == CompanyId && x.categoryId == CategoryId && x.isAddOnceInMonth == true).SingleOrDefault();
                    //if (ut != null)
                    //{
                    //    return ut.id;
                    //}
                    //else
                    //{
                    //    return 0;
                    //}
                    var ut = (from db in context.tblExpectedBudgetLines
                              where db.companyId == CompanyId
                              && db.categoryId == CategoryId
                              && db.isAddOnceInMonth == true
                              select db).ToList();

                   
                    if (ut.Any())
                    {
                        return 1;
                    }
                    else
                    {
                        return 0;
                    }

                }
            }
            catch (Exception)
            {
                return 0;
            }
        }
        //public int checkOnceInmonth(int CategoryId, int CompanyId,DateTime date)
        //{
        //    try
        //    {
        //        using (managementsoftwaredbEntities context = new managementsoftwaredbEntities())
        //        {
        //            //check for username isExit or Not
        //            //var ut = context.tblExpectedBudgetLines.Where(x => x.companyId == CompanyId && x.categoryId == CategoryId && x.isAddOnceInMonth == true).SingleOrDefault();
        //            //if (ut != null)
        //            //{
        //            //    return ut.id;
        //            //}
        //            //else
        //            //{
        //            //    return 0;
        //            //}
        //            var ut = (from db in context.tblExpectedBudgetLines
        //                      where db.companyId == CompanyId
        //                      where db.categoryId == CategoryId
        //                      where db.date.Month == date.Month
        //                      where db.isAddOnceInMonth == true
        //                      select db).ToList();
        //            if (ut.Any())
        //            {
        //                return 1;
        //            }
        //            else
        //            {
        //                return 0;
        //            }

        //        }
        //    }
        //    catch (Exception)
        //    {
        //        return 0;
        //    }
        //}
        public decimal HaveentryInOnceInmonth(int CategoryId, int CompanyId, DateTime date)
        {
            try
            {
                using (managementsoftwaredbEntities context = new managementsoftwaredbEntities())
                {
                    //check for username isExit or Not
                    // var ut = context.tblExpectedBudgetLines.Where(x => x.date.Contains(model.date)).SingleOrDefault();
                    //var ut = context.tblRealBudgets.Where(x => x.companyId == CompanyId && x.categoryId == CategoryId && x.date.Month == date.Month).SingleOrDefault();
                    //if (ut != null)
                    //{
                    //    return ut.id;
                    //}
                    //else
                    //{
                    //    return 0;
                    //}
                    var ut = (from db in context.tblRealBudgets
                              where db.companyId == CompanyId
                              && db.categoryId == CategoryId
                              && db.date.Month == date.Month
                              && db.date.Year == date.Year
                              select db).ToList();
                    if (ut.Any())
                    {
                        return ut.FirstOrDefault().budget;
                    }
                    else
                    {
                        return 0;
                    }

                }
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
        public List<MyParameterList> EditMyCategoryValueFetch(int cid, int CompanyId, DateTime date)
        {

            try
            {
                var categoryTypeList = new List<MyParameterList>();
                // var categoryTypeList = new List<CategoryList>();
                using (managementsoftwaredbEntities context = new managementsoftwaredbEntities())
                {
                    //var obj = (from db in context.tblCategories where db.parentId == cid
                    //           where db.isActive == true
                    //           select db).ToList();

                    //-----------COMMENTED ON 28-12-2016   --------------------------------------
                    //var obj = (from cc in context.tblRealBudgets
                    //           join ty in context.tblCategories on cc.categoryId equals ty.id
                    //           join mc in context.tblCategories on ty.parentId equals mc.id
                    //           where ty.parentId == cid
                    //           where cc.companyId == CompanyId
                    //           where cc.date == date
                    //           where cc.isActive == true
                    //           select new
                    //           {
                    //               ty.id,
                    //               ty.name,
                    //               cc.budget,
                    //               mybudgetId = cc.id
                    //           }).ToList();

                    //if (obj.Any())
                    //{

                    //    for (int i = 0; i < obj.Count; i++)
                    //    {
                    //        var mycatTypeList = new MyParameterList();
                    //        mycatTypeList.name = obj[i].name;
                    //        mycatTypeList.id = obj[i].id;
                    //        mycatTypeList.budget = obj[i].budget;
                    //        mycatTypeList.MyBudgetid = obj[i].mybudgetId;
                    //        categoryTypeList.Add(mycatTypeList);
                    //        //var catTypeList = new CategoryList();
                    //        //catTypeList.name = obj[i].name;
                    //        //catTypeList.id = obj[i].id;
                    //        //categoryTypeList.Add(catTypeList);
                    //    }
                    //}
                    //--------------------END COMMENT ON 28-12-2016   --------------------

                    // ------------------- ADDED FROR FIELDS NOT COMING IN EDIT --------------------
                    var allcat = (from ccc in context.tblExpectedBudgetLines
                                  join tyc in context.tblCategories on ccc.categoryId equals tyc.id
                                  join mcc in context.tblCategories on tyc.parentId equals mcc.id
                                  where tyc.parentId == cid
                                  && ccc.companyId == CompanyId
                                  && ((ccc.date <= date && ccc.reportType <3 ) || (ccc.reportType == 3 && ccc.date.Year == date.Year))
                                  && ccc.isActive == true
                                  select new
                                  {
                                      tyc.id,
                                      tyc.name,
                                      ccc.budget,
                                      BudegtLineId = ccc.id
                                  }).GroupBy(row => new{ id = row.id, name = row.name }
                                  ).Select(x => new
                                  {
                                      id = x.Key.id,
                                      name = x.Key.name,
                                      budget=x.Max(z => z.budget),
                                      BudegtLineId = x.Max(z => z.BudegtLineId)

                                  }).ToList();

                    //if (!allcat.Any())
                    //{
                    //     allcat = (from ccc in context.tblExpectedBudgetLines
                    //                  join tyc in context.tblCategories on ccc.categoryId equals tyc.id
                    //                  join mcc in context.tblCategories on tyc.parentId equals mcc.id
                    //                  where tyc.parentId == cid
                    //                  where ccc.companyId == CompanyId
                    //                  where (ccc.date <= date && ccc.reportType < 3) 
                    //                  where ccc.isActive == true
                    //                  select new
                    //                  {
                    //                      tyc.id,
                    //                      tyc.name,
                    //                      ccc.budget,
                    //                      BudegtLineId = ccc.id
                    //                  }).ToList();
                    //}

                    //var allcat = (from db in context.tblCategories
                    //              where db.parentId == cid
                    //              where db.isActive == true
                    //              select new
                    //              {
                    //                  db.id,
                    //                  db.name
                    //              }).ToList();
                    if (allcat.Any())
                    {
                        for (int c = 0; c < allcat.Count; c++)
                        {
                            var CatId = allcat[c].id;
                            var obj = (from cc in context.tblRealBudgets
                                       join ty in context.tblCategories on cc.categoryId equals ty.id
                                       where cc.categoryId == CatId
                                       && cc.companyId == CompanyId
                                       && cc.date == date
                                       && cc.isActive == true
                                       select new
                                       {
                                           ty.id,
                                           ty.name,
                                           cc.budget,
                                           mybudgetId = cc.id
                                       }).ToList();

                            if (obj.Any())
                            {

                                for (int i = 0; i < obj.Count; i++)
                                {
                                    var mycatTypeList = new MyParameterList();
                                    mycatTypeList.name = obj[i].name;
                                    mycatTypeList.id = obj[i].id;
                                    mycatTypeList.budget = obj[i].budget;
                                    mycatTypeList.MyBudgetid = obj[i].mybudgetId;
                                    categoryTypeList.Add(mycatTypeList);
                                }
                            }
                            else
                            {
                                if (checkOnceInmonth(CatId, CompanyId, date) > 0)
                                {
                                    // 23-12-17 RGK
                                    // Change OnceMonth Check and add the item in both case. In caseof entry found in same month then make that Disabled
                                    decimal lastWeekValue = HaveentryInOnceInmonth(CatId, CompanyId, date);
                                    if (lastWeekValue == 0)
                                    {
                                        var cmycatTypeList = new MyParameterList();
                                        cmycatTypeList.name = allcat[c].name;
                                        cmycatTypeList.id = CatId;
                                        cmycatTypeList.budget = 0;
                                        cmycatTypeList.MyBudgetid = 0;
                                        categoryTypeList.Add(cmycatTypeList);
                                        cmycatTypeList.IsDisabled = false;
                                    }
                                    else
                                    {                                        
                                        var cmycatTypeList = new MyParameterList();
                                        cmycatTypeList.name = allcat[c].name;
                                        cmycatTypeList.id = CatId;
                                        cmycatTypeList.budget = 0;
                                        cmycatTypeList.MyBudgetid = 0;
                                        categoryTypeList.Add(cmycatTypeList);
                                        cmycatTypeList.IsDisabled = false;
                                        cmycatTypeList.IsDisabled = true;
                                        cmycatTypeList.budget = lastWeekValue;
                                    }
                                    //if (HaveentryInOnceInmonth(CatId, CompanyId, date) == 0)
                                    //{
                                    //    var cmycatTypeList = new MyParameterList();
                                    //    cmycatTypeList.name = allcat[c].name;
                                    //    cmycatTypeList.id = CatId;
                                    //    cmycatTypeList.budget = 0;
                                    //    cmycatTypeList.MyBudgetid = 0;
                                    //    categoryTypeList.Add(cmycatTypeList);
                                    //}
                                }
                                else
                                {
                                    var cmycatTypeList = new MyParameterList();
                                    cmycatTypeList.name = allcat[c].name;
                                    cmycatTypeList.id = CatId;
                                    cmycatTypeList.budget = 0;
                                    cmycatTypeList.MyBudgetid = 0;
                                    categoryTypeList.Add(cmycatTypeList);
                                }

                            }
                        }
                    }
                    // ------------------- END FROR FIELDS NOT COMING IN EDIT --------------------
                }
                return categoryTypeList;
            }
            catch (Exception e)
            {
                throw e;
            }

        }
        // <summary>
        ///  Bind dropdown of the category type in add product page.
        /// </summary>
        public List<SelectListItem> CategoryValueFetch(int cid, int CompanyId)
        {

            try
            {
                var categoryTypeList = new List<SelectListItem>();
                // var categoryTypeList = new List<CategoryList>();
                using (managementsoftwaredbEntities context = new managementsoftwaredbEntities())
                {
                    //var obj = (from db in context.tblCategories where db.parentId == cid
                    //           where db.isActive == true
                    //           select db).ToList();
                    var obj = (from cc in context.tblcompanyCategories
                               join ty in context.tblCategories on cc.categoryId equals ty.id
                               join mc in context.tblCategories on ty.parentId equals mc.id
                               where ty.parentId == cid
                               && cc.companyId == CompanyId
                               && cc.isActive == true
                               select new
                               {
                                   ty.id,
                                   ty.name,
                                   cc.isActive
                               }).ToList();

                    if (obj.Any())
                    {

                        for (int i = 0; i < obj.Count; i++)
                        {
                            var catTypeList = new SelectListItem();
                            catTypeList.Text = obj[i].name;
                            catTypeList.Value = obj[i].id.ToString();
                            categoryTypeList.Add(catTypeList);
                            //var catTypeList = new CategoryList();
                            //catTypeList.name = obj[i].name;
                            //catTypeList.id = obj[i].id;
                            //categoryTypeList.Add(catTypeList);
                        }
                    }
                }
                return categoryTypeList;
            }
            catch (Exception e)
            {
                throw e;
            }

        }

        public List<ReportTypeList> ReportTypefetch()
        {
            try
            {
                //var budget_TypeList = new List<SelectListItem>();
                var budget_TypeList = new List<ReportTypeList>();
                using (managementsoftwaredbEntities context = new managementsoftwaredbEntities())
                {
                    var obj = (from db in context.tblReportTypes
                               where db.isActive == true
                               select db).ToList();
                    if (obj.Any())
                    {

                        for (int i = 0; i < obj.Count; i++)
                        {
                            //var budget_Type_item = new SelectListItem();
                            //budget_Type_item.Text = obj[i].name;
                            //budget_Type_item.Value = obj[i].id.ToString();
                            //budget_TypeList.Add(budget_Type_item);
                            var budget_Type_item = new ReportTypeList();
                            budget_Type_item.name = obj[i].name;
                            budget_Type_item.id = obj[i].id;
                            budget_TypeList.Add(budget_Type_item);
                        }
                    }
                }
                return budget_TypeList;
            }
            catch (Exception e)
            {
                throw e;
            }

        }


        public bool AddBudgetLine(tblExpectedBudgetLine model)
        {
            try
            {
                using (managementsoftwaredbEntities context = new managementsoftwaredbEntities())
                {
                    //check for username isExit or Not
                    // var ut = context.tblExpectedBudgetLines.Where(x => x.date.Contains(model.date)).SingleOrDefault();
                    // var ut = context.tblExpectedBudgetLines.Where(x => x.companyId == model.companyId && x.categoryId == model.categoryId).SingleOrDefault();
                    if (!IsBudgetExists(model))
                    {
                        context.tblExpectedBudgetLines.Add(model);
                        context.SaveChanges();
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public bool AddBudgetLineNew(tblExpectedBudgetLine model)
        {
            try
            {
                using (managementsoftwaredbEntities context = new managementsoftwaredbEntities())
                {
                    //check for username isExit or Not
                    // var ut = context.tblExpectedBudgetLines.Where(x => x.date.Contains(model.date)).SingleOrDefault();
                    // var ut = context.tblExpectedBudgetLines.Where(x => x.companyId == model.companyId && x.categoryId == model.categoryId).SingleOrDefault();
                    
                        context.tblExpectedBudgetLines.Add(model);
                        context.SaveChanges();
                        return true;                    

                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public int getIdOfAdded(tblExpectedBudgetLine model)
        {
            int nReturn = 0;
            try
            {
                using (managementsoftwaredbEntities context = new managementsoftwaredbEntities())
                {
                    //check for username isExit or Not
                    // var ut = context.tblExpectedBudgetLines.Where(x => x.date.Contains(model.date)).SingleOrDefault();
                    List<tblExpectedBudgetLine> ut = null;

                    ut = context.tblExpectedBudgetLines.Where(x => x.companyId == model.companyId && x.categoryId == model.categoryId && x.budgetTypeId == model.budgetTypeId).ToList();
                    if (ut != null)
                    {
                        var ut1 = ut.Where(x => (x.date.Month == model.date.Month && x.date.Year == model.date.Year) || (x.reportType == 3 && x.date.Year == model.date.Year)).SingleOrDefault();
                        if (ut1 != null)
                        {
                            nReturn = ut1.id;
                        }
                    }


                }
            }
            catch (Exception)
            {
                // nReturn =  0;
            }
            return nReturn;
        }

        public bool AddMyBudgetLine(tblRealBudget model)
        {
            try
            {
                using (managementsoftwaredbEntities context = new managementsoftwaredbEntities())
                {
                    //check for username isExit or Not
                    int compId = Convert.ToInt32(model.companyId);
                    var ut = context.tblRealBudgets.Where(x => x.date.Equals(model.date) && x.companyId == compId && x.categoryId == model.categoryId).SingleOrDefault();
                    if (ut == null)
                    {
                        context.tblRealBudgets.Add(model);
                        context.SaveChanges();
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                }
            }
            catch (Exception e)
            {
                throw e;
            }
            //catch (Exception)
            //{
            //    return false;
            //}
        }
        public bool AddmyBudgetCarCounts(tblRealCarCount model)
        {
            try
            {
                using (managementsoftwaredbEntities context = new managementsoftwaredbEntities())
                {
                    //check for username isExit or Not
                    int compId = Convert.ToInt32(model.compnayId);
                    var ut = context.tblRealCarCounts.Where(x => x.compnayId == compId && x.date == model.date).SingleOrDefault();
                    if (ut == null)
                    {
                        context.tblRealCarCounts.Add(model);
                        context.SaveChanges();
                    }
                    return true;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            //catch (Exception)
            //{
            //    return false;
            //}
        }
        public bool EditmyBudgetCarCounts(tblRealCarCount model)
        {
            try
            {
                using (managementsoftwaredbEntities context = new managementsoftwaredbEntities())
                {
                    //check for username isExit or Not
                    int compId = Convert.ToInt32(model.compnayId);
                    var ut = context.tblRealCarCounts.Where(x => x.id == model.id).SingleOrDefault();
                    if (ut != null)
                    {
                        ut.date = model.date;
                        ut.compnayId = model.compnayId;
                        ut.carCounts = model.carCounts;
                        ut.id = model.id;
                        context.SaveChanges();
                    }
                    else
                    {
                        var utl = context.tblRealCarCounts.Where(p => p.compnayId == compId && p.date == model.date).SingleOrDefault();
                        if (utl == null)
                        {
                            context.tblRealCarCounts.Add(model);
                            context.SaveChanges();
                        }
                    }
                    return true;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            //catch (Exception)
            //{
            //    return false;
            //}
        }
        public bool UpdateBudgetLine(AddCompnayBudgetLine model, int id)
        {

            try
            {
                using (managementsoftwaredbEntities context = new managementsoftwaredbEntities())
                {
                    //var ut = context.tblExpectedBudgetLines.Where(x => x.companyId == model.companyId && x.categoryId == model.categoryValueId && x.id != id).SingleOrDefault();
                    if (!IsBudgetExists(model, model.id))
                    {
                        var query = context.tblExpectedBudgetLines.Where(x => x.id == id).FirstOrDefault();
                        var Checkquery = context.tblExpectedBudgetLines.Where(x => x.id == id && x.isAddOnceInMonth == true).FirstOrDefault();
                        if (query != null)
                        {
                            if (Checkquery == null && model.isAddOnceInMonth == true)
                            {
                                List<tblRealBudget> ADMinDElList = context.tblRealBudgets.Where(w => w.companyId == model.companyId && w.categoryId == model.categoryValueId).ToList<tblRealBudget>();
                                for (int j = 0; j < ADMinDElList.Count; j++)
                                {
                                    context.tblRealBudgets.Remove(ADMinDElList[j]);
                                    context.SaveChanges();
                                }
                            }
                            query.budget = model.budget;
                            query.budgetTypeId = model.SelectedBusgetTypeId;
                            query.categoryId = model.categoryValueId;
                            query.companyId = model.companyId;
                            query.date = model.date;
                            query.reportType = model.SelectedreportTypeId;
                            query.updateDate = model.updateDate;
                            query.isActive = Convert.ToBoolean(model.isActive);
                            query.isAddOnceInMonth = Convert.ToBoolean(model.isAddOnceInMonth);
                            context.SaveChanges();
                            return true;
                        }
                    }
                }
            }
            catch (Exception)
            {
                return false;
            }
            return false;
        }
        public bool UpdateMyBudget(tblRealBudget model, int id)
        {
            try
            {
                using (managementsoftwaredbEntities context = new managementsoftwaredbEntities())
                {
                    var query = context.tblRealBudgets.Where(x => x.id == id).FirstOrDefault();
                    if (query != null)
                    {
                        query.budget = model.budget;
                        //query.budgetTypeId = model.SelectedBusgetTypeId;
                        //query.categoryId = model.categoryValueId;
                        //query.companyId = model.companyId;
                        query.date = model.date;
                        //query.reportType = model.SelectedreportTypeId;
                        query.updateDate = model.updateDate;
                        // query.isActive = Convert.ToBoolean(model.isActive);
                        context.SaveChanges();
                        return true;
                    }
                    else
                    {
                        //check for username isExit or Not
                        //int compId = Convert.ToInt32(model.companyId);
                        //var ut = context.tblRealBudgets.Where(x => x.date.Equals(model.date) && x.companyId == compId && x.categoryId == model.categoryId).SingleOrDefault();
                        //if (ut == null)
                        //{
                        //    context.tblRealBudgets.Add(model);
                        //    context.SaveChanges();
                        //    return true;
                        //}
                        using (managementsoftwaredbEntities contextaa = new managementsoftwaredbEntities())
                        {
                            //check for username isExit or Not
                            int compId = Convert.ToInt32(model.companyId);
                            var ut = context.tblRealBudgets.Where(x => x.date.Equals(model.date) && x.companyId == compId && x.categoryId == model.categoryId).SingleOrDefault();
                            if (ut == null)
                            {
                                if (model.categoryId==6 || model.budget > 0)
                                {
                                    contextaa.tblRealBudgets.Add(model);
                                    contextaa.SaveChanges();
                                }
                                return true;
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
                return false;
            }
            return false;
        }
        public string BudgetLineDelete(int id)
        {
            try
            {
                using (managementsoftwaredbEntities context = new managementsoftwaredbEntities())
                {
                    tblExpectedBudgetLine role = context.tblExpectedBudgetLines.Find(id);
                    context.tblExpectedBudgetLines.Remove(role);
                    context.SaveChanges();
                }
                return "Budget Line deleted successfully..";
            }
            catch (Exception e)
            {
                throw e;
            }
            finally { }
        }
        public string BudgetLineDeleteWithrealBudget(int id, int companyId)
        {
            try
            {
                using (managementsoftwaredbEntities context = new managementsoftwaredbEntities())
                {
                    tblExpectedBudgetLine role = context.tblExpectedBudgetLines.Find(id);
                    //List<tblRealBudget> ADMinDElList = context.tblRealBudgets.Where(w => w.companyId == companyId && w.categoryId == role.categoryId).ToList<tblRealBudget>();
                    //for (int j = 0; j < ADMinDElList.Count; j++)
                    //{
                    //    context.tblRealBudgets.Remove(ADMinDElList[j]);
                    //    context.SaveChanges();
                    //}
                    context.tblExpectedBudgetLines.Remove(role);
                    context.SaveChanges();
                }
                return "Budget Line deleted successfully..";
            }
            catch (Exception e)
            {
                throw e;
            }
            finally { }
        }
        public string MyBudgetDelete(int CompanyId, DateTime date)
        {
            try
            {
                //using (managementsoftwaredbEntities context = new managementsoftwaredbEntities())
                //{
                //    tblRealBudget role = context.tblRealBudgets.Find(id);
                //    context.tblRealBudgets.Remove(role);
                //    context.SaveChanges();
                //}
                using (managementsoftwaredbEntities context = new managementsoftwaredbEntities())
                {
                    List<tblRealBudget> ADMinDElList = context.tblRealBudgets.Where(w => w.companyId == CompanyId && w.date == date).ToList<tblRealBudget>();
                    for (int j = 0; j < ADMinDElList.Count; j++)
                    {
                        context.tblRealBudgets.Remove(ADMinDElList[j]);
                        context.SaveChanges();
                    }
                }
                return "Your selected Budget deleted successfully..";
            }
            catch (Exception e)
            {
                throw e;
            }
            finally { }
        }
        public int MyBudgetCarCount(int CompanyId, DateTime date)
        {
            try
            {
                int CarCounts = 0;
                using (managementsoftwaredbEntities context = new managementsoftwaredbEntities())
                {
                    var obj = (from db in context.tblRealCarCounts
                               where db.compnayId == CompanyId
                               && db.date == date
                               select db).ToList();
                    if (obj.Any())
                    {
                        CarCounts = Convert.ToInt32(obj[0].carCounts);
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
        public int MyBudgetCarCountID(int CompanyId, DateTime date)
        {
            try
            {
                int CarCountID = 0;
                using (managementsoftwaredbEntities context = new managementsoftwaredbEntities())
                {
                    var obj = (from db in context.tblRealCarCounts
                               where db.compnayId == CompanyId
                               && db.date == date
                               select db).ToList();
                    if (obj.Any())
                    {
                        CarCountID = Convert.ToInt32(obj[0].id);
                    }
                }
                return CarCountID;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally { }
        }
        public PagedListModel<CompanyAccountModel> ViewAllBudgetLineList(string filter, int page, int pageSize, string sort, string sortdir, int cid)
        {
            var records = new PagedListModel<CompanyAccountModel>();
            try
            {
                using (managementsoftwaredbEntities context = new managementsoftwaredbEntities())
                {
                    records.Content = (from pd in context.tblExpectedBudgetLines
                                       join ct in context.tblCategories on pd.categoryId equals ct.id
                                       join bt in context.tblBudgetTypes on pd.budgetTypeId equals bt.id
                                       join rt in context.tblReportTypes on pd.reportType equals rt.id
                                       where pd.companyId == cid
                                       select new CompanyAccountModel
                                       {
                                           id = pd.id,
                                           budgetTypeId = pd.budgetTypeId,
                                           budgetType = bt.name,
                                           categoryId = pd.categoryId,
                                           category = ct.name,
                                           // companyId = Convert.ToInt32(pd.companyId),
                                           companyId = pd.companyId.Value,
                                           date = pd.date,
                                           reportType = pd.reportType,
                                           reportName = rt.name,
                                           budget = pd.budget,
                                           createDate = pd.createDate,
                                           // isActive = Convert.ToBoolean(pd.isActive)
                                           isActive = pd.isActive.Value
                                       }).Where(x => filter == null || (x.category.Contains(filter))).OrderBy(sort + " " + sortdir).Skip((page - 1) * pageSize).Take(pageSize).ToList();
                    records.TotalRecords = (from pd in context.tblExpectedBudgetLines
                                            join ct in context.tblCategories on pd.categoryId equals ct.id
                                            join bt in context.tblBudgetTypes on pd.budgetTypeId equals bt.id
                                            join rt in context.tblReportTypes on pd.reportType equals rt.id
                                            where pd.companyId == cid
                                            select new CompanyAccountModel
                                            {
                                                id = pd.id,
                                                budgetTypeId = pd.budgetTypeId,
                                                budgetType = bt.name,
                                                categoryId = pd.categoryId,
                                                category = ct.name,
                                                // companyId = Convert.ToInt32(pd.companyId),
                                                companyId = pd.companyId.Value,
                                                date = pd.date,
                                                reportType = pd.reportType,
                                                reportName = rt.name,
                                                budget = pd.budget,
                                                createDate = pd.createDate,
                                                //  isActive = Convert.ToBoolean(pd.isActive)
                                                isActive = pd.isActive.Value
                                            }).Where(x => filter == null || (x.category.Contains(filter))).OrderBy(sort + " " + sortdir).Count();
                    records.CurrentPage = page;
                    records.PageSize = pageSize;

                    return records;


                }
            }
            catch (Exception e)
            {
                throw e;
            }
            finally { }
        }

        public PagedListModel<CompanyAccountModel> ViewAllMyBudgetList(string filter, int page, int pageSize, string sort, string sortdir, int cid)
        {
            var records = new PagedListModel<CompanyAccountModel>();
            try
            {
                using (managementsoftwaredbEntities context = new managementsoftwaredbEntities())
                {
                    records.Content = (from pd in context.tblRealBudgets
                                       join ct in context.tblCategories on pd.categoryId equals ct.id
                                       join bt in context.tblBudgetTypes on pd.budgetTypeId equals bt.id
                                       where pd.companyId == cid
                                       select new CompanyAccountModel
                                       {
                                           id = pd.id,
                                           budgetTypeId = pd.budgetTypeId,
                                           budgetType = bt.name,
                                           categoryId = pd.categoryId,
                                           category = ct.name,
                                           // companyId = Convert.ToInt32(pd.companyId),
                                           companyId = pd.companyId.Value,
                                           date = pd.date,
                                           budget = pd.budget,
                                           createDate = pd.createDate,
                                           // isActive = Convert.ToBoolean(pd.isActive)
                                           isActive = pd.isActive.Value
                                       }).Where(x => filter == null).OrderBy(sort + " " + sortdir).Skip((page - 1) * pageSize).Take(pageSize).ToList();
                    records.TotalRecords = (from pd in context.tblRealBudgets
                                            join ct in context.tblCategories on pd.categoryId equals ct.id
                                            join bt in context.tblBudgetTypes on pd.budgetTypeId equals bt.id
                                            select new CompanyAccountModel
                                            {
                                                id = pd.id,
                                                budgetTypeId = pd.budgetTypeId,
                                                budgetType = bt.name,
                                                categoryId = pd.categoryId,
                                                category = ct.name,
                                                // companyId = Convert.ToInt32(pd.companyId),
                                                companyId = pd.companyId.Value,
                                                date = pd.date,
                                                budget = pd.budget,
                                                createDate = pd.createDate,
                                                //  isActive = Convert.ToBoolean(pd.isActive)
                                                isActive = pd.isActive.Value
                                            }).Where(x => filter == null).OrderBy(sort + " " + sortdir).Count();
                    records.CurrentPage = page;
                    records.PageSize = pageSize;

                    return records;


                }
            }
            catch (Exception e)
            {
                throw e;
            }
            finally { }
        }

        public PagedListModel<Mybudget> ViewDateofMyBudgetList(string filter, int page, int pageSize, string sort, string sortdir, int cid)
        {
            var records = new PagedListModel<Mybudget>();
            try
            {
                using (managementsoftwaredbEntities context = new managementsoftwaredbEntities())
                {
                    records.Content = (from pd in context.tblRealBudgets
                                       where pd.companyId == cid
                                       select new Mybudget
                                       {
                                           date = pd.date

                                       }).Where(x => filter == null).Distinct().OrderBy(sort + " " + sortdir).Skip((page - 1) * pageSize).Take(pageSize).ToList();
                    records.TotalRecords = (from pd in context.tblRealBudgets
                                            where pd.companyId == cid
                                            select new Mybudget
                                            {
                                                date = pd.date

                                            }).Where(x => filter == null).Distinct().OrderBy(sort + " " + sortdir).Count();
                    records.CurrentPage = page;
                    records.PageSize = pageSize;

                    return records;


                }
            }
            catch (Exception e)
            {
                throw e;
            }
            finally { }
        }
        public bool IsCurrentMonthBudgetExists(DateTime date, int companyId)
        {
            bool bReturn = false;

            try
            {
                using (managementsoftwaredbEntities context = new managementsoftwaredbEntities())
                {
                    //check for username isExit or Not
                    // var ut = context.tblExpectedBudgetLines.Where(x => x.date.Contains(model.date)).SingleOrDefault();
                    List<tblExpectedBudgetLine> ut = null;                   
                    ut = context.tblExpectedBudgetLines.Where(x => x.companyId == companyId ).ToList();
                    
                    if (ut != null)
                    {
                        var ut1 = ut.Where(x => (x.date.Month == date.Month && x.date.Year == date.Year)).SingleOrDefault();
                        if (ut1 != null)
                            bReturn = true;

                    }


                }
            }
            catch (Exception)
            {
                return false;
            }
            return bReturn;
        }

        #region "Private Methods"
        private bool IsBudgetExists(tblExpectedBudgetLine model, int id = -1)
        {
            bool bReturn = false;

            try
            {
                using (managementsoftwaredbEntities context = new managementsoftwaredbEntities())
                {
                    //check for username isExit or Not
                    // var ut = context.tblExpectedBudgetLines.Where(x => x.date.Contains(model.date)).SingleOrDefault();
                    List<tblExpectedBudgetLine> ut = null;
                    if (id != -1)
                        ut = context.tblExpectedBudgetLines.Where(x => x.id != model.id && x.companyId == model.companyId && x.categoryId == model.categoryId && x.budgetTypeId == model.budgetTypeId).ToList();
                    else
                        ut = context.tblExpectedBudgetLines.Where(x => x.companyId == model.companyId && x.categoryId == model.categoryId && x.budgetTypeId == model.budgetTypeId).ToList();
                    if (ut != null)
                    {
                        var ut1 = ut.Where(x => (x.date.Month == model.date.Month && x.date.Year == model.date.Year) || (x.reportType == 3 && x.date.Year == model.date.Year)).SingleOrDefault();
                        if (ut1 != null)
                            bReturn = true;

                    }


                }
            }
            catch (Exception)
            {
                return false;
            }
            return bReturn;
        }
        private bool IsBudgetExists(AddCompnayBudgetLine model, int id = -1)
        {
            bool bReturn = false;

            try
            {
                using (managementsoftwaredbEntities context = new managementsoftwaredbEntities())
                {
                    //check for username isExit or Not
                    // var ut = context.tblExpectedBudgetLines.Where(x => x.date.Contains(model.date)).SingleOrDefault();
                    List<tblExpectedBudgetLine> ut = null;
                    if (id != -1)
                        ut = context.tblExpectedBudgetLines.Where(x => x.id != model.id && x.companyId == model.companyId && x.categoryId == model.categoryId && x.budgetTypeId == model.budgetTypeId).ToList();
                    else
                        ut = context.tblExpectedBudgetLines.Where(x => x.companyId == model.companyId && x.categoryId == model.categoryId && x.budgetTypeId == model.budgetTypeId).ToList();
                    if (ut != null)
                    {
                        var ut1 = ut.Where(x => (x.date.Month == model.date.Month && x.date.Year == model.date.Year) || (x.reportType == 3 && x.date.Year == model.date.Year)).SingleOrDefault();
                        if (ut1 != null)
                            bReturn = true;

                    }


                }
            }
            catch (Exception)
            {
                return false;
            }
            return bReturn;
        }
        #endregion "Private Methods"
    }
}