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
    public class EntryTestInterviewMarksController : Controller
    {
        private ModelCMSContainer db = new ModelCMSContainer();
        GetSelectionCriteria_ResultViewModel model = new GetSelectionCriteria_ResultViewModel();

        public ActionResult Index()
        {
            model.GetSelectionCriteria_Results = db.GetSelectionCriteria(0, 0, "").ToList();
            model.SelectedGetSelectionCriteria_Result = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc");
            ViewBag.BatchProgramID = new SelectList(db.GetBatchProgramNameConcat("", 0), "ID", "Name");
            ViewBag.DegreeID = new SelectList(db.Degrees.Where(d => d.DegreeName == "Interview" || d.DegreeName == "Test"), "DegreeID", "DegreeName");
            ViewBag.MessageType = "";
            ViewBag.Message = "";
            return View(model);
        }

        [HttpGet]
        public ActionResult Search()
        {
            model.GetSelectionCriteria_Results = db.GetSelectionCriteria(0, 0, "").ToList();
            model.SelectedGetSelectionCriteria_Result = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.BatchProgramID = new SelectList(db.GetBatchProgramNameConcat("", 0), "ID", "Name");
            ViewBag.DegreeID = new SelectList(db.Degrees.Where(d => d.DegreeName == "Interview" || d.DegreeName == "Test"), "DegreeID", "DegreeName");

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
                if(selectionCriteria.FullName == null)
                {
                    selectionCriteria.FullName = "";
                }
                lstSC = db.GetSelectionCriteria(selectionCriteria.BatchProgramID, selectionCriteria.DegreeID, selectionCriteria.FullName).ToList();
            }
            catch (SqlException ex)
            {
                ViewBag.MessageType = "error";
                ViewBag.Message = ex.Message;
                ModelState.AddModelError(string.Empty, ex.Message);
            }

            if(lstSC.Count == 0)
            {
                ViewBag.MessageType = "error";
                ViewBag.Message = "No result found.";
            }
            model.GetSelectionCriteria_Results = lstSC;
            model.SelectedGetSelectionCriteria_Result = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.BatchProgramID = new SelectList(db.GetBatchProgramNameConcat("", 0), "ID", "Name", selectionCriteria.BatchProgramID);
            ViewBag.DegreeID = new SelectList(db.Degrees.Where(d => d.DegreeName == "Interview" || d.DegreeName == "Test"), "DegreeID", "DegreeName");
            ViewBag.hdnDegreeID = selectionCriteria.DegreeID;
            ViewBag.hdnBatchProgramID = selectionCriteria.BatchProgramID;
            ViewBag.hdnFullName = selectionCriteria.FullName;
            return View("Index", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveAll(FormCollection fc)
        {
            int hdnDegreeID = 0;
            int hdnBatchProgramID = 0;
            int totalRows = 0;
            string hdnFullName = "";
            float txtTotalMarks = 0;
            float txtObtainedMarks = 0;
            try
            {
                if(fc.Count > 0)
                {
                    totalRows = Convert.ToInt32(fc["TotalRows"]);
                    if (totalRows > 0)
                    {
                        hdnDegreeID = Convert.ToInt32(fc["hdnDegreeID"]);
                        hdnBatchProgramID = Convert.ToInt32(fc["hdnBatchProgramID"]);
                        hdnFullName = fc["hdnFullName"];

                        for (int i = 1; i <= totalRows; i++)
                        {
                            string txtFormNo = fc["txtFormNo_" + i];
                            if(fc["txtTotalMarks_" + i] != "")
                            {
                                txtTotalMarks = Convert.ToSingle(fc["txtTotalMarks_" + i]);
                            }

                            if(fc["txtObtainedMarks_" + i] != "")
                            {
                                txtObtainedMarks = Convert.ToSingle(fc["txtObtainedMarks_" + i]);
                            }

                            if (txtTotalMarks > 0)
                            {
                                db.InsertEntryTestInterviewMarks(txtFormNo, hdnDegreeID, txtTotalMarks, txtObtainedMarks, "", 0);
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

            model.GetSelectionCriteria_Results = db.GetSelectionCriteria(hdnBatchProgramID, hdnDegreeID, hdnFullName).ToList();
            model.SelectedGetSelectionCriteria_Result = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.BatchProgramID = new SelectList(db.GetBatchProgramNameConcat("", 0), "ID", "Name", hdnBatchProgramID);
            ViewBag.DegreeID = new SelectList(db.Degrees.Where(d => d.DegreeName == "Interview" || d.DegreeName == "Test"), "DegreeID", "DegreeName", hdnDegreeID);
            ViewBag.hdnDegreeID = hdnDegreeID;
            ViewBag.hdnBatchProgramID = hdnBatchProgramID;
            ViewBag.hdnFullName = hdnFullName;
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
