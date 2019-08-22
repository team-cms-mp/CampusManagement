using CampusManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CampusManagement.Controllers
{
    public class ExamActiveDateSheetDisplayController : Controller
    {
        private ModelCMSNewContainer db = new ModelCMSNewContainer();
        ExamDateSheetViewModel model = new ExamDateSheetViewModel();
        // GET: ExamActiveDateSheetDisplay
        public ActionResult Index()
        {
            Exam ExamObj = new Exam();
            ExamObj = db.Exams.Where(x => x.IsDateSheetApproved == 1 && x.IsActive == "Yes").FirstOrDefault();
            List<GetExamDateSheetCoursesByIDs_Result> ExamCourseList = new List<GetExamDateSheetCoursesByIDs_Result>();
            if (ExamObj != null) {
                model.ExamID = ExamObj.ExamID;
                model.ExamTitle = ExamObj.ExamTitle;
                ViewBag.IsDateSheetApproved = ExamObj.IsDateSheetApproved;
                model.ExamDateList = db.ExamDates.Where(x => x.ExamID == ExamObj.ExamID).ToList();
                model.ExamDateTimeSlotList = db.ExamDateTimeSlots.Where(x => x.ExamID == ExamObj.ExamID).ToList();
                model.RoomCount = db.Rooms.Where(z => z.RoomTypeID == 1).Count();
                model.SubjectCount = db.ExamsDateSheetDetails.Where(y => y.ExamID == ExamObj.ExamID).Count();
            }
           
            return View(model);
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