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
    public class ExamDateTimeSoltsController : Controller
    {
        private ModelCMSNewContainer db = new ModelCMSNewContainer();
        GetExamDateTimeSlot_By_ExamIDExamDateID_ResultsViewModel model = new GetExamDateTimeSlot_By_ExamIDExamDateID_ResultsViewModel();

        public ActionResult Index(int? ExamID)
        {

            model.GetExamDateTimeSlot_By_ExamIDExamDateID_Results = db.GetExamDateTimeSlot_By_ExamIDExamDateID(ExamID).ToList();
            model.SelectedGetExamDateTimeSlot_By_ExamIDExamDateID_Result = null;
            model.DisplayMode = "WriteOnly";

            ViewBag.hdnExamID = ExamID;
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc");

            ViewBag.MessageType = "";
            ViewBag.Message = "";
            return View(model);
        }

        [HttpGet]
        public ActionResult Create(int? ExamID)
        {
        
            ViewBag.hdnExamID = ExamID;
            model.GetExamDateTimeSlot_By_ExamIDExamDateID_Results = db.GetExamDateTimeSlot_By_ExamIDExamDateID(ExamID).ToList();
            model.SelectedGetExamDateTimeSlot_By_ExamIDExamDateID_Result = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.ExamID = new SelectList(db.Exams, "ExamID", "ExamTitle");
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc");
            ViewBag.MessageType = "";
            ViewBag.Message = "";
            return View("Index", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(GetExamDateTimeSlot_By_ExamIDExamDateID_Result examDateTime)
        {
            try
            {
                GetExamDateTimeSlot_By_ExamIDExamDateID_Result ts = db.GetExamDateTimeSlot_By_ExamIDExamDateID(examDateTime.ExamID).FirstOrDefault(d => d.TimeSlot == examDateTime.TimeSlot);
                if (ts == null)
                {

                    List<GetExamDateTimeSlot_By_ExamIDExamDateID_Result> ListTS = db.GetExamDateTimeSlot_By_ExamIDExamDateID(examDateTime.ExamID).ToList();
                    ExamDateTimeSlot edts = new ExamDateTimeSlot();
                    if (ListTS.Count() < 5)
                    {
                        edts.CreatedBy = Convert.ToInt32(Session["emp_id"]);
                        edts.CreatedOn = DateTime.Now;

                        edts.TimeSlot = examDateTime.StartTime + "-" +examDateTime.EndTime;

                       
                        edts.ExamID = examDateTime.ExamID;
                        edts.IsActive = examDateTime.IsActive;
                        db.ExamDateTimeSlots.Add(edts);
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
                        ViewBag.Message = "You cannot add more then 5 time slots.";
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
                string ErrorMessage = "";
                int count = 0;
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
            model.GetExamDateTimeSlot_By_ExamIDExamDateID_Results = db.GetExamDateTimeSlot_By_ExamIDExamDateID(examDateTime.ExamID).ToList();
            model.SelectedGetExamDateTimeSlot_By_ExamIDExamDateID_Result = null;
          
            ViewBag.hdnExamID = examDateTime.ExamID;
            model.DisplayMode = "WriteOnly";

            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc", examDateTime.IsActive);

            return View("Index", model);
        }

        public ActionResult Update(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ExamDateTimeSlot examDateTime = db.ExamDateTimeSlots.Find(id);
            if (examDateTime == null)
            {
                return HttpNotFound();
            }
            string[] timeSolts = examDateTime.TimeSlot.Split('-');
               
            model.GetExamDateTimeSlot_By_ExamIDExamDateID_Results = db.GetExamDateTimeSlot_By_ExamIDExamDateID(examDateTime.ExamID).ToList();
            model.SelectedGetExamDateTimeSlot_By_ExamIDExamDateID_Result = db.GetExamDateTimeSlot_By_ExamIDExamDateID(examDateTime.ExamID).FirstOrDefault(s => s.ExamDateTimeSlotID == examDateTime.ExamDateTimeSlotID);
            model.SelectedGetExamDateTimeSlot_By_ExamIDExamDateID_Result.StartTime = timeSolts[0];
            model.SelectedGetExamDateTimeSlot_By_ExamIDExamDateID_Result.EndTime = timeSolts[1];
            model.DisplayMode = "ReadWrite";
          
            ViewBag.hdnExamID = examDateTime.ExamID;
            ViewBag.ExamID = new SelectList(db.Exams, "ExamID", "ExamTitle");
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc", examDateTime.IsActive);
          
          
            ViewBag.MessageType = "";
            ViewBag.Message = "";
            return View("Index", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ExamDateTimeSlot examDateTime)
        {


            try
            {
                GetExamDateTimeSlot_By_ExamIDExamDateID_Result ts = db.GetExamDateTimeSlot_By_ExamIDExamDateID(examDateTime.ExamID).FirstOrDefault(d => d.TimeSlot == examDateTime.TimeSlot);
                if (ts == null)
                {

                    examDateTime.TimeSlot = examDateTime.StartTime + "-" + examDateTime.EndTime;
                    db.Entry(examDateTime).State = EntityState.Modified;
                    examDateTime.ModifiedBy = Convert.ToInt32(Session["emp_id"]);
                    examDateTime.ModifiedOn = DateTime.Now;
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


            model.GetExamDateTimeSlot_By_ExamIDExamDateID_Results = db.GetExamDateTimeSlot_By_ExamIDExamDateID(examDateTime.ExamID).ToList();
            model.SelectedGetExamDateTimeSlot_By_ExamIDExamDateID_Result = db.GetExamDateTimeSlot_By_ExamIDExamDateID(examDateTime.ExamID).FirstOrDefault(s => s.ExamDateTimeSlotID == examDateTime.ExamDateTimeSlotID);
        
            model.DisplayMode = "ReadWrite";
            ViewBag.hdnExamID = examDateTime.ExamID;
          
            ViewBag.ExamID = new SelectList(db.Exams, "ExamID", "ExamTitle");
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc", examDateTime.IsActive);

            return View("Index", model);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ExamDateTimeSlot examDateTime = db.ExamDateTimeSlots.Find(id);
            if (examDateTime == null)
            {
                return HttpNotFound();
            }

            model.GetExamDateTimeSlot_By_ExamIDExamDateID_Results = db.GetExamDateTimeSlot_By_ExamIDExamDateID(examDateTime.ExamID).ToList();

            model.SelectedGetExamDateTimeSlot_By_ExamIDExamDateID_Result = db.GetExamDateTimeSlot_By_ExamIDExamDateID(examDateTime.ExamID).FirstOrDefault(s => s.ExamDateTimeSlotID == examDateTime.ExamDateTimeSlotID);
            model.DisplayMode = "Delete";
            ViewBag.MessageType = "";
            ViewBag.Message = "";
            return View("Index", model);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ExamDateTimeSlot examDateTime = db.ExamDateTimeSlots.Find(id);
            try
            {
                db.ExamDateTimeSlots.Remove(examDateTime);
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

            model.GetExamDateTimeSlot_By_ExamIDExamDateID_Results = db.GetExamDateTimeSlot_By_ExamIDExamDateID(examDateTime.ExamID).ToList();

            model.DisplayMode = "WriteOnly";
            ViewBag.ExamID = new SelectList(db.Exams, "ExamID", "ExamTitle");
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