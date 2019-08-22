using CampusManagement.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;


namespace CampusManagement.Controllers
{
    public class ExamAwardListTeacherDisplayController : Controller
    {
        private ModelCMSNewContainer db = new ModelCMSNewContainer();
        ExamSubjectsViewModel model = new ExamSubjectsViewModel();
        ExamSubjectsStudentSecondViewModel modelstudentlist = new ExamSubjectsStudentSecondViewModel();
        ExamResultForDisplayViewModel modelExamResultForDisplay = new ExamResultForDisplayViewModel();
        // GET: ExamAwardListTeacherDisplay
        public ActionResult Index()
        {
            ViewBag.ExamID = new SelectList(db.Exams, "ExamID", "ExamTitle");
            ViewBag.MessageType = "";
            ViewBag.Message = "";
            return View(model);
        }

        [HttpPost]
        public ActionResult Index(ExamSubjectsViewModel ES)
        {
            ViewBag.hdnExamID = ES.ExamID;
            ViewBag.MessageType = "";
            ViewBag.Message = "";
            ViewBag.ExamID = new SelectList(db.Exams, "ExamID", "ExamTitle");

            return View(ES);
        }

        public ActionResult ExamSubjectsStudent(int? ExamID, int? ProgramCourseID, int? ExamsDateSheetDetailID, int? TeacherID)
        {

            modelstudentlist.StudeltListSub = db.GetExamSubjectsStudentForResult(ExamID, ProgramCourseID).ToList();
            //ViewBag.hfExamID = ExamID;
            //ViewBag.hfProgramCourseID = ProgramCourseID;
            //ViewBag.hfExamsDateSheetDetailID = ExamsDateSheetDetailID;
            //ViewBag.hfTeacherID = TeacherID;
            modelstudentlist.ExamID = (int)ExamID;
            modelstudentlist.ProgramCourseID = (int)ProgramCourseID;
            modelstudentlist.ExamsDateSheetDetailID = (int)ExamsDateSheetDetailID;
            modelstudentlist.TeacherID = (int)TeacherID;

            ExamSubStudentViewModel LocalObject = new ExamSubStudentViewModel();
            List<ExamSubStudentViewModel> list = new List<ExamSubStudentViewModel>();

            for (int i = 0; i < modelstudentlist.StudeltListSub.Count; i++)
            {
                LocalObject = new ExamSubStudentViewModel();
                LocalObject.StudentObj = new GetExamSubjectsStudentForResult_Result();
                LocalObject.StudentObj = (GetExamSubjectsStudentForResult_Result)modelstudentlist.StudeltListSub[i];
                list.Add(LocalObject);
                // modelstudentlistFilter.StudeltList.Insert(i,LocalObject);
            }
            modelstudentlist.StudeltList = (List<ExamSubStudentViewModel>)list;
            return View(modelstudentlist);
        }


        public ActionResult ExamGradeFormRequestApproveUpdate(ExamSubjectsStudentSecondViewModel modelstudentlist2)
        {
            for (int i = 0; i < modelstudentlist2.StudeltList.Count; i++)
            {

                //modelstudentlist2.StudeltList[i].ExamResultForDisplayObj.

                if (modelstudentlist2.ExamResultApproveObj.TeacherName != null && modelstudentlist2.ExamResultApproveObj.TeacherApproveOrDisapproveDate == null)
                {
                    modelstudentlist2.ExamResultApproveObj.TeacherApproveOrDisapproveDate = DateTime.Now;
                }

                if (modelstudentlist2.ExamResultApproveObj.HodName != null && modelstudentlist2.ExamResultApproveObj.HodApproveOrDisapproveDate == null)
                {
                    modelstudentlist2.ExamResultApproveObj.HodApproveOrDisapproveDate = DateTime.Now;
                }

                db.UpdateExamGradFormApproval(modelstudentlist2.StudeltList[i].ExamResultForDisplayObj.ExamStudentSubjectMarkID,
                    modelstudentlist2.ExamResultApproveObj.TeacherName,
                    modelstudentlist2.ExamResultApproveObj.IsTeacherApproveOrDisapprove,
                    modelstudentlist2.ExamResultApproveObj.TeacherApproveOrDisapproveDate,
                    modelstudentlist2.ExamResultApproveObj.HodName,
                    modelstudentlist2.ExamResultApproveObj.IsHodApproveOrDisapprove,
                    modelstudentlist2.ExamResultApproveObj.HodApproveOrDisapproveDate,
                    modelstudentlist2.StudeltList[i].ExamResultForDisplayObj.IsTeacherStudentApprove,
                    modelstudentlist2.StudeltList[i].ExamResultForDisplayObj.IsHodStudentApprove,
                    Convert.ToInt32(Session["CurrentUserID"]));
            }


            return RedirectToAction("ExamSubjectsStudent", new { ExamID = modelstudentlist2.ExamID, ProgramCourseID = modelstudentlist2.ProgramCourseID, ExamsDateSheetDetailID = modelstudentlist2.ExamsDateSheetDetailID, TeacherID = modelstudentlist2.TeacherID });
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