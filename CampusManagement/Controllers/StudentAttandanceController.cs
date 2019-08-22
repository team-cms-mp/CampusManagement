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
    public class StudentAttandanceController : Controller
    {
        private ModelCMSNewContainer db = new ModelCMSNewContainer();
        // GET: StudentAttandance
        public ActionResult ViewMarkAttendance()
        {
            return View();
        }

        public ActionResult ViewAttendance()
        {
            int CurrentUserID = Convert.ToInt32(Session["CurrentUserID"]);
            List<GetTeacherActiveCources_by_TeacherID_Result> TeacherActiveCourcesList = new List<GetTeacherActiveCources_by_TeacherID_Result>();
            TeacherActiveCourcesList = db.GetTeacherActiveCources_by_TeacherID(CurrentUserID).ToList();
            return View(TeacherActiveCourcesList);
        }

        public ActionResult AddAttendance()
        {
            int CurrentUserID = Convert.ToInt32(Session["CurrentUserID"]);
            List<GetTeacherActiveCources_by_TeacherID_Result> TeacherActiveCourcesList = new List<GetTeacherActiveCources_by_TeacherID_Result>();
            TeacherActiveCourcesList = db.GetTeacherActiveCources_by_TeacherID(CurrentUserID).ToList();
            return View(TeacherActiveCourcesList);
        }

        public ActionResult MarkAttendance(int? ProgramCourseID, int? CourseID, int? BatchProgramID, int? TeacherID, int? YearSemesterNo, string MessageType, string Message)
        {
            if (CourseID == null || BatchProgramID == null || TeacherID == null || YearSemesterNo == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (MessageType != null || Message != null)
            {
                ViewBag.MessageType = MessageType;
                ViewBag.Message = Message;
            }
            ViewBag.ProgramCourseID = ProgramCourseID;
            ViewBag.CourseID = CourseID;
            ViewBag.BatchProgramID = BatchProgramID;
            ViewBag.TeacherID = TeacherID;
            ViewBag.YearSemesterNo = YearSemesterNo;

            ViewBag.PresentStatus = new SelectList(db.StudentPresentStatus, "StudentPresentStatusID", "StudentPresentStatusName");

            return View();
        }

        public ActionResult MarkAttendanceManual(int? ProgramCourseID, int? CourseID, int? BatchProgramID, int? TeacherID, int? YearSemesterNo, string MessageType, string Message)
        {
            if (CourseID == null || BatchProgramID == null || TeacherID == null || YearSemesterNo == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (MessageType != null || Message != null)
            {
                ViewBag.MessageType = MessageType;
                ViewBag.Message = Message;
            }
            ViewBag.ProgramCourseID = ProgramCourseID;
            ViewBag.CourseID = CourseID;
            ViewBag.BatchProgramID = BatchProgramID;
            ViewBag.TeacherID = TeacherID;
            ViewBag.YearSemesterNo = YearSemesterNo;

            ViewBag.PresentStatus = new SelectList(db.StudentPresentStatus, "StudentPresentStatusID", "StudentPresentStatusName");

            return View();
        }


        public ActionResult CreateAttendance(FormCollection fc)
        {
            int count = Convert.ToInt32(fc["hdnRowCount"]);
            int StudentID = 0;
            string CourseID = fc["CourseID"];
            string BatchProgramID = fc["BatchProgramID"];
            string ProgramCourseID = fc["ProgramCourseID"];
            string TeacherID = fc["TeacherID"];
            string YearSemesterNo = fc["YearSemesterNo"];
            string StudentName = fc["StudentName"];
            string FatherName = fc["FatherName"];
            string PresentStatus = fc["PresentStatus"];
            List<InsertRollBack> InsertRollBackList = new List<InsertRollBack>();
            InsertRollBack InsertRollBackObj;
            Boolean IsToRollBack = false;
            int ResultValue = Convert.ToInt32(db.GetStudentAttandanceExist(Convert.ToInt32(BatchProgramID), Convert.ToInt32(YearSemesterNo), Convert.ToInt32(CourseID), Convert.ToInt32(TeacherID), DateTime.Now).FirstOrDefault());
            if (ResultValue != 1)
            {
                for (int i = 1; i <= count; i++)
                {
                    InsertStudentAttandance_Result InsertClass = new InsertStudentAttandance_Result();
                    StudentAttandance List = new StudentAttandance();
                    StudentID = Convert.ToInt32(fc["StudentID_" + i]);
                    StudentName = (fc["StudentName_" + i]);
                    FatherName = (fc["FatherName_" + i]);
                    PresentStatus = (fc["PresentStatus_" + i]);
                    int PrimeryIDValue = Convert.ToInt32(db.InsertStudentAttandance(Convert.ToInt32(BatchProgramID), Convert.ToInt32(YearSemesterNo), Convert.ToInt32(CourseID), StudentID, Convert.ToInt32(TeacherID), Convert.ToInt32(PresentStatus), DateTime.Now, DateTime.Now, Convert.ToInt32(Session["emp_id"]), "Yes", DateTime.Now, Convert.ToInt32(Session["emp_id"])).FirstOrDefault());
                    if (PrimeryIDValue > 0)
                    {
                        InsertRollBackObj = new InsertRollBack();
                        InsertRollBackObj.InsertID = PrimeryIDValue;
                        InsertRollBackList.Add(InsertRollBackObj);
                    }
                    else
                    {
                        IsToRollBack = true;
                        break;
                    }
                }
                if (IsToRollBack == true)
                {

                    foreach (InsertRollBack obj in InsertRollBackList)
                    {
                        // Delete All List // Delete All List  
                        db.DeleteStudentAttandance_ByID(obj.InsertID);

                    }
                    IsToRollBack = false;

                    // Display Message Please Try Again Action Was Roll Back Due To Some Reason
                    ViewBag.MessageType = "error";
                    ViewBag.Message = "Uncertain Error! Please try again";
                }
                else
                {

                    // Display Message Successfully Added
                    ViewBag.MessageType = "success";
                    ViewBag.Message = "Attendance has been sucessfully added";
                }
            }
            else
            {
                // Display Message Already Attandance Added
                ViewBag.MessageType = "error";
                ViewBag.Message = "Attendance already done";

            }

            return RedirectToAction("MarkAttendance", new { ProgramCourseID = Convert.ToInt32(ProgramCourseID), CourseID = Convert.ToInt32(CourseID), BatchProgramID = Convert.ToInt32(BatchProgramID), TeacherID = Convert.ToInt32(TeacherID), YearSemesterNo = Convert.ToInt32(YearSemesterNo), MessageType = ViewBag.MessageType, Message = ViewBag.Message });
        }


        public ActionResult CreateManualAttendance(FormCollection fc)
        {
            int count = Convert.ToInt32(fc["hdnRowCount"]);
            int StudentID = 0;
            DateTime PresentDate;
            string CourseID = fc["CourseID"];
            string BatchProgramID = fc["BatchProgramID"];
            string ProgramCourseID = fc["ProgramCourseID"];
            string TeacherID = fc["TeacherID"];
            string YearSemesterNo = fc["YearSemesterNo"];
            string StudentName = fc["StudentName"];
            string FatherName = fc["FatherName"];
            string PresentStatus = fc["PresentStatus"];
            if (fc["PresentDate"] != "")
            {
                PresentDate = Convert.ToDateTime(fc["PresentDate"]);
            }
            else
            {
                ViewBag.MessageType = "error";
                ViewBag.Message = "Please select present date before you proceed!";
                return RedirectToAction("MarkAttendanceManual", new { CourseID = Convert.ToInt32(CourseID), BatchProgramID = Convert.ToInt32(BatchProgramID), TeacherID = Convert.ToInt32(TeacherID), YearSemesterNo = Convert.ToInt32(YearSemesterNo), MessageType = ViewBag.MessageType, Message = ViewBag.Message });
            }

            List<InsertRollBack> InsertRollBackList = new List<InsertRollBack>();
            InsertRollBack InsertRollBackObj;
            Boolean IsToRollBack = false;
            int ResultValue = Convert.ToInt32(db.GetStudentAttandanceExist(Convert.ToInt32(BatchProgramID), Convert.ToInt32(YearSemesterNo), Convert.ToInt32(CourseID), Convert.ToInt32(TeacherID), PresentDate).FirstOrDefault());
            if (ResultValue != 1)
            {
                for (int i = 1; i <= count; i++)
                {
                    InsertStudentAttandance_Result InsertClass = new InsertStudentAttandance_Result();
                    StudentAttandance List = new StudentAttandance();
                    StudentID = Convert.ToInt32(fc["StudentID_" + i]);
                    StudentName = (fc["StudentName_" + i]);
                    FatherName = (fc["FatherName_" + i]);
                    PresentStatus = (fc["PresentStatus_" + i]);
                    int PrimeryIDValue = Convert.ToInt32(db.InsertStudentAttandance(Convert.ToInt32(BatchProgramID), Convert.ToInt32(YearSemesterNo), Convert.ToInt32(CourseID), StudentID, Convert.ToInt32(TeacherID), Convert.ToInt32(PresentStatus), PresentDate, DateTime.Now, Convert.ToInt32(Session["emp_id"]), "Yes", DateTime.Now, Convert.ToInt32(Session["emp_id"])).FirstOrDefault());
                    db.SaveChanges();
                    if (InsertClass.PrimeryIDValue > 0)
                    {
                        InsertRollBackObj = new InsertRollBack();
                        InsertRollBackObj.InsertID = PrimeryIDValue;
                        InsertRollBackList.Add(InsertRollBackObj);
                    }
                    else
                    {
                        IsToRollBack = true;
                        break;
                    }
                }
                if (IsToRollBack == true)
                {

                    foreach (InsertRollBack obj in InsertRollBackList)
                    {
                        // Delete All List // Delete All List  
                        db.DeleteStudentAttandance_ByID(obj.InsertID);

                    }
                    IsToRollBack = false;

                    // Display Message Please Try Again Action Was Roll Back Due To Some Reason
                    ViewBag.MessageType = "error";
                    ViewBag.Message = "Uncertain Error! Please try again";
                }
                else
                {
                    // Display Message Successfully Added
                    ViewBag.MessageType = "success";
                    ViewBag.Message = "Attendance has been sucessfully added";
                }
            }
            else
            {
                // Display Message Already Attandance Added
                ViewBag.MessageType = "error";
                ViewBag.Message = "Attendance already done";

            }

            return RedirectToAction("MarkAttendanceManual", new { ProgramCourseID = Convert.ToInt32(ProgramCourseID), CourseID = Convert.ToInt32(CourseID), BatchProgramID = Convert.ToInt32(BatchProgramID), TeacherID = Convert.ToInt32(TeacherID), YearSemesterNo = Convert.ToInt32(YearSemesterNo), MessageType = ViewBag.MessageType, Message = ViewBag.Message });
        }

        public ActionResult ActiveSubjects()
        {
            ViewBag.BatchID = new SelectList(db.Batches, "BatchID", "BatchName");
            ViewBag.BatchProgramID = new SelectList(db.GetBatchProgramNameConcat("", 0).ToList(), "ID", "Name");
            ViewBag.YearSemesterNo = new SelectList(db.Semesters, "YearSemesterNo", "YearSemesterNo");
            return View();
        }

        [HttpPost]
        public ActionResult ActiveSubjects(StudentAttandance sa)
        {
            string ErrorMessage = "";
            int count = 0;
            ViewBag.hdnBatchProgramID = sa.BatchProgramID;
            ViewBag.hdnYearSemesterNo = sa.YearSemesterNo;
            if (sa.BatchProgramID == 0)
            {
                count++;
                ErrorMessage += count + "-Please select program.<br/>";
            }
            if (sa.YearSemesterNo == 0)
            {
                count++;
                ErrorMessage += count + "-Please select semester.<br/>";
            }

            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ViewBag.MessageType = "error";
                ViewBag.Message = ErrorMessage;
            }

            ViewBag.BatchID = new SelectList(db.Batches, "BatchID", "BatchName");
            ViewBag.BatchProgramID = new SelectList(db.GetBatchProgramNameConcat("", 0), "ID", "Name");
            ViewBag.YearSemesterNo = new SelectList(db.Semesters, "YearSemesterNo", "YearSemesterNo");
            return View();
        }

        public ActionResult SubjectWiseStudentList(int? CourseID)
        {
            ViewBag.hdnCourseID = CourseID;
            return View();
        }

        public ActionResult ActiveCoursesByStudentID(int? StudentID)
        {
            if (StudentID == null)
            {
                StudentID = Convert.ToInt32(Session["CurrentUserID"]);
            }
            ViewBag.hdnStudentID = StudentID;
            return View();
        }

        public ActionResult StudentAttendanceDetail(int? StudentID, int? CourseID)
        {
            ViewBag.hdnStudentID = StudentID;
            ViewBag.hdnCourseID = CourseID;
            return View();
        }

        public ActionResult StudentAttendanceSummary(int? StudentID, int? CourseID)
        {
            return View(db.GetStudentAttendanceSummaryByID(StudentID, CourseID).OrderByDescending(x => x.FullName).ToList());
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