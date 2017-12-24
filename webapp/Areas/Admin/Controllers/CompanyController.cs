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
    public class CompanyController : Controller
    {
        // GET: Admin/Company
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// Display the user details in admin portal.
        /// </summary>
        public ActionResult CompanyManagement(string filter = null, int page = 1, string sort = "id", string sortdir = "DESC")
        {
            if (Session["AdminUser"] != null)
            {
                var records = new PagedListModel<tblCompany>();
                ViewBag.filter = filter;
                int pageSize = Convert.ToInt16(System.Configuration.ConfigurationManager.AppSettings["Pagesize"].ToString());
                ViewBag.Message = "Admin Management.";
                CompanyBL obj_AdminBL = new CompanyBL();
                // var model = new List<telerik_Users>();
                records = obj_AdminBL.ShowAllCompany(filter, page, pageSize, sort, sortdir);
                return View(records);
            }

            return RedirectToAction("Login", "Admin");

        }

        public ActionResult addCompany()
        {
            if (Session["AdminUser"] != null)
            {
                // List<subjectmodel> listsubject = new List<subjectmodel>();
                addCompany User_obj = new addCompany();
                CompanyBL obj_AdminBL = new CompanyBL();
                User_obj.isActive = true;
               // User_obj.CategoryValueSelected = obj_AdminBL.CategorySelectBind();
                User_obj.CategorywithList = obj_AdminBL.Categorytoselectlist();
                if (User_obj.CategorywithList.Count > 0)
                {
                    for (int j = 0; j < User_obj.CategorywithList.Count; j++)
                    {
                        for (int i = 0; i < User_obj.CategorywithList[j].CategoryValueList.Count; i++)
                        {
                            User_obj.CategorywithList[j].CategoryValueList[i].selected = true;
                        }                       
                    }
                }               
                return View(User_obj);
            }

            return RedirectToAction("Login", "Admin");
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult AddCompany(addCompany model)
        {
            try
            {
                tblCompany obj = new tblCompany();
                obj.name = model.name;
                obj.emailId = model.emailId;
                obj.password = model.password;
                obj.isActive = Convert.ToBoolean(model.isActive);
                obj.contactNo = model.contactNo;
                obj.contactName = model.contactName;
                obj.address = model.address;
                obj.createDate = System.DateTime.UtcNow;
                obj.city = model.city;
                obj.state = model.state;
                obj.country = model.country;

                CompanyBL obj_AdminBL = new CompanyBL();
                bool id = false;
                bool roleid = false;
                id = obj_AdminBL.AddCompany(obj);
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
                                roleid = obj_AdminBL.CategorySelectAssign(obj.id, model.CategorywithList[j].CategoryValueList[i].id, obj.createDate, true,true);
                            }
                        }
                    }
                    bool keyGiven = obj_AdminBL.KeyPoinDefaultAssign(obj.id, obj.createDate, true);
                    return RedirectToAction("CompanyManagement", "Company", new { success = "Company added successfully." });
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
        public ActionResult Edit(int id, SmartAdminMvc.Areas.Admin.Models.EditCompany updatemodel)
        {
            if (Session["AdminUser"] != null)
            {
                CompanyBL obj_AdminBL = new CompanyBL();
                addCompany adminuser = new addCompany();
               // adminuser.CategoryValueSelected = obj_AdminBL.CategorySelectBind();
                using (managementsoftwaredbEntities context = new managementsoftwaredbEntities())
                {
                    var obj = (from db in context.tblCompanies
                               where db.id == id
                               select db).ToList();
                    if (obj.Any())
                    {
                        updatemodel.id = obj[0].id;
                        updatemodel.name = obj[0].name;
                        updatemodel.emailId = obj[0].emailId;
                        updatemodel.contactName = obj[0].contactName;
                        updatemodel.contactNo = obj[0].contactNo;
                        updatemodel.address = obj[0].address;
                        updatemodel.createDate = obj[0].createDate;
                        updatemodel.isActive = Convert.ToBoolean(obj[0].isActive);
                        updatemodel.password = obj[0].password;
                        updatemodel.city = obj[0].city;
                        updatemodel.state = obj[0].state;
                        updatemodel.country = obj[0].country;
                        updatemodel.CategorywithList= obj_AdminBL.Categorytoselectlist();
                        //updatemodel.CategoryValueSelected = adminuser.CategoryValueSelected;
                        //if (updatemodel.CategoryValueSelected.Count > 0)
                        //{
                        //    List<tblcompanyCategory> dt = new List<tblcompanyCategory>();
                        //    dt = obj_AdminBL.UserSelectAssignedCategories(id);

                        //    if (dt.Any())
                        //    {
                        //        for (int i = 0; i < dt.Count; i++)
                        //        {
                        //            for (int j = 0; j < updatemodel.CategoryValueSelected.Count; j++)
                        //            {
                        //                if (updatemodel.CategoryValueSelected[j].id == dt[i].categoryId)
                        //                {
                        //                    updatemodel.CategoryValueSelected[j].selected = true;
                        //                }
                        //            }
                        //        }
                        //    }
                        //}
                        if (updatemodel.CategorywithList.Count > 0)
                        {
                            List<tblcompanyCategory> dt = new List<tblcompanyCategory>();
                            dt = obj_AdminBL.UserSelectAssignedCategories(id);

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
        public ActionResult Edit(EditCompany model, FormCollection form, int id)
        {

            try
            {
                bool roleid = false;
                string delmsg = "";
                CompanyBL obj_CompanyBL = new CompanyBL();
                addCompany adminuser = new addCompany();
                model.updateDate = DateTime.UtcNow;
                adminuser.CategoryValueSelected = obj_CompanyBL.CategorySelectBind();
                bool msg = obj_CompanyBL.UpdateCompany(model, id);
                if (msg != false)
                {
                 
                    if (id != null)
                    {
                         delmsg = obj_CompanyBL.CompnaySelectedCategoryDelete(id);
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
                                    roleid = obj_CompanyBL.CategorySelectAssign(id, model.CategorywithList[j].CategoryValueList[i].id, model.updateDate, true, false);
                                }
                            }
                        }
                        //bool keyGiven = obj_CompanyBL.KeyPoinDefaultAssign(id, model.updateDate, true);
                    }
                    string page = "";
                    if (Request.QueryString["page"] != null)
                    {
                        page = Request.QueryString["page"].ToString();
                    }
                    return RedirectToAction("CompanyManagement", "Company", new { success = "Company details updated successfully.", page = page });
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
        public ActionResult Delete(int id)
        {

            CompanyBL obj_CompanyBL = new CompanyBL();
            string msg = obj_CompanyBL.UserDelete(id);
            string page = "";
            if (Request.QueryString["page"] != null)
            {
                page = Request.QueryString["page"].ToString();
            }
            if (msg != "")
            {
                //return RedirectToAction("CompanyManagement", "Company", new { success = "Company deleted successfully." });
                return RedirectToAction("CompanyManagement", "Company", new { success = msg, page = page });
            }
            else
            {
                //ModelState.AddModelError("", "Can not delete");
                //return View("Index");
                return RedirectToAction("CompanyManagement", "Company", new { fail = "Company Can not be deleted, It has data with Budget.", page = page }); 
             }
        }
        private Exception ErrorCodeToString(MembershipCreateStatus statusCode)
        {
            throw new NotImplementedException();
        }
    }
}