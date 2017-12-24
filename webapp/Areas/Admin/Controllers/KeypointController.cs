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
    public class KeypointController : Controller
    {
        // GET: Admin/Keypoint
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult KeyPointManagement(string filter = null, int page = 1, string sort = "id", string sortdir = "DESC", int cid = 0)
        {
            if (Session["AdminUser"] != null)
            {
                var records = new PagedListModel<tblKeyPointGroup>();
                ViewBag.filter = filter;
                int pageSize = Convert.ToInt16(System.Configuration.ConfigurationManager.AppSettings["Pagesize"].ToString());
                ViewBag.Message = "Key Point Management.";
                KeyPointBL obj_AdminBL = new KeyPointBL();
                // var model = new List<telerik_Users>();
                records = obj_AdminBL.ShowAllKeyPointGroups(filter, page, pageSize, sort, sortdir,cid);
                return View(records);
            }

            return RedirectToAction("Login", "Admin");

        }
        [AllowAnonymous]
        public ActionResult addKeyPoint(int cid)
        {
            if (Session["AdminUser"] != null)
            {
                // List<subjectmodel> listsubject = new List<subjectmodel>();
                addKeyPoint User_obj = new addKeyPoint();
                KeyPointBL obj_AdminBL = new KeyPointBL();
                User_obj.isActive = true;
                User_obj.companyId = cid;
                // User_obj.CategoryValueSelected = obj_AdminBL.CategorySelectBind();
                User_obj.CategorywithList = obj_AdminBL.CategorytoselectforKeyPointlist(cid); 
                //User_obj.CategorywithList = obj_AdminBL.Categorytoselectlist();
                //if (User_obj.CategorywithList.Count > 0)
                //{
                //    for (int j = 0; j < User_obj.CategorywithList.Count; j++)
                //    {
                //        for (int i = 0; i < User_obj.CategorywithList[j].CategoryValueList.Count; i++)
                //        {
                //            User_obj.CategorywithList[j].CategoryValueList[i].selected = true;
                //        }
                //    }
                //}

                return View(User_obj);
            }

            return RedirectToAction("Login", "Admin");
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult addKeyPoint(addKeyPoint model)
        {
            try
            {
                tblKeyPointGroup obj = new tblKeyPointGroup();
                obj.name = model.name;
                obj.companyId = model.companyId;
                obj.percentage = model.percentage;
                obj.isActive = Convert.ToBoolean(model.isActive);
                obj.isnetProfitKey = Convert.ToBoolean(model.isnetProfitKey);
                obj.isBusinessDevKey = Convert.ToBoolean(model.isBusinessDevKey);
                obj.createDate = System.DateTime.UtcNow;
                int compnyId = model.companyId;
                KeyPointBL obj_AdminBL = new KeyPointBL();
                bool id = false;
                bool roleid = false;
                id = obj_AdminBL.AddKeyPoint(obj);
                if (id != false)
                {
                    //for (int i = 0; i < model.CategoryValueSelected.Count; i++)
                    //{
                    //    if (model.CategoryValueSelected[i].selected == true)
                    //    {
                    //        roleid = obj_AdminBL.CategorySelectAssign(obj.id, model.CategoryValueSelected[i].id, obj.createDate, true);
                    //    }

                    //}
                    for (int j = 0; j < model.CategorywithList.Count(); j++)
                    {
                        for (int i = 0; i < model.CategorywithList[j].CategoryValueList.Count; i++)
                        {
                            if (model.CategorywithList[j].CategoryValueList[i].selected == true)
                            {
                                roleid = obj_AdminBL.CategorySelectAssignforKeyPoint(obj.id, model.CategorywithList[j].CategoryValueList[i].id, obj.createDate, true);
                            }
                        }
                    }
                    return RedirectToAction("KeyPointManagement", "KeyPoint", new { cid= compnyId, success = "Key Point Group added successfully." });
                }
            }
            catch (MembershipCreateUserException e)
            {
                ModelState.AddModelError("", ErrorCodeToString(e.StatusCode));
            }
            return View(model);
        }

        [HttpGet]
        public ActionResult EditKeyPoint(int id,int cid, SmartAdminMvc.Areas.Admin.Models.addKeyPoint updatemodel)
        {
            if (Session["AdminUser"] != null)
            {
                KeyPointBL obj_AdminBL = new KeyPointBL();
                addKeyPoint adminuser = new addKeyPoint();
                // adminuser.CategoryValueSelected = obj_AdminBL.CategorySelectBind();
                using (managementsoftwaredbEntities context = new managementsoftwaredbEntities())
                {
                    var obj = (from db in context.tblKeyPointGroups
                               where db.id == id
                               select db).ToList();
                    if (obj.Any())
                    {
                        updatemodel.id = obj[0].id;
                        updatemodel.name = obj[0].name;
                        updatemodel.companyId = cid;
                        updatemodel.percentage = Convert.ToDecimal(obj[0].percentage);
                        updatemodel.createDate = Convert.ToDateTime(obj[0].createDate);
                        updatemodel.isActive = Convert.ToBoolean(obj[0].isActive);
                        updatemodel.isnetProfitKey = Convert.ToBoolean(obj[0].isnetProfitKey);
                        updatemodel.isBusinessDevKey = Convert.ToBoolean(obj[0].isBusinessDevKey);
                        updatemodel.CategorywithList = obj_AdminBL.CategorytoselectforKeyPointlist(cid);

                        if (updatemodel.CategorywithList.Count > 0)
                        {
                            List<tblKeyPointBudgetLine> dt = new List<tblKeyPointBudgetLine>();
                            dt = obj_AdminBL.UserSelectAssignedCategoriesforKeyPoint(id);

                            if (dt.Any())
                            {
                                for (int i = 0; i < dt.Count; i++)
                                {
                                    for (int j = 0; j < updatemodel.CategorywithList.Count; j++)
                                    {
                                        for (int k = 0; k < updatemodel.CategorywithList[j].CategoryValueList.Count; k++)
                                        {
                                            if (updatemodel.CategorywithList[j].CategoryValueList[k].id == dt[i].categoryId)
                                            {
                                                updatemodel.CategorywithList[j].CategoryValueList[k].selected = true;
                                            }
                                        }
                                    }
                                }
                            }
                        }
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
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult EditKeyPoint(addKeyPoint model, FormCollection form, int id)
        {

            try
            {
                bool roleid = false;
                string delmsg = "";
                KeyPointBL obj_CompanyBL = new KeyPointBL();
               // addCompany adminuser = new addCompany();
                model.updateDate = DateTime.UtcNow;
                model.isnetProfitKey = Convert.ToBoolean(model.isnetProfitKey);
                model.isBusinessDevKey = Convert.ToBoolean(model.isBusinessDevKey);
                int compnyId = model.companyId;
               // adminuser.CategoryValueSelected = obj_CompanyBL.CategorySelectBind();
                bool msg = obj_CompanyBL.UpdateKeyPointGroup(model, id);
                if (msg != false)
                {
                    if (Session["AdminId"].ToString() == "1")
                    {
                        if (id != null)
                        {
                            delmsg = obj_CompanyBL.KeyPointSelectedCategoryDelete(id);
                        }
                        if (delmsg != "")
                        {
                            //for (int i = 0; i < model.CategoryValueSelected.Count; i++)
                            //{
                            //    if (model.CategoryValueSelected[i].selected == true)
                            //    {
                            //        roleid = obj_CompanyBL.CategorySelectAssign(id, model.CategoryValueSelected[i].id,model.updateDate,true);
                            //    }

                            //}
                            for (int j = 0; j < model.CategorywithList.Count(); j++)
                            {
                                for (int i = 0; i < model.CategorywithList[j].CategoryValueList.Count; i++)
                                {
                                    if (model.CategorywithList[j].CategoryValueList[i].selected == true)
                                    {
                                        //roleid = obj_CompanyBL.CategorySelectAssign(id, model.CategorywithList[j].CategoryValueList[i].id, model.updateDate, true, false);
                                        roleid = obj_CompanyBL.CategorySelectAssignforKeyPoint(id, model.CategorywithList[j].CategoryValueList[i].id, model.updateDate, true);
                                    }
                                }
                            }
                        }
                    }
                    string page = "";
                    if (Request.QueryString["page"] != null)
                    {
                        page = Request.QueryString["page"].ToString();
                    }
                    return RedirectToAction("KeyPointManagement", "KeyPoint", new { cid = compnyId , success = "Key Point Group details updated successfully.",page =page });
                }

            }
            catch (MembershipCreateUserException e)
            {
                ModelState.AddModelError("", ErrorCodeToString(e.StatusCode));
            }
            return View(model);
        }
        /// <summary>
        /// Delete Company by userid in admin portal.
        /// </summary>
        [HttpGet]
        public ActionResult Delete(int id,int cid)
        {

            KeyPointBL obj_CompanyBL = new KeyPointBL();
            string msg = obj_CompanyBL.KeyPointGroupDelete(id);
            if (msg != "")
            {
                //return RedirectToAction("CompanyManagement", "Company", new { success = "Company deleted successfully." });
                return RedirectToAction("KeyPointManagement", "KeyPoint", new { cid = cid, success = msg });
            }
            else
            {
                ModelState.AddModelError("", "Can not delete");
                return View("Index");
            }
        }

        public ActionResult DefaultKeyPointManagement(string filter = null, int page = 1, string sort = "id", string sortdir = "DESC")
        {
            if (Session["AdminUser"] != null)
            {
                var records = new PagedListModel<tblKeyPointGroup>();
                ViewBag.filter = filter;
                int pageSize = Convert.ToInt16(System.Configuration.ConfigurationManager.AppSettings["Pagesize"].ToString());
                ViewBag.Message = "Key Point Management.";
                KeyPointBL obj_AdminBL = new KeyPointBL();
                // var model = new List<telerik_Users>();
                records = obj_AdminBL.DefaultShowAllKeyPointGroups(filter, page, pageSize, sort, sortdir);
                return View(records);
            }

            return RedirectToAction("Login", "Admin");

        }
        [AllowAnonymous]
        public ActionResult DefaultaddKeyPoint()
        {
            if (Session["AdminUser"] != null)
            {
                // List<subjectmodel> listsubject = new List<subjectmodel>();
                addKeyPoint User_obj = new addKeyPoint();
                KeyPointBL obj_AdminBL = new KeyPointBL();
                User_obj.isActive = true;
               // User_obj.companyId = cid;
                // User_obj.CategoryValueSelected = obj_AdminBL.CategorySelectBind();
                User_obj.CategorywithList = obj_AdminBL.DefaultCategorytoselectforKeyPointlist();
                //User_obj.CategorywithList = obj_AdminBL.Categorytoselectlist();
                //if (User_obj.CategorywithList.Count > 0)
                //{
                //    for (int j = 0; j < User_obj.CategorywithList.Count; j++)
                //    {
                //        for (int i = 0; i < User_obj.CategorywithList[j].CategoryValueList.Count; i++)
                //        {
                //            User_obj.CategorywithList[j].CategoryValueList[i].selected = true;
                //        }
                //    }
                //}

                return View(User_obj);
            }

            return RedirectToAction("Login", "Admin");
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult DefaultaddKeyPoint(addKeyPoint model)
        {
            try
            {
                tblKeyPointGroup obj = new tblKeyPointGroup();
                obj.name = model.name;
                //obj.companyId = model.companyId;
                obj.percentage = model.percentage;
                obj.isActive = Convert.ToBoolean(model.isActive);
                obj.isnetProfitKey = Convert.ToBoolean(model.isnetProfitKey);
                obj.isBusinessDevKey = Convert.ToBoolean(model.isBusinessDevKey);
                obj.createDate = System.DateTime.UtcNow;
                //int compnyId = model.companyId;
                KeyPointBL obj_AdminBL = new KeyPointBL();
                bool id = false;
                bool roleid = false;
                id = obj_AdminBL.AddKeyPoint(obj);
                if (id != false)
                {
                    //for (int i = 0; i < model.CategoryValueSelected.Count; i++)
                    //{
                    //    if (model.CategoryValueSelected[i].selected == true)
                    //    {
                    //        roleid = obj_AdminBL.CategorySelectAssign(obj.id, model.CategoryValueSelected[i].id, obj.createDate, true);
                    //    }

                    //}
                    for (int j = 0; j < model.CategorywithList.Count(); j++)
                    {
                        for (int i = 0; i < model.CategorywithList[j].CategoryValueList.Count; i++)
                        {
                            if (model.CategorywithList[j].CategoryValueList[i].selected == true)
                            {
                                roleid = obj_AdminBL.CategorySelectAssignforKeyPoint(obj.id, model.CategorywithList[j].CategoryValueList[i].id, obj.createDate, true);
                            }
                        }
                    }
                    return RedirectToAction("DefaultKeyPointManagement", "KeyPoint", new { success = "Key Point Group added successfully." });
                }
            }
            catch (MembershipCreateUserException e)
            {
                ModelState.AddModelError("", ErrorCodeToString(e.StatusCode));
            }
            return View(model);
        }

        [HttpGet]
        public ActionResult DefaultEditKeyPoint(int id, SmartAdminMvc.Areas.Admin.Models.addKeyPoint updatemodel)
        {
            if (Session["AdminUser"] != null)
            {
                KeyPointBL obj_AdminBL = new KeyPointBL();
                addKeyPoint adminuser = new addKeyPoint();
                // adminuser.CategoryValueSelected = obj_AdminBL.CategorySelectBind();
                using (managementsoftwaredbEntities context = new managementsoftwaredbEntities())
                {
                    var obj = (from db in context.tblKeyPointGroups
                               where db.id == id
                               select db).ToList();
                    if (obj.Any())
                    {
                        updatemodel.id = obj[0].id;
                        updatemodel.name = obj[0].name;
                        //updatemodel.companyId = cid;
                        updatemodel.percentage = Convert.ToDecimal(obj[0].percentage);
                        updatemodel.createDate = Convert.ToDateTime(obj[0].createDate);
                        updatemodel.isActive = Convert.ToBoolean(obj[0].isActive);
                        updatemodel.isnetProfitKey = Convert.ToBoolean(obj[0].isnetProfitKey);
                        updatemodel.isBusinessDevKey = Convert.ToBoolean(obj[0].isBusinessDevKey);
                        updatemodel.CategorywithList = obj_AdminBL.DefaultCategorytoselectforKeyPointlist();

                        if (updatemodel.CategorywithList.Count > 0)
                        {
                            List<tblKeyPointBudgetLine> dt = new List<tblKeyPointBudgetLine>();
                            dt = obj_AdminBL.UserSelectAssignedCategoriesforKeyPoint(id);

                            if (dt.Any())
                            {
                                for (int i = 0; i < dt.Count; i++)
                                {
                                    for (int j = 0; j < updatemodel.CategorywithList.Count; j++)
                                    {
                                        for (int k = 0; k < updatemodel.CategorywithList[j].CategoryValueList.Count; k++)
                                        {
                                            if (updatemodel.CategorywithList[j].CategoryValueList[k].id == dt[i].categoryId)
                                            {
                                                updatemodel.CategorywithList[j].CategoryValueList[k].selected = true;
                                            }
                                        }
                                    }
                                }
                            }
                        }
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
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult DefaultEditKeyPoint(addKeyPoint model, FormCollection form, int id)
        {

            try
            {
                bool roleid = false;
                string delmsg = "";
                KeyPointBL obj_CompanyBL = new KeyPointBL();
                // addCompany adminuser = new addCompany();
                model.updateDate = DateTime.UtcNow;
                model.isnetProfitKey = Convert.ToBoolean(model.isnetProfitKey);
                model.isBusinessDevKey = Convert.ToBoolean(model.isBusinessDevKey);
                //int compnyId = model.companyId;
                // adminuser.CategoryValueSelected = obj_CompanyBL.CategorySelectBind();
                bool msg = obj_CompanyBL.UpdateKeyPointGroup(model, id);
                if (msg != false)
                {
                    if (Session["AdminId"].ToString() == "1")
                    {
                        if (id != null)
                        {
                            delmsg = obj_CompanyBL.KeyPointSelectedCategoryDelete(id);
                        }
                        if (delmsg != "")
                        {
                            //for (int i = 0; i < model.CategoryValueSelected.Count; i++)
                            //{
                            //    if (model.CategoryValueSelected[i].selected == true)
                            //    {
                            //        roleid = obj_CompanyBL.CategorySelectAssign(id, model.CategoryValueSelected[i].id,model.updateDate,true);
                            //    }

                            //}
                            for (int j = 0; j < model.CategorywithList.Count(); j++)
                            {
                                for (int i = 0; i < model.CategorywithList[j].CategoryValueList.Count; i++)
                                {
                                    if (model.CategorywithList[j].CategoryValueList[i].selected == true)
                                    {
                                        //roleid = obj_CompanyBL.CategorySelectAssign(id, model.CategorywithList[j].CategoryValueList[i].id, model.updateDate, true, false);
                                        roleid = obj_CompanyBL.CategorySelectAssignforKeyPoint(id, model.CategorywithList[j].CategoryValueList[i].id, model.updateDate, true);
                                    }
                                }
                            }
                        }
                    }
                    string page = "";
                    if (Request.QueryString["page"] != null)
                    {
                        page = Request.QueryString["page"].ToString();
                    }
                    return RedirectToAction("DefaultKeyPointManagement", "KeyPoint", new { success = "Key Point Group details updated successfully.",page=page });
                }

            }
            catch (MembershipCreateUserException e)
            {
                ModelState.AddModelError("", ErrorCodeToString(e.StatusCode));
            }
            return View(model);
        }
        [HttpGet]
        public ActionResult DefaultDelete(int id)
        {

            KeyPointBL obj_CompanyBL = new KeyPointBL();
            string msg = obj_CompanyBL.KeyPointGroupDelete(id);
            if (msg != "")
            {
                //return RedirectToAction("CompanyManagement", "Company", new { success = "Company deleted successfully." });
                return RedirectToAction("DefaultKeyPointManagement", "KeyPoint", new { success = msg });
            }
            else
            {
                ModelState.AddModelError("", "Can not delete");
                return View("Index");
            }
        }
        private Exception ErrorCodeToString(MembershipCreateStatus statusCode)
        {
            throw new NotImplementedException();
        }
    }
}