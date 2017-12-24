using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SmartAdminMvc.Models;
using SmartAdminMvc.Areas.Admin.Models;
using System.Linq.Dynamic;

namespace SmartAdminMvc.Areas.Admin.BL
{
    public class BudgetTypeBL
    {
        public bool addBudget(tblBudgetType model)
        {
            try
            {
                using (managementsoftwaredbEntities context = new managementsoftwaredbEntities())
                {
                    //check for username isExit or Not
                    var ut = context.tblBudgetTypes.Where(x => x.name.Contains(model.name)).SingleOrDefault();
                    if (ut == null)
                    {
                        context.tblBudgetTypes.Add(model);
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
        public bool UpdateBudgetType(EditBudgetType model, int id)
        {

            try
            {
                using (managementsoftwaredbEntities context = new managementsoftwaredbEntities())
                {
                    var query = context.tblBudgetTypes.Where(x => x.id == id).FirstOrDefault();
                    if (query != null)
                    {
                        query.name = model.name;
                        query.rank = model.rank;
                        query.isActive = Convert.ToBoolean(model.isACTIVE);
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
                    tblBudgetType role = context.tblBudgetTypes.Find(id);
                    context.tblBudgetTypes.Remove(role);
                    context.SaveChanges();

                    List<tblBudgetType> BudgetTypeDElList = context.tblBudgetTypes.Where(w => w.id == id).ToList<tblBudgetType>();
                    for (int j = 0; j < BudgetTypeDElList.Count; j++)
                    {
                        context.tblBudgetTypes.Remove(BudgetTypeDElList[j]);
                        context.SaveChanges();
                    }

                }
                return "User delete successfully..";
            }
            catch (Exception e)
            {
                throw e;
            }
            finally { }
        }
        public int lastRank()
        {
            try
            {
                using (managementsoftwaredbEntities context = new managementsoftwaredbEntities())
                {
                    int maxrank = 0;
                    int? intIdt = context.tblBudgetTypes.Max(u => (int?)u.rank);
                    if (intIdt > 0)
                    {
                        maxrank = Convert.ToInt32(intIdt);
                    }
                    return maxrank;
                }
            }
            catch
            {
                return 0;
            }
           
        }
        public PagedListModel<tblBudgetType> ShowAllBudgetTypes(string filter, int page, int pageSize, string sort, string sortdir)
        {
            var records = new PagedListModel<tblBudgetType>();
            try
            {
                using (managementsoftwaredbEntities context = new managementsoftwaredbEntities())
                {
                    //var obj = (from db in context.telerik_Roles
                    //           select db).ToList();
                    //return obj;
                    records.Content = (from db in context.tblBudgetTypes
                                       select db).Where(x => filter == null || (x.name.Contains(filter))).OrderBy(sort + " " + sortdir).Skip((page - 1) * pageSize).Take(pageSize).ToList();
                    records.TotalRecords = (from db in context.tblBudgetTypes
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