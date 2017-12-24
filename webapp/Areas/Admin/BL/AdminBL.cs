using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SmartAdminMvc.Models;
using SmartAdminMvc.Areas.Admin.Models;
using System.Linq.Dynamic;


namespace SmartAdminMvc.Areas.Admin.BL
{
    public class AdminBL
    {
        /// <summary>
        /// Check the login.
        /// </summary>
        public List<Addadminuser> CheckUserLogin(string USerNM, string password)
        {
            try
            {
                using (managementsoftwaredbEntities context = new managementsoftwaredbEntities())
                {
                    var obj = (from us in context.tblAdmins
                               where us.emailId == USerNM
                               && us.password == password
                               select new Addadminuser
                               {
                                   name = us.emailId,
                                   id = us.id
                               }).ToList();
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
        ///  Add new user.
        /// </summary>
        public bool AddAdminUser(tblAdmin model)
        {
            try
            {
                using (managementsoftwaredbEntities context = new managementsoftwaredbEntities())
                {
                    //check for username isExit or Not
                    var ut = context.tblAdmins.Where(x => x.emailId.Contains(model.emailId)).SingleOrDefault();
                    if (ut == null)
                    {
                        context.tblAdmins.Add(model);
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
        public bool UpdateAdminUser(Editadminuser model, int id)
        {

            try
            {
                using (managementsoftwaredbEntities context = new managementsoftwaredbEntities())
                {
                    var query = context.tblAdmins.Where(x => x.id == id).FirstOrDefault();
                    if (query != null)
                    {
                        query.name = model.name;
                        query.emailId = model.email;
                        query.password = model.password;
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
                    tblAdmin role = context.tblAdmins.Find(id);
                    context.tblAdmins.Remove(role);
                    context.SaveChanges();

                    List<tblAdmin> ADMinDElList = context.tblAdmins.Where(w => w.id == id).ToList<tblAdmin>();
                    for (int j = 0; j < ADMinDElList.Count; j++)
                    {
                        context.tblAdmins.Remove(ADMinDElList[j]);
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

        public PagedListModel<tblAdmin> ShowAllAdmin(string filter, int page, int pageSize, string sort, string sortdir)
        {
            var records = new PagedListModel<tblAdmin>();
            try
            {
                using (managementsoftwaredbEntities context = new managementsoftwaredbEntities())
                {
                    //var obj = (from db in context.telerik_Roles
                    //           select db).ToList();
                    //return obj;
                    records.Content = (from db in context.tblAdmins
                                       select db).Where(x => x.id != 1).Where(x => filter == null || (x.name.Contains(filter))).OrderBy(sort + " " + sortdir).Skip((page - 1) * pageSize).Take(pageSize).ToList();
                    records.TotalRecords = (from db in context.tblAdmins
                                            select db).Where(x => x.id != 1).Where(x => filter == null || (x.name.Contains(filter))).OrderBy(sort + " " + sortdir).Count();
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