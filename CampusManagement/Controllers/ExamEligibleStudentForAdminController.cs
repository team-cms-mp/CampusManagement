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
    public class ExamEligibleStudentForAdminController : Controller
    {
        private ModelCMSNewContainer db = new ModelCMSNewContainer();
        ExamEligibleStudentsForAdminViewModel model = new ExamEligibleStudentsForAdminViewModel();
        // GET: ExamEligibleStudentForAdmin
        public ActionResult Index()
        {
            if (TempData["EligibleStudents"] == null)
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

                List<Batch> BatchList = new List<Batch>();
                Batch BatchObj = new Batch();
                BatchObj.BatchID = 0;
                BatchObj.BatchName = "--ALL--";
                BatchList = db.Batches.ToList();
                BatchList.Insert(0, BatchObj);
                ViewBag.ExamID = new SelectList(db.Exams, "ExamID", "ExamTitle");
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
                ViewBag.MessageType = "";
                ViewBag.Message = "";
            }
            else
            {
                model = (ExamEligibleStudentsForAdminViewModel)TempData["EligibleStudents"];
                model.StudentDefaulterObj = null;
                model.StudentList = null;
                ViewBag.ExamID = new SelectList(db.Exams, "ExamID", "ExamTitle", model.ExamID);
                List<Batch> BatchList = new List<Batch>();
                Batch BatchObj = new Batch();
                BatchObj.BatchID = 0;
                BatchObj.BatchName = "--ALL--";
                BatchList = db.Batches.ToList();
                BatchList.Insert(0, BatchObj);
                ViewBag.BatchID = new SelectList(BatchList, "BatchID", "BatchName", model.BatchID);
                List<GetBatchProgramNameConcat_Result> BatchProgramNameList = new List<GetBatchProgramNameConcat_Result>();
                GetBatchProgramNameConcat_Result GetBatchProgramNameConcat_ResultObj = new GetBatchProgramNameConcat_Result();
                GetBatchProgramNameConcat_ResultObj.ID = 0;
                GetBatchProgramNameConcat_ResultObj.Name = "--ALL--";
                BatchProgramNameList = db.GetBatchProgramNameConcat("", 0).ToList();
                BatchProgramNameList.Insert(0, GetBatchProgramNameConcat_ResultObj);
                ViewBag.BatchProgramID = new SelectList(BatchProgramNameList, "ID", "Name", model.BatchProgramID);
                ViewBag.YearSemesterNo = new SelectList(db.Semesters, "YearSemesterNo", "YearSemesterNo", model.YearSemesterNo);
                ViewBag.hdnBatchProgramID = model.BatchProgramID;
                ViewBag.hdnYearSemesterNo = model.YearSemesterNo;
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
                ViewBag.ProgramCourseID = new SelectList(ProgramCourse, "ProgramCourseID", "CourseName", model.ProgramCourseID);
                //if (IsShowMessage == "show")
                //{
                //    ViewBag.MessageType = "success";
                //    ViewBag.Message = "Data has been saved successfully.";
                //}
                //else {
                ViewBag.MessageType = "";
                ViewBag.Message = "";
                // }
            }

            return View(model);
        }

        [HttpPost]
        public ActionResult Index(ExamEligibleStudentsForAdminViewModel Eesfa)
        {

            ViewBag.ExamID = new SelectList(db.Exams, "ExamID", "ExamTitle", Eesfa.ExamID);
            List<Batch> BatchList = new List<Batch>();
            Batch BatchObj = new Batch();
            BatchObj.BatchID = 0;
            BatchObj.BatchName = "--ALL--";
            BatchList = db.Batches.ToList();
            BatchList.Insert(0, BatchObj);
            ViewBag.BatchID = new SelectList(BatchList, "BatchID", "BatchName", Eesfa.BatchID);
            List<GetBatchProgramNameConcat_Result> BatchProgramNameList = new List<GetBatchProgramNameConcat_Result>();
            GetBatchProgramNameConcat_Result GetBatchProgramNameConcat_ResultObj = new GetBatchProgramNameConcat_Result();
            GetBatchProgramNameConcat_ResultObj.ID = 0;
            GetBatchProgramNameConcat_ResultObj.Name = "--ALL--";
            BatchProgramNameList = db.GetBatchProgramNameConcat("", 0).ToList();
            BatchProgramNameList.Insert(0, GetBatchProgramNameConcat_ResultObj);
            ViewBag.BatchProgramID = new SelectList(BatchProgramNameList, "ID", "Name", Eesfa.BatchProgramID);
            ViewBag.YearSemesterNo = new SelectList(db.Semesters, "YearSemesterNo", "YearSemesterNo", Eesfa.YearSemesterNo);
            ViewBag.hdnBatchProgramID = Eesfa.BatchProgramID;
            ViewBag.hdnYearSemesterNo = Eesfa.YearSemesterNo;
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
            ViewBag.ProgramCourseID = new SelectList(ProgramCourse, "ProgramCourseID", "CourseName", Eesfa.ProgramCourseID);
            //if (IsShowMessage == "show")
            //{
            //    ViewBag.MessageType = "success";
            //    ViewBag.Message = "Data has been saved successfully.";
            //}
            //else {
            ViewBag.MessageType = "";
            ViewBag.Message = "";
            // }


            return View(Eesfa);

        }

        [HttpPost]
        public ActionResult CreateExamEligibleStudents(ExamEligibleStudentsForAdminViewModel Eesfa)
        {
            if (Eesfa != null)
            {
                if (Eesfa.StudentList != null)
                {
                    if (Eesfa.StudentList.Count > 0)
                    {
                        foreach (GetExamEligibleStudentsForAdmin_Result Obj in Eesfa.StudentList)
                        {
                            // Write a store procedure for Insert or update 
                            db.InsertExamEligibleStudent(Obj.StudentBatchProgramCourseID, Obj.ExamsDateSheetDetailID, DateTime.Now, Convert.ToInt32(Session["emp_id"]));
                        }

                    }

                }

            }

            TempData["EligibleStudents"] = Eesfa;
            return RedirectToAction("Index");
            //  TempData["EligibleStudents"] = Eesfa;
            // return View("Index", Eesfa);
            // return Index(Eesfa);
            //return RedirectToAction("Index", new { Eesfa = Eesfa });
            //  return RedirectToAction("Index", "ExamEligibleStudentForAdmin", Eesfa );
        }


        public ActionResult MarkNotEligible(int ExamID, int BatchID, int BatchProgramID, int YearSemesterNo, int ProgramCourseID, int ExamEligibleStudentForExamID)
        {

            ExamEligibleStudentsForAdminViewModel Eesfa = new ExamEligibleStudentsForAdminViewModel();
            Eesfa.ExamID = ExamID;
            Eesfa.BatchID = BatchID;
            Eesfa.BatchProgramID = BatchProgramID;
            Eesfa.YearSemesterNo = YearSemesterNo;
            Eesfa.ProgramCourseID = ProgramCourseID;
            db.UpdateExamEligibleStudentForExam(ExamEligibleStudentForExamID, Convert.ToInt32(Session["emp_id"]));
            TempData["EligibleStudents"] = Eesfa;
            return RedirectToAction("Index");

            // make Student un Eligible Code Here With static Remarks Here 
            // TempData["EligibleStudents"] = Eesfa;
            // return RedirectToAction("Index", new { Eesfa = Eesfa });
        }


        //public ActionResult IndexLoad(ExamEligibleStudentsForAdminViewModel Eesfa)
        //{

        //    ViewBag.ExamID = new SelectList(db.Exams, "ExamID", "ExamTitle", Eesfa.ExamID);
        //    ViewBag.BatchID = new SelectList(db.Batches, "BatchID", "BatchName", Eesfa.BatchID);
        //    List<GetBatchProgramNameConcat_Result> BatchProgramNameList = new List<GetBatchProgramNameConcat_Result>();
        //    GetBatchProgramNameConcat_Result GetBatchProgramNameConcat_ResultObj = new GetBatchProgramNameConcat_Result();
        //    GetBatchProgramNameConcat_ResultObj.ID = 0;
        //    GetBatchProgramNameConcat_ResultObj.Name = "--ALL--";
        //    BatchProgramNameList = db.GetBatchProgramNameConcat("", 0).ToList();
        //    BatchProgramNameList.Insert(0, GetBatchProgramNameConcat_ResultObj);
        //    ViewBag.BatchProgramID = new SelectList(BatchProgramNameList, "ID", "Name", Eesfa.BatchProgramID);
        //    ViewBag.YearSemesterNo = new SelectList(db.Semesters, "YearSemesterNo", "YearSemesterNo", Eesfa.YearSemesterNo);
        //    ViewBag.hdnBatchProgramID = Eesfa.BatchProgramID;
        //    ViewBag.hdnYearSemesterNo = Eesfa.YearSemesterNo;
        //    List<GetProgramCourse_Result> lstProgramCourse = new List<GetProgramCourse_Result>();
        //    lstProgramCourse = db.GetProgramCourse(0, 0, 0).ToList();
        //    GetProgramCourse_Result Obj = new GetProgramCourse_Result();
        //    Obj.ProgramCourseID = 0;
        //    Obj.CourseCode = "--ALL--";
        //    Obj.CourseName = "";
        //    lstProgramCourse.Insert(0, Obj);
        //    var ProgramCourse = lstProgramCourse.Select(S => new
        //    {
        //        ProgramCourseID = S.ProgramCourseID,
        //        CourseName = S.CourseCode + " - " + S.CourseName
        //    });
        //    ViewBag.ProgramCourseID = new SelectList(ProgramCourse, "ProgramCourseID", "CourseName", Eesfa.ProgramCourseID);
        //    ViewBag.MessageType = "";
        //    ViewBag.Message = "";
        //    return View(Eesfa);

        //}


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