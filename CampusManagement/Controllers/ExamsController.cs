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
    public class ExamsController : Controller
    {
        private ModelCMSNewContainer db = new ModelCMSNewContainer();
        ExamsViewModel model = new ExamsViewModel();

        public ActionResult Exam()
        {
            return View();
        }

        public ActionResult Index(int? ExamID)
        {
            model.Exams = db.Exams.OrderByDescending(a=>a.ExamID).ToList();
            model.SelectedExam = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc");
            ViewBag.ExamTermID = new SelectList(db.ExamTerms, "ExamTermID", "TermName");
            ViewBag.ExamSeasonID = new SelectList(db.ExamSeasons, "ExamSeasonID", "SeasonName");
            ViewBag.MessageType = "";
            ViewBag.Message = "";
            return View(model);
        }
        public ActionResult MarkAsActive(int ExamID)
        {
            int EmpID = Convert.ToInt32(Session["emp_id"]);
            db.UpdateActiveExam(ExamID, EmpID);
            // Update Request For Approve Here
            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult Create(int? ExamID)
        {
            model.Exams = db.Exams.OrderByDescending(a=>a.ExamID).ToList();
            model.SelectedExam = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc");
            ViewBag.ExamTermID = new SelectList(db.ExamTerms, "ExamTermID", "TermName");
            ViewBag.ExamSeasonID = new SelectList(db.ExamSeasons, "ExamSeasonID", "SeasonName");
            ViewBag.MessageType = "";
            ViewBag.Message = "";
            return View("Index", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Exam Exam)
        {
            try
            {
                Exam d = db.Exams.FirstOrDefault(de => de.ExamTitle == Exam.ExamTitle);
                if (d == null)
                {
                    Exam.CreatedBy = Convert.ToInt32(Session["emp_id"]);
                    Exam.CreatedOn = DateTime.Now;
                    db.Exams.Add(Exam);
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
                    ModelState.AddModelError(string.Empty, "Exam Name already exists.");
                    ViewBag.MessageType = "error";
                    ViewBag.Message = "Exam Name already exists.";
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
            model.Exams = db.Exams.OrderByDescending(a=>a.ExamID).ToList();
            model.SelectedExam = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc", Exam.IsActive);
            ViewBag.ExamTermID = new SelectList(db.ExamTerms, "ExamTermID", "TermName", Exam.ExamTermID);
            ViewBag.ExamSeasonID = new SelectList(db.ExamSeasons, "ExamSeasonID", "SeasonName", Exam.ExamSeasonID);

            return View("Index", model);
        }

        public ActionResult Update(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Exam Exam = db.Exams.Find(id);
            if (Exam == null)
            {
                return HttpNotFound();
            }

            model.Exams = db.Exams.OrderByDescending(a=>a.ExamID).ToList();
            model.SelectedExam = Exam;
            model.DisplayMode = "ReadWrite";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc", Exam.IsActive);
            ViewBag.ExamTermID = new SelectList(db.ExamTerms, "ExamTermID", "TermName", Exam.ExamTermID);
            ViewBag.ExamSeasonID = new SelectList(db.ExamSeasons, "ExamSeasonID", "SeasonName", Exam.ExamSeasonID);
            ViewBag.MessageType = "";
            ViewBag.Message = "";
            return View("Index", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Exam Exam)
        {
            try
            {
                db.Entry(Exam).State = EntityState.Modified;
                Exam.ModifiedBy = Convert.ToInt32(Session["emp_id"]);
                Exam.ModifiedOn = DateTime.Now;
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
            model.Exams = db.Exams.OrderByDescending(a=>a.ExamID).ToList();
            model.SelectedExam = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc", Exam.IsActive);
            ViewBag.ExamTermID = new SelectList(db.ExamTerms, "ExamTermID", "TermName", Exam.ExamTermID);
            ViewBag.ExamSeasonID = new SelectList(db.ExamSeasons, "ExamSeasonID", "SeasonName", Exam.ExamSeasonID);

            return View("Index", model);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Exam Exam = db.Exams.Find(id);
            if (Exam == null)
            {
                return HttpNotFound();
            }

            model.Exams = db.Exams.OrderByDescending(a=>a.ExamID).ToList();
            model.SelectedExam = Exam;
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
                Exam Exam = db.Exams.Find(id);
                db.Exams.Remove(Exam);
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
            model.Exams = db.Exams.OrderByDescending(a=>a.ExamID).ToList();
            model.SelectedExam = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc");
            ViewBag.ExamTermID = new SelectList(db.ExamTerms, "ExamTermID", "TermName");
            ViewBag.ExamSeasonID = new SelectList(db.ExamSeasons, "ExamSeasonID", "SeasonName");

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
