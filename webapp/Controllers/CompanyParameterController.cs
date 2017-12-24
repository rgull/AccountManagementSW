using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SmartAdminMvc.Models;
using System.Web.Security;
using SmartAdminMvc.BL;
using System.Reflection;
using System.IO;

namespace SmartAdminMvc.Controllers
{
    public class CompanyParameterController : Controller
    {
        // GET: CompanyParameter
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult UpdateParameters()
        {
            return View();
        }
        public ActionResult ParametersView(string filter = null, int page = 1, string sort = "BudgetTypeName", string sortdir = "DESC")
        {
            if (Session["CompanyUser"] != null)
            {
                // var records = new PagedListModel<tblCategory>();
                var records = new PagedListModel<ViewCompanyParameter>();
                ViewBag.filter = filter;
                int companyId = Convert.ToInt32(Session["CompanyId"].ToString());
                int pageSize = Convert.ToInt16(System.Configuration.ConfigurationManager.AppSettings["Pagesize"].ToString());
                ViewBag.Message = "Parameter Management.";
                CompanyParameterBL obj_CompanyParameterBL = new CompanyParameterBL();
                // var model = new List<telerik_Users>();
                records = obj_CompanyParameterBL.ShowParameters(filter, page, pageSize, sort, sortdir, companyId);
                return View(records);
            }

            return RedirectToAction("Companylogin", "Companylogin");

        }
        [HttpGet]
        public ActionResult EditParameters(int id, CompanyParameter updatemodel)
        {
            if (Session["CompanyUser"] != null)
            {
                CompanyParameterBL obj_AdminBL = new CompanyParameterBL();
                CompanyParameter adminuser = new CompanyParameter();
                int companyId = Convert.ToInt32(Session["CompanyId"].ToString());
                using (managementsoftwaredbEntities context = new managementsoftwaredbEntities())
                {
                    var obj = (from cc in context.tblcompanyCategories
                                       join db in context.tblCategories on cc.categoryId equals db.id
                                       join mc in context.tblCategories on db.parentId equals mc.id
                                       join bt in context.tblBudgetTypes on mc.budgetTypeId equals bt.id
                                       where cc.companyId == companyId where cc.categoryId==id
                               select new CompanyParameter
                                       {
                                           Id = db.id,
                                           name = db.name,
                                           budgetTypeId = db.budgetTypeId,
                                          // Categoryname =mc.name,
                                           parentId = db.parentId,
                                          // BudgetTypeName = bt.name,
                                           createDate = db.createDate,
                                           isActive = cc.isActive.Value
                                       }).ToList();
                    if (obj.Any())
                    {
                        updatemodel.Id = obj[0].Id;
                        updatemodel.name = obj[0].name;
                        updatemodel.createDate = obj[0].createDate;
                        updatemodel.parentId = obj[0].parentId;
                        updatemodel.budgetTypeId = obj[0].budgetTypeId;
                        updatemodel.BudgetTypeList = obj_AdminBL.BudgetTypefetch();
                        updatemodel.MaincategoryList= obj_AdminBL.Categoryfetch();
                        updatemodel.Parameterslist = obj_AdminBL.ParameterCategoryValueFetch(obj[0].parentId, companyId);

                        return View(updatemodel);
                    }

                }

            }

            return RedirectToAction("Companylogin", "Companylogin");
        }

        /// <summary>
        /// Edit user Details in admin portal.
        /// </summary>
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult EditParameters(CompanyParameter model, FormCollection form, int id)
        {

            try
            {
                bool msg = false;
                CompanyParameterBL obj_CompanyparaBL = new CompanyParameterBL();
                CompanyParameter adminuser = new CompanyParameter();
                int companyId= Convert.ToInt32(Session["CompanyId"].ToString());
                int CatId = model.parentId;
                for (int i = 0; i < model.Parameterslist.Count; i++)
                {
                       msg = obj_CompanyparaBL.ParameterUpdate(companyId,CatId, Convert.ToInt32(model.Parameterslist[i].Value), model.Parameterslist[i].Selected);
                }
                string page = "";
                if (Request.QueryString["page"] != null)
                {
                    page = Request.QueryString["page"].ToString();
                }
                if (msg != false)
                {
                    return RedirectToAction("ParametersView", "CompanyParameter", new { success = "Parameters updated successfully.", page = page });
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
    }
}