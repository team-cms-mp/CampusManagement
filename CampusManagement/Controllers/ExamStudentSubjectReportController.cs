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
    public class ExamStudentSubjectReportController : Controller
    {
        private ModelCMSNewContainer db = new ModelCMSNewContainer();
        ExamStudentSubjectReportViewModel model = new ExamStudentSubjectReportViewModel();
        // GET: ExamStudentSubjectReport
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
        public ActionResult Index(ExamStudentSubjectReportViewModel ESSR)
        {
            ViewBag.hdnExamID = ESSR.ExamID;  
            ViewBag.ExamID = new SelectList(db.Exams, "ExamID", "ExamTitle", ESSR.ExamID);
         
            return View(ESSR);
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