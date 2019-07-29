using System;
using System.Data.Entity.Core;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using CampusManagement.Models;
//using VBClassLibrary;

namespace CampusManagement.Controllers
{
    public class HomeController : Controller
    {
        ModelUserManagementContainer dbUser = new ModelUserManagementContainer();
        ModelCMSContainer dbCMS = new ModelCMSContainer();
        //Encryption encryption = new Encryption();

        public ActionResult Index()
        {
            //try
            //{
            //    //Configuring webMail class to send emails  
            //    //gmail smtp server  
            //    WebMail.SmtpServer = "smtp.gmail.com";
            //    //gmail port to send emails  
            //    WebMail.SmtpPort = 587;
            //    WebMail.SmtpUseDefaultCredentials = true;
            //    //sending emails with secure protocol  
            //    WebMail.EnableSsl = true;
            //    //EmailId used to send emails from application  
            //    WebMail.UserName = "youraccount@gmail.com";
            //    WebMail.Password = "password";

            //    string body = "<strong>Email From: {0} ({1}) <br />Message:{2}<br /><br /><br />Regards,<br />{3}</strong>";
            //    //Send email  
            //    WebMail.Send(to: "farooq.39@gmail.com", subject: "Subject", body: string.Format(body, "Muhammad Farooq", "farooq.39@gmail.com", "This is my first email from C#.", "Muhammad Farooq"), cc: null, bcc: null, isBodyHtml: true);
            //    ViewBag.Status = "Email Sent Successfully.";
            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}
            ViewBag.Selected = new SelectList(dbCMS.Cities, "CityID", "CityName");

            return View();
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult ContactUs()
        {
            return View();
        }

        public ActionResult Login1()
        {
            return View();
        }

        public ActionResult Login2()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login2(GetLoginUser_Result user, string returnURL)
        {
            using (dbUser)
            {
                //Code for Decryption
                //GetLoginUser_Result usr = dbUser.GetLoginUser(user.UserName, encryption.Encrypt_Main(user.Password, true)).FirstOrDefault();
                GetLoginUser_Result usr = dbUser.GetLoginUser(user.UserName, user.Password).FirstOrDefault();
                if (usr != null)
                {
                    FormsAuthentication.SetAuthCookie(usr.UserName, false);
                    Session.Add("emp_id", usr.EmpID);
                    Session.Add("dept_id", usr.DeptID);
                    Session.Add("SubDeptid", usr.SubDeptId);
                    Session.Add("empName", usr.EFName);
                    Session.Add("Designation", usr.Designation_Name);
                    Session.Add("deptName", usr.Dept_Name);
                    Session.Add("SubDeptName", usr.SubDept_Name);
                    Session.Add("Designation_ID", usr.DesignationID);
                    Session.Add("UserName", usr.UserName);

                    if (Url.IsLocalUrl(returnURL)
                        && returnURL.Length > 1
                        && returnURL.StartsWith("/")
                        && !returnURL.StartsWith("//")
                        && !returnURL.StartsWith("/\\"))
                    {
                        return Redirect(returnURL);
                    }
                    else
                    {
                        return RedirectToAction("Index");
                    }
                }
                else
                {
                    ViewBag.ErrorMessage = "User Name or Password is incorrect.";
                    return View();
                }
            }
        }

        [Authorize]
        public ActionResult SignOut()
        {
            FormsAuthentication.SignOut();
            Session.Clear();
            return RedirectToAction("Login2", "Home");
        }

        public ActionResult Dashboard()
        {
            return View();
        }

        public ActionResult RegisterApplicant()
        {
            return View();
        }

        [HttpPost]
        public ActionResult RegisterApplicant(User user)
        {
            string ErrorMessage = "";
            int count = 0;
            var us = dbUser.Logins.FirstOrDefault(u => u.UserName == user.UserName);

            try
            {
                if (user.UserName.Trim().Contains(" "))
                {
                    count++;
                    ErrorMessage += count + "-User Name cannot contain spaces.<br />";
                    ModelState.AddModelError(string.Empty, "User Name cannot contain spaces.");
                    ViewBag.MessageType = "error";
                }

                if (user.UserPassword.Trim().Contains(" ") || user.ReEnterPassword.Trim().Contains(" "))
                {
                    count++;
                    ErrorMessage += count + "-Passwords cannot contain spaces.<br />";
                    ModelState.AddModelError(string.Empty, "Passwords cannot contain spaces.");
                    ViewBag.MessageType = "error";
                }

                if (!user.UserPassword.Trim().Equals(user.ReEnterPassword.Trim()))
                {
                    count++;
                    ErrorMessage += count + "-Passwords must match.<br />";
                    ModelState.AddModelError(string.Empty, "Passwords must match.");
                    ViewBag.MessageType = "error";
                }

                if (us != null)
                {
                    count++;
                    ErrorMessage += count + "-User Name already exists.<br />";
                    ModelState.AddModelError(string.Empty, "User Name already exists.");
                    ViewBag.MessageType = "error";
                }

                if (!string.IsNullOrEmpty(ErrorMessage))
                {
                    ViewBag.ErrorMessage = ErrorMessage;
                }
                else
                {
                    if (us == null)
                    {
                        try
                        {
                            dbUser.InsertApplicantLoginWithPages(user.UserName, user.UserPassword, "Applicant");
                            ViewBag.MessageType = "success";
                            ViewBag.ErrorMessage = "You are registered successfully, use these credentials while applying online.";
                        }
                        catch (EntityCommandExecutionException ex)
                        {
                            ViewBag.MessageType = "error";
                            ViewBag.ErrorMessage = ex.Message;
                            ModelState.AddModelError(string.Empty, ex.Message);
                        }
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                foreach (DbEntityValidationResult validationResult in ex.EntityValidationErrors)
                {
                    string entityName = validationResult.Entry.Entity.GetType().Name;
                    foreach (DbValidationError error in validationResult.ValidationErrors)
                    {
                        ModelState.AddModelError(string.Empty, error.ErrorMessage);
                        count++;
                        ErrorMessage += count + "-" + string.Concat(error.PropertyName, " is required.") + "<br />";
                    }
                }
                ViewBag.MessageType = "error";
                ViewBag.ErrorMessage = ErrorMessage;
            }

            return View();
        }
    }
}