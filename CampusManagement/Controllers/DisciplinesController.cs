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
using Newtonsoft.Json;

namespace CampusManagement.Controllers
{
    [Authorize]
    public class DisciplinesController : Controller
    {
        private ModelCMSNewContainer db = new ModelCMSNewContainer();
        DisciplineViewModel model = new DisciplineViewModel();

        public ActionResult Index()
        {
            model.Disciplines = db.Disciplines.OrderByDescending(a=>a.DisciplineID).ToList();
            model.SelectedDiscipline = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc");
            
            ViewBag.DisciplineCategoryID = new SelectList(db.DisciplineCategories, "DisciplineCategoryID", "DisciplineCategoryName");
            ViewBag.DisciplineCommittedID = new SelectList(db.DisciplineCommitteds, "DisciplineCommittedID", "DisciplineCommittedName");
            ViewBag.StatusID = new SelectList(db.Status.Where(s => s.StatusType == "discipline"), "StatusID", "StatusName");
            ViewBag.MessageType = "";
            ViewBag.Message = "";
            return View(model);
        }

        public ActionResult DisciplineType()
        {
           return View();
        }
        public ActionResult ListOfDisciplines()
        {
            model.Disciplines = db.Disciplines.OrderByDescending(a => a.DisciplineID).ToList();
            model.SelectedDiscipline = null;
            model.DisplayMode = "WriteOnly";
            return View(model);
        }

        [HttpPost]
        public ActionResult ListOfDisciplines(string Search)
        {
            model.Disciplines = db.Disciplines.Where(x => x.DisciplineDate.Equals(Search)).OrderByDescending(a => a.DisciplineID).ToList();
            return View("ListOfDisciplines", model);
        }

        [HttpGet]
        public ActionResult Create()
        {
            model.Disciplines = db.Disciplines.OrderByDescending(a=>a.DisciplineID).ToList();
            model.SelectedDiscipline = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc");
            
            ViewBag.DisciplineCategoryID = new SelectList(db.DisciplineCategories, "DisciplineCategoryID", "DisciplineCategoryName");
            ViewBag.DisciplineCommittedID = new SelectList(db.DisciplineCommitteds, "DisciplineCommittedID", "DisciplineCommittedName");
            ViewBag.StatusID = new SelectList(db.Status.Where(s => s.StatusType == "discipline"), "StatusID", "StatusName");
            ViewBag.MessageType = "";
            ViewBag.Message = "";
            return View("Index", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Discipline discipline)
        {
            try
            {
                Discipline d = db.Disciplines.FirstOrDefault(de => de.DisciplineDate == discipline.DisciplineDate);
                if (d == null)
                {
                    discipline.CreatedBy = Convert.ToInt32(Session["emp_id"]);
                    discipline.CreatedOn = DateTime.Now;
                    db.Disciplines.Add(discipline);
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
                    ModelState.AddModelError(string.Empty, "Discipline Name already exists.");
                    ViewBag.MessageType = "error";
                    ViewBag.Message = "Discipline Name already exists.";
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
            model.Disciplines = db.Disciplines.OrderByDescending(a=>a.DisciplineID).ToList();
            model.SelectedDiscipline = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc");
            
            ViewBag.DisciplineCategoryID = new SelectList(db.DisciplineCategories, "DisciplineCategoryID", "DisciplineCategoryName", discipline.DisciplineCategoryID);
            ViewBag.DisciplineCommittedID = new SelectList(db.DisciplineCommitteds, "DisciplineCommittedID", "DisciplineCommittedName", discipline.DisciplineCommittedID);
            ViewBag.StatusID = new SelectList(db.Status.Where(s => s.StatusType == "discipline"), "StatusID", "StatusName", discipline.StatusID);

            return RedirectToAction("Index", model);
        }

        public ActionResult Update(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Discipline discipline = db.Disciplines.Find(id);
            if (discipline == null)
            {
                return HttpNotFound();
            }

            model.Disciplines = db.Disciplines.OrderByDescending(a=>a.DisciplineID).ToList();
            model.SelectedDiscipline = discipline;
            model.DisplayMode = "ReadWrite";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc");
            
            ViewBag.DisciplineCategoryID = new SelectList(db.DisciplineCategories, "DisciplineCategoryID", "DisciplineCategoryName", discipline.DisciplineCategoryID);
            ViewBag.DisciplineCommittedID = new SelectList(db.DisciplineCommitteds, "DisciplineCommittedID", "DisciplineCommittedName", discipline.DisciplineCommittedID);
            ViewBag.StatusID = new SelectList(db.Status.Where(s => s.StatusType == "discipline"), "StatusID", "StatusName", discipline.StatusID);
            ViewBag.MessageType = "";
            ViewBag.Message = "";
            return View("Index", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Discipline discipline)
        {
            try
            {
                db.Entry(discipline).State = EntityState.Modified;
                discipline.ModifiedBy = Convert.ToInt32(Session["emp_id"]);
                discipline.ModifiedOn = DateTime.Now;
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
            model.Disciplines = db.Disciplines.OrderByDescending(a=>a.DisciplineID).ToList();
            model.SelectedDiscipline = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc");
            
            ViewBag.DisciplineCategoryID = new SelectList(db.DisciplineCategories, "DisciplineCategoryID", "DisciplineCategoryName", discipline.DisciplineCategoryID);
            ViewBag.DisciplineCommittedID = new SelectList(db.DisciplineCommitteds, "DisciplineCommittedID", "DisciplineCommittedName", discipline.DisciplineCommittedID);
            ViewBag.StatusID = new SelectList(db.Status.Where(s => s.StatusType == "discipline"), "StatusID", "StatusName", discipline.StatusID);
            return RedirectToAction("Index", model);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Discipline discipline = db.Disciplines.Find(id);
            if (discipline == null)
            {
                return HttpNotFound();
            }

            model.Disciplines = db.Disciplines.OrderByDescending(a=>a.DisciplineID).ToList();
            model.SelectedDiscipline = discipline;
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
                Discipline discipline = db.Disciplines.Find(id);
                db.Disciplines.Remove(discipline);
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
            model.Disciplines = db.Disciplines.OrderByDescending(a=>a.DisciplineID).ToList();
            model.SelectedDiscipline = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc");
            
            ViewBag.DisciplineCategoryID = new SelectList(db.DisciplineCategories, "DisciplineCategoryID", "DisciplineCategoryName");
            ViewBag.DisciplineCommittedID = new SelectList(db.DisciplineCommitteds, "DisciplineCommittedID", "DisciplineCommittedName");
            ViewBag.StatusID = new SelectList(db.Status.Where(s => s.StatusType == "discipline"), "StatusID", "StatusName");

            return View("Index", model);
        }
        public JsonResult GetStudentsList(string searchValue)
        {
            List<Student> lstStudent = new List<Student>();

            lstStudent = db.Students.Where(a => a.FirstName.Contains(searchValue)
            || a.LastName.Contains(searchValue)
            || a.FormNo.Contains(searchValue)
            || a.ACNIC.Contains(searchValue)).ToList();

            var Students = lstStudent.Select(s => new
            {
                AddAppID = s.StudentID,
                FirstName = s.FirstName,
                LastName = s.LastName,
                FormNo = s.FormNo,
                ACNIC = s.ACNIC
            });

            string result = JsonConvert.SerializeObject(Students, Formatting.Indented);
            return new JsonResult { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
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
