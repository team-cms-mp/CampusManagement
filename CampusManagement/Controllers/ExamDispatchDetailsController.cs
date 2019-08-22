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
    public class ExamDispatchDetailsController : Controller
    {
        private ModelCMSNewContainer db = new ModelCMSNewContainer();
        ExamDispatchDetailsViewModel model = new ExamDispatchDetailsViewModel();
        // GET: ExamDispatchDetails
        public ActionResult Index()
        {
            List<Exam> ExamList = new List<Exam>();
            ExamList = db.Exams.ToList();
            if (ExamList != null)
            {
                if (ExamList.Count > 0)
                {
                    model.ExamID = 
                    ViewBag.hdnExamID = ExamList[0].ExamID;
                }
            }
            ViewBag.ExamID = new SelectList(db.Exams, "ExamID", "ExamTitle");
            ViewBag.ExamDateID = new SelectList(db.GetExamDateByExamID(0), "ExamDateID", "ExamDateTitle");
            ViewBag.ExamDateTimeSlotID = new SelectList(db.GetExamDateTimeSlotByExamID(0), "ExamDateTimeSlotID", "TimeSlot");
            //ViewBag.ExamDateTimeSlotID = new SelectList(db.Semesters, "YearSemesterNo", "YearSemesterNo");

            ViewBag.MessageType = "";
            ViewBag.Message = "";
            return View(model);
           
        }
        [HttpPost]
        public ActionResult Index(ExamDispatchDetailsViewModel RA)
        {
            string ErrorMessage = "";
            int count = 0;
            ViewBag.hdnExamDateID = RA.ExamDateID;
            ViewBag.hdnExamDateTimeSlotID = RA.ExamDateTimeSlotID;
            ViewBag.hdnExamID = RA.ExamID;

            //if (RA.ExamDateID == 0)
            //{
            //    count++;
            //    ErrorMessage += count + "-Please select Exam Date.<br/>";
            //}
            //if (RA.ExamDateTimeSlotID == 0)
            //{
            //    count++;
            //    ErrorMessage += count + "-Please select Exam Time Slot.<br/>";
            //}

            //if (!string.IsNullOrEmpty(ErrorMessage))
            //{
            //    ViewBag.MessageType = "error";
            //    ViewBag.Message = ErrorMessage;
            //}
            //else
            //{

            //    ViewBag.MessageType = "";
            //    ViewBag.Message = "";
            //}


            ViewBag.ExamID = new SelectList(db.Exams, "ExamID", "ExamTitle");
            ViewBag.ExamDateID = new SelectList(db.GetExamDateByExamID(RA.ExamDateID), "ExamDateID", "ExamDateTitle");
            ViewBag.ExamDateTimeSlotID = new SelectList(db.GetExamDateTimeSlotByExamID(RA.ExamDateTimeSlotID), "ExamDateTimeSlotID", "TimeSlot");
            //ViewBag.ExamDateTimeSlotID = new SelectList(db.Semesters, "YearSemesterNo", "YearSemesterNo");
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc");
            return View(RA);
        }

        public JsonResult GetExamDatesByExamID(string ExamID)
        {
            List<GetExamDateByExamID_Result> lstExamDates = new List<GetExamDateByExamID_Result>();

            lstExamDates = db.GetExamDateByExamID(Convert.ToInt32(ExamID)).ToList();
            var Exams = lstExamDates.Select(p => new
            {
                ExamDateID = p.ExamDateID,
                ExamDateTitle = p.ExamDateTitle
            });
            string result = JsonConvert.SerializeObject(Exams, Formatting.Indented);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetExamDateTimeSlotByExamID(string ExamID)
        {
            List<GetExamDateTimeSlotByExamID_Result> lstTimeSlots = new List<GetExamDateTimeSlotByExamID_Result>();
            int ExamIDInt = Convert.ToInt32(ExamID);

            lstTimeSlots = db.GetExamDateTimeSlotByExamID(ExamIDInt).ToList();
            var semesters = lstTimeSlots.Select(S => new
            {
                ExamDateTimeSlotID = S.ExamDateTimeSlotID,
                TimeSlot = S.TimeSlot
            });
            string result = JsonConvert.SerializeObject(semesters, Formatting.Indented);
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