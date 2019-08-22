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
    public class ExamSubjectsForResultController : Controller
    {
        private ModelCMSNewContainer db = new ModelCMSNewContainer();
        ExamSubjectsViewModel model = new ExamSubjectsViewModel();
        ExamSubjectsStudentViewModel modelstudentlist = new ExamSubjectsStudentViewModel();
        ExamStudentSubjectMarkViewModel modelStudentMarks = new ExamStudentSubjectMarkViewModel();
        ExamStudentSubjectMarkDetailViewModel modelStudentMarksDetail = new ExamStudentSubjectMarkDetailViewModel();
        // GET: ExamSubjectsForResult
        public ActionResult Index()
        {
            ViewBag.ExamID = new SelectList(db.Exams, "ExamID", "ExamTitle");
            // ViewBag.hdnExamID = db.Exams.FirstOrDefault().ExamID;
            ViewBag.MessageType = "";
            ViewBag.Message = "";
            return View(model);
        }

        [HttpPost]
        public ActionResult Index(ExamSubjectsViewModel ES)
        {
            ViewBag.hdnExamID = ES.ExamID;
            ViewBag.MessageType = "";
            ViewBag.Message = "";
            ViewBag.ExamID = new SelectList(db.Exams, "ExamID", "ExamTitle");

            return View(ES);
        }

        public ActionResult ExamSubjectsStudent(int? ExamID, int? ProgramCourseID, int? ExamsDateSheetDetailID, int? TeacherID)
        {
            modelstudentlist.StudeltList = db.GetExamSubjectsStudentForResult(ExamID, ProgramCourseID).ToList();

            return View(modelstudentlist);
        }

        public ActionResult ExamSubjectsStudentMarkEntry(int? ExamID, int? ProgramCourseID, int? StudentID)
        {

            
            ViewBag.MessageType = "";
            ViewBag.Message = "";
            modelStudentMarks.ExamStudentSubjectMarkObj = db.GetExamStudentSubjectMarkByID(ExamID, ProgramCourseID, StudentID).FirstOrDefault();
            if (modelStudentMarks.ExamStudentSubjectMarkObj.ExamStudentSubjectMarkID == 0)
            {
                GetExamStudentSubjectMarkByID_Result Obj = new GetExamStudentSubjectMarkByID_Result();
                Obj.ExamID = ExamID;
                Obj.ProgramCourseID = ProgramCourseID;
                Obj.StudentID = StudentID;
                modelStudentMarks.ExamStudentSubjectMarkObj = Obj;
                ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc");
                ViewBag.StudentPresentStatusID = new SelectList(db.StudentPresentStatus, "StudentPresentStatusID", "StudentPresentStatusName");

            }
            else {
                ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc", modelStudentMarks.ExamStudentSubjectMarkObj.IsActive);
                ViewBag.StudentPresentStatusID = new SelectList(db.StudentPresentStatus, "StudentPresentStatusID", "StudentPresentStatusName", modelStudentMarks.ExamStudentSubjectMarkObj.StudentPresentStatusID);
            }
           
            return View(modelStudentMarks);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateExamSubjectsStudentMarkEntry(ExamStudentSubjectMarkViewModel ESSM , FormCollection fc)
        {
            int CurrentUserID =  Convert.ToInt32(Session["CurrentUserID"]);
            int StudentPresentStatusID = Convert.ToInt32(fc["StudentPresentStatusID"]) ;
            string IsActive = fc["IsActive"];
            ESSM.ExamStudentSubjectMarkObj.CreatedBy = CurrentUserID;
            ESSM.ExamStudentSubjectMarkObj.ModifiedBy = CurrentUserID;
            ESSM.ExamStudentSubjectMarkObj.StudentPresentStatusID = StudentPresentStatusID;
            ESSM.ExamStudentSubjectMarkObj.IsActive = IsActive;
            db.InsertOrUpdateExamStudentSubjectMark(ESSM.ExamStudentSubjectMarkObj.ExamStudentSubjectMarkID, ESSM.ExamStudentSubjectMarkObj.ExamID, ESSM.ExamStudentSubjectMarkObj.ProgramCourseID, ESSM.ExamStudentSubjectMarkObj.StudentID, ESSM.ExamStudentSubjectMarkObj.TotalSubjectMarks, ESSM.ExamStudentSubjectMarkObj.WrittenTestMarks, ESSM.ExamStudentSubjectMarkObj.ObtainWrittenTestMarks, ESSM.ExamStudentSubjectMarkObj.StudentPresentStatusID, ESSM.ExamStudentSubjectMarkObj.IsTeacherSubmitted, ESSM.ExamStudentSubjectMarkObj.CreatedBy, ESSM.ExamStudentSubjectMarkObj.ModifiedBy);

            return RedirectToAction("ExamSubjectsStudentMarkEntry", new { ESSM.ExamStudentSubjectMarkObj.ExamID, ESSM.ExamStudentSubjectMarkObj.ProgramCourseID, ESSM.ExamStudentSubjectMarkObj.StudentID });
           // return View();
            //return View("Index", model);
        }

        public ActionResult UpdateExamSubjectsStudentMarkEntry(int? ExamStudentSubjectMarkID, int? ExamID, int? ProgramCourseID, int? StudentID)
        {
            int CurrentUserID = Convert.ToInt32(Session["CurrentUserID"]);
            int result  = db.UpdateExamSubjectsStudentMarkEntry(ExamStudentSubjectMarkID, CurrentUserID);
            return RedirectToAction("ExamSubjectsStudentMarkEntry", new { ExamID, ProgramCourseID, StudentID });
        }


        public ActionResult ExamSubjectsStudentMarkDetailEntry(int? ExamStudentSubjectMarkID, int? ExamID, int? ProgramCourseID, int? StudentID)
        {
           

            ViewBag.MessageType = "";
            ViewBag.Message = "";
            modelStudentMarksDetail.ExamStudentSubjectMarkDetailList = db.GetExamStudentSubjectMarkDetailByID(ExamStudentSubjectMarkID,ExamID, ProgramCourseID, StudentID).ToList();

            GetExamStudentSubjectMarkDetailByID_Result Obj = new GetExamStudentSubjectMarkDetailByID_Result();
            Obj.ExamStudentSubjectMarkID = Convert.ToInt32(ExamStudentSubjectMarkID);
            Obj.ExamID = ExamID;
            Obj.ProgramCourseID = ProgramCourseID;
            Obj.StudentID = StudentID;
            modelStudentMarksDetail.ExamStudentSubjectMarkDetailObj = Obj;
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc");
            ViewBag.StudentPresentStatusID = new SelectList(db.StudentPresentStatus, "StudentPresentStatusID", "StudentPresentStatusName");
            ViewBag.ExamStudentSubjectMarkTypeID = new SelectList(db.ExamStudentSubjectMarkTypes, "ExamStudentSubjectMarkTypeID", "MarkTypeName");


            //if (modelStudentMarksDetail.ExamStudentSubjectMarkDetailList != null) {
            //    if (modelStudentMarksDetail.ExamStudentSubjectMarkDetailList.Count > 0) {

            //        if (modelStudentMarksDetail.ExamStudentSubjectMarkDetailList[0].ExamStudentSubjectMarkDetailID == 0)
            //        {
            //            GetExamStudentSubjectMarkDetailByID_Result Obj = new GetExamStudentSubjectMarkDetailByID_Result();
            //            Obj.ExamStudentSubjectMarkID = Convert.ToInt32(ExamStudentSubjectMarkID);
            //            Obj.ExamID = ExamID;
            //            Obj.ProgramCourseID = ProgramCourseID;
            //            Obj.StudentID = StudentID;
            //            modelStudentMarksDetail.ExamStudentSubjectMarkDetailObj = Obj;
            //            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc");
            //            ViewBag.StudentPresentStatusID = new SelectList(db.StudentPresentStatus, "StudentPresentStatusID", "StudentPresentStatusName");
            //            ViewBag.ExamStudentSubjectMarkTypeID = new SelectList(db.ExamStudentSubjectMarkTypes, "ExamStudentSubjectMarkTypeID", "MarkTypeName");

            //        }


            //    }

            //}


            return View(modelStudentMarksDetail);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateExamSubjectsStudentMarkDetailEntry(ExamStudentSubjectMarkDetailViewModel ESSMD, FormCollection fc)
        {
            int CurrentUserID = Convert.ToInt32(Session["CurrentUserID"]);
            int StudentPresentStatusID = Convert.ToInt32(fc["StudentPresentStatusID"]);
            int ExamStudentSubjectMarkTypeID = Convert.ToInt32(fc["ExamStudentSubjectMarkTypeID"]);
            string IsActive = fc["IsActive"];
            ESSMD.ExamStudentSubjectMarkDetailObj.CreatedBy = CurrentUserID;
            ESSMD.ExamStudentSubjectMarkDetailObj.ModifiedBy = CurrentUserID;
            ESSMD.ExamStudentSubjectMarkDetailObj.StudentPresentStatusID = StudentPresentStatusID;
            ESSMD.ExamStudentSubjectMarkDetailObj.IsActive = IsActive;
            db.InsertOrUpdateExamStudentSubjectMarkDetail(ESSMD.ExamStudentSubjectMarkDetailObj.ExamStudentSubjectMarkDetailID ,ESSMD.ExamStudentSubjectMarkDetailObj.ExamStudentSubjectMarkID, ESSMD.ExamStudentSubjectMarkDetailObj.ExamID, ESSMD.ExamStudentSubjectMarkDetailObj.ProgramCourseID, ESSMD.ExamStudentSubjectMarkDetailObj.StudentID, ExamStudentSubjectMarkTypeID,ESSMD.ExamStudentSubjectMarkDetailObj.TotalMarks, ESSMD.ExamStudentSubjectMarkDetailObj.ObtainMarks, ESSMD.ExamStudentSubjectMarkDetailObj.StudentPresentStatusID, ESSMD.ExamStudentSubjectMarkDetailObj.IsTeacherSubmitted, ESSMD.ExamStudentSubjectMarkDetailObj.CreatedBy, ESSMD.ExamStudentSubjectMarkDetailObj.ModifiedBy);

            return RedirectToAction("ExamSubjectsStudentMarkDetailEntry", new { ESSMD.ExamStudentSubjectMarkDetailObj.ExamStudentSubjectMarkID, ESSMD.ExamStudentSubjectMarkDetailObj.ExamID, ESSMD.ExamStudentSubjectMarkDetailObj.ProgramCourseID, ESSMD.ExamStudentSubjectMarkDetailObj.StudentID });


            //return View();
            //return View("Index", model);
        }

        public ActionResult UpdateExamSubjectsStudentMarkDetailEntry(int? ExamStudentSubjectMarkDetailID, int? ExamStudentSubjectMarkID, int? ExamID, int? ProgramCourseID, int? StudentID)
        {
            int CurrentUserID = Convert.ToInt32(Session["CurrentUserID"]);
            int result = db.UpdateExamStudentSubjectMarkDetailEntry(ExamStudentSubjectMarkDetailID, CurrentUserID);
            return RedirectToAction("ExamSubjectsStudentMarkDetailEntry", new { ExamStudentSubjectMarkID,ExamID, ProgramCourseID, StudentID });
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