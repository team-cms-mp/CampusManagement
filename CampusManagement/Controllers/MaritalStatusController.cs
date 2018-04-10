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
    [Authorize(Roles = "Account Officer,Accounts Officer,Admin Assistant,Admin Officer,Admin.Assistant,Assist. Account Officer,Assist.Technician,Import Manager,Manager Servive & Support,Office Manager,Officer QMS,RSM - Center 2,RSM - South,Sales & Service Executive,Sales Executive,Sales Manager,Sales Representative,Sr.Accounts Officer,Sr.Associate Engineer,Sr.Sales Executive,Sr.Sales Representative,Store Assistant,Store Incharge,Technician")]
    public class MaritalStatusController : Controller
    {
        private ModelCMSContainer db = new ModelCMSContainer();
        MaritalStatusViewModel model = new MaritalStatusViewModel();

        public ActionResult Index()
        {
            model.MaritalStatus = db.MaritalStatus.OrderByDescending(a => a.MaritalStatusID).ToList();
            model.SelectedMaritalStatu = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc");
            ViewBag.MessageType = "";
            ViewBag.Message = "";
            return View(model);
        }

        [HttpGet]
        public ActionResult Create()
        {
            model.MaritalStatus = db.MaritalStatus.OrderByDescending(a => a.MaritalStatusID).ToList();
            model.SelectedMaritalStatu = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc");
            ViewBag.MessageType = "";
            ViewBag.Message = "";
            return View("Index", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(MaritalStatu maritalStatu)
        {
            try
            {
                MaritalStatu m = db.MaritalStatus.FirstOrDefault(de => de.MaritalStatusName == maritalStatu.MaritalStatusName);
                if (m == null)
                {
                    maritalStatu.CreatedBy = Convert.ToInt32(Session["emp_id"]);
                    maritalStatu.CreatedOn = DateTime.Now;
                    db.MaritalStatus.Add(maritalStatu);
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
                    ModelState.AddModelError(string.Empty, "Marital Status is already exists.");
                    ViewBag.MessageType = "error";
                    ViewBag.Message = "Marital Status is already exists.";
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
            model.MaritalStatus = db.MaritalStatus.OrderByDescending(a => a.MaritalStatusID).ToList();
            model.SelectedMaritalStatu = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc", maritalStatu.IsActive);

            return View("Index", model);
        }

        public ActionResult Update(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            MaritalStatu maritalStatu = db.MaritalStatus.Find(id);
            if (maritalStatu == null)
            {
                return HttpNotFound();
            }

            model.MaritalStatus = db.MaritalStatus.OrderByDescending(a => a.MaritalStatusID).ToList();
            model.SelectedMaritalStatu = maritalStatu;
            model.DisplayMode = "ReadWrite";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc", maritalStatu.IsActive);
            ViewBag.MessageType = "";
            ViewBag.Message = "";
            return View("Index", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(MaritalStatu maritalStatu)
        {
            try
            {
                db.Entry(maritalStatu).State = EntityState.Modified;
                maritalStatu.ModifiedBy = Convert.ToInt32(Session["emp_id"]);
                maritalStatu.ModifiedOn = DateTime.Now;
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
            model.MaritalStatus = db.MaritalStatus.OrderByDescending(a => a.MaritalStatusID).ToList();
            model.SelectedMaritalStatu = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc", maritalStatu.IsActive);

            return View("Index", model);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MaritalStatu maritalStatu = db.MaritalStatus.Find(id);
            if (maritalStatu == null)
            {
                return HttpNotFound();
            }

            model.MaritalStatus = db.MaritalStatus.OrderByDescending(a => a.MaritalStatusID).ToList();
            model.SelectedMaritalStatu = maritalStatu;
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
                MaritalStatu maritalStatu = db.MaritalStatus.Find(id);
                db.MaritalStatus.Remove(maritalStatu);
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
            model.MaritalStatus = db.MaritalStatus.OrderByDescending(a => a.MaritalStatusID).ToList();
            model.SelectedMaritalStatu = null;
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
