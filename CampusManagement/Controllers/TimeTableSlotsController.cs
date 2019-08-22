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
    public class TimeTableSlotsController : Controller
    {
        private ModelCMSNewContainer db = new ModelCMSNewContainer();
        TimeTableSlotsViewModel model = new TimeTableSlotsViewModel();

        public ActionResult Index()
        {
            model.TimeTableSlots = db.TimeTableSlots.OrderByDescending(t => t.TimeTableSlotID).ToList();
            model.SelectedTimeTableSlot = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.TimeTableMainID = new SelectList(db.TimeTableMains, "TimeTableMainID", "TimeTableMainName");
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc");

            ViewBag.MessageType = "";
            ViewBag.Message = "";
            return View(model);
        }

        [HttpGet]
        public ActionResult Create()
        {
            model.TimeTableSlots = db.TimeTableSlots.OrderByDescending(t => t.TimeTableSlotID).ToList();
            model.SelectedTimeTableSlot = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.TimeTableMainID = new SelectList(db.TimeTableMains, "TimeTableMainID", "TimeTableMainName");
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc");

            ViewBag.MessageType = "";
            ViewBag.Message = "";
            return View("Index", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(TimeTableSlot timetableSlot, FormCollection fc)
        {

            string ErrorMessage = "";
            int count = 0;
            try
            {
                TimeTableSlot edts = db.TimeTableSlots.FirstOrDefault(d => d.TimeTableSlotName == timetableSlot.TimeTableSlotName
                && d.TimeTableMainID == timetableSlot.TimeTableMainID);

                if (edts == null)
                {
                    string StartTime = string.Concat(fc["StartTime"]);
                    string EndTime = string.Concat(fc["EndTime"]);

                    TimeTableSlot tts = new TimeTableSlot();
                    tts.CreatedBy = Convert.ToInt32(Session["emp_id"]);
                    tts.CreatedOn = DateTime.Now;
                    tts.TimeTableSlotName = StartTime + "-" + EndTime;
                    tts.TimeTableMainID = timetableSlot.TimeTableMainID;
                    tts.IsActive = timetableSlot.IsActive;

                    db.TimeTableSlots.Add(tts);
                    try
                    {
                        db.SaveChanges();
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
                else
                {
                    ViewBag.MessageType = "error";
                    ViewBag.Message = "Time Slot already exists.";
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

            model.TimeTableSlots = db.TimeTableSlots.OrderByDescending(t => t.TimeTableSlotID).ToList();
            model.SelectedTimeTableSlot = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.TimeTableMainID = new SelectList(db.TimeTableMains, "TimeTableMainID", "TimeTableMainName");
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc");

            ViewBag.MessageType = "";
            ViewBag.Message = "";
            return View("Index", model);
        }

        public ActionResult Update(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            TimeTableSlot timeTableslot = db.TimeTableSlots.Find(id);
            if (timeTableslot == null)
            {
                return HttpNotFound();
            }
            string[] timeSolts = timeTableslot.TimeTableSlotName.Split('-');

            model.TimeTableSlots = db.TimeTableSlots.OrderByDescending(a => a.TimeTableSlotID).ToList();
            model.SelectedTimeTableSlot = timeTableslot;
            ViewBag.hdnStartTime = timeSolts[0];
            ViewBag.hdnEndTime = timeSolts[1];
            model.DisplayMode = "ReadWrite";
            ViewBag.TimeTableMainID = new SelectList(db.TimeTableMains, "TimeTableMainID", "TimeTableMainName");
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc", timeTableslot.IsActive);
            ViewBag.MessageType = "";
            ViewBag.Message = "";
            return View("Index", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(TimeTableSlot timeTableSolt, FormCollection fc)
        {
            try
            {
                string StartTime = string.Concat(fc["StartTime"]);
                string EndTime = string.Concat(fc["EndTime"]);
                timeTableSolt.TimeTableSlotName = StartTime + "-" + EndTime;
                db.Entry(timeTableSolt).State = EntityState.Modified;
                timeTableSolt.TimeTableMainID = timeTableSolt.TimeTableMainID;
                timeTableSolt.IsActive = timeTableSolt.IsActive;
                timeTableSolt.ModifiedBy = Convert.ToInt32(Session["emp_id"]);
                timeTableSolt.ModifiedOn = DateTime.Now;
                try
                {
                    db.SaveChanges();
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
                string ErrorMessage = "";
                int count = 0;
                foreach (DbEntityValidationResult validationResult in ex.EntityValidationErrors)
                {
                    string entityName = validationResult.Entry.Entity.GetType().Name;
                    foreach (DbValidationError error in validationResult.ValidationErrors)
                    {
                        ModelState.AddModelError(string.Empty, error.ErrorMessage);
                        count++;
                        ErrorMessage += string.Concat(count, "-", error.ErrorMessage, "\n");
                    }
                }
                ViewBag.MessageType = "error";
                ViewBag.Message = ErrorMessage;
            }
            model.TimeTableSlots = db.TimeTableSlots.OrderByDescending(a => a.TimeTableSlotID).ToList();
            model.SelectedTimeTableSlot = timeTableSolt;

            model.DisplayMode = "WriteOnly";

            ViewBag.TimeTableMainID = new SelectList(db.TimeTableMains, "TimeTableMainID", "TimeTableMainName");
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc", timeTableSolt.IsActive);

            return View("Index", model);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TimeTableSlot timeTaleSlot = db.TimeTableSlots.Find(id);
            if (timeTaleSlot == null)
            {
                return HttpNotFound();
            }

            model.TimeTableSlots = db.TimeTableSlots.OrderByDescending(t => t.TimeTableSlotID).ToList();
            model.SelectedTimeTableSlot = null;
            model.DisplayMode = "Delete";
            ViewBag.MessageType = "";
            ViewBag.Message = "";
            return View("Index", model);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TimeTableSlot timeTableslot = db.TimeTableSlots.Find(id);
            try
            {
                db.TimeTableSlots.Remove(timeTableslot);
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

            model.TimeTableSlots = db.TimeTableSlots.OrderByDescending(t => t.TimeTableSlotID).ToList();
            model.DisplayMode = "WriteOnly";
            ViewBag.TimeTableMainID = new SelectList(db.TimeTableMains, "TimeTableMainID", "TimeTableMainName");
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
