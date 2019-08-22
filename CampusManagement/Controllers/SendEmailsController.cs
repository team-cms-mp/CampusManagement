using CampusManagement.App_Code;
using CampusManagement.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace CampusManagement.Controllers
{
    public class SendEmailsController : Controller
    {
        private ModelCMSNewContainer db = new ModelCMSNewContainer();

        public ActionResult EntryTestEmail()
        {
            ViewBag.ApplicantList = db.Applicants.Where(x => x.BatchProgramID == 0).ToList();
            ViewBag.BatchID = new SelectList(db.Batches, "BatchID", "BatchName");
            ViewBag.BatchProgramID = new SelectList(db.GetBatchProgramNameConcat("", 0), "ID", "Name");
            return View();
        }

        [HttpPost]
        public ActionResult EntryTestEmail(int BatchID, int BatchProgramID)
        {
            if (BatchProgramID == 0)
            {
                ViewBag.MessageType = "error";
                ViewBag.Message = "Please select program.";
            }
            ViewBag.hdnBatchProgramID = BatchProgramID;
            ViewBag.hdnBatchID = BatchID;

            ViewBag.BatchID = new SelectList(db.Batches, "BatchID", "BatchName");
            ViewBag.BatchProgramID = new SelectList(db.GetBatchProgramNameConcat("", 0), "ID", "Name");

            return View();
        }

        public ActionResult EntryTestEmailSent()
        {
            ViewBag.BatchID = new SelectList(db.Batches, "BatchID", "BatchName");
            ViewBag.BatchProgramID = new SelectList(db.GetBatchProgramNameConcat("", 0), "ID", "Name");
            return View();
        }


        [HttpPost]
        public ActionResult EntryTestEmailSent(int BatchID, int BatchProgramID)
        {
           if (BatchProgramID == 0)
            {
                ViewBag.MessageType = "error";
                ViewBag.Message = "Please select program.";
            }
            ViewBag.hdnBatchProgramID = BatchProgramID;
            ViewBag.hdnBatchID = BatchID;
            ViewBag.BatchID = new SelectList(db.Batches, "BatchID", "BatchName");
            ViewBag.BatchProgramID = new SelectList(db.GetBatchProgramNameConcat("", 0), "ID", "Name");

            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SendEntryTestEmail(List<HttpPostedFileBase> fileUploader, int hdnBatchProgramID, string EmailBody, string txtEntryTestDate, string txtEntryTestTime, FormCollection fc, int hdnBatchID)
        {
            try
            {
                int count = Convert.ToInt32(fc["hdnRowCount"]);

                string FormNo = "";
                string ApplicantName = "";
                string FatherName = "";
                string CellNo = "";
                string Email = "";
                string checkedbox = "";

                if (count == 0)
                {
                    ViewBag.MessageType = "error";
                    ViewBag.Message = "No record found to send email.";
                }
                else
                {
                    Program Prog = null;
                    Batch batch = null;
                    BatchProgram BatchProg = db.BatchPrograms.FirstOrDefault(x => x.BatchProgramID == hdnBatchProgramID);
                    if (BatchProg != null)
                    {
                        Prog = db.Programs.FirstOrDefault(x => x.ProgramID == BatchProg.ProgramID);
                        batch = db.Batches.FirstOrDefault(x => x.BatchID == BatchProg.BatchID);

                        for (int i = 1; i <= count; i++)
                        {
                            FormNo = fc["FormNo_" + i];
                            ApplicantName = fc["ApplicantName_" + i];
                            FatherName = fc["FatherName_" + i];
                            CellNo = fc["CellNo_" + i];
                            Email = fc["Email_" + i];
                            checkedbox = fc["checkedbox_" + i];
                            string strBody = EmailBody.Replace("###CurrentDate", DateTime.Now.ToShortDateString()).Replace("###ApplicantName", ApplicantName).Replace("###FatherName", FatherName).Replace("###FormNo", FormNo).Replace("###ProgramName", Prog.ProgramName).Replace("###BatchSemester ", batch.BatchName).Replace("###DepositDate", Convert.ToDateTime(txtEntryTestDate).ToShortDateString()).Replace("###EntryTestTime", txtEntryTestTime);
                            if (checkedbox == "Yes")
                            {
                                SendEmail(fileUploader, "Entry Test Email", strBody, Email, FormNo, "EntryTestEmailSent", checkedbox);
                            }
                        }
                        ViewBag.MessageType = "success";
                        ViewBag.Message = "Email sent successfully.";
                    }
                }

                ViewBag.hdnBatchProgramID = hdnBatchProgramID;
                ViewBag.hdnBatchID = hdnBatchID;
                ViewBag.BatchID = new SelectList(db.Batches, "BatchID", "BatchName");
                ViewBag.BatchProgramID = new SelectList(db.GetBatchProgramNameConcat("", 0), "ID", "Name");
            }
            catch (Exception ex)
            {
                ViewBag.MessageType = "error";
                ViewBag.Message = "Problem while sending email, Please check details, Exception: " + ex.Message;
            }
            return RedirectToAction("EntryTestEmail");
        }

        [HttpGet]
        public ActionResult OfferLetterEmail()
        {
            ViewBag.ApplicantList = db.Applicants.Where(x => x.BatchProgramID == 0).ToList();
            ViewBag.BatchID = new SelectList(db.Batches, "BatchID", "BatchName");
            ViewBag.BatchProgramID = new SelectList(db.GetBatchProgramNameConcat("", 0), "ID", "Name");
            return View();
        }

        [HttpPost]
        public ActionResult OfferLetterEmail(int BatchID, int BatchProgramID)
        {
            if (BatchProgramID == 0)
            {
                ViewBag.MessageType = "error";
                ViewBag.Message = "Please select program.";
            }
            ViewBag.hdnBatchProgramID = BatchProgramID;
            ViewBag.hdnBatchID = BatchID;
            ViewBag.BatchID = new SelectList(db.Batches, "BatchID", "BatchName");
            ViewBag.BatchProgramID = new SelectList(db.GetBatchProgramNameConcat("", 0), "ID", "Name");

            return View();
        }

        public ActionResult OfferLetterEmailSent()
        {
            ViewBag.ApplicantList = db.Applicants.Where(x => x.BatchProgramID == 0).ToList();
            ViewBag.BatchID = new SelectList(db.Batches, "BatchID", "BatchName");
            ViewBag.BatchProgramID = new SelectList(db.GetBatchProgramNameConcat("", 0), "ID", "Name");
            return View();
        }

        [HttpPost]
        public ActionResult OfferLetterEmailSent(int BatchID, int BatchProgramID)
        {
            if (BatchProgramID == 0)
            {
                ViewBag.MessageType = "error";
                ViewBag.Message = "Please select program.";
            }
            ViewBag.hdnBatchProgramID = BatchProgramID;
            ViewBag.hdnBatchID = BatchID;
            ViewBag.BatchID = new SelectList(db.Batches, "BatchID", "BatchName");
            ViewBag.BatchProgramID = new SelectList(db.GetBatchProgramNameConcat("", 0), "ID", "Name");

            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SendOfferLetterEmail(List<HttpPostedFileBase> fileUploader, int hdnBatchProgramID, string EmailBody, string DepositFee, string DepositDate, FormCollection fc, int hdnBatchID)
        {
            try
            {
                int count = Convert.ToInt32(fc["hdnRowCount"]);

                if (count == 0)
                {
                    ViewBag.MessageType = "error";
                    ViewBag.Message = "No record found to send email.";
                }
                else
                {
                    Program Prog = null;
                    Batch batch = null;
                    BatchProgram BatchProg = db.BatchPrograms.FirstOrDefault(x => x.BatchProgramID == hdnBatchProgramID);
                    if (BatchProg != null)
                    {
                        Prog = db.Programs.FirstOrDefault(x => x.ProgramID == BatchProg.ProgramID);
                        batch = db.Batches.FirstOrDefault(x => x.BatchID == BatchProg.BatchID);

                        string FormNo = "";
                        string ApplicantName = "";
                        string FatherName = "";
                        string CellNo = "";
                        string Email = "";
                        string checkedbox = "";

                        for (int i = 1; i <= count; i++)
                        {
                            FormNo = fc["FormNo_" + i];
                            ApplicantName = fc["ApplicantName_" + i];
                            FatherName = fc["FatherName_" + i];
                            CellNo = fc["CellNo_" + i];
                            Email = fc["Email_" + i];
                            checkedbox = fc["checkedbox_" + i];
                            string strBody = EmailBody.Replace("###CurrentDate", DateTime.Now.ToShortDateString()).Replace("###ApplicantName", ApplicantName).Replace("###FatherName", FatherName).Replace("###FormNo", FormNo).Replace("###ProgramName", Prog.ProgramName).Replace("###BatchSemester", batch.BatchName).Replace("###DepositDate", Convert.ToDateTime(DepositDate).ToShortDateString()).Replace("###DepositFee", DepositFee);
                            if (checkedbox == "Yes")
                            {
                                SendEmail(fileUploader, "Offer Letter Email", strBody, Email, FormNo, "OfferLetterEmailSent", checkedbox);
                            }

                        }
                        ViewBag.MessageType = "success";
                        ViewBag.Message = "Email sent successfully.";
                    }
                }

                ViewBag.hdnBatchProgramID = hdnBatchProgramID;
                ViewBag.hdnBatchID = hdnBatchID;
                ViewBag.BatchID = new SelectList(db.Batches, "BatchID", "BatchName");
                ViewBag.BatchProgramID = new SelectList(db.GetBatchProgramNameConcat("", 0), "ID", "Name");
            }
            catch (Exception ex)
            {
                ViewBag.MessageType = "error";
                ViewBag.Message = "Problem while sending email, Please check details, Exception: " + ex.Message;
            }
            return RedirectToAction("OfferLetterEmail");
        }


        public ActionResult OrientationEmail()
        {
            ViewBag.ApplicantList = db.Applicants.Where(x => x.BatchProgramID == 0).ToList();
            ViewBag.BatchID = new SelectList(db.Batches, "BatchID", "BatchName");
            ViewBag.BatchProgramID = new SelectList(db.GetBatchProgramNameConcat("", 0), "ID", "Name");
            return View();
        }

        [HttpPost]
        public ActionResult OrientationEmail(int BatchID, int BatchProgramID)
        {
            if (BatchProgramID == 0)
            {
                ViewBag.MessageType = "error";
                ViewBag.Message = "Please select program.";
            }
            ViewBag.hdnBatchProgramID = BatchProgramID;
            ViewBag.hdnBatchID = BatchID;
            ViewBag.BatchID = new SelectList(db.Batches, "BatchID", "BatchName");
            ViewBag.BatchProgramID = new SelectList(db.GetBatchProgramNameConcat("", 0), "ID", "Name");

            return View();
        }

        public ActionResult OrientationEmailSent()
        {
            ViewBag.BatchID = new SelectList(db.Batches, "BatchID", "BatchName");
            ViewBag.BatchProgramID = new SelectList(db.GetBatchProgramNameConcat("", 0), "ID", "Name");
            return View();
        }


        [HttpPost]
        public ActionResult OrientationEmailSent(int BatchID, int BatchProgramID)
        {
            if (BatchProgramID == 0)
            {
                ViewBag.MessageType = "error";
                ViewBag.Message = "Please select program.";
            }
            ViewBag.hdnBatchProgramID = BatchProgramID;
            ViewBag.hdnBatchID = BatchID;
            ViewBag.BatchID = new SelectList(db.Batches, "BatchID", "BatchName");
            ViewBag.BatchProgramID = new SelectList(db.GetBatchProgramNameConcat("", 0), "ID", "Name");

            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SendOrientationEmail(List<HttpPostedFileBase> fileUploader, int hdnBatchProgramID, string EmailBody, string txtEntryTestDate, string txtEntryTestTime, FormCollection fc, int hdnBatchID)
        {
            try
            {
                int count = Convert.ToInt32(fc["hdnRowCount"]);

                string FormNo = "";
                string ApplicantName = "";
                string FatherName = "";
                string CellNo = "";
                string Email = "";
                string checkedbox = "";

                if (count == 0)
                {
                    ViewBag.MessageType = "error";
                    ViewBag.Message = "No record found to send email.";
                }
                else
                {
                    Program Prog = null;
                    Batch batch = null;
                    BatchProgram BatchProg = db.BatchPrograms.FirstOrDefault(x => x.BatchProgramID == hdnBatchProgramID);
                    if (BatchProg != null)
                    {
                        Prog = db.Programs.FirstOrDefault(x => x.ProgramID == BatchProg.ProgramID);
                        batch = db.Batches.FirstOrDefault(x => x.BatchID == BatchProg.BatchID);

                        for (int i = 1; i <= count; i++)
                        {
                            FormNo = fc["FormNo_" + i];
                            ApplicantName = fc["ApplicantName_" + i];
                            FatherName = fc["FatherName_" + i];
                            CellNo = fc["CellNo_" + i];
                            Email = fc["Email_" + i];
                            checkedbox = fc["checkedbox_" + i];
                            string strBody = EmailBody.Replace("###CurrentDate", DateTime.Now.ToShortDateString()).Replace("###ApplicantName", ApplicantName).Replace("###FatherName", FatherName).Replace("###FormNo", FormNo).Replace("###ProgramName", Prog.ProgramName).Replace("###BatchSemester ", batch.BatchName).Replace("###DepositDate", Convert.ToDateTime(txtEntryTestDate).ToShortDateString()).Replace("###EntryTestTime", txtEntryTestTime);
                            if (checkedbox == "Yes")
                            {
                                SendEmail(fileUploader, "Orientation Email", strBody, Email, FormNo, "OrientationEmailSent", checkedbox);
                            }

                        }
                        ViewBag.MessageType = "success";
                        ViewBag.Message = "Email sent successfully.";
                    }
                }

                ViewBag.hdnBatchProgramID = hdnBatchProgramID;
                ViewBag.hdnBatchID = hdnBatchID;
                ViewBag.BatchID = new SelectList(db.Batches, "BatchID", "BatchName");
                ViewBag.BatchProgramID = new SelectList(db.GetBatchProgramNameConcat("", 0), "ID", "Name");
            }
            catch (Exception ex)
            {
                ViewBag.MessageType = "error";
                ViewBag.Message = "Problem while sending email, Please check details, Exception: " + ex.Message;
            }
            return RedirectToAction("OrientationEmail");
        }
        // GEt programs by Faculty and Batch.
        public JsonResult GetPrograms_by_FacultyLevelBatch(string FacultyID, string BatchID)
        {
            List<GetPrograms_by_FacultyLevelBatch_Result> lstPrograms = new List<GetPrograms_by_FacultyLevelBatch_Result>();

            lstPrograms = db.GetPrograms_by_FacultyLevelBatch(Convert.ToInt32(FacultyID), 0, Convert.ToInt32(BatchID), 0).ToList();
            var programs = lstPrograms.Select(p => new
            {
                BatchProgramID = p.BatchProgramID,
                ProgramName = p.ProgramName
            });
            string result = JsonConvert.SerializeObject(programs, Formatting.Indented);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        public string SendEmail(List<HttpPostedFileBase> fileUploader, string EmailSubject, string EmailBody, string ToEmail, string FormNo, string EmailType, string checkedbox)
        {
            try
            {
                string FromEmail = (ConfigurationManager.AppSettings["FromEmail"] == null || ConfigurationManager.AppSettings["FromEmail"] == "") ? "" : Convert.ToString(ConfigurationManager.AppSettings["FromEmail"]);
                string FromEmailPassword = (ConfigurationManager.AppSettings["FromEmailPassword"] == null || ConfigurationManager.AppSettings["FromEmailPassword"] == "") ? "" : Convert.ToString(ConfigurationManager.AppSettings["FromEmailPassword"]);
                string CCEmail = (ConfigurationManager.AppSettings["CCEmail"] == null || ConfigurationManager.AppSettings["CCEmail"] == "") ? "" : Convert.ToString(ConfigurationManager.AppSettings["CCEmail"]);
                string BCCEmail = (ConfigurationManager.AppSettings["BCCEmail"] == null || ConfigurationManager.AppSettings["BCCEmail"] == "") ? "" : Convert.ToString(ConfigurationManager.AppSettings["BCCEmail"]);
                string SendEmailFlag = (ConfigurationManager.AppSettings["SendEmailFlag"] == null || ConfigurationManager.AppSettings["SendEmailFlag"] == "") ? "" : Convert.ToString(ConfigurationManager.AppSettings["SendEmailFlag"]);

                using (MailMessage mail = new MailMessage(FromEmail, ToEmail, CCEmail, BCCEmail))
                {
                    mail.Subject = EmailSubject;
                    mail.Body = EmailBody;

                    MailAddress copy = null;
                    if (!string.IsNullOrEmpty(CCEmail))
                    {
                        copy = new MailAddress(CCEmail);
                        mail.CC.Add(copy);
                    }

                    MailAddress copy1 = null;
                    if (!string.IsNullOrEmpty(BCCEmail))
                    {
                        copy1 = new MailAddress(BCCEmail);
                        mail.Bcc.Add(copy1);
                    }

                    foreach (HttpPostedFileBase attachment in fileUploader)
                    {
                        if (fileUploader[0] != null)
                        {
                            string fileName = Path.GetFileName(attachment.FileName);
                            mail.Attachments.Add(new Attachment(attachment.InputStream, fileName));
                        }
                    }

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
                        if (SendEmailFlag == "1")
                        {
                            smtp.Send(mail);
                        }
                    }
                    catch (Exception)
                    {

                    }

                    Applicant ap = db.Applicants.FirstOrDefault(a => a.FormNo == FormNo);
                    db.Entry(ap).State = EntityState.Modified;
                    ap.ModifiedBy = Convert.ToInt32(Session["emp_id"]);
                    ap.ModifiedOn = DateTime.Now;

                    if (EmailType == "EntryTestEmailSent")
                    {
                        ap.StatusID = 4014; // StatusName = EntryTestEmailSent
                    }
                    else if (EmailType == "OfferLetterEmailSent")
                    {
                        ap.StatusID = 4015; // StatusName = OfferLetterEmailSent
                    }
                    else if (EmailType == "OrientationEmailSent")
                    {
                        ap.StatusID = 5014; // StatusName = OfferLetterEmailSent
                    }

                    try
                    {
                        db.SaveChanges();
                        ViewBag.MessageType = "success";
                        ViewBag.Message = "Data has been saved successfully.";
                    }
                    catch (DbUpdateException ex)
                    {
                        ViewBag.MessageType = "error";
                        ViewBag.Message = ex.Message;
                        ModelState.AddModelError(string.Empty, ex.Message);
                    }

                    return "Email sent successfully";
                }
            }
            catch (Exception ex)
            {
                return "Problem sending email, Exception: " + ex.Message;
            }
        }
    }
}