using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SmartAdminMvc.Areas.Admin.Models;
using SmartAdminMvc.Models;
using SmartAdminMvc.Areas.Admin.BL;
using System.Web.Security;


namespace SmartAdminMvc.Areas.Admin.Controllers
{
    public class ReportTypeController : Controller
    {
        // GET: Admin/ReportType
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult ReportTypeManagement(string filter = null, int page = 1, string sort = "id", string sortdir = "DESC")
        {
            if (Session["AdminUser"] != null)
            {
                var records = new PagedListModel<tblReportType>();
                ViewBag.filter = filter;
                int pageSize = Convert.ToInt16(System.Configuration.ConfigurationManager.AppSettings["Pagesize"].ToString());
                ViewBag.Message = "Admin Management.";
                ReportTypeBL obj_ReportTypeBL = new ReportTypeBL();
                // var model = new List<telerik_Users>();
                records = obj_ReportTypeBL.ShowAllReportTypes(filter, page, pageSize, sort, sortdir);
                return View(records);
            }

            return RedirectToAction("Login", "Admin");

        }

        [HttpGet]
        public ActionResult AddReportType()
        {
            AddReportType obj = new AddReportType();
            ReportTypeBL obj_BudgetBL = new ReportTypeBL();
            int LastRank = 0;
            LastRank = obj_BudgetBL.lastRank();
            obj.rank = LastRank + 1;
            obj.isActive = true;
            return View(obj);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult AddReportType(AddReportType model)
        {
            try
            {
                tblReportType obj = new tblReportType();
                obj.name = model.name;
                obj.rank = model.rank;
                obj.isActive = model.isActive;
                obj.createDate = DateTime.UtcNow;
                ReportTypeBL budgetObj = new ReportTypeBL();
                bool Id = false;
                Id = budgetObj.addreportType(obj);
                if (Id != false)
                {
                    return RedirectToAction("ReportTypeManagement", "ReportType", new { success = "Budget type added successfully." });
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
        public ActionResult Edit(int id, EditReportType updatemodel)
        {
            if (Session["AdminUser"] != null)
            {
                ReportTypeBL obj_AdminBL = new ReportTypeBL();
                AddReportType adminuser = new AddReportType();
                using (managementsoftwaredbEntities context = new managementsoftwaredbEntities())
                {
                    var obj = (from db in context.tblReportTypes
                               where db.id == id
                               select db).ToList();
                    if (obj.Any())
                    {
                        updatemodel.name = obj[0].name;
                        updatemodel.rank = obj[0].rank;
                        updatemodel.isActive = Convert.ToBoolean(obj[0].isActive);
                        return View(updatemodel);
                    }

                }

            }

            return RedirectToAction("Login", "Admin");
        }

        /// <summary>
        /// Edit user Details Of Budget Type.
        /// </summary>
        [HttpPost]
        public ActionResult Edit(EditReportType model, FormCollection form, int id)
        {

            try
            {
                string delmsg = "";
                ReportTypeBL obj_ReportTypeBL = new ReportTypeBL();
                Addadminuser adminuser = new Addadminuser();
                bool msg = obj_ReportTypeBL.UpdateReportType(model, id);
                if (msg != false)
                {
                    return RedirectToAction("ReportTypeManagement", "ReportType", new { success = "Budget type updated successfully." });
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

            ReportTypeBL obj_ReportTypeBL = new ReportTypeBL();
            string msg = obj_ReportTypeBL.UserDelete(id);
            if (msg != "")
            {
                return RedirectToAction("ReportTypeManagement", "ReportType");
            }
            else
            {
                ModelState.AddModelError("", "Can not delete");
                return View("Index");
            }
        }
        private Exception ErrorCodeToString(object statusCode)
        {
            throw new NotImplementedException();
        }
    }
}