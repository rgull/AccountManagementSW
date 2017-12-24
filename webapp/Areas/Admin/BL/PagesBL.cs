using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SmartAdminMvc.Areas.Admin.Models;
using SmartAdminMvc.Models;
using System.Linq.Dynamic;

namespace SmartAdminMvc.Areas.Admin.BL
{
    public class PagesBL
    {
        public PagedListModel<tblContentPage> ViewAllContent(string filter, int page, int pageSize, string sort, string sortdir)
        {
            var records = new PagedListModel<tblContentPage>();
            try
            {
                using (managementsoftwaredbEntities context = new managementsoftwaredbEntities())
                {
                    records.Content = (from db in context.tblContentPages
                                       select db).Where(x => filter == null || (x.title.Contains(filter))).OrderBy(sort + " " + sortdir).Skip((page - 1) * pageSize).Take(pageSize).ToList();
                    records.TotalRecords = (from db in context.tblContentPages
                                            select db).Where(x => filter == null || (x.title.Contains(filter))).OrderBy(sort + " " + sortdir).Count();
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
        /// Add content(view) details in CMS. 
        /// </summary>
        public bool AddPages(tblContentPage entity)
        {
            try
            {
                using (managementsoftwaredbEntities context = new managementsoftwaredbEntities())
                {
                    var ex = context.tblContentPages.Where(x => x.title.Contains(entity.title)).SingleOrDefault();
                    if (ex == null)
                    {
                        context.tblContentPages.Add(entity);
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
        ///Delete content of CMS.
        /// </summary>
        public string PagesDelete(int id)
        {
            try
            {
                using (managementsoftwaredbEntities context = new managementsoftwaredbEntities())
                {
                    tblContentPage objtblContentPage = context.tblContentPages.Find(id);
                    context.tblContentPages.Remove(objtblContentPage);
                    context.SaveChanges();
                }
                return "Content Page delete successfully.";
            }
            catch (Exception e)
            {
                throw e;
            }
            finally { }

        }

        /// <summary>
        /// Edit content(view) of the CMS. 
        /// </summary>
        public bool UpdatePages(Pages model, int id)
        {

            try
            {
                using (managementsoftwaredbEntities context = new managementsoftwaredbEntities())
                {
                    var test = context.tblContentPages.Where(x => x.id == id).FirstOrDefault();
                    if (test != null)
                    {
                        test.title = model.name;
                        test.isActive = Convert.ToBoolean(model.status);
                        test.descpriction = model.description;
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