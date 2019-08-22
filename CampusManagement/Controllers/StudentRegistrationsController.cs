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
using CampusManagement.App_Code;

namespace CampusManagement.Controllers
{
    [Authorize]
    public class StudentRegistrationsController : Controller
    {
        private ModelCMSNewContainer db = new ModelCMSNewContainer();

        [HttpGet]
        public ActionResult Index()
        {
            ViewBag.MessageType = "";
            ViewBag.Message = "";
            List<GetStudentsToMoveToNextSemester_Result> lstStudents = db.GetStudentsToMoveToNextSemester(0, 0, 0).ToList();
            return View(lstStudents);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(int? BatchID, int? BatchProgramID, int? YearSemesterNo, int? StatusID)
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

            if (StatusID == null)
            {
                StatusID = 0;
            }

            ViewBag.hdnBatchID = BatchID;
            ViewBag.hdnBatchProgramID = BatchProgramID;
            ViewBag.hdnYearSemesterNo = YearSemesterNo;
            ViewBag.hdnStatusID = StatusID;

            List<GetStudentsToMoveToNextSemester_Result> lstStudents = db.GetStudentsToMoveToNextSemester(BatchProgramID, YearSemesterNo, StatusID).ToList();
            return View(lstStudents);
        }

        public ActionResult PromoteStudent(int? BatchProgramID, int? YearSemesterNo, int? StudentID)
        {
            int EmpID = Convert.ToInt32(Session["emp_id"]);
            string error = "";
            if (EmpID == 0)
            {
                return RedirectToAction("Login2", "Home");
            }
            else
            {
                if (BatchProgramID == null)
                {
                    BatchProgramID = 0;
                    error += "BatchProgramID is empty.";
                }

                if (YearSemesterNo == null)
                {
                    YearSemesterNo = 0;
                    error += "YearSemesterNo is empty.";
                }

                if (StudentID == null)
                {
                    StudentID = 0;
                    error += "StudentID is empty.";
                }

                if(string.IsNullOrEmpty(error))
                {
                    db.MoveStudentsToNextSemester(BatchProgramID, YearSemesterNo, StudentID, EmpID);
                    ViewBag.MessageType = "success";
                    ViewBag.Message = "Student has been promoted to next semester successfully.";
                }
            }
            return RedirectToAction("Index");
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

        // GEt Status
        public JsonResult GetApplicantStudentStatus(string StatusType, int QueryID)
        {
            List<GetApplicantStudentStatus_Result> lstStatus = CommonFunctions.GetApplicantStudentStatus("student", QueryID);
            var status = lstStatus.Select(s => new
            {
                StatusID = s.StatusID,
                StatusName = s.StatusName,
                StatusType = s.StatusType
            });
            string result = JsonConvert.SerializeObject(status, Formatting.Indented);
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