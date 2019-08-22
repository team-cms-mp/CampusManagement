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
    public class TimeTableDatesController : Controller
    {
        private ModelCMSNewContainer db = new ModelCMSNewContainer();
        TimeTableDateViewModel model = new TimeTableDateViewModel();

        public ActionResult Index()
        {
            model.TimeTableDates = db.TimeTableDates.OrderBy(t => t.TimeTableDateName).ToList();
            model.SelectedTimeTableDate = null;
            ViewBag.YearNo = new SelectList(db.TimeTableYears, "YearNo", "YearNo");
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc");

            ViewBag.MessageType = "";
            ViewBag.Message = "";
            return View(model);
        }

        [HttpGet]
        public ActionResult Create()
        {
            model.TimeTableDates = db.TimeTableDates.OrderBy(t => t.TimeTableDateName).ToList();
            model.SelectedTimeTableDate = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.YearNo = new SelectList(db.TimeTableYears, "YearNo", "YearNo");
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc");
            ViewBag.MessageType = "";
            ViewBag.Message = "";
            return View("Index", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(string YearNo)
        {
            try
            {
                TimeTableDate td = db.TimeTableDates.FirstOrDefault(d => d.TimeTableDateName.Equals(YearNo));
                if (td == null)
                {
                    try
                    {
                        int YN = Convert.ToInt32(YearNo);
                        db.InsertDatesOfYear(YN);

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
                    ViewBag.Message = "Year dates already exist.";
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
                        count++;
                        ErrorMessage += count + "-" + string.Concat(error.PropertyName, " is required.") + "<br />";
                    }
                }
                ViewBag.MessageType = "error";
                ViewBag.Message = ErrorMessage;
            }

            model.TimeTableDates = db.TimeTableDates.OrderBy(t => t.TimeTableDateName).ToList();
            model.SelectedTimeTableDate = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.YearNo = new SelectList(db.TimeTableYears, "YearNo", "YearNo");
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc");

            return View("Index", model);
        }

        public ActionResult MarkAsHoliday(int? id, string IsActive)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                TimeTableDate ttd = db.TimeTableDates.Find(id);

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
            model.TimeTableDates = db.TimeTableDates.OrderBy(t => t.TimeTableDateName).ToList();
            model.SelectedTimeTableDate = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.YearNo = new SelectList(db.TimeTableYears, "YearNo", "YearNo");
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