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

namespace CampusManagement.Controllers
{
    [Authorize(Roles = "Account Officer,Accounts Officer,Admin Assistant,Admin Officer,Admin.Assistant,Assist. Account Officer,Assist.Technician,Import Manager,Manager Servive & Support,Office Manager,Officer QMS,RSM - Center 2,RSM - South,Sales & Service Executive,Sales Executive,Sales Manager,Sales Representative,Sr.Accounts Officer,Sr.Associate Engineer,Sr.Sales Executive,Sr.Sales Representative,Store Assistant,Store Incharge,Technician")]
    public class TimeTablesController : Controller
    {
        private ModelCMSContainer db = new ModelCMSContainer();

        public ActionResult TimeTableTabs()
        {
            return View();
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


        public ActionResult TimeTableView()
        {
            return View();
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
