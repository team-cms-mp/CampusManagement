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
    public class ExamRoomDispatchController : Controller
    {
        private ModelCMSNewContainer db = new ModelCMSNewContainer();
        ExamEligibleStudentForRoomAssignmentViewModel model = new ExamEligibleStudentForRoomAssignmentViewModel();
        // GET: ExamRoomDispatchTest

        public ActionResult Index()
        {
            ViewBag.ExamID = new SelectList(db.Exams, "ExamID", "ExamTitle");
            ViewBag.ExamDateID = new SelectList(db.GetExamDateByExamID(0), "ExamDateID", "ExamDateTitle");
            ViewBag.ExamDateTimeSlotID = new SelectList(db.GetExamDateTimeSlotByExamID(0), "ExamDateTimeSlotID", "TimeSlot");

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
            ViewBag.ExamDateID = new SelectList(db.GetExamDateByExamID(RA.ExamID), "ExamDateID", "ExamDateTitle");
            ViewBag.ExamDateTimeSlotID = new SelectList(db.GetExamDateTimeSlotByExamID(RA.ExamID), "ExamDateTimeSlotID", "TimeSlot");

            // ViewBag.ExamDateTimeSlotID = new SelectList(db.Semesters, "YearSemesterNo", "YearSemesterNo");
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc");
            return View(RA);
        }
        //public ActionResult RoomDispatch( FormCollection fc)
        //{
        //    return View();
        //}


        //public ActionResult RoomDispatch(int? ExamsDateSheetDetailID , int? StudentCount, FormCollection fc)
        //{
        //    return View();
        //}
        public JsonResult RoomDispatch(string ExamsDateSheetDetailID, string StudentCount, string RSelected)
        {
            string[] SelectedRoomsWithCapacity = RSelected.Split(',');
            List<RoomsWithCapacity> SelectedRoomList = new List<RoomsWithCapacity>();
            RoomsWithCapacity RoomsWithCapacityObj = null;
            int RowIndexFrom = 1;
            int RowIndexTo = 0;
            int LoopToRunCount = SelectedRoomsWithCapacity.Length - 1;
            for (int i = 0; i <= LoopToRunCount; i++) {
                RoomsWithCapacityObj = new RoomsWithCapacity();
                string[] SelectRoomDetailArray = SelectedRoomsWithCapacity[i].Split('_');
                RoomsWithCapacityObj.RoomID = Convert.ToInt32(SelectRoomDetailArray[0].ToString());
                RoomsWithCapacityObj.RoomCapacity = Convert.ToInt32(SelectRoomDetailArray[1].ToString());
                SelectedRoomList.Add(RoomsWithCapacityObj);
            }

            // Top To Calculate from SelectedRoomList Than Apply Query up to n.... Top Remaining from Student Count Below
            int RemainingStudentCount = Convert.ToInt32(StudentCount);
            int EachTopToExecute = 0;

            if (SelectedRoomList.Count == 1)
            {
                EachTopToExecute = RemainingStudentCount;
                RowIndexTo = EachTopToExecute;
                db.UpdateRoomDispatch(RowIndexFrom, RowIndexTo, Convert.ToInt32(ExamsDateSheetDetailID), SelectedRoomList[0].RoomID);
                // Update Query Here With ExamsDateSheetDetailID ToDo***************************
            }
            else {
                for (int x = 0; x < SelectedRoomList.Count; x++)
                {
                    if (RemainingStudentCount >= SelectedRoomList[x].RoomCapacity)
                    {
                        RemainingStudentCount = RemainingStudentCount - SelectedRoomList[x].RoomCapacity;
                        if (x == 0)
                        {
                            RowIndexTo = SelectedRoomList[x].RoomCapacity;
                            db.UpdateRoomDispatch(RowIndexFrom, RowIndexTo, Convert.ToInt32(ExamsDateSheetDetailID), SelectedRoomList[x].RoomID);
                            RowIndexFrom = RowIndexTo + 1;
                        }
                        else {
                            RowIndexTo = RowIndexTo + EachTopToExecute;
                            db.UpdateRoomDispatch(RowIndexFrom, RowIndexTo, Convert.ToInt32(ExamsDateSheetDetailID), SelectedRoomList[x].RoomID);
                        }
                        
                        EachTopToExecute = SelectedRoomList[x].RoomCapacity;

                        // Update Query Here With ExamsDateSheetDetailID ToDo***************************v
                    }
                    else {
                        EachTopToExecute = RemainingStudentCount;
                        RowIndexFrom = RowIndexTo + 1;
                        RowIndexTo = RowIndexTo + EachTopToExecute;
                        db.UpdateRoomDispatch(RowIndexFrom, RowIndexTo, Convert.ToInt32(ExamsDateSheetDetailID), SelectedRoomList[x].RoomID);
                        // Update Query Here With ExamsDateSheetDetailID ToDo***************************
                    }
                }
            }


            string result = JsonConvert.SerializeObject(RSelected, Formatting.Indented);
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

        public partial class RoomsWithCapacity
        {        
            public int RoomCapacity { get; set; }
            public int RoomID { get; set; }
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