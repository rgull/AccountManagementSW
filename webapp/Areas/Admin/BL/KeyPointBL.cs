using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SmartAdminMvc.Models;
using SmartAdminMvc.Areas.Admin.Models;
using System.Linq.Dynamic;

namespace SmartAdminMvc.Areas.Admin.BL
{
    public class KeyPointBL
    {
        public PagedListModel<tblKeyPointGroup> ShowAllKeyPointGroups(string filter, int page, int pageSize, string sort, string sortdir,int cid)
        {
            var records = new PagedListModel<tblKeyPointGroup>();
            try
            {
                using (managementsoftwaredbEntities context = new managementsoftwaredbEntities())
                {
                    //var obj = (from db in context.telerik_Roles
                    //           select db).ToList();
                    //return obj;
                    records.Content = (from db in context.tblKeyPointGroups where db.companyId == cid
                                       select db).Where(x => filter == null || (x.name.Contains(filter))).OrderBy(sort + " " + sortdir).Skip((page - 1) * pageSize).Take(pageSize).ToList();
                    records.TotalRecords = (from db in context.tblKeyPointGroups where db.companyId == cid
                                            select db).Where(x => filter == null || (x.name.Contains(filter))).OrderBy(sort + " " + sortdir).Count();
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

        public PagedListModel<tblKeyPointGroup> DefaultShowAllKeyPointGroups(string filter, int page, int pageSize, string sort, string sortdir)
        {
            var records = new PagedListModel<tblKeyPointGroup>();
            try
            {
                using (managementsoftwaredbEntities context = new managementsoftwaredbEntities())
                {
                    //var obj = (from db in context.telerik_Roles
                    //           select db).ToList();
                    //return obj;
                    records.Content = (from db in context.tblKeyPointGroups
                                       where db.companyId == null
                                       select db).Where(x => filter == null || (x.name.Contains(filter))).OrderBy(sort + " " + sortdir).Skip((page - 1) * pageSize).Take(pageSize).ToList();
                    records.TotalRecords = (from db in context.tblKeyPointGroups
                                            where db.companyId == null
                                            select db).Where(x => filter == null || (x.name.Contains(filter))).OrderBy(sort + " " + sortdir).Count();
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
        public bool AddKeyPoint(tblKeyPointGroup model)
        {
            try
            {
                using (managementsoftwaredbEntities context = new managementsoftwaredbEntities())
                {
                    //check for username isExit or Not
                    var ut = context.tblKeyPointGroups.Where(x => x.name.Contains(model.name) && x.companyId == model.companyId).SingleOrDefault();
                    if (ut == null)
                    {
                        context.tblKeyPointGroups.Add(model);
                        context.SaveChanges();
                    }
                    return true;
                }
            }
            catch (Exception ex)
            {
               // throw ex;
                return false;
            }
        }

        public bool CategorySelectAssignforKeyPoint(int kId, int CategoryId, DateTime? Cratedate, bool isactive)
        {
            try
            {
                using (managementsoftwaredbEntities context = new managementsoftwaredbEntities())
                {

                    tblKeyPointBudgetLine obj = new tblKeyPointBudgetLine();
                    obj.keypointId = kId;
                    obj.categoryId = CategoryId;
                    obj.creatDate = Cratedate;
                    obj.isActive = isactive;
                    context.tblKeyPointBudgetLines.Add(obj);
                    context.SaveChanges();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
            //  return "User role added successfully.";
        }

        public List<CategorywithtypeList> CategorytoselectforKeyPointlist(int cid)
        {
            try
            {
                // var budget_TypeList = new List<SelectListItem>();
                var mybudget_List = new List<CategorywithtypeList>();
                using (managementsoftwaredbEntities context = new managementsoftwaredbEntities())
                {
                    //var obj = (from db in context.tblBudgetTypes
                    //           join ct in context.tblCategories on db.id equals ct.budgetTypeId
                    //           join mct in context.tblCategories on ct.parentId equals mct.id
                    //           join cc in context.tblcompanyCategories on mct.id equals cc.categoryId
                    //           where ct.parentId == 0
                    //           where db.isActive == true
                    //           where cc.companyId == cid
                    var obj = (from cc in context.tblcompanyCategories
                               join mct in context.tblCategories on cc.categoryId equals mct.id
                               join ct in context.tblCategories on mct.parentId equals ct.id
                               join db in context.tblBudgetTypes on ct.budgetTypeId equals db.id
                               where ct.parentId == 0
                               where db.isActive == true
                               where cc.companyId == cid
                               select new
                               {
                                   budgetTypeId = db.id,
                                   budgetTypeName = db.name,
                                   categoryId = ct.id,
                                   categoryName = ct.name
                               }).Distinct().ToList();
                    if (obj.Any())
                    {

                        for (int i = 0; i < obj.Count; i++)
                        {
                            //var budget_Type_item = new SelectListItem();
                            //budget_Type_item.Text = obj[i].name;
                            //budget_Type_item.Value = obj[i].id.ToString();
                            //budget_TypeList.Add(budget_Type_item);
                            var mybudget_item = new CategorywithtypeList();
                            mybudget_item.budgetTypeId = obj[i].budgetTypeId;
                            mybudget_item.budgetTypeName = obj[i].budgetTypeName;
                            mybudget_item.categoryId = obj[i].categoryId;
                            mybudget_item.categoryName = obj[i].categoryName.ToString();
                            mybudget_item.CategoryValueList = CategoryforKeyPointValueFetch(obj[i].categoryId, cid);
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

        public List<CategorywithtypeList> DefaultCategorytoselectforKeyPointlist()
        {
            try
            {
                // var budget_TypeList = new List<SelectListItem>();
                var mybudget_List = new List<CategorywithtypeList>();
                using (managementsoftwaredbEntities context = new managementsoftwaredbEntities())
                {
                    //var obj = (from db in context.tblBudgetTypes
                    //           join ct in context.tblCategories on db.id equals ct.budgetTypeId
                    //           join mct in context.tblCategories on ct.parentId equals mct.id
                    //           join cc in context.tblcompanyCategories on mct.id equals cc.categoryId
                    //           where ct.parentId == 0
                    //           where db.isActive == true
                    //           where cc.companyId == cid
                    var obj = (from db in context.tblBudgetTypes
                               join ct in context.tblCategories on db.id equals ct.budgetTypeId
                               // join mct in context.tblCategories on ct.parentId equals mct.id
                               where ct.parentId == 0
                               where db.isActive == true
                               //var obj = (from cc in context.tblcompanyCategories
                               //           join mct in context.tblCategories on cc.categoryId equals mct.id
                               //           join ct in context.tblCategories on mct.parentId equals ct.id
                               //           join db in context.tblBudgetTypes on ct.budgetTypeId equals db.id
                               //           where ct.parentId == 0
                               //           where db.isActive == true
                               //           where cc.companyId == null
                               select new
                               {
                                   budgetTypeId = db.id,
                                   budgetTypeName = db.name,
                                   categoryId = ct.id,
                                   categoryName = ct.name
                               }).Distinct().ToList();
                    if (obj.Any())
                    {

                        for (int i = 0; i < obj.Count; i++)
                        {
                            //var budget_Type_item = new SelectListItem();
                            //budget_Type_item.Text = obj[i].name;
                            //budget_Type_item.Value = obj[i].id.ToString();
                            //budget_TypeList.Add(budget_Type_item);
                            var mybudget_item = new CategorywithtypeList();
                            mybudget_item.budgetTypeId = obj[i].budgetTypeId;
                            mybudget_item.budgetTypeName = obj[i].budgetTypeName;
                            mybudget_item.categoryId = obj[i].categoryId;
                            mybudget_item.categoryName = obj[i].categoryName.ToString();
                            mybudget_item.CategoryValueList = DefaultCategoryforKeyPointValueFetch(obj[i].categoryId);
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
        public List<CategoryValueselcet> CategoryforKeyPointValueFetch(int catid, int cid)
        {

            try
            {
                var categoryTypeList = new List<CategoryValueselcet>();
                // var categoryTypeList = new List<CategoryList>();
                using (managementsoftwaredbEntities context = new managementsoftwaredbEntities())
                {
                    //var obj = (from db in context.tblCategories where db.parentId == cid
                    //           where db.isActive == true
                    //           select db).ToList();
                    //var obj = (from mc in context.tblCategories
                    //           join cc in context.tblcompanyCategories on mc.id equals cc.categoryId
                    //           where mc.parentId == catid
                    //           where mc.isActive == true
                    var obj = (from cc in context.tblcompanyCategories
                               join mc in context.tblCategories on cc.categoryId equals mc.id
                               where mc.parentId == catid
                               where cc.companyId == cid
                               where mc.isActive == true
                               select new
                               {
                                   mc.id,
                                   mc.name

                               }).Distinct().ToList();

                    if (obj.Any())
                    {

                        for (int i = 0; i < obj.Count; i++)
                        {
                            var mycatTypeList = new CategoryValueselcet();
                            mycatTypeList.name = obj[i].name;
                            mycatTypeList.id = obj[i].id;
                            // mycatTypeList.selected = true;
                            categoryTypeList.Add(mycatTypeList);
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
        public List<CategoryValueselcet> DefaultCategoryforKeyPointValueFetch(int catid)
        {

            try
            {
                var categoryTypeList = new List<CategoryValueselcet>();
                // var categoryTypeList = new List<CategoryList>();
                using (managementsoftwaredbEntities context = new managementsoftwaredbEntities())
                {
                    //var obj = (from db in context.tblCategories where db.parentId == cid
                    //           where db.isActive == true
                    //           select db).ToList();
                    var obj = (from mc in context.tblCategories
                               join cc in context.tblcompanyCategories on mc.id equals cc.categoryId
                               where mc.parentId == catid
                               where mc.isActive == true
                    //var obj = (from cc in context.tblcompanyCategories
                    //           join mc in context.tblCategories on cc.categoryId equals mc.id
                    //           where mc.parentId == catid
                    //           where cc.companyId == null
                    //           where mc.isActive == true
                               select new
                               {
                                   mc.id,
                                   mc.name

                               }).Distinct().ToList();

                    if (obj.Any())
                    {

                        for (int i = 0; i < obj.Count; i++)
                        {
                            var mycatTypeList = new CategoryValueselcet();
                            mycatTypeList.name = obj[i].name;
                            mycatTypeList.id = obj[i].id;
                            // mycatTypeList.selected = true;
                            categoryTypeList.Add(mycatTypeList);
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
        public List<tblKeyPointBudgetLine> UserSelectAssignedCategoriesforKeyPoint(int keypointId)
        {
            try
            {
                using (managementsoftwaredbEntities context = new managementsoftwaredbEntities())
                {
                    var obj = (from db in context.tblKeyPointBudgetLines
                               where db.keypointId == keypointId
                               select db).ToList();
                    return obj;

                }
            }
            catch (Exception e)
            {
                throw e;
            }
            finally { }
        }

        public bool UpdateKeyPointGroup(addKeyPoint model, int id)
        {

            try
            {
                using (managementsoftwaredbEntities context = new managementsoftwaredbEntities())
                {
                    var query = context.tblKeyPointGroups.Where(x => x.id == id).FirstOrDefault();
                    if (query != null)
                    {
                        query.name = model.name;
                        query.updateDate = model.updateDate;
                        query.percentage = model.percentage;
                        query.isActive = Convert.ToBoolean(model.isActive);
                        query.isnetProfitKey = Convert.ToBoolean(model.isnetProfitKey);
                        query.isBusinessDevKey = Convert.ToBoolean(model.isBusinessDevKey);
                        context.SaveChanges();
                        return true;
                    }
                }
            }
            catch (Exception)
            {
                return false;
            }
            return false;
        }

        public string KeyPointSelectedCategoryDelete(int keyPointId)
        {
            try
            {
                using (managementsoftwaredbEntities context = new managementsoftwaredbEntities())
                {
                    List<tblKeyPointBudgetLine> ADMinDElList = context.tblKeyPointBudgetLines.Where(w => w.keypointId == keyPointId).ToList<tblKeyPointBudgetLine>();
                    for (int j = 0; j < ADMinDElList.Count; j++)
                    {
                        context.tblKeyPointBudgetLines.Remove(ADMinDElList[j]);
                        context.SaveChanges();
                    }
                    return "Delete Key Point successfully.";
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            finally { }

        }
        public string KeyPointGroupDelete(int id)
        {
            try
            {
                using (managementsoftwaredbEntities context = new managementsoftwaredbEntities())
                {
                    List<tblKeyPointBudgetLine> CompanyDElList = context.tblKeyPointBudgetLines.Where(w => w.keypointId == id).ToList<tblKeyPointBudgetLine>();
                    for (int j = 0; j < CompanyDElList.Count; j++)
                    {
                        context.tblKeyPointBudgetLines.Remove(CompanyDElList[j]);
                        context.SaveChanges();
                    }
                    tblKeyPointGroup role = context.tblKeyPointGroups.Find(id);
                    context.tblKeyPointGroups.Remove(role);
                    context.SaveChanges();
                }
                return "Key Point Group deleted successfully..";
            }
            catch (Exception e)
            {
                //throw e;
                return "Key Point Group Can not be deleted, It has data within.";
            }
            finally { }
        }
    }
}