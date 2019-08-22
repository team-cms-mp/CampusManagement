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
    public class OBE_QuestionTypesController : Controller
    {
        private ModelCMSContainer db = new ModelCMSContainer();
        OBE_QuestionTypeViewModel model = new OBE_QuestionTypeViewModel();

        public ActionResult Index()
        {
            model.OBE_QuestionTypes = db.OBE_QuestionType.OrderByDescending(a => a.QuestionTypeID).ToList();
            model.SelectedOBE_QuestionType = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc");
            ViewBag.MessageType = "";
            ViewBag.Message = "";
            return View(model);
        }



        public ActionResult Create()
        {
            model.OBE_QuestionTypes = db.OBE_QuestionType.OrderByDescending(a => a.QuestionTypeID).ToList();
            model.SelectedOBE_QuestionType = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc");
            ViewBag.MessageType = "";
            ViewBag.Message = "";
            return View("Index", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(OBE_QuestionType obe_QuestionType)
        {
            try
            {
                OBE_QuestionType ba = db.OBE_QuestionType.FirstOrDefault(bac => bac.QuestionTypeID == obe_QuestionType.QuestionTypeID);
                if (ba == null)
                {
                    obe_QuestionType.CreatedBy = Convert.ToInt32(Session["emp_id"]);
                    obe_QuestionType.CreatedOn = DateTime.Now;
                    db.OBE_QuestionType.Add(obe_QuestionType);
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
                    ModelState.AddModelError(string.Empty, "Account # already exists.");
                    ViewBag.MessageType = "error";
                    ViewBag.Message = "Account # already exists.";
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
            model.OBE_QuestionTypes = db.OBE_QuestionType.OrderByDescending(a => a.QuestionTypeID).ToList();
            model.SelectedOBE_QuestionType = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc", obe_QuestionType.IsActive);

            return View("Index", model);
        }

        public ActionResult Update(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            OBE_QuestionType obe_QuestionType = db.OBE_QuestionType.Find(id);
            if (obe_QuestionType == null)
            {
                return HttpNotFound();
            }

            model.OBE_QuestionTypes = db.OBE_QuestionType.OrderByDescending(a => a.QuestionTypeID).ToList();
            model.SelectedOBE_QuestionType = obe_QuestionType;
            model.DisplayMode = "ReadWrite";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc", obe_QuestionType.IsActive);
            ViewBag.MessageType = "";
            ViewBag.Message = "";
            return View("Index", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(OBE_QuestionType obe_QuestionType)
        {
            try
            {
                db.Entry(obe_QuestionType).State = EntityState.Modified;
                obe_QuestionType.ModifiedBy = Convert.ToInt32(Session["emp_id"]);
                obe_QuestionType.ModifiedOn = DateTime.Now;
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
            model.OBE_QuestionTypes = db.OBE_QuestionType.OrderByDescending(a => a.QuestionTypeID).ToList();
            model.SelectedOBE_QuestionType = obe_QuestionType;
            model.DisplayMode = "WriteOnly";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc", obe_QuestionType.IsActive);

            return View("Index", model);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OBE_QuestionType obe_QuestionType = db.OBE_QuestionType.Find(id);
            if (obe_QuestionType == null)
            {
                return HttpNotFound();
            }

            model.OBE_QuestionTypes = db.OBE_QuestionType.OrderByDescending(a => a.QuestionTypeID).ToList();
            model.SelectedOBE_QuestionType = obe_QuestionType;
            model.DisplayMode = "Delete";
            ViewBag.MessageType = "";
            ViewBag.Message = "";
            return View("Index", model);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int? id)
        {
            try
            {
                OBE_QuestionType obe_QuestionType = db.OBE_QuestionType.Find(id);
                db.OBE_QuestionType.Remove(obe_QuestionType);
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
            model.OBE_QuestionTypes = db.OBE_QuestionType.OrderByDescending(a => a.QuestionTypeID).ToList();
            model.SelectedOBE_QuestionType = null;
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
