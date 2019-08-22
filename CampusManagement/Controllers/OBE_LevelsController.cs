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
    public class OBE_LevelsController : Controller
    {
        private ModelCMSContainer db = new ModelCMSContainer();
        OBE_LevelViewModel model = new OBE_LevelViewModel();

        public ActionResult Index()
        {
            model.OBE_Levels = db.OBE_Level.OrderByDescending(a => a.LevelID).ToList();
            model.SelectedOBE_Level = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc");
            ViewBag.MessageType = "";
            ViewBag.Message = "";
            return View(model);
        }

        [HttpGet]
        public ActionResult Create()
        {
            model.OBE_Levels = db.OBE_Level.OrderByDescending(a => a.LevelID).ToList();
            model.SelectedOBE_Level = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc");
            ViewBag.MessageType = "";
            ViewBag.Message = "";
            return View("Index", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(OBE_Level obeLevel)
        {
            try
            {
                OBE_Level ba = db.OBE_Level.FirstOrDefault(bac => bac.LevelID == obeLevel.LevelID);
                if (ba == null)
                {
                    obeLevel.CreatedBy = Convert.ToInt32(Session["emp_id"]);
                    obeLevel.CreatedOn = DateTime.Now;
                    db.OBE_Level.Add(obeLevel);
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
            model.OBE_Levels = db.OBE_Level.OrderByDescending(a => a.LevelID).ToList();
            model.SelectedOBE_Level = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc", obeLevel.IsActive);

            return View("Index", model);
        }

        public ActionResult Update(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            OBE_Level obeLevel = db.OBE_Level.Find(id);
            if (obeLevel == null)
            {
                return HttpNotFound();
            }

            model.OBE_Levels = db.OBE_Level.OrderByDescending(a => a.LevelID).ToList();
            model.SelectedOBE_Level = obeLevel;
            model.DisplayMode = "ReadWrite";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc", obeLevel.IsActive);
            ViewBag.MessageType = "";
            ViewBag.Message = "";
            return View("Index", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(OBE_Level obeLevel)
        {
            try
            {
                db.Entry(obeLevel).State = EntityState.Modified;
                obeLevel.ModifiedBy = Convert.ToInt32(Session["emp_id"]);
                obeLevel.ModifiedOn = DateTime.Now;
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
            model.OBE_Levels = db.OBE_Level.OrderByDescending(a => a.LevelID).ToList();
            model.SelectedOBE_Level = obeLevel;
            model.DisplayMode = "WriteOnly";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc", obeLevel.IsActive);

            return View("Index", model);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OBE_Level obeLevel = db.OBE_Level.Find(id);
            if (obeLevel == null)
            {
                return HttpNotFound();
            }

            model.OBE_Levels = db.OBE_Level.OrderByDescending(a => a.LevelID).ToList();
            model.SelectedOBE_Level = obeLevel;
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
                OBE_Level obeLevel = db.OBE_Level.Find(id);
                db.OBE_Level.Remove(obeLevel);
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
            model.OBE_Levels = db.OBE_Level.OrderByDescending(a => a.LevelID).ToList();
            model.SelectedOBE_Level = null;
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
