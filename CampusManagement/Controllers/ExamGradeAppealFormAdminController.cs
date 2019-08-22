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


    public class ExamGradeAppealFormAdminController : Controller
    {
        private ModelCMSNewContainer db = new ModelCMSNewContainer();
        ExamGradeAppealFormStudentListForAdminViewModel model = new ExamGradeAppealFormStudentListForAdminViewModel();
        // GET: ExamGradeAppealFormAdmin
        public ActionResult Index()
        {
            List<GetExam_Result> ExamList = new List<GetExam_Result>();
            ExamList = db.GetExam().ToList();
            if (ExamList == null || ExamList.Count == 0)
            {
                GetExam_Result GetExam_ResultObj = new GetExam_Result();
                GetExam_ResultObj.ExamID = 0;
                GetExam_ResultObj.ExamDetail = "Please Add Exam First";
                ExamList.Add(GetExam_ResultObj);
            }
            model.ExamID = ExamList[0].ExamID;
            ViewBag.ExamID = new SelectList(ExamList, "ExamID", "ExamDetail");

            return View(model);
        }


        [HttpPost]
        public ActionResult index(ExamGradeAppealFormStudentListForAdminViewModel egaf)
        {
            List<GetExam_Result> ExamList = new List<GetExam_Result>();
            ExamList = db.GetExam().ToList();
            if (ExamList == null || ExamList.Count == 0)
            {
                GetExam_Result GetExam_ResultObj = new GetExam_Result();
                GetExam_ResultObj.ExamID = 0;
                GetExam_ResultObj.ExamDetail = "Please Add Exam First";
                ExamList.Add(GetExam_ResultObj);
            }
            model.ExamID = ExamList[0].ExamID;
            ViewBag.ExamID = new SelectList(ExamList, "ExamID", "ExamDetail", egaf.ExamID);
            return View(egaf);
        }

        public ActionResult ExamGradeAppealRequestForm(int? ExamGradeAppealFormID, int? ExamID, int? StudentID)
        {
            ExamGradeAppealFormForAdminViewModel modelObj = new ExamGradeAppealFormForAdminViewModel();

            modelObj.ExamID = (int)ExamID;
            modelObj.ExamGradeAppealFormForStudentObj = db.GetExamGradeAppealFormForStudent(modelObj.ExamID, StudentID).FirstOrDefault();
            //modelObj.SubjectListAppeal = db.GetExamGradeAppealStudentCoursesForAdmin(ExamGradeAppealFormID, ExamID,StudentID).ToList();
            modelObj.StudentDetailObj = db.GetStudentDetailByStudentID(StudentID).FirstOrDefault();

            //string ProgramCourseList = "";
            //// Session["CurrentUserID"] = "9005";
            //if (modelObj.ExamGradeAppealFormForStudentObj == null)
            //{
            //    modelObj.ExamGradeAppealFormForStudentObj = db.GetExamGradeAppealFormForStudent(modelObj.ExamID, Convert.ToInt32(Session["CurrentUserID"])).FirstOrDefault();
            //    modelObj.ExamGradeAppealFormForStudentObj.ExamID = modelObj.ExamID;
            //    modelObj.ExamGradeAppealFormForStudentObj.StudentID = modelObj.SubjectList[0].StudentID;
            //    modelObj.StudentDetailObj = db.GetStudentDetailByStudentID(modelObj.SubjectList[0].StudentID).FirstOrDefault();

            //    for (int i = 0; i < modelObj.SubjectList.Count; i++)
            //    {
            //        if (i == modelObj.SubjectList.Count - 1)
            //        {
            //            ProgramCourseList = ProgramCourseList + modelObj.SubjectList[i].ProgramCourseID;
            //        }
            //        else
            //        {
            //            ProgramCourseList = ProgramCourseList + modelObj.SubjectList[i].ProgramCourseID + ",";
            //        }


            //    }
            //    if (Session["ProgramCourseList"] == null)
            //    {
            //        Session["ProgramCourseList"] = ProgramCourseList;
            //    }
            //    modelObj.SubjectListAppeal = db.GetExamGradeAppealStudentCourses(modelObj.ExamID, modelObj.SubjectList[0].StudentID, ProgramCourseList).ToList();


            //}
            //else
            //{
            //    db.InsertOrUpdateExamGradeAppealForm(modelObj.ExamGradeAppealFormForStudentObj.ExamGradeAppealFormID,
            //    modelObj.ExamGradeAppealFormForStudentObj.ExamID,
            //    modelObj.ExamGradeAppealFormForStudentObj.StudentID,
            //    modelObj.ExamGradeAppealFormForStudentObj.StudentSignature,
            //    modelObj.ExamGradeAppealFormForStudentObj.IsStudentSignature,
            //    modelObj.ExamGradeAppealFormForStudentObj.StudentComment,
            //    modelObj.ExamGradeAppealFormForStudentObj.ApproveComment,
            //    modelObj.ExamGradeAppealFormForStudentObj.NotApprove,
            //    modelObj.ExamGradeAppealFormForStudentObj.HodComment,
            //    modelObj.ExamGradeAppealFormForStudentObj.HodSignature,
            //    modelObj.ExamGradeAppealFormForStudentObj.IsHodSignature,
            //    modelObj.ExamGradeAppealFormForStudentObj.CreatedOn,
            //    Convert.ToInt32(Session["CurrentUserID"]),
            //    modelObj.ExamGradeAppealFormForStudentObj.IsActive,
            //    modelObj.ExamGradeAppealFormForStudentObj.ModifiedOn,
            //    Convert.ToInt32(Session["CurrentUserID"]));
            //    modelObj.ExamGradeAppealFormForStudentObj = db.GetExamGradeAppealFormForStudent(modelObj.ExamID, Convert.ToInt32(Session["CurrentUserID"])).FirstOrDefault();
            //    modelObj.StudentDetailObj = db.GetStudentDetailByStudentID(modelObj.ExamGradeAppealFormForStudentObj.StudentID).FirstOrDefault();

            //    for (int x = 0; x < modelObj.SubjectListAppeal.Count; x++)
            //    {
            //        db.InsertOrUpdateExamGradeAppealFormCourseDetail(modelObj.SubjectListAppeal[x].ExamGradeAppealFormCourseDetailID, modelObj.ExamGradeAppealFormForStudentObj.ExamGradeAppealFormID, modelObj.ExamGradeAppealFormForStudentObj.ExamID, modelObj.ExamGradeAppealFormForStudentObj.StudentID, modelObj.SubjectListAppeal[x].ProgramCourseID, modelObj.SubjectListAppeal[x].TeacherID, modelObj.SubjectListAppeal[x].PaperRecheking, DateTime.Now, Convert.ToInt32(Session["CurrentUserID"]), "Yes", DateTime.Now, Convert.ToInt32(Session["CurrentUserID"]));
            //    }
            //    if (Session["ProgramCourseList"] != null)
            //    {
            //        ProgramCourseList = (string)Session["ProgramCourseList"];
            //    }
            //    modelObj.SubjectListAppeal = db.GetExamGradeAppealStudentCourses(modelObj.ExamGradeAppealFormForStudentObj.ExamID, modelObj.ExamGradeAppealFormForStudentObj.StudentID, ProgramCourseList).ToList();

            //    ViewBag.MessageType = "success";
            //    ViewBag.Message = "Data has been saved successfully.";
            //}

            return View(modelObj);
        }

        [HttpPost]
        public ActionResult ExamGradeAppealRequestForm(ExamGradeAppealFormForAdminViewModel modelObj)
        {
            db.InsertOrUpdateExamGradeAppealForm(modelObj.ExamGradeAppealFormForStudentObj.ExamGradeAppealFormID,
                    modelObj.ExamGradeAppealFormForStudentObj.ExamID,
                    modelObj.ExamGradeAppealFormForStudentObj.StudentID,
                    modelObj.ExamGradeAppealFormForStudentObj.StudentSignature,
                    modelObj.ExamGradeAppealFormForStudentObj.IsStudentSignature,
                    modelObj.ExamGradeAppealFormForStudentObj.StudentComment,
                    modelObj.ExamGradeAppealFormForStudentObj.ApproveComment,
                    modelObj.ExamGradeAppealFormForStudentObj.NotApprove,
                    modelObj.ExamGradeAppealFormForStudentObj.HodComment,
                    modelObj.ExamGradeAppealFormForStudentObj.HodSignature,
                    modelObj.ExamGradeAppealFormForStudentObj.IsHodSignature,
                    modelObj.ExamGradeAppealFormForStudentObj.CreatedOn,
                    Convert.ToInt32(Session["CurrentUserID"]),
                    modelObj.ExamGradeAppealFormForStudentObj.IsActive,
                    modelObj.ExamGradeAppealFormForStudentObj.ModifiedOn,
                    Convert.ToInt32(Session["CurrentUserID"]));

            modelObj.StudentDetailObj = db.GetStudentDetailByStudentID(modelObj.SubjectListAppeal[0].StudentID).FirstOrDefault();
            return View(modelObj);
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