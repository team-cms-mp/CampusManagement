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
    public class ExamRosterDetailsTeacherForAdminController : Controller
    {
        private ModelCMSNewContainer db = new ModelCMSNewContainer();
        ExamRosterDetailsTeacherViewModel model = new ExamRosterDetailsTeacherViewModel();
        // GET: ExamRosterDetailsTeacherForAdmin
        public ActionResult Index()
        {
            Exam ExamObj = new Exam();
            ExamObj = db.Exams.Where(x => x.IsDateSheetApproved == 1 && x.IsActive == "Yes").FirstOrDefault();
            if (ExamObj != null)
            {
                ViewBag.ExamID = new SelectList(db.Exams, "ExamID", "ExamTitle", ExamObj.ExamID);
                ViewBag.hdnExamID = ExamObj.ExamID;
            }
            else
            {
                ExamObj = db.Exams.FirstOrDefault();
                List<Exam> ExamList = new List<Exam>();
                ExamList = db.Exams.ToList();
                if (ExamObj != null && ExamList != null)
                {
                    ViewBag.hdnExamID = ExamObj.ExamID;
                    ViewBag.ExamID = new SelectList(db.Exams, "ExamID", "ExamTitle", db.Exams.FirstOrDefault().ExamID);
                }
                else
                {
                    ExamObj = new Exam();
                    ExamObj.ExamTitle = "Please Add Exam First";
                    ExamObj.ExamID = 0;
                    ExamList.Add(ExamObj);
                    ViewBag.hdnExamID = 0;
                    ViewBag.ExamID = new SelectList(ExamList, "ExamID", "ExamTitle");
                }
            }



            List<GetUserDetail_Result> TeacherList = new List<GetUserDetail_Result>();
            TeacherList = db.GetUserDetail(2, "31,59").ToList();
            GetUserDetail_Result Obj = new GetUserDetail_Result();
            Obj.EmpID = 0;
            Obj.UserDetail = "--ALL--";
            TeacherList.Insert(0, Obj);
            ViewBag.EmpID = new SelectList(TeacherList, "EmpID", "UserDetail");
            return View(model);
        }


        [HttpPost]
        public ActionResult Index(ExamRosterDetailsTeacherViewModel ERD)
        {
            ViewBag.hdnExamID = ERD.ExamID;
            ViewBag.hdnEmpID = ERD.EmpID;
            List<GetUserDetail_Result> TeacherList = new List<GetUserDetail_Result>();
            TeacherList = db.GetUserDetail(2, "31,59").ToList();
            GetUserDetail_Result Obj = new GetUserDetail_Result();
            Obj.EmpID = 0;
            Obj.UserDetail = "--ALL--";
            TeacherList.Insert(0, Obj);
            ViewBag.ExamID = new SelectList(db.Exams, "ExamID", "ExamTitle", ERD.ExamID);
            ViewBag.EmpID = new SelectList(TeacherList, "EmpID", "UserDetail",ERD.EmpID);
            return View(ERD);
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