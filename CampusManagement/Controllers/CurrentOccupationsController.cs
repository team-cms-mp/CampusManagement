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
    public class CurrentOccupationsController : Controller
    {
        private ModelCMSNewContainer db = new ModelCMSNewContainer();
        CurrentOccupationsViewModel model = new CurrentOccupationsViewModel();

        public ActionResult Index()
        {
            model.CurrentOccupations = db.CurrentOccupations.OrderByDescending(a=>a.CurrentOccupationID).ToList();
            model.SelectedCurrentOccupation = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc");
            ViewBag.MessageType = "";
            ViewBag.Message = "";
            return View(model);
        }

        [HttpPost]
        public ActionResult Index(string Search)
        {
            var lst = db.CurrentOccupations.Where(x => x.CurrentOccupationName.Contains(Search)).OrderByDescending(a => a.CurrentOccupationID).ToList();
            model.CurrentOccupations = lst;
            model.SelectedCurrentOccupation = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc");
            ViewBag.MessageType = "";
            ViewBag.Message = "";
            return View(model);
        }

        [HttpGet]
        public ActionResult Create()
        {
            model.CurrentOccupations = db.CurrentOccupations.OrderByDescending(a=>a.CurrentOccupationID).ToList();
            model.SelectedCurrentOccupation = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc");
            ViewBag.MessageType = "";
            ViewBag.Message = "";
            return View("Index", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CurrentOccupation currentoccupation)
        {
            try
            {
                CurrentOccupation co = db.CurrentOccupations.FirstOrDefault(de => de.CurrentOccupationName == currentoccupation.CurrentOccupationName);
                if (co == null)
                {
                    currentoccupation.CreatedBy = Convert.ToInt32(Session["emp_id"]);
                    currentoccupation.CreatedOn = DateTime.Now;
                    db.CurrentOccupations.Add(currentoccupation);
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
                    ModelState.AddModelError(string.Empty, "Occupation Name already exists.");
                    ViewBag.MessageType = "error";
                    ViewBag.Message = "Occupation Name already exists.";
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
            model.CurrentOccupations = db.CurrentOccupations.OrderByDescending(a=>a.CurrentOccupationID).ToList();
            model.SelectedCurrentOccupation = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc", currentoccupation.IsActive);

            return View("Index", model);
        }

        public ActionResult Update(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            CurrentOccupation currentoccupation = db.CurrentOccupations.Find(id);
            if (currentoccupation == null)
            {
                return HttpNotFound();
            }

            model.CurrentOccupations = db.CurrentOccupations.OrderByDescending(a=>a.CurrentOccupationID).ToList();
            model.SelectedCurrentOccupation = currentoccupation;
            model.DisplayMode = "ReadWrite";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc", currentoccupation.IsActive);
            ViewBag.MessageType = "";
            ViewBag.Message = "";
            return View("Index", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CurrentOccupation currentoccupation)
        {
            try
            {
                db.Entry(currentoccupation).State = EntityState.Modified;
                currentoccupation.ModifiedBy = Convert.ToInt32(Session["emp_id"]);
                currentoccupation.ModifiedOn = DateTime.Now;
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
            model.CurrentOccupations = db.CurrentOccupations.OrderByDescending(a=>a.CurrentOccupationID).ToList();
            model.SelectedCurrentOccupation = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc", currentoccupation.IsActive);

            return View("Index", model);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CurrentOccupation currentoccupation = db.CurrentOccupations.Find(id);
            if (currentoccupation == null)
            {
                return HttpNotFound();
            }

            model.CurrentOccupations = db.CurrentOccupations.OrderByDescending(a=>a.CurrentOccupationID).ToList();
            model.SelectedCurrentOccupation = currentoccupation;
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
                CurrentOccupation currentoccupation = db.CurrentOccupations.Find(id);
                db.CurrentOccupations.Remove(currentoccupation);
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
            model.CurrentOccupations = db.CurrentOccupations.OrderByDescending(a=>a.CurrentOccupationID).ToList();
            model.SelectedCurrentOccupation = null;
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
