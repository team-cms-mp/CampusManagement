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
    public class StudentInformationController : Controller
    {
        private ModelCMSNewContainer db = new ModelCMSNewContainer();
        GetStudentInformationList_ResultViewModel model = new GetStudentInformationList_ResultViewModel();

        [HttpGet]
        public ActionResult Index()
        {
            List<Batch> BatchList = new List<Batch>();
            Batch BatchObj = new Batch();
            BatchObj.BatchID = 0;
            BatchObj.BatchName = "--ALL--";
            BatchList = db.Batches.ToList();
            BatchList.Insert(0, BatchObj);
            ViewBag.BatchID = new SelectList(BatchList, "BatchID", "BatchName");
            ViewBag.FacultyID = new SelectList(db.GetSubDepartments_by_HospitalID(MvcApplication.Hospital_ID), "SubDept_Id", "SubDept_Name");
            ViewBag.BatchProgramID = new SelectList(db.GetBatchProgramNameConcat("", 0).ToList(), "ID", "Name");
          
            ViewBag.YearSemesterNo = new SelectList(db.Semesters, "YearSemesterNo", "YearSemesterNo");

            //ViewBag.paramSearch = paramSearch;
            int EmpID = Convert.ToInt32(Session["emp_id"]);
            return View(db.GetStudentInformationList("", 0, 0, 0, 0, 0).ToList());
        }



        [HttpPost]
        public ActionResult Index(string Search, string BatchID,string BatchProgramID, string SubDept_Id,string YearSemesterNo)
        {
            List<Batch> BatchList = new List<Batch>();
            Batch BatchObj = new Batch();
            BatchObj.BatchID = 0;
            BatchObj.BatchName = "--ALL--";
            BatchList = db.Batches.ToList();
            BatchList.Insert(0, BatchObj);
            ViewBag.hdnBatchID = BatchID;
            ViewBag.hdnBatchProgramID = BatchProgramID;
            ViewBag.hdnYearSemesterNo = YearSemesterNo;
            ViewBag.BatchID = new SelectList(BatchList, "BatchID", "BatchName", BatchID);
            ViewBag.FacultyID = new SelectList(db.GetSubDepartments_by_HospitalID(MvcApplication.Hospital_ID), "SubDept_Id", "SubDept_Name");
            ViewBag.BatchProgramID = new SelectList(db.GetBatchProgramNameConcat("", 0).ToList(), "ID", "Name", BatchProgramID);
            ViewBag.YearSemesterNo = new SelectList(db.Semesters, "YearSemesterNo", "YearSemesterNo", YearSemesterNo);
            return View(db.GetStudentInformationList("", Convert.ToInt32(BatchID), Convert.ToInt32(BatchProgramID), Convert.ToInt32(SubDept_Id), Convert.ToInt32(YearSemesterNo), 0).ToList());
        }

        
        /// <summary>
        ///  department///
        /// </summary>
        /// <param name="FacultyID"></param>
        /// <param name="BatchID"></param>
        /// <returns></returns>
        public JsonResult GetSubDepartement_by_FacultyLevelBatch( string BatchID)
        {
            List<GetPrograms_by_FacultyLevelBatch_Result> lstDepartment = new List<GetPrograms_by_FacultyLevelBatch_Result>();
            GetPrograms_by_FacultyLevelBatch_Result GetSubDepartments_by_HospitalID_ResultObj = new GetPrograms_by_FacultyLevelBatch_Result();
            GetSubDepartments_by_HospitalID_ResultObj.BatchProgramID = 0;
            GetSubDepartments_by_HospitalID_ResultObj.ProgramName = "--ALL--";
            lstDepartment = db.GetPrograms_by_FacultyLevelBatch(0, 0, Convert.ToInt32(BatchID), 1).ToList();
            lstDepartment.Insert(0, GetSubDepartments_by_HospitalID_ResultObj);
            var programs = lstDepartment.Select(p => new
            {
                BatchProgramID = p.BatchProgramID,
                ProgramName = p.ProgramName
            });
            string result = JsonConvert.SerializeObject(programs, Formatting.Indented);
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetPrograms_by_FacultyLevelBatch(string BatchID, string FacultyID)
        {
            List<GetPrograms_by_FacultyLevelBatch_Result> lstDepartment = new List<GetPrograms_by_FacultyLevelBatch_Result>();
            GetPrograms_by_FacultyLevelBatch_Result GetSubDepartments_by_HospitalID_ResultObj = new GetPrograms_by_FacultyLevelBatch_Result();
            GetSubDepartments_by_HospitalID_ResultObj.BatchProgramID = 0;
            GetSubDepartments_by_HospitalID_ResultObj.ProgramName = "--ALL--";
            lstDepartment = db.GetPrograms_by_FacultyLevelBatch(Convert.ToInt32(FacultyID), 0, Convert.ToInt32(BatchID),0).ToList();
            lstDepartment.Insert(0, GetSubDepartments_by_HospitalID_ResultObj);
            var programs = lstDepartment.Select(p => new
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

            lstSemester = db.BatchProgramSemesters.Where(s => s.BatchProgramID == bpId).OrderBy(a =>a.YearSemesterNo).ToList();
            var semesters = lstSemester.Select(S => new
            {
                YearSemesterNo = S.YearSemesterNo
            });
            string result = JsonConvert.SerializeObject(semesters, Formatting.Indented);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetBatchProgramCourseList(string BatchID, string BatchProgramID, string YearSemesterNo)
        {
            List<GetProgramCourse_Result> lstProgramCourse = new List<GetProgramCourse_Result>();
            int bpId = Convert.ToInt32(BatchProgramID);
            GetProgramCourse_Result Obj = new GetProgramCourse_Result();
            Obj.ProgramCourseID = 0;
            Obj.CourseCode = "--ALL--";
            Obj.CourseName = "";
            lstProgramCourse = db.GetProgramCourse(Convert.ToInt32(BatchID), Convert.ToInt32(BatchProgramID), Convert.ToInt32(YearSemesterNo)).ToList();
            lstProgramCourse.Insert(0, Obj);
            var ProgramCourse = lstProgramCourse.Select(S => new
            {
                ProgramCourseID = S.ProgramCourseID,
                CourseName = S.CourseCode + " - " + S.CourseName
            });
            string result = JsonConvert.SerializeObject(ProgramCourse, Formatting.Indented);
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