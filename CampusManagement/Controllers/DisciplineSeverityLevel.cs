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
    public class DisciplineSeverityLevelController : Controller
    {
        private ModelCMSNewContainer db = new ModelCMSNewContainer();
        DisciplineSeverityLevelViewModel model = new DisciplineSeverityLevelViewModel();

        public ActionResult Index()
        {
            model.DisciplineSeverityLevels = db.DisciplineSeverityLevels.OrderByDescending(a=>a.DisciplineSeverityLevelID).ToList();
            model.SelectedDisciplineSeverityLevels = null; 
            model.DisplayMode = "WriteOnly";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc");
            ViewBag.MessageType = "";
            ViewBag.Message = "";
            return View(model);
        }

        [HttpGet]
        public ActionResult Create()
        {
            model.DisciplineSeverityLevels = db.DisciplineSeverityLevels.OrderByDescending(a=>a.DisciplineSeverityLevelID).ToList();
            model.SelectedDisciplineSeverityLevels = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc");
            ViewBag.MessageType = "";
            ViewBag.Message = "";
            return View("Index", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(DisciplineSeverityLevel disciplineSeverityLevel)
        {
            try
            {
                DisciplineSeverityLevel d = db.DisciplineSeverityLevels.FirstOrDefault(de => de.DisciplineSeverityLevelName == disciplineSeverityLevel.DisciplineSeverityLevelName);
                if (d == null)
                {
                    disciplineSeverityLevel.CreatedBy = Convert.ToInt32(Session["emp_id"]);
                    disciplineSeverityLevel.CreatedOn = DateTime.Now;
                    db.DisciplineSeverityLevels.Add(disciplineSeverityLevel);
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
                    ModelState.AddModelError(string.Empty, "Deposit Type already exists.");
                    ViewBag.MessageType = "error";
                    ViewBag.Message = "Deposit Type already exists.";
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
            model.DisciplineSeverityLevels = db.DisciplineSeverityLevels.OrderByDescending(a=>a.DisciplineSeverityLevelID).ToList();
            model.SelectedDisciplineSeverityLevels = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc", disciplineSeverityLevel.IsActive);

            return View("Index", model);
        }

        public ActionResult Update(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            DisciplineSeverityLevel disciplineSeverityLevel = db.DisciplineSeverityLevels.Find(id);
            if (disciplineSeverityLevel == null)
            {
                return HttpNotFound();
            }

            model.DisciplineSeverityLevels = db.DisciplineSeverityLevels.OrderByDescending(a=>a.DisciplineSeverityLevelID).ToList();
            model.SelectedDisciplineSeverityLevels = disciplineSeverityLevel;
            model.DisplayMode = "ReadWrite";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc", disciplineSeverityLevel.IsActive);
            ViewBag.MessageType = "";
            ViewBag.Message = "";
            return View("Index", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(DisciplineSeverityLevel disciplineSeverityLevel)
        {
            try
            {
                db.Entry(disciplineSeverityLevel).State = EntityState.Modified;
                disciplineSeverityLevel.ModifiedBy = Convert.ToInt32(Session["emp_id"]);
                disciplineSeverityLevel.ModifiedOn = DateTime.Now;
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
            model.DisciplineSeverityLevels = db.DisciplineSeverityLevels.OrderByDescending(a=>a.DisciplineSeverityLevelID).ToList();
            model.SelectedDisciplineSeverityLevels = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc", disciplineSeverityLevel.IsActive);

            return View("Index", model);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
      
            DisciplineSeverityLevel disciplineSeverityLevel = db.DisciplineSeverityLevels.Find(id);
            if (disciplineSeverityLevel == null)
            {
                return HttpNotFound();
            }

            model.DisciplineSeverityLevels = db.DisciplineSeverityLevels.OrderByDescending(a=>a.DisciplineSeverityLevelID).ToList();
            model.SelectedDisciplineSeverityLevels = disciplineSeverityLevel;
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
                DisciplineSeverityLevel disciplineSeverityLevel = db.DisciplineSeverityLevels.Find(id);
                db.DisciplineSeverityLevels.Remove(disciplineSeverityLevel);
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
            model.DisciplineSeverityLevels = db.DisciplineSeverityLevels.OrderByDescending(a=>a.DisciplineSeverityLevelID).ToList();
            model.SelectedDisciplineSeverityLevels = null;
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
