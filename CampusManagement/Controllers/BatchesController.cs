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
    public class BatchesController : Controller
    {
        private ModelCMSNewContainer db = new ModelCMSNewContainer();
        BatchViewModel model = new BatchViewModel();

        public ActionResult Index()
        {
            model.Batches = db.Batches.OrderByDescending(a => a.BatchID).ToList();
            model.SelectedBatch = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc");
            ViewBag.MessageType = "";
            ViewBag.Message = "";
            return View(model);
        }

        [HttpGet]
        public ActionResult ActiveSession()
        {
            model.Batches = db.Batches.OrderByDescending(a => a.BatchID).ToList();
            model.SelectedBatch = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc");
            ViewBag.MessageType = "";
            ViewBag.Message = "";
            return View(model);
        }

        public ActionResult ActiveSessionByBatchID(int BatchID)
        {
            int EmpID = Convert.ToInt32(Session["emp_id"]);
            db.UpdateActiveSession(BatchID, EmpID);
            // Update Request For Approve Here

            //model.Batches = db.Batches.OrderByDescending(a => a.BatchID).ToList();
            //model.SelectedBatch = null;
            //model.DisplayMode = "WriteOnly";
            //ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc");
            ViewBag.MessageType = "";
            ViewBag.Message = "";
            return RedirectToAction("ActiveSession");
        }



        [HttpGet]
        public ActionResult Create()
        {
            model.Batches = db.Batches.OrderByDescending(a => a.BatchID).ToList();
            model.SelectedBatch = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc");
            ViewBag.MessageType = "";
            ViewBag.Message = "";
            return View("Index", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Batch batch)
        {
            string ErrorMessage = "";
            int count = 0;
            try
            {
                Batch ba = new Batch();
                ba = db.Batches.FirstOrDefault(bac => bac.BatchName == batch.BatchName);
                if (ba != null)
                {
                    ModelState.AddModelError(string.Empty, "Session Name already exists.");
                    count++;
                    ErrorMessage += count + "-" + "Session Name already exists." + "<br />";
                }

                ba = db.Batches.FirstOrDefault(bac => bac.BatchCode == batch.BatchCode);
                if (ba != null)
                {
                    ModelState.AddModelError(string.Empty, "Session Code already exists.");
                    count++;
                    ErrorMessage += count + "-" + "Session Code already exists." + "<br />";
                }

                if (!string.IsNullOrEmpty(ErrorMessage))
                {
                    ViewBag.MessageType = "error";
                    ViewBag.Message = ErrorMessage;
                }

                if (ba == null)
                {
                    batch.CreatedBy = Convert.ToInt32(Session["emp_id"]);
                    batch.CreatedOn = DateTime.Now;
                    db.Batches.Add(batch);
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
            }
            catch (DbEntityValidationException ex)
            {
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
            model.Batches = db.Batches.OrderByDescending(a => a.BatchID).ToList();
            model.SelectedBatch = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc", batch.IsActive);

            return View("Index", model);
        }

        public ActionResult Update(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Batch batch = db.Batches.Find(id);
            if (batch == null)
            {
                return HttpNotFound();
            }

            model.Batches = db.Batches.OrderByDescending(a => a.BatchID).ToList();
            model.SelectedBatch = batch;
            model.DisplayMode = "ReadWrite";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc", batch.IsActive);
            ViewBag.MessageType = "";
            ViewBag.Message = "";
            return View("Index", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Batch batch)
        {
            try
            {
                db.Entry(batch).State = EntityState.Modified;
                batch.ModifiedBy = Convert.ToInt32(Session["emp_id"]);
                batch.ModifiedOn = DateTime.Now;
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
            model.Batches = db.Batches.OrderByDescending(a => a.BatchID).ToList();
            model.SelectedBatch = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc", batch.IsActive);

            return View("Index", model);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Batch batch = db.Batches.Find(id);
            if (batch == null)
            {
                return HttpNotFound();
            }

            model.Batches = db.Batches.OrderByDescending(a => a.BatchID).ToList();
            model.SelectedBatch = batch;
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
                Batch batch = db.Batches.Find(id);
                db.Batches.Remove(batch);
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
            model.Batches = db.Batches.OrderByDescending(a => a.BatchID).ToList();
            model.SelectedBatch = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc");

            return View("Index", model);
        }

        public ActionResult SetCurrentSession(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Batch batch = db.Batches.Find(id);
            if (batch == null)
            {
                return HttpNotFound();
            }

            db.UpdateActiveSession(id, Convert.ToInt32(Session["emp_id"])); //Update to Current Session

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
