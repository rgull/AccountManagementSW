using SmartAdminMvc.Areas.Admin.Models;
using SmartAdminMvc.Models;
using SmartAdminMvc.Areas.Admin.BL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace SmartAdminMvc.Areas.Admin.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin/Admin
        public ActionResult Index()
        {
            if (Session["AdminUser"] != null)
            {
                return View();
            }else
            { return RedirectToAction("Login", "Admin"); }
        }

        /// <summary>
        /// Display the user details in admin portal.
        /// </summary>
        public ActionResult AdminManagement(string filter = null, int page = 1, string sort = "id", string sortdir = "DESC")
        {
            if (Session["AdminUser"] != null)
            {
                var records = new PagedListModel<tblAdmin>();
                ViewBag.filter = filter;
                int pageSize = Convert.ToInt16(System.Configuration.ConfigurationManager.AppSettings["Pagesize"].ToString());
                ViewBag.Message = "Admin Management.";
                AdminBL obj_AdminBL = new AdminBL();
                // var model = new List<telerik_Users>();
                records = obj_AdminBL.ShowAllAdmin(filter, page, pageSize, sort, sortdir);
                return View(records);
            }

            return RedirectToAction("Login", "Admin");

        }
        public ActionResult login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult login(adminLoginModel objlogin, string returnUrl)
        {
            AdminBL obj_AdminBL = new AdminBL();
            List<Addadminuser> dt = new List<Addadminuser>();
            dt = obj_AdminBL.CheckUserLogin(objlogin.email, objlogin.password);
            if (dt.Count > 0)
            {
                Session["AdminUser"] = dt[0].name.ToString();
                Session["AdminId"] = dt[0].id.ToString();
                return RedirectToAction("Index", "Admin");
                // return RedirectToLocal(returnUrl);
            }

            ModelState.AddModelError("", "The user name or password provided is incorrect.");
            return View(objlogin);
        }

        /// <summary>
        /// Logout for user portal.
        /// </summary>
        [AllowAnonymous]
        public ActionResult Logout(string returnUrl)
        {
            Session["AdminUser"] = null;
            Session["AdminId"] = null;
            Session.Abandon();
            return RedirectToAction("Login", "Admin", new { Logout = "LogOut" });
        }
        public ActionResult AddAdmin()
        {
            if (Session["AdminUser"] != null)
            {
               // List<subjectmodel> listsubject = new List<subjectmodel>();
                Addadminuser User_obj = new Addadminuser();
                AdminBL obj_AdminBL = new AdminBL();
                User_obj.isActive = true;
                return View(User_obj);
            }

            return RedirectToAction("Login", "Admin");
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult AddAdmin(Addadminuser model)
        {
            try
            {
                tblAdmin obj = new tblAdmin();
                obj.name = model.name;
                obj.emailId = model.email;
                obj.password = model.password;
                obj.isActive = Convert.ToBoolean(model.isActive);
                obj.createDate = System.DateTime.UtcNow;
               
                AdminBL obj_AdminBL = new AdminBL();
                bool id = false;
                id = obj_AdminBL.AddAdminUser(obj);
                if (id != false)
                {
                   return RedirectToAction("AdminManagement", "Admin", new { success = "Admin added successfully." });
                }
            }
            catch (MembershipCreateUserException e)
            {
                ModelState.AddModelError("", ErrorCodeToString(e.StatusCode));
            }
            return View(model);
        }

        /// <summary>
        /// Edit user Details in admin portal.
        /// </summary>
        [HttpGet]
        public ActionResult Edit(int id, SmartAdminMvc.Areas.Admin.Models.Editadminuser updatemodel)
        {
            if (Session["AdminUser"] != null)
            {
                AdminBL obj_AdminBL = new AdminBL();
                Addadminuser adminuser = new Addadminuser();
                using (managementsoftwaredbEntities context = new managementsoftwaredbEntities())
                {
                    var obj = (from db in context.tblAdmins
                               where db.id == id
                               select db).ToList();
                    if (obj.Any())
                    {
                        updatemodel.id = obj[0].id;
                        updatemodel.name = obj[0].name;
                        updatemodel.email = obj[0].emailId;
                        updatemodel.isActive = Convert.ToBoolean(obj[0].isActive);
                        updatemodel.password = obj[0].password;
                        return View(updatemodel);
                    }

                }

            }

            return RedirectToAction("Login", "Admin");
        }

        /// <summary>
        /// Edit user Details in admin portal.
        /// </summary>
        [HttpPost]
        public ActionResult Edit(Editadminuser model, FormCollection form, int id)
        {

            try
            {
                string delmsg = "";
                AdminBL obj_AdminBL = new AdminBL();
                Addadminuser adminuser = new Addadminuser();
                model.updateDate = DateTime.UtcNow;
                bool msg = obj_AdminBL.UpdateAdminUser(model, id);
                string page = "";
                if (Request.QueryString["page"] != null)
                {
                    page = Request.QueryString["page"].ToString();
                }
                if (msg != false)
                {
                   return RedirectToAction("Adminmanagement", "Admin", new { success = "Admin updated successfully.", page = page });
                }

            }
            catch (MembershipCreateUserException e)
            {
                ModelState.AddModelError("", ErrorCodeToString(e.StatusCode));
            }
            return View(model);
        }

        /// <summary>
        /// Delete user by userid in admin portal.
        /// </summary>
        [HttpGet]
        public ActionResult Delete(int id)
        {

            AdminBL obj_AdminBL = new AdminBL();
            string msg = obj_AdminBL.UserDelete(id);
            string page = "";
            if (Request.QueryString["page"] != null)
            {
                page = Request.QueryString["page"].ToString();
            }
            if (msg != "")
            {
                return RedirectToAction("Adminmanagement", "Admin", new { success = "Admin deleted successfully.", page = page });
            }
            else
            {
                ModelState.AddModelError("", "Can Not Delete");
                return View("Index");
            }
        }
        private Exception ErrorCodeToString(MembershipCreateStatus statusCode)
        {
            throw new NotImplementedException();
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Admin", new { login = "login" });
            }
        }
    }

}