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
    public class ExamRosterDetailsForTeacherController : Controller
    {
        private ModelCMSNewContainer db = new ModelCMSNewContainer();
        ExamRosterDetailsTeacherViewModel model = new ExamRosterDetailsTeacherViewModel();
        // GET: ExamRosterDetailsForTeacher
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
                ViewBag.hdnExamID = db.Exams.FirstOrDefault().ExamID;
                ViewBag.ExamID = new SelectList(db.Exams, "ExamID", "ExamTitle", db.Exams.FirstOrDefault().ExamID);
            }

            int CurrentUserID = -1;
            if (Session["CurrentUserID"] != null) {
                CurrentUserID = Convert.ToInt32(Session["CurrentUserID"]);
            }
            model.EmpID = CurrentUserID;
            ViewBag.hdnEmpID = CurrentUserID;
            return View(model);
        }

        [HttpPost]
        public ActionResult Index(ExamRosterDetailsTeacherViewModel ERD)
        {
            ViewBag.hdnExamID = ERD.ExamID;
            ViewBag.hdnEmpID = ERD.EmpID;
            ViewBag.ExamID = new SelectList(db.Exams, "ExamID", "ExamTitle", ERD.ExamID);
            return View(ERD);
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