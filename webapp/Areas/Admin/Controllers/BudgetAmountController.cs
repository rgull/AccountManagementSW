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
    public class BudgetAmountController : Controller
    {
        // GET: Admin/BudgetAmount
        public ActionResult Index()
        {
            return View();
        }
        [AllowAnonymous]
        public ActionResult adminAddBudgetLine(int cid)
        {
            if (Session["AdminUser"] != null)
            {
                adminAddCompnayBudgetLine obj = new adminAddCompnayBudgetLine();
                BudgetAmountBL obj_productBL = new BudgetAmountBL();
                List<adminBudgetTypeList> BudgetType = new List<adminBudgetTypeList>();
                List<adminCategoryList> Category = new List<adminCategoryList>();
                List<adminReportTypeList> ReportType = new List<adminReportTypeList>();
                List<SelectListItem> productTypeList = new List<SelectListItem>();
                obj.adminBudgetTypeList = obj_productBL.BudgetTypefetch();
                obj.SelectedBusgetTypeId = Convert.ToInt32(obj.adminBudgetTypeList[0].id);
                obj.MaincategoryList = productTypeList;
                obj.adminReportTypeList = obj_productBL.ReportTypefetch();
                obj.SelectedreportTypeId = Convert.ToInt32(obj.adminReportTypeList[0].id);
                obj.companyId = cid;
                return View(obj);
            }
            return RedirectToAction("Login", "Admin");
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult adminAddBudgetLine(adminAddCompnayBudgetLine model)
        {
            try
            {
                tblExpectedBudgetLine obj = new tblExpectedBudgetLine();
                obj.budgetTypeId = model.SelectedBusgetTypeId;
                obj.categoryId = model.categoryValueId;
                if (model.selectedDate != null)
                {
                    // obj.date = Convert.ToDateTime(model.selectedDate);
                    obj.date = DateTime.ParseExact(model.selectedDate, "MM/dd/yyyy", null);
                }
                obj.reportType = model.SelectedreportTypeId;
                obj.budget = model.budget;
                obj.isActive = true;
                obj.isAddOnceInMonth = model.isAddOnceInMonth ? model.isAddOnceInMonth : false;
                obj.companyId = Convert.ToInt32(model.companyId);
                obj.createDate = System.DateTime.UtcNow;
                BudgetAmountBL obj_CompnayAccountBL = new BudgetAmountBL();
                bool id = false;
                id = obj_CompnayAccountBL.AddBudgetLine(obj);
                if (id != false)
                {
                    return RedirectToAction("adminBudgetLineView", "BudgetAmount", new { success = "Budgeted amount added successfully.", id = Convert.ToInt32(model.companyId),cid = Convert.ToInt32(model.companyId) });
                }
                else
                {
                    return RedirectToAction("adminBudgetLineView", "BudgetAmount", new { fail = "Budgeted amount alrady exist.", id = Convert.ToInt32(model.companyId), cid = Convert.ToInt32(model.companyId) });
                }
            }
            catch (MembershipCreateUserException e)
            {
                ModelState.AddModelError("", ErrorCodeToString(e.StatusCode));
            }
            return View(model);
        }
        [AllowAnonymous]
        public ActionResult DefaultAddBudgetLine()
        {
            if (Session["AdminUser"] != null)
            {
                adminAddCompnayBudgetLine obj = new adminAddCompnayBudgetLine();
                BudgetAmountBL obj_productBL = new BudgetAmountBL();
                List<adminBudgetTypeList> BudgetType = new List<adminBudgetTypeList>();
                List<adminCategoryList> Category = new List<adminCategoryList>();
                List<adminReportTypeList> ReportType = new List<adminReportTypeList>();
                List<SelectListItem> productTypeList = new List<SelectListItem>();
                obj.adminBudgetTypeList = obj_productBL.BudgetTypefetch();
                obj.SelectedBusgetTypeId = Convert.ToInt32(obj.adminBudgetTypeList[0].id);
                obj.MaincategoryList = productTypeList;
                obj.adminReportTypeList = obj_productBL.ReportTypefetch();
                obj.SelectedreportTypeId = Convert.ToInt32(obj.adminReportTypeList[0].id);
                //  obj.companyId = cid;
                return View(obj);
            }
            return RedirectToAction("Login", "Admin");
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult DefaultAddBudgetLine(adminAddCompnayBudgetLine model)
        {
            try
            {
                tblExpectedBudgetLine obj = new tblExpectedBudgetLine();
                obj.budgetTypeId = model.SelectedBusgetTypeId;
                obj.categoryId = model.categoryValueId;
                if (model.selectedDate != null)
                {
                    // obj.date = Convert.ToDateTime(model.selectedDate);
                    obj.date = DateTime.ParseExact(model.selectedDate, "MM/dd/yyyy", null);
                }
                obj.reportType = model.SelectedreportTypeId;
                obj.budget = model.budget;
                obj.isActive = true;
                obj.isAddOnceInMonth = model.isAddOnceInMonth ? model.isAddOnceInMonth : false;
                //obj.companyId = Convert.ToInt32(model.companyId);
                obj.companyId = null;
                obj.createDate = System.DateTime.UtcNow;
                BudgetAmountBL obj_CompnayAccountBL = new BudgetAmountBL();
                bool id = false;
                id = obj_CompnayAccountBL.AddBudgetLine(obj);
                if (id != false)
                {
                    return RedirectToAction("DefaultBudgetLineView", "BudgetAmount", new { success = "Budgeted amount added successfully.", id = Convert.ToInt32(model.companyId), cid = Convert.ToInt32(model.companyId) });
                }
                else
                {
                    return RedirectToAction("DefaultBudgetLineView", "BudgetAmount", new { fail = "Budgeted amount alrady exist.", id = Convert.ToInt32(model.companyId), cid = Convert.ToInt32(model.companyId) });
                }
            }
            catch (MembershipCreateUserException e)
            {
                ModelState.AddModelError("", ErrorCodeToString(e.StatusCode));
            }
            return View(model);
        }

        private Exception ErrorCodeToString(MembershipCreateStatus statusCode)
        {
            throw new NotImplementedException();
        }

        [HttpGet]
        public ActionResult adminEditBudgetLine(int id,int cid, adminAddCompnayBudgetLine updatemodel)
        {
            if (Session["AdminUser"] != null)
            {
                //int cid = 1;
                using (managementsoftwaredbEntities context = new managementsoftwaredbEntities())
                {
                    //var obj = (from db in context.tblExpectedBudgetLines
                    //           where db.id == id
                    //           select db).ToList();
                    var obj = (from db in context.tblExpectedBudgetLines
                               join ct in context.tblCategories on db.categoryId equals ct.id
                               where db.id == id
                               select new adminAddCompnayBudgetLine
                               {
                                   id = db.id,
                                   budgetTypeId = db.budgetTypeId,
                                   categoryId = ct.parentId,
                                   categoryValueId = db.categoryId,
                                   reportType = db.reportType,
                                   date = db.date,
                                   budget = db.budget,
                                   isActive = db.isActive.Value,
                                   isAddOnceInMonth = db.isAddOnceInMonth.HasValue ? db.isAddOnceInMonth.Value : false
                               }).ToList();

                    if (obj.Any())
                    {
                        //List<adminAddCompnayBudgetLine> objupdatemodel14 = new List<adminAddCompnayBudgetLine>();
                        adminAddCompnayBudgetLine objupdatemodel = new adminAddCompnayBudgetLine();
                        //var updatemodel = new adminAddCompnayBudgetLine();
                        BudgetAmountBL obj_productBL = new BudgetAmountBL();
                        List<adminBudgetTypeList> BudgetType = new List<adminBudgetTypeList>();
                        List<adminCategoryList> Category = new List<adminCategoryList>();
                        List<adminReportTypeList> ReportType = new List<adminReportTypeList>();
                        List<SelectListItem> productTypeList = new List<SelectListItem>();
                        objupdatemodel.adminBudgetTypeList = obj_productBL.BudgetTypefetch();
                        objupdatemodel.SelectedBusgetTypeId = obj[0].budgetTypeId;
                        objupdatemodel.MaincategoryList = obj_productBL.Categoryfetch(obj[0].budgetTypeId);
                        objupdatemodel.categoryId = obj[0].categoryId;
                        int CompanyId = Convert.ToInt32(cid);
                        objupdatemodel.companyId = CompanyId;
                        objupdatemodel.categoryValueList = obj_productBL.CategoryValueFetch(obj[0].categoryId, CompanyId);
                        objupdatemodel.categoryValueId = obj[0].categoryValueId;
                        objupdatemodel.adminReportTypeList = obj_productBL.ReportTypefetch();
                        objupdatemodel.SelectedreportTypeId = obj[0].reportType;
                        objupdatemodel.selectedDate = obj[0].date.ToString("MM/dd/yyyy");
                        objupdatemodel.budget = obj[0].budget;
                        objupdatemodel.isActive = Convert.ToBoolean(obj[0].isActive);
                        objupdatemodel.isAddOnceInMonth = Convert.ToBoolean(obj[0].isAddOnceInMonth);
                        // objupdatemodel14.Add(objupdatemodel);
                        return View(objupdatemodel);
                    }

                }


            }
            return RedirectToAction("Login", "Admin");
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult adminEditBudgetLine(adminAddCompnayBudgetLine model, FormCollection form, int id)
        {
            try
            {
                bool roleid = false;
                string delmsg = "";
                BudgetAmountBL obj_BudgetLineBL = new BudgetAmountBL();
                adminAddCompnayBudgetLine adminuser = new adminAddCompnayBudgetLine();
                model.updateDate = DateTime.UtcNow;
                model.companyId = Convert.ToInt32(model.companyId);
                if (model.selectedDate != null)
                {
                    // obj.date = Convert.ToDateTime(model.selectedDate);
                    model.date = DateTime.ParseExact(model.selectedDate, "MM/dd/yyyy", null);
                }
                bool msg = obj_BudgetLineBL.UpdateBudgetLine(model, id);
                string page = "";
                if (Request.QueryString["page"] != null)
                {
                    page = Request.QueryString["page"].ToString();
                }
                if (msg != false)
                {
                    return RedirectToAction("adminBudgetLineView", "BudgetAmount", new { success = "Budgeted amount updated successfully.", id = Convert.ToInt32(model.companyId), cid = Convert.ToInt32(model.companyId), page = page });
                }
                else
                {
                    return RedirectToAction("adminBudgetLineView", "BudgetAmount", new { fail = "Budgeted amount alrady exist.", id = Convert.ToInt32(model.companyId), cid = Convert.ToInt32(model.companyId), page = page });
                }

            }
            catch (MembershipCreateUserException e)
            {
                ModelState.AddModelError("", ErrorCodeToString(e.StatusCode));
            }
            return View(model);
        }
        [HttpGet]
        public ActionResult DefaultEditBudgetLine(int id, DefaultAddCompnayBudgetLine Dupdatemodel)
        {
            if (Session["AdminUser"] != null)
            {
                //int cid = 1;
                using (managementsoftwaredbEntities context = new managementsoftwaredbEntities())
                {
                    //var obj = (from db in context.tblExpectedBudgetLines
                    //           where db.id == id
                    //           select db).ToList();
                    var obj = (from db in context.tblExpectedBudgetLines
                               join ct in context.tblCategories on db.categoryId equals ct.id
                               where db.id == id
                               select new DefaultAddCompnayBudgetLine
                               {
                                   id = db.id,
                                   budgetTypeId = db.budgetTypeId,
                                   categoryId = ct.parentId,
                                   categoryValueId = db.categoryId,
                                   reportType = db.reportType,
                                   date = db.date,
                                   budget = db.budget,
                                   isActive = db.isActive.Value,
                                   isAddOnceInMonth = db.isAddOnceInMonth.HasValue ? db.isAddOnceInMonth.Value : false
                               }).ToList();

                    if (obj.Any())
                    {
                        BudgetAmountBL obj_productBL = new BudgetAmountBL();
                        List<adminBudgetTypeList> BudgetType = new List<adminBudgetTypeList>();
                        List<adminCategoryList> Category = new List<adminCategoryList>();
                        List<adminReportTypeList> ReportType = new List<adminReportTypeList>();
                        List<SelectListItem> productTypeList = new List<SelectListItem>();
                        Dupdatemodel.adminBudgetTypeList = obj_productBL.BudgetTypefetch();
                        Dupdatemodel.SelectedBusgetTypeId = obj[0].budgetTypeId;
                        Dupdatemodel.MaincategoryList = obj_productBL.Categoryfetch(obj[0].budgetTypeId);
                        Dupdatemodel.categoryId = obj[0].categoryId;
                        Dupdatemodel.categoryValueList = obj_productBL.DefaultCategoryValueFetch(obj[0].categoryId);
                        Dupdatemodel.categoryValueId = obj[0].categoryValueId;
                        Dupdatemodel.adminReportTypeList = obj_productBL.ReportTypefetch();
                        Dupdatemodel.SelectedreportTypeId = obj[0].reportType;
                        Dupdatemodel.selectedDate = obj[0].date.ToString("MM/dd/yyyy");
                        Dupdatemodel.budget = obj[0].budget;
                        Dupdatemodel.isActive = Convert.ToBoolean(obj[0].isActive);
                        Dupdatemodel.isAddOnceInMonth = Convert.ToBoolean(obj[0].isAddOnceInMonth);
                        return View(Dupdatemodel);
                    }

                }


            }
            return RedirectToAction("Login", "Admin");
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult DefaultEditBudgetLine(adminAddCompnayBudgetLine model, FormCollection form, int id)
        {
            try
            {
                bool roleid = false;
                string delmsg = "";
                BudgetAmountBL obj_BudgetLineBL = new BudgetAmountBL();
                adminAddCompnayBudgetLine adminuser = new adminAddCompnayBudgetLine();
                model.updateDate = DateTime.UtcNow;
                //model.companyId = Convert.ToInt32(model.companyId);
                if (model.selectedDate != null)
                {
                    // obj.date = Convert.ToDateTime(model.selectedDate);
                    model.date = DateTime.ParseExact(model.selectedDate, "MM/dd/yyyy", null);
                }
                bool msg = obj_BudgetLineBL.UpdateBudgetLine(model, id);
                string page = "";
                if (Request.QueryString["page"] != null)
                {
                    page = Request.QueryString["page"].ToString();
                }
                if (msg != false)
                {
                    return RedirectToAction("DefaultBudgetLineView", "BudgetAmount", new { success = "Budgeted amount updated successfully.", id = Convert.ToInt32(model.companyId), cid = Convert.ToInt32(model.companyId), page = page });
                }
                else
                {
                    return RedirectToAction("DefaultBudgetLineView", "BudgetAmount", new { fail = "Budgeted amount alrady exist.", id = Convert.ToInt32(model.companyId), cid = Convert.ToInt32(model.companyId), page = page });
                }

            }
            catch (MembershipCreateUserException e)
            {
                ModelState.AddModelError("", ErrorCodeToString(e.StatusCode));
            }
            return View(model);
        }

        [HttpGet]
        public ActionResult Delete(int id,int cid)
        {

            BudgetAmountBL obj_CompanyBL = new BudgetAmountBL();
            //string msg = obj_CompanyBL.BudgetLineDelete(id);
            string msg = obj_CompanyBL.BudgetLineDeleteWithrealBudget(id,cid);
            string page = "";
            if (Request.QueryString["page"] != null)
            {
                page = Request.QueryString["page"].ToString();
            }
            if (msg != "")
            {
                return RedirectToAction("adminBudgetLineView", "BudgetAmount", new { success = "Budgeted amount deleted successfully.", id = Convert.ToInt32(cid), cid = Convert.ToInt32(cid), page = page });
            }
            else
            {
                ModelState.AddModelError("", "Can not delete");
                return View("Index");
            }
        }

        [HttpGet]
        public ActionResult DefaultDelete(int id)
        {

            BudgetAmountBL obj_CompanyBL = new BudgetAmountBL();
            string msg = obj_CompanyBL.BudgetLineDelete(id);
            string page = "";
            if (Request.QueryString["page"] != null)
            {
                page = Request.QueryString["page"].ToString();
            }
            if (msg != "")
            {
                return RedirectToAction("DefaultBudgetLineView", "BudgetAmount", new { success = "Budgeted amount deleted successfully.",page=page });
            }
            else
            {
                ModelState.AddModelError("", "Can not delete");
                return View("Index");
            }
        }
        public ActionResult AdminBudgetLineView(string filter = null, int page = 1, string sort = "budgetType", string sortdir = "DESC", int cid = 0)
        {
            if (Session["AdminUser"] != null)
            {
                int pageSize = Convert.ToInt16(System.Configuration.ConfigurationManager.AppSettings["Pagesize"].ToString());
                var records = new PagedListModel<adminCompanyAccountModel>();
                ViewBag.filter = filter;
                BudgetAmountBL obj_ProductBL = new BudgetAmountBL();
                //var Product_list = new List<Product>();
                records = obj_ProductBL.AdminViewAllBudgetLineList(filter, page, pageSize, sort, sortdir, cid);
                return View(records);
            }
            return RedirectToAction("Login", "Admin");
        }
        public ActionResult DefaultBudgetLineView(string filter = null, int page = 1, string sort = "budgetType", string sortdir = "DESC")
        {
            if (Session["AdminUser"] != null)
            {
                int pageSize = Convert.ToInt16(System.Configuration.ConfigurationManager.AppSettings["Pagesize"].ToString());
                var records = new PagedListModel<adminCompanyAccountModel>();
                ViewBag.filter = filter;
                BudgetAmountBL obj_ProductBL = new BudgetAmountBL();
                //var Product_list = new List<Product>();
                records = obj_ProductBL.DefaultViewAllBudgetLineList(filter, page, pageSize, sort, sortdir);
                return View(records);
            }
            return RedirectToAction("Login", "Admin");
        }
        public JsonResult Cat_SelectedState(string id)
        {
            JsonResult result = new JsonResult();
            if (id != "" && id != null)
            {
                int Bid = Convert.ToInt32(id);
                result.Data = GetCategorySelectListItem(Bid);
                result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;

            }
            return result;
        }
        /// <summary>
        ///  Bind dropdown of the category type in add product page.
        /// </summary>
        private List<SelectListItem> GetCategorySelectListItem(int Budget_type_id)
        {

            BudgetAmountBL obj_ProductBL = new BudgetAmountBL();
            var subbutton = obj_ProductBL.Categoryfetch(Budget_type_id);

            return subbutton;
        }

        public JsonResult CatValue_SelectedState(string id,string cid)
        {
            JsonResult result = new JsonResult();
            if (id != "" && id != null)
            {
                int catid = Convert.ToInt32(id);
                int companyid = Convert.ToInt32(cid);
                result.Data = GetCategoryValueSelectListItem(catid, companyid);
                result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;

            }
            return result;
        }
        /// <summary>
        ///  Bind dropdown of the category type in add product page.
        /// </summary>
        private List<SelectListItem> GetCategoryValueSelectListItem(int Cat_type_id,int companyid)
        {

            BudgetAmountBL obj_ProductBL = new BudgetAmountBL();
            int CompanyId = companyid;
            var subbutton = obj_ProductBL.CategoryValueFetch(Cat_type_id, CompanyId);

            return subbutton;
        }
        public JsonResult DefaultCatValue_SelectedState(string id)
        {
            JsonResult result = new JsonResult();
            if (id != "" && id != null)
            {
                int catid = Convert.ToInt32(id);
                result.Data = GetDefaultCategoryValueSelectListItem(catid);
                result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;

            }
            return result;
        }
        /// <summary>
        ///  Bind dropdown of the category type in add product page.
        /// </summary>
        private List<SelectListItem> GetDefaultCategoryValueSelectListItem(int Cat_type_id)
        {

            BudgetAmountBL obj_ProductBL = new BudgetAmountBL();
            var subbutton = obj_ProductBL.DefaultCategoryValueFetch(Cat_type_id);

            return subbutton;
        }
    }
}