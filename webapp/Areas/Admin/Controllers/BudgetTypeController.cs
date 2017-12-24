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
    public class BudgetTypeController : Controller
    {
        // GET: Admin/BudgetType
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult BudgetTypeManagement(string filter = null, int page = 1, string sort = "id", string sortdir = "DESC")
        {
            if (Session["AdminUser"] != null)
            {
                var records = new PagedListModel<tblBudgetType>();
                ViewBag.filter = filter;
                int pageSize = Convert.ToInt16(System.Configuration.ConfigurationManager.AppSettings["Pagesize"].ToString());
                ViewBag.Message = "Admin Management.";
                BudgetTypeBL obj_BudgetTypeBL = new BudgetTypeBL();
                // var model = new List<telerik_Users>();
                records = obj_BudgetTypeBL.ShowAllBudgetTypes(filter, page, pageSize, sort, sortdir);
                return View(records);
            }

            return RedirectToAction("Login", "Admin");

        }

        [HttpGet]
        public ActionResult AddBudgetType()
        {
            AddBudgetType obj = new AddBudgetType();
            BudgetTypeBL obj_BudgetBL = new BudgetTypeBL();
            int LastRank = 0;
            LastRank = obj_BudgetBL.lastRank();
            obj.rank = LastRank + 1;
            obj.isACTIVE = true;
            return View(obj);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult AddBudgetType(AddBudgetType model)
        {
            try
            {
                tblBudgetType obj = new tblBudgetType();
                obj.name = model.name;
                obj.rank = model.rank;
                obj.isActive = model.isACTIVE;
                obj.createDate = DateTime.UtcNow;
                BudgetTypeBL budgetObj = new BudgetTypeBL();
                bool Id = false;
                Id = budgetObj.addBudget(obj);
                if(Id != false)
                {
                    return RedirectToAction("BudgetTypeManagement","BudgetType",new {success="Budget type added successfully."});
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
        public ActionResult Edit(int id, EditBudgetType updatemodel)
        {
            if (Session["AdminUser"] != null)
            {
                BudgetTypeBL obj_AdminBL = new BudgetTypeBL();
                AddBudgetType adminuser = new AddBudgetType();
                using (managementsoftwaredbEntities context = new managementsoftwaredbEntities())
                {
                    var obj = (from db in context.tblBudgetTypes
                               where db.id == id
                               select db).ToList();
                    if (obj.Any())
                    {
                        updatemodel.name = obj[0].name;
                        updatemodel.rank = obj[0].rank;
                        updatemodel.isACTIVE = Convert.ToBoolean(obj[0].isActive);
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
        public ActionResult Edit(EditBudgetType model, FormCollection form, int id)
        {

            try
            {
                string delmsg = "";
                BudgetTypeBL obj_BudgetTypeBL = new BudgetTypeBL();
                Addadminuser adminuser = new Addadminuser();
                bool msg = obj_BudgetTypeBL.UpdateBudgetType(model, id);
                if (msg != false)
                {
                    return RedirectToAction("BudgetTypeManagement", "BudgetType", new { success = "Budget type updated successfully." });
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

            BudgetTypeBL obj_BudgetTypeBL = new BudgetTypeBL();
            string msg = obj_BudgetTypeBL.UserDelete(id);
            if (msg != "")
            {
                return RedirectToAction("BudgetTypeManagement", "BudgetType");
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