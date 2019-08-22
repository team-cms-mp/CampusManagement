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
    public class ExamEligibleStudentController : Controller
    {
        private ModelCMSNewContainer db = new ModelCMSNewContainer();
        ExamEligibleStudentForExamByCoursetViewModel StudentListModel = new ExamEligibleStudentForExamByCoursetViewModel();
        // GET: ExamEligibleStudent
        public ActionResult ActiveTeacherCourses()
        {
            ViewBag.ExamID = new SelectList(db.Exams, "ExamID", "ExamTitle");
            ViewBag.BatchID = new SelectList(db.Batches, "BatchID", "BatchName");
            ViewBag.BatchProgramID = new SelectList(db.GetBatchProgramNameConcat("", 0).ToList(), "ID", "Name");
            ViewBag.YearSemesterNo = new SelectList(db.Semesters, "YearSemesterNo", "YearSemesterNo");
            ViewBag.MessageType = "";
            ViewBag.Message = "";
            return View();
        }

        [HttpPost]
        public ActionResult ActiveTeacherCourses(ExamSubjectForEligibleStudentViewModel sa)
        {
            string ErrorMessage = "";
            int count = 0;
            ViewBag.hdnExamID = sa.ExamID;
            ViewBag.hdnBatchProgramID = sa.BatchProgramID;
            ViewBag.hdnYearSemesterNo = sa.YearSemesterNo;
            ViewBag.hdnBatchID = sa.BatchID;
            if (sa.BatchProgramID == 0)
            {
                count++;
                ErrorMessage += count + "-Please select session.<br/>";
            }
            if (sa.BatchID == 0)
            {
                count++;
                ErrorMessage += count + "-Please select program.<br/>";
            }
            if (sa.YearSemesterNo == 0)
            {
                count++;
                ErrorMessage += count + "-Please select semester.<br/>";
            }

            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ViewBag.MessageType = "error";
                ViewBag.Message = ErrorMessage;
            }
            else
            {

                ViewBag.MessageType = "";
                ViewBag.Message = "";
            }
            ViewBag.ExamID = new SelectList(db.Exams, "ExamID", "ExamTitle");
            ViewBag.BatchID = new SelectList(db.Batches, "BatchID", "BatchName");
            ViewBag.BatchProgramID = new SelectList(db.GetBatchProgramNameConcat("", 0), "ID", "Name");
            ViewBag.YearSemesterNo = new SelectList(db.Semesters, "YearSemesterNo", "YearSemesterNo");
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc");
            return View();
        }

        public ActionResult StudentAttendanceSummary(int? ProgramCourseID, int? ExamID)
        {
            // To Do 1:::  GetExamEligibleStudentByCourseID(ExamID,ProgramCourseID)   2::   db.ExamsDateSheetDetails.Where(x => x.ProgramCourseID == ProgramCourseID && x.ExamID = ExamId)
            StudentListModel.StudentList = db.GetExamEligibleStudentForExamByCourseID(ExamID,ProgramCourseID).OrderByDescending(x => x.FullName).ToList();
            if (db.ExamsDateSheetDetails.Where(x => x.ExamID == ExamID && x.ProgramCourseID == ProgramCourseID).FirstOrDefault() != null)
            {
                StudentListModel.ExamsDateSheetDetailID = db.ExamsDateSheetDetails.Where(x => x.ExamID == ExamID && x.ProgramCourseID == ProgramCourseID).FirstOrDefault().ExamsDateSheetDetailID;
            }
            else {
                StudentListModel.ExamsDateSheetDetailID = 0;
            }
            
            StudentListModel.ExamID = (int)ExamID;
            StudentListModel.ProgramCourseID = (int) ProgramCourseID;
            
            return View(StudentListModel);
        }


        [HttpPost]
        public ActionResult CreateExamEligibleStudents(ExamEligibleStudentForExamByCoursetViewModel ListObj)
        {
            foreach (GetExamEligibleStudentForExamByCourseID_Result Obj in ListObj.StudentList) {

                if (Obj.IsEligible == true && Obj.ExamEligibleStudentsID == null && Obj.EESStudentBatchProgramCourseID == null) {

                    db.InsertExamEligibleStudent(Obj.StudentBatchProgramCourseID, ListObj.ExamsDateSheetDetailID, DateTime.Now, Convert.ToInt32(Session["emp_id"]));
                }

            }
            
            return RedirectToAction("StudentAttendanceSummary",new  { ProgramCourseID = ListObj.ProgramCourseID , ExamID = ListObj.ExamID });
        }

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