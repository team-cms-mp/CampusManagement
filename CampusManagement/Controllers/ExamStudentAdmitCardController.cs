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
    public class ExamStudentAdmitCardController : Controller
    {
        private ModelCMSNewContainer db = new ModelCMSNewContainer();
        ExamStudentAdmitCardDetailsViewModel model = new ExamStudentAdmitCardDetailsViewModel();
        // GET: ExamStudentAdmitCard
        public ActionResult Index()
        {
            ViewBag.ExamID = new SelectList(db.Exams, "ExamID", "ExamTitle");
            ViewBag.MessageType = "";
            ViewBag.Message = "";
            return View(model);
           
        }

        [HttpPost]
        public ActionResult Index(ExamStudentAdmitCardDetailsViewModel ESAC)
        {
            ViewBag.hdnExamID = ESAC.ExamID;

            ViewBag.MessageType = "";
            ViewBag.Message = "";
            

            ViewBag.ExamID = new SelectList(db.Exams, "ExamID", "ExamTitle");
         
            return View(ESAC);
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