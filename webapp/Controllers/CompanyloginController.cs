using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SmartAdminMvc.Models;
using System.Web.Security;
using SmartAdminMvc.BL;

namespace SmartAdminMvc.Controllers
{
    public class CompanyloginController : Controller
    {
        // GET: Company
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Companylogin()
        {
            UserCompnyBL obj_compBL = new UserCompnyBL();
            CompanyLogin objlogin = new CompanyLogin();
            objlogin.ListOfPages = obj_compBL.ContentpageListfetch();
            return View(objlogin);
            //return View();
        }

        public ActionResult ForgotPassword()
        {
            UserCompnyBL obj_compBL = new UserCompnyBL();
            CompanyLogin objlogin = new CompanyLogin();
            objlogin.ListOfPages = obj_compBL.ContentpageListfetch();
            return View(objlogin);
            //return View();
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ForgotPassword(CompanyLogin objlogin, string returnUrl)
        {
            UserCompnyBL obj_AdminBL = new UserCompnyBL();
            List<Company> dt = new List<Company>();
            dt = obj_AdminBL.CheckUserEmailEXist(objlogin.emailId);
            if (dt.Count > 0)
            {
                obj_AdminBL.SendMail("mohit@webtechsystem.com", "samarth.abbacus@gmail.com", "", "", "Hello, <br/> <br/> Your username is : " + dt[0].name.ToString() + ".<br/> Your password is : " + dt[0].password.ToString() + ". <br/> <br/> Thank You.", "Forgot Password", "", "","");
                //return RedirectToLocal(returnUrl);
                ModelState.AddModelError("", "The user name and password sent to your email address.");
            }
             UserCompnyBL obj_compBL = new UserCompnyBL();
            objlogin.ListOfPages = obj_compBL.ContentpageListfetch();
            return View(objlogin);
        }
        public ActionResult CompanyHome()
        {
            if (Session["CompanyUser"] != null)
            {
                return View();
            }
            return RedirectToAction("Companylogin", "Companylogin");
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Companylogin(CompanyLogin objlogin, string returnUrl)
        {
            UserCompnyBL obj_AdminBL = new UserCompnyBL();
            List<Company> dt = new List<Company>();
            dt = obj_AdminBL.CheckUserLogin(objlogin.emailId, objlogin.password);
            if (dt.Count > 0)
            {
                Session["CompanyUser"] = dt[0].name.ToString();
                Session["CompanyId"] = dt[0].id;
                //return RedirectToLocal(returnUrl);
                return RedirectToAction("CompanyHome", "Companylogin");
            }

            ModelState.AddModelError("", "The user name or password provided is incorrect.");
            UserCompnyBL obj_compBL = new UserCompnyBL();
            objlogin.ListOfPages = obj_compBL.ContentpageListfetch();
            return View(objlogin);
        }
        [AllowAnonymous]
        public ActionResult Logout(string returnUrl)
        {
            Session["CompanyUser"] = null;
            Session["CompanyId"] = null;
            Session.Abandon();
            return RedirectToAction("Companylogin", "Companylogin", new { Logout = "LogOut" });
        }

        [HttpGet]
        public ActionResult CompanyProfile(Company updatemodel)
        {
            if (Session["CompanyUser"] != null)
            {
                UserCompnyBL obj_AdminBL = new UserCompnyBL();
                Company adminuser = new Company();
                int id= Convert.ToInt32(Session["CompanyId"].ToString());
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
                        updatemodel.city = obj[0].city;
                        updatemodel.state = obj[0].state;
                        updatemodel.country = obj[0].country;
                        updatemodel.address = obj[0].address;
                        updatemodel.createDate = obj[0].createDate;
                        updatemodel.isActive = Convert.ToBoolean(obj[0].isActive);
                        updatemodel.password = obj[0].password;
                        return View(updatemodel);
                    }

                }

            }

            return RedirectToAction("Companylogin", "Companylogin");
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult CompanyProfile(Company model, FormCollection form)
        {

            try
            {
                int id = Convert.ToInt32(Session["CompanyId"].ToString());
                UserCompnyBL obj_CompanyBL = new UserCompnyBL();
                Company adminuser = new Company();
                model.updateDate = DateTime.UtcNow;
                model.isActive = true;
                bool msg = obj_CompanyBL.UpdateCompany(model, id);
                if (msg != false)
                {
                    return RedirectToAction("CompanyProfile", "Companylogin", new { success = "Profile updated successfully." });
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

        private ActionResult RedirectToLocal(string returnUrl)
        {
            throw new NotImplementedException();
        }

      
    }
}