﻿using CampusManagement.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace CampusManagement.Controllers
{
    public class LmsOfTeacherController : Controller
    {
        int count = 0;
        string ErrorMessage = "";
        private ModelCMSNewContainer db = new ModelCMSNewContainer();
        LMSSubjectLearningViewModel model = new LMSSubjectLearningViewModel();

        public ActionResult ActiveTeacherCourses()
        {
            ViewBag.BatchID = new SelectList(db.Batches, "BatchID", "BatchName");
            ViewBag.BatchProgramID = new SelectList(db.GetBatchProgramNameConcat("", 0).ToList(), "ID", "Name");
            ViewBag.YearSemesterNo = new SelectList(db.Semesters, "YearSemesterNo", "YearSemesterNo");
            ViewBag.MessageType = "";
            ViewBag.Message = "";
            return View();
        }

        [HttpPost]
        public ActionResult ActiveTeacherCourses(StudentAttandance sa)
        {
            string ErrorMessage = "";
            int count = 0;
            ViewBag.hdnBatchProgramID = sa.BatchProgramID;
            ViewBag.hdnYearSemesterNo = sa.YearSemesterNo;
            ViewBag.hdnBatchID = sa.BatchID;
            if (sa.BatchProgramID == 0)
            {
                count++;
                ErrorMessage += count + "-Please select program.<br/>";
            }
            if (sa.YearSemesterNo == 0)
            {
                count++;
                ErrorMessage += count + "-Please select semester.<br/>";
            }

            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ViewBag.MessageType = "error";
                ViewBag.Message = ErrorMessage;
            }
            else
            {

                ViewBag.MessageType = "";
                ViewBag.Message = "";
            }

            ViewBag.BatchID = new SelectList(db.Batches, "BatchID", "BatchName");
            ViewBag.BatchProgramID = new SelectList(db.GetBatchProgramNameConcat("", 0), "ID", "Name");
            ViewBag.YearSemesterNo = new SelectList(db.Semesters, "YearSemesterNo", "YearSemesterNo");
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc");
            return View();
        }

        public ActionResult Index(int? ProgramCourseID)
        {
            model.LMSSubjectLearnings = db.LMSSubjectLearnings.Where(x => x.ProgramCourseID == ProgramCourseID).OrderByDescending(a => a.LMSSubjectLearningID).ToList();
            model.SelectedLMSSubjectLearning = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc");
            ViewBag.MessageType = "";
            ViewBag.ProgramCourseID = ProgramCourseID;
            ViewBag.Message = "";
            return View(model);
        }

        [HttpGet]
        public ActionResult Create(int? ProgramCourseID)
        {
            model.LMSSubjectLearnings = db.LMSSubjectLearnings.Where(x => x.ProgramCourseID == ProgramCourseID).OrderByDescending(a => a.LMSSubjectLearningID).ToList();
            model.SelectedLMSSubjectLearning = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.ProgramCourseID = ProgramCourseID;
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc");
            ViewBag.MessageType = "";
            ViewBag.Message = "";
            return View("Index", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(LMSSubjectLearning LMS)
        {
            try
            {
                LMSSubjectLearning d = db.LMSSubjectLearnings.FirstOrDefault(de => de.Title == LMS.Title);

                if (d == null)
                {
                    try
                    {
                        LMS.FilePath = string.Concat("~/DegreeDocument/", DateTime.Now.Ticks, "_", LMS.UploadFiles.FileName.Replace(" ", ""));
                        LMS.UploadFiles.SaveAs(Server.MapPath(LMS.FilePath));
                    }
                    catch (Exception)
                    {
                        ModelState.AddModelError(string.Empty, "-Please attach the document.");
                        count++;
                        ErrorMessage += count + "-Please attach the document.<br />";
                    }

                    LMS.CreatedBy = Convert.ToInt32(Session["emp_id"]);
                    LMS.CreatedOn = DateTime.Now;
                    db.LMSSubjectLearnings.Add(LMS);
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
                    ModelState.AddModelError(string.Empty, "Name already exists.");
                    ViewBag.MessageType = "error";
                    ViewBag.Message = "Name already exists.";
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
            model.LMSSubjectLearnings = db.LMSSubjectLearnings.Where(x => x.ProgramCourseID == LMS.ProgramCourseID).OrderByDescending(a => a.LMSSubjectLearningID).ToList();
            model.SelectedLMSSubjectLearning = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc", LMS.IsActive);

            return View("Index", model);
        }

        public ActionResult Update(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            LMSSubjectLearning LMS = db.LMSSubjectLearnings.Find(id);
            if (LMS == null)
            {
                return HttpNotFound();
            }

            model.LMSSubjectLearnings = db.LMSSubjectLearnings.Where(x => x.ProgramCourseID == LMS.ProgramCourseID).OrderByDescending(a => a.LMSSubjectLearningID).ToList();
            model.SelectedLMSSubjectLearning = LMS;
            model.DisplayMode = "ReadWrite";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc", LMS.IsActive);
            ViewBag.ProgramCourseID = LMS.ProgramCourseID;
            ViewBag.MessageType = "";
            ViewBag.Message = "";
            return View("Index", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(LMSSubjectLearning LMS)
        {
            try
            {
                try
                {
                    if (LMS.UploadFiles != null)
                    {
                        LMS.FilePath = string.Concat("~/DegreeDocument/", DateTime.Now.Ticks, "_", LMS.UploadFiles.FileName.Replace(" ", ""));
                        LMS.UploadFiles.SaveAs(Server.MapPath(LMS.FilePath));
                    }
                }
                catch (Exception)
                {
                    ModelState.AddModelError(string.Empty, "-Please attach the document.");
                    count++;
                    ErrorMessage += count + "-Please attach the document.<br />";
                }
                db.Entry(LMS).State = EntityState.Modified;
                LMS.ModifiedBy = Convert.ToInt32(Session["emp_id"]);
                LMS.ModifiedOn = DateTime.Now;
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
            model.LMSSubjectLearnings = db.LMSSubjectLearnings.Where(x => x.ProgramCourseID == LMS.ProgramCourseID).OrderByDescending(a => a.LMSSubjectLearningID).ToList();
            model.SelectedLMSSubjectLearning = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc", LMS.IsActive);

            return View("Index", model);
        }



        public ActionResult CreateWeeksIndex(int? LMSSubjectLearningID)
        {
            LMSWeekViewModel model = new LMSWeekViewModel();
            model.LMSWeeks = db.LMSWeeks.Where(x => x.LMSSubjectLearningID == LMSSubjectLearningID).OrderByDescending(a => a.LMSSubjectLearningID).ToList();
            model.SelectedLMSWeeks = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc");
            ViewBag.MessageType = "";
            ViewBag.LMSSubjectLearningID = LMSSubjectLearningID;
            ViewBag.Message = "";
            return View(model);
        }
        public ActionResult CreateWeeks(int? LMSSubjectLearningID)
        {
            LMSWeekViewModel model = new LMSWeekViewModel();
            model.LMSWeeks = db.LMSWeeks.Where(x => x.LMSSubjectLearningID == LMSSubjectLearningID).OrderByDescending(a => a.LMSSubjectLearningID).ToList();
            model.SelectedLMSWeeks = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc");
            ViewBag.LMSSubjectLearningID = LMSSubjectLearningID;
            ViewBag.MessageType = "";
            ViewBag.Message = "";
            return View("CreateWeeksIndex", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateWeeks(LMSWeek LMS)
        {
            LMSWeekViewModel model = new LMSWeekViewModel();
            try
            {
                LMSWeek d = db.LMSWeeks.FirstOrDefault(de => de.Title == LMS.Title && de.LMSSubjectLearningID == LMS.LMSSubjectLearningID);
                if (d == null)
                {
                    LMS.CreatedBy = Convert.ToInt32(Session["emp_id"]);
                    LMS.CreatedOn = DateTime.Now;
                    db.LMSWeeks.Add(LMS);
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
                    ModelState.AddModelError(string.Empty, "Name already exists.");
                    ViewBag.MessageType = "error";
                    ViewBag.Message = "Name already exists.";
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
            model.LMSWeeks = db.LMSWeeks.Where(x => x.LMSSubjectLearningID == LMS.LMSSubjectLearningID).OrderByDescending(a => a.LMSWeekID).ToList();
            model.SelectedLMSWeeks = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc", LMS.IsActive);

            return View("CreateWeeksIndex", model);
        }

        public ActionResult UpdateWeeks(int? id)
        {
            LMSWeekViewModel model = new LMSWeekViewModel();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            LMSWeek LMS = db.LMSWeeks.Find(id);
            if (LMS == null)
            {
                return HttpNotFound();
            }

            model.LMSWeeks = db.LMSWeeks.Where(x => x.LMSSubjectLearningID == LMS.LMSSubjectLearningID).OrderByDescending(a => a.LMSWeekID).ToList();
            model.SelectedLMSWeeks = LMS;
            model.DisplayMode = "ReadWrite";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc", LMS.IsActive);
            ViewBag.LMSSubjectLearningID = LMS.LMSSubjectLearningID;
            ViewBag.MessageType = "";
            ViewBag.Message = "";
            return View("CreateWeeksIndex", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditWeeks(LMSWeek LMS)
        {
            LMSWeekViewModel model = new LMSWeekViewModel();
            try
            {
                db.Entry(LMS).State = EntityState.Modified;
                LMS.ModifiedBy = Convert.ToInt32(Session["emp_id"]);
                LMS.ModifiedOn = DateTime.Now;
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
            model.LMSWeeks = db.LMSWeeks.Where(x => x.LMSSubjectLearningID == LMS.LMSSubjectLearningID).OrderByDescending(a => a.LMSWeekID).ToList();
            model.SelectedLMSWeeks = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc", LMS.IsActive);

            return View("CreateWeeksIndex", model);
        }

        public JsonResult GetPrograms_by_FacultyLevelBatch(string FacultyID, string BatchID)
        {
            List<GetPrograms_by_FacultyLevelBatch_Result> lstPrograms = new List<GetPrograms_by_FacultyLevelBatch_Result>();

            lstPrograms = db.GetPrograms_by_FacultyLevelBatch(Convert.ToInt32(FacultyID), 0, Convert.ToInt32(BatchID), 0).ToList();
            var programs = lstPrograms.Select(p => new
            {
                BatchProgramID = p.BatchProgramID,
                ProgramName = p.ProgramName
            });
            string result = JsonConvert.SerializeObject(programs, Formatting.Indented);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetBatchProgramSemesterList(string BatchProgramID)
        {
            List<BatchProgramSemester> lstSemester = new List<BatchProgramSemester>();
            int bpId = Convert.ToInt32(BatchProgramID);

            lstSemester = db.BatchProgramSemesters.Where(s => s.BatchProgramID == bpId).ToList();
            var semesters = lstSemester.Select(S => new
            {
                YearSemesterNo = S.YearSemesterNo
            });
            string result = JsonConvert.SerializeObject(semesters, Formatting.Indented);
            return Json(result, JsonRequestBehavior.AllowGet);
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