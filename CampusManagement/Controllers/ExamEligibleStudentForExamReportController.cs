using CampusManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;


namespace CampusManagement.Controllers
{
    public class ExamEligibleStudentForExamReportController : Controller
    {
        private ModelCMSNewContainer db = new ModelCMSNewContainer();
        EligibleStudentsForExamReportViewModel model = new EligibleStudentsForExamReportViewModel();
        public ActionResult Index()
        {
            List<GetExam_Result> ExamList = new List<GetExam_Result>();
            List<EligibleModel> EligibleModelList = new List<EligibleModel>();
            ExamList = db.GetExam().ToList();
            EligibleModelList = GenrateEligibleModel();
            if (ExamList == null || ExamList.Count == 0)
            {
                GetExam_Result GetExam_ResultObj = new GetExam_Result();
                GetExam_ResultObj.ExamID = 0;
                GetExam_ResultObj.ExamDetail = "Please Add Exam First";
                ExamList.Add(GetExam_ResultObj);
            }
            model.ExamID = ExamList[0].ExamID;
            ViewBag.ExamID = new SelectList(ExamList, "ExamID", "ExamDetail");
            ViewBag.IsEligible = new SelectList(EligibleModelList, "IsEligible", "EligibleText");
            model.IsEligible = EligibleModelList[0].IsEligible;
            List<Batch> BatchList = new List<Batch>();
            BatchList = db.Batches.ToList();
            Batch BatchObj = new Batch();
            BatchObj.BatchID = 0;
            BatchObj.BatchName = "--Please Select--";
            BatchList.Insert(0, BatchObj);
            ViewBag.BatchID = new SelectList(BatchList, "BatchID", "BatchName");
            ViewBag.BatchProgramID = new SelectList(db.GetBatchProgramNameConcat("", 0).ToList(), "ID", "Name");
            ViewBag.YearSemesterNo = new SelectList(db.Semesters, "YearSemesterNo", "YearSemesterNo");

            return View(model);
        }


        [HttpPost]
        public ActionResult index(EligibleStudentsForExamReportViewModel ees)
        {
            string ErrorMessage = "";

            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ViewBag.MessageType = "error";
                ViewBag.Message = ErrorMessage;
            }
            List<GetExam_Result> ExamList = new List<GetExam_Result>();
            List<EligibleModel> EligibleModelList = new List<EligibleModel>();
            ViewBag.ExamID = new SelectList(db.GetExam().ToList(), "ExamID", "ExamDetail", ees.ExamID);
            EligibleModelList = GenrateEligibleModel();
            ViewBag.IsEligible = new SelectList(EligibleModelList, "IsEligible", "EligibleText");
            List<Batch> BatchList = new List<Batch>();
            BatchList = db.Batches.ToList();
            Batch BatchObj = new Batch();
            BatchObj.BatchID = 0;
            BatchObj.BatchName = "--Please Select--";
            BatchList.Insert(0, BatchObj);
            ViewBag.BatchID = new SelectList(BatchList, "BatchID", "BatchName", ees.BatchID);
            ViewBag.BatchProgramID = new SelectList(db.GetBatchProgramNameConcat("", 0).ToList(), "ID", "Name", ees.BatchProgramID);
            ViewBag.YearSemesterNo = new SelectList(db.Semesters, "YearSemesterNo", "YearSemesterNo", ees.YearSemesterNo);
            ViewBag.hdnBatchID = ees.BatchID;
            ViewBag.hdnBatchProgramID = ees.BatchProgramID;
            ViewBag.hdnYearSemesterNo = ees.YearSemesterNo;
            model.ExamID = ees.ExamID;
            model.IsEligible = ees.IsEligible;
            return View(model);
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

        private class EligibleModel
        {
            public int IsEligible { get; set; }
            public string EligibleText { get; set; }

        }

        private List<EligibleModel> GenrateEligibleModel()
        {
            List<EligibleModel> List = new List<EligibleModel>();
            EligibleModel Obj1 = new EligibleModel();
            Obj1.IsEligible = -1;
            Obj1.EligibleText = "All";
            EligibleModel Obj2 = new EligibleModel();
            Obj2.IsEligible = 1;
            Obj2.EligibleText = "Eligible";
            EligibleModel Obj3 = new EligibleModel();
            Obj3.IsEligible = 0;
            Obj3.EligibleText = "Not Eligible";
            List.Add(Obj1);
            List.Add(Obj2);
            List.Add(Obj3);
            return List;
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