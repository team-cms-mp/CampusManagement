using System;
using System.Data.Entity.Core;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using CampusManagement.Models;
using System.Web.Helpers;
using System.Web;
using System.IO;
using System.Net.Mail;
using System.Net;
using System.Collections.Generic;
using System.Configuration;
using Newtonsoft.Json;

namespace CampusManagement.Controllers
{
    public class HomeController : Controller
    {
        ModelCMSNewContainer db = new ModelCMSNewContainer();

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
            ViewBag.Selected = new SelectList(db.Cities, "CityID", "CityName");

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
        public ActionResult Login2(GetLoginUser_Result user, string Password, string returnURL)
        {
            using (db)
            {
                //Code for Decryption
                //GetLoginUser_Result usr = dbUser.GetLoginUser(user.UserName, encryption.Encrypt_Main(user.Password, true)).FirstOrDefault();
                GetLoginUser_Result usr = db.GetLoginUser(user.UserName, Password, user.Designation_Name).FirstOrDefault();
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

                    string empType = "Employee";
                    if (usr.Designation_Name.Trim() == "Applicant")
                    {
                        empType = "Applicant";
                    }
                    else if (usr.Designation_Name.Trim() == "Student")
                    {
                        empType = "Student";
                    }

                    um_GetCurrentUserID_Result objCurrentUser = db.um_GetCurrentUserID(usr.EmpID, Password, empType).FirstOrDefault();
                    if (objCurrentUser != null)
                    {
                        Session.Add("CurrentUserID", objCurrentUser.CurrentUserID);
                        Session.Add("FormNo", objCurrentUser.FormNo);
                    }

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
                    ViewBag.MessageType = "error";
                    ViewBag.ErrorMessage = "User Name or Password is incorrect.";
                    return View();
                }
            }
        }

        public ActionResult Login3()
        {
            ViewBag.UserName = Session["UserName"];
            return View();
        }

        //Login3 for LMS
        [HttpPost]
        public ActionResult Login3(GetLoginUser_Result user, string Password, string returnURL)
        {
            using (db)
            {
                //Code for Decryption
                //GetLoginUser_Result usr = dbUser.GetLoginUser(user.UserName, encryption.Encrypt_Main(user.Password, true)).FirstOrDefault();
                GetLoginUser_Result usr = db.GetLoginUser(user.UserName.Trim(), Password.Trim(), "Student").FirstOrDefault();
                if (usr != null)
                {
                    FormsAuthentication.SetAuthCookie(usr.UserName, false);

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
                        return RedirectToAction("StudentActiveCourses", "LmsStudentActiveCourses");
                    }
                }
                else
                {
                    ViewBag.ErrorMessage = "User Name or Password is incorrect.";
                    return View();
                }
            }
        }


        public ActionResult Login4()
        {
            ViewBag.UserName = Session["UserName"];
            return View();
        }


        [HttpPost]
        public ActionResult Login4(GetLoginUser_Result user, string Password, string returnURL)
        {
            using (db)
            {
                //var usr = dbCMS.Logins.FirstOrDefault(u => u.UserName == user.UserName && u.Password == user.Password && u.EmpType == "AlumniUser");
                //GetLoginUserAlumni_Result usr = dbCMS.GetLoginUserAlumni(user.UserName.Trim(), user.Password.Trim()).FirstOrDefault();
                GetLoginUser_Result usr = db.GetLoginUser(user.UserName.Trim(), Password.Trim(), "AlumniUser").FirstOrDefault();
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

                    string empType = "Employee";
                    if (usr.Designation_Name.Equals("Applicant"))
                    {
                        empType = "Applicant";
                    }
                    else if (usr.Designation_Name.Equals("Student"))
                    {
                        empType = "Student";
                    }

                    um_GetCurrentUserID_Result objCurrentUser = db.um_GetCurrentUserID(usr.EmpID, Password, empType).FirstOrDefault();
                    if (objCurrentUser != null)
                    {
                        Session.Add("CurrentUserID", objCurrentUser.CurrentUserID);
                        Session.Add("FormNo", objCurrentUser.FormNo);
                    }
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
                        return RedirectToAction("AlumniAccount", "Alumni");
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
            ViewBag.CountryCode = new SelectList(db.Countries, "CountryCode", "CountryCode");
            return View();
        }

        [HttpPost]
        public ActionResult RegisterApplicant(User user)
        {
            Boolean IsToRemove = true;
            string ErrorMessage = "";
            int count = 0;
            Login us = new Login();
            Applicant ap = new Applicant();

            try
            {
                if (user.UserName.Trim().Contains(" "))
                {
                    count++;
                    ErrorMessage += count + "-User Name cannot contain spaces.<br />";
                    ViewBag.MessageType = "error";
                }

                if (user.UserPassword.Trim().Contains(" ") || user.ReEnterPassword.Trim().Contains(" "))
                {
                    count++;
                    ErrorMessage += count + "-Passwords cannot contain spaces.<br />";
                    ViewBag.MessageType = "error";
                }

                if (!user.UserPassword.Trim().Equals(user.ReEnterPassword.Trim()))
                {
                    count++;
                    ErrorMessage += count + "-Passwords must match.<br />";
                    ViewBag.MessageType = "error";
                }

                us = db.Logins.FirstOrDefault(u => u.UserName == user.UserName);
                if (us != null)
                {
                    count++;
                    ErrorMessage += count + "-User Name already exists.<br />";
                    ViewBag.MessageType = "error";
                }

                //us = dbCMS.Logins.FirstOrDefault(u => u.Email == user.Email);
                //ap = dbCMS.Applicants.FirstOrDefault(a => a.Email == user.Email);
                //if (us != null)
                //{
                //    count++;
                //    ErrorMessage += count + "-Email already exists.<br />";
                //    ViewBag.MessageType = "error";
                //}
                //else if (ap != null)
                //{
                //    count++;
                //    ErrorMessage += count + "-Email already exists.<br />";
                //    ViewBag.MessageType = "error";
                //}

                //us = dbCMS.Logins.FirstOrDefault(u => u.CNIC == user.CNIC);
                //ap = dbCMS.Applicants.FirstOrDefault(a => a.ACNIC == user.CNIC);
                //if (us != null)
                //{
                //    count++;
                //    ErrorMessage += count + "-CNIC already exists.<br />";
                //    ViewBag.MessageType = "error";
                //}
                //else if (ap != null)
                //{
                //    count++;
                //    ErrorMessage += count + "-CNIC already exists.<br />";
                //    ViewBag.MessageType = "error";
                //}

                if (!string.IsNullOrEmpty(ErrorMessage))
                {
                    ViewBag.ErrorMessage = ErrorMessage;
                    IsToRemove = false;
                    ViewBag.scripCall = "0";
                    ViewBag.CountryCode = new SelectList(db.Countries, "CountryCode", "CountryCode");
                    return View();
                }
                else
                {
                    try
                    {
                        int MaxEmployeeID = Convert.ToInt32(db.um_GetMaxEmployeeID().FirstOrDefault());

                        db.InsertApplicantLoginWithPages(user.UserName, user.UserPassword, "Applicant", user.Email, user.MobileNumber, user.CNIC, MaxEmployeeID, user.CountryCode.Trim());
                        ViewBag.MessageType = "success";
                        ViewBag.ErrorMessage = "You are registered successfully, use these credentials while applying online.";

                    }
                    catch (EntityCommandExecutionException ex)
                    {
                        ViewBag.MessageType = "error";
                        ViewBag.ErrorMessage = ex.InnerException.ToString();
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
            if (IsToRemove == true)
            {
                ViewBag.scripCall = "1";
            }
            else
            {
                ViewBag.scripCall = "0";
            }
            ViewBag.CountryCode = new SelectList(db.Countries, "CountryCode", "CountryCode");

            return View("Login2");
        }

        [NonAction]
        public void SendVerificationLinkEmail(string emailID, string activationCode, string emailFor = "VerifyAccount")
        {
            var verifyUrl = "/User/" + emailFor + "/" + activationCode;
            var link = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, verifyUrl);

            var fromEmail = new MailAddress("dotnetawesome@gmail.com", "Dotnet Awesome");
            var toEmail = new MailAddress(emailID);
            var fromEmailPassword = "******"; // Replace with actual password

            string subject = "";
            string body = "";
            if (emailFor == "VerifyAccount")
            {
                subject = "Your account is successfully created!";
                body = "<br/><br/>We are excited to tell you that your Dotnet Awesome account is" +
                    " successfully created. Please click on the below link to verify your account" +
                    " <br/><br/><a href='" + link + "'>" + link + "</a> ";
            }
            else if (emailFor == "ResetPassword")
            {
                subject = "Reset Password";
                body = "Hi,<br/>br/>We got request for reset your account password. Please click on the below link to reset your password" +
                    "<br/><br/><a href=" + link + ">Reset Password link</a>";
            }


            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromEmail.Address, fromEmailPassword)
            };

            using (var message = new MailMessage(fromEmail, toEmail)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            })
                smtp.Send(message);
        }

        [HttpGet]


        public ActionResult ForgotPassword()
        {

            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ForgotPassword(string ToEmail)
        {
            Login login = db.Logins.FirstOrDefault(a => a.Email == ToEmail);

            if (login != null)
            {
                string strBody = "Your password is : " + login.Password;
                string EmailSubject = "Forget Password";

                SendEmail(EmailSubject, strBody, ToEmail);

                ViewBag.MessageType = "success";
                ViewBag.ErrorMessage = "Password sent successfully.";
            }
            else
            {
                ViewBag.MessageType = "error";
                ViewBag.ErrorMessage = "Given email is not registered.";
            }
            return View();
        }

        public static string SendEmail(string EmailSubject, string EmailBody, string ToEmail)
        {
            try
            {
                string FromEmail = (ConfigurationManager.AppSettings["FromEmail"] == null || ConfigurationManager.AppSettings["FromEmail"] == "") ? "" : Convert.ToString(ConfigurationManager.AppSettings["FromEmail"]);
                string FromEmailPassword = (ConfigurationManager.AppSettings["FromEmailPassword"] == null || ConfigurationManager.AppSettings["FromEmailPassword"] == "") ? "" : Convert.ToString(ConfigurationManager.AppSettings["FromEmailPassword"]);

                using (MailMessage mail = new MailMessage(FromEmail, ToEmail))
                {
                    mail.Subject = EmailSubject;
                    mail.Body = EmailBody;
                    mail.IsBodyHtml = true;

                    SmtpClient smtp = new SmtpClient();

                    smtp.Host = "smtp.gmail.com";

                    smtp.EnableSsl = true;

                    NetworkCredential networkCredential = new NetworkCredential(FromEmail, FromEmailPassword);

                    smtp.UseDefaultCredentials = true;

                    smtp.Credentials = networkCredential;

                    smtp.Port = 587;

                    try
                    {
                        smtp.Send(mail);
                    }
                    catch (Exception)
                    {

                    }

                    return "Email sent successfully";
                }
            }
            catch (Exception ex)
            {
                return "Problem sending email, Exception: " + ex.Message;
            }
        }

        public JsonResult GetTests()
        {
            List<EntryTest> lstTests = new List<EntryTest>();

            lstTests = db.EntryTests.Where(t => t.EntryTestName.StartsWith("Test")).ToList();
            var tests = lstTests.Select(S => new
            {
                EntryTestID = S.EntryTestID,
                EntryTestName = S.EntryTestName,
            });
            string result = JsonConvert.SerializeObject(tests, Formatting.Indented);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public string UpdateTestByFormNo(string FormNo, int EntryTestID)
        {
            db.UpdateTestByFormNo(FormNo, EntryTestID, 1);

            EntryTest et = db.EntryTests.FirstOrDefault(t => t.EntryTestID == EntryTestID);


            return et.EntryTestName;
        }
    }
}