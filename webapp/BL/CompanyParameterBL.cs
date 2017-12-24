using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SmartAdminMvc.Models;
using System.Linq.Dynamic;
using System.Web.Mvc;

namespace SmartAdminMvc.BL
{
    public class CompanyParameterBL
    {
        public PagedListModel<ViewCompanyParameter> ShowParameters(string filter, int page, int pageSize, string sort, string sortdir,int CompanyId)
        {
            var records = new PagedListModel<ViewCompanyParameter>();
            try
            {
                using (managementsoftwaredbEntities context = new managementsoftwaredbEntities())
                {
                    records.Content = (from cc in context.tblcompanyCategories
                                       join db in context.tblCategories on cc.categoryId equals db.id
                                       join mc in context.tblCategories on db.parentId equals mc.id
                                       join bt in context.tblBudgetTypes on mc.budgetTypeId equals bt.id
                                       where cc.companyId == CompanyId
                                       where cc.isActive == true
                                       select new ViewCompanyParameter
                                       {
                                           Id = db.id,
                                           name = db.name,
                                           budgetTypeId = db.budgetTypeId,
                                           Categoryname =mc.name,
                                           parentId = db.parentId,
                                           BudgetTypeName = bt.name,
                                           createDate = db.createDate,
                                           isActive = cc.isActive.Value
                                       }).Where(x => filter == null || (x.name.Contains(filter))).OrderBy(sort + " " + sortdir).Skip((page - 1) * pageSize).Take(pageSize).ToList();
                    records.TotalRecords = (from cc in context.tblcompanyCategories
                                            join db in context.tblCategories on cc.categoryId equals db.id
                                            join mc in context.tblCategories on db.parentId equals mc.id
                                            join bt in context.tblBudgetTypes on mc.budgetTypeId equals bt.id
                                            where cc.companyId == CompanyId
                                            select new ViewCompanyParameter
                                            {
                                                Id = db.id,
                                                name = db.name,
                                                budgetTypeId = db.budgetTypeId,
                                                parentId = db.parentId,
                                                BudgetTypeName = bt.name,
                                                createDate = db.createDate,
                                                isActive = cc.isActive.Value
                                            }).Where(x => filter == null || (x.name.Contains(filter))).OrderBy(sort + " " + sortdir).Count();

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

        public List<SelectListItem> Categoryfetch()
        {
            try
            {
                var CategoryList = new List<SelectListItem>();
                using (managementsoftwaredbEntities context = new managementsoftwaredbEntities())
                {
                    var obj = (from db in context.tblCategories
                               where db.parentId == 0
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

        public List<SelectListItem> ParameterCategoryValueFetch(int cid,int companyId)
        {

            try
            {
                var categoryTypeList = new List<SelectListItem>();
                using (managementsoftwaredbEntities context = new managementsoftwaredbEntities())
                {
                    var obj = (from cc in context.tblcompanyCategories 
                               join ty in context.tblCategories on cc.categoryId equals  ty.id
                               join mc in context.tblCategories on ty.parentId equals mc.id
                               where ty.parentId == cid where cc.companyId == companyId
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
                            catTypeList.Selected = obj[i].isActive.Value;
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
        public bool UpdateCategory(CompanyParameter model, int id)
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

        public bool ParameterUpdate(int CompnayId, int CategoryId, int ParameterId, bool isactive)
        {

            try
            {
                using (managementsoftwaredbEntities context = new managementsoftwaredbEntities())
                {
                    var query = context.tblcompanyCategories.Where(x => x.companyId == CompnayId).Where(x => x.categoryId == ParameterId).FirstOrDefault();
                    if (query != null)
                    {
                        query.isActive = Convert.ToBoolean(isactive);
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
    }
}