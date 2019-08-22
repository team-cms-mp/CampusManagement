using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CampusManagement.Models;
using Newtonsoft.Json;
using PagedList;
using System.Globalization;

using CampusManagement.App_Code;
using System.Configuration;
using System.IO;
using System.Net.Mail;


namespace CampusManagement.Controllers
{
    public class ApprovedAsStudentController : Controller
    {
        private ModelCMSNewContainer db = new ModelCMSNewContainer();
        ApproveStudentViewModel model = new ApproveStudentViewModel();

        [HttpGet]
        public ActionResult Index(string Search, string BatchID, string BatchProgramID, string YearSemesterNo, string message)
        {
            if (Search == null)
            {
                Search = "";
            }
            if (BatchID == null)
            {
                BatchID = "0";
            }
            if (BatchProgramID == null)
            {
                BatchProgramID = "0";
            }
            if (YearSemesterNo == null)
            {
                YearSemesterNo = "0";
            }
            List<Batch> BatchList = new List<Batch>();
            Batch BatchObj = new Batch();
            BatchObj.BatchID = 0;
            BatchObj.BatchName = "--ALL--";
            BatchList = db.Batches.ToList();
            BatchList.Insert(0, BatchObj);
            ViewBag.BatchID = new SelectList(BatchList, "BatchID", "BatchName", BatchID);
            ViewBag.BatchProgramID = new SelectList(db.GetBatchProgramNameConcat("", 0).ToList(), "ID", "Name", BatchProgramID);
            ViewBag.YearSemesterNo = new SelectList(db.Semesters, "YearSemesterNo", "YearSemesterNo", YearSemesterNo);
            ViewBag.hdnBatchID = BatchID;
            ViewBag.hdnBatchProgramID = BatchProgramID;
            ViewBag.hdnYearSemesterNo = YearSemesterNo;
            model.StudentList = db.GetStudentApprovalList("", Convert.ToInt32(BatchID), Convert.ToInt32(BatchProgramID), Convert.ToInt32(YearSemesterNo), 5015, null, null).ToList();

            if (message == "1")
            {
                ViewBag.MessageType = "success";
                ViewBag.Message = "Data has been saved successfully.";
            }
            else if (message == "0")
            {
                ViewBag.MessageType = "error";
                ViewBag.Message = "Error durring process! retry again for remaining";

            }


            return View(model);
        }



        [HttpPost]
        public ActionResult Index(string Search, string BatchID, string BatchProgramID, string YearSemesterNo)
        {

            List<Batch> BatchList = new List<Batch>();
            Batch BatchObj = new Batch();
            BatchObj.BatchID = 0;
            BatchObj.BatchName = "--ALL--";
            BatchList = db.Batches.ToList();
            BatchList.Insert(0, BatchObj);
            ViewBag.hdnBatchID = BatchID;
            ViewBag.hdnBatchProgramID = BatchProgramID;
            ViewBag.hdnYearSemesterNo = YearSemesterNo;
            ViewBag.BatchID = new SelectList(BatchList, "BatchID", "BatchName", BatchID);
            ViewBag.BatchProgramID = new SelectList(db.GetBatchProgramNameConcat("", 0).ToList(), "ID", "Name", BatchProgramID);
            ViewBag.YearSemesterNo = new SelectList(db.Semesters, "YearSemesterNo", "YearSemesterNo", YearSemesterNo);
            model.StudentList = db.GetStudentApprovalList("", Convert.ToInt32(BatchID), Convert.ToInt32(BatchProgramID), Convert.ToInt32(YearSemesterNo), 5015, null, null).ToList();
            return View(model);

        }

        [HttpPost]
        public ActionResult SaveApproveStudents(string BatchID, string BatchProgramID, string YearSemesterNo, List<GetStudentApprovalList_Result> StudentList)
        {
            int? page = 10;
            string Search = "";
            string message = "1";

            try
            {
                for (int i = 0; i < StudentList.Count; i++)
                {
                    if (StudentList[i].IsApproved == true)
                    {
                        db.UpdateStudentApprovalStatus(StudentList[i].StudentID, 6015, StudentList[i].FormNo);

                        string strBody = "<html><head></head><body> Dear Mr." + StudentList[i].studentname + ", <br/>Good Day! <br/><br/>This is an acknowledgement that your admission has been approved.<br/>Your can join the " + MvcApplication.UniversityName + " Web portal https://vip194.ddns.net:8088/iqcms with following credentials.<br/> Username  :" + StudentList[i].UserName + " <br/> Password: " + StudentList[i].Password + "<br/> <br/><strong> Note:</strong> Please feel free to contact Admission Office for any query on <strong> " + MvcApplication.PhoneNumber1 + " ext " + MvcApplication.Extension1 + ", " + MvcApplication.MobileNumber1 + " from 9:00 am to 5:00 pm <br/>Regards <br/>Iqra Unversity Adminitration Team </body> </html>";


                        SendEmail("(" + MvcApplication.UniversityName + ") - Your Student Credentials", strBody, StudentList[i].Email);
                        //SendEmail("(" + MvcApplication.UniversityName + ") - Your Student Credentials", strBody, "khizer@megaplus.com.pk");

                    }
                    if (message == "0")
                    {
                        break;
                    }
                }

            }
            catch (Exception ex)
            {
                message = "0";
            }

            return RedirectToAction("Index", new { Search = Search, BatchID = BatchID, BatchProgramID = BatchProgramID, YearSemesterNo = YearSemesterNo, message = message });

        }


        public JsonResult GetPrograms_by_FacultyLevelBatch(string FacultyID, string BatchID)
        {
            List<GetPrograms_by_FacultyLevelBatch_Result> lstPrograms = new List<GetPrograms_by_FacultyLevelBatch_Result>();
            GetPrograms_by_FacultyLevelBatch_Result GetPrograms_by_FacultyLevelBatch_ResultObj = new GetPrograms_by_FacultyLevelBatch_Result();
            GetPrograms_by_FacultyLevelBatch_ResultObj.BatchProgramID = 0;
            GetPrograms_by_FacultyLevelBatch_ResultObj.ProgramName = "--ALL--";
            lstPrograms = db.GetPrograms_by_FacultyLevelBatch(Convert.ToInt32(FacultyID), 0, Convert.ToInt32(BatchID), 0).ToList();
            lstPrograms.Insert(0, GetPrograms_by_FacultyLevelBatch_ResultObj);
            var programs = lstPrograms.Select(p => new
            {
                BatchProgramID = p.BatchProgramID,
                ProgramName = p.ProgramName
            });
            string result = JsonConvert.SerializeObject(programs, Formatting.Indented);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetBatchProgramSemesterList(string BatchProgramID)
        {
            List<BatchProgramSemester> lstSemester = new List<BatchProgramSemester>();
            int bpId = Convert.ToInt32(BatchProgramID);

            lstSemester = db.BatchProgramSemesters.Where(s => s.BatchProgramID == bpId).ToList();
            var semesters = lstSemester.Select(S => new
            {
                YearSemesterNo = S.YearSemesterNo
            });
            string result = JsonConvert.SerializeObject(semesters, Formatting.Indented);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetBatchProgramCourseList(string BatchID, string BatchProgramID, string YearSemesterNo)
        {
            List<GetProgramCourse_Result> lstProgramCourse = new List<GetProgramCourse_Result>();
            int bpId = Convert.ToInt32(BatchProgramID);
            GetProgramCourse_Result Obj = new GetProgramCourse_Result();
            Obj.ProgramCourseID = 0;
            Obj.CourseCode = "--ALL--";
            Obj.CourseName = "";
            lstProgramCourse = db.GetProgramCourse(Convert.ToInt32(BatchID), Convert.ToInt32(BatchProgramID), Convert.ToInt32(YearSemesterNo)).ToList();
            lstProgramCourse.Insert(0, Obj);
            var ProgramCourse = lstProgramCourse.Select(S => new
            {
                ProgramCourseID = S.ProgramCourseID,
                CourseName = S.CourseCode + " - " + S.CourseName
            });
            string result = JsonConvert.SerializeObject(ProgramCourse, Formatting.Indented);
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        public string SendEmail(string EmailSubject, string EmailBody, string ToEmail)
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

                    return "Email sent successfully";
                }
            }
            catch (Exception ex)
            {
                return "Problem sending email, Exception: " + ex.Message;
            }
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}