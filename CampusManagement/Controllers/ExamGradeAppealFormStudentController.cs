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
    public class ExamGradeAppealFormStudentController : Controller
    {

        private ModelCMSNewContainer db = new ModelCMSNewContainer();
        ExamStudentSubjectForGradeAppealRequestViewModel model = new ExamStudentSubjectForGradeAppealRequestViewModel();
        // GET: ExamGradeAppealFormStudent
        public ActionResult Index()
        {
            Exam ExamObj = new Exam();
            ExamObj = db.Exams.Where(x => x.IsDateSheetApproved == 1 && x.IsActive == "Yes").FirstOrDefault();

            if (ExamObj != null)
            {
                ViewBag.ExamID = new SelectList(db.Exams, "ExamID", "ExamTitle", ExamObj.ExamID);
                ViewBag.hdnExamID = ExamObj.ExamID;
                model.ExamID = ExamObj.ExamID;
               // Session["CurrentUserID"] = "9005";
                if (Session["CurrentUserID"] != null)
                {
                    ViewBag.StudentUserID = Convert.ToInt32(Session["CurrentUserID"]);
                }
                else
                {
                    ViewBag.StudentUserID = -1;
                }

            }
            Session["ProgramCourseList"] = null;
            ViewBag.MessageType = "";
            ViewBag.Message = "";
            return View(model);
        }


        public ActionResult ExamGradeAppealRequestForm(ExamStudentSubjectForGradeAppealRequestViewModel modelObj)
        {
            string ProgramCourseList = "";
           // Session["CurrentUserID"] = "9005";
            if (modelObj.ExamGradeAppealFormForStudentObj == null)
            {
                modelObj.ExamGradeAppealFormForStudentObj = db.GetExamGradeAppealFormForStudent(modelObj.ExamID, Convert.ToInt32(Session["CurrentUserID"])).FirstOrDefault();
                modelObj.ExamGradeAppealFormForStudentObj.ExamID = modelObj.ExamID;
                modelObj.ExamGradeAppealFormForStudentObj.StudentID = modelObj.SubjectList[0].StudentID;
                modelObj.StudentDetailObj = db.GetStudentDetailByStudentID(modelObj.SubjectList[0].StudentID).FirstOrDefault();
                
                for (int i = 0; i < modelObj.SubjectList.Count; i++) {
                    if (i == modelObj.SubjectList.Count - 1)
                    {
                        ProgramCourseList = ProgramCourseList + modelObj.SubjectList[i].ProgramCourseID;
                    }
                    else {
                        ProgramCourseList = ProgramCourseList + modelObj.SubjectList[i].ProgramCourseID + ",";
                    }
                    
                    
                }
                if (Session["ProgramCourseList"] == null) {
                    Session["ProgramCourseList"] = ProgramCourseList;
                }
                modelObj.SubjectListAppeal = db.GetExamGradeAppealStudentCourses(modelObj.ExamID, modelObj.SubjectList[0].StudentID, ProgramCourseList).ToList();

                
            }
            else {
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
                    modelObj.ExamGradeAppealFormForStudentObj = db.GetExamGradeAppealFormForStudent(modelObj.ExamID, Convert.ToInt32(Session["CurrentUserID"])).FirstOrDefault();
                   modelObj.StudentDetailObj = db.GetStudentDetailByStudentID(modelObj.ExamGradeAppealFormForStudentObj.StudentID).FirstOrDefault();

                for (int x = 0; x < modelObj.SubjectListAppeal.Count; x++) {
                    db.InsertOrUpdateExamGradeAppealFormCourseDetail(modelObj.SubjectListAppeal[x].ExamGradeAppealFormCourseDetailID, modelObj.ExamGradeAppealFormForStudentObj.ExamGradeAppealFormID, modelObj.ExamGradeAppealFormForStudentObj.ExamID, modelObj.ExamGradeAppealFormForStudentObj.StudentID, modelObj.SubjectListAppeal[x].ProgramCourseID, modelObj.SubjectListAppeal[x].TeacherID, modelObj.SubjectListAppeal[x].PaperRecheking, DateTime.Now, Convert.ToInt32(Session["CurrentUserID"]), "Yes", DateTime.Now, Convert.ToInt32(Session["CurrentUserID"]));
                }
                if (Session["ProgramCourseList"] != null)
                {
                    ProgramCourseList =(string)Session["ProgramCourseList"] ;
                }
                modelObj.SubjectListAppeal = db.GetExamGradeAppealStudentCourses(modelObj.ExamGradeAppealFormForStudentObj.ExamID, modelObj.ExamGradeAppealFormForStudentObj.StudentID, ProgramCourseList).ToList();

                ViewBag.MessageType = "success";
                ViewBag.Message = "Data has been saved successfully.";
            }

            //List<GetUserDetail_Result> TeacherList = new List<GetUserDetail_Result>();
            //TeacherList = db.GetUserDetail(2, "31,59").ToList();
            //GetUserDetail_Result Obj = new GetUserDetail_Result();
            //Obj.EmpID = 0;
            //Obj.UserDetail = "--ALL--";
            //TeacherList.Insert(0, Obj);
            //modelObj.TeacherListSelected = new SelectList(TeacherList, "EmpID", "UserDetail");
            //return RedirectToAction("ExamGradeAppealRequestForm", modelObj);
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
        //[HttpPost]
        //public ActionResult ExamGradeAppealRequestForm(ExamStudentSubjectForGradeAppealRequestViewModel modelObj)
        //{
        //    Session["CurrentUserID"] = "9005";
        //    modelObj.ExamGradeAppealFormForStudentObj = db.GetExamGradeAppealFormForStudent(modelObj.ExamID, Convert.ToInt32(Session["CurrentUserID"])).LastOrDefault();
        //    return View(modelObj);
        //}
    }
}