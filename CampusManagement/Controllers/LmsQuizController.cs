using CampusManagement.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace CampusManagement.Controllers
{
    public class LmsQuizController : Controller
    {

        private ModelCMSNewContainer db = new ModelCMSNewContainer();
        LMSQuizViewModel model = new LMSQuizViewModel();

        public ActionResult Index(int? LMSWeekDetailID)
        {
            model.LMSQuizs = db.LMSQuizs.Where(x => x.LMSWeekDetailID == LMSWeekDetailID).OrderByDescending(a => a.LMSQuizID).ToList();
            model.SelectedLMSQuizs = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc");
            ViewBag.MessageType = "";
            ViewBag.ProgramCourseID = LMSWeekDetailID;
            ViewBag.Message = "";
            return View(model);
        }


        [HttpGet]
        public ActionResult Create(int? LMSWeekDetailID)
        {
            model.LMSQuizs = db.LMSQuizs.Where(x => x.LMSWeekDetailID == LMSWeekDetailID).OrderByDescending(a => a.LMSQuizID).ToList();
            model.SelectedLMSQuizs = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.ProgramCourseID = LMSWeekDetailID;
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc");
            ViewBag.MessageType = "";
            ViewBag.Message = "";
            return View("Index", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(LMSQuiz LMS)
        {
            try
            {
                LMSQuiz d = db.LMSQuizs.FirstOrDefault(de => de.QuizTitle == LMS.QuizTitle && de.LMSWeekDetailID == LMS.LMSWeekDetailID);

                if (d == null)
                {
                    //LMS.CreatedBy = Convert.ToInt32(Session["emp_id"]);
                    LMS.CreatedBy = Convert.ToInt32(Session["CurrentUserID"]);
                    LMS.CreatedOn = DateTime.Now;
                    db.LMSQuizs.Add(LMS);
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
                    ModelState.AddModelError(string.Empty, "Name already exists.");
                    ViewBag.MessageType = "error";
                    ViewBag.Message = "Name already exists.";
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
            model.LMSQuizs = db.LMSQuizs.Where(x => x.LMSWeekDetailID == LMS.LMSWeekDetailID).OrderByDescending(a => a.LMSQuizID).ToList();
            model.SelectedLMSQuizs = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc", LMS.IsActive);

            return View("Index", model);
        }

        public ActionResult Update(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            LMSQuiz LMS = db.LMSQuizs.Find(id);
            if (LMS == null)
            {
                return HttpNotFound();
            }

            model.LMSQuizs = db.LMSQuizs.Where(x => x.LMSWeekDetailID == LMS.LMSWeekDetailID).OrderByDescending(a => a.LMSQuizID).ToList();
            model.SelectedLMSQuizs = LMS;
            model.DisplayMode = "ReadWrite";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc", LMS.IsActive);
            ViewBag.ProgramCourseID = LMS.LMSWeekDetailID;
            ViewBag.MessageType = "";
            ViewBag.Message = "";
            return View("Index", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(LMSQuiz LMS)
        {
            try
            {
                db.Entry(LMS).State = EntityState.Modified;
                //LMS.ModifiedBy = Convert.ToInt32(Session["emp_id"]);
                LMS.ModifiedBy = Convert.ToInt32(Session["CurrentUserID"]);
                LMS.ModifiedOn = DateTime.Now;
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
            model.LMSQuizs = db.LMSQuizs.Where(x => x.LMSWeekDetailID == LMS.LMSWeekDetailID).OrderByDescending(a => a.LMSQuizID).ToList();
            model.SelectedLMSQuizs = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc", LMS.IsActive);

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