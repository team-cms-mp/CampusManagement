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
    public class ExamRosterDispatchController : Controller
    {
        private ModelCMSNewContainer db = new ModelCMSNewContainer();
        ExamEligibleStudentForRoomAssignmentViewModel model = new ExamEligibleStudentForRoomAssignmentViewModel();
        ExamSubjectForRosterDispatchViewModel modelSubjectRooms = new ExamSubjectForRosterDispatchViewModel();
        RosterDispatchListViewModel modelrosterdispatch = new RosterDispatchListViewModel();
        // GET: ExamRoomDispatchTest

        public ActionResult Index()
        {
            ViewBag.ExamID = new SelectList(db.Exams, "ExamID", "ExamTitle");
            ViewBag.ExamDateID = new SelectList(db.GetExamDateByExamID(0), "ExamDateID", "ExamDateTitle");
            ViewBag.ExamDateTimeSlotID = new SelectList(db.GetExamDateTimeSlotByExamID(0), "ExamDateTimeSlotID", "TimeSlot");
            //ViewBag.ExamDateTimeSlotID = new SelectList(db.Semesters, "YearSemesterNo", "YearSemesterNo");

            ViewBag.MessageType = "";
            ViewBag.Message = "";
            return View(model);
        }

        [HttpPost]
        public ActionResult Index(ExamEligibleStudentForRoomAssignmentViewModel RA)
        {
            string ErrorMessage = "";
            int count = 0;
            ViewBag.hdnExamDateID = RA.ExamDateID;
            ViewBag.hdnExamDateTimeSlotID = RA.ExamDateTimeSlotID;
            ViewBag.hdnExamID = RA.ExamID;

            if (RA.ExamDateID == 0)
            {
                count++;
                ErrorMessage += count + "-Please select Exam Date.<br/>";
            }
            if (RA.ExamDateTimeSlotID == 0)
            {
                count++;
                ErrorMessage += count + "-Please select Exam Time Slot.<br/>";
            }

            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ViewBag.MessageType = "error";
                ViewBag.Message = ErrorMessage;
            }
            else
            {

                ViewBag.MessageType = "";
                ViewBag.Message = "";
            }


            ViewBag.ExamID = new SelectList(db.Exams, "ExamID", "ExamTitle");
            ViewBag.ExamDateID = new SelectList(db.GetExamDateByExamID(RA.ExamDateID), "ExamDateID", "ExamDateTitle");
            ViewBag.ExamDateTimeSlotID = new SelectList(db.GetExamDateTimeSlotByExamID(RA.ExamDateTimeSlotID), "ExamDateTimeSlotID", "TimeSlot");
            //ViewBag.ExamDateTimeSlotID = new SelectList(db.Semesters, "YearSemesterNo", "YearSemesterNo");
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc");
            return View(RA);
        }
        public ActionResult RosterDispatch(int? ExamsDateSheetDetailID)
        {
            ViewBag.ExamsDateSheetDetailID = ExamsDateSheetDetailID;
            ViewBag.MessageType = "";
            modelSubjectRooms.LecturarList =  db.GetChatUser(2, "31,59").OrderBy(x => x.UserName).ToList();
            return View(modelSubjectRooms);
        }

        public JsonResult TeacherDispatch(string ExamsDateSheetDetailID, string RoomID, string TSelected)
        {
            string[] SelectedTeachersArray = TSelected.Split(',');
            int EmpID = Convert.ToInt32(Session["emp_id"]);
            int LoopToRunCount = SelectedTeachersArray.Length - 1;
            for (int i = 0; i <= LoopToRunCount; i++)
            {
                int DutyRosterID = Convert.ToInt32(SelectedTeachersArray[i]);
                db.InsertExamRosterDetail(DutyRosterID, Convert.ToInt32(ExamsDateSheetDetailID), Convert.ToInt32(RoomID), EmpID);
               // string[] SelectTArray = SelectedTeachersArray[i].Split('_');
             //   int DutyRosterID = Convert.ToInt32(SelectTArray[0].ToString());
                
            }

            //// Top To Calculate from SelectedRoomList Than Apply Query up to n.... Top Remaining from Student Count Below
            //int RemainingStudentCount = Convert.ToInt32(StudentCount);
            //int EachTopToExecute = 0;

            //if (SelectedRoomList.Count == 1)
            //{
            //    EachTopToExecute = RemainingStudentCount;
            //    RowIndexTo = EachTopToExecute;
            //    db.UpdateRoomDispatch(RowIndexFrom, RowIndexTo, Convert.ToInt32(ExamsDateSheetDetailID), SelectedRoomList[0].RoomID);
            //    // Update Query Here With ExamsDateSheetDetailID ToDo***************************
            //}
            //else
            //{
            //    for (int x = 0; x < SelectedRoomList.Count; x++)
            //    {
            //        if (RemainingStudentCount >= SelectedRoomList[x].RoomCapacity)
            //        {
            //            RemainingStudentCount = RemainingStudentCount - SelectedRoomList[x].RoomCapacity;
            //            if (x == 0)
            //            {
            //                RowIndexTo = SelectedRoomList[x].RoomCapacity;
            //                db.UpdateRoomDispatch(RowIndexFrom, RowIndexTo, Convert.ToInt32(ExamsDateSheetDetailID), SelectedRoomList[x].RoomID);
            //                RowIndexFrom = RowIndexTo + 1;
            //            }
            //            else
            //            {
            //                RowIndexTo = RowIndexTo + EachTopToExecute;
            //                db.UpdateRoomDispatch(RowIndexFrom, RowIndexTo, Convert.ToInt32(ExamsDateSheetDetailID), SelectedRoomList[x].RoomID);
            //            }

            //            EachTopToExecute = SelectedRoomList[x].RoomCapacity;

            //            // Update Query Here With ExamsDateSheetDetailID ToDo***************************v
            //        }
            //        else
            //        {
            //            EachTopToExecute = RemainingStudentCount;
            //            RowIndexFrom = RowIndexTo + 1;
            //            RowIndexTo = RowIndexTo + EachTopToExecute;
            //            db.UpdateRoomDispatch(RowIndexFrom, RowIndexTo, Convert.ToInt32(ExamsDateSheetDetailID), SelectedRoomList[x].RoomID);
            //            // Update Query Here With ExamsDateSheetDetailID ToDo***************************
            //        }
            //    }
            //}


            string result = JsonConvert.SerializeObject(TSelected, Formatting.Indented);
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        public ActionResult RosterDispatchList(int? ExamsDateSheetDetailID,int? RoomID)
        {

            modelrosterdispatch.RosterDispatchList = db.GetExamRosterDispatchList(ExamsDateSheetDetailID, RoomID).ToList();
            return View(modelrosterdispatch);
        }
        public JsonResult RemoveRosterDispatch(string ExamRosterDetailID)
        {
            try {

                db.DeleteExamDutyRosterByID(Convert.ToInt32(ExamRosterDetailID));

            } catch (Exception) {
                // Not Remove Row so make ExamRosterDetailID = ""
                ExamRosterDetailID = "";
            }
           
            string result = JsonConvert.SerializeObject(ExamRosterDetailID, Formatting.Indented);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetExamDatesByExamID(string ExamID)
        {
            List<GetExamDateByExamID_Result> lstExamDates = new List<GetExamDateByExamID_Result>();

            lstExamDates = db.GetExamDateByExamID(Convert.ToInt32(ExamID)).ToList();
            var Exams = lstExamDates.Select(p => new
            {
                ExamDateID = p.ExamDateID,
                ExamDateTitle = p.ExamDateTitle
            });
            string result = JsonConvert.SerializeObject(Exams, Formatting.Indented);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetExamDateTimeSlotByExamID(string ExamID)
        {
            List<GetExamDateTimeSlotByExamID_Result> lstTimeSlots = new List<GetExamDateTimeSlotByExamID_Result>();
            int ExamIDInt = Convert.ToInt32(ExamID);

            lstTimeSlots = db.GetExamDateTimeSlotByExamID(ExamIDInt).ToList();
            var semesters = lstTimeSlots.Select(S => new
            {
                ExamDateTimeSlotID = S.ExamDateTimeSlotID,
                TimeSlot = S.TimeSlot
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