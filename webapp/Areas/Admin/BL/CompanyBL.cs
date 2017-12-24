using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SmartAdminMvc.Models;
using SmartAdminMvc.Areas.Admin.Models;
using System.Linq.Dynamic;

namespace SmartAdminMvc.Areas.Admin.BL
{
    public class CompanyBL
    {
        /// <summary>
        ///  Add new user.
        /// </summary>
        public bool AddCompany(tblCompany model)
        {
            try
            {
                using (managementsoftwaredbEntities context = new managementsoftwaredbEntities())
                {
                    //check for username isExit or Not
                    var ut = context.tblCompanies.Where(x => x.emailId.Contains(model.emailId)).SingleOrDefault();
                    if (ut == null)
                    {
                        context.tblCompanies.Add(model);
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
        public bool UpdateCompany(EditCompany model, int id)
        {

            try
            {
                using (managementsoftwaredbEntities context = new managementsoftwaredbEntities())
                {
                    var query = context.tblCompanies.Where(x => x.id == id).FirstOrDefault();
                    if (query != null)
                    {
                        query.name = model.name;
                        query.emailId = model.emailId;
                        query.password = model.password;
                        query.contactName = model.contactName;
                        query.contactNo = model.contactNo;
                        query.address = model.address;
                        query.updateDate = model.updateDate;
                        query.isActive = Convert.ToBoolean(model.isActive);
                        query.city = model.city;
                        query.state = model.state;
                        query.country = model.country;
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
        ///  Bind dropdown of the Roles in add admin user page.
        /// </summary>
        public List<CategoryValueselcet> CategorySelectBind()
        {
            try
            {
                var model = new List<CategoryValueselcet>();
                using (managementsoftwaredbEntities context = new managementsoftwaredbEntities())
                {
                    var obj = (from db in context.tblCategories where db.parentId != 0
                               select db).ToList();
                    if (obj.Any())
                    {

                        for (int i = 0; i < obj.Count; i++)
                        {
                            var rolesss = new CategoryValueselcet();
                            rolesss.name = obj[i].name;
                            rolesss.id = obj[i].id;
                            rolesss.selected = false;
                            model.Add(rolesss);
                        }
                    }
                }
                return model;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally { }
        }

        public List<CategorywithtypeList> Categorytoselectlist()
        {
            try
            {
                // var budget_TypeList = new List<SelectListItem>();
                var mybudget_List = new List<CategorywithtypeList>();
                using (managementsoftwaredbEntities context = new managementsoftwaredbEntities())
                {
                    var obj = (from db in context.tblBudgetTypes 
                               join ct in context.tblCategories on db.id equals ct.budgetTypeId
                              // join mct in context.tblCategories on ct.parentId equals mct.id
                              where ct.parentId==0
                               where db.isActive == true
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
                            mybudget_item.CategoryValueList = CategoryValueFetch(obj[i].categoryId);
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

      
        public List<CategoryValueselcet> CategoryValueFetch(int cid)
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
                               where mc.parentId == cid
                               where mc.isActive == true
                               select new
                               {
                                   mc.id,
                                   mc.name
                                  
                               }).ToList();

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

        /// <summary>
        ///  Add new user's Roles.
        /// </summary>
        public bool CategorySelectAssign(int CompnayId, int CategoryId,DateTime Cratedate,bool isactive,bool AddDefaltBugetAmount)
        {
            try
            {
                using (managementsoftwaredbEntities context = new managementsoftwaredbEntities())
                {

                    tblcompanyCategory obj = new tblcompanyCategory();
                    obj.companyId = CompnayId;
                    obj.categoryId = CategoryId;
                    obj.createDate = Cratedate;
                    obj.isActive = isactive;
                    context.tblcompanyCategories.Add(obj);
                    context.SaveChanges();
                    if(AddDefaltBugetAmount)
                    { 
                    var bugeamountObj = (from db in context.tblExpectedBudgetLines
                               where db.isActive == true
                               where db.companyId == null
                               where db.categoryId == CategoryId
                                         select db).ToList();
                    if (bugeamountObj.Any())
                    {
                        for (int i = 0; i < bugeamountObj.Count; i++)
                        {
                            tblExpectedBudgetLine butetLineobj = new tblExpectedBudgetLine();
                            butetLineobj.budgetTypeId = bugeamountObj[i].budgetTypeId;
                            butetLineobj.categoryId = bugeamountObj[i].categoryId;
                            butetLineobj.date = bugeamountObj[i].date;
                            butetLineobj.reportType = bugeamountObj[i].reportType;
                            butetLineobj.budget = bugeamountObj[i].budget;
                            butetLineobj.companyId = CompnayId;
                            butetLineobj.createDate = Cratedate;
                            butetLineobj.isActive = isactive;
                            context.tblExpectedBudgetLines.Add(butetLineobj);
                            context.SaveChanges();
                        }
                    }
                    }
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
            //  return "User role added successfully.";
        }


        public bool KeyPoinDefaultAssign(int CompnayId, DateTime Cratedate, bool isactive)
        {
            try
            {
                using (managementsoftwaredbEntities context = new managementsoftwaredbEntities())
                {
                      var keyGroupObj = (from db in context.tblKeyPointGroups
                                             where db.isActive == true
                                             where db.companyId == null
                                             select db).ToList();
                        if (keyGroupObj.Any())
                        {
                            for (int i = 0; i < keyGroupObj.Count; i++)
                            {
                                tblKeyPointGroup butetLineobj = new tblKeyPointGroup();
                                butetLineobj.name = keyGroupObj[i].name;
                                butetLineobj.isnetProfitKey = keyGroupObj[i].isnetProfitKey;
                                butetLineobj.percentage = keyGroupObj[i].percentage;
                                butetLineobj.companyId = CompnayId;
                                butetLineobj.createDate = Cratedate;
                                butetLineobj.isActive = isactive;
                                context.tblKeyPointGroups.Add(butetLineobj);
                                context.SaveChanges();
                                int NEWid = butetLineobj.id; // get Inserted Id
                                int defaultId = keyGroupObj[i].id;
                            var keyDefaultCtegoys = (from dc in context.tblKeyPointBudgetLines
                                                       where dc.isActive == true
                                                       where dc.keypointId == defaultId
                                                     select dc).ToList();
                            if (keyDefaultCtegoys.Any())
                            {
                                for (int j = 0; j < keyDefaultCtegoys.Count; j++)
                                {
                                    int CatId = Convert.ToInt32(keyDefaultCtegoys[j].categoryId);
                                    var CheckAssingCategory = (from dd in context.tblExpectedBudgetLines
                                                               where dd.isActive == true
                                                               where dd.companyId == CompnayId
                                                               where dd.categoryId == CatId
                                                               select dd).ToList();
                                    if (CheckAssingCategory.Any())
                                    {
                                        tblKeyPointBudgetLine keybugetObj = new tblKeyPointBudgetLine();
                                        keybugetObj.categoryId = keyDefaultCtegoys[j].categoryId;
                                        keybugetObj.keypointId = NEWid;
                                        keybugetObj.creatDate = Cratedate;
                                        keybugetObj.isActive = isactive;
                                        context.tblKeyPointBudgetLines.Add(keybugetObj);
                                        context.SaveChanges();
                                    }

                                 }
                            }
                                    
                        }
                        }
                 
                    return true;
                }
            }
            catch (Exception ex)
            {
               // throw ex;
                return false;
            }
            //  return "User role added successfully.";
        }
        /// <summary>
        ///  Fetch user's Roles.
        /// </summary>
        public List<tblcompanyCategory> UserSelectAssignedCategories(int CompanyId)
        {
            try
            {
                using (managementsoftwaredbEntities context = new managementsoftwaredbEntities())
                {
                    var obj = (from db in context.tblcompanyCategories
                               where db.companyId == CompanyId
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
        /// <summary>
        /// Delete  user's old roles in update user time.
        /// </summary>
        public string CompnaySelectedCategoryDelete(int CompanyId)
        {
            try
            {
                using (managementsoftwaredbEntities context = new managementsoftwaredbEntities())
                {
                    List<tblcompanyCategory> ADMinDElList = context.tblcompanyCategories.Where(w => w.companyId == CompanyId).ToList<tblcompanyCategory>();
                    for (int j = 0; j < ADMinDElList.Count; j++)
                    {
                        context.tblcompanyCategories.Remove(ADMinDElList[j]);
                        context.SaveChanges();
                    }
                    return "Delete user roles successfully.";
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            finally { }

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
                    List<tblExpectedBudgetLine> ExpectedBudgetLineDElList = context.tblExpectedBudgetLines.Where(w => w.companyId == id).ToList<tblExpectedBudgetLine>();
                    for (int j = 0; j < ExpectedBudgetLineDElList.Count; j++)
                    {
                        context.tblExpectedBudgetLines.Remove(ExpectedBudgetLineDElList[j]);
                        context.SaveChanges();
                    }
                    List<tblRealBudget> RealBudgetDElList = context.tblRealBudgets.Where(w => w.companyId == id).ToList<tblRealBudget>();
                    for (int j = 0; j < RealBudgetDElList.Count; j++)
                    {
                        context.tblRealBudgets.Remove(RealBudgetDElList[j]);
                        context.SaveChanges();
                    }
                    List<tblcompanyCategory> CompanyDElList = context.tblcompanyCategories.Where(w => w.companyId == id).ToList<tblcompanyCategory>();
                    for (int j = 0; j < CompanyDElList.Count; j++)
                    {
                        context.tblcompanyCategories.Remove(CompanyDElList[j]);
                        context.SaveChanges();
                    }
                    List<tblKeyPointGroup> keyGroupDetails = context.tblKeyPointGroups.Where(w => w.companyId == id).ToList<tblKeyPointGroup>();
                    for (int j = 0; j < keyGroupDetails.Count; j++)
                    {
                        int KeyId = Convert.ToInt32(keyGroupDetails[j].id);
                        List<tblKeyPointBudgetLine> keypointBuget = context.tblKeyPointBudgetLines.Where(w => w.keypointId == KeyId).ToList<tblKeyPointBudgetLine>();
                        for (int k = 0; k < keypointBuget.Count; k++)
                        {
                            context.tblKeyPointBudgetLines.Remove(keypointBuget[k]);
                            context.SaveChanges();
                        }
                        context.tblKeyPointGroups.Remove(keyGroupDetails[j]);
                        context.SaveChanges();
                    }
                    tblCompany role = context.tblCompanies.Find(id);
                    context.tblCompanies.Remove(role);
                    context.SaveChanges();
                }
                return "Company deleted successfully..";
            }
            catch (Exception e)
            {
                //throw e;
                return "";
            }
            finally { }
        }
        public PagedListModel<tblCompany> ShowAllCompany(string filter, int page, int pageSize, string sort, string sortdir)
        {
            var records = new PagedListModel<tblCompany>();
            try
            {
                using (managementsoftwaredbEntities context = new managementsoftwaredbEntities())
                {
                    //var obj = (from db in context.telerik_Roles
                    //           select db).ToList();
                    //return obj;
                    records.Content = (from db in context.tblCompanies
                                       select db).Where(x => filter == null || (x.name.Contains(filter))).OrderBy(sort + " " + sortdir).Skip((page - 1) * pageSize).Take(pageSize).ToList();
                    records.TotalRecords = (from db in context.tblCompanies
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
      
    }

}