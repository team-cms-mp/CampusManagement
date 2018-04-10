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
    public class TeachersCourseAllocationsController : Controller
    {
        private ModelCMSContainer db = new ModelCMSContainer();
        TeachersCourseAllocationsViewModel model = new TeachersCourseAllocationsViewModel();

        public ActionResult Index()
        {
            model.TeachersCourseAllocations = db.TeachersCourseAllocations.OrderByDescending(t => t.TCourseAllocationID).ToList();
            model.SelectedTeachersCourseAllocation = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc");
            ViewBag.ProgramCourseID = new SelectList(db.GetBatchProgramNameConcat("", 1), "ID", "Name");
            ViewBag.TeacherID = new SelectList(db.Teachers, "TeacherID", "TeacherName");
            ViewBag.MessageType = "";
            ViewBag.Message = "";
            return View(model);
        }

        [HttpGet]
        public ActionResult Create()
        {
            model.TeachersCourseAllocations = db.TeachersCourseAllocations.OrderByDescending(t => t.TCourseAllocationID).ToList();
            model.SelectedTeachersCourseAllocation = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc");
            ViewBag.ProgramCourseID = new SelectList(db.GetBatchProgramNameConcat("", 1), "ID", "Name");
            ViewBag.TeacherID = new SelectList(db.Teachers, "TeacherID", "TeacherName");
            ViewBag.MessageType = "";
            ViewBag.Message = "";
            return View("Index", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(TeachersCourseAllocation teachersCourseAllocation)
        {
            string ErrorMessage = "";
            int count = 0;
            try
            {
                TeachersCourseAllocation tc = db.TeachersCourseAllocations.FirstOrDefault(
                    p => p.ProgramCourseID == teachersCourseAllocation.ProgramCourseID
                    && p.TeacherID == teachersCourseAllocation.TeacherID);

                if (tc != null)
                {
                    ViewBag.MessageType = "error";
                    ViewBag.Message = "Selected Teacher Course is already exists.";
                    ModelState.AddModelError(string.Empty, "Selected Teacher Course is already exists.");
                }
                else
                {
                    teachersCourseAllocation.CreatedBy = Convert.ToInt32(Session["emp_id"]);
                    teachersCourseAllocation.CreatedOn = DateTime.Now;
                    db.TeachersCourseAllocations.Add(teachersCourseAllocation);
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
            model.TeachersCourseAllocations = db.TeachersCourseAllocations.OrderByDescending(t => t.TCourseAllocationID).ToList();
            model.SelectedTeachersCourseAllocation = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc", teachersCourseAllocation.IsActive);
            ViewBag.ProgramCourseID = new SelectList(db.GetBatchProgramNameConcat("", 1), "ID", "Name", teachersCourseAllocation.ProgramCourseID);
            ViewBag.TeacherID = new SelectList(db.Teachers, "TeacherID", "TeacherName", teachersCourseAllocation.TeacherID);
            return View("Index", model);
        }

        public ActionResult Update(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            TeachersCourseAllocation teachersCourseAllocation = db.TeachersCourseAllocations.Find(id);
            if (teachersCourseAllocation == null)
            {
                return HttpNotFound();
            }

            model.TeachersCourseAllocations = db.TeachersCourseAllocations.OrderByDescending(t => t.TCourseAllocationID).ToList();
            model.SelectedTeachersCourseAllocation = teachersCourseAllocation;
            model.DisplayMode = "ReadWrite";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc", teachersCourseAllocation.IsActive);
            ViewBag.ProgramCourseID = new SelectList(db.GetBatchProgramNameConcat("", 1), "ID", "Name", teachersCourseAllocation.ProgramCourseID);
            ViewBag.TeacherID = new SelectList(db.Teachers, "TeacherID", "TeacherName", teachersCourseAllocation.TeacherID);
            ViewBag.MessageType = "";
            ViewBag.Message = "";
            return View("Index", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(TeachersCourseAllocation teachersCourseAllocation)
        {
            try
            {
                db.Entry(teachersCourseAllocation).State = EntityState.Modified;
                teachersCourseAllocation.ModifiedBy = Convert.ToInt32(Session["emp_id"]);
                teachersCourseAllocation.ModifiedOn = DateTime.Now;
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
            model.TeachersCourseAllocations = db.TeachersCourseAllocations.OrderByDescending(t => t.TCourseAllocationID).ToList();
            model.SelectedTeachersCourseAllocation = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc", teachersCourseAllocation.IsActive);
            ViewBag.ProgramCourseID = new SelectList(db.GetBatchProgramNameConcat("", 1), "ID", "Name", teachersCourseAllocation.ProgramCourseID);
            ViewBag.TeacherID = new SelectList(db.Teachers, "TeacherID", "TeacherName", teachersCourseAllocation.TeacherID);
            return View("Index", model);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TeachersCourseAllocation teachersCourseAllocation = db.TeachersCourseAllocations.Find(id);
            if (teachersCourseAllocation == null)
            {
                return HttpNotFound();
            }

            model.TeachersCourseAllocations = db.TeachersCourseAllocations.OrderByDescending(t => t.TCourseAllocationID).ToList();
            model.SelectedTeachersCourseAllocation = teachersCourseAllocation;
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
                TeachersCourseAllocation teachersCourseAllocation = db.TeachersCourseAllocations.Find(id);
                db.TeachersCourseAllocations.Remove(teachersCourseAllocation);
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
            model.TeachersCourseAllocations = db.TeachersCourseAllocations.OrderByDescending(t => t.TCourseAllocationID).ToList();
            model.SelectedTeachersCourseAllocation = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc");
            ViewBag.ProgramCourseID = new SelectList(db.GetBatchProgramNameConcat("", 1), "ID", "Name");
            ViewBag.TeacherID = new SelectList(db.Teachers, "TeacherID", "TeacherName");
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
