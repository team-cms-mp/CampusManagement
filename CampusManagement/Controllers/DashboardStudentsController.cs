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
    public class DashboardStudentsController : Controller
    {
        private ModelCMSNewContainer db = new ModelCMSNewContainer();

        // GET: Dashboard


        public ActionResult Faculty()
        {
            return View();
        }

        public ActionResult Attendance()
        {
            return View();
        }
        public ActionResult StudentCount()
        {
            return View();
        }
        public ActionResult Index(GetApplicantStudentSummaryForDashboard_Result deshBord)
        {



            List<GetApplicantStudentSummaryForDashboard_Result> StudataPoints = db.GetApplicantStudentSummaryForDashboard(0, 1).ToList();

            List<DataPoint1> dataPoints = new List<DataPoint1>();

            foreach (var item in StudataPoints)
            {
                dataPoints.Add(new DataPoint1(item.StatusName, Convert.ToDouble(item.totalApStudent)));
            }

            ViewBag.DataPoints = JsonConvert.SerializeObject(dataPoints);

            List<GetApplicantStudentSummaryForDashboard_Result> StudataPoints2 = db.GetApplicantStudentSummaryForDashboard(0, 2).ToList();

            List<DataPoint1> dataPoints2 = new List<DataPoint1>();

            foreach (var item in StudataPoints2)
            {
                dataPoints2.Add(new DataPoint1(item.StatusName, Convert.ToDouble(item.totalApStudent)));
            }

            ViewBag.DataPoints2 = JsonConvert.SerializeObject(dataPoints2);


            List<GetApplicantStudentSummaryForDashboard_Result> StudataPoints3 = db.GetApplicantStudentSummaryForDashboard(0, 3).ToList();

            List<DataPoint1> dataPoints3 = new List<DataPoint1>();

            foreach (var item in StudataPoints3)
            {
                dataPoints3.Add(new DataPoint1(item.StatusName, Convert.ToDouble(item.totalApStudent)));
            }

            ViewBag.DataPoints3 = JsonConvert.SerializeObject(dataPoints3);

            /////////////////////


            List<GetApplicantStudentSummaryForDashboard_Result> StudataPoints4 = db.GetApplicantStudentSummaryForDashboard(0, 4).ToList();

            List<DataPoint1> dataPoints4 = new List<DataPoint1>();

            foreach (var item in StudataPoints4)
            {
                dataPoints4.Add(new DataPoint1(item.StatusName, Convert.ToDouble(item.totalApStudent)));
            }

            ViewBag.DataPoints4 = JsonConvert.SerializeObject(dataPoints4);

            ViewBag.hdnBatchID = deshBord.BatchID;
            ViewBag.hdnBatchProgramID = deshBord.BatchProgramID;

            //ViewBag.hdnBatchID =  deshBord.BatchID;
            ViewBag.BatchID = new SelectList(db.Batches, "BatchID", "BatchName");

            ViewBag.BatchProgramID = new SelectList(db.GetBatchProgramNameConcat("", 0), "ID", "Name");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(int? BatchProgramID, GetApplicantStudentSummaryForDashboard_Result deshBord)
        {

            List<GetApplicantStudentSummaryForDashboard_Result> StudataPoints = db.GetApplicantStudentSummaryForDashboard(BatchProgramID, 1).ToList();

            List<DataPoint1> dataPoints = new List<DataPoint1>();

            foreach (var item in StudataPoints)
            {
                dataPoints.Add(new DataPoint1(item.StatusName, Convert.ToDouble(item.totalApStudent)));
            }

            ViewBag.DataPoints = JsonConvert.SerializeObject(dataPoints);

            ///////////////////

            List<GetApplicantStudentSummaryForDashboard_Result> StudataPoints2 = db.GetApplicantStudentSummaryForDashboard(BatchProgramID, 2).ToList();

            List<DataPoint1> dataPoints2 = new List<DataPoint1>();

            foreach (var item in StudataPoints2)
            {
                dataPoints2.Add(new DataPoint1(item.StatusName, Convert.ToDouble(item.totalApStudent)));

            }

            ViewBag.DataPoints2 = JsonConvert.SerializeObject(dataPoints2);





            //////////////////////////


            List<GetApplicantStudentSummaryForDashboard_Result> StudataPoints3 = db.GetApplicantStudentSummaryForDashboard(0, 3).ToList();

            List<DataPoint1> dataPoints3 = new List<DataPoint1>();

            foreach (var item in StudataPoints3)
            {
                dataPoints3.Add(new DataPoint1(item.StatusName, Convert.ToDouble(item.totalApStudent)));
            }

            ViewBag.DataPoints3 = JsonConvert.SerializeObject(dataPoints3);



            /////////////////////


            List<GetApplicantStudentSummaryForDashboard_Result> StudataPoints4 = db.GetApplicantStudentSummaryForDashboard(0, 4).ToList();

            List<DataPoint1> dataPoints4 = new List<DataPoint1>();

            foreach (var item in StudataPoints4)
            {
                dataPoints4.Add(new DataPoint1(item.StatusName, Convert.ToDouble(item.totalApStudent)));
            }

            ViewBag.DataPoints4 = JsonConvert.SerializeObject(dataPoints4);



            ViewBag.hdnBatchID = deshBord.BatchID;
            ViewBag.hdnBatchProgramID = deshBord.BatchProgramID;
            ViewBag.BatchID = new SelectList(db.Batches, "BatchID", "BatchName");
            ViewBag.BatchProgramID = new SelectList(db.GetBatchProgramNameConcat("", 0), "ID", "Name");

            return View("Index");
        }

        [HttpGet]
        public ActionResult FacultyDashboard()
        {
            List<GetApplicantStudentSummaryForDashboard_Result> StudataPoints5 = db.GetApplicantStudentSummaryForDashboard(0, 5).ToList();

            List<DataPoint1> dataPoints5 = new List<DataPoint1>();

            foreach (var item in StudataPoints5)
            {
                dataPoints5.Add(new DataPoint1(item.StatusName, Convert.ToDouble(item.totalApStudent)));
            }

            ViewBag.DataPoints5 = JsonConvert.SerializeObject(dataPoints5);

            ////////smester wise///


            List<GetApplicantStudentSummaryForDashboard_Result> StudataPoints6 = db.GetApplicantStudentSummaryForDashboard(0, 6).ToList();

            List<DataPoint1> dataPoints6 = new List<DataPoint1>();

            foreach (var item in StudataPoints6)
            {
                dataPoints6.Add(new DataPoint1(item.StatusName, Convert.ToDouble(item.totalApStudent)));
            }

            ViewBag.DataPoints6 = JsonConvert.SerializeObject(dataPoints6);





            return View();
        }

        public ActionResult TeacherSubjectCount()
        {

            List<GetApplicantStudentSummaryForDashboard_Result> StudataPoints7 = db.GetApplicantStudentSummaryForDashboard(0, 7).ToList();

            List<DataPoint1> dataPoints7 = new List<DataPoint1>();

            foreach (var item in StudataPoints7)
            {
                dataPoints7.Add(new DataPoint1(item.StatusName, Convert.ToDouble(item.totalApStudent)));
            }

            ViewBag.DataPoints7 = JsonConvert.SerializeObject(dataPoints7);

            return View();
        }

        public ActionResult StudentAttandance(GetAttandanceStudentSummaryForDashboard_Result SATSD)
        {
            ////////Student Attendance summary///


            List<GetAttandanceStudentSummaryForDashboard_Result> StudataPoints1 = db.GetAttandanceStudentSummaryForDashboard(1).ToList();

            List<DataPoint1> dataPoints1 = new List<DataPoint1>();

            foreach (var item in StudataPoints1)
            {
                dataPoints1.Add(new DataPoint1(item.StatusName, Convert.ToDouble(item.StatusPercentage)));
            }

            ViewBag.DataPoints1 = JsonConvert.SerializeObject(dataPoints1);

            /// total On Sick Leave student
            /// 

            List<GetAttandanceStudentSummaryForDashboard_Result> StudataPoints2 = db.GetAttandanceStudentSummaryForDashboard(4).ToList();

            List<DataPoint1> dataPoints2 = new List<DataPoint1>();

            foreach (var item in StudataPoints2)
            {
                dataPoints2.Add(new DataPoint1(item.StatusName, Convert.ToDouble(item.totalApStudent)));
            }

            ViewBag.DataPoints2 = JsonConvert.SerializeObject(dataPoints2);

            /// total Present student
            /// 

            List<GetAttandanceStudentSummaryForDashboard_Result> StudataPoints3 = db.GetAttandanceStudentSummaryForDashboard(5).ToList();

            List<DataPoint1> dataPoints3 = new List<DataPoint1>();

            foreach (var item in StudataPoints3)
            {
                dataPoints3.Add(new DataPoint1(item.StatusName, Convert.ToDouble(item.totalApStudent)));
            }

            ViewBag.DataPoints3 = JsonConvert.SerializeObject(dataPoints3);

            /// total Absent student
            /// 

            List<GetAttandanceStudentSummaryForDashboard_Result> StudataPoints4 = db.GetAttandanceStudentSummaryForDashboard(6).ToList();

            List<DataPoint1> dataPoints4 = new List<DataPoint1>();

            foreach (var item in StudataPoints4)
            {
                dataPoints4.Add(new DataPoint1(item.StatusName, Convert.ToDouble(item.totalApStudent)));
            }

            ViewBag.DataPoints4 = JsonConvert.SerializeObject(dataPoints4);

            /// total 'On Casual Leave' student
            /// 

            List<GetAttandanceStudentSummaryForDashboard_Result> StudataPoints5 = db.GetAttandanceStudentSummaryForDashboard(7).ToList();

            List<DataPoint1> dataPoints5 = new List<DataPoint1>();

            foreach (var item in StudataPoints5)
            {
                dataPoints5.Add(new DataPoint1(item.StatusName, Convert.ToDouble(item.totalApStudent)));
            }

            ViewBag.DataPoints5 = JsonConvert.SerializeObject(dataPoints5);
            return View();
        }
        [HttpGet]
        public ActionResult StudentAttendanceProgramWise()
        {
            List<GetBatchProgramWiseAttandanceStudentSummaryForDashboard_Result> StudataPoints1 = db.GetBatchProgramWiseAttandanceStudentSummaryForDashboard(0, 0, 0, 1).ToList();

            List<DataPoint1> dataPoints1 = new List<DataPoint1>();

            foreach (var item in StudataPoints1)
            {
                dataPoints1.Add(new DataPoint1(item.StatusName, Convert.ToDouble(item.totalApStudent)));
            }

            ViewBag.DataPoints1 = JsonConvert.SerializeObject(dataPoints1);





            //ViewBag.hdnBatchID =  deshBord.BatchID;
            ViewBag.BatchID = new SelectList(db.Batches, "BatchID", "BatchName");

            ViewBag.BatchProgramID = new SelectList(db.GetBatchProgramNameConcat("", 0), "ID", "Name");
            ViewBag.YearSemesterNo = new SelectList(db.Semesters, "YearSemesterNo", "YearSemesterNo");

            return View();
        }
        [HttpPost]
        public ActionResult StudentAttendanceProgramWise(int? BatchProgramID, int? BatchID, int? YearSemesterNo)
        {
            List<GetBatchProgramWiseAttandanceStudentSummaryForDashboard_Result> StudataPoints1 = db.GetBatchProgramWiseAttandanceStudentSummaryForDashboard(BatchProgramID, BatchID, YearSemesterNo, 1).ToList();

            List<DataPoint1> dataPoints1 = new List<DataPoint1>();

            foreach (var item in StudataPoints1)
            {
                dataPoints1.Add(new DataPoint1(item.StatusName, Convert.ToDouble(item.totalApStudent)));
            }

            ViewBag.DataPoints1 = JsonConvert.SerializeObject(dataPoints1);




            ViewBag.hdnBatchID = BatchID;
            ViewBag.hdnBatchProgramID = BatchProgramID;
            ViewBag.hdnYearSemesterNo = YearSemesterNo;

            ViewBag.BatchID = new SelectList(db.Batches, "BatchID", "BatchName");
            ViewBag.BatchProgramID = new SelectList(db.GetBatchProgramNameConcat("", 0), "ID", "Name");
            ViewBag.YearSemesterNo = new SelectList(db.Semesters, "YearSemesterNo", "YearSemesterNo");

            return View();
        }

        public ActionResult DesignationEmployeeDashboard()
        {
            //-Designation wise Teacher Summary
            List<GetAttandanceStudentSummaryForDashboard_Result> StudataPoints8 = db.GetAttandanceStudentSummaryForDashboard(8).ToList();

            List<DataPoint1> dataPoints8 = new List<DataPoint1>();

            foreach (var item in StudataPoints8)
            {
                dataPoints8.Add(new DataPoint1(item.StatusName, Convert.ToDouble(item.totalApStudent)));
            }

            ViewBag.DataPoints8 = JsonConvert.SerializeObject(dataPoints8);

            return View();
        }

        public ActionResult DepartmentEmployeeDashboard()
        {



            //Department wise Employee Summary
            List<GetAttandanceStudentSummaryForDashboard_Result> StudataPoints9 = db.GetAttandanceStudentSummaryForDashboard(9).ToList();

            List<DataPoint1> dataPoints9 = new List<DataPoint1>();

            foreach (var item in StudataPoints9)
            {
                dataPoints9.Add(new DataPoint1(item.StatusName, Convert.ToDouble(item.totalApStudent)));
            }

            ViewBag.DataPoints9 = JsonConvert.SerializeObject(dataPoints9);
            return View();
        }

        public ActionResult GenderWiseEmployeeDashboard()
        {



            //Gender wise Employee Summary
            List<GetAttandanceStudentSummaryForDashboard_Result> StudataPoints10 = db.GetAttandanceStudentSummaryForDashboard(10).ToList();

            List<DataPoint1> dataPoints10 = new List<DataPoint1>();

            foreach (var item in StudataPoints10)
            {
                dataPoints10.Add(new DataPoint1(item.StatusName, Convert.ToDouble(item.totalApStudent)));
            }

            ViewBag.DataPoints10 = JsonConvert.SerializeObject(dataPoints10);
            return View();
        }


        public ActionResult BatchWiseAmountsDashboard()
        {



            //GetBatchWiseAmounts
            List<SF_GetBatchWiseAmounts_Result> StudataPoints11 = db.SF_GetBatchWiseAmounts("Yes", "", "Student", 0, 0, 1).Where(a => a.BatchName != "Total Amount").ToList();

            List<DataPoint1> dataPoints11 = new List<DataPoint1>();

            foreach (var item in StudataPoints11)
            {
                dataPoints11.Add(new DataPoint1(item.BatchName, Convert.ToDouble(item.Amount)));
            }

            ViewBag.DataPoints11 = JsonConvert.SerializeObject(dataPoints11);


            return View();
        }


        [HttpGet]
        public ActionResult ProgramWiseAmountsDashboard()
        {

            //GetBatchWiseAmounts
            List<SF_GetBatchWiseAmounts_Result> StudataPoints12 = db.SF_GetBatchWiseAmounts("Yes", "", "Student", 0, 0, 2).Where(a => a.BatchName != "Total Amount").ToList();

            List<DataPoint1> dataPoints12 = new List<DataPoint1>();

            foreach (var item in StudataPoints12)
            {
                dataPoints12.Add(new DataPoint1(item.BatchName, Convert.ToDouble(item.Amount)));
            }

            ViewBag.DataPoints12 = JsonConvert.SerializeObject(dataPoints12);

            ViewBag.BatchID = new SelectList(db.Batches, "BatchID", "BatchName");

            ViewBag.BatchProgramID = new SelectList(db.GetBatchProgramNameConcat("", 0), "ID", "Name");



            return View();
        }
        [HttpPost]
        public ActionResult ProgramWiseAmountsDashboard(SF_GetBatchWiseAmounts_Result amnt)

        {



            //GetBatchWiseAmounts
            List<SF_GetBatchWiseAmounts_Result> StudataPoints12 = db.SF_GetBatchWiseAmounts("Yes", "", "Student", amnt.BatchID, 0, 2).Where(a => a.BatchName != "Total Amount").ToList();

            List<DataPoint1> dataPoints12 = new List<DataPoint1>();

            foreach (var item in StudataPoints12)
            {
                dataPoints12.Add(new DataPoint1(item.BatchName, Convert.ToDouble(item.Amount)));
            }

            ViewBag.DataPoints12 = JsonConvert.SerializeObject(dataPoints12);

            ViewBag.hdnBatchID = amnt.BatchID;
            ViewBag.hdnBatchProgramID = amnt.BatchProgramID;
            ViewBag.BatchID = new SelectList(db.Batches, "BatchID", "BatchName");
            ViewBag.BatchProgramID = new SelectList(db.GetBatchProgramNameConcat("", 0), "ID", "Name");

            return View("ProgramWiseAmountsDashboard");
        }


        public ActionResult test()
        {
            List<GetApplicantStudentSummaryForDashboard_Result> StudataPoints5 = db.GetApplicantStudentSummaryForDashboard(0, 5).ToList();

            List<DataPoint1> dataPoints5 = new List<DataPoint1>();

            foreach (var item in StudataPoints5)
            {
                dataPoints5.Add(new DataPoint1(item.StatusName, Convert.ToDouble(item.totalApStudent)));
            }

            ViewBag.DataPoints5 = JsonConvert.SerializeObject(dataPoints5);

            List<GetApplicantStudentSummaryForDashboard_Result> StudataPoints6 = db.GetApplicantStudentSummaryForDashboard(0, 6).ToList();

            List<DataPoint1> dataPoints6 = new List<DataPoint1>();

            foreach (var item in StudataPoints6)
            {
                dataPoints6.Add(new DataPoint1(item.StatusName, Convert.ToDouble(item.totalApStudent)));
            }

            ViewBag.DataPoints6 = JsonConvert.SerializeObject(dataPoints6);
            return View();
        }


        public ActionResult CollegeServiceWiseAmounts(int? BatchProgramID, int? BatchID, SF_GetCollegeServiceWiseAmounts_Result deshBord)
        {
            List<SF_GetCollegeServiceWiseAmounts_Result> CollegeServiceWiseAmounts = db.SF_GetCollegeServiceWiseAmounts("Yes", "52642", "Student", BatchID, BatchProgramID).ToList();

            List<DataPoint1> dataPoints5 = new List<DataPoint1>();

            foreach (var item in CollegeServiceWiseAmounts)
            {
                dataPoints5.Add(new DataPoint1(item.CollegeServiceName, Convert.ToDouble(item.Amount)));
            }

            ViewBag.DataPoints5 = JsonConvert.SerializeObject(dataPoints5);


            ViewBag.hdnBatchID = deshBord.BatchID;
            ViewBag.hdnBatchProgramID = deshBord.BatchProgramID;

            ViewBag.BatchID = new SelectList(db.Batches, "BatchID", "BatchName");

            ViewBag.BatchProgramID = new SelectList(db.GetBatchProgramNameConcat("", 0), "ID", "Name");

            return View();
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
        public ActionResult Pie()
        {
            //Below code can be used to include dynamic data in Chart.Check view page and uncomment the line "dataPoints: @Html.Raw(ViewBag.DataPoints)"
            //ViewBag.DataPoints = JsonConvert.SerializeObject(DataService.GetRandomDataForCategoryAxis(5), _jsonSetting);

            return View();
        }

        public ActionResult TestView()
        {
            return View();
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