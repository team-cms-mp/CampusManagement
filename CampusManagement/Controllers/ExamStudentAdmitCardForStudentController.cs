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
    public class ExamStudentAdmitCardForStudentController : Controller
    {
        private ModelCMSNewContainer db = new ModelCMSNewContainer();
        ExamStudentAdmitCardDetailsViewModel model = new ExamStudentAdmitCardDetailsViewModel();
        // GET: ExamStudentAdmitCardForStudent
        public ActionResult Index()
        {
            Exam ExamObj = new Exam();
            ExamObj = db.Exams.Where(x => x.IsDateSheetApproved == 1 && x.IsActive == "Yes").FirstOrDefault();

            if (ExamObj != null)
            {
                ViewBag.ExamID = new SelectList(db.Exams, "ExamID", "ExamTitle", ExamObj.ExamID);
                ViewBag.hdnExamID = ExamObj.ExamID;
                if (Session["CurrentUserID"] != null)
                {
                    ViewBag.StudentUserID = Convert.ToInt32(Session["CurrentUserID"]);
                }
                else {
                    ViewBag.StudentUserID = -1;
                }
                
            }
        
            ViewBag.MessageType = "";
            ViewBag.Message = "";
            return View(model);
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