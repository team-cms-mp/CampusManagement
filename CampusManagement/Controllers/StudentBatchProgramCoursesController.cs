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
    [Authorize]
    public class StudentBatchProgramCoursesController : Controller
    {
        private ModelCMSNewContainer db = new ModelCMSNewContainer();
        public ActionResult Index(int? StudentID)
        {
            Student stu = new Student();
            int EmpID = Convert.ToInt32(Session["emp_id"]);
            if (EmpID > 0)
            {
                string FormNo = "";
                if (Session["FormNo"] != null)
                {
                    if (!string.IsNullOrEmpty(Session["FormNo"].ToString()))
                    {
                        FormNo = Session["FormNo"].ToString();
                        stu = db.Students.FirstOrDefault(s => s.FormNo == FormNo);
                    }
                }

                ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc");
                ViewBag.BatchProgramID = new SelectList(db.GetBatchProgramNameConcat("", 0), "ID", "Name");
                ViewBag.YearSemesterNo = new SelectList(db.Semesters.OrderBy(s => s.YearSemesterNo), "YearSemesterNo", "YearSemesterNo");
                ViewBag.StudentID = stu.StudentID;
                ViewBag.MessageType = "";
                ViewBag.Message = "";
                return View();
            }
            else
            {
                return RedirectToAction("Login2", "Home");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(StudentBatchProgramCourse studentBatchProgramCourse)
        {
            if (studentBatchProgramCourse.StudentID == null)
            {
                studentBatchProgramCourse.StudentID = 0;
            }

            ViewBag.BatchProgramID = new SelectList(db.GetBatchProgramNameConcat("", 0), "ID", "Name", studentBatchProgramCourse.BatchProgramID);
            ViewBag.YearSemesterNo = new SelectList(db.Semesters.OrderBy(s => s.YearSemesterNo), "YearSemesterNo", "YearSemesterNo", studentBatchProgramCourse.YearSemesterNo);
            ViewBag.hdnBatchProgramID = studentBatchProgramCourse.BatchProgramID;
            ViewBag.hdnYearSemesterNo = studentBatchProgramCourse.YearSemesterNo;
            ViewBag.StudentID = studentBatchProgramCourse.StudentID;

            return View("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitForSubjectsApproval(StudentBatchProgramCourse studentBatchProgramCourse)
        {
            int EmpID = Convert.ToInt32(Session["emp_id"]);
            string error = "";
            if (EmpID == 0)
            {
                return RedirectToAction("Login2", "Home");
            }
            else
            {
                if (studentBatchProgramCourse.BatchProgramID == null || studentBatchProgramCourse.BatchProgramID == 0)
                {
                    error += "BatchProgramID";
                }
                if (studentBatchProgramCourse.YearSemesterNo == null || studentBatchProgramCourse.YearSemesterNo == 0)
                {
                    error += "YearSemesterNo";
                }
                if (studentBatchProgramCourse.StudentID == null || studentBatchProgramCourse.StudentID == 0)
                {
                    error += "StudentID";
                }

                if (string.IsNullOrEmpty(error))
                {
                    db.UpdateStudentStatus(studentBatchProgramCourse.BatchProgramID, studentBatchProgramCourse.YearSemesterNo, studentBatchProgramCourse.StudentID, 0, 0);
                }
            }

            return RedirectToAction("Index");
        }

        public ActionResult StudentSubjectsApproval()
        {
            int EmpID = Convert.ToInt32(Session["emp_id"]);
            if (EmpID == 0)
            {
                return RedirectToAction("Login2", "Home");
            }
            else
            {
                List<GetStudentSubjects_Result> lstSubjects = db.GetStudentSubjects(0, 0, "Subjects Approval", 0).ToList();
                return View(lstSubjects);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult StudentSubjectsApproval(int? BatchID, int? BatchProgramID, int? YearSemesterNo)
        {
            int EmpID = Convert.ToInt32(Session["emp_id"]);
            if (EmpID == 0)
            {
                return RedirectToAction("Login2", "Home");
            }
            else
            {
                if (BatchID == null)
                {
                    BatchID = 0;
                }

                if (BatchProgramID == null)
                {
                    BatchProgramID = 0;
                }

                if (YearSemesterNo == null)
                {
                    YearSemesterNo = 0;
                }

                ViewBag.hdnBatchID = BatchID;
                ViewBag.hdnBatchProgramID = BatchProgramID;
                ViewBag.hdnYearSemesterNo = YearSemesterNo;

                List<GetStudentSubjects_Result> lstSubjects = db.GetStudentSubjects(BatchProgramID, YearSemesterNo, "Subjects Approval", 0).ToList();
                return View(lstSubjects);
            }
        }

        public ActionResult ApproveStudentSubjects(int? BatchProgramID, int? YearSemesterNo, int? StudentID, int? CourseID)
        {
            int EmpID = Convert.ToInt32(Session["emp_id"]);
            string error = "";
            if (EmpID == 0)
            {
                return RedirectToAction("Login2", "Home");
            }
            else
            {
                if (BatchProgramID == null)
                {
                    BatchProgramID = 0;
                    error += "BatchProgramID is empty.";
                }

                if (YearSemesterNo == null)
                {
                    YearSemesterNo = 0;
                    error += "YearSemesterNo is empty.";
                }

                if (StudentID == null)
                {
                    StudentID = 0;
                    error += "StudentID is empty.";
                }

                if (CourseID == null)
                {
                    CourseID = 0;
                    error += "CourseID is empty.";
                }

                if (string.IsNullOrEmpty(error))
                {
                    db.UpdateStudentStatus(BatchProgramID, YearSemesterNo, StudentID, CourseID, 1);
                }
            }
            return RedirectToAction("StudentSubjectsApproval");
        }

        public ActionResult StudentSubjectsApproved()
        {
            int EmpID = Convert.ToInt32(Session["emp_id"]);
            if (EmpID == 0)
            {
                return RedirectToAction("Login2", "Home");
            }
            else
            {
                List<GetStudentSubjects_Result> lstSubjects = db.GetStudentSubjects(0, 0, "Subjects Approved", 0).ToList();
                return View(lstSubjects);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult StudentSubjectsApproved(int? BatchID, int? BatchProgramID, int? YearSemesterNo)
        {
            int EmpID = Convert.ToInt32(Session["emp_id"]);
            if (EmpID == 0)
            {
                return RedirectToAction("Login2", "Home");
            }
            else
            {
                if (BatchID == null)
                {
                    BatchID = 0;
                }

                if (BatchProgramID == null)
                {
                    BatchProgramID = 0;
                }

                if (YearSemesterNo == null)
                {
                    YearSemesterNo = 0;
                }

                ViewBag.hdnBatchID = BatchID;
                ViewBag.hdnBatchProgramID = BatchProgramID;
                ViewBag.hdnYearSemesterNo = YearSemesterNo;

                List<GetStudentSubjects_Result> lstSubjects = db.GetStudentSubjects(BatchProgramID, YearSemesterNo, "Subjects Approved", 0).ToList();
                return View(lstSubjects);
            }
        }

        // Get Active Batches
        public JsonResult GetActiveBatches()
        {
            List<Batch> lstBatches = new List<Batch>();

            lstBatches = db.Batches.Where(b => b.IsActive == "Yes").ToList();
            var batches = lstBatches.Select(b => new
            {
                BatchID = b.BatchID,
                BatchName = b.BatchName,
                BatchSession = b.BatchSession,
                BatchCode = b.BatchCode
            });
            string result = JsonConvert.SerializeObject(batches, Formatting.Indented);
            return Json(result, JsonRequestBehavior.AllowGet);
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

        [HttpGet]
        public ActionResult Assign(int? id, int? sID, int? YearSemesterNo)
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
                    List<StudentBatchProgramCourse> lstCheckCredit = new List<StudentBatchProgramCourse>();
                    int programID = 0;
                    int LevelID = 0;
                    int maxCredit = 0;
                    BatchProgram bp = db.BatchPrograms.FirstOrDefault(p => p.BatchProgramID == bpc.BatchProgramID);
                    if (bp != null)
                    {
                        programID = bp.ProgramID;
                        Program P = db.Programs.FirstOrDefault(p => p.ProgramID == programID);
                        LevelID = Convert.ToInt32(P.LevelID);
                        GeneralSetting GS = db.GeneralSettings.OrderByDescending(s => s.GeneralSettingID).FirstOrDefault();

                        LevelID = Convert.ToInt32(P.LevelID);
                        if (LevelID == 2)
                        {
                            maxCredit = Convert.ToInt32(GS.BachelorMaxCreditHours);
                        }
                        else if (LevelID == 3)
                        {
                            maxCredit = Convert.ToInt32(GS.MastersMaxCreditHours);
                        }
                        else if (LevelID == 4)
                        {
                            maxCredit = Convert.ToInt32(GS.MPhilMaxCreditHours);
                        }
                    }

                    lstCheckCredit = db.StudentBatchProgramCourses.Where(s => s.BatchProgramID == bpc.BatchProgramID && s.StudentID == sID && s.YearSemesterNo == YearSemesterNo).ToList();

                    int SumCreditHours = 0;


                    var CH = db.Courses.FirstOrDefault(a => a.CourseID == bpc.CourseID);

                    SumCreditHours += CH.CreditHours;

                    foreach (var item in lstCheckCredit)
                    {
                        SumCreditHours += item.Course.CreditHours;
                    }

                    if (SumCreditHours > maxCredit)
                    {
                        ViewBag.MessageType = "error";
                        ViewBag.Message = "You cannot exceed your credit hours more than " + maxCredit;

                        ViewBag.BatchProgramID = new SelectList(db.GetBatchProgramNameConcat("", 0), "ID", "Name", bpc.BatchProgramID);
                        ViewBag.YearSemesterNo = new SelectList(db.Semesters.OrderBy(s => s.YearSemesterNo), "YearSemesterNo", "YearSemesterNo", YearSemesterNo);
                        ViewBag.hdnBatchProgramID = bpc.BatchProgramID;
                        ViewBag.hdnYearSemesterNo = YearSemesterNo;
                        ViewBag.StudentID = sID;
                        return View("Index");
                    }
                    else
                    {
                        StudentBatchProgramCourse sbpc = db.StudentBatchProgramCourses.FirstOrDefault(a => a.BatchProgramID == bpc.BatchProgramID && a.YearSemesterNo == YearSemesterNo && a.ProgramCourseID == bpc.ProgramCourseID && a.StudentID == sID && a.CourseID == bpc.CourseID);
                        if (sbpc != null)
                        {
                            ViewBag.MessageType = "error";
                            ViewBag.Message = "Subject is already added.";
                        }
                        else
                        {
                            bpc.CreatedBy = Convert.ToInt32(Session["emp_id"]);
                            db.InsertStudentBatchProgramCourse(bpc.ProgramCourseID, sID, bpc.CourseID
                                , YearSemesterNo, bpc.BatchProgramID, bpc.CreatedBy, bpc.IsActive, 0);
                            ViewBag.MessageType = "success";
                            ViewBag.Message = "Data has been saved successfully.";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.MessageType = "error";
                ViewBag.Message = ex.Message;
            }
            ViewBag.BatchProgramID = new SelectList(db.GetBatchProgramNameConcat("", 0), "ID", "Name", bpc.BatchProgramID);
            ViewBag.YearSemesterNo = new SelectList(db.Semesters.OrderBy(s => s.YearSemesterNo), "YearSemesterNo", "YearSemesterNo", YearSemesterNo);
            ViewBag.hdnBatchProgramID = bpc.BatchProgramID;
            ViewBag.hdnYearSemesterNo = YearSemesterNo;
            ViewBag.StudentID = sID;
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
            ViewBag.YearSemesterNo = new SelectList(db.Semesters.OrderBy(s => s.YearSemesterNo), "YearSemesterNo", "YearSemesterNo");
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