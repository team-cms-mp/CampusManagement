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
    public class TimeTableSlotsDatesController : Controller
    {
        private ModelCMSNewContainer db = new ModelCMSNewContainer();
        GetTimeTableSlotDateData_ResultViewModel model = new GetTimeTableSlotDateData_ResultViewModel();

        public ActionResult Index()
        {
            model.GetTimeTableSlotDateData_Results = db.GetTimeTableSlotDateData().ToList();
            model.SelectedGetTimeTableSlotDateData_Result = null;
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc");
            model.DisplayMode = "WriteOnly";
            ViewBag.MessageType = "";
            ViewBag.Message = "";
            return View(model);
        }

        [HttpGet]
        public ActionResult Create()
        {
            model.GetTimeTableSlotDateData_Results = db.GetTimeTableSlotDateData().ToList();
            model.SelectedGetTimeTableSlotDateData_Result = null;
            model.DisplayMode = "WriteOnly";
          
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc");
            ViewBag.MessageType = "";
            ViewBag.Message = "";
            return View("Index", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create( string StartDate, string EndDate)
        {
          
            try
            {
                DateTime sd = Convert.ToDateTime(StartDate);
                DateTime ed = Convert.ToDateTime(EndDate);
                int empid = Convert.ToInt32(Session["emp_id"]);
                 db.Insert_TimeTableSlotDate(sd, ed, empid);
                
               
            }
            catch (Exception ex)
            {
              
                ViewBag.MessageType = "error";
                ViewBag.Message = ex.Message;
            }

            model.GetTimeTableSlotDateData_Results = db.GetTimeTableSlotDateData().ToList();
            model.SelectedGetTimeTableSlotDateData_Result = null;
            model.DisplayMode = "WriteOnly";
          
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc");

            return View("Index", model);
        }

        public ActionResult MarkAsActiveAndDeActive(int? TimeSlotID, int TimeTableDateID, string IsActive)
        {
            try
            {

                if (TimeSlotID == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                TimeTableSlotDate ttd = db.TimeTableSlotDates.FirstOrDefault(a => a.TimeTableDateID == TimeTableDateID && a.TimeTableSlotID == TimeSlotID);

                if (ttd == null)
                {
                    return HttpNotFound();
                }


                db.Entry(ttd).State = EntityState.Modified;
                ttd.ModifiedBy = Convert.ToInt32(Session["emp_id"]);
                ttd.ModifiedOn = DateTime.Now;
                ttd.IsActive = IsActive;
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
            model.GetTimeTableSlotDateData_Results = db.GetTimeTableSlotDateData().ToList();
            model.SelectedGetTimeTableSlotDateData_Result = null;
            model.DisplayMode = "WriteOnly";

            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc");
            return RedirectToAction ("Index");
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