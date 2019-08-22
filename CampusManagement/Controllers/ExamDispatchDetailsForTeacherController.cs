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
    public class ExamDispatchDetailsForTeacherController : Controller
    {
        private ModelCMSNewContainer db = new ModelCMSNewContainer();
        ExamDispatchDetailsViewModel model = new ExamDispatchDetailsViewModel();
        // GET: ExamDispatchDetailsForTeacher
        public ActionResult Index()
        {
            Exam ExamObj = new Exam();
            ExamObj = db.Exams.Where(x => x.IsDateSheetApproved == 1 && x.IsActive == "Yes").FirstOrDefault();
            
            if (ExamObj != null)
            {
                model.ExamID = ExamObj.ExamID;
                ViewBag.hdnExamID = ExamObj.ExamID;
                ViewBag.ExamID = new SelectList(db.Exams, "ExamID", "ExamTitle", ExamObj.ExamID);
                ViewBag.ExamDateID = new SelectList(GetExamDatesByExamID(ExamObj.ExamID), "ExamDateID", "ExamDateTitle");
                ViewBag.ExamDateTimeSlotID = new SelectList(GetExamDateTimeSlotByExamID(ExamObj.ExamID), "ExamDateTimeSlotID", "TimeSlot");
            }
           
            ViewBag.MessageType = "";
            ViewBag.Message = "";
            return View(model);
        }
        [HttpPost]
        public ActionResult Index(ExamDispatchDetailsViewModel RA)
        {
            ViewBag.hdnExamDateID = RA.ExamDateID;
            ViewBag.hdnExamDateTimeSlotID = RA.ExamDateTimeSlotID;
            ViewBag.hdnExamID = RA.ExamID;
            ViewBag.ExamID = new SelectList(db.Exams, "ExamID", "ExamTitle", RA.ExamID);
            ViewBag.ExamDateID = new SelectList(GetExamDatesByExamID(RA.ExamID), "ExamDateID", "ExamDateTitle", RA.ExamDateID);
            ViewBag.ExamDateTimeSlotID = new SelectList(GetExamDateTimeSlotByExamID(RA.ExamID), "ExamDateTimeSlotID", "TimeSlot", RA.ExamDateTimeSlotID);
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc");
            return View(RA);
        }

        //public JsonResult GetExamDatesByExamID(string ExamID)
        //{
        //    List<GetExamDateByExamID_Result> lstExamDates = new List<GetExamDateByExamID_Result>();
        //    GetExamDateByExamID_Result Obj = new GetExamDateByExamID_Result();
        //    Obj.ExamDateID = 0;
        //    Obj.ExamDateTitle = "--Please Select--";
        //    lstExamDates.Insert(0, Obj);

        //    lstExamDates = db.GetExamDateByExamID(Convert.ToInt32(ExamID)).ToList();
        //    var Exams = lstExamDates.Select(p => new
        //    {
        //        ExamDateID = p.ExamDateID,
        //        ExamDateTitle = p.ExamDateTitle
        //    });
        //    string result = JsonConvert.SerializeObject(Exams, Formatting.Indented);
        //    return Json(result, JsonRequestBehavior.AllowGet);
        //}

        public List<GetExamDateByExamID_Result> GetExamDatesByExamID(int ExamID)
        {
            List<GetExamDateByExamID_Result> lstExamDates = new List<GetExamDateByExamID_Result>();
            GetExamDateByExamID_Result Obj = new GetExamDateByExamID_Result();
            Obj.ExamDateID = 0;
            Obj.ExamDateTitle = "--Please Select--";
            lstExamDates = db.GetExamDateByExamID(Convert.ToInt32(ExamID)).ToList();
            lstExamDates.Insert(0, Obj);

            return lstExamDates;
        }


        //public JsonResult GetExamDateTimeSlotByExamID(string ExamID)
        //{
        //    List<GetExamDateTimeSlotByExamID_Result> lstTimeSlots = new List<GetExamDateTimeSlotByExamID_Result>();
        //    GetExamDateTimeSlotByExamID_Result Obj = new GetExamDateTimeSlotByExamID_Result();
        //    Obj.ExamDateTimeSlotID = 0;
        //    Obj.TimeSlot = "--Please Select--";
        //    int ExamIDInt = Convert.ToInt32(ExamID);
        //    lstTimeSlots = db.GetExamDateTimeSlotByExamID(ExamIDInt).ToList();
        //    lstTimeSlots.Insert(0,Obj);
        //    var timeslot = lstTimeSlots.Select(S => new
        //    {
        //        ExamDateTimeSlotID = S.ExamDateTimeSlotID,
        //        TimeSlot = S.TimeSlot
        //    });
        //    string result = JsonConvert.SerializeObject(timeslot, Formatting.Indented);
        //    return Json(result, JsonRequestBehavior.AllowGet);
        //}

        public List<GetExamDateTimeSlotByExamID_Result> GetExamDateTimeSlotByExamID(int ExamID)
        {
            List<GetExamDateTimeSlotByExamID_Result> lstTimeSlots = new List<GetExamDateTimeSlotByExamID_Result>();
            GetExamDateTimeSlotByExamID_Result Obj = new GetExamDateTimeSlotByExamID_Result();
            Obj.ExamDateTimeSlotID = 0;
            Obj.TimeSlot = "--Please Select--";
            int ExamIDInt = Convert.ToInt32(ExamID);
            lstTimeSlots = db.GetExamDateTimeSlotByExamID(ExamIDInt).ToList();
            lstTimeSlots.Insert(0, Obj);
            return lstTimeSlots;
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