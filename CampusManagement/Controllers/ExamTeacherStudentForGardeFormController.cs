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
    public class ExamTeacherStudentForGardeFormController : Controller
    {

        private ModelCMSNewContainer db = new ModelCMSNewContainer();
        ExamAdminStudentForGardeFormViewModel model = new ExamAdminStudentForGardeFormViewModel();
        ExamResultForDisplayViewModel modelExamResultForDisplay = new ExamResultForDisplayViewModel();
        ExamGardeFormDeatilViewModel modelExamGardeFormDeatil = new ExamGardeFormDeatilViewModel();
        // GET: ExamTeacherStudentForGardeForm
        public ActionResult Index()
        {
            Exam ExamObj = new Exam();
            ExamObj = db.Exams.Where(x => x.IsDateSheetApproved == 1 && x.IsActive == "Yes").FirstOrDefault();
            if (ExamObj != null)
            {
                ViewBag.ExamID = new SelectList(db.Exams, "ExamID", "ExamTitle", ExamObj.ExamID);
                ViewBag.hdnExamID = ExamObj.ExamID;
            }
            else
            {
                ExamObj = db.Exams.FirstOrDefault();
                List<Exam> ExamList = new List<Exam>();
                ExamList = db.Exams.ToList();
                if (ExamObj != null && ExamList != null)
                {
                    ViewBag.hdnExamID = ExamObj.ExamID;
                    ViewBag.ExamID = new SelectList(db.Exams, "ExamID", "ExamTitle", db.Exams.FirstOrDefault().ExamID);
                }
                else
                {
                    ExamObj = new Exam();
                    ExamObj.ExamTitle = "Please Add Exam First";
                    ExamObj.ExamID = 0;
                    ExamList.Add(ExamObj);
                    ViewBag.hdnExamID = 0;
                    ViewBag.ExamID = new SelectList(ExamList, "ExamID", "ExamTitle");
                }
            }
            return View(model);
        }


        [HttpPost]
        public ActionResult Index(ExamAdminStudentForGardeFormViewModel ESSR)
        {
            ViewBag.hdnExamID = ESSR.ExamID;
            ViewBag.ExamID = new SelectList(db.Exams, "ExamID", "ExamTitle", ESSR.ExamID);

            return View(ESSR);
        }


        public ActionResult ExamSubjectsStudentMarkDisplay(int? ExamID, int? ProgramCourseID, int? StudentID)
        {


            ViewBag.MessageType = "";
            ViewBag.Message = "";
            modelExamResultForDisplay.ExamResultForDisplayObj = db.GetExamResultForDisplay(ExamID, ProgramCourseID, StudentID).FirstOrDefault();
            if (modelExamResultForDisplay.ExamResultForDisplayObj.ExamStudentSubjectMarkID == 0)
            {
                GetExamResultForDisplay_Result Obj = new GetExamResultForDisplay_Result();
                Obj.ExamID = ExamID;
                Obj.ProgramCourseID = ProgramCourseID;
                Obj.StudentID = StudentID;
                modelExamResultForDisplay.ExamResultForDisplayObj = Obj;
            }


            return View(modelExamResultForDisplay);
        }


        public ActionResult ExamGradeFormRequest(int? ExamID, int? ProgramCourseID, int? StudentID, int? ExamStudentSubjectMarkID)
        {
            modelExamGardeFormDeatil.StudentSubjectDeatilForGardeFormObj = db.GetStudentSubjectDeatilForGardeForm(ProgramCourseID, StudentID).FirstOrDefault();
            modelExamGardeFormDeatil.ExamGardeFormDeatilByStudentIDObj = db.GetExamGardeFormDeatilByStudentID(ExamID, ProgramCourseID, StudentID).FirstOrDefault();


            if (modelExamGardeFormDeatil.ExamGardeFormDeatilByStudentIDObj != null)
            {
                if (modelExamGardeFormDeatil.ExamGardeFormDeatilByStudentIDObj.CreatedOn == null)
                {
                    modelExamGardeFormDeatil.ExamGardeFormDeatilByStudentIDObj.CreatedOn = DateTime.Now;
                }
                if (modelExamGardeFormDeatil.ExamGardeFormDeatilByStudentIDObj.ExamID == null || modelExamGardeFormDeatil.ExamGardeFormDeatilByStudentIDObj.ExamID == 0)
                {
                    modelExamGardeFormDeatil.ExamGardeFormDeatilByStudentIDObj.ExamID = ExamID;
                }
                if (modelExamGardeFormDeatil.ExamGardeFormDeatilByStudentIDObj.ProgramCourseID == null || modelExamGardeFormDeatil.ExamGardeFormDeatilByStudentIDObj.ProgramCourseID == 0)
                {
                    modelExamGardeFormDeatil.ExamGardeFormDeatilByStudentIDObj.ProgramCourseID = ProgramCourseID;
                }
                if (modelExamGardeFormDeatil.ExamGardeFormDeatilByStudentIDObj.StudentID == null || modelExamGardeFormDeatil.ExamGardeFormDeatilByStudentIDObj.StudentID == 0)
                {
                    modelExamGardeFormDeatil.ExamGardeFormDeatilByStudentIDObj.StudentID = StudentID;
                }
                if (modelExamGardeFormDeatil.ExamGardeFormDeatilByStudentIDObj.ExamStudentSubjectMarkID == null || modelExamGardeFormDeatil.ExamGardeFormDeatilByStudentIDObj.ExamStudentSubjectMarkID == 0)
                {
                    modelExamGardeFormDeatil.ExamGardeFormDeatilByStudentIDObj.ExamStudentSubjectMarkID = ExamStudentSubjectMarkID;
                }

                if (modelExamGardeFormDeatil.ExamGardeFormDeatilByStudentIDObj.CreatedBy == null || modelExamGardeFormDeatil.ExamGardeFormDeatilByStudentIDObj.CreatedBy == 0)
                {
                    //Session["CurrentUserID"] = "9005";
                    modelExamGardeFormDeatil.ExamGardeFormDeatilByStudentIDObj.CreatedBy = Convert.ToInt32(Session["CurrentUserID"]);
                }
            }

            return View(modelExamGardeFormDeatil);
        }


        public ActionResult ExamGradeFormRequestiInsertOrUpdate(ExamGardeFormDeatilViewModel modelExamGardeFormDeatilObj)
        {


            db.InsertOrUpdateExamStudentGradeRequest(modelExamGardeFormDeatilObj.ExamGardeFormDeatilByStudentIDObj.ExamStudentSubjectChangeGradeFormID,
                                                    modelExamGardeFormDeatilObj.ExamGardeFormDeatilByStudentIDObj.ExamStudentSubjectMarkID,
                                                    modelExamGardeFormDeatilObj.ExamGardeFormDeatilByStudentIDObj.ExamID,
                                                    modelExamGardeFormDeatilObj.ExamGardeFormDeatilByStudentIDObj.ProgramCourseID,
                                                    modelExamGardeFormDeatilObj.ExamGardeFormDeatilByStudentIDObj.StudentID,
                                                    modelExamGardeFormDeatilObj.ExamGardeFormDeatilByStudentIDObj.ObtainOldMarks,
                                                    modelExamGardeFormDeatilObj.ExamGardeFormDeatilByStudentIDObj.ObtainNewMarks,
                                                    modelExamGardeFormDeatilObj.StudentSubjectDeatilForGardeFormObj.YearSemesterNo,
                                                    modelExamGardeFormDeatilObj.ExamGardeFormDeatilByStudentIDObj.Reason,
                                                    modelExamGardeFormDeatilObj.ExamGardeFormDeatilByStudentIDObj.TeacherName,
                                                    Convert.ToInt32(modelExamGardeFormDeatilObj.ExamGardeFormDeatilByStudentIDObj.IsTeacherApproveOrDisapprove),
                                                    modelExamGardeFormDeatilObj.ExamGardeFormDeatilByStudentIDObj.HodName,
                                                    Convert.ToInt32(modelExamGardeFormDeatilObj.ExamGardeFormDeatilByStudentIDObj.IsHodApproveOrDisapprove),
                                                    modelExamGardeFormDeatilObj.ExamGardeFormDeatilByStudentIDObj.DeanName,
                                                    Convert.ToInt32(modelExamGardeFormDeatilObj.ExamGardeFormDeatilByStudentIDObj.IsDeanApproveOrDisapprove),
                                                    modelExamGardeFormDeatilObj.ExamGardeFormDeatilByStudentIDObj.CreatedBy,
                                                    "",
                                                    Convert.ToInt32(Session["CurrentUserID"]));


            return RedirectToAction("ExamGradeFormRequest", new { ExamID = modelExamGardeFormDeatilObj.ExamGardeFormDeatilByStudentIDObj.ExamID, ProgramCourseID = modelExamGardeFormDeatilObj.ExamGardeFormDeatilByStudentIDObj.ProgramCourseID, StudentID = modelExamGardeFormDeatilObj.ExamGardeFormDeatilByStudentIDObj.StudentID, ExamStudentSubjectMarkID = modelExamGardeFormDeatilObj.ExamGardeFormDeatilByStudentIDObj.ExamStudentSubjectMarkID });
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