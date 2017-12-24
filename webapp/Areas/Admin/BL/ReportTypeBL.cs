using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SmartAdminMvc.Models;
using SmartAdminMvc.Areas.Admin.Models;
using System.Linq.Dynamic;

namespace SmartAdminMvc.Areas.Admin.BL
{
    public class ReportTypeBL
    {
        public bool addreportType(tblReportType model)
        {
            try
            {
                using (managementsoftwaredbEntities context = new managementsoftwaredbEntities())
                {
                    //check for username isExit or Not
                    var ut = context.tblReportTypes.Where(x => x.name.Contains(model.name)).SingleOrDefault();
                    if (ut == null)
                    {
                        context.tblReportTypes.Add(model);
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
        public bool UpdateReportType(EditReportType model, int id)
        {

            try
            {
                using (managementsoftwaredbEntities context = new managementsoftwaredbEntities())
                {
                    var query = context.tblReportTypes.Where(x => x.id == id).FirstOrDefault();
                    if (query != null)
                    {
                        query.name = model.name;
                        query.rank = model.rank;
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
                    tblReportType role = context.tblReportTypes.Find(id);
                    context.tblReportTypes.Remove(role);
                    context.SaveChanges();

                    List<tblReportType> ReportTypeDElList = context.tblReportTypes.Where(w => w.id == id).ToList<tblReportType>();
                    for (int j = 0; j < ReportTypeDElList.Count; j++)
                    {
                        context.tblReportTypes.Remove(ReportTypeDElList[j]);
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
                    int? intIdt = context.tblReportTypes.Max(u => (int?)u.rank);
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
        public PagedListModel<tblReportType> ShowAllReportTypes(string filter, int page, int pageSize, string sort, string sortdir)
        {
            var records = new PagedListModel<tblReportType>();
            try
            {
                using (managementsoftwaredbEntities context = new managementsoftwaredbEntities())
                {
                    //var obj = (from db in context.telerik_Roles
                    //           select db).ToList();
                    //return obj;
                    records.Content = (from db in context.tblReportTypes
                                       select db).Where(x => filter == null || (x.name.Contains(filter))).OrderBy(sort + " " + sortdir).Skip((page - 1) * pageSize).Take(pageSize).ToList();
                    records.TotalRecords = (from db in context.tblReportTypes
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