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
    [Authorize]
    public class MeritListApproveAdmissionsController : Controller
    {
        private ModelCMSNewContainer db = new ModelCMSNewContainer();
        GetSelectionCriteria_ResultViewModel model = new GetSelectionCriteria_ResultViewModel();

        public ActionResult Index()
        {
            model.GetSelectionCriteria_Results = db.GetSelectionCriteria(0, 22, "", 0, 0).ToList();
            model.SelectedGetSelectionCriteria_Result = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.BatchID = new SelectList(db.Batches, "BatchID", "BatchName");
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc");
            ViewBag.BatchProgramID = new SelectList(db.GetBatchProgramNameConcat("", 0), "ID", "Name");
            ViewBag.DegreeID = new SelectList(db.Degrees.Where(d => d.DegreeID == 21 || d.DegreeID == 22 || d.DegreeID == 4020), "DegreeID", "DegreeName");
            ViewBag.EntryTestID = new SelectList(db.EntryTests.Where(et => et.IsActive == "Yes"), "EntryTestID", "EntryTestName");
            ViewBag.MessageType = "";
            ViewBag.Message = "";
            return View(model);
        }

        [HttpGet]
        public ActionResult Search()
        {
            model.GetSelectionCriteria_Results = db.GetSelectionCriteria(0, 22, "", 0, 0).ToList();
            model.SelectedGetSelectionCriteria_Result = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.BatchID = new SelectList(db.Batches, "BatchID", "BatchName");
            ViewBag.BatchProgramID = new SelectList(db.GetBatchProgramNameConcat("", 0), "ID", "Name");
            ViewBag.DegreeID = new SelectList(db.Degrees.Where(d => d.DegreeID == 21 || d.DegreeID == 22 || d.DegreeID == 4020), "DegreeID", "DegreeName");
            ViewBag.EntryTestID = new SelectList(db.EntryTests.Where(et => et.IsActive == "Yes"), "EntryTestID", "EntryTestName");

            ViewBag.MessageType = "";
            ViewBag.Message = "";
            return View("Index", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Search(GetSelectionCriteria_Result selectionCriteria)
        {
            List<GetSelectionCriteria_Result> lstSC = new List<GetSelectionCriteria_Result>();
            try
            {
                if (selectionCriteria.FullName == null)
                {
                    selectionCriteria.FullName = "";
                }
                lstSC = db.GetSelectionCriteria(selectionCriteria.BatchProgramID, 22, selectionCriteria.FullName, 0, 0).ToList();
            }
            catch (SqlException ex)
            {
                ViewBag.MessageType = "error";
                ViewBag.Message = ex.Message;
                ModelState.AddModelError(string.Empty, ex.Message);
            }

            if (lstSC.Count == 0)
            {
                ViewBag.MessageType = "error";
                ViewBag.Message = "No result found.";
            }
            model.GetSelectionCriteria_Results = lstSC;
            model.SelectedGetSelectionCriteria_Result = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.BatchID = new SelectList(db.Batches, "BatchID", "BatchName", selectionCriteria.BatchID);
            ViewBag.BatchProgramID = new SelectList(db.GetBatchProgramNameConcat("", 0), "ID", "Name", selectionCriteria.BatchProgramID);
            ViewBag.DegreeID = new SelectList(db.Degrees.Where(d => d.DegreeID == 21 || d.DegreeID == 22 || d.DegreeID == 4020), "DegreeID", "DegreeName", selectionCriteria.DegreeID);
            ViewBag.EntryTestID = new SelectList(db.EntryTests.Where(et => et.IsActive == "Yes"), "EntryTestID", "EntryTestName", 0);
            ViewBag.hdnDegreeID = selectionCriteria.DegreeID;
            ViewBag.hdnEntryTestID = selectionCriteria.EntryTestID;
            ViewBag.hdnBatchProgramID = selectionCriteria.BatchProgramID;
            ViewBag.hdnBatchID = selectionCriteria.BatchID;
            ViewBag.hdnFullName = selectionCriteria.FullName;
            return View("Index", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveAll(FormCollection fc)
        {
            int hdnBatchID = 0;
            int hdnDegreeID = 0;
            int hdnEntryTestID = 0;
            int hdnBatchProgramID = 0;
            int totalRows = 0;
            int EmpID = 0;
            string hdnFullName = "";
            string chkIsSelected = "";
            string hdnStatusName = "";
            int? EntryTestID = 0;
            try
            {
                if (fc.Count > 0)
                {
                    totalRows = Convert.ToInt32(fc["TotalRows"]);
                    if (totalRows > 0)
                    {
                        hdnBatchID = Convert.ToInt32(fc["hdnBatchID"]);
                        //hdnDegreeID = Convert.ToInt32(fc["hdnDegreeID"]);
                        hdnBatchProgramID = Convert.ToInt32(fc["hdnBatchProgramID"]);
                        //hdnEntryTestID = Convert.ToInt32(fc["hdnEntryTestID"]);
                        hdnFullName = fc["hdnFullName"];
                        EmpID = Convert.ToInt32(Session["emp_id"]);
                        for (int i = 1; i <= totalRows; i++)
                        {
                            string txtFormNo = fc["txtFormNo_" + i];
                            chkIsSelected = fc["chkIsSelected_" + i];
                            if (chkIsSelected == "on")
                            {
                                hdnStatusName = "Selected";
                            }
                            else
                            {
                                hdnStatusName = fc["hdnStatusName_" + i];
                            }

                            db.InsertEntryTestInterviewMarks(txtFormNo, 0, 0, 0, hdnStatusName, 1, 0, EmpID);
                        }
                    }
                    else
                    {
                        ViewBag.MessageType = "error";
                        ViewBag.Message = "No record found to save.";
                    }
                }
            }
            catch (SqlException ex)
            {
                ViewBag.MessageType = "error";
                ViewBag.Message = ex.Message;
                ModelState.AddModelError(string.Empty, ex.Message);
            }

            model.GetSelectionCriteria_Results = db.GetSelectionCriteria(hdnBatchProgramID, 22, hdnFullName, 0, 0).ToList();
            model.SelectedGetSelectionCriteria_Result = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.BatchID = new SelectList(db.Batches, "BatchID", "BatchName", hdnBatchID);
            ViewBag.BatchProgramID = new SelectList(db.GetBatchProgramNameConcat("", 0), "ID", "Name", hdnBatchProgramID);
            ViewBag.DegreeID = new SelectList(db.Degrees.Where(d => d.DegreeID == 21 || d.DegreeID == 22 || d.DegreeID == 4020), "DegreeID", "DegreeName", hdnDegreeID);
            ViewBag.EntryTestID = new SelectList(db.EntryTests.Where(et => et.IsActive == "Yes"), "EntryTestID", "EntryTestName", hdnEntryTestID);
            ViewBag.hdnDegreeID = hdnDegreeID;
            ViewBag.hdnEntryTestID = hdnEntryTestID;
            ViewBag.hdnBatchProgramID = hdnBatchProgramID;
            ViewBag.hdnFullName = hdnFullName;
            ViewBag.hdnBatchID = hdnBatchID;
            return RedirectToAction("Index");
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

        public JsonResult GetEntryTestsByDegreeID(string DegreeID)
        {
            List<EntryTest> lstEntryTests = new List<EntryTest>();
            int dID = Convert.ToInt32(DegreeID);

            lstEntryTests = db.EntryTests.Where(d => d.DegreeID == dID).ToList();
            var EntryTests = lstEntryTests.Select(et => new
            {
                DegreeID = et.DegreeID,
                EntryTestID = et.EntryTestID,
                EntryTestName = et.EntryTestName
            });
            string result = JsonConvert.SerializeObject(EntryTests, Formatting.Indented);
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