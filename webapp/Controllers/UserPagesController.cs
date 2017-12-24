using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SmartAdminMvc.BL;
using System.Data.SqlClient;
using System.Data;
using SmartAdminMvc.Models;

namespace SmartAdminMvc.Controllers
{
    public class UserPagesController : Controller
    {
        [HttpGet]
        // GET: UserPages
        public ActionResult Index(int id = 1)
        {
            UserCompnyBL obj_userCompanyBL = new UserCompnyBL();
            using (managementsoftwaredbEntities context = new managementsoftwaredbEntities())
            {
                var obj = (from db in context.tblContentPages
                           where db.id == id
                           select db).ToList();
                if (obj.Any())
                {
                    UserPagesModels objtblContentPage = new UserPagesModels();
                    objtblContentPage.id = obj[0].id;
                    objtblContentPage.name = obj[0].title;
                    objtblContentPage.description = obj[0].descpriction;
                    objtblContentPage.status = obj[0].isActive.Value;
                    objtblContentPage.ListOfPages = obj_userCompanyBL.ContentpageListfetch();
                    return View(objtblContentPage);
                }
            }

            return View();
        }
    }
}