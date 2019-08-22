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
    [Authorize(Roles = "Account Officer,Accounts Officer,Admin Assistant,Admin Officer,Admin.Assistant,Assist. Account Officer,Assist.Technician,Import Manager,Manager Servive & Support,Office Manager,Officer QMS,RSM - Center 2,RSM - South,Sales & Service Executive,Sales Executive,Sales Manager,Sales Representative,Sr.Accounts Officer,Sr.Associate Engineer,Sr.Sales Executive,Sr.Sales Representative,Store Assistant,Store Incharge,Technician")]
    public class TimeSlotCourseAllocationsController : Controller
    {
        private ModelCMSContainer db = new ModelCMSContainer();
        TimeSlotCourseAllocationsViewModel model = new TimeSlotCourseAllocationsViewModel();

        public ActionResult Index()
        {
            model.TimeSlotCourseAllocations = db.TimeSlotCourseAllocations.OrderByDescending(a =>a.TimeSlotCourseAllocationID).ToList();
            model.SelectedTimeSlotCourseAllocation = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc");
            ViewBag.TimeSlotID = new SelectList(db.GetBatchProgramNameConcat("", 4), "ID", "Name");
            ViewBag.TCourseAllocationID = new SelectList(db.GetBatchProgramNameConcat("", 5), "ID", "Name");
            ViewBag.MessageType = "";
            ViewBag.Message = "";
            return View(model);
        }

        [HttpGet]
        public ActionResult Create()
        {
            model.TimeSlotCourseAllocations = db.TimeSlotCourseAllocations.OrderByDescending(a =>a.TimeSlotCourseAllocationID).ToList();
            model.SelectedTimeSlotCourseAllocation = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc");
            ViewBag.TimeSlotID = new SelectList(db.GetBatchProgramNameConcat("", 4), "ID", "Name");
            ViewBag.TCourseAllocationID = new SelectList(db.GetBatchProgramNameConcat("", 5), "ID", "Name");
            ViewBag.MessageType = "";
            ViewBag.Message = "";
            return View("Index", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(TimeSlotCourseAllocation timeSlotCourseAllocation)
        {
            string ErrorMessage = "";
            int count = 0;
            try
            {
                TimeSlotCourseAllocation tc = db.TimeSlotCourseAllocations.FirstOrDefault(
                    p => p.TimeSlotID == timeSlotCourseAllocation.TimeSlotID);

                if (tc != null)
                {
                    ViewBag.MessageType = "error";
                    ViewBag.Message = "Selected Slot is booked.";
                    ModelState.AddModelError(string.Empty, "Selected Slot is booked.");
                }
                else
                {
                    timeSlotCourseAllocation.CreatedBy = Convert.ToInt32(Session["emp_id"]);
                    timeSlotCourseAllocation.CreatedOn = DateTime.Now;
                    db.TimeSlotCourseAllocations.Add(timeSlotCourseAllocation);
                    try
                    {
                        db.SaveChanges();
                        db.InsertTimeTable(timeSlotCourseAllocation.TimeSlotID, timeSlotCourseAllocation.TCourseAllocationID, 0);
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
            model.TimeSlotCourseAllocations = db.TimeSlotCourseAllocations.OrderByDescending(a =>a.TimeSlotCourseAllocationID).ToList();
            model.SelectedTimeSlotCourseAllocation = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc", timeSlotCourseAllocation.IsActive);
            ViewBag.TimeSlotID = new SelectList(db.GetBatchProgramNameConcat("", 4), "ID", "Name", timeSlotCourseAllocation.TimeSlotID);
            ViewBag.TCourseAllocationID = new SelectList(db.GetBatchProgramNameConcat("", 5), "ID", "Name", timeSlotCourseAllocation.TCourseAllocationID);
            return View("Index", model);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TimeSlotCourseAllocation timeSlotCourseAllocation = db.TimeSlotCourseAllocations.Find(id);
            if (timeSlotCourseAllocation == null)
            {
                return HttpNotFound();
            }

            model.TimeSlotCourseAllocations = db.TimeSlotCourseAllocations.OrderByDescending(a =>a.TimeSlotCourseAllocationID).ToList();
            model.SelectedTimeSlotCourseAllocation = timeSlotCourseAllocation;
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
                TimeSlotCourseAllocation timeSlotCourseAllocation = db.TimeSlotCourseAllocations.Find(id);
                db.InsertTimeTable(timeSlotCourseAllocation.TimeSlotID, timeSlotCourseAllocation.TCourseAllocationID, 1);
                db.TimeSlotCourseAllocations.Remove(timeSlotCourseAllocation);
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
            model.TimeSlotCourseAllocations = db.TimeSlotCourseAllocations.OrderByDescending(a =>a.TimeSlotCourseAllocationID).ToList();
            model.SelectedTimeSlotCourseAllocation = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc");
            ViewBag.TimeSlotID = new SelectList(db.GetBatchProgramNameConcat("", 4), "ID", "Name");
            ViewBag.TCourseAllocationID = new SelectList(db.GetBatchProgramNameConcat("", 5), "ID", "Name");
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
