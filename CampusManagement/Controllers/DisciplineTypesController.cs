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
    public class DisciplineTypesController : Controller
    {
        private ModelCMSNewContainer db = new ModelCMSNewContainer();
        DisciplineTypeViewModel model = new DisciplineTypeViewModel();

        public ActionResult Index()
        {
            model.DisciplineTypes = db.DisciplineTypes.OrderByDescending(a=>a.DisciplineTypeID).ToList();
            model.SelectedDisciplineType = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc");
            ViewBag.MessageType = "";
            ViewBag.Message = "";
            return View(model);
        }

        [HttpGet]
        public ActionResult Create()
        {
            model.DisciplineTypes = db.DisciplineTypes.OrderByDescending(a => a.DisciplineTypeID).ToList();
            model.SelectedDisciplineType = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc");
            ViewBag.MessageType = "";
            ViewBag.Message = "";
            return View("Index", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(DisciplineType disciplineType)
        {
            try
            {
                DisciplineType d = db.DisciplineTypes.FirstOrDefault(de => de.DisciplineTypeName == disciplineType.DisciplineTypeName);
                if (d == null)
                {
                    disciplineType.CreatedBy = Convert.ToInt32(Session["emp_id"]);
                    disciplineType.CreatedOn = DateTime.Now;
                    db.DisciplineTypes.Add(disciplineType);
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
                    ModelState.AddModelError(string.Empty, "Discipline Type Name already exists.");
                    ViewBag.MessageType = "error";
                    ViewBag.Message = "Discipline Type Name already exists.";
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
            model.DisciplineTypes = db.DisciplineTypes.OrderByDescending(a=>a.DisciplineTypeID).ToList();
            model.SelectedDisciplineType = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc", disciplineType.IsActive);

            return View("Index", model);
        }

        public ActionResult Update(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            DisciplineType disciplineTypes = db.DisciplineTypes.Find(id);
            if (disciplineTypes == null)
            {
                return HttpNotFound();
            }

            model.DisciplineTypes = db.DisciplineTypes.OrderByDescending(a=>a.DisciplineTypeID).ToList();
            model.SelectedDisciplineType = disciplineTypes;
            model.DisplayMode = "ReadWrite";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc", disciplineTypes.IsActive);
            ViewBag.MessageType = "";
            ViewBag.Message = "";
            return View("Index", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(DisciplineType disciplineType)
        {
            try
            {
                db.Entry(disciplineType).State = EntityState.Modified;
                disciplineType.ModifiedBy = Convert.ToInt32(Session["emp_id"]);
                disciplineType.ModifiedOn = DateTime.Now;
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
            model.DisciplineTypes = db.DisciplineTypes.OrderByDescending(a=>a.DisciplineTypeID).ToList();
            model.SelectedDisciplineType = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc", disciplineType.IsActive);

            return View("Index", model);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DisciplineType disciplineType = db.DisciplineTypes.Find(id);
            if (disciplineType == null)
            {
                return HttpNotFound();
            }

            model.DisciplineTypes = db.DisciplineTypes.OrderByDescending(a=>a.DisciplineTypeID).ToList();
            model.SelectedDisciplineType = disciplineType;
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
                DisciplineType disciplineTypes = db.DisciplineTypes.Find(id);
                db.DisciplineTypes.Remove(disciplineTypes);
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
            model.DisciplineTypes = db.DisciplineTypes.OrderByDescending(a => a.DisciplineTypeID).ToList();
            model.SelectedDisciplineType = null;
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
