using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CampusManagement.Models;

namespace CampusManagement.Controllers
{
    [Authorize]
    public class TimeTableManualTimeSlotsController : Controller
    {
        private ModelCMSNewContainer db = new ModelCMSNewContainer();

        tt_GetTimeTableManualTimeSlot_ResultViewModel model = new tt_GetTimeTableManualTimeSlot_ResultViewModel();

        public ActionResult Index()
        {
            model.tt_GetTimeTableManualTimeSlot_Results = db.tt_GetTimeTableManualTimeSlot("").ToList();
            model.Selectedtt_GetTimeTableManualTimeSlot_Result = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc");
            ViewBag.DayName = new SelectList(db.Days, "DayName", "DayName");
            ViewBag.RoomID = new SelectList(db.Rooms, "RoomID", "RoomName");
            ViewBag.TimeTableSlotID = new SelectList(db.TimeTableSlots, "TimeTableSlotID", "TimeTableSlotName");
            ViewBag.MessageType = "";
            ViewBag.Message = "";
            return View(model);
        }

        [HttpGet]
        public ActionResult Create()
        {
            model.tt_GetTimeTableManualTimeSlot_Results = db.tt_GetTimeTableManualTimeSlot("").ToList();
            model.Selectedtt_GetTimeTableManualTimeSlot_Result = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc");
            ViewBag.DayName = new SelectList(db.Days, "DayName", "DayName");
            ViewBag.RoomID = new SelectList(db.Rooms, "RoomID", "RoomName");
            ViewBag.TimeTableSlotID = new SelectList(db.TimeTableSlots, "TimeTableSlotID", "TimeTableSlotName");
          
            ViewBag.MessageType = "";
            ViewBag.Message = "";
            return View("Index", model);
        }

      
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(tt_GetTimeTableManualTimeSlot_Result timeSlot)
        {
            string ErrorMessage = "";
            int count = 0;
           int? Emp_id = Convert.ToInt32(Session["emp_id"]);
            //tt_GetTimeTableManualTimeSlot_Result ts = new tt_GetTimeTableManualTimeSlot_Result();
            try
            {
                try
                {
                    db.tt_InsertUpdateDelete_TimeTableManualTimeSlot(0,timeSlot.TimeTableSlotID,timeSlot.DayName,timeSlot.RoomID,Emp_id, "Yes",0,1);
 
                        ViewBag.MessageType = "success";
                        ViewBag.Message = "Data has been saved successfully.";
                 
                }
                catch (DbUpdateException ex)
                {
                    ViewBag.MessageType = "error";
                    ViewBag.Message = ex.Message;
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }
            catch (DbEntityValidationException ex)
            {
                foreach (DbEntityValidationResult validationResult in ex.EntityValidationErrors)
                {
                    string entityName = validationResult.Entry.Entity.GetType().Name;
                    foreach (DbValidationError error in validationResult.ValidationErrors)
                    {
                        ModelState.AddModelError(string.Empty, error.ErrorMessage);
                        count++;
                        ErrorMessage += count + "-" + string.Concat(error.PropertyName, " is required.") + "<br />";
                    }
                }
                ViewBag.MessageType = "error";
                ViewBag.Message = ErrorMessage;
            }
            model.tt_GetTimeTableManualTimeSlot_Results = db.tt_GetTimeTableManualTimeSlot("").ToList();
            model.Selectedtt_GetTimeTableManualTimeSlot_Result = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc", "Yes");
            ViewBag.DayName = new SelectList(db.Days, "DayName", "DayName", timeSlot.DayName);
            ViewBag.RoomID = new SelectList(db.Rooms, "RoomID", "RoomName", timeSlot.RoomID);
            ViewBag.TimeTableSlotID = new SelectList(db.TimeTableSlots, "TimeTableSlotID", "TimeTableSlotName",timeSlot.TimeTableSlotID);

            return View("Index", model);
        }
        
       

        public ActionResult Delete(int? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                db.tt_InsertUpdateDelete_TimeTableManualTimeSlot(id, 0, "", 0, 0, "Yes", 0,2);

                model.tt_GetTimeTableManualTimeSlot_Results = db.tt_GetTimeTableManualTimeSlot("").ToList();
                model.Selectedtt_GetTimeTableManualTimeSlot_Result = null;
                ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc");
                ViewBag.DayName = new SelectList(db.Days, "DayName", "DayName");
                ViewBag.RoomID = new SelectList(db.Rooms, "RoomID", "RoomName");
                ViewBag.TimeTableSlotID = new SelectList(db.TimeTableSlots, "TimeTableSlotID", "TimeTableSlotName");
                model.DisplayMode = "WriteOnly";
                ViewBag.MessageType = "";
                ViewBag.Message = "";
                ViewBag.MessageType = "success";
                ViewBag.Message = "Data  has been deleted successfully.";

            }
            catch (DbUpdateException ex)
            {
                ViewBag.MessageType = "error";
                ViewBag.Message = ex.Message;
                ModelState.AddModelError(string.Empty, ex.Message);
            }
           
            return View("Index", model);
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
