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
    public class EntryTestsController : Controller
    {
        private ModelCMSNewContainer db = new ModelCMSNewContainer();
        EntryTestViewModel model = new EntryTestViewModel();

        public ActionResult Index()
        {
            model.EntryTests = db.EntryTests.OrderBy(a => a.EntryTestID).ToList();
            model.SelectedEntryTests = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc");
            ViewBag.MessageType = "";
            ViewBag.Message = "";
            return View(model);
        }

        [HttpGet]
        public ActionResult Create()
        {
            model.EntryTests = db.EntryTests.OrderBy(a => a.EntryTestID).ToList();
            model.SelectedEntryTests = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc");
            ViewBag.MessageType = "";
            ViewBag.Message = "";
            return View("Index", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(EntryTest EntryTest)
        {
            try
            {
                EntryTest ba = db.EntryTests.FirstOrDefault(bac => bac.EntryTestName == EntryTest.EntryTestName);
                if (ba == null)
                {
                    EntryTest.CreatedBy = Convert.ToInt32(Session["emp_id"]);
                    EntryTest.CreatedOn = DateTime.Now;
                    db.EntryTests.Add(EntryTest);
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
                    ModelState.AddModelError(string.Empty, "Entry Test already exists.");
                    ViewBag.MessageType = "error";
                    ViewBag.Message = "Entry Test already exists.";
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
            model.EntryTests = db.EntryTests.OrderBy(a => a.EntryTestID).ToList();
            model.SelectedEntryTests = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc", EntryTest.IsActive);

            return View("Index", model);
        }

        public ActionResult Update(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            EntryTest EntryTest = db.EntryTests.Find(id);
            if (EntryTest == null)
            {
                return HttpNotFound();
            }

            model.EntryTests = db.EntryTests.OrderBy(a => a.EntryTestID).ToList();
            model.SelectedEntryTests = EntryTest;
            model.DisplayMode = "ReadWrite";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc", EntryTest.IsActive);
            ViewBag.MessageType = "";
            ViewBag.Message = "";
            return View("Index", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EntryTest entryTest)
        {
            try
            {
                db.Entry(entryTest).State = EntityState.Modified;
                entryTest.ModifiedBy = Convert.ToInt32(Session["emp_id"]);
                entryTest.ModifiedOn = DateTime.Now;
                entryTest.IsActive = entryTest.IsActive;
                try
                {
                    db.Entry(entryTest).State = EntityState.Modified;
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
            model.EntryTests = db.EntryTests.OrderBy(a => a.EntryTestID).ToList();
            model.SelectedEntryTests = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc", entryTest.IsActive);

            return View("Index", model);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EntryTest EntryTest = db.EntryTests.Find(id);
            if (EntryTest == null)
            {
                return HttpNotFound();
            }

            model.EntryTests = db.EntryTests.OrderBy(a => a.EntryTestID).ToList();
            model.SelectedEntryTests = EntryTest;
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
                EntryTest EntryTest = db.EntryTests.Find(id);
                db.EntryTests.Remove(EntryTest);
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
            model.EntryTests = db.EntryTests.OrderBy(a => a.EntryTestID).ToList();
            model.SelectedEntryTests = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc");

            return View("Index", model);
        }

        public ActionResult UpdateEntryTestStatus(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            int EmpID = Convert.ToInt32(Session["emp_id"]);


            db.UpdateEntryTestStatus(id, EmpID);

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
