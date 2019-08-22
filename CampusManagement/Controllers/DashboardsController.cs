using CampusManagement.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static CampusManagement.Models.ASPNET_MVC_Charts;

namespace CampusManagement.Controllers
{
    public class DashboardsController : Controller
    {
        private ModelCMSNewContainer db = new ModelCMSNewContainer();

        // GET: Dashboard
        public ActionResult Index()
        {
            List<GetApplicantStudentSummaryForDashboard_Result> StudataPoints = db.GetApplicantStudentSummaryForDashboard(0, 1).ToList();

            List<DataPoint1> dataPoints = new List<DataPoint1>();

            foreach (var item in StudataPoints)
            {
                dataPoints.Add(new DataPoint1(item.StatusName, Convert.ToDouble(item.StatusPercentage)));
            }

            ViewBag.DataPoints = JsonConvert.SerializeObject(dataPoints);

            List<GetApplicantStudentSummaryForDashboard_Result> StudataPoints2 = db.GetApplicantStudentSummaryForDashboard(0, 2).ToList();

            List<DataPoint1> dataPoints2 = new List<DataPoint1>();

            foreach (var item in StudataPoints2)
            {
                dataPoints2.Add(new DataPoint1(item.StatusName, Convert.ToDouble(item.StatusPercentage)));
            }

            ViewBag.DataPoints2 = JsonConvert.SerializeObject(dataPoints2);


            List<GetApplicantStudentSummaryForDashboard_Result> StudataPoints3 = db.GetApplicantStudentSummaryForDashboard(0, 3).ToList();

            List<DataPoint1> dataPoints3 = new List<DataPoint1>();

            foreach (var item in StudataPoints3)
            {
                dataPoints3.Add(new DataPoint1(item.StatusName, Convert.ToDouble(item.totalApStudent)));
            }

            ViewBag.DataPoints3 = JsonConvert.SerializeObject(dataPoints3);


            ViewBag.BatchID = new SelectList(db.Batches, "BatchID", "BatchName");
            ViewBag.BatchProgramID = new SelectList(db.GetBatchProgramNameConcat("", 0), "ID", "Name");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(int? BatchProgramID)
        {
            ViewBag.BatchID = new SelectList(db.Batches, "BatchID", "BatchName");
            ViewBag.BatchProgramID = new SelectList(db.GetBatchProgramNameConcat("", 0), "ID", "Name");


            List<GetApplicantStudentSummaryForDashboard_Result> StudataPoints = db.GetApplicantStudentSummaryForDashboard(BatchProgramID, 1).ToList();

            List<DataPoint1> dataPoints = new List<DataPoint1>();

            foreach (var item in StudataPoints)
            {
                dataPoints.Add(new DataPoint1(item.StatusName, Convert.ToDouble(item.StatusPercentage)));
            }

            ViewBag.DataPoints = JsonConvert.SerializeObject(dataPoints);

            ///////////////////

            List<GetApplicantStudentSummaryForDashboard_Result> StudataPoints2 = db.GetApplicantStudentSummaryForDashboard(BatchProgramID, 2).ToList();

            List<DataPoint1> dataPoints2 = new List<DataPoint1>();

            foreach (var item in StudataPoints2)
            {
                dataPoints2.Add(new DataPoint1(item.StatusName, Convert.ToDouble(item.StatusPercentage)));
            }

            ViewBag.DataPoints2 = JsonConvert.SerializeObject(dataPoints2);


            ViewBag.hdnBatchProgramID = BatchProgramID;
            //////////////////////////


            List<GetApplicantStudentSummaryForDashboard_Result> StudataPoints3 = db.GetApplicantStudentSummaryForDashboard(0, 3).ToList();

            List<DataPoint1> dataPoints3 = new List<DataPoint1>();

            foreach (var item in StudataPoints3)
            {
                dataPoints3.Add(new DataPoint1(item.StatusName, Convert.ToDouble(item.totalApStudent)));
            }

            ViewBag.DataPoints3 = JsonConvert.SerializeObject(dataPoints3);
            return View("Index");
        }

        // GEt programs by Faculty and Batch.
        public JsonResult GetPrograms_by_FacultyLevelBatch(string FacultyID, string BatchID)
        {
            List<GetPrograms_by_FacultyLevelBatch_Result> lstPrograms = new List<GetPrograms_by_FacultyLevelBatch_Result>();

            lstPrograms = db.GetPrograms_by_FacultyLevelBatch(Convert.ToInt32(0), 0, Convert.ToInt32(BatchID), 0).ToList();
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