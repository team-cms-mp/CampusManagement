using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CampusManagement.Models;

namespace CampusManagement.Controllers
{
    [Authorize]
    public class EmployeesAdmissionRightAssignController : Controller
    {
        private ModelCMSNewContainer db = new ModelCMSNewContainer();
        EmployeePagesForAdmissionRightViewModel model = new EmployeePagesForAdmissionRightViewModel();

        public ActionResult Index()
        {
            //int EmpID = Convert.ToInt32(Session["emp_id"]);
            //return View(db.um_GetEmployees("").Where(e => e.EmpID != EmpID));
            return View(db.um_GetEmployeesForAdmission(""));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult Index(string paramSearch)
        {
            //ViewBag.paramSearch = paramSearch;
            //int EmpID = Convert.ToInt32(Session["emp_id"]);
            //return View(db.um_GetEmployees(paramSearch).Where(e => e.EmpID != EmpID));

            ViewBag.paramSearch = paramSearch;
            return View(db.um_GetEmployeesForAdmission(paramSearch));
        }

        public ActionResult AssignPages(int EmpID, string EmployeeName)
        {
            ViewBag.EmpID = EmpID;
            ViewBag.EmployeeName = EmployeeName;
            model.EmployesPagesList = db.GetSubAdminEmployeePagesForRight(EmpID).ToList();
            return View(model);
        }

        [HttpPost]
        public JsonResult EmployeeAssignRight(string Emp_ID, string Module_ID, string Page_ID,string id, Boolean IsChecked)
        {
            int ID = Convert.ToInt32(db.InsertOrUpdateEmployeePagesRight(Convert.ToInt32(Emp_ID), Convert.ToInt32(Module_ID), Convert.ToInt32(Page_ID), Convert.ToInt32(id), 0, 0, IsChecked).FirstOrDefault());
            return Json(new { success = true, responseText = ID }, JsonRequestBehavior.AllowGet);
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

