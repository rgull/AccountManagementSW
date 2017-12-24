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
using System.Globalization;

namespace SmartAdminMvc.Controllers
{
    public class CompanyAccountController : Controller
    {
        // GET: CompanyAccount
        public ActionResult Index()
        {
            return View();
        }
        [AllowAnonymous]
        public ActionResult AddBudgetLine()
        {
            if (Session["CompanyUser"] != null)
            {
                AddCompnayBudgetLine obj = new AddCompnayBudgetLine();
                CompanyAccountBL obj_productBL = new CompanyAccountBL();
                List<BudgetTypeList> BudgetType = new List<BudgetTypeList>();
                List<CategoryList> Category = new List<CategoryList>();
                List<ReportTypeList> ReportType = new List<ReportTypeList>();
                List<SelectListItem> productTypeList = new List<SelectListItem>();
                obj.BudgetTypeList = obj_productBL.BudgetTypefetch();
                obj.SelectedBusgetTypeId = Convert.ToInt32(obj.BudgetTypeList[0].id);
                obj.MaincategoryList = productTypeList;
                obj.ReportTypeList = obj_productBL.ReportTypefetch();
                obj.SelectedreportTypeId = Convert.ToInt32(obj.ReportTypeList[0].id);
                return View(obj);
            }
            return RedirectToAction("Companylogin", "Companylogin");
        }

        public ActionResult AddBudgetLines()
        {
            if (Session["CompanyUser"] != null)
            {
                Mybudget obj = new Mybudget();
                CompanyAccountBL obj_productBL = new CompanyAccountBL();
                int CompanyId = Convert.ToInt32(Session["CompanyId"].ToString());
                //obj.MyValuebudgetFullList = obj_productBL.MybudgetLine(CompanyId);
                obj.BudgetTypeList = obj_productBL.BudgetTypefetch();
                obj.SelectedBudgetTypeId = Convert.ToInt32(obj.BudgetTypeList[0].id);
                return View(obj);
            }
            return RedirectToAction("Companylogin", "Companylogin");
            
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult AddBudgetLines(Mybudget model)
        {
            List<int> Bids = new List<int>();
            if (Session["CompanyUser"] != null)
            {
                string monthName = "";
                try
                {
                    string Updatedate = "";
                    bool id = false;
                    if (model.MyValuebudgetFullList != null)
                    {
                        for (int j = 0; j < model.MyValuebudgetFullList.Count; j++)
                        {
                            tblExpectedBudgetLine obj = new tblExpectedBudgetLine();
                            CompanyAccountBL obj_CompnayAccountBL = new CompanyAccountBL();
                            tblRealCarCount objcar = new tblRealCarCount();
                            
                            if (model.date != null)
                            {
                                // obj.date = Convert.ToDateTime(model.selectedDate);
                                var m = model.selectedDate.Split(' ')[0];
                                monthName = m;
                                int month = DateTimeFormatInfo.CurrentInfo.MonthNames.ToList().IndexOf(m) + 1;
                                var year = model.selectedDate.Split(' ')[1];
                                var date = new DateTime(int.Parse(year), month, 1);
                                obj.date = date; //DateTime.ParseExact(model.selectedDate, "MM/dd/yyyy", null);
                                //obj.budgetTypeId = model.SelectedBudgetTypeId;
                                Updatedate = obj.date.ToString("MM/dd/yyyy");
                                //objcar.date = DateTime.ParseExact(model.selectedDate, "MM/dd/yyyy", null);
                                //objcar.carCounts = model.carCounts;
                                //objcar.compnayId = Convert.ToInt32(Session["CompanyId"].ToString());
                                //obj_CompnayAccountBL.AddmyBudgetCarCounts(objcar);

                                //obj.budgetTypeId = model.SelectedBudgetTypeId == 0 ? model.MyValuebudgetFullList[j].budgetTypeId : model.SelectedBudgetTypeId;
                                
                                //RGK Test one entry in a month
                                //if (obj_CompnayAccountBL.IsCurrentMonthBudgetExists(date, Convert.ToInt32(Session["CompanyId"].ToString())) == true)
                                //{
                                //    return RedirectToAction("BudgetLineView", "CompanyAccount", new { fail = "You have already added a budget for the month of " + monthName + ". Please select an item below to edit.", id = 0, Bid = 0 });
                                //}
                            }
                            if (model.MyValuebudgetFullList[j] != null && model.MyValuebudgetFullList[j].MyValueParameterList != null)
                            {
                                if (model.MyValuebudgetFullList[j].MyValueParameterList != null)
                                {
                                    if (model.MyValuebudgetFullList[j].MyValueParameterList.Count > 0)
                                    {
                                        for (int i = 0; i < model.MyValuebudgetFullList[j].MyValueParameterList.Count; i++)
                                        {
                                            obj.categoryId = model.MyValuebudgetFullList[j].MyValueParameterList[i].id;
                                            obj.budget = model.MyValuebudgetFullList[j].MyValueParameterList[i].budget;
                                            obj.budgetTypeId = model.MyValuebudgetFullList[j].MyValueParameterList[i].budgetTypeId;
                                            obj.isActive = true;
                                            obj.companyId = Convert.ToInt32(Session["CompanyId"].ToString());
                                            obj.createDate = System.DateTime.UtcNow;
                                            obj.reportType = model.MyValuebudgetFullList[j].MyValueParameterList[i].SelectedreportTypeId;
                                            obj.isAddOnceInMonth = model.MyValuebudgetFullList[j].MyValueParameterList[i].isAddOnceInMonth;
                                            if (obj.budget > 0 || obj.budget < 0)
                                            {
                                                //if (model.MyValuebudgetFullList[j].MyValueParameterList[i].isUpdated == 1)
                                                //{
                                                id = obj_CompnayAccountBL.AddBudgetLine(obj);
                                                if (id == false)
                                                {
                                                    //int bid = 0;
                                                    //bid = obj_CompnayAccountBL.getIdOfAdded(obj);
                                                    //Bids.Add(bid);
                                                    int bid = 0;
                                                    bid = obj_CompnayAccountBL.getIdOfAdded(obj);
                                                    Bids.Add(i);
                                                    //return RedirectToAction("BudgetLineView", "CompanyAccount", new { fail = "You have already added a budget for the month of " + monthName + ". Please select an item below to edit.", id = bid, Bid = bid });

                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }

                    if (Bids.Count == 0)
                    {
                        return RedirectToAction("BudgetLineView", "CompanyAccount", new { success = "Budgeted amount added successfully." });
                    }
                    else
                    {

                        //return RedirectToAction("BudgetLineView", "CompanyAccount", new { fail = "Budgeted amount already exist.", Bids = Bids });
                        return RedirectToAction("BudgetLineView", "CompanyAccount", new { fail = "You have already added a budget for the month of " + monthName + ". Please select an item below to edit.", id = 0, Bid = 0 });
                    }
                }
                catch (MembershipCreateUserException e)
                {
                    ModelState.AddModelError("", ErrorCodeToString(e.StatusCode));
                }
                return View(model);
            }
            return RedirectToAction("Companylogin", "Companylogin");
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult AddBudgetLine(AddCompnayBudgetLine model)
        {
            try
            {
                tblExpectedBudgetLine obj = new tblExpectedBudgetLine();
                obj.budgetTypeId = model.SelectedBusgetTypeId;
                obj.categoryId = model.categoryValueId;
                if (model.selectedDate != null)
                {
                    // obj.date = Convert.ToDateTime(model.selectedDate);
                    obj.date = DateTime.ParseExact("01 " + model.selectedDate, "dd MMMM yyyy", null);
                }
                obj.reportType = model.SelectedreportTypeId;
                obj.budget = model.budget;
                obj.isActive = true;
                obj.isAddOnceInMonth = Convert.ToBoolean(model.isAddOnceInMonth);
                obj.companyId = Convert.ToInt32(Session["CompanyId"].ToString());
                obj.createDate = System.DateTime.UtcNow;
                CompanyAccountBL obj_CompnayAccountBL = new CompanyAccountBL();
                bool id = false;
                id = obj_CompnayAccountBL.AddBudgetLine(obj);
                if (id != false)
                {
                    return RedirectToAction("BudgetLineView", "CompanyAccount", new { success = "Budgeted amount added successfully." });
                }
                else
                {
                    int bid = 0;
                    bid = obj_CompnayAccountBL.getIdOfAdded(obj);
                    return RedirectToAction("BudgetLineView", "CompanyAccount", new { fail = "Budgeted amount already exist.", id = bid, Bid = bid });
                }
            }
            catch (MembershipCreateUserException e)
            {
                ModelState.AddModelError("", ErrorCodeToString(e.StatusCode));
            }
            return View(model);
        }
        [HttpGet]
        public ActionResult EditBudgetLine(int id, AddCompnayBudgetLine updatemodel)
        {
            if (Session["CompanyUser"] != null)
            {
                using (managementsoftwaredbEntities context = new managementsoftwaredbEntities())
                {
                    //var obj = (from db in context.tblExpectedBudgetLines
                    //           where db.id == id
                    //           select db).ToList();
                    var obj = (from db in context.tblExpectedBudgetLines
                               join ct in context.tblCategories on db.categoryId equals ct.id
                               where db.id == id
                               select new AddCompnayBudgetLine
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
                        CompanyAccountBL obj_productBL = new CompanyAccountBL();
                        List<BudgetTypeList> BudgetType = new List<BudgetTypeList>();
                        List<CategoryList> Category = new List<CategoryList>();
                        List<ReportTypeList> ReportType = new List<ReportTypeList>();
                        List<SelectListItem> productTypeList = new List<SelectListItem>();
                        updatemodel.BudgetTypeList = obj_productBL.BudgetTypefetch();
                        updatemodel.SelectedBusgetTypeId = obj[0].budgetTypeId;
                        updatemodel.MaincategoryList = obj_productBL.Categoryfetch(obj[0].budgetTypeId);
                        updatemodel.categoryId = obj[0].categoryId;
                        int CompanyId = Convert.ToInt32(Session["CompanyId"].ToString());
                        updatemodel.categoryValueList = obj_productBL.CategoryValueFetch(obj[0].categoryId, CompanyId);
                        updatemodel.categoryValueId = obj[0].categoryValueId;
                        updatemodel.ReportTypeList = obj_productBL.ReportTypefetch();
                        updatemodel.SelectedreportTypeId = obj[0].reportType;
                        updatemodel.selectedDate = obj[0].date.ToString("MM/dd/yyyy");
                        updatemodel.budget = obj[0].budget;
                        updatemodel.isActive = Convert.ToBoolean(obj[0].isActive);
                        updatemodel.isAddOnceInMonth = Convert.ToBoolean(obj[0].isAddOnceInMonth);
                        return View(updatemodel);
                    }

                }


            }

            return RedirectToAction("Companylogin", "Companylogin");
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult EditBudgetLine(AddCompnayBudgetLine model, FormCollection form, int id)
        {
            try
            {
                bool roleid = false;
                string delmsg = "";
                CompanyAccountBL obj_BudgetLineBL = new CompanyAccountBL();
                AddCompnayBudgetLine adminuser = new AddCompnayBudgetLine();
                model.updateDate = DateTime.UtcNow;
                model.companyId = Convert.ToInt32(Session["CompanyId"].ToString());
                model.isActive = true;
                string page = "";
                if (Request.QueryString["page"] != null)
                {
                    page = Request.QueryString["page"].ToString();
                }
                if (model.selectedDate != null)
                {
                    // obj.date = Convert.ToDateTime(model.selectedDate);
                    model.date = DateTime.ParseExact("01 " + model.selectedDate, "dd MMMM yyyy", null);
                }
                bool msg = obj_BudgetLineBL.UpdateBudgetLine(model, id);
                if (msg != false)
                {
                    return RedirectToAction("BudgetLineView", "CompanyAccount", new { success = "Budgeted amount updated successfully.", page = page });
                }
                else
                {
                    return RedirectToAction("BudgetLineView", "CompanyAccount", new { fail = "Budgeted amount already exist.", page = page });
                }
            }
            catch (MembershipCreateUserException e)
            {
                ModelState.AddModelError("", ErrorCodeToString(e.StatusCode));
            }
            return View(model);
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            string page = "";
            if (Request.QueryString["page"] != null)
            {
                page = Request.QueryString["page"].ToString();
            }
            CompanyAccountBL obj_CompanyBL = new CompanyAccountBL();
            //string msg = obj_CompanyBL.BudgetLineDelete(id);
            int companyId = Convert.ToInt32(Session["CompanyId"].ToString());
            string msg = obj_CompanyBL.BudgetLineDeleteWithrealBudget(id, companyId);
            if (msg != "")
            {
                return RedirectToAction("BudgetLineView", "CompanyAccount", new { success = "Budgeted amount deleted.", page = page });
            }
            else
            {
                ModelState.AddModelError("", "Can not delete");
                return View("Index");
            }
        }        

        public ActionResult BudgetLineView(string filter = null, int page = 1, string sort = "budgetType", string sortdir = "DESC", List<int> Bids = null)
        {
            if (Session["CompanyUser"] != null)
            {
                int pageSize = Convert.ToInt16(System.Configuration.ConfigurationManager.AppSettings["Pagesize"].ToString());
                var records = new PagedListModel<CompanyAccountModel>();
                ViewBag.filter = filter;
                CompanyAccountBL obj_ProductBL = new CompanyAccountBL();
                int companyId = Convert.ToInt32(Session["CompanyId"].ToString());
                //var Product_list = new List<Product>();
                records = obj_ProductBL.ViewAllBudgetLineList(filter, page, pageSize, sort, sortdir, companyId);
                if(Bids != null && Bids.Count > 0)
                {
                    ViewBag.Bids = Bids;
                }
                return View(records);
            }
            return RedirectToAction("Companylogin", "Companylogin");
        }

        private Exception ErrorCodeToString(MembershipCreateStatus statusCode)
        {
            throw new NotImplementedException();
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

            CompanyAccountBL obj_ProductBL = new CompanyAccountBL();
            var subbutton = obj_ProductBL.Categoryfetch(Budget_type_id);

            return subbutton;
        }

        public JsonResult CatValue_SelectedState(string id)
        {
            JsonResult result = new JsonResult();
            if (id != "" && id != null)
            {
                int cid = Convert.ToInt32(id);
                result.Data = GetCategoryValueSelectListItem(cid);
                result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;

            }
            return result;
        }
        /// <summary>
        ///  Bind dropdown of the category type in add product page.
        /// </summary>
        private List<SelectListItem> GetCategoryValueSelectListItem(int Cat_type_id)
        {

            CompanyAccountBL obj_ProductBL = new CompanyAccountBL();
            int CompanyId = Convert.ToInt32(Session["CompanyId"].ToString());
            var subbutton = obj_ProductBL.CategoryValueFetch(Cat_type_id, CompanyId);

            return subbutton;
        }
        [HttpGet]
        [AllowAnonymous]
        public ActionResult AddMyBudget()
        {
            if (Session["CompanyUser"] != null)
            {
                Mybudget obj = new Mybudget();
                CompanyAccountBL obj_productBL = new CompanyAccountBL();
                int CompanyId = Convert.ToInt32(Session["CompanyId"].ToString());
                //obj.MyValuebudgetFullList = obj_productBL.MybudgetLine(CompanyId);                
                return View(obj);
            }
            return RedirectToAction("Companylogin", "Companylogin");
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult AddMyBudget(Mybudget model)
        {
            if (Session["CompanyUser"] != null)
            {
                try
                {
                    string Updatedate = "";
                    bool id = false;
                    if (model.MyValuebudgetFullList != null)
                    {
                        for (int j = 0; j < model.MyValuebudgetFullList.Count; j++)
                        {
                            tblRealBudget obj = new tblRealBudget();
                            CompanyAccountBL obj_CompnayAccountBL = new CompanyAccountBL();
                            tblRealCarCount objcar = new tblRealCarCount();
                            if (model.date != null)
                            {
                                // obj.date = Convert.ToDateTime(model.selectedDate);
                                obj.date = DateTime.ParseExact(model.selectedDate, "MM/dd/yyyy", null);
                                Updatedate = obj.date.ToString("MM/dd/yyyy");
                                objcar.date = DateTime.ParseExact(model.selectedDate, "MM/dd/yyyy", null);
                                objcar.carCounts = model.carCounts;
                                objcar.compnayId = Convert.ToInt32(Session["CompanyId"].ToString());
                                obj_CompnayAccountBL.AddmyBudgetCarCounts(objcar);
                            }
                            obj.budgetTypeId = model.MyValuebudgetFullList[j].budgetTypeId;
                            if (model.MyValuebudgetFullList[j] != null && model.MyValuebudgetFullList[j].MyValueParameterList != null)
                            {
                                if (model.MyValuebudgetFullList[j].MyValueParameterList != null)
                                {
                                    if (model.MyValuebudgetFullList[j].MyValueParameterList.Count > 0)
                                    {
                                        for (int i = 0; i < model.MyValuebudgetFullList[j].MyValueParameterList.Count; i++)
                                        {
                                            obj.categoryId = model.MyValuebudgetFullList[j].MyValueParameterList[i].id;
                                            obj.budget = model.MyValuebudgetFullList[j].MyValueParameterList[i].budget;
                                            obj.isActive = true;
                                            obj.companyId = Convert.ToInt32(Session["CompanyId"].ToString());
                                            obj.createDate = System.DateTime.UtcNow;
                                            if ((obj.budget > 0 || obj.budget<0))
                                            {
                                                id = obj_CompnayAccountBL.AddMyBudgetLine(obj);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    if (id != false)
                    {
                        return RedirectToAction("MyBudgetView", "CompanyAccount", new { success = "My Income/Expense added successfully." });
                    }
                    else
                    {
                        return RedirectToAction("MyBudgetView", "CompanyAccount", new { fail = "You have already entered a value for " + Updatedate.ToString() + " date.", date = Updatedate.ToString() });
                    }
                }
                catch (MembershipCreateUserException e)
                {
                    ModelState.AddModelError("", ErrorCodeToString(e.StatusCode));
                }
                return View(model);
            }
            return RedirectToAction("Companylogin", "Companylogin");
        }
        [HttpGet]
        [AllowAnonymous]
        public ActionResult EditMyBudget(DateTime date, Mybudget updatemodel)
        {
            if (Session["CompanyUser"] != null)
            {
                CompanyAccountBL obj_productBL = new CompanyAccountBL();
                int CompanyId = Convert.ToInt32(Session["CompanyId"].ToString());
                updatemodel.selectedDate = date.ToString("MM/dd/yyyy");
                updatemodel.carCounts = obj_productBL.MyBudgetCarCount(CompanyId, date);
                updatemodel.carCountId = obj_productBL.MyBudgetCarCountID(CompanyId, date);
                updatemodel.MyValuebudgetFullList = obj_productBL.EditMybudgetLine(CompanyId, date);
                return View(updatemodel);
            }
            return RedirectToAction("Companylogin", "Companylogin");
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult EditMyBudget(Mybudget model)
        {
            if (Session["CompanyUser"] != null)
            {
                try
                {
                    string Updatedatetoadd = "";
                    bool id = false;
                    for (int j = 0; j < model.MyValuebudgetFullList.Count; j++)
                    {
                        tblRealBudget obj = new tblRealBudget();
                        tblRealCarCount objcar = new tblRealCarCount();
                        CompanyAccountBL obj_CompnayAccountBL = new CompanyAccountBL();
                        obj.date = DateTime.ParseExact(model.selectedDate, "MM/dd/yyyy", null);
                        Updatedatetoadd = obj.date.ToString("MM/dd/yyyy");
                        obj.createDate = System.DateTime.UtcNow;
                        if (model.date != null)
                        {
                            // obj.date = Convert.ToDateTime(model.selectedDate);
                            obj.date = DateTime.ParseExact(model.selectedDate, "MM/dd/yyyy", null);
                            objcar.date = DateTime.ParseExact(model.selectedDate, "MM/dd/yyyy", null);
                            objcar.carCounts = model.carCounts;
                            objcar.compnayId = Convert.ToInt32(Session["CompanyId"].ToString());
                            objcar.id = model.carCountId;
                            obj_CompnayAccountBL.EditmyBudgetCarCounts(objcar);
                        }
                        obj.budgetTypeId = model.MyValuebudgetFullList[j].budgetTypeId;
                        if (model.MyValuebudgetFullList[j] != null && model.MyValuebudgetFullList[j].MyValueParameterList != null)
                        {
                            for (int i = 0; i < model.MyValuebudgetFullList[j].MyValueParameterList.Count; i++)
                            {
                                obj.categoryId = model.MyValuebudgetFullList[j].MyValueParameterList[i].id;
                                obj.budget = model.MyValuebudgetFullList[j].MyValueParameterList[i].budget;
                                obj.isActive = true;
                                obj.companyId = Convert.ToInt32(Session["CompanyId"].ToString());
                                obj.updateDate = System.DateTime.UtcNow;
                                id = obj_CompnayAccountBL.UpdateMyBudget(obj, model.MyValuebudgetFullList[j].MyValueParameterList[i].MyBudgetid);
                            }
                        }
                    }
                    string page = "";
                    if (Request.QueryString["page"] != null)
                    {
                        page = Request.QueryString["page"].ToString();
                    }
                    if (id != false)
                    {
                        return RedirectToAction("MyBudgetView", "CompanyAccount", new { success = "My Income/Expense added successfully.", page = page });
                    }
                    else
                    {
                        return RedirectToAction("MyBudgetView", "CompanyAccount", new { fail = "My Income/Expense already exist.", page = page });
                    }

                }
                catch (MembershipCreateUserException e)
                {
                    ModelState.AddModelError("", ErrorCodeToString(e.StatusCode));
                }
                return View(model);
            }
            return RedirectToAction("Companylogin", "Companylogin");
        }
        public ActionResult MyBudgetView(string filter = null, int page = 1, string sort = "date", string sortdir = "DESC")
        {
            if (Session["CompanyUser"] != null)
            {
                int pageSize = Convert.ToInt16(System.Configuration.ConfigurationManager.AppSettings["Pagesize"].ToString());
                // var records = new PagedListModel<CompanyAccountModel>();
                var records = new PagedListModel<Mybudget>();
                ViewBag.filter = filter;
                int companyId = Convert.ToInt32(Session["CompanyId"].ToString());
                CompanyAccountBL obj_ProductBL = new CompanyAccountBL();
                records = obj_ProductBL.ViewDateofMyBudgetList(filter, page, pageSize, sort, sortdir, companyId);
                // records = obj_ProductBL.ViewAllMyBudgetList(filter, page, pageSize, sort, sortdir,companyId);
                return View(records);
            }
            return RedirectToAction("Companylogin", "Companylogin");

        }
        [HttpGet]
        public ActionResult DeleteMyBudget(DateTime date)
        {

            CompanyAccountBL obj_CompanyBL = new CompanyAccountBL();
            int companyId = Convert.ToInt32(Session["CompanyId"].ToString());
            string msg = obj_CompanyBL.MyBudgetDelete(companyId, date);
            string page = "";
            if (Request.QueryString["page"] != null)
            {
                page = Request.QueryString["page"].ToString();
            }
            if (msg != "")
            {
                return RedirectToAction("MyBudgetView", "CompanyAccount", new { success = "Budgeted amount deleted.", page = page });
            }
            else
            {
                ModelState.AddModelError("", "Can not delete");
                return View("Index");
            }
        }

        [HttpGet]
        public ActionResult FilterFildsWithdate(DateTime date)
        {
            Mybudget obj = new Mybudget();
            CompanyAccountBL obj_productBL = new CompanyAccountBL();
            int CompanyId = Convert.ToInt32(Session["CompanyId"].ToString());
            List<ReportTypeList> ReportType = new List<ReportTypeList>();            
            obj.MyValuebudgetFullList = obj_productBL.MybudgetLineWithFilter(CompanyId, date);
            return PartialView("~/Views/CompanyAccount/AddMybugetFieldsView.cshtml", obj);
            // return PartialView("~/Areas/Admin/Views/CompanyAccount/AddMybugetFieldsView.cshtml", obj);//TODO: use T4MVC
        }

        [HttpGet]
        public ActionResult FilterBudgetFieldsWithdate(DateTime date)
        {
            Mybudget obj = new Mybudget();
            CompanyAccountBL obj_productBL = new CompanyAccountBL();
            int CompanyId = Convert.ToInt32(Session["CompanyId"].ToString());
            List<ReportTypeList> ReportType = new List<ReportTypeList>();
            obj.ReportTypeList = obj_productBL.ReportTypefetch();
            obj.MyValuebudgetFullList = obj_productBL.MybudgetLinesWithFilter(CompanyId, date);
            return PartialView("~/Views/CompanyAccount/AddBudgetLinesView.cshtml", obj);
            // return PartialView("~/Areas/Admin/Views/CompanyAccount/AddMybugetFieldsView.cshtml", obj);//TODO: use T4MVC
        }
        public JsonResult dateAlradyadded(string date)
        {
            JsonResult result = new JsonResult();
            if (date != "" && date != null)
            {
                int cid = Convert.ToInt32(Session["CompanyId"].ToString());

                // result.Data = GetCategorySelectListItem(cid,date);
                result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;

            }
            return result;
        }
    }
}