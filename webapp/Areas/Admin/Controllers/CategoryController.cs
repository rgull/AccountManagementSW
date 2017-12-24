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
    public class CategoryController : Controller
    {
        // GET: Admin/Category
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Display the user details in admin portal.
        /// </summary>
        public ActionResult CategoryView(string filter = null, int page = 1, string sort = "BudgetTypeName", string sortdir = "DESC")
        {
            if (Session["AdminUser"] != null)
            {
                // var records = new PagedListModel<tblCategory>();
                var records = new PagedListModel<ViewCategory>(); 
                ViewBag.filter = filter;
                int pageSize = Convert.ToInt16(System.Configuration.ConfigurationManager.AppSettings["Pagesize"].ToString());
                ViewBag.Message = "Cayegory Management.";
                CategoryBL obj_AdminBL = new CategoryBL();
                // var model = new List<telerik_Users>();
                records = obj_AdminBL.ShowCategory(filter, page, pageSize, sort, sortdir);
                return View(records);
            }

            return RedirectToAction("Login", "Admin");

        }
        public ActionResult CategoryValueView(string filter = null, int page = 1, string sort = "BudgetTypeName", string sortdir = "DESC")
        {
            if (Session["AdminUser"] != null)
            {
                //var records = new PagedListModel<tblCategory>();
                var records = new PagedListModel<ViewCategory>();
                ViewBag.filter = filter;
                int pageSize = Convert.ToInt16(System.Configuration.ConfigurationManager.AppSettings["Pagesize"].ToString());
                ViewBag.Message = "Category Value Management.";
                CategoryBL obj_AdminBL = new CategoryBL();
                // var model = new List<telerik_Users>();
                records = obj_AdminBL.ShowCategoryValue(filter, page, pageSize, sort, sortdir);
                return View(records);
            }

            return RedirectToAction("Login", "Admin");

        }

        [AllowAnonymous]
        public ActionResult addCategory()
        {
            if (Session["AdminUser"] != null)
            {
                // List<subjectmodel> listsubject = new List<subjectmodel>();
                Category User_obj = new Category();
                CategoryBL obj_AdminBL = new CategoryBL();
                User_obj.isActive = true;
                User_obj.BudgetTypeList = obj_AdminBL.BudgetTypefetch();
                return View(User_obj);
            }

            return RedirectToAction("Login", "Admin");
        }
        [AllowAnonymous]
        public ActionResult addCategoryValue()
        {
            if (Session["AdminUser"] != null)
            {
                // List<subjectmodel> listsubject = new List<subjectmodel>();
                CategoryValue User_obj = new CategoryValue();
                CategoryBL obj_AdminBL = new CategoryBL();
                User_obj.isActive = true;
                User_obj.BudgetTypeList = obj_AdminBL.BudgetTypefetch();
                User_obj.MaincategoryList = obj_AdminBL.Categoryfetch();
                return View(User_obj);
            }

            return RedirectToAction("Login", "Admin");
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult AddCategory(Category model)
        {
            try
            {
                tblCategory obj = new tblCategory();
                obj.name = model.name;
                obj.parentId = 0;
                obj.budgetTypeId = model.budgetTypeId;
                obj.isActive = Convert.ToBoolean(model.isActive);
                obj.createDate = System.DateTime.UtcNow;
                CategoryBL obj_AdminBL = new CategoryBL();
                bool id = false;
                id = obj_AdminBL.AddCategory(obj);
                if (id != false)
                {
                    return RedirectToAction("CategoryView", "Category", new { success = "Category added successfully." });
                }
            }
            catch (MembershipCreateUserException e)
            {
                ModelState.AddModelError("", ErrorCodeToString(e.StatusCode));
            }
            return View(model);
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult AddCategoryValue(Category model)
        {
            try
            {
                tblCategory obj = new tblCategory();
                obj.name = model.name;
                obj.parentId = model.parentId;
                obj.budgetTypeId = model.budgetTypeId;
                obj.isActive = Convert.ToBoolean(model.isActive);
                obj.createDate = System.DateTime.UtcNow;
                CategoryBL obj_AdminBL = new CategoryBL();
                bool id = false;
                id = obj_AdminBL.AddCategory(obj);
                if (id != false)
                {
                    return RedirectToAction("CategoryValueView", "Category", new { success = "Line Item added successfully." });
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
        public ActionResult EditCategory(int id, Category updatemodel)
        {
            if (Session["AdminUser"] != null)
            {
                CategoryBL obj_AdminBL = new CategoryBL();
                Category adminuser = new Category();
                using (managementsoftwaredbEntities context = new managementsoftwaredbEntities())
                {
                    var obj = (from db in context.tblCategories
                               where db.id == id
                               select db).ToList();
                    if (obj.Any())
                    {
                        updatemodel.Id = obj[0].id;
                        updatemodel.name = obj[0].name;
                        updatemodel.createDate = obj[0].createDate;
                        updatemodel.parentId = obj[0].parentId;
                        updatemodel.budgetTypeId = obj[0].budgetTypeId;
                        updatemodel.isActive = Convert.ToBoolean(obj[0].isActive);
                        updatemodel.BudgetTypeList = obj_AdminBL.BudgetTypefetch();
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
        public ActionResult EditCategory(Category model, FormCollection form, int id)
        {

            try
            {
                string delmsg = "";
                CategoryBL obj_CategoryBL = new CategoryBL();
                Category adminuser = new Category();
                model.updateDate = DateTime.UtcNow;
                bool msg = obj_CategoryBL.UpdateCategory(model, id);
                string page = "";
                if (Request.QueryString["page"] != null)
                {
                    page = Request.QueryString["page"].ToString();
                }
                if (msg != false)
                {
                    return RedirectToAction("CategoryView", "Category", new { success = "Category updated successfully.", page = page });
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
        public ActionResult EditCategoryValue(int id, CategoryValue updatemodel)
        {
            if (Session["AdminUser"] != null)
            {
                CategoryBL obj_AdminBL = new CategoryBL();
                Category adminuser = new Category();
                using (managementsoftwaredbEntities context = new managementsoftwaredbEntities())
                {
                    var obj = (from db in context.tblCategories
                               where db.id == id
                               select db).ToList();
                    if (obj.Any())
                    {
                        updatemodel.Id = obj[0].id;
                        updatemodel.name = obj[0].name;
                        updatemodel.createDate = obj[0].createDate;
                        updatemodel.parentId = obj[0].parentId;
                        updatemodel.budgetTypeId = obj[0].budgetTypeId;
                        updatemodel.isActive = Convert.ToBoolean(obj[0].isActive);
                        updatemodel.BudgetTypeList = obj_AdminBL.BudgetTypefetch();
                        updatemodel.MaincategoryList = obj_AdminBL.CategoryValueFetch(obj[0].budgetTypeId);
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
        public ActionResult EditCategoryValue(Category model, FormCollection form, int id)
        {

            try
            {
                string delmsg = "";
                CategoryBL obj_CategoryBL = new CategoryBL();
                CategoryValue adminuser = new CategoryValue();
                model.updateDate = DateTime.UtcNow;
                bool msg = obj_CategoryBL.UpdateCategory(model, id);
                string page = "";
                if (Request.QueryString["page"] != null)
                {
                    page = Request.QueryString["page"].ToString();
                }
                if (msg != false)
                {
                    return RedirectToAction("CategoryValueView", "Category", new { success = "Line Item updated successfully.", page = page });
                }

            }
            catch (MembershipCreateUserException e)
            {
                ModelState.AddModelError("", ErrorCodeToString(e.StatusCode));
            }
            return View(model);
        }
        /// <summary>
        /// Delete Category by userid in admin portal.
        /// </summary>
        [HttpGet]
        public ActionResult Delete(int id)
        {

            CategoryBL obj_CategoryBL = new CategoryBL();
            string msg = obj_CategoryBL.UserDelete(id);
            string page = "";
            if (Request.QueryString["page"] != null)
            {
                page = Request.QueryString["page"].ToString();
            }
            if (msg != "")
            {
                return RedirectToAction("CategoryView", "Category", new { success = "Deleted successfully.", page = page });
            }
            else
            {
                ModelState.AddModelError("", "Can not delete");
                return View("Index");
            }
        }
        /// <summary>
        ///  Bind dropdown of the category type in add product page.
        /// </summary>
        public JsonResult CatType_SelectedState(string id)
        {
            JsonResult result = new JsonResult();
            if (id != "" && id != null)
            {
                int cid = Convert.ToInt32(id);
                result.Data = GetSubbuttonsSelectListItem(cid);
                result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;

            }
            return result;
        }
        /// <summary>
        ///  Bind dropdown of the category type in add product page.
        /// </summary>
        private List<SelectListItem> GetSubbuttonsSelectListItem(int cat_type_id)
        {

            CategoryBL obj_ProductBL = new CategoryBL();
            //var subbutton = obj_ProductBL.CategoryValueFetch(cat_type_id);
            var subbutton = obj_ProductBL.CategoryValueFetch(cat_type_id); 

            return subbutton;
        }
        private Exception ErrorCodeToString(MembershipCreateStatus statusCode)
        {
            throw new NotImplementedException();
        }
    }
}