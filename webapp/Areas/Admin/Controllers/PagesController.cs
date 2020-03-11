using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SmartAdminMvc.Areas.Admin.Models;
using SmartAdminMvc.Areas.Admin.BL;
using System.Data.SqlClient;
using System.Data;
using SmartAdminMvc.Models;

namespace SmartAdminMvc.Areas.Admin.Controllers
{
    public class PagesController : Controller
    {
        // GET: Admin/Pages
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// Bind the content details of CMS.
        /// </summary>
        /// 
        public ActionResult PagesView(string filter = null, int page = 1, string sort = "id", string sortdir = "DESC")
        {
            if (Session["AdminUser"] != null)
            {
                int pageSize = Convert.ToInt16(System.Configuration.ConfigurationManager.AppSettings["Pagesize"].ToString());
                var records = new PagedListModel<tblContentPage>();
                ViewBag.filter = filter;
                PagesBL Page_obj = new PagesBL();
                var model = new List<tblContentPage>();
                //model = Page_obj.ViewAllContent();
                records = Page_obj.ViewAllContent(filter, page, pageSize, sort, sortdir);
                return View(records);
            }
            return RedirectToAction("Login", "Admin");
        }

        /// <summary>
        /// Add content(view) details in CMS. 
        /// </summary>
        [AllowAnonymous]
        public ActionResult AddPages()
        {
            if (Session["AdminUser"] != null)
            {
                Pages obj = new Pages();
                obj.status = true;
                return View(obj);
            }
            return RedirectToAction("Login", "Admin");
        }

        /// <summary>
        /// Add content(model) details in CMS. 
        /// </summary>
        [HttpPost]
        [ValidateInput(false)]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult AddPages(Pages model)
        {
            try
            {
                PagesBL Page_obj = new PagesBL();
                tblContentPage obj = new tblContentPage();
                obj.title = model.name;
                obj.descpriction = model.description;
                obj.isActive = model.status;
                string msg = "";
                if (Page_obj.AddPages(obj)) { msg = "success"; }
                else { msg = "fail"; }
                return RedirectToAction("PagesView", "Pages", new { success = "Content added successfully." });

            }
            catch (System.Web.Security.MembershipCreateUserException e)
            {
                ModelState.AddModelError("", ErrorCodeToString(e.StatusCode));
            }
            return View(model);
        }

        /// <summary>
        ///Delete content of CMS.
        /// </summary>
        [HttpGet]
        public ActionResult PagesDelete(int id)
        {
            PagesBL Page_obj = new PagesBL();
            string msg = Page_obj.PagesDelete(id);          
            if (msg != "")
            {
                return RedirectToAction("PagesView", "Pages");
            }
            else
            {
                ModelState.AddModelError("", "Can not delete");
                return View("Index");
            }
        }

        /// <summary>
        /// Edit content(view) of the CMS. 
        /// </summary>
        [HttpGet]
        public ActionResult PagesEdit(int id, SmartAdminMvc.Areas.Admin.Models.Pages updatemodel)
        {
            if (Session["AdminUser"] != null)
            {

                using (managementsoftwaredbEntities context = new managementsoftwaredbEntities())
                {
                    var obj = (from db in context.tblContentPages
                               where db.id == id
                               select db).ToList();
                    if (obj.Any())
                    {
                        Pages objtblContentPage = new Pages();
                        objtblContentPage.id = obj[0].id;
                        objtblContentPage.name = obj[0].title;
                        objtblContentPage.description = obj[0].descpriction;
                        objtblContentPage.status = obj[0].isActive.Value;
                        objtblContentPage.metaTitle = obj[0].metaTitle;
                        objtblContentPage.metaDescription = obj[0].metaDescription;
                        return View(objtblContentPage);
                    }
                }

            }
            return RedirectToAction("Login", "Admin");
        }

        /// <summary>
        /// Edit details(view) of the product category. 
        /// </summary>
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult PagesEdit(Pages model, FormCollection form, int id)
        {
            try
            {
                PagesBL Page_obj = new PagesBL();
                bool msg = Page_obj.UpdatePages(model, id);
                 string page = "";
                    if (Request.QueryString["page"] != null)
                    {
                        page = Request.QueryString["page"].ToString();
                    }
                if (msg != false)
                {
                    return RedirectToAction("PagesView", "Pages", new { success = "Content updated successfully." ,page =page});
                }

            }
            catch (System.Web.Security.MembershipCreateUserException e)
            {
                ModelState.AddModelError("", ErrorCodeToString(e.StatusCode));
            }
            return View(model);
        }

        /// <summary>
        /// Error handling function.
        /// </summary>
        private string ErrorCodeToString(System.Web.Security.MembershipCreateStatus membershipCreateStatus)
        {
            throw new NotImplementedException();
        }

    }
}