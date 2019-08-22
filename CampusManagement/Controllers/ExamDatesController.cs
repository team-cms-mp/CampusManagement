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
    public class ExamDatesController : Controller
    {
        private ModelCMSNewContainer db = new ModelCMSNewContainer();
        ExamDatesViewModel model = new ExamDatesViewModel();

        public ActionResult Index(int? ExamID)
        {

            model.ExamDates = db.ExamDates.Where(a => a.ExamID == ExamID).OrderByDescending(a => a.ExamDateID).ToList();
            model.SelectedExamDate = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.ExamID = new SelectList(db.Exams, "ExamID", "ExamTitle");
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc");
            ViewBag.hdnExamID = ExamID;
            ViewBag.MessageType = "";
            ViewBag.Message = "";
            return View(model);
        }

        [HttpGet]
        public ActionResult Create(int? ExamID)
        {
            model.ExamDates = db.ExamDates.Where(a => a.ExamID == ExamID).OrderByDescending(a => a.ExamDateID).ToList();
            model.SelectedExamDate = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.ExamID = new SelectList(db.Exams, "ExamID", "ExamTitle");
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc");
            ViewBag.hdnExamID = ExamID;
            ViewBag.MessageType = "";
            ViewBag.Message = "";
            return View("Index", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ExamDate examDate)
        {
            try
            {
                ExamDate d = db.ExamDates.FirstOrDefault(de => de.ExamID == examDate.ExamID && de.ExamDateTitle == examDate.ExamDateTitle);
                if (d == null)
                {
                    List<ExamDate> dateList = new List<ExamDate>();
                    dateList = db.ExamDates.Where(a => a.ExamID == examDate.ExamID).ToList();

                    if (dateList.Count() < 6)
                    {
                        examDate.CreatedBy = Convert.ToInt32(Session["emp_id"]);

                        try
                        {
                            db.InsertExamDate(examDate.ExamID, examDate.ExamDateTitle, examDate.CreatedBy);
                            ViewBag.MessageType = "success";
                            ViewBag.Message = "Data has been saved successfully.";
                        }
                        catch (Exception ex)
                        {
                            ViewBag.MessageType = "error";
                            ViewBag.Message = ex.Message;
                            ModelState.AddModelError(string.Empty, ex.Message);
                        }
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "You cant not exceed more then 6 days.");
                        ViewBag.MessageType = "error";
                        ViewBag.Message = "You cant not exceed more then 6 days..";
                    }


                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Same Date already exists.");
                    ViewBag.MessageType = "error";
                    ViewBag.Message = "Same Date already exists.";
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
            model.ExamDates = db.ExamDates.Where(a => a.ExamID == examDate.ExamID).OrderByDescending(a => a.ExamDateID).ToList();
            model.SelectedExamDate = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.hdnExamID = examDate.ExamID;
             ViewBag.hdnExamDateID = examDate.ExamDateID;
            ViewBag.ExamID = new SelectList(db.Exams, "ExamID", "ExamTitle");
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc", examDate.IsActive);

            return View("Index", model);
        }

        public ActionResult Update(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ExamDate examDate = db.ExamDates.Find(id);
            if (examDate == null)
            {
                return HttpNotFound();
            }

            model.ExamDates = db.ExamDates.Where(a => a.ExamID == examDate.ExamID).OrderByDescending(a => a.ExamDateID).ToList();
            model.SelectedExamDate = examDate;
            model.DisplayMode = "ReadWrite";
           
            ViewBag.ExamID = new SelectList(db.Exams, "ExamID", "ExamTitle");
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc", examDate.IsActive);
            ViewBag.hdnExamID = examDate.ExamID;

            ViewBag.MessageType = "";
            ViewBag.Message = "";
            return View("Index", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ExamDate Exam)
        {

            try
            {
                ExamDate d = db.ExamDates.FirstOrDefault(de => de.ExamID == Exam.ExamID && de.ExamDateTitle == Exam.ExamDateTitle);
                if (d == null)
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
                else
                {
                    ModelState.AddModelError(string.Empty, "Same Date already exists.");
                    ViewBag.MessageType = "error";
                    ViewBag.Message = "Same Date already exists.";
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


            model.ExamDates = db.ExamDates.Where(a => a.ExamID == Exam.ExamID).OrderByDescending(a => a.ExamDateID).ToList();
            model.SelectedExamDate = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.hdnExamID = Exam.ExamID;
            ViewBag.ExamID = new SelectList(db.Exams, "ExamID", "ExamTitle");
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc", Exam.IsActive);

            return View("Index", model);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ExamDate Exam = db.ExamDates.Find(id);
            if (Exam == null)
            {
                return HttpNotFound();
            }

            model.ExamDates = db.ExamDates.OrderByDescending(a => a.ExamDateID).ToList();
            model.SelectedExamDate = Exam;
            ViewBag.hdnExamID = Exam.ExamID;
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
                ExamDate Exam = db.ExamDates.Find(id);
                db.ExamDates.Remove(Exam);
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
            model.ExamDates = db.ExamDates.OrderByDescending(a => a.ExamDateID).ToList();
            model.SelectedExamDate = null;
            model.DisplayMode = "WriteOnly";

            ViewBag.ExamID = new SelectList(db.Exams, "ExamID", "ExamTitle");
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
