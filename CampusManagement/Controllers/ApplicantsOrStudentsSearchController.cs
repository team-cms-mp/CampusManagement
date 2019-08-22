using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CampusManagement.Models;
using Newtonsoft.Json;
using PagedList;
using System.Globalization;

using CampusManagement.App_Code;
using System.Configuration;
using System.IO;
using System.Net.Mail;


namespace CampusManagement.Controllers
{
    public class ApplicantsOrStudentsSearchController : Controller
    {
        private ModelCMSNewContainer db = new ModelCMSNewContainer();
      

        // GET: ApplicantsOrStudentsSearch
        [HttpGet]
        public ActionResult Index(int? page, int? pageSize, string BatchID, string BatchProgramID, string Status, string message)
        {
            if (pageSize == null)
            {
                pageSize = 10;
            }
            ViewBag.pageSize = pageSize;
            int pageNumber = (page ?? 1);
            if (BatchID == null)
            {
                BatchID = "0";
            }
            if (BatchProgramID == null)
            {
                BatchProgramID = "0";
            }
            if (Status == null)
            {
                Status = "0";
            }
            List<StatusModel> StatusModelList = new List<StatusModel>();
            StatusModelList = GenrateStatusModel();
            ViewBag.Status = new SelectList(StatusModelList, "StatusID", "StatusName", Status);
            List<Batch> BatchList = new List<Batch>();
            Batch BatchObj = new Batch();
            BatchObj.BatchID = 0;
            BatchObj.BatchName = "--ALL--";
            BatchList = db.Batches.ToList();
            BatchList.Insert(0, BatchObj);
            ViewBag.BatchID = new SelectList(BatchList, "BatchID", "BatchName", BatchID);
            ViewBag.BatchProgramID = new SelectList(db.GetBatchProgramNameConcat("", 0).ToList(), "ID", "Name", BatchProgramID);
            ViewBag.hdnBatchID = BatchID;
            ViewBag.hdnBatchProgramID = BatchProgramID;
            // model.ProgramPloList = db.GetAllOBE_ProgramPLO(Convert.ToInt32(ProgramID), PLOCode, PLOName, "0").ToList().ToPagedList(pageNumber, Convert.ToInt32(pageSize));
            return View(db.GetStudentOrApplicantList("",Convert.ToInt32(BatchID), Convert.ToInt32(BatchProgramID), "0", Convert.ToInt32(Status),null, null).ToList().ToPagedList(pageNumber, Convert.ToInt32(pageSize)));
        }

        [HttpPost]
        public ActionResult Index(int? page, string BatchID, string BatchProgramID, string Status)
        {
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            if (BatchID == null)
            {
                BatchID = "0";
            }
            if (BatchProgramID == null)
            {
                BatchProgramID = "0";
            }
            if (Status == null)
            {
                Status = "0";
            }
            List<StatusModel> StatusModelList = new List<StatusModel>();
            StatusModelList = GenrateStatusModel();
            ViewBag.Status = new SelectList(StatusModelList, "StatusID", "StatusName", Status);
            List<Batch> BatchList = new List<Batch>();
            Batch BatchObj = new Batch();
            BatchObj.BatchID = 0;
            BatchObj.BatchName = "--ALL--";
            BatchList = db.Batches.ToList();
            BatchList.Insert(0, BatchObj);
            ViewBag.BatchID = new SelectList(BatchList, "BatchID", "BatchName", BatchID);
            ViewBag.BatchProgramID = new SelectList(db.GetBatchProgramNameConcat("", 0).ToList(), "ID", "Name", BatchProgramID);
            ViewBag.hdnBatchID = BatchID;
            ViewBag.hdnBatchProgramID = BatchProgramID;
            return View(db.GetStudentOrApplicantList("", Convert.ToInt32(BatchID), Convert.ToInt32(BatchProgramID), "0", Convert.ToInt32(Status), null, null).ToList().ToPagedList(pageNumber, Convert.ToInt32(pageSize)));


        }



        public JsonResult GetPrograms_by_FacultyLevelBatch(string FacultyID, string BatchID)
        {
            List<GetPrograms_by_FacultyLevelBatch_Result> lstPrograms = new List<GetPrograms_by_FacultyLevelBatch_Result>();
            GetPrograms_by_FacultyLevelBatch_Result GetPrograms_by_FacultyLevelBatch_ResultObj = new GetPrograms_by_FacultyLevelBatch_Result();
            GetPrograms_by_FacultyLevelBatch_ResultObj.BatchProgramID = 0;
            GetPrograms_by_FacultyLevelBatch_ResultObj.ProgramName = "--ALL--";
            lstPrograms = db.GetPrograms_by_FacultyLevelBatch(Convert.ToInt32(FacultyID), 0, Convert.ToInt32(BatchID), 0).ToList();
            lstPrograms.Insert(0, GetPrograms_by_FacultyLevelBatch_ResultObj);
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


        private class StatusModel
        {
            public int StatusID { get; set; }
            public string StatusName { get; set; }

        }

        private List<StatusModel> GenrateStatusModel()
        {
            List<StatusModel> List = new List<StatusModel>();
            StatusModel Obj1 = new StatusModel();
            Obj1.StatusID = 0;
            Obj1.StatusName = "All";
            StatusModel Obj2 = new StatusModel();
            Obj2.StatusID = 6015;
            Obj2.StatusName = "Students";
            StatusModel Obj3 = new StatusModel();
            Obj3.StatusID = -1;
            Obj3.StatusName = "Applicants";
            List.Add(Obj1);
            List.Add(Obj2);
            List.Add(Obj3);
            return List;
        }
    }
}