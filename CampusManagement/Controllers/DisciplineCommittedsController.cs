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
    public class DisciplineCommittedsController : Controller
    {
        private ModelCMSNewContainer db = new ModelCMSNewContainer();
        DisciplineCommittedViewModel model = new DisciplineCommittedViewModel();

        public ActionResult Index()
        {
            model.DisciplineCommitteds = db.DisciplineCommitteds.OrderByDescending(a=>a.DisciplineCommittedID).ToList();
            model.SelectedDisciplineCommitted = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc");
            ViewBag.MessageType = "";
            ViewBag.Message = "";
            return View(model);
        }

        [HttpGet]
        public ActionResult Create()
        {
            model.DisciplineCommitteds = db.DisciplineCommitteds.OrderByDescending(a=>a.DisciplineCommittedID).ToList();
            model.SelectedDisciplineCommitted = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc");
            ViewBag.MessageType = "";
            ViewBag.Message = "";
            return View("Index", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(DisciplineCommitted disciplineCommitted)
        {
            try
            {
                DisciplineCommitted d = db.DisciplineCommitteds.FirstOrDefault(de => de.DisciplineCommittedName == disciplineCommitted.DisciplineCommittedName);
                if (d == null)
                {
                    disciplineCommitted.CreatedBy = Convert.ToInt32(Session["emp_id"]);
                    disciplineCommitted.CreatedOn = DateTime.Now;
                    db.DisciplineCommitteds.Add(disciplineCommitted);
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
                    ModelState.AddModelError(string.Empty, "Discipline Committed Name already exists.");
                    ViewBag.MessageType = "error";
                    ViewBag.Message = "Discipline Committed Name already exists.";
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
            model.DisciplineCommitteds = db.DisciplineCommitteds.OrderByDescending(a=>a.DisciplineCommittedID).ToList();
            model.SelectedDisciplineCommitted = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc", disciplineCommitted.IsActive);

            return View("Index", model);
        }

        public ActionResult Update(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            DisciplineCommitted disciplineCommitted = db.DisciplineCommitteds.Find(id);
            if (disciplineCommitted == null)
            {
                return HttpNotFound();
            }

            model.DisciplineCommitteds = db.DisciplineCommitteds.OrderByDescending(a=>a.DisciplineCommittedID).ToList();
            model.SelectedDisciplineCommitted = disciplineCommitted;
            model.DisplayMode = "ReadWrite";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc", disciplineCommitted.IsActive);
            ViewBag.MessageType = "";
            ViewBag.Message = "";
            return View("Index", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(DisciplineCommitted disciplineCommitted)
        {
            try
            {
                db.Entry(disciplineCommitted).State = EntityState.Modified;
                disciplineCommitted.ModifiedBy = Convert.ToInt32(Session["emp_id"]);
                disciplineCommitted.ModifiedOn = DateTime.Now;
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
            model.DisciplineCommitteds = db.DisciplineCommitteds.OrderByDescending(a=>a.DisciplineCommittedID).ToList();
            model.SelectedDisciplineCommitted = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc", disciplineCommitted.IsActive);

            return View("Index", model);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DisciplineCommitted disciplineCommitted = db.DisciplineCommitteds.Find(id);
            if (disciplineCommitted == null)
            {
                return HttpNotFound();
            }

            model.DisciplineCommitteds = db.DisciplineCommitteds.OrderByDescending(a=>a.DisciplineCommittedID).ToList();
            model.SelectedDisciplineCommitted = disciplineCommitted;
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
                DisciplineCommitted disciplineCommitted = db.DisciplineCommitteds.Find(id);
                db.DisciplineCommitteds.Remove(disciplineCommitted);
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
            model.DisciplineCommitteds = db.DisciplineCommitteds.OrderByDescending(a=>a.DisciplineCommittedID).ToList();
            model.SelectedDisciplineCommitted = null;
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
