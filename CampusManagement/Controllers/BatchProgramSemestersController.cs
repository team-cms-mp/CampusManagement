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
    public class BatchProgramSemestersController : Controller
    {
        private ModelCMSContainer db = new ModelCMSContainer();
        BatchProgramSemesterViewModel model = new BatchProgramSemesterViewModel();

        public ActionResult Index()
        {
            model.BatchProgramSemesters = db.BatchProgramSemesters.OrderByDescending(a=>a.BatchProgramSemesterID).ToList();
            model.SelectedBatchProgramSemester = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc");
            ViewBag.BatchProgramID = new SelectList(db.GetBatchProgramNameConcat("", 0).ToList(), "ID", "Name");
            ViewBag.MessageType = "";
            ViewBag.Message = "";
            return View(model);
        }

        [HttpGet]
        public ActionResult Create()
        {
            model.BatchProgramSemesters = db.BatchProgramSemesters.OrderByDescending(a=>a.BatchProgramSemesterID).ToList();
            model.SelectedBatchProgramSemester = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc");
            ViewBag.BatchProgramID = new SelectList(db.GetBatchProgramNameConcat("", 0).ToList(), "ID", "Name");
            ViewBag.MessageType = "";
            ViewBag.Message = "";
            return View("Index", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(BatchProgramSemester batchProgramSemester)
        {
            try
            {
                BatchProgramSemester bps = db.BatchProgramSemesters.FirstOrDefault(
                    b => b.BatchProgramID == batchProgramSemester.BatchProgramID
                    && b.YearSemesterNo == batchProgramSemester.YearSemesterNo);

                if (bps == null)
                {
                    batchProgramSemester.CreatedBy = Convert.ToInt32(Session["emp_id"]);
                    batchProgramSemester.CreatedOn = DateTime.Now;
                    db.BatchProgramSemesters.Add(batchProgramSemester);
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
                    ModelState.AddModelError(string.Empty, "Selected Batch Program is already exist against the same Year/Semester #.");
                    ViewBag.MessageType = "error";
                    ViewBag.Message = "Selected Batch Program is already exist against the same Year/Semester #.";
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
            model.BatchProgramSemesters = db.BatchProgramSemesters.OrderByDescending(a=>a.BatchProgramSemesterID).ToList();
            model.SelectedBatchProgramSemester = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc", batchProgramSemester.IsActive);
            ViewBag.BatchProgramID = new SelectList(db.GetBatchProgramNameConcat("", 0).ToList(), "ID", "Name", batchProgramSemester.BatchProgramID);
            return View("Index", model);
        }

        public ActionResult Update(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            BatchProgramSemester batchProgramSemester = db.BatchProgramSemesters.Find(id);
            if (batchProgramSemester == null)
            {
                return HttpNotFound();
            }

            model.BatchProgramSemesters = db.BatchProgramSemesters.OrderByDescending(a=>a.BatchProgramSemesterID).ToList();
            model.SelectedBatchProgramSemester = batchProgramSemester;
            model.DisplayMode = "ReadWrite";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc", batchProgramSemester.IsActive);
            ViewBag.BatchProgramID = new SelectList(db.GetBatchProgramNameConcat("", 0).ToList(), "ID", "Name", batchProgramSemester.BatchProgramID);
            ViewBag.MessageType = "";
            ViewBag.Message = "";
            return View("Index", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(BatchProgramSemester batchProgramSemester)
        {
            try
            {
                db.Entry(batchProgramSemester).State = EntityState.Modified;
                batchProgramSemester.ModifiedBy = Convert.ToInt32(Session["emp_id"]);
                batchProgramSemester.ModifiedOn = DateTime.Now;
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
            model.BatchProgramSemesters = db.BatchProgramSemesters.OrderByDescending(a=>a.BatchProgramSemesterID).ToList();
            model.SelectedBatchProgramSemester = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc", batchProgramSemester.IsActive);
            ViewBag.BatchProgramID = new SelectList(db.GetBatchProgramNameConcat("", 0).ToList(), "ID", "Name", batchProgramSemester.BatchProgramID);
            return View("Index", model);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BatchProgramSemester batchProgramSemester = db.BatchProgramSemesters.Find(id);
            if (batchProgramSemester == null)
            {
                return HttpNotFound();
            }

            model.BatchProgramSemesters = db.BatchProgramSemesters.OrderByDescending(a=>a.BatchProgramSemesterID).ToList();
            model.SelectedBatchProgramSemester = batchProgramSemester;
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
                BatchProgramSemester batchProgramSemester = db.BatchProgramSemesters.Find(id);
                db.BatchProgramSemesters.Remove(batchProgramSemester);
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
            model.BatchProgramSemesters = db.BatchProgramSemesters.OrderByDescending(a=>a.BatchProgramSemesterID).ToList();
            model.SelectedBatchProgramSemester = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc");
            ViewBag.BatchProgramID = new SelectList(db.GetBatchProgramNameConcat("", 0).ToList(), "ID", "Name");
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
