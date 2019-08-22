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
    public class LmsQuizQuestionsController : Controller
    {

        private ModelCMSNewContainer db = new ModelCMSNewContainer();
        LMSQuizIQuestionViewModel model = new LMSQuizIQuestionViewModel();

        public ActionResult Index(int? LMSQuizID)
        {
            model.LMSQuizIQuestions = db.LMSQuizIQuestions.Where(x => x.LMSQuizID == LMSQuizID).OrderBy(a => a.LMSQuestionTypeID).ToList();
            model.SelectedLMSQuizIQuestions = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc");
            ViewBag.LMSQuestionTypeID = new SelectList(db.LMSQuestionTypes, "LMSQuestionTypeID", "QuestionTypeName");
            ViewBag.MessageType = "";
            ViewBag.LMSQuizID = LMSQuizID;
            ViewBag.Message = "";
            return View(model);
        }


        [HttpGet]
        public ActionResult Create(int? LMSQuizID)
        {
            model.LMSQuizIQuestions = db.LMSQuizIQuestions.Where(x => x.LMSQuizID == LMSQuizID).OrderBy(a => a.LMSQuestionTypeID).ToList();
            model.SelectedLMSQuizIQuestions = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.LMSQuizID = LMSQuizID;
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc");
            ViewBag.LMSQuestionTypeID = new SelectList(db.LMSQuestionTypes, "LMSQuestionTypeID", "QuestionTypeName");
            ViewBag.MessageType = "";
            ViewBag.Message = "";
            return View("Index", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(LMSQuizIQuestion LMS, string AnswerMCQ1, string AnswerMCQ2, string AnswerMCQ3, string AnswerMCQ4, string AnswerSCQ1, string AnswerSCQ2, string AnswerSCQ3, string AnswerSCQ4)
        {
            try
            {
                LMSQuizIQuestion d = db.LMSQuizIQuestions.FirstOrDefault(de => de.AskedQuestion == LMS.AskedQuestion && de.LMSQuizID == LMS.LMSQuizID);
                
                if (d == null)
                {

                    if (AnswerMCQ1 == "on")
                        LMS.AnswerMCQ1 = true;
                    else
                        LMS.AnswerMCQ1 = false;

                    if (AnswerMCQ2 == "on")
                        LMS.AnswerMCQ2 = true;
                    else
                        LMS.AnswerMCQ2 = false;

                    if (AnswerMCQ3 == "on")
                        LMS.AnswerMCQ3 = true;
                    else
                        LMS.AnswerMCQ3 = false;

                    if (AnswerMCQ4 == "on")
                        LMS.AnswerMCQ4 = true;
                    else
                        LMS.AnswerMCQ4 = false;

                    if (AnswerSCQ1 == "true")
                        LMS.AnswerSCQ1 = true;
                    else
                        LMS.AnswerSCQ1 = false;

                    if (AnswerSCQ2 == "true")
                        LMS.AnswerSCQ2 = true;
                    else
                        LMS.AnswerSCQ2 = false;

                    if (AnswerSCQ3 == "true")
                        LMS.AnswerSCQ3 = true;
                    else
                        LMS.AnswerSCQ3 = false;

                    if (AnswerSCQ4 == "true")
                        LMS.AnswerSCQ4 = true;
                    else
                        LMS.AnswerSCQ4 = false;

                    //LMS.CreatedBy = Convert.ToInt32(Session["emp_id"]);
                    LMS.CreatedBy = Convert.ToInt32(Session["CurrentUserID"]);
                    LMS.CreatedOn = DateTime.Now;
                    db.LMSQuizIQuestions.Add(LMS);
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
            model.LMSQuizIQuestions = db.LMSQuizIQuestions.Where(x => x.LMSQuizID == LMS.LMSQuizID).OrderBy(a => a.LMSQuestionTypeID).ToList();
            model.SelectedLMSQuizIQuestions = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc", LMS.IsActive);
            ViewBag.LMSQuestionTypeID = new SelectList(db.LMSQuestionTypes, "LMSQuestionTypeID", "QuestionTypeName");

            return View("Index", model);
        }

        public ActionResult Update(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            LMSQuizIQuestion LMS = db.LMSQuizIQuestions.Find(id);
            if (LMS == null)
            {
                return HttpNotFound();
            }

            model.LMSQuizIQuestions = db.LMSQuizIQuestions.Where(x => x.LMSQuizID == LMS.LMSQuizID).OrderBy(a => a.LMSQuestionTypeID).ToList();
            model.SelectedLMSQuizIQuestions = LMS;
            model.DisplayMode = "ReadWrite";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc", LMS.IsActive);
            ViewBag.LMSQuestionTypeID = new SelectList(db.LMSQuestionTypes, "LMSQuestionTypeID", "QuestionTypeName", LMS.LMSQuestionTypeID);
            ViewBag.LMSQuizID = LMS.LMSQuizID;
            ViewBag.MessageType = "";
            ViewBag.Message = "";
            return View("Index", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(LMSQuizIQuestion LMS, string AnswerMCQ1, string AnswerMCQ2, string AnswerMCQ3, string AnswerMCQ4, string AnswerSCQ1, string AnswerSCQ2, string AnswerSCQ3, string AnswerSCQ4)
        {
            try
            {

                if (AnswerMCQ1 == "on")
                    LMS.AnswerMCQ1 = true;
                else
                    LMS.AnswerMCQ1 = false;

                if (AnswerMCQ2 == "on")
                    LMS.AnswerMCQ2 = true;
                else
                    LMS.AnswerMCQ2 = false;

                if (AnswerMCQ3 == "on")
                    LMS.AnswerMCQ3 = true;
                else
                    LMS.AnswerMCQ3 = false;

                if (AnswerMCQ4 == "on")
                    LMS.AnswerMCQ4 = true;
                else
                    LMS.AnswerMCQ4 = false;

                if (AnswerSCQ1 == "true")
                    LMS.AnswerSCQ1 = true;
                else
                    LMS.AnswerSCQ1 = false;

                if (AnswerSCQ2 == "true")
                    LMS.AnswerSCQ2 = true;
                else
                    LMS.AnswerSCQ2 = false;

                if (AnswerSCQ3 == "true")
                    LMS.AnswerSCQ3 = true;
                else
                    LMS.AnswerSCQ3 = false;

                if (AnswerSCQ4 == "true")
                    LMS.AnswerSCQ4 = true;
                else
                    LMS.AnswerSCQ4 = false;

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
            model.LMSQuizIQuestions = db.LMSQuizIQuestions.Where(x => x.LMSQuizID == LMS.LMSQuizID).OrderBy(a => a.LMSQuestionTypeID).ToList();
            model.SelectedLMSQuizIQuestions = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc", LMS.IsActive);
            ViewBag.LMSQuestionTypeID = new SelectList(db.LMSQuestionTypes, "LMSQuestionTypeID", "QuestionTypeName", LMS.LMSQuestionTypeID);

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