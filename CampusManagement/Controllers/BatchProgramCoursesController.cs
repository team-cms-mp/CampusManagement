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
    public class BatchProgramCoursesController : Controller
    {
        private ModelCMSContainer db = new ModelCMSContainer();
        BatchProgramCourseViewModel model = new BatchProgramCourseViewModel();

        public ActionResult Index()
        {
            model.BatchProgramCourses = db.BatchProgramCourses.OrderByDescending(a => a.ProgramCourseID).ToList();
            model.SelectedBatchProgramCourse = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc");
            ViewBag.CourseID = new SelectList(db.Courses, "CourseID", "CourseName");
            ViewBag.BatchProgramID = new SelectList(db.GetBatchProgramNameConcat("", 0).ToList(), "ID", "Name");
            ViewBag.CourseTypeID = new SelectList(db.CourseTypes, "CourseTypeID", "CourseTypeName");
            ViewBag.MessageType = "";
            ViewBag.Message = "";
            return View(model);
        }

        [HttpGet]
        public ActionResult Create()
        {
            model.BatchProgramCourses = db.BatchProgramCourses.OrderByDescending(a => a.ProgramCourseID).ToList();
            model.SelectedBatchProgramCourse = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc");
            ViewBag.CourseID = new SelectList(db.Courses, "CourseID", "CourseName");
            ViewBag.BatchProgramID = new SelectList(db.GetBatchProgramNameConcat("", 0).ToList(), "ID", "Name");
            ViewBag.CourseTypeID = new SelectList(db.CourseTypes, "CourseTypeID", "CourseTypeName");
            ViewBag.MessageType = "";
            ViewBag.Message = "";
            return View("Index", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(BatchProgramCourse batchProgramCourse)
        {
            string ErrorMessage = "";
            int count = 0;
            try
            {
                if (batchProgramCourse.YearSemesterNo == 0)
                {
                    ViewBag.MessageType = "error";
                    ViewBag.Message = "Year/Semester # should not be 0.";
                    ModelState.AddModelError(string.Empty, "Year/Semester # should not be 0.");
                }
                else
                { 
                    BatchProgramCourse pc = db.BatchProgramCourses.FirstOrDefault(
                        p => p.BatchProgramID == batchProgramCourse.BatchProgramID
                        && p.CourseID == batchProgramCourse.CourseID
                        && p.YearSemesterNo == batchProgramCourse.YearSemesterNo);

                    if (pc != null)
                    {
                        ViewBag.MessageType = "error";
                        ViewBag.Message = "Selected Course is already exists against the same Program and Year/Semester #.";
                        ModelState.AddModelError(string.Empty, "Selected Course is already exists against the same Program and Year/Semester #.");
                    }
                    else
                    {
                        batchProgramCourse.CreatedBy = Convert.ToInt32(Session["emp_id"]);
                        batchProgramCourse.CreatedOn = DateTime.Now;
                        db.BatchProgramCourses.Add(batchProgramCourse);
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
            model.BatchProgramCourses = db.BatchProgramCourses.OrderByDescending(a => a.ProgramCourseID).ToList();
            model.SelectedBatchProgramCourse = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc", batchProgramCourse.IsActive);
            ViewBag.CourseID = new SelectList(db.Courses, "CourseID", "CourseName", batchProgramCourse.CourseID);
            ViewBag.BatchProgramID = new SelectList(db.GetBatchProgramNameConcat("", 0).ToList(), "ID", "Name", batchProgramCourse.BatchProgramID);
            ViewBag.CourseTypeID = new SelectList(db.CourseTypes, "CourseTypeID", "CourseTypeName", batchProgramCourse.CourseTypeID);
            return View("Index", model);
        }

        public ActionResult Update(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            BatchProgramCourse batchProgramCourse = db.BatchProgramCourses.Find(id);
            if (batchProgramCourse == null)
            {
                return HttpNotFound();
            }

            model.BatchProgramCourses = db.BatchProgramCourses.OrderByDescending(a => a.ProgramCourseID).ToList();
            model.SelectedBatchProgramCourse = batchProgramCourse;
            model.DisplayMode = "ReadWrite";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc", batchProgramCourse.IsActive);
            ViewBag.CourseID = new SelectList(db.Courses, "CourseID", "CourseName", batchProgramCourse.CourseID);
            ViewBag.BatchProgramID = new SelectList(db.GetBatchProgramNameConcat("", 0).ToList(), "ID", "Name", batchProgramCourse.BatchProgramID);
            ViewBag.CourseTypeID = new SelectList(db.CourseTypes, "CourseTypeID", "CourseTypeName", batchProgramCourse.CourseTypeID);
            ViewBag.MessageType = "";
            ViewBag.Message = "";
            return View("Index", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(BatchProgramCourse batchProgramCourse)
        {
            try
            {
                if (batchProgramCourse.YearSemesterNo == 0)
                {
                    ViewBag.MessageType = "error";
                    ViewBag.Message = "Year/Semester # should not be 0.";
                    ModelState.AddModelError(string.Empty, "Year/Semester # should not be 0.");
                }
                else
                {
                    db.Entry(batchProgramCourse).State = EntityState.Modified;
                    batchProgramCourse.ModifiedBy = Convert.ToInt32(Session["emp_id"]);
                    batchProgramCourse.ModifiedOn = DateTime.Now;
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
            model.BatchProgramCourses = db.BatchProgramCourses.OrderByDescending(a => a.ProgramCourseID).ToList();
            model.SelectedBatchProgramCourse = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc", batchProgramCourse.IsActive);
            ViewBag.CourseID = new SelectList(db.Courses, "CourseID", "CourseName", batchProgramCourse.CourseID);
            ViewBag.BatchProgramID = new SelectList(db.GetBatchProgramNameConcat("", 0).ToList(), "ID", "Name", batchProgramCourse.BatchProgramID);
            ViewBag.CourseTypeID = new SelectList(db.CourseTypes, "CourseTypeID", "CourseTypeName", batchProgramCourse.CourseTypeID);
            return View("Index", model);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BatchProgramCourse batchProgramCourse = db.BatchProgramCourses.Find(id);
            if (batchProgramCourse == null)
            {
                return HttpNotFound();
            }

            model.BatchProgramCourses = db.BatchProgramCourses.OrderByDescending(a => a.ProgramCourseID).ToList();
            model.SelectedBatchProgramCourse = batchProgramCourse;
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
                BatchProgramCourse batchProgramCourse = db.BatchProgramCourses.Find(id);
                db.BatchProgramCourses.Remove(batchProgramCourse);
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
            model.BatchProgramCourses = db.BatchProgramCourses.OrderByDescending(a => a.ProgramCourseID).ToList();
            model.SelectedBatchProgramCourse = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc");
            ViewBag.CourseID = new SelectList(db.Courses, "CourseID", "CourseName");
            ViewBag.BatchProgramID = new SelectList(db.GetBatchProgramNameConcat("", 0).ToList(), "ID", "Name");
            ViewBag.CourseTypeID = new SelectList(db.CourseTypes, "CourseTypeID", "CourseTypeName");
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
