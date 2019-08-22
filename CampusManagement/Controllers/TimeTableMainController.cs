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
    public class TimeTableMainController : Controller
    {
        private ModelCMSNewContainer db = new ModelCMSNewContainer();
        TimeTableMainViewModel model = new TimeTableMainViewModel();

        public ActionResult Index()
        {
            model.TimeTableMains = db.TimeTableMains.OrderByDescending(a => a.TimeTableMainID).ToList();
            model.SelectedTimeTableMain = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc");
            ViewBag.MessageType = "";
            ViewBag.Message = "";
            return View(model);
        }

        [HttpGet]
        public ActionResult Create()
        {
            model.TimeTableMains = db.TimeTableMains.OrderByDescending(a => a.TimeTableMainID).ToList();
            model.SelectedTimeTableMain = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc");
            ViewBag.MessageType = "";
            ViewBag.Message = "";
            return View("Index", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(TimeTableMain timeTableMain)
        {
            try
            {
                TimeTableMain ba = db.TimeTableMains.FirstOrDefault(bac => bac.TimeTableMainID == timeTableMain.TimeTableMainID);
                if (ba == null)
                {
                    timeTableMain.CreatedBy = Convert.ToInt32(Session["emp_id"]);
                    timeTableMain.CreatedOn = DateTime.Now;
                    timeTableMain.StatusID = 1007;
                    timeTableMain.Description = timeTableMain.Description;
                    db.TimeTableMains.Add(timeTableMain);
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
                    ModelState.AddModelError(string.Empty, "Time Table Name # already exists.");
                    ViewBag.MessageType = "error";
                    ViewBag.Message = "Time Table Name # already exists.";
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
            model.TimeTableMains = db.TimeTableMains.OrderByDescending(a => a.TimeTableMainID).ToList();
            model.SelectedTimeTableMain = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc", timeTableMain.IsActive);

            return View("Index", model);
        }

        public ActionResult Update(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            TimeTableMain timeTableMain = db.TimeTableMains.Find(id);
            if (timeTableMain == null)
            {
                return HttpNotFound();
            }

            model.TimeTableMains = db.TimeTableMains.OrderByDescending(a => a.TimeTableMainID).ToList();
            model.SelectedTimeTableMain = timeTableMain;
            model.DisplayMode = "ReadWrite";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc", timeTableMain.IsActive);
            ViewBag.MessageType = "";
            ViewBag.Message = "";
            return View("Index", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(TimeTableMain timeTableMain)
        {
            try
            {
                db.Entry(timeTableMain).State = EntityState.Modified;
                timeTableMain.ModifiedBy = Convert.ToInt32(Session["emp_id"]);
                timeTableMain.ModifiedOn = DateTime.Now;
                timeTableMain.StatusID = 1007;
                timeTableMain.Description = timeTableMain.Description;
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
            model.TimeTableMains = db.TimeTableMains.OrderByDescending(a => a.TimeTableMainID).ToList();
            model.SelectedTimeTableMain = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc", timeTableMain.IsActive);

            return View("Index", model);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TimeTableMain timeTableMain = db.TimeTableMains.Find(id);
            if (timeTableMain == null)
            {
                return HttpNotFound();
            }

            model.TimeTableMains = db.TimeTableMains.OrderByDescending(a => a.TimeTableMainID).ToList();
            model.SelectedTimeTableMain = timeTableMain;
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
                TimeTableMain timeTableMain = db.TimeTableMains.Find(id);
                db.TimeTableMains.Remove(timeTableMain);
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
            model.TimeTableMains = db.TimeTableMains.OrderByDescending(a => a.TimeTableMainID).ToList();
            model.SelectedTimeTableMain = null;
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
