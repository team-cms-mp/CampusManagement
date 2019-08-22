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
    public class ExamResultDisplayController : Controller
    {
        private ModelCMSNewContainer db = new ModelCMSNewContainer();
        ExamSubjectsViewModel model = new ExamSubjectsViewModel();
        ExamSubjectsStudentViewModel modelstudentlist = new ExamSubjectsStudentViewModel();
        ExamResultForDisplayViewModel modelExamResultForDisplay = new ExamResultForDisplayViewModel();
        


        // GET: ExamResultDisplay
        public ActionResult Index()
        {
            ViewBag.ExamID = new SelectList(db.Exams, "ExamID", "ExamTitle");
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