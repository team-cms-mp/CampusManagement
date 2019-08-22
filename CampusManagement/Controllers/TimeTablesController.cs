using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using CampusManagement.Models;
using Newtonsoft.Json;

namespace CampusManagement.Controllers
{
    [Authorize]
    public class TimeTablesController : Controller
    {
        private ModelCMSNewContainer db = new ModelCMSNewContainer();

        public ActionResult TimeTableTabs()
        {
            return View();
        }

        public ActionResult TimeTableDataTemp()
        {
            return View(db.TimeTableDataTempTables.ToList().OrderBy(t => t.TimeSlotID));
        }



        // GET: TimeTables
        public ActionResult RoomWiseTimeTable()
        {
            return View(db.TimeTables.ToList().OrderBy(t => t.RoomName));
        }

        public ActionResult SubjectWiseTimeTable()
        {
            var data = db.TimeTables.ToList().OrderBy(t => t.CourseName);
            return View(data);
        }

        public ActionResult SemesterWiseTimeTable()
        {
            return View(db.TimeTables.ToList().OrderBy(t => t.SemesterName));
        }

        public ActionResult BatchWiseTimeTable()
        {
            return View(db.TimeTables.ToList().OrderBy(t => t.BatchName));
        }

        public ActionResult TeacherWiseTimeTable()
        {
            return View(db.TimeTables.ToList().OrderBy(t => t.TeacherName));
        }

        public ActionResult SlotWiseTimeTable()
        {
            return View(db.TimeTables.ToList().OrderBy(t => t.TimeSlot));
        }

        public ActionResult TimeTableSetup()
        {
            return View();
        }
        public ActionResult TimeTableSetuptest()
        {
           

            return View(db.TimeTableDataTempTables.ToList().OrderBy(t => t.TimeSlotID));
      
        }


        public ActionResult TimeTableView()
        {
            return View();
        }
        // GEt TimeTableMainID
        public JsonResult GetTimeTableMain()
        {

            List<TimeTableMain> lstTimetables = new List<TimeTableMain>();

            lstTimetables = db.TimeTableMains.ToList();
            var timetable = lstTimetables.Select(p => new
            {
                TimeTableMainID = p.TimeTableMainID,
                TimeTableMainName = p.TimeTableMainName
            });
            string result = JsonConvert.SerializeObject(timetable, Formatting.Indented);
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
