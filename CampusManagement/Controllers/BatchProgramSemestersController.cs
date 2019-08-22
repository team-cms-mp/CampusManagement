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
using Newtonsoft.Json;

namespace CampusManagement.Controllers
{
    [Authorize]
    public class BatchProgramSemestersController : Controller
    {
        private ModelCMSNewContainer db = new ModelCMSNewContainer();
        GetBatchProgramSemester_ResultViewModel model = new GetBatchProgramSemester_ResultViewModel();

        public ActionResult Index()
        {
            model.GetBatchProgramSemester_Results = db.GetBatchProgramSemester("").ToList();
            model.SelectedGetBatchProgramSemester_Result = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc");
            ViewBag.BatchID = new SelectList(db.Batches, "BatchID", "BatchName");
            ViewBag.BatchProgramID = new SelectList(db.GetBatchProgramNameConcat("", 0).ToList(), "ID", "Name");
            ViewBag.YearSemesterNo = new SelectList(db.Semesters.Where(s => s.IsActive == "Yes").OrderBy(o => o.YearSemesterNo), "YearSemesterNo", "YearSemesterNo");
            ViewBag.MessageType = "";
            ViewBag.Message = "";
            return View(model);
        }

        [HttpGet]
        public ActionResult Create()
        {
            model.GetBatchProgramSemester_Results = db.GetBatchProgramSemester("").ToList();
            model.SelectedGetBatchProgramSemester_Result = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc");
            ViewBag.BatchID = new SelectList(db.Batches, "BatchID", "BatchName");
            ViewBag.BatchProgramID = new SelectList(db.GetBatchProgramNameConcat("", 0).ToList(), "ID", "Name");
            ViewBag.YearSemesterNo = new SelectList(db.Semesters.Where(s => s.IsActive == "Yes").OrderBy(o => o.YearSemesterNo), "YearSemesterNo", "YearSemesterNo");
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
                    ModelState.AddModelError(string.Empty, "Selected Batch Program is already exist against the same Semester #.");
                    ViewBag.MessageType = "error";
                    ViewBag.Message = "Selected Batch Program is already exist against the same Semester #.";
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
            model.GetBatchProgramSemester_Results = db.GetBatchProgramSemester("").ToList();
            model.SelectedGetBatchProgramSemester_Result = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc", batchProgramSemester.IsActive);
            ViewBag.BatchID = new SelectList(db.Batches, "BatchID", "BatchName");
            ViewBag.BatchProgramID = new SelectList(db.GetBatchProgramNameConcat("", 0).ToList(), "ID", "Name", batchProgramSemester.BatchProgramID);
            ViewBag.YearSemesterNo = new SelectList(db.Semesters.Where(s => s.IsActive == "Yes").OrderBy(o => o.YearSemesterNo), "YearSemesterNo", "YearSemesterNo", batchProgramSemester.YearSemesterNo);
            ViewBag.hdnBatchID = batchProgramSemester.BatchID;
            ViewBag.hdnBatchProgramID = batchProgramSemester.BatchProgramID;
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

            model.GetBatchProgramSemester_Results = db.GetBatchProgramSemester("").ToList();
            model.SelectedGetBatchProgramSemester_Result = batchProgramSemester;
            model.DisplayMode = "ReadWrite";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc", batchProgramSemester.IsActive);
            ViewBag.BatchID = new SelectList(db.Batches, "BatchID", "BatchName");
            ViewBag.BatchProgramID = new SelectList(db.GetBatchProgramNameConcat("", 0).ToList(), "ID", "Name", batchProgramSemester.BatchProgramID);
            ViewBag.YearSemesterNo = new SelectList(db.Semesters.Where(s => s.IsActive == "Yes").OrderBy(o => o.YearSemesterNo), "YearSemesterNo", "YearSemesterNo", batchProgramSemester.YearSemesterNo);
            ViewBag.MessageType = "";
            ViewBag.Message = "";
            ViewBag.hdnBatchID = batchProgramSemester.BatchID;
            ViewBag.hdnBatchProgramID = batchProgramSemester.BatchProgramID;
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

            model.GetBatchProgramSemester_Results = db.GetBatchProgramSemester("").ToList();
            model.SelectedGetBatchProgramSemester_Result = batchProgramSemester;
            model.DisplayMode = "WriteOnly";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc", batchProgramSemester.IsActive);
            ViewBag.BatchID = new SelectList(db.Batches, "BatchID", "BatchName");
            ViewBag.BatchProgramID = new SelectList(db.GetBatchProgramNameConcat("", 0).ToList(), "ID", "Name", batchProgramSemester.BatchProgramID);
            ViewBag.YearSemesterNo = new SelectList(db.Semesters.Where(s => s.IsActive == "Yes").OrderBy(o => o.YearSemesterNo), "YearSemesterNo", "YearSemesterNo", batchProgramSemester.YearSemesterNo);
            ViewBag.hdnBatchID = batchProgramSemester.BatchID;
            ViewBag.hdnBatchProgramID = batchProgramSemester.BatchProgramID;
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
            model.GetBatchProgramSemester_Results = db.GetBatchProgramSemester("").ToList();
            model.SelectedGetBatchProgramSemester_Result = batchProgramSemester;
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
            model.GetBatchProgramSemester_Results = db.GetBatchProgramSemester("").ToList();
            model.SelectedGetBatchProgramSemester_Result = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc");
            ViewBag.BatchID = new SelectList(db.Batches, "BatchID", "BatchName");
            ViewBag.BatchProgramID = new SelectList(db.GetBatchProgramNameConcat("", 0).ToList(), "ID", "Name");
            ViewBag.YearSemesterNo = new SelectList(db.Semesters.Where(s => s.IsActive == "Yes").OrderBy(o => o.YearSemesterNo), "YearSemesterNo", "YearSemesterNo");
            return View("Index", model);
        }
        // GEt programs by Faculty and Batch.
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
