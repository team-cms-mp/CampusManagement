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
    public class BatchProgramsController : Controller
    {
        private ModelCMSContainer db = new ModelCMSContainer();
        BatchProgramViewModel model = new BatchProgramViewModel();

        public ActionResult Index()
        {
            model.BatchPrograms = db.BatchPrograms.OrderByDescending(a => a.BatchProgramID).ToList();
            model.SelectedBatchProgram = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc");
            ViewBag.BatchID = new SelectList(db.Batches, "BatchID", "BatchName");
            ViewBag.ProgramID = new SelectList(db.Programs, "ProgramID", "ProgramName");
            ViewBag.MessageType = "";
            ViewBag.Message = "";
            return View(model);
        }

        [HttpGet]
        public ActionResult Create()
        {
            model.BatchPrograms = db.BatchPrograms.OrderByDescending(a => a.BatchProgramID).ToList();
            model.SelectedBatchProgram = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc");
            ViewBag.BatchID = new SelectList(db.Batches, "BatchID", "BatchName");
            ViewBag.ProgramID = new SelectList(db.Programs, "ProgramID", "ProgramName");
            ViewBag.MessageType = "";
            ViewBag.Message = "";
            return View("Index", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(BatchProgram batchprogram)
        {
            string ErrorMessage = "";
            int count = 0;
            try
            {
                BatchProgram bp = db.BatchPrograms.FirstOrDefault(
                    b => b.ProgramID == batchprogram.ProgramID
                    && b.BatchID == batchprogram.BatchID);

                if (bp != null)
                {
                    ViewBag.MessageType = "error";
                    ViewBag.Message = "Selected Program and Batch are already exist.";
                    ModelState.AddModelError(string.Empty, "Selected Program and Batch are already exist.");
                }
                else
                {
                    batchprogram.CreatedBy = Convert.ToInt32(Session["emp_id"]);
                    batchprogram.CreatedOn = DateTime.Now;
                    db.BatchPrograms.Add(batchprogram);
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
            model.BatchPrograms = db.BatchPrograms.OrderByDescending(a => a.BatchProgramID).ToList();
            model.SelectedBatchProgram = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc", batchprogram.IsActive);
            ViewBag.BatchID = new SelectList(db.Batches, "BatchID", "BatchName", batchprogram.BatchID);
            ViewBag.ProgramID = new SelectList(db.Programs, "ProgramID", "ProgramName", batchprogram.ProgramID);
            return View("Index", model);
        }

        public ActionResult Update(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            BatchProgram batchprogram = db.BatchPrograms.Find(id);
            if (batchprogram == null)
            {
                return HttpNotFound();
            }

            model.BatchPrograms = db.BatchPrograms.OrderByDescending(a => a.BatchProgramID).ToList();
            model.SelectedBatchProgram = batchprogram;
            model.DisplayMode = "ReadWrite";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc", batchprogram.IsActive);
            ViewBag.BatchID = new SelectList(db.Batches, "BatchID", "BatchName", batchprogram.BatchID);
            ViewBag.ProgramID = new SelectList(db.Programs, "ProgramID", "ProgramName", batchprogram.ProgramID);
            ViewBag.MessageType = "";
            ViewBag.Message = "";
            return View("Index", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(BatchProgram batchprogram)
        {
            try
            {
                db.Entry(batchprogram).State = EntityState.Modified;
                batchprogram.ModifiedBy = Convert.ToInt32(Session["emp_id"]);
                batchprogram.ModifiedOn = DateTime.Now;
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
            model.BatchPrograms = db.BatchPrograms.OrderByDescending(a => a.BatchProgramID).ToList();
            model.SelectedBatchProgram = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc", batchprogram.IsActive);
            ViewBag.BatchID = new SelectList(db.Batches, "BatchID", "BatchName", batchprogram.BatchID);
            ViewBag.ProgramID = new SelectList(db.Programs, "ProgramID", "ProgramName", batchprogram.ProgramID);
            return View("Index", model);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BatchProgram batchprogram = db.BatchPrograms.Find(id);
            if (batchprogram == null)
            {
                return HttpNotFound();
            }

            model.BatchPrograms = db.BatchPrograms.OrderByDescending(a => a.BatchProgramID).ToList();
            model.SelectedBatchProgram = batchprogram;
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
                BatchProgram batchprogram = db.BatchPrograms.Find(id);
                db.BatchPrograms.Remove(batchprogram);
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
            model.BatchPrograms = db.BatchPrograms.OrderByDescending(a => a.BatchProgramID).ToList();
            model.SelectedBatchProgram = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc");
            ViewBag.BatchID = new SelectList(db.Batches, "BatchID", "BatchName");
            ViewBag.ProgramID = new SelectList(db.Programs, "ProgramID", "ProgramName");
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
