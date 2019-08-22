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
    public class GeneralSettingController : Controller
    {
        private ModelCMSNewContainer db = new ModelCMSNewContainer();
        GeneralSettingViewModel model = new GeneralSettingViewModel();

        public ActionResult Index()
        {
            model.GeneralSettings = db.GeneralSettings.OrderByDescending(a=>a.GeneralSettingID).ToList();
            model.SelectedGeneralSettings = null;
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc");
            model.DisplayMode = "WriteOnly";
            ViewBag.MessageType = "";
            ViewBag.Message = "";
            return View(model);
        }

        [HttpGet]
        public ActionResult Create()
        {
            model.GeneralSettings = db.GeneralSettings.OrderByDescending(a=>a.GeneralSettingID).ToList();
            model.SelectedGeneralSettings = null;
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc");
            model.DisplayMode = "WriteOnly";
            ViewBag.MessageType = "";
            ViewBag.Message = "";
            return View("Index", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(GeneralSetting generalSetting)
        {
            try
            {
                GeneralSetting d = db.GeneralSettings.FirstOrDefault(de => de.GeneralSettingID == generalSetting.GeneralSettingID);
                if (d == null)
                {
                    generalSetting.CreatedBy = Convert.ToInt32(Session["emp_id"]);
                    generalSetting.CreatedOn = DateTime.Now;
                    db.GeneralSettings.Add(generalSetting);
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
                    ModelState.AddModelError(string.Empty, "General Setting Name already exists.");
                    ViewBag.MessageType = "error";
                    ViewBag.Message = "General Setting Name already exists.";
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
                ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc");
                ViewBag.MessageType = "error";
                ViewBag.Message = ErrorMessage;
            }
            model.GeneralSettings = db.GeneralSettings.OrderByDescending(a=>a.GeneralSettingID).ToList();
            model.SelectedGeneralSettings = null;
            model.DisplayMode = "WriteOnly";
            return RedirectToAction("Index", model);
        }

        public ActionResult Update(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            GeneralSetting discipline = db.GeneralSettings.Find(id);
            if (discipline == null)
            {
                return HttpNotFound();
            }
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc");
            model.GeneralSettings = db.GeneralSettings.OrderByDescending(a=>a.GeneralSettingID).ToList();
            model.SelectedGeneralSettings = discipline;
            model.DisplayMode = "ReadWrite";
            ViewBag.MessageType = "";
            ViewBag.Message = "";
            return View("Index", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(GeneralSetting generalSetting)
        {
            try
            {
                db.Entry(generalSetting).State = EntityState.Modified;
                generalSetting.ModifiedBy = Convert.ToInt32(Session["emp_id"]);
                generalSetting.ModifiedOn = DateTime.Now;
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
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc");
            model.GeneralSettings = db.GeneralSettings.OrderByDescending(a=>a.GeneralSettingID).ToList();
            model.SelectedGeneralSettings = null;
            model.DisplayMode = "WriteOnly";
            return RedirectToAction("Index", model);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GeneralSetting discipline = db.GeneralSettings.Find(id);
            if (discipline == null)
            {
                return HttpNotFound();
            }

            model.GeneralSettings = db.GeneralSettings.OrderByDescending(a=>a.GeneralSettingID).ToList();
            model.SelectedGeneralSettings = discipline;
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc");
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
                GeneralSetting discipline = db.GeneralSettings.Find(id);
                db.GeneralSettings.Remove(discipline);
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
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc");
            model.GeneralSettings = db.GeneralSettings.OrderByDescending(a=>a.GeneralSettingID).ToList();
            model.SelectedGeneralSettings = null;
            model.DisplayMode = "WriteOnly";
            
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
