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
    public class TimeSlotsController : Controller
    {
        private ModelCMSNewContainer db = new ModelCMSNewContainer();
        TimeSlotsViewModel model = new TimeSlotsViewModel();

        public ActionResult Index()
        {
            model.TimeSlots = db.TimeSlots.OrderByDescending(t => t.TimeSlotID).ToList();
            model.SelectedTimeSlot = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc");
            ViewBag.DayName = new SelectList(db.Days, "DayName", "DayName");
            ViewBag.RoomID = new SelectList(db.Rooms, "RoomID", "RoomName");
            ViewBag.DurationID = new SelectList(db.Durations, "DurationID", "DurationMinutes");
            FillHours();
            ViewBag.MessageType = "";
            ViewBag.Message = "";
            return View(model);
        }

        [HttpGet]
        public ActionResult Create()
        {
            model.TimeSlots = db.TimeSlots.OrderByDescending(t => t.TimeSlotID).ToList();
            model.SelectedTimeSlot = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc");
            ViewBag.DayName = new SelectList(db.Days, "DayName", "DayName");
            ViewBag.RoomID = new SelectList(db.Rooms, "RoomID", "RoomName");
            ViewBag.DurationID = new SelectList(db.Durations, "DurationID", "DurationMinutes");
            FillHours();
            ViewBag.MessageType = "";
            ViewBag.Message = "";
            return View("Index", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(TimeSlot timeSlot)
        {
            return CreateEditTimeSlots(timeSlot);
        }
        
        private ActionResult CreateEditTimeSlots(TimeSlot timeSlot)
        {
            string ErrorMessage = "";
            int count = 0;
            TimeSlot ts = new TimeSlot();
            try
            {
                try
                {
                    if (timeSlot.EndingHour <= timeSlot.StartingHour)
                    {
                        ViewBag.MessageType = "error";
                        ViewBag.Message = "Ending Hour should be greater than Starting Hour.";
                    }
                    else
                    {
                        DateTime dt = new DateTime(2017, 12, 29, timeSlot.StartingHour, 0, 0);
                        int slotDuration = timeSlot.DurationMunutes;
                        int slotsMinutes = (timeSlot.EndingHour - timeSlot.StartingHour) * 60;
                        int NumberOfSlots = slotsMinutes / slotDuration;

                        List<TimeSlot> lstTS = db.TimeSlots.Where(
                            t => t.DayName == timeSlot.DayName).ToList();

                        if (lstTS.Count > 0)
                        {
                            db.TimeSlots.RemoveRange(lstTS);
                            db.SaveChanges();
                        }

                        for (int i = 1; i <= NumberOfSlots; i++)
                        {
                            string strTimeSlot = dt.ToShortTimeString() + "-" + dt.AddMinutes(slotDuration).ToShortTimeString();
                            dt = dt.AddMinutes(slotDuration);
                            ts.TimeSlot1 = strTimeSlot.Replace(" ", "");
                            ts.DayName = timeSlot.DayName;
                            ts.RoomID = timeSlot.RoomID;
                            ts.DurationID = timeSlot.DurationID;
                            ts.IsActive = timeSlot.IsActive;
                            ts.CreatedBy = Convert.ToInt32(Session["emp_id"]);
                            ts.CreatedOn = DateTime.Now;
                            db.TimeSlots.Add(ts);
                            db.SaveChanges();
                        }
                        ViewBag.MessageType = "success";
                        ViewBag.Message = "Data has been saved successfully.";
                    }
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
            model.TimeSlots = db.TimeSlots.OrderByDescending(t => t.TimeSlotID).ToList();
            model.SelectedTimeSlot = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc", timeSlot.IsActive);
            ViewBag.DayName = new SelectList(db.Days, "DayName", "DayName", timeSlot.DayName);
            ViewBag.RoomID = new SelectList(db.Rooms, "RoomID", "RoomName", timeSlot.RoomID);
            ViewBag.DurationID = new SelectList(db.Durations, "DurationID", "DurationMinutes", timeSlot.DurationID);
            FillHours();
            return View("Index", model);
        }
        
        private void FillHours()
        {
            List<TimeSlot> lstHour = new List<TimeSlot>();
            for (int i = 1; i <= 24; i++)
            {
                TimeSlot h = new TimeSlot();
                h.StartingHour = i;
                h.EndingHour = i;
                lstHour.Add(h);
            }

            ViewBag.StartingHour = new SelectList(lstHour, "StartingHour", "StartingHour");
            ViewBag.EndingHour = new SelectList(lstHour, "EndingHour", "EndingHour");
        }


        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TimeSlot ts = db.TimeSlots.Find(id);
            if (ts == null)
            {
                return HttpNotFound();
            }

            model.TimeSlots = db.TimeSlots.OrderByDescending(a => a.TimeSlotID).ToList();
            model.SelectedTimeSlot = ts;
            model.DisplayMode = "Delete";
            ViewBag.MessageType = "";
            ViewBag.Message = "";
            return View("Index", model);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                TimeSlot ts = db.TimeSlots.Find(id);
                db.TimeSlots.Remove(ts);
                db.SaveChanges();
                ViewBag.MessageType = "success";
                ViewBag.Message = "Record has been removed successfully.";
            }
            catch (DbUpdateException ex)
            {
                ViewBag.MessageType = "error";
                ViewBag.Message = ex.Message;
                ModelState.AddModelError(string.Empty, ex.Message);
            }
            model.TimeSlots = db.TimeSlots.OrderByDescending(a => a.TimeSlotID).ToList();
            model.SelectedTimeSlot = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc");

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
