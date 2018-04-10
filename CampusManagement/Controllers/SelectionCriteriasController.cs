using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CampusManagement.Models;
using Newtonsoft.Json;

namespace CampusManagement.Controllers
{
    [Authorize(Roles = "Account Officer,Accounts Officer,Admin Assistant,Admin Officer,Admin.Assistant,Assist. Account Officer,Assist.Technician,Import Manager,Manager Servive & Support,Office Manager,Officer QMS,RSM - Center 2,RSM - South,Sales & Service Executive,Sales Executive,Sales Manager,Sales Representative,Sr.Accounts Officer,Sr.Associate Engineer,Sr.Sales Executive,Sr.Sales Representative,Store Assistant,Store Incharge,Technician")]
    public class SelectionCriteriasController : Controller
    {
        private ModelCMSContainer db = new ModelCMSContainer();
        SelectionCriteriasViewModel model = new SelectionCriteriasViewModel();

        public ActionResult Index()
        {
            model.SelectionCriterias = db.SelectionCriterias.OrderByDescending(s => s.CriteriaID).ToList();
            model.SelectedSelectionCriteria = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc");
            ViewBag.BatchProgramID = new SelectList(db.GetBatchProgramNameConcat("", 0), "ID", "Name");
            ViewBag.DegreeID = new SelectList(db.Degrees, "DegreeID", "DegreeName");
            ViewBag.MessageType = "";
            ViewBag.Message = "";
            return View(model);
        }

        [HttpGet]
        public ActionResult Create()
        {
            model.SelectionCriterias = db.SelectionCriterias.OrderByDescending(s => s.CriteriaID).ToList();
            model.SelectedSelectionCriteria = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc");
            ViewBag.BatchProgramID = new SelectList(db.GetBatchProgramNameConcat("", 0), "ID", "Name");
            ViewBag.DegreeID = new SelectList(db.Degrees, "DegreeID", "DegreeName");

            ViewBag.MessageType = "";
            ViewBag.Message = "";
            return View("Index", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(SelectionCriteria selectionCriteria)
        {
            try
            {
                SelectionCriteria sc = db.SelectionCriterias.FirstOrDefault(s => s.BatchProgramID == selectionCriteria.BatchProgramID
                && s.DegreeID == selectionCriteria.DegreeID);
                if (sc == null)
                {
                    selectionCriteria.CreatedBy = Convert.ToInt32(Session["emp_id"]);
                    selectionCriteria.CreatedOn = DateTime.Now;
                    db.SelectionCriterias.Add(selectionCriteria);
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
                    ModelState.AddModelError(string.Empty, "Selected Batch Program is already exists against selected Degree.");
                    ViewBag.MessageType = "error";
                    ViewBag.Message = "Selected Batch Program is already exists against selected Degree.";
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
            model.SelectionCriterias = db.SelectionCriterias.OrderByDescending(s => s.CriteriaID).ToList();
            model.SelectedSelectionCriteria = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc", selectionCriteria.IsActive);
            ViewBag.BatchProgramID = new SelectList(db.GetBatchProgramNameConcat("", 0), "ID", "Name", selectionCriteria.BatchProgramID);
            ViewBag.DegreeID = new SelectList(db.Degrees, "DegreeID", "DegreeName", selectionCriteria.DegreeID);
            return View("Index", model);
        }

        public ActionResult Update(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            SelectionCriteria selectionCriteria = db.SelectionCriterias.Find(id);
            if (selectionCriteria == null)
            {
                return HttpNotFound();
            }

            model.SelectionCriterias = db.SelectionCriterias.OrderByDescending(s => s.CriteriaID).ToList();
            model.SelectedSelectionCriteria = selectionCriteria;
            model.DisplayMode = "ReadWrite";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc", selectionCriteria.IsActive);
            ViewBag.BatchProgramID = new SelectList(db.GetBatchProgramNameConcat("", 0), "ID", "Name", selectionCriteria.BatchProgramID);
            ViewBag.DegreeID = new SelectList(db.Degrees, "DegreeID", "DegreeName", selectionCriteria.DegreeID);
            ViewBag.MessageType = "";
            ViewBag.Message = "";
            return View("Index", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(SelectionCriteria selectionCriteria)
        {
            try
            {
                db.Entry(selectionCriteria).State = EntityState.Modified;
                selectionCriteria.ModifiedBy = Convert.ToInt32(Session["emp_id"]);
                selectionCriteria.ModifiedOn = DateTime.Now;
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
            model.SelectionCriterias = db.SelectionCriterias.OrderByDescending(s => s.CriteriaID).ToList();
            model.SelectedSelectionCriteria = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc", selectionCriteria.IsActive);
            ViewBag.BatchProgramID = new SelectList(db.GetBatchProgramNameConcat("", 0), "ID", "Name", selectionCriteria.BatchProgramID);
            ViewBag.DegreeID = new SelectList(db.Degrees, "DegreeID", "DegreeName", selectionCriteria.DegreeID);

            return View("Index", model);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SelectionCriteria selectionCriteria = db.SelectionCriterias.Find(id);
            if (selectionCriteria == null)
            {
                return HttpNotFound();
            }

            model.SelectionCriterias = db.SelectionCriterias.OrderByDescending(s => s.CriteriaID).ToList();
            model.SelectedSelectionCriteria = selectionCriteria;
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
                SelectionCriteria selectionCriteria = db.SelectionCriterias.Find(id);
                db.SelectionCriterias.Remove(selectionCriteria);
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
            model.SelectionCriterias = db.SelectionCriterias.OrderByDescending(s => s.CriteriaID).ToList();
            model.SelectedSelectionCriteria = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc");
            ViewBag.BatchProgramID = new SelectList(db.GetBatchProgramNameConcat("", 0), "ID", "Name");
            ViewBag.DegreeID = new SelectList(db.Degrees, "DegreeID", "DegreeName");
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
