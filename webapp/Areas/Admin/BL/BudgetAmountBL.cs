using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SmartAdminMvc.Models;
using SmartAdminMvc.Areas.Admin.Models;
using System.Linq.Dynamic;
using System.Web.Mvc;

namespace SmartAdminMvc.Areas.Admin.BL
{
    public class BudgetAmountBL
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
                               where db.parentId == 0
                               where db.isActive == true
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
        public List<adminBudgetTypeList> BudgetTypefetch()
        {
            try
            {
                // var budget_TypeList = new List<SelectListItem>();
                var budget_TypeList = new List<adminBudgetTypeList>();
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
                            var budget_Type_item = new adminBudgetTypeList();
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
                               where cc.companyId == CompanyId
                               where cc.isActive == true
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
        public List<SelectListItem> DefaultCategoryValueFetch(int cid)
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
                    var obj = (from ty in context.tblCategories 
                               join mc in context.tblCategories on ty.parentId equals mc.id
                               where ty.parentId == cid
                               where ty.isActive == true
                               select new
                               {
                                   ty.id,
                                   ty.name,
                                   ty.isActive
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
        public List<adminReportTypeList> ReportTypefetch()
        {
            try
            {
                //var budget_TypeList = new List<SelectListItem>();
                var budget_TypeList = new List<adminReportTypeList>();
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
                            var budget_Type_item = new adminReportTypeList();
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
                    //var ut = context.tblExpectedBudgetLines.Where(x => x.companyId.Equals(model.companyId)).Where(x => x.categoryId.Equals(model.categoryId)).SingleOrDefault();
                    var ut = context.tblExpectedBudgetLines.Where(x => x.companyId==model.companyId && x.categoryId == model.categoryId).SingleOrDefault();
                    if (ut == null)
                    {
                        context.tblExpectedBudgetLines.Add(model);
                        context.SaveChanges();
                    }
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }
        public bool UpdateBudgetLine(adminAddCompnayBudgetLine model, int id)
        {

            try
            {
                using (managementsoftwaredbEntities context = new managementsoftwaredbEntities())
                {
                    var ut = context.tblExpectedBudgetLines.Where(x => x.companyId == model.companyId && x.categoryId == model.categoryValueId && x.id != id).SingleOrDefault();
                    if (ut == null)
                    {
                        //var query = context.tblExpectedBudgetLines.Where(x => x.id == id).FirstOrDefault();
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
                            if (model.companyId == 0)
                            {
                                query.companyId = null;
                            }
                            else
                            {
                                query.companyId = model.companyId;
                            }
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
        public string BudgetLineDeleteWithrealBudget(int id,int companyId)
        {
            try
            {
                using (managementsoftwaredbEntities context = new managementsoftwaredbEntities())
                {
                    tblExpectedBudgetLine role = context.tblExpectedBudgetLines.Find(id);
                    List<tblRealBudget> ADMinDElList = context.tblRealBudgets.Where(w => w.companyId == companyId && w.categoryId == role.categoryId).ToList<tblRealBudget>();
                    for (int j = 0; j < ADMinDElList.Count; j++)
                    {
                        context.tblRealBudgets.Remove(ADMinDElList[j]);
                        context.SaveChanges();
                    }                    
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
        
    public PagedListModel<adminCompanyAccountModel> AdminViewAllBudgetLineList(string filter, int page, int pageSize, string sort, string sortdir,int cid)
        {
            var records = new PagedListModel<adminCompanyAccountModel>();
            try
            {
                using (managementsoftwaredbEntities context = new managementsoftwaredbEntities())
                {
                    records.Content = (from pd in context.tblExpectedBudgetLines
                                       join ct in context.tblCategories on pd.categoryId equals ct.id
                                       join bt in context.tblBudgetTypes on pd.budgetTypeId equals bt.id
                                       join rt in context.tblReportTypes on pd.reportType equals rt.id
                                       where pd.companyId==cid
                                       select new adminCompanyAccountModel
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
                                            select new adminCompanyAccountModel
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
        public PagedListModel<adminCompanyAccountModel> DefaultViewAllBudgetLineList(string filter, int page, int pageSize, string sort, string sortdir)
        {
            var records = new PagedListModel<adminCompanyAccountModel>();
            try
            {
                using (managementsoftwaredbEntities context = new managementsoftwaredbEntities())
                {
                    records.Content = (from pd in context.tblExpectedBudgetLines
                                       join ct in context.tblCategories on pd.categoryId equals ct.id
                                       join bt in context.tblBudgetTypes on pd.budgetTypeId equals bt.id
                                       join rt in context.tblReportTypes on pd.reportType equals rt.id
                                       where pd.companyId == null
                                       select new adminCompanyAccountModel
                                       {
                                           id = pd.id,
                                           budgetTypeId = pd.budgetTypeId,
                                           budgetType = bt.name,
                                           categoryId = pd.categoryId,
                                           category = ct.name,
                                           // companyId = Convert.ToInt32(pd.companyId),
                                          // companyId = pd.companyId.Value,
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
                                            where pd.companyId == null                                            select new adminCompanyAccountModel
                                            {
                                                id = pd.id,
                                                budgetTypeId = pd.budgetTypeId,
                                                budgetType = bt.name,
                                                categoryId = pd.categoryId,
                                                category = ct.name,
                                                // companyId = Convert.ToInt32(pd.companyId),
                                               // companyId = pd.companyId.Value,
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

    }
}