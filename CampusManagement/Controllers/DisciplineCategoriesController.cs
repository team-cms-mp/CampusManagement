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
    public class DisciplineCategoriesController : Controller
    {
        private ModelCMSNewContainer db = new ModelCMSNewContainer();
        DisciplineCategoryViewModel model = new DisciplineCategoryViewModel();

        public ActionResult Index()
        {
            model.DisciplineCategories = db.DisciplineCategories.OrderByDescending(a => a.DisciplineCategoryID).ToList();
            model.SelectedDisciplineCategory = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc");
            ViewBag.DisciplineTypeID = new SelectList(db.DisciplineTypes, "DisciplineTypeID", "DisciplineTypeName");
            ViewBag.DisciplineSeverityLevelID = new SelectList(db.DisciplineSeverityLevels, "DisciplineSeverityLevelID", "DisciplineSeverityLevelName");
            ViewBag.MessageType = "";
            ViewBag.Message = "";
            return View(model);
        }


        [HttpGet]
        public ActionResult Create()
        {
            model.DisciplineCategories = db.DisciplineCategories.OrderByDescending(a => a.DisciplineCategoryID).ToList();
            model.SelectedDisciplineCategory = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc");
            ViewBag.DisciplineTypeID = new SelectList(db.DisciplineTypes, "DisciplineTypeID", "DisciplineTypeName");
            ViewBag.DisciplineSeverityLevelID = new SelectList(db.DisciplineSeverityLevels, "DisciplineSeverityLevelID", "DisciplineSeverityLevelName");
            ViewBag.MessageType = "";
            ViewBag.Message = "";
            return View("Index", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(DisciplineCategory disciplineCategory)
        {
            try
            {
                DisciplineCategory d = db.DisciplineCategories.FirstOrDefault(de => de.DisciplineCategoryName == disciplineCategory.DisciplineCategoryName);
                if (d == null)
                {
                    disciplineCategory.CreatedBy = Convert.ToInt32(Session["emp_id"]);
                    disciplineCategory.CreatedOn = DateTime.Now;
                    db.DisciplineCategories.Add(disciplineCategory);
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
                    ModelState.AddModelError(string.Empty, "Discipline Category already exists.");
                    ViewBag.MessageType = "error";
                    ViewBag.Message = "Discipline Category already exists.";
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
            model.DisciplineCategories = db.DisciplineCategories.OrderByDescending(a => a.DisciplineCategoryID).ToList();
            model.SelectedDisciplineCategory = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc", disciplineCategory.IsActive);
            ViewBag.DisciplineTypeID = new SelectList(db.DisciplineTypes, "DisciplineTypeID", "DisciplineTypeName", disciplineCategory.DisciplineTypeID);
            ViewBag.DisciplineSeverityLevelID = new SelectList(db.DisciplineSeverityLevels, "DisciplineSeverityLevelID", "DisciplineSeverityLevelName", disciplineCategory.DisciplineSeverityLevelID);

            return View("Index", model);
        }

        public ActionResult Update(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            DisciplineCategory disciplineCategory = db.DisciplineCategories.Find(id);
            if (disciplineCategory == null)
            {
                return HttpNotFound();
            }

            model.DisciplineCategories = db.DisciplineCategories.OrderByDescending(a => a.DisciplineCategoryID).ToList();
            model.SelectedDisciplineCategory = disciplineCategory;
            model.DisplayMode = "ReadWrite";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc", disciplineCategory.IsActive);
            ViewBag.DisciplineTypeID = new SelectList(db.DisciplineTypes, "DisciplineTypeID", "DisciplineTypeName", disciplineCategory.DisciplineTypeID);
            ViewBag.DisciplineSeverityLevelID = new SelectList(db.DisciplineSeverityLevels, "DisciplineSeverityLevelID", "DisciplineSeverityLevelName", disciplineCategory.DisciplineSeverityLevelID);
            ViewBag.MessageType = "";
            ViewBag.Message = "";
            return View("Index", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(DisciplineCategory disciplineCategory)
        {
            try
            {
                db.Entry(disciplineCategory).State = EntityState.Modified;
                disciplineCategory.ModifiedBy = Convert.ToInt32(Session["emp_id"]);
                disciplineCategory.ModifiedOn = DateTime.Now;
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
            model.DisciplineCategories = db.DisciplineCategories.OrderByDescending(a => a.DisciplineCategoryID).ToList();
            model.SelectedDisciplineCategory = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc", disciplineCategory.IsActive);
            ViewBag.DisciplineTypeID = new SelectList(db.DisciplineTypes, "DisciplineTypeID", "DisciplineTypeName", disciplineCategory.DisciplineTypeID);
            ViewBag.DisciplineSeverityLevelID = new SelectList(db.DisciplineSeverityLevels, "DisciplineSeverityLevelID", "DisciplineSeverityLevelName", disciplineCategory.DisciplineSeverityLevelID);

            return View("Index", model);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DisciplineCategory disciplineCategory = db.DisciplineCategories.Find(id);
            if (disciplineCategory == null)
            {
                return HttpNotFound();
            }

            model.DisciplineCategories = db.DisciplineCategories.OrderByDescending(a => a.DisciplineCategoryID).ToList();
            model.SelectedDisciplineCategory = disciplineCategory;
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
                DisciplineCategory disciplineCategory = db.DisciplineCategories.Find(id);
                db.DisciplineCategories.Remove(disciplineCategory);
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
            model.DisciplineCategories = db.DisciplineCategories.OrderByDescending(a => a.DisciplineCategoryID).ToList();
            model.SelectedDisciplineCategory = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc");
            ViewBag.DisciplineTypeID = new SelectList(db.DisciplineTypes, "DisciplineTypeID", "DisciplineTypeName");
            ViewBag.DisciplineSeverityLevelID = new SelectList(db.DisciplineSeverityLevels, "DisciplineSeverityLevelID", "DisciplineSeverityLevelName");

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
