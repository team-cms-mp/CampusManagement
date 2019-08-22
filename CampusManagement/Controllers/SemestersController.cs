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
    public class SemestersController : Controller
    {
        private ModelCMSNewContainer db = new ModelCMSNewContainer();
        SemestersViewModel model = new SemestersViewModel();

        public ActionResult Index()
        {
            model.Semesters = db.Semesters.OrderByDescending(a =>a.SemesterID).ToList();
            model.SelectedSemester = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc");
            ViewBag.MessageType = "";
            ViewBag.Message = "";
            return View(model);
        }

        [HttpGet]
        public ActionResult Create()
        {
            model.Semesters = db.Semesters.OrderByDescending(a =>a.SemesterID).ToList();
            model.SelectedSemester = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc");
            ViewBag.MessageType = "";
            ViewBag.Message = "";
            return View("Index", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Semester semester)
        {
            string ErrorMessage = "";
            int count = 0;
            Semester s = new Semester();
            try
            {
                s = db.Semesters.FirstOrDefault(de => de.SemesterCode == semester.SemesterCode);
                if(s != null)
                {
                    ModelState.AddModelError(string.Empty, "Semester Code already exists.");
                    count++;
                    ErrorMessage += count + "-Semester Code already exists.<br />";
                }

                s = db.Semesters.FirstOrDefault(de => de.SemesterName == semester.SemesterName);
                if (s != null)
                {
                    ModelState.AddModelError(string.Empty, "Semester Name already exists.");
                    count++;
                    ErrorMessage += count + "-Semester Name already exists.<br />";
                }

                s = db.Semesters.FirstOrDefault(de => de.YearSemesterNo == semester.YearSemesterNo);
                if (s != null)
                {
                    ModelState.AddModelError(string.Empty, "Semester # already exists.");
                    count++;
                    ErrorMessage += count + "-Semester # already exists.<br />";
                }

                if(!string.IsNullOrEmpty(ErrorMessage))
                {
                    ViewBag.MessageType = "error";
                    ViewBag.Message = ErrorMessage;
                }

                if (s == null)
                {
                    semester.CreatedBy = Convert.ToInt32(Session["emp_id"]);
                    semester.CreatedOn = DateTime.Now;
                    db.Semesters.Add(semester);
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
            model.Semesters = db.Semesters.OrderByDescending(a =>a.SemesterID).ToList();
            model.SelectedSemester = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc", semester.IsActive);

            return View("Index", model);
        }

        public ActionResult Update(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Semester semester = db.Semesters.Find(id);
            if (semester == null)
            {
                return HttpNotFound();
            }

            model.Semesters = db.Semesters.OrderByDescending(a =>a.SemesterID).ToList();
            model.SelectedSemester = semester;
            model.DisplayMode = "ReadWrite";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc", semester.IsActive);
            ViewBag.MessageType = "";
            ViewBag.Message = "";
            return View("Index", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Semester semester)
        {
            try
            {
                db.Entry(semester).State = EntityState.Modified;
                semester.ModifiedBy = Convert.ToInt32(Session["emp_id"]);
                semester.ModifiedOn = DateTime.Now;
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
            model.Semesters = db.Semesters.OrderByDescending(a =>a.SemesterID).ToList();
            model.SelectedSemester = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc", semester.IsActive);

            return View("Index", model);
        }

        public ActionResult Delete(int? id)
        {
            Semester semester = new Semester();
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                semester = db.Semesters.Find(id);
                if (semester == null)
                {
                    return HttpNotFound();
                }
            }
            catch (Exception ex)
            {
                ViewBag.MessageType = "error";
                ViewBag.Message = ex.Message;
            }

            model.Semesters = db.Semesters.OrderByDescending(a =>a.SemesterID).ToList();
            model.SelectedSemester = semester;
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
                Semester semester = db.Semesters.Find(id);
                db.Semesters.Remove(semester);
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
            model.Semesters = db.Semesters.OrderByDescending(a =>a.SemesterID).ToList();
            model.SelectedSemester = null;
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
