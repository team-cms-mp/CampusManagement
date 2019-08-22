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
    public class ExamResultStudentDisplayController : Controller
    {
        private ModelCMSNewContainer db = new ModelCMSNewContainer();
        ExamStudentSubjectsForResult model = new ExamStudentSubjectsForResult();
        ExamResultForDisplayViewModel modelExamResultForDisplay = new ExamResultForDisplayViewModel();
        // GET: ExamResultStudentDisplay
        public ActionResult Index()
        {
            ViewBag.ExamID = new SelectList(db.Exams, "ExamID", "ExamTitle");
            ViewBag.MessageType = "";
            ViewBag.Message = "";
            return View(model);
        }

        [HttpPost]
        public ActionResult Index(ExamStudentSubjectsForResult ES)
        {
            ViewBag.hdnExamID = ES.ExamID;
            ViewBag.MessageType = "";
            ViewBag.Message = "";
            ViewBag.ExamID = new SelectList(db.Exams, "ExamID", "ExamTitle");

            return View(ES);
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

        public ActionResult RequestChangeGradeForm(int? ExamStudentSubjectMarkID,int? ExamStudentSubjectMarkDetailID, int? ExamID, int? ProgramCourseID, int? StudentID,int? StudentPresentStatusID, int? ExamStudentSubjectMarkTypeID, decimal TotalMarks , decimal ObtainMarks)
        {


            //ViewBag.MessageType = "";
            //ViewBag.Message = "";
            //modelExamResultForDisplay.ExamResultForDisplayObj = db.GetExamResultForDisplay(ExamID, ProgramCourseID, StudentID).FirstOrDefault();
            //if (modelExamResultForDisplay.ExamResultForDisplayObj.ExamStudentSubjectMarkID == 0)
            //{
            //    GetExamResultForDisplay_Result Obj = new GetExamResultForDisplay_Result();
            //    Obj.ExamID = ExamID;
            //    Obj.ProgramCourseID = ProgramCourseID;
            //    Obj.StudentID = StudentID;
            //    modelExamResultForDisplay.ExamResultForDisplayObj = Obj;
            //}



            //return View(modelExamResultForDisplay);
            return View();
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