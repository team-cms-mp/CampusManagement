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
    public class EntryTestInterviewMarksController : Controller
    {
        int[] lstDegreeID = new[] { 21, 22, 4020 };
        private ModelCMSNewContainer db = new ModelCMSNewContainer();
        GetSelectionCriteria_ResultViewModel model = new GetSelectionCriteria_ResultViewModel();

        public ActionResult Index()
        {
            db.Degrees.Where(d => lstDegreeID.Contains(d.DegreeID)).ToList();

            model.GetSelectionCriteria_Results = db.GetSelectionCriteria(0, 22, "", 4014, 0).Where(c => c.ObtainedMarks == 0).ToList();
            model.SelectedGetSelectionCriteria_Result = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc");
            ViewBag.BatchID = new SelectList(db.Batches, "BatchID", "BatchName");
            ViewBag.BatchProgramID = new SelectList(db.GetBatchProgramNameConcat("", 0), "ID", "Name");
            ViewBag.DegreeID = new SelectList(db.Degrees.Where(d => lstDegreeID.Contains(d.DegreeID)).ToList(), "DegreeID", "DegreeName");
            ViewBag.EntryTestID = new SelectList(db.EntryTests.Where(et => et.IsActive == "Yes"), "EntryTestID", "EntryTestName");
            ViewBag.MessageType = "";
            ViewBag.Message = "";
            return View(model);
        }

        [HttpGet]
        public ActionResult Search()
        {
            model.GetSelectionCriteria_Results = db.GetSelectionCriteria(0, 22, "", 4014, 0).Where(c => c.ObtainedMarks == 0).ToList();
            model.SelectedGetSelectionCriteria_Result = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.BatchID = new SelectList(db.Batches, "BatchID", "BatchName");
            ViewBag.BatchProgramID = new SelectList(db.GetBatchProgramNameConcat("", 0), "ID", "Name");
            ViewBag.DegreeID = new SelectList(db.Degrees.Where(d => lstDegreeID.Contains(d.DegreeID)).ToList(), "DegreeID", "DegreeName");
            ViewBag.EntryTestID = new SelectList(db.EntryTests.Where(et => et.IsActive == "Yes"), "EntryTestID", "EntryTestName");

            ViewBag.MessageType = "";
            ViewBag.Message = "";
            return View("Index", model);
        }
        public ActionResult EntryTestUpdate()
        {
            return View();
        }
        public ActionResult EntryTestAdd()
        {
            return View();
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
                lstSC = db.GetSelectionCriteria(selectionCriteria.BatchProgramID, selectionCriteria.DegreeID, selectionCriteria.FullName, 4014, selectionCriteria.EntryTestID).Where(c => c.ObtainedMarks == 0).ToList();
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
            ViewBag.DegreeID = new SelectList(db.Degrees.Where(d => lstDegreeID.Contains(d.DegreeID)).ToList(), "DegreeID", "DegreeName", selectionCriteria.DegreeID);
            ViewBag.EntryTestID = new SelectList(db.EntryTests.Where(et => et.IsActive == "Yes"), "EntryTestID", "EntryTestName");
            ViewBag.hdnDegreeID = selectionCriteria.DegreeID;
            ViewBag.hdnBatchID = selectionCriteria.BatchID;
            ViewBag.hdnBatchProgramID = selectionCriteria.BatchProgramID;
            ViewBag.hdnFullName = selectionCriteria.FullName;
            return View("Index", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveAll(FormCollection fc)
        {
            int hdnBatchID = 0;
            int hdnDegreeID = 0;
            int hdnBatchProgramID = 0;
            int totalRows = 0;
            int EmpID = 0;
            string hdnFullName = "";
            float txtTotalMarks = 0;
            float txtObtainedMarks = 0;
            int EntryTestID = 0;
            try
            {
                if (fc.Count > 0)
                {
                    totalRows = Convert.ToInt32(fc["TotalRows"]);
                    if (totalRows > 0)
                    {
                        hdnBatchID = Convert.ToInt32(fc["hdnBatchID"]);
                        hdnDegreeID = Convert.ToInt32(fc["hdnDegreeID"]);
                        hdnBatchProgramID = Convert.ToInt32(fc["hdnBatchProgramID"]);
                        EntryTestID = Convert.ToInt32(fc["hdnEntryTestID"]);
                        hdnFullName = fc["hdnFullName"];
                        EmpID = Convert.ToInt32(Session["emp_id"]);


                        for (int i = 1; i <= totalRows; i++)
                        {
                            string txtFormNo = fc["txtFormNo_" + i];
                            if (fc["txtTotalMarks_" + i] != "")
                            {
                                txtTotalMarks = Convert.ToSingle(fc["txtTotalMarks_" + i]);
                            }

                            if (fc["txtObtainedMarks_" + i] != "")
                            {
                                txtObtainedMarks = Convert.ToSingle(fc["txtObtainedMarks_" + i]);
                            }

                            if (fc["cmbEntryTestID_" + i] != "")
                            {
                                EntryTestID = Convert.ToInt32(fc["cmbEntryTestID_" + i]);
                            }

                            if (txtTotalMarks > 0)
                            {
                                db.InsertEntryTestInterviewMarks(txtFormNo, hdnDegreeID, txtTotalMarks, txtObtainedMarks, "", 0, EntryTestID, EmpID);
                            }
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

            model.GetSelectionCriteria_Results = db.GetSelectionCriteria(hdnBatchProgramID, hdnDegreeID, hdnFullName, 4014, EntryTestID).Where(c => c.ObtainedMarks == 0).ToList();
            model.SelectedGetSelectionCriteria_Result = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.BatchID = new SelectList(db.Batches, "BatchID", "BatchName", hdnBatchID);
            ViewBag.BatchProgramID = new SelectList(db.GetBatchProgramNameConcat("", 0), "ID", "Name", hdnBatchProgramID);
            ViewBag.DegreeID = new SelectList(db.Degrees.Where(d => lstDegreeID.Contains(d.DegreeID)).ToList(), "DegreeID", "DegreeName", hdnDegreeID);
            ViewBag.EntryTestID = new SelectList(db.EntryTests.Where(et => et.IsActive == "Yes"), "EntryTestID", "EntryTestName");
            ViewBag.hdnDegreeID = hdnDegreeID;
            ViewBag.hdnBatchID = hdnBatchID;
            ViewBag.hdnBatchProgramID = hdnBatchProgramID;
            ViewBag.hdnFullName = hdnFullName;
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

        public JsonResult GetEntryTest(string FormNo)
        {
            UpdateEntryTest_Result UpdateEntryTests = new UpdateEntryTest_Result();
            int cId = Convert.ToInt32(FormNo);

            UpdateEntryTests = db.UpdateEntryTest(cId).FirstOrDefault();

            string result = JsonConvert.SerializeObject(UpdateEntryTests, Formatting.Indented);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetApplicantEntryTestInfot(int FormNo, int QueryID)
        {
            GetApplicantEntryTestInfo_Result GetApplicantEntryTestInfot = new GetApplicantEntryTestInfo_Result();

            GetApplicantEntryTestInfot = db.GetApplicantEntryTestInfo(FormNo, QueryID).FirstOrDefault();

            string result = JsonConvert.SerializeObject(GetApplicantEntryTestInfot, Formatting.Indented);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult UpdateEntryTest(int FormNo, int QueryID, float EntryTestMarks, float InterviewMarks, float DrawingTestMakrs)
        {
            int ModifiedBy = Convert.ToInt32(Session["emp_id"]);
            db.UpdateEntryTestForApplicants(FormNo, ModifiedBy, QueryID, EntryTestMarks, InterviewMarks, DrawingTestMakrs);

            string result = JsonConvert.SerializeObject(Formatting.Indented);
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