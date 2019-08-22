using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CampusManagement.Models;
using Newtonsoft.Json;
using System.Data.SqlClient;

namespace CampusManagement.Controllers
{
    [Authorize(Roles = "Account Officer,Accounts Officer,Admin Assistant,Admin Officer,Admin.Assistant,Assist. Account Officer,Assist.Technician,Import Manager,Manager Servive & Support,Office Manager,Officer QMS,RSM - Center 2,RSM - South,Sales & Service Executive,Sales Executive,Sales Manager,Sales Representative,Sr.Accounts Officer,Sr.Associate Engineer,Sr.Sales Executive,Sr.Sales Representative,Store Assistant,Store Incharge,Technician")]
    public class StudentBatchProgramCoursesController : Controller
    {
        private ModelCMSContainer db = new ModelCMSContainer();
        public ActionResult Index()
        {
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc");
            ViewBag.BatchProgramID = new SelectList(db.GetBatchProgramNameConcat("", 0), "ID", "Name");
            ViewBag.YearSemesterNo = new SelectList(db.Semesters, "YearSemesterNo", "YearSemesterNo");
            ViewBag.hdnBatchProgramID = 0;
            ViewBag.hdnYearSemesterNo = 0;
            ViewBag.StudentID = 0;
            ViewBag.MessageType = "";
            ViewBag.Message = "";
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(StudentBatchProgramCourse studentBatchProgramCourse)
        {
            if(studentBatchProgramCourse.StudentID == null)
            {
                studentBatchProgramCourse.StudentID = 0;
            }

            ViewBag.BatchProgramID = new SelectList(db.GetBatchProgramNameConcat("", 0), "ID", "Name", studentBatchProgramCourse.BatchProgramID);
            ViewBag.YearSemesterNo = new SelectList(db.Semesters, "YearSemesterNo", "YearSemesterNo", studentBatchProgramCourse.YearSemesterNo);
            ViewBag.hdnBatchProgramID = studentBatchProgramCourse.BatchProgramID;
            ViewBag.hdnYearSemesterNo = studentBatchProgramCourse.YearSemesterNo;
            ViewBag.StudentID = studentBatchProgramCourse.StudentID;

            return View("Index");
        }

        public JsonResult GetBatchProgramSemesterList(string BatchProgramID)
        {
            List<BatchProgramSemester> lstSemester = new List<BatchProgramSemester>();
            int bpId = Convert.ToInt32(BatchProgramID);

            lstSemester = db.BatchProgramSemesters.Where(s => s.BatchProgramID == bpId).OrderByDescending(b => b.YearSemesterNo).ToList();
            var semesters = lstSemester.Select(S => new
            {
                YearSemesterNo = S.YearSemesterNo
            });
            string result = JsonConvert.SerializeObject(semesters, Formatting.Indented);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Assign(int? id, int? sID)
        {
            BatchProgramCourse bpc = new BatchProgramCourse();
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                bpc = db.BatchProgramCourses.Find(id);
                if (bpc == null)
                {
                    return HttpNotFound();
                }
                else
                {
                    bpc.CreatedBy = Convert.ToInt32(Session["emp_id"]);
                    db.InsertStudentBatchProgramCourse(bpc.ProgramCourseID, sID, bpc.CourseID
                        , bpc.YearSemesterNo, bpc.BatchProgramID, bpc.CreatedBy, bpc.IsActive, 0);
                    ViewBag.MessageType = "success";
                    ViewBag.Message = "Data has been saved successfully.";
                    ViewBag.BatchProgramID = new SelectList(db.GetBatchProgramNameConcat("", 0), "ID", "Name", bpc.BatchProgramID);
                    ViewBag.YearSemesterNo = new SelectList(db.Semesters, "YearSemesterNo", "YearSemesterNo", bpc.YearSemesterNo);
                    ViewBag.hdnBatchProgramID = bpc.BatchProgramID;
                    ViewBag.hdnYearSemesterNo = bpc.YearSemesterNo;
                    ViewBag.StudentID = sID;
                }
            }
            catch (Exception ex)
            {
                ViewBag.MessageType = "error";
                ViewBag.Message = ex.Message;
            }
            
            return View("Index");
        }

        [HttpGet]
        public ActionResult Unassign(int? id, int? sID)
        {
            int? BatchProgramID = 0;
            int? YearSemesterNo = 0;
            StudentBatchProgramCourse sbpc = new StudentBatchProgramCourse();
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                sbpc = db.StudentBatchProgramCourses.Find(id);
                if (sbpc == null)
                {
                    return HttpNotFound();
                }
                else
                {
                    BatchProgramID = sbpc.BatchProgramID;
                    YearSemesterNo = sbpc.YearSemesterNo;
                    db.StudentBatchProgramCourses.Remove(sbpc);
                    db.SaveChanges();
                    ViewBag.MessageType = "success";
                    ViewBag.Message = "Record has been removed successfully.";
                }
            }
            catch (Exception ex)
            {
                ViewBag.MessageType = "error";
                ViewBag.Message = ex.Message;
            }
            ViewBag.BatchProgramID = new SelectList(db.GetBatchProgramNameConcat("", 0), "ID", "Name");
            ViewBag.YearSemesterNo = new SelectList(db.Semesters, "YearSemesterNo", "YearSemesterNo");
            ViewBag.hdnBatchProgramID = BatchProgramID;
            ViewBag.hdnYearSemesterNo = YearSemesterNo;
            ViewBag.StudentID = sID;

            return View("Index");
        }

        public JsonResult GetStudentsList(string searchValue, string BatchProgramID)
        {
            List<GetBatchProgramNameConcat_Result> lstStudent = db.GetBatchProgramNameConcat(BatchProgramID, 7).Where(s => s.Name.Contains(searchValue)).ToList();

            var Students = lstStudent.Select(s => new
            {
                StudentID = s.ID,
                Name = s.Name
            });

            string result = JsonConvert.SerializeObject(Students, Formatting.Indented);
            return new JsonResult { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
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
