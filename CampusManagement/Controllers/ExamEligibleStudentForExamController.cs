using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using CampusManagement.Models;
using Newtonsoft.Json;


namespace CampusManagement.Controllers
{
    public class ExamEligibleStudentForExamController : Controller
    {
        private ModelCMSNewContainer db = new ModelCMSNewContainer();
        StudentListForExamEligibilityViewModel model = new StudentListForExamEligibilityViewModel();
        // GET: ExamEligibleStudentForExam
        public ActionResult Index()
        {
            int CurrentUserID = Convert.ToInt32(Session["CurrentUserID"]);
            List<GetTeacherActiveCources_by_TeacherID_Result> TeacherActiveCourcesList = new List<GetTeacherActiveCources_by_TeacherID_Result>();
            TeacherActiveCourcesList = db.GetTeacherActiveCources_by_TeacherID(CurrentUserID).ToList();
            return View(TeacherActiveCourcesList);
        }


        public ActionResult LoadEligibleStudentsForExam(int? ProgramCourseID)
        {
            if (ProgramCourseID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            int CurrentUserID = Convert.ToInt32(Session["CurrentUserID"]);
            Exam ExamObj = new Exam();
            ExamObj = db.Exams.Where(e => e.IsActive == "Yes").FirstOrDefault();
            if (ExamObj != null)
            {
                model.ExamTitle = ExamObj.ExamTitle;
                model.ExamID = ExamObj.ExamID;
                model.ProgramCourseID = ProgramCourseID;
                model.StudentList = db.GetStudentListForExamEligibility(CurrentUserID, ProgramCourseID, ExamObj.ExamID).ToList();
            }

            return View(model);
        }

        [HttpPost]
        public ActionResult CreateExamEligibleStudents(StudentListForExamEligibilityViewModel ListObj)
        {
            foreach (GetStudentListForExamEligibility_Result Obj in ListObj.StudentList)
            {
                db.InsertOrUpdateExamEligibleStudentForExam(Obj.ExamEligibleStudentForExamID, ListObj.ExamID, Obj.StudentBatchProgramCourseID, Convert.ToInt32(Obj.IsEligible), Obj.Remarks, DateTime.Now, Convert.ToInt32(Session["CurrentUserID"]), "Yes", DateTime.Now, Convert.ToInt32(Session["CurrentUserID"]));
            }

            return RedirectToAction("LoadEligibleStudentsForExam", new { ProgramCourseID = ListObj.ProgramCourseID });
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