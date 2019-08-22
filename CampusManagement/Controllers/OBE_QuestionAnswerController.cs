using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CampusManagement.Models;
using Newtonsoft.Json;
using PagedList;
using System.Globalization;

using CampusManagement.App_Code;
using System.Configuration;
using System.IO;
using System.Net.Mail;


namespace CampusManagement.Controllers
{
    public class OBE_QuestionAnswerController : Controller
    {
        private ModelCMSContainer db = new ModelCMSContainer();
        QuestionAnswer ModelObj = new QuestionAnswer();


        // GET: ObeProgramPLO
        [HttpGet]
        public ActionResult Index(int? page, int? pageSize,  string IsCorrectAnswer,string QuestionName, string message)
        {
            if (pageSize == null)
            {
                pageSize = 10;
            }
            ViewBag.pageSize = pageSize;
            int pageNumber = (page ?? 1);


            if (string.IsNullOrEmpty(IsCorrectAnswer))
            {
                IsCorrectAnswer = "0";
            }
            if (QuestionName == null || QuestionName == "")
            {
                QuestionName = "";
            }

            ViewBag.QuestionID = new SelectList(db.QuestionBanks, "QuestionID", "QuestionName");
            ViewBag.hdnQuestionName = QuestionName;
            ViewBag.hdnIsCorrectAnswer = IsCorrectAnswer;
            return View(db.GetAllOBE_QuestionAnswer(Convert.ToInt32(IsCorrectAnswer), QuestionName, "").OrderByDescending(a => a.QuestionAnswerID).ToList().ToPagedList(pageNumber, Convert.ToInt32(pageSize)));
        }

        [HttpPost]
        public ActionResult Index(int? page, string IsCorrectAnswer, string QuestionName)
        {
            int pageSize = 10;
            int pageNumber = (page ?? 1);

            if (string.IsNullOrEmpty(IsCorrectAnswer))
            {
                IsCorrectAnswer = "0";
            }
            if (QuestionName == null || QuestionName == "")
            {
                QuestionName = "";
            }
            
            ViewBag.hdnQuestionName = QuestionName;
            ViewBag.hdnIsCorrectAnswer= IsCorrectAnswer;
            ViewBag.QuestionID = new SelectList(db.QuestionBanks, "QuestionID", "QuestionName");
            return View(db.GetAllOBE_QuestionAnswer(Convert.ToInt32(IsCorrectAnswer), QuestionName, "").OrderByDescending(a=>a.QuestionAnswerID).ToList().ToPagedList(pageNumber, Convert.ToInt32(pageSize)));
        }

        [HttpGet]
        public ActionResult Create(int? QuestionAnswerID)
        {
            ViewBag.QuestionID = new SelectList(db.QuestionBanks, "QuestionID", "QuestionName");
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc");
            return View(ModelObj);
        }

        [HttpPost]
        public ActionResult Create(QuestionAnswer obj)
        {
            ViewBag.QuestionID = new SelectList(db.QuestionBanks, "QuestionID", "QuestionName");
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc", obj.IsActive);
            int QuestionAnswerID = Convert.ToInt32(db.InsertOrUpdateOBE_QuestionAnswer(
           obj.QuestionAnswerID,
             obj.QuestionID,
           obj.IsCorrectAnswer,
         
           obj.AnswerDescription,
           null,
           Convert.ToInt32(Session["emp_id"]),
           obj.IsActive,
           null,
           Convert.ToInt32(Session["emp_id"])).FirstOrDefault());
            if (QuestionAnswerID > 0)
            {
                ViewBag.MessageType = "success";
                ViewBag.Message = "Data has been saved successfully.";
            }
            else
            {
                ViewBag.MessageType = "error";
                ViewBag.Message = "Error while executing query, please try again";
            }

            if (QuestionAnswerID == 0)
            {

            }
            else
            {
                obj = db.QuestionAnswers.Where(c => c.QuestionAnswerID == QuestionAnswerID).FirstOrDefault();
            }

            db.InsertOrUpdateOBE_QuestionAnswer(obj.QuestionAnswerID, obj.QuestionID, obj.IsCorrectAnswer, obj.AnswerDescription,
              DateTime.Now,
              Convert.ToInt32(Session["emp_id"]),
               obj.IsActive,
               DateTime.Now,
              Convert.ToInt32(Session["emp_id"]));


            ViewBag.QuestionID = new SelectList(db.QuestionBanks, "QuestionID", "QuestionName");
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc");
            return View(obj);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            QuestionAnswer questionanswer = db.QuestionAnswers.Find(id);
           
            if (questionanswer == null)
            {
                return HttpNotFound();
            }
            else
            {
                ModelObj = db.QuestionAnswers.Where(c => c.QuestionAnswerID == id).FirstOrDefault();
            }

            
            ViewBag.QuestionID = new SelectList(db.QuestionBanks, "QuestionID","QuestionName" ,questionanswer.QuestionID);
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc",questionanswer.IsActive);
            ViewBag.MessageType = "";
            ViewBag.Message = "";
            return View(ModelObj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(QuestionAnswer questionanswer)
        {
            try
            {
                db.Entry(questionanswer).State = EntityState.Modified;
                questionanswer.ModifiedBy = Convert.ToInt32(Session["emp_id"]);
                questionanswer.ModifiedOn = DateTime.Now;
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
            
            ViewBag.QuestionID = new SelectList(db.QuestionBanks, "QuestionID", "QuestionName", questionanswer.QuestionID);
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc", questionanswer.IsActive);

            return RedirectToAction("Index");
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            QuestionAnswer questionanswer = db.QuestionAnswers.Find(id);
            if (questionanswer == null)
            {
                return HttpNotFound();
            }
           
            else
            {
                ModelObj = db.QuestionAnswers.Where(c => c.QuestionAnswerID == id).FirstOrDefault();
            }

            ViewBag.QuestionID = new SelectList(db.QuestionBanks, "QuestionID", "QuestionName", questionanswer.QuestionID);
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc", questionanswer.IsActive);
        
            ViewBag.MessageType = "";
            ViewBag.Message = "";
            return View(ModelObj);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                QuestionAnswer questionanswer = db.QuestionAnswers.Find(id);
                db.QuestionAnswers.Remove(questionanswer);
                db.SaveChanges();
                ViewBag.MessageType = "success";
                ViewBag.Message = "Record has been removed successfully.";
                ViewBag.QuestionID = new SelectList(db.QuestionAnswers, "QuestionID", "QuestionName", questionanswer.QuestionID);
                ViewBag.IsActive = new SelectList(db.QuestionAnswers, "OptionDesc", "OptionDesc", questionanswer.IsActive);
            }
            catch (DbUpdateException ex)
            {
                ViewBag.MessageType = "error";
                ViewBag.Message = ex.Message;
                ModelState.AddModelError(string.Empty, ex.Message);
            }

            return RedirectToAction("Index");
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
