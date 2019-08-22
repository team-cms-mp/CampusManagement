using CampusManagement.Models;
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
    public class LmsTeacherWeekDetailsController : Controller
    {
        int count = 0;
        string ErrorMessage = "";
        private ModelCMSNewContainer db = new ModelCMSNewContainer();
        LMSWeekDetailsViewModel model = new LMSWeekDetailsViewModel();

        public ActionResult Index(int? LMSWeekID)
        {
            model.LMSWeekDetails = db.LMSWeekDetails.Where(x => x.LMSWeekID == LMSWeekID).OrderByDescending(a => a.LMSWeekDetailID).ToList();
            model.SelectedLMSWeekDetails = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc");
            ViewBag.WeekListTypeID = new SelectList(db.LMSWeekListTypes, "WeekListTypeID", "WeekListTypeName");
            ViewBag.MessageType = "";
            ViewBag.LMSWeekID = LMSWeekID;
            ViewBag.Message = "";
            return View(model);
        }
        
        [HttpGet]
        public ActionResult Create(int? LMSWeekID)
        {
            model.LMSWeekDetails = db.LMSWeekDetails.Where(x => x.LMSWeekID == LMSWeekID).OrderByDescending(a => a.LMSWeekDetailID).ToList();
            model.SelectedLMSWeekDetails = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.LMSWeekID = LMSWeekID;
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc");
            ViewBag.WeekListTypeID = new SelectList(db.LMSWeekListTypes, "WeekListTypeID", "WeekListTypeName");
            ViewBag.MessageType = "";
            ViewBag.Message = "";
            return View("Index", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(LMSWeekDetail LMS)
        {
            try
            {
                LMSWeekDetail d = db.LMSWeekDetails.FirstOrDefault(de => de.Title == LMS.Title);
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
                    // LMS.CreatedBy = Convert.ToInt32(Session["emp_id"]);
                    LMS.CreatedBy = Convert.ToInt32(Session["CurrentUserID"]);
                    LMS.CreatedOn = DateTime.Now;
                    db.LMSWeekDetails.Add(LMS);
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
            model.LMSWeekDetails = db.LMSWeekDetails.Where(x => x.LMSWeekID == LMS.LMSWeekID).OrderByDescending(a => a.LMSWeekDetailID).ToList();
            model.SelectedLMSWeekDetails = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc", LMS.IsActive);
            ViewBag.WeekListTypeID = new SelectList(db.LMSWeekListTypes, "WeekListTypeID", "WeekListTypeName", LMS.WeekListTypeID);

            return View("Index", model);
        }

        public ActionResult Update(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            LMSWeekDetail LMS = db.LMSWeekDetails.Find(id);
            if (LMS == null)
            {
                return HttpNotFound();
            }

            model.LMSWeekDetails = db.LMSWeekDetails.Where(x => x.LMSWeekID == LMS.LMSWeekID).OrderByDescending(a => a.LMSWeekDetailID).ToList();
            model.SelectedLMSWeekDetails = LMS;
            model.DisplayMode = "ReadWrite";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc", LMS.IsActive);
            ViewBag.WeekListTypeID = new SelectList(db.LMSWeekListTypes, "WeekListTypeID", "WeekListTypeName", LMS.WeekListTypeID);
            ViewBag.LMSWeekID = LMS.LMSWeekID;
            ViewBag.MessageType = "";
            ViewBag.Message = "";
            return View("Index", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(LMSWeekDetail LMS)
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
                // LMS.ModifiedBy = Convert.ToInt32(Session["emp_id"]);
                LMS.ModifiedBy = Convert.ToInt32(Session["CurrentUserID"]);
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
            model.LMSWeekDetails = db.LMSWeekDetails.Where(x => x.LMSWeekID == LMS.LMSWeekID).OrderByDescending(a => a.LMSWeekDetailID).ToList();
            model.SelectedLMSWeekDetails = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc", LMS.IsActive);
            ViewBag.WeekListTypeID = new SelectList(db.LMSWeekListTypes, "WeekListTypeID", "WeekListTypeName", LMS.WeekListTypeID);

            return View("Index", model);
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