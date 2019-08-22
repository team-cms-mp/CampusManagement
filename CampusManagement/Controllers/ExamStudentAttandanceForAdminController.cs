using CampusManagement.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;


namespace CampusManagement.Controllers
{
    public class ExamStudentAttandanceForAdminController : Controller
    {
        private ModelCMSNewContainer db = new ModelCMSNewContainer();
        ExamStudentAttandanceForAdminViewModel model = new ExamStudentAttandanceForAdminViewModel();
        // GET: ExamStudentAttandanceForAdmin
        [HttpGet]
        public ActionResult Index(string ExamID, string ExamDateID, string ExamDateTimeSlotID, string BatchID, string BatchProgramID, string YearSemesterNo, string ProgramCourseID, string message )
        {
            if (ExamID == null || ExamID == "") {
                ExamID = "0";
            }
            if (ExamDateID == null || ExamDateID == "")
            {
                ExamDateID = "0";
            }
            if (ExamDateTimeSlotID == null || ExamDateTimeSlotID == "")
            {
                ExamDateTimeSlotID = "0";
            }
            if (BatchID == null || BatchID == "")
            {
                BatchID = "0";
            }
            if (BatchProgramID == null || BatchProgramID == "")
            {
                BatchProgramID = "0";
            }
            if (YearSemesterNo == null || YearSemesterNo == "")
            {
                YearSemesterNo = "0";
            }
            if (ProgramCourseID == null || ProgramCourseID == "")
            {
                ProgramCourseID = "0";
            }



            List<Exam> ExamList = new List<Exam>();
            ExamList = db.Exams.ToList();
            if (ExamList != null)
            {
                if (ExamList.Count > 0)
                {
                    model.ExamID = ExamList[0].ExamID;
                }
            }

            ViewBag.hdnExamID = ExamID;
            ViewBag.hdnExamDateID = ExamDateID;
            ViewBag.hdnExamDateTimeSlotID = ExamDateTimeSlotID;
            ViewBag.hdnBatchID = BatchID;
            ViewBag.hdnBatchProgramID = BatchProgramID;
            ViewBag.hdnYearSemesterNo = YearSemesterNo;
            ViewBag.hdnProgramCourseID = ProgramCourseID;
            
            List<Batch> BatchList = new List<Batch>();
            Batch BatchObj = new Batch();
            BatchObj.BatchID = 0;
            BatchObj.BatchName = "--ALL--";
            BatchList = db.Batches.ToList();
            BatchList.Insert(0, BatchObj);
            ViewBag.ExamID = new SelectList(db.Exams, "ExamID", "ExamTitle");
            ViewBag.ExamDateID = new SelectList(db.GetExamDateByExamID(0), "ExamDateID", "ExamDateTitle");
            ViewBag.ExamDateTimeSlotID = new SelectList(db.GetExamDateTimeSlotByExamID(0), "ExamDateTimeSlotID", "TimeSlot");
            ViewBag.BatchID = new SelectList(BatchList, "BatchID", "BatchName");
            ViewBag.BatchProgramID = new SelectList(db.GetBatchProgramNameConcat("", 0).ToList(), "ID", "Name");
            ViewBag.YearSemesterNo = new SelectList(db.Semesters, "YearSemesterNo", "YearSemesterNo");
            List<GetProgramCourse_Result> lstProgramCourse = new List<GetProgramCourse_Result>();
            lstProgramCourse = db.GetProgramCourse(0, 0, 0).ToList();
            GetProgramCourse_Result Obj = new GetProgramCourse_Result();
            Obj.ProgramCourseID = 0;
            Obj.CourseCode = "--ALL--";
            Obj.CourseName = "";
            lstProgramCourse.Insert(0, Obj);
            var ProgramCourse = lstProgramCourse.Select(S => new
            {
                ProgramCourseID = S.ProgramCourseID,
                CourseName = S.CourseCode + " - " + S.CourseName
            });
            ViewBag.ProgramCourseID = new SelectList(ProgramCourse, "ProgramCourseID", "CourseName");
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
            else
            {
                ViewBag.MessageType = "";
                ViewBag.Message = "";
            }

            model.StudentList = db.GetExamStudentAttandanceForAdmin(Convert.ToInt32(ExamID) , Convert.ToInt32(ExamDateID), Convert.ToInt32(ExamDateTimeSlotID), Convert.ToInt32(BatchID), Convert.ToInt32(BatchProgramID), Convert.ToInt32(YearSemesterNo), Convert.ToInt32(ProgramCourseID)).ToList();
            return View(model);
        }
        [HttpPost]
        public ActionResult Index(int ExamID, int ExamDateID, int ExamDateTimeSlotID, int BatchID, int BatchProgramID, int YearSemesterNo, int ProgramCourseID)
        {

            List<Exam> ExamList = new List<Exam>();
            ExamList = db.Exams.ToList();
            if (ExamList != null)
            {
                if (ExamList.Count > 0)
                {
                    model.ExamID = ExamList[0].ExamID;
                }
            }
            ViewBag.hdnExamID = ExamID;
            ViewBag.hdnExamDateID = ExamDateID;
            ViewBag.hdnExamDateTimeSlotID = ExamDateTimeSlotID;
            ViewBag.hdnBatchID = BatchID;
            ViewBag.hdnBatchProgramID = BatchProgramID;
            ViewBag.hdnYearSemesterNo = YearSemesterNo;
            ViewBag.hdnProgramCourseID = ProgramCourseID;

            List<Batch> BatchList = new List<Batch>();
            Batch BatchObj = new Batch();
            BatchObj.BatchID = 0;
            BatchObj.BatchName = "--ALL--";
            BatchList = db.Batches.ToList();
            BatchList.Insert(0, BatchObj);
            ViewBag.ExamID = new SelectList(db.Exams, "ExamID", "ExamTitle", ExamID);
            ViewBag.ExamDateID = new SelectList(db.GetExamDateByExamID(ExamID), "ExamDateID", "ExamDateTitle", ExamDateID);
            ViewBag.ExamDateTimeSlotID = new SelectList(db.GetExamDateTimeSlotByExamID(ExamID), "ExamDateTimeSlotID", "TimeSlot", ExamDateTimeSlotID);
            ViewBag.BatchID = new SelectList(BatchList, "BatchID", "BatchName", BatchID);
            ViewBag.BatchProgramID = new SelectList(db.GetBatchProgramNameConcat("", 0).ToList(), "ID", "Name", BatchProgramID);
            ViewBag.YearSemesterNo = new SelectList(db.Semesters, "YearSemesterNo", "YearSemesterNo", YearSemesterNo);
            List<GetProgramCourse_Result> lstProgramCourse = new List<GetProgramCourse_Result>();
            lstProgramCourse = db.GetProgramCourse(BatchID, BatchProgramID, YearSemesterNo).ToList();
            GetProgramCourse_Result Obj = new GetProgramCourse_Result();
            Obj.ProgramCourseID = 0;
            Obj.CourseCode = "--ALL--";
            Obj.CourseName = "";
            lstProgramCourse.Insert(0, Obj);
            var ProgramCourse = lstProgramCourse.Select(S => new
            {
                ProgramCourseID = S.ProgramCourseID,
                CourseName = S.CourseCode + " - " + S.CourseName
            });
            ViewBag.ProgramCourseID = new SelectList(ProgramCourse, "ProgramCourseID", "CourseName", ProgramCourseID);
            ViewBag.MessageType = "";
            ViewBag.Message = "";
            model.StudentList = db.GetExamStudentAttandanceForAdmin(ExamID, ExamDateID, ExamDateTimeSlotID, BatchID, BatchProgramID, YearSemesterNo, ProgramCourseID).ToList();

            return View(model);
        }



        [HttpPost]
        public ActionResult SaveStudentsAttendance(string ExamID, string ExamDateID, string ExamDateTimeSlotID, string BatchID, string BatchProgramID, string YearSemesterNo, string ProgramCourseID, List<GetExamStudentAttandanceForAdmin_Result> StudentList)
        {
            string message = "1";
            try
            {
                int CurrentUserID = Convert.ToInt32(Session["CurrentUserID"]);
                for (int i = 0; i < StudentList.Count; i++)
                {
                    db.UpdateExamStudentAttandancebyEEStudentsID(StudentList[i].ExamEligibleStudentsID, StudentList[i].StudentPresentStatusID, StudentList[i].Remarks, CurrentUserID);
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

            return RedirectToAction("Index", new { ExamID = ExamID, ExamDateID = ExamDateID, ExamDateTimeSlotID = ExamDateTimeSlotID, BatchID = BatchID, BatchProgramID = BatchProgramID, YearSemesterNo = YearSemesterNo, ProgramCourseID = ProgramCourseID, message = message });

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
        public JsonResult GetExamDatesByExamID(string ExamID)
        {
            List<GetExamDateByExamID_Result> lstExamDates = new List<GetExamDateByExamID_Result>();

            lstExamDates = db.GetExamDateByExamID(Convert.ToInt32(ExamID)).ToList();
            var Exams = lstExamDates.Select(p => new
            {
                ExamDateID = p.ExamDateID,
                ExamDateTitle = p.ExamDateTitle
            });
            string result = JsonConvert.SerializeObject(Exams, Formatting.Indented);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetExamDateTimeSlotByExamID(string ExamID)
        {
            List<GetExamDateTimeSlotByExamID_Result> lstTimeSlots = new List<GetExamDateTimeSlotByExamID_Result>();
            int ExamIDInt = Convert.ToInt32(ExamID);

            lstTimeSlots = db.GetExamDateTimeSlotByExamID(ExamIDInt).ToList();
            var semesters = lstTimeSlots.Select(S => new
            {
                ExamDateTimeSlotID = S.ExamDateTimeSlotID,
                TimeSlot = S.TimeSlot
            });
            string result = JsonConvert.SerializeObject(semesters, Formatting.Indented);
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