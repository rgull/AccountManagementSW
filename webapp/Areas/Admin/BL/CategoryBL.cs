using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SmartAdminMvc.Models;
using SmartAdminMvc.Areas.Admin.Models;
using System.Data;
using System.Linq.Dynamic;
using System.Web.Mvc;

namespace SmartAdminMvc.Areas.Admin.BL
{
    public class CategoryBL
    {
        /// <summary>
        ///  Add new user.
        /// </summary>
        public bool AddCategory(tblCategory model)
        {
            try
            {
                using (managementsoftwaredbEntities context = new managementsoftwaredbEntities())
                {
                    //check for username isExit or Not
                    var ut = context.tblCategories.Where(x => x.name.Contains(model.name)).SingleOrDefault();
                    if (ut == null)
                    {
                        context.tblCategories.Add(model);
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
        /// <summary>
        /// Edit user Details.
        /// </summary>
        public bool UpdateCategory(Category model, int id)
        {

            try
            {
                using (managementsoftwaredbEntities context = new managementsoftwaredbEntities())
                {
                    var query = context.tblCategories.Where(x => x.id == id).FirstOrDefault();
                    if (query != null)
                    {
                        query.name = model.name;
                        query.budgetTypeId = model.budgetTypeId;
                        query.parentId = model.parentId;
                        query.updateDate = model.updateDate;
                        query.isActive = Convert.ToBoolean(model.isActive);
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
        /// <summary>
        /// Delete user by userid.
        /// </summary>
        public string UserDelete(int id)
        {
            try
            {
                using (managementsoftwaredbEntities context = new managementsoftwaredbEntities())
                {
                    List<tblcompanyCategory> companyCategoryDElList = context.tblcompanyCategories.Where(w => w.categoryId == id).ToList<tblcompanyCategory>();
                    for (int j = 0; j < companyCategoryDElList.Count; j++)
                    {
                        context.tblcompanyCategories.Remove(companyCategoryDElList[j]);
                        context.SaveChanges();
                    }
                    List<tblExpectedBudgetLine> ExpectedBudgetLineDElList = context.tblExpectedBudgetLines.Where(w => w.categoryId == id).ToList<tblExpectedBudgetLine>();
                    for (int j = 0; j < ExpectedBudgetLineDElList.Count; j++)
                    {
                        context.tblExpectedBudgetLines.Remove(ExpectedBudgetLineDElList[j]);
                        context.SaveChanges();
                    }
                    List<tblRealBudget> RealBudgetDElList = context.tblRealBudgets.Where(w => w.categoryId == id).ToList<tblRealBudget>();
                    for (int j = 0; j < RealBudgetDElList.Count; j++)
                    {
                        context.tblRealBudgets.Remove(RealBudgetDElList[j]);
                        context.SaveChanges();
                    }
                    List<tblKeyPointBudgetLine> KeyPointBudgetLineDElList = context.tblKeyPointBudgetLines.Where(w => w.categoryId == id).ToList<tblKeyPointBudgetLine>();
                    for (int j = 0; j < KeyPointBudgetLineDElList.Count; j++)
                    {
                        context.tblKeyPointBudgetLines.Remove(KeyPointBudgetLineDElList[j]);
                        context.SaveChanges();
                    }               
                    tblCategory role = context.tblCategories.Find(id);
                    context.tblCategories.Remove(role);
                    context.SaveChanges();
                    List<tblCategory> CategoryDElList = context.tblCategories.Where(w => w.id == id).ToList<tblCategory>();
                    for (int j = 0; j < CategoryDElList.Count; j++)
                    {
                        context.tblCategories.Remove(CategoryDElList[j]);
                        context.SaveChanges();
                    }
                }
                return "Category delete successfully..";
            }
            catch (Exception e)
            {
                throw e;
            }
            finally { }
        }
        //public PagedListModel<tblCategory> ShowCategory(string filter, int page, int pageSize, string sort, string sortdir)
        //{
        //    var records = new PagedListModel<tblCategory>();
        //    try
        //    {
        //        using (AccountManagement_dbEntities context = new AccountManagement_dbEntities())
        //        {
        //            records.Content = (from db in context.tblCategories
        //                               select db).Where(x => x.parentId == 0)
        //                               .Where(x => filter == null || (x.name.Contains(filter))).OrderBy(sort + " " + sortdir).Skip((page - 1) * pageSize).Take(pageSize).ToList();
        //            records.TotalRecords = (from db in context.tblCategories
        //                                    select db).Where(x => x.parentId == 0)
        //                                    .Where(x => filter == null || (x.name.Contains(filter))).OrderBy(sort + " " + sortdir).Count();

        //            records.CurrentPage = page;
        //            records.PageSize = pageSize;
        //            return records;


        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        throw e;
        //    }
        //    finally { }
        //}

        public PagedListModel<ViewCategory> ShowCategory(string filter, int page, int pageSize, string sort, string sortdir)
        {
            var records = new PagedListModel<ViewCategory>();
            try
            {
                using (managementsoftwaredbEntities context = new managementsoftwaredbEntities())
                {
                    records.Content = (from db in context.tblCategories
                                       join bt in context.tblBudgetTypes on db.budgetTypeId equals bt.id
                                       select new ViewCategory
                                       {
                                           Id = db.id,
                                           name = db.name,
                                           budgetTypeId = db.budgetTypeId,
                                           parentId = db.parentId,
                                           BudgetTypeName = bt.name,
                                           createDate = db.createDate,
                                           isActive = db.isActive.Value
                                       }).Where(x => x.parentId == 0)
                                       .Where(x => filter == null || (x.name.Contains(filter))).OrderBy(sort + " " + sortdir).Skip((page - 1) * pageSize).Take(pageSize).ToList();
                    records.TotalRecords = (from db in context.tblCategories
                                            join bt in context.tblBudgetTypes on db.budgetTypeId equals bt.id
                                            select new ViewCategory
                                            {
                                                Id = db.id,
                                                name = db.name,
                                                budgetTypeId = db.budgetTypeId,
                                                parentId = db.parentId,
                                                BudgetTypeName = bt.name,
                                                createDate = db.createDate,
                                                isActive = db.isActive.Value
                                            }).Where(x => x.parentId == 0)
                                            .Where(x => filter == null || (x.name.Contains(filter))).OrderBy(sort + " " + sortdir).Count();

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




        //public PagedListModel<tblCategory> ShowCategoryValue(string filter, int page, int pageSize, string sort, string sortdir)
        //{
        //    var records = new PagedListModel<tblCategory>();
        //    try
        //    {
        //        using (AccountManagement_dbEntities context = new AccountManagement_dbEntities())
        //        {
        //            //var obj = (from db in context.telerik_Roles
        //            //           select db).ToList();
        //            //return obj;
        //            records.Content = (from db in context.tblCategories
        //                               select db).Where(x=> x.parentId != 0)
        //                               .Where(x => filter == null || (x.name.Contains(filter))).OrderBy(sort + " " + sortdir).Skip((page - 1) * pageSize).Take(pageSize).ToList();
        //            records.TotalRecords = (from db in context.tblCategories
        //                                    select db).Where(x => x.parentId != 0)
        //                                    .Where(x => filter == null || (x.name.Contains(filter))).OrderBy(sort + " " + sortdir).Count();
        //            records.CurrentPage = page;
        //            records.PageSize = pageSize;
        //            return records;


        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        throw e;
        //    }
        //    finally { }
        //}

        public PagedListModel<ViewCategory> ShowCategoryValue(string filter, int page, int pageSize, string sort, string sortdir)
        {
            var records = new PagedListModel<ViewCategory>();
            try
            {
                using (managementsoftwaredbEntities context = new managementsoftwaredbEntities())
                {
                    //var obj = (from db in context.telerik_Roles
                    //           select db).ToList();
                    //return obj;
                    records.Content = (from db in context.tblCategories
                                       join bt in context.tblBudgetTypes on db.budgetTypeId equals bt.id
                                       join ct in context.tblCategories on db.parentId equals ct.id
                                       select new ViewCategory {
                                           Id=db.id,
                                           Categoryname=ct.name,
                                           name =db.name,
                                           budgetTypeId=db.budgetTypeId,
                                           parentId = db.parentId,
                                           BudgetTypeName = bt.name,
                                           createDate =db.createDate,
                                           isActive =db.isActive.Value
    }).Where(x => x.parentId != 0)
                                       .Where(x => filter == null || (x.name.Contains(filter))).OrderBy(sort + " " + sortdir).Skip((page - 1) * pageSize).Take(pageSize).ToList();
                    records.TotalRecords = (from db in context.tblCategories
                                            join bt in context.tblBudgetTypes on db.budgetTypeId equals bt.id
                                            join ct in context.tblCategories on db.parentId equals ct.id
                                            select new ViewCategory
                                            {
                                                Id = db.id,
                                                Categoryname = ct.name,
                                                name = db.name,
                                                budgetTypeId = db.budgetTypeId,
                                                parentId = db.parentId,
                                                BudgetTypeName = bt.name,
                                                createDate = db.createDate,
                                                isActive = db.isActive.Value
                                            }).Where(x => x.parentId != 0)
                                            .Where(x => filter == null || (x.name.Contains(filter))).OrderBy(sort + " " + sortdir).Count();
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
        /// <summary>
        ///  Bind dropdown of the category in add category page.
        /// </summary>
        public List<SelectListItem> Categoryfetch()
        {
            try
            {
                var CategoryList = new List<SelectListItem>();
                using (managementsoftwaredbEntities context = new managementsoftwaredbEntities())
                {
                    var obj = (from db in context.tblCategories where db.parentId == 0
                               where db.isActive == true
                               select db).ToList();
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
        public List<SelectListItem> BudgetTypefetch()
        {
            try
            {
                var budget_TypeList = new List<SelectListItem>();
                using (managementsoftwaredbEntities context = new managementsoftwaredbEntities())
                {
                    var obj = (from db in context.tblBudgetTypes
                               where db.isActive == true
                               select db).ToList();
                    if (obj.Any())
                    {

                        for (int i = 0; i < obj.Count; i++)
                        {
                            var budget_Type_item = new SelectListItem();
                            budget_Type_item.Text = obj[i].name;
                            budget_Type_item.Value = obj[i].id.ToString();
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
        // <summary>
        ///  Bind dropdown of the category type in add product page.
        /// </summary>
        public List<SelectListItem> CategoryValueFetch(int cid)
        {

            try
            {
                var categoryTypeList = new List<SelectListItem>();
                using (managementsoftwaredbEntities context = new managementsoftwaredbEntities())
                {
                    var obj = (from ty in context.tblCategories
                               join lk in context.tblBudgetTypes on ty.budgetTypeId equals lk.id
                               where lk.id == cid where ty.parentId==0
                               select new
                               {
                                   ty.id,
                                   ty.name
                               }).ToList();
                    if (obj.Any())
                    {

                        for (int i = 0; i < obj.Count; i++)
                        {
                            var catTypeList = new SelectListItem();
                            catTypeList.Text = obj[i].name;
                            catTypeList.Value = obj[i].id.ToString();
                            categoryTypeList.Add(catTypeList);
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
    }
}