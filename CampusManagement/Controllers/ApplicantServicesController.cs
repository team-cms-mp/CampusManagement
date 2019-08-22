using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CampusManagement.Models;
using Newtonsoft.Json;
using System.Data.SqlClient;

namespace CampusManagement.Controllers
{
    [Authorize]
    public class ApplicantServicesController : Controller
    {
        private ModelCMSNewContainer db = new ModelCMSNewContainer();
        GetStudentServicesAccumulate_ResultsViewModel model = new GetStudentServicesAccumulate_ResultsViewModel();
        public ActionResult StudentServicesApproval()
        {
            int EmpID = Convert.ToInt32(Session["emp_id"]);
            if (EmpID == 0)
            {
                return RedirectToAction("Login2", "Home");
            }
            else
            {
                List<SF_GetStudentServiceDetail_Result> lstServices = db.SF_GetStudentServiceDetail(0, 0, 1007, 0, 1).ToList();
                return View(lstServices);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult StudentServicesApproval(int? BatchID, int? BatchProgramID, int? YearSemesterNo)
        {
            int EmpID = Convert.ToInt32(Session["emp_id"]);
            if (EmpID == 0)
            {
                return RedirectToAction("Login2", "Home");
            }
            else
            {
                if (BatchID == null)
                {
                    BatchID = 0;
                }

                if (BatchProgramID == null)
                {
                    BatchProgramID = 0;
                }

                if (YearSemesterNo == null)
                {
                    YearSemesterNo = 0;
                }

                ViewBag.hdnBatchID = BatchID;
                ViewBag.hdnBatchProgramID = BatchProgramID;
                ViewBag.hdnYearSemesterNo = YearSemesterNo;

                List<SF_GetStudentServiceDetail_Result> lstServices = db.SF_GetStudentServiceDetail(BatchProgramID, YearSemesterNo, 1007, 0, 1).ToList();
                return View(lstServices);
            }
        }

        public ActionResult ApproveStudentServicesApproval(int? hdnBatchProgramID, int? hdnYearSemesterNo)
        {
            int EmpID = Convert.ToInt32(Session["emp_id"]);
            string error = "";
            if (EmpID == 0)
            {
                return RedirectToAction("Login2", "Home");
            }
            else
            {
                if (hdnBatchProgramID == null)
                {
                    hdnBatchProgramID = 0;
                    error += "BatchProgramID is empty.";
                }

                if (hdnYearSemesterNo == null)
                {
                    hdnYearSemesterNo = 0;
                    error += "hdnYearSemesterNo is empty.";
                }

                db.SF_UpdateStudentServiceStatus(hdnBatchProgramID, hdnYearSemesterNo, 1007, 0, EmpID, 0, 1, 3009);
            }
            return RedirectToAction("StudentServicesApproval");
        }

        public ActionResult StudentServicesApproved()
        {
            int EmpID = Convert.ToInt32(Session["emp_id"]);
            if (EmpID == 0)
            {
                return RedirectToAction("Login2", "Home");
            }
            else
            {
                List<SF_GetStudentServiceDetail_Result> lstServices = db.SF_GetStudentServiceDetail(0, 0, 3009, 0, 1).ToList();
                return View(lstServices);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult StudentServicesApproved(int? BatchID, int? BatchProgramID, int? YearSemesterNo)
        {
            int EmpID = Convert.ToInt32(Session["emp_id"]);
            if (EmpID == 0)
            {
                return RedirectToAction("Login2", "Home");
            }
            else
            {
                if (BatchID == null)
                {
                    BatchID = 0;
                }

                if (BatchProgramID == null)
                {
                    BatchProgramID = 0;
                }

                if (YearSemesterNo == null)
                {
                    YearSemesterNo = 0;
                }

                ViewBag.hdnBatchID = BatchID;
                ViewBag.hdnBatchProgramID = BatchProgramID;
                ViewBag.hdnYearSemesterNo = YearSemesterNo;

                List<SF_GetStudentServiceDetail_Result> lstServices = db.SF_GetStudentServiceDetail(BatchProgramID, YearSemesterNo, 3009, 0, 1).ToList();
                return View(lstServices);
            }
        }

        public ActionResult SingleStudentServicesApproval(int? StudentID)
        {
            int EmpID = Convert.ToInt32(Session["emp_id"]);
            ViewBag.StudentID = StudentID;
            if (EmpID == 0)
            {
                return RedirectToAction("Login2", "Home");
            }
            else
            {
                List<SF_GetStudentServiceDetail_Result> lstServices = db.SF_GetStudentServiceDetail(0, 0, 0, StudentID, 1).ToList();
                return View(lstServices);
            }
        }
        public ActionResult UnApproveSingleStudentServicesApproval(int? StudentServiceID, int? StudentID, string Status)
        {
            int EmpID = Convert.ToInt32(Session["emp_id"]);
            ViewBag.StudentID = StudentID;
            if (EmpID == 0)
            {
                return RedirectToAction("Login2", "Home");
            }
            else
            {
                if (Status == "Approved")
                {
                    db.SF_UpdateStudentServiceStatus(0, 0, 3009, 0, EmpID, StudentServiceID, 2, 1007);
                }
                else if (Status == "Unapproved")
                {
                    db.SF_UpdateStudentServiceStatus(0, 0, 1007, 0, EmpID, StudentServiceID, 2, 3009);
                }

                List<SF_GetStudentServiceDetail_Result> lstServices = db.SF_GetStudentServiceDetail(0, 0, 0, StudentID, 1).ToList();
                return RedirectToAction("SingleStudentServicesApproval", new { StudentID = StudentID });
            }
        }
        public ActionResult ApproveSingleStudentServicesApproval(int? StudentServiceID)
        {
            int EmpID = Convert.ToInt32(Session["emp_id"]);
            string error = "";
            if (EmpID == 0)
            {
                return RedirectToAction("Login2", "Home");
            }
            else
            {
                if (StudentServiceID == null)
                {
                    StudentServiceID = 0;
                    error += "StudentServiceID is empty.";
                }

                if (string.IsNullOrEmpty(error))
                {
                    db.SF_UpdateStudentServiceStatus(0, 0, 1007, 0, EmpID, StudentServiceID, 2, 3009);
                }
            }
            return RedirectToAction("SingleStudentServicesApproval");
        }

        public ActionResult ApproveStudentServicesApproval_ByStudentServiceID(int? StudentServiceID)
        {
            int EmpID = Convert.ToInt32(Session["emp_id"]);
            string error = "";
            if (EmpID == 0)
            {
                return RedirectToAction("Login2", "Home");
            }
            else
            {
                if (StudentServiceID == null)
                {
                    StudentServiceID = 0;
                    error += "StudentServiceID is empty.";
                }

                if (string.IsNullOrEmpty(error))
                {
                    db.SF_UpdateStudentServiceStatus(0, 0, 1007, 0, EmpID, StudentServiceID, 2, 3009);
                }
            }
            return RedirectToAction("StudentServicesApproval");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GenerateChallans(string IssueDate, string LastDate, int? AccountID
            , int? hdnBatchID, int? hdnBatchProgramID, int? hdnYearSemesterNo)
        {
            int EmpID = Convert.ToInt32(Session["emp_id"]);
            string error = "";
            int count = 0;
            if (EmpID == 0)
            {
                return RedirectToAction("Login2", "Home");
            }
            else
            {
                if (hdnBatchID == null || hdnBatchID == 0)
                {
                    hdnBatchID = 0;
                }

                if (hdnBatchProgramID == null || hdnBatchProgramID == 0)
                {
                    hdnBatchProgramID = 0;
                }

                if (hdnYearSemesterNo == null || hdnYearSemesterNo == 0)
                {
                    hdnYearSemesterNo = 0;
                }

                if (AccountID == null || AccountID == 0)
                {
                    AccountID = 0;
                    count++;
                    error += string.Concat(count.ToString(), "-Please select Account.</ br>");
                }

                if (string.IsNullOrEmpty(error))
                {
                    db.SF_GenerateStudentServiceChallans(hdnBatchProgramID, hdnYearSemesterNo, Convert.ToDateTime(IssueDate)
                        , Convert.ToDateTime(LastDate), AccountID, EmpID, 1, 0);

                    TempData["messageType"] = "success";
                    TempData["message"] = "Challans have been generated successfully.";
                }
                else
                {
                    TempData["messageType"] = "error";
                    TempData["message"] = error;
                }

                ViewBag.hdnBatchID = hdnBatchID;
                ViewBag.hdnBatchProgramID = hdnBatchProgramID;
                ViewBag.hdnYearSemesterNo = hdnYearSemesterNo;

                return RedirectToAction("StudentServicesApproved");
            }
        }

        [HttpGet]
        public ActionResult Search()
        {
            model.GetStudentServicesAccumulate_Results = db.GetStudentServicesAccumulate(0, "", 1, "Applicant", 0).ToList();
            model.SelectedGetStudentServicesAccumulate_Result = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.BatchID = new SelectList(db.Batches, "BatchID", "BatchName");
            ViewBag.BatchProgramID = new SelectList(db.GetBatchProgramNameConcat("", 0), "ID", "Name");
            ViewBag.YearSemesterNo = new SelectList(db.Semesters, "YearSemesterNo", "YearSemesterNo");

            ViewBag.MessageType = "";
            ViewBag.Message = "";
            return View("StudentServicesAccumulate", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Search(Challan challan)
        {
            if (string.IsNullOrEmpty(challan.FormNo))
            {
                challan.FormNo = "";
            }

            model.GetStudentServicesAccumulate_Results = db.GetStudentServicesAccumulate(challan.BatchProgramID, "", 1, "Applicant", 0).ToList();
            model.SelectedGetStudentServicesAccumulate_Result = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.BatchID = new SelectList(db.Batches, "BatchID", "BatchName", challan.BatchID);
            ViewBag.BatchProgramID = new SelectList(db.GetBatchProgramNameConcat("", 0), "ID", "Name", challan.BatchProgramID);
            ViewBag.YearSemesterNo = new SelectList(db.Semesters, "YearSemesterNo", "YearSemesterNo", challan.YearSemesterNo);
            if (model.GetStudentServicesAccumulate_Results.Count == 0)
            {
                ViewBag.MessageType = "error";
                ViewBag.Message = "No record found.";
            }
            ViewBag.hdnBatchID = challan.BatchID;
            ViewBag.hdnBatchProgramID = challan.BatchProgramID;
            ViewBag.hdnYearSemesterNo = challan.YearSemesterNo;
            return View("StudentServicesAccumulate", model);
        }


        [HttpGet]
        public ActionResult StudentServicesAccumulate()
        {
            model.GetStudentServicesAccumulate_Results = db.GetStudentServicesAccumulate(0, "", 1, "Applicant", 0).ToList();
            model.SelectedGetStudentServicesAccumulate_Result = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.BatchID = new SelectList(db.Batches, "BatchID", "BatchName");
            ViewBag.BatchProgramID = new SelectList(db.GetBatchProgramNameConcat("", 0), "ID", "Name");
            ViewBag.YearSemesterNo = new SelectList(db.Semesters, "YearSemesterNo", "YearSemesterNo");

            ViewBag.MessageType = "";
            ViewBag.Message = "";
            return View(model);
        }


        //  GenerateChallansAccumulate
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GenerateChallansAccumulate(string IssueDate, string LastDate, int? AccountID
     , int? hdnBatchID, int? hdnBatchProgramID, int? hdnYearSemesterNo, FormCollection fc)
        {
            int hdnCount = Convert.ToInt32(fc["hdnCount"]);
            int EmpID = Convert.ToInt32(Session["emp_id"]);
            string error = "";
            int count = 0;
            if (EmpID == 0)
            {
                return RedirectToAction("Login2", "Home");
            }
            else
            {

                if (hdnBatchID == null || hdnBatchID == 0)
                {
                    hdnBatchID = 0;
                }

                if (hdnBatchProgramID == null || hdnBatchProgramID == 0)
                {
                    hdnBatchProgramID = 0;
                }

                if (hdnYearSemesterNo == null || hdnYearSemesterNo == 0)
                {
                    hdnYearSemesterNo = 0;
                }

                if (AccountID == null || AccountID == 0)
                {
                    AccountID = 0;
                    count++;
                    error += string.Concat(count.ToString(), "-Please select Account.</ br>");
                }
                if (string.IsNullOrEmpty(error))
                {
                    for (int i = 1; i <= hdnCount; i++)
                    {

                        string chkGenerateChallan = Convert.ToString(fc["chkGenerateChallan_" + i]);
                        if (chkGenerateChallan == "Yes")
                        {
                            int StudentID = Convert.ToInt32(fc["StudentID_" + i]);
                            db.SF_GenerateStudentServiceChallans(hdnBatchProgramID, hdnYearSemesterNo, Convert.ToDateTime(IssueDate)
                                , Convert.ToDateTime(LastDate), AccountID, EmpID, 1, StudentID);

                            TempData["messageType"] = "success";
                            TempData["message"] = "Challans have been generated successfully.";
                        }

                    }

                }
                else
                {
                    TempData["messageType"] = "error";
                    TempData["message"] = error;
                }

                ViewBag.hdnBatchID = hdnBatchID;
                ViewBag.hdnBatchProgramID = hdnBatchProgramID;
                ViewBag.hdnYearSemesterNo = hdnYearSemesterNo;

                return RedirectToAction("StudentServicesAccumulate");
            }
        }

        [HttpGet]
        public ActionResult StudentServicesAccumulateGenerated()
        {
            model.GetStudentServicesAccumulate_Results = db.GetStudentServicesAccumulate(0, "", 1, "ApplicantStatus", 4013).ToList();
            model.SelectedGetStudentServicesAccumulate_Result = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.BatchID = new SelectList(db.Batches, "BatchID", "BatchName");
            ViewBag.BatchProgramID = new SelectList(db.GetBatchProgramNameConcat("", 0), "ID", "Name");
            ViewBag.YearSemesterNo = new SelectList(db.Semesters, "YearSemesterNo", "YearSemesterNo");

            ViewBag.MessageType = "";
            ViewBag.Message = "";
            return View(model);
        }
        [HttpGet]
        public ActionResult StudentServicesAccumulateGeneratedSearch()
        {
            model.GetStudentServicesAccumulate_Results = db.GetStudentServicesAccumulate(0, "", 1, "ApplicantStatus", 4013).ToList();
            model.SelectedGetStudentServicesAccumulate_Result = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.BatchID = new SelectList(db.Batches, "BatchID", "BatchName");
            ViewBag.BatchProgramID = new SelectList(db.GetBatchProgramNameConcat("", 0), "ID", "Name");
            ViewBag.YearSemesterNo = new SelectList(db.Semesters, "YearSemesterNo", "YearSemesterNo");

            ViewBag.MessageType = "";
            ViewBag.Message = "";
            return View("StudentServicesAccumulateGenerated", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult StudentServicesAccumulateGeneratedSearch(Challan challan)
        {
            if (string.IsNullOrEmpty(challan.FormNo))
            {
                challan.FormNo = "";
            }

            model.GetStudentServicesAccumulate_Results = db.GetStudentServicesAccumulate(challan.BatchProgramID, "", 1, "ApplicantStatus", 4013).ToList();
            model.SelectedGetStudentServicesAccumulate_Result = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.BatchID = new SelectList(db.Batches, "BatchID", "BatchName", challan.BatchID);
            ViewBag.BatchProgramID = new SelectList(db.GetBatchProgramNameConcat("", 0), "ID", "Name", challan.BatchProgramID);
            ViewBag.YearSemesterNo = new SelectList(db.Semesters, "YearSemesterNo", "YearSemesterNo", challan.YearSemesterNo);
            if (model.GetStudentServicesAccumulate_Results.Count == 0)
            {
                ViewBag.MessageType = "error";
                ViewBag.Message = "No record found.";
            }
            ViewBag.hdnBatchID = challan.BatchID;
            ViewBag.hdnBatchProgramID = challan.BatchProgramID;
            ViewBag.hdnYearSemesterNo = challan.YearSemesterNo;
            return View("StudentServicesAccumulateGenerated", model);
        }
        // Get Active Batches
        public JsonResult GetActiveBatches()
        {
            List<Batch> lstBatches = new List<Batch>();

            lstBatches = db.Batches.Where(b => b.IsActive == "Yes").ToList();
            var batches = lstBatches.Select(b => new
            {
                BatchID = b.BatchID,
                BatchName = b.BatchName,
                BatchSession = b.BatchSession,
                BatchCode = b.BatchCode
            });
            string result = JsonConvert.SerializeObject(batches, Formatting.Indented);
            return Json(result, JsonRequestBehavior.AllowGet);
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

        public JsonResult GetStudentsList(string searchValue, string BatchProgramID)
        {
            List<GetBatchProgramNameConcat_Result> lstStudent = db.GetBatchProgramNameConcat(BatchProgramID, 7).Where(s => s.Name.Contains(searchValue)).ToList();

            var Students = lstStudent.Select(s => new
            {
                StudentID = s.ID,
                Name = s.Name
            });

            string result = JsonConvert.SerializeObject(Students, Formatting.Indented);
            return new JsonResult { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult GetBankAccounts()
        {
            List<Finance_Bank_Accounts> lstBA = new List<Finance_Bank_Accounts>();

            lstBA = db.Finance_Bank_Accounts.ToList();
            var banks = lstBA.Select(b => new
            {
                Account_ID = b.Account_ID,
                Account_No = b.Account_No
            });
            string result = JsonConvert.SerializeObject(banks, Formatting.Indented);
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
    }
}


