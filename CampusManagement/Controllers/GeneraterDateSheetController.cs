using CampusManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CampusManagement.Controllers
{
    public class GeneraterDateSheetController : Controller
    {
        private ModelCMSNewContainer db = new ModelCMSNewContainer();
        ExamDateSheetViewModel model = new ExamDateSheetViewModel();
        ExamDateSheetDeatilCreateViewModel modelcreate = new ExamDateSheetDeatilCreateViewModel();
        ExamDateSheetRoomDispatchViewModel modelRoomDispatch = new ExamDateSheetRoomDispatchViewModel();


        //  List<Exam> modelExam = new List<Exam>();
        Exam ExamObj = new Exam();
        // GET: GeneraterDateSheet
        public ActionResult Index()
        {
            int ExamID = 2004;
          
            List<GetExamDateSheetCoursesByIDs_Result> ExamCourseList = new List<GetExamDateSheetCoursesByIDs_Result>();
           // model.ExamCourseList = db.GetExamDateSheetCoursesByIDs(122,12).ToList();
            ExamObj = db.Exams.Where(x => x.ExamID == ExamID).FirstOrDefault();
            model.ExamID = ExamID;
            model.ExamTitle = ExamObj.ExamTitle;
            model.ExamDateList  = db.ExamDates.Where(x => x.ExamID == ExamID).ToList();
            model.ExamDateTimeSlotList = db.ExamDateTimeSlots.Where(x => x.ExamID == ExamID).ToList(); 
            return View(model);
        }

        public ActionResult IndexDateSheet(int ExamID)
        {
          
            List<GetExamDateSheetCoursesByIDs_Result> ExamCourseList = new List<GetExamDateSheetCoursesByIDs_Result>();
            //model.ExamCourseList = db.GetExamDateSheetCoursesByIDs(122, 12).ToList();
            ExamObj = db.Exams.Where(x => x.ExamID == ExamID).FirstOrDefault();
            model.ExamID = ExamID;
            model.ExamTitle = ExamObj.ExamTitle;
            ViewBag.IsDateSheetApproved = ExamObj.IsDateSheetApproved;
            model.ExamDateList = db.ExamDates.Where(x => x.ExamID == ExamID).ToList();
            model.ExamDateTimeSlotList = db.ExamDateTimeSlots.Where(x => x.ExamID == ExamID).ToList();
            model.RoomCount = db.Rooms.Where(z => z.RoomTypeID == 1).Count();
            model.SubjectCount = db.ExamsDateSheetDetails.Where(y => y.ExamID == ExamID).Count();
            return View(model);
        }
        public ActionResult IndexExamList()
        {
           // modelExam = db.Exams.OrderByDescending(a => a.ExamID).ToList();
            return View();
        }

        public ActionResult ApproveDateSheet(int ExamID)
        { 
            int EmpID = Convert.ToInt32(Session["emp_id"]);
            db.UpdateDateSheetApproveByExamID(ExamID,1, EmpID);
            // Update Request For Approve Here
            return RedirectToAction("IndexExamList");
        }

     
        

        public ActionResult CreateDateSheet(int ExamID) {
           // int ExamID = 2004;
            ViewBag.hdnExamID = ExamID;
            modelcreate.ExamID = ExamID;
            ExamObj = db.Exams.Where(x => x.ExamID == ExamID).FirstOrDefault();
            ViewBag.IsDateSheetApproved = ExamObj.IsDateSheetApproved;
            modelcreate.DisplayMode = "WriteOnly";
            ViewBag.ExamID = new SelectList(db.Exams, "ExamID", "ExamTitle");
            ViewBag.ExamDateTimeSlotID = new SelectList(db.ExamDateTimeSlots.Where(z => z.ExamID == ExamID) , "ExamDateTimeSlotID", "TimeSlot");
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc");
            ViewBag.MessageType = "";
            ViewBag.Message = "";
            
            // int ExamID = 2004;
            return View(modelcreate);
        }

        public ActionResult CreateDateSheetInsert(ExamDateSheetDeatilCreateViewModel Obj)
        {
            int EmpID = Convert.ToInt32(Session["emp_id"]);
            List<GetExamDatesheetSemister_Result> SemisterList = new List<GetExamDatesheetSemister_Result>();
           // int ExamID = 2004;
            int Sem1 = 0;
            int Sem2 = 0;
            int Sem3 = 0;
            int Sem4 = 0;
            int Sem5 = 0;
            int Sem6 = 0;
            int Sem7 = 0;
            int Sem8 = 0;


            if (Obj.Semister1 == false)
            {
                Sem1 = 0;
            }
            else {
                Sem1 = 1;
            }
            if (Obj.Semister2 == false)
            {
                Sem2 = 0;
            }
            else
            {
                Sem2 = 2;
            }
            if (Obj.Semister3 == false)
            {
                Sem3 = 0;
            }
            else
            {
                Sem3 = 3;
            }
            if (Obj.Semister4 == false)
            {
                Sem4 = 0;
            }
            else
            {
                Sem4 = 4;
            }
            if (Obj.Semister5 == false)
            {
                Sem5 = 0;
            }
            else
            {
                Sem5 = 5;
            }
            if (Obj.Semister6 == false)
            {
                Sem6 = 0;
            }
            else
            {
                Sem6 = 6;
            }
            if (Obj.Semister7 == false)
            {
                Sem7 = 0;
            }
            else
            {
                Sem7 = 7;
            }
            if (Obj.Semister8 == false)
            {
                Sem8 = 0;
            }
            else
            {
                Sem8 = 8;
            }

            db.InsertExamDatesheetDetail(Obj.ExamID, Sem1, Sem2, Sem3, Sem4, Sem5, Sem6, Sem7, Sem8, Obj.ExamDateTimeSlotID,DateTime.Now, EmpID);

           return RedirectToAction("CreateDateSheet", "GeneraterDateSheet", new { ExamID = Obj.ExamID});
           // return View();
        }

        [HttpPost]
        public JsonResult UpdateDateSheetInsert(string ExamsDateSheetDetailID, string ExamDateID, string ExamDateTimeSlotID , string ProgramCourseID)
        {
            int EmpID = Convert.ToInt32(Session["emp_id"]);
            db.UpdateExamDatesheetDetail(Convert.ToInt32(ExamsDateSheetDetailID), Convert.ToInt32(ExamDateID), Convert.ToInt32(ExamDateTimeSlotID), Convert.ToInt32(ProgramCourseID), DateTime.Now, EmpID);
            return Json(new { success = true, responseText = "Your message successfuly sent!" }, JsonRequestBehavior.AllowGet);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }


        //public ActionResult DateSheetRoomDispatch(int ExamID)
        //{

        //    modelRoomDispatch.ExamDateList = db.ExamDates.Where(x => x.ExamID == ExamID).ToList();
        //    modelRoomDispatch.ExamDateTimeSlotList = db.ExamDateTimeSlots.Where(x => x.ExamID == ExamID).ToList();
        //    modelRoomDispatch.RoomList = db.Rooms.Where(z => z.RoomTypeID == 1).OrderBy(c => c.RoomCapacity).ToList();
        //    foreach (ExamDate ExamDateObj in modelRoomDispatch.ExamDateList) {
        //        foreach (ExamDateTimeSlot ExamDateTimeSlotObj in modelRoomDispatch.ExamDateTimeSlotList)
        //        {
        //           // modelRoomDispatch.RoomList
        //            modelRoomDispatch.ExamCourseList = db.GetExamDateSheetCoursesByIDs(Convert.ToInt32(ExamDateObj.ExamDateID), Convert.ToInt32(ExamDateTimeSlotObj.ExamDateTimeSlotID)).ToList();
        //        }

        //    }


        //    return View();
        //}

    }
}