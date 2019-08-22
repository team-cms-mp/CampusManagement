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
    public class ExamStudentChangeGradeFormController : Controller
    {

        private ModelCMSNewContainer db = new ModelCMSNewContainer();
        ExamStudentSubjectForGradeRequestViewModel model = new ExamStudentSubjectForGradeRequestViewModel();
        ExamResultForDisplayViewModel modelExamResultForDisplay = new ExamResultForDisplayViewModel();
        ExamGardeFormDeatilViewModel modelExamGardeFormDeatil = new ExamGardeFormDeatilViewModel();

        // GET: ExamStudentChangeGradeForm
        public ActionResult Index()
        {
            Exam ExamObj = new Exam();
            ExamObj = db.Exams.Where(x => x.IsDateSheetApproved == 1 && x.IsActive == "Yes").FirstOrDefault();

            if (ExamObj != null)
            {
                ViewBag.ExamID = new SelectList(db.Exams, "ExamID", "ExamTitle", ExamObj.ExamID);
                ViewBag.hdnExamID = ExamObj.ExamID;
               // Session["CurrentUserID"] = "9005";
                if (Session["CurrentUserID"] != null)
                {
                    ViewBag.StudentUserID = Convert.ToInt32(Session["CurrentUserID"]);
                }
                else
                {
                    ViewBag.StudentUserID = -1;
                }

            }

            ViewBag.MessageType = "";
            ViewBag.Message = "";
            //return View(model);
            return View(model);
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

        public ActionResult ExamGradeFormRequest(int? ExamID, int? ProgramCourseID, int? StudentID,int? ExamStudentSubjectMarkID)
        {
            modelExamGardeFormDeatil.StudentSubjectDeatilForGardeFormObj = db.GetStudentSubjectDeatilForGardeForm(ProgramCourseID, StudentID).FirstOrDefault();
            modelExamGardeFormDeatil.ExamGardeFormDeatilByStudentIDObj = db.GetExamGardeFormDeatilByStudentID(ExamID,ProgramCourseID, StudentID).FirstOrDefault();


            if (modelExamGardeFormDeatil.ExamGardeFormDeatilByStudentIDObj != null) {
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
                   //  Session["CurrentUserID"] = "9005";
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
                                                    Convert.ToInt32(modelExamGardeFormDeatilObj.ExamGardeFormDeatilByStudentIDObj.IsTeacherApproveOrDisapprove) ,
                                                    modelExamGardeFormDeatilObj.ExamGardeFormDeatilByStudentIDObj.HodName,
                                                    Convert.ToInt32(modelExamGardeFormDeatilObj.ExamGardeFormDeatilByStudentIDObj.IsHodApproveOrDisapprove),
                                                    modelExamGardeFormDeatilObj.ExamGardeFormDeatilByStudentIDObj.DeanName,
                                                    Convert.ToInt32(modelExamGardeFormDeatilObj.ExamGardeFormDeatilByStudentIDObj.IsDeanApproveOrDisapprove),
                                                    modelExamGardeFormDeatilObj.ExamGardeFormDeatilByStudentIDObj.CreatedBy,
                                                    "",
                                                    modelExamGardeFormDeatilObj.ExamGardeFormDeatilByStudentIDObj.ModifiedBy);
         

            return RedirectToAction("ExamGradeFormRequest", new { ExamID = modelExamGardeFormDeatilObj.ExamGardeFormDeatilByStudentIDObj.ExamID, ProgramCourseID = modelExamGardeFormDeatilObj.ExamGardeFormDeatilByStudentIDObj.ProgramCourseID, StudentID = modelExamGardeFormDeatilObj.ExamGardeFormDeatilByStudentIDObj.StudentID, ExamStudentSubjectMarkID  = modelExamGardeFormDeatilObj.ExamGardeFormDeatilByStudentIDObj.ExamStudentSubjectMarkID }) ;
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