using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SmartAdminMvc.Models;
using System.Linq.Dynamic;
using System.Net;

namespace SmartAdminMvc.BL
{
    public class UserCompnyBL
    {
        public List<Company> CheckUserLogin(string USerNM, string password)
        {
            try
            {
                using (managementsoftwaredbEntities context = new managementsoftwaredbEntities())
                {
                    var obj = (from us in context.tblCompanies
                               where us.emailId == USerNM
                               && us.password == password
                               select new Company
                               {
                                   name = us.emailId,
                                   id=us.id,
                                   
                               }).ToList();
                    return obj;

                }
            }
            catch (Exception e)
            {
                throw e;
            }
            finally { }
        }

        public List<Company> CheckUserEmailEXist(string USerNM)
        {
            try
            {
                using (managementsoftwaredbEntities context = new managementsoftwaredbEntities())
                {
                    var obj = (from us in context.tblCompanies
                               where us.emailId == USerNM                           
                               select new Company
                               {
                                   name = us.emailId,
                                   password= us.password,
                                   id = us.id,

                               }).ToList();
                    return obj;

                }
            }
            catch (Exception e)
            {
                throw e;
            }
            finally { }
        }
        public bool UpdateCompany(Company model, int id)
        {

            try
            {
                using (managementsoftwaredbEntities context = new managementsoftwaredbEntities())
                {
                    var query = context.tblCompanies.Where(x => x.id == id).FirstOrDefault();
                    if (query != null)
                    {
                        query.name = model.name;
                        query.emailId = model.emailId;
                        query.password = model.password;
                        query.contactName = model.contactName;
                        query.contactNo = model.contactNo;
                        query.address = model.address;
                        query.updateDate = model.updateDate;
                        query.isActive = Convert.ToBoolean(model.isActive);
                        query.city = model.city;
                        query.state = model.state;
                        query.country = model.country;
                        context.SaveChanges();
                        return true;
                    }
                }
            }
            catch (Exception)
            {
                return false;
            }
            return false;
        }
        public List<PagesList> ContentpageListfetch()
        {
            try
            {
                // var budget_TypeList = new List<SelectListItem>();
                var ContentpageList = new List<PagesList>();
                using (managementsoftwaredbEntities context = new managementsoftwaredbEntities())
                {
                    var obj = (from db in context.tblContentPages
                               where db.isActive == true
                               select db).ToList();
                    if (obj.Any())
                    {

                        for (int i = 0; i < obj.Count; i++)
                        {
                            var ContentpageList_Item = new PagesList();
                            ContentpageList_Item.name = obj[i].title;
                            ContentpageList_Item.id = obj[i].id;
                            ContentpageList.Add(ContentpageList_Item);
                        }
                    }
                }
                return ContentpageList;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public void SendMail(string from, string Recipients, string RecipientsCC, string RecipientsBCC, string mailbody, string Subject, string user, string password, string smtpServer)
        {
            System.Net.Mail.MailMessage message = new System.Net.Mail.MailMessage();
            message.To.Add(Recipients);
            if (RecipientsCC != "")
                message.CC.Add(RecipientsCC);
            if (RecipientsBCC != "")
                message.Bcc.Add(RecipientsBCC);
            message.Subject = Subject;
            message.From = new System.Net.Mail.MailAddress(from);
            message.Body = mailbody;
            message.BodyEncoding = System.Text.UTF8Encoding.UTF8;
            message.SubjectEncoding = System.Text.UTF8Encoding.UTF8;
            message.IsBodyHtml = true;
            System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient("mail.webtechsystem.com", 25);
            smtp.Credentials = new NetworkCredential("donotdelete@takeeat.com", "D0notDe1et0@7");
            try { smtp.Send(message); }
            catch (Exception ex) { }
        }
    }
}