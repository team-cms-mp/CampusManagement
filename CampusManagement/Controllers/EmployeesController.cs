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
    public class EmployeesController : Controller
    {
        private ModelCMSNewContainer db = new ModelCMSNewContainer();

        public ActionResult Index()
        {
            int EmpID = Convert.ToInt32(Session["emp_id"]);
            return View(db.um_GetEmployees("").Where(e => e.EmpID != EmpID));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(string paramSearch)
        {
            ViewBag.paramSearch = paramSearch;
            int EmpID = Convert.ToInt32(Session["emp_id"]);
            return View(db.um_GetEmployees(paramSearch).Where(e => e.EmpID != EmpID));
        }

        public ActionResult GetIncompleteApplicantDetail()
        {
            return View(db.um_rpt_GetIncompleteApplicantDetail(""));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetIncompleteApplicantDetail(string paramSearch)
        {
            ViewBag.paramSearch = paramSearch;
            return View(db.um_rpt_GetIncompleteApplicantDetail(paramSearch));
        }

        public ActionResult um_DeleteUser(string UserName)
        {
            db.um_DeleteUser(UserName);
            TempData["messageType"] = "success";
            TempData["message"] = "Record has been removed successfully.";
            return RedirectToAction("GetIncompleteApplicantDetail");
        }


        public ActionResult AssignPages(int EmpID, string RightsType)
        {
            db.um_AssignPages_Employees(EmpID, RightsType);
            return RedirectToAction("Index");
        }

        public ActionResult um_RevertEmployeeRights(int EmpID)
        {
            db.um_RevertEmployeeRights(EmpID);
            return RedirectToAction("Index");
        }

        public ActionResult UsersDetail()
        {
            List<Um_GetUserDetailForAdmin_Result> ListUser = db.Um_GetUserDetailForAdmin("", "", "").ToList();

            return View(ListUser);
        }

        [HttpPost]
        public ActionResult UsersDetail(string paramSearch)
        {
            List<Um_GetUserDetailForAdmin_Result> ListUser = db.Um_GetUserDetailForAdmin(paramSearch, "", "").ToList();

            return View(ListUser);
        }

        public ActionResult UsersChangePassword(string UserName)
        {
            ViewBag.UserName = UserName;

            return View();
        }

        [HttpPost]
        public ActionResult UsersChangePassword(string UserName, string OldPassword, string NewPassword, string CnfrmPassword)
        {
            Login login = db.Logins.FirstOrDefault(a => a.Password == OldPassword && a.UserName == UserName);

            if (login != null)
            {
                if (NewPassword.Trim() != CnfrmPassword.Trim())
                {
                    ViewBag.MessageType = "error";
                    ViewBag.ErrorMessage = "New Password and Confirm Password must be same";
                }
                else
                {
                    db.um_UpdatePassword(UserName, OldPassword, NewPassword);
                    ViewBag.MessageType = "success";
                    ViewBag.ErrorMessage = "Password changed successfully.";
                }
            }
            else
            {
                ViewBag.MessageType = "error";
                ViewBag.ErrorMessage = "User Name or Old Password is incorrect.";
            }

            ViewBag.UserName = UserName;

            return View();
        }

        public ActionResult AdminChangePassword()
        {
            //ViewBag.UserName = UserName;

            return View();
        }

        [HttpPost]
        public ActionResult AdminChangePassword(string UserName, string OldPassword, string NewPassword, string CnfrmPassword)
        {
            Login login = db.Logins.FirstOrDefault(a => a.Password == OldPassword && a.UserName == UserName);

            if (login != null)
            {
                if (NewPassword.Trim() != CnfrmPassword.Trim())
                {
                    ViewBag.MessageType = "error";
                    ViewBag.ErrorMessage = "New Password and Confirm Password must be same";
                }
                else
                {
                    db.um_UpdatePassword(UserName, OldPassword, NewPassword);
                    ViewBag.MessageType = "success";
                    ViewBag.ErrorMessage = "Password changed successfully.";
                }
            }
            else
            {
                ViewBag.MessageType = "error";
                ViewBag.ErrorMessage = "User Name or Old Password is incorrect.";
            }

            ViewBag.UserName = UserName;

            return View();
        }


        public ActionResult AdminResetPassword(string UserName, string NewPassword)
        {
            Login login = db.Logins.FirstOrDefault(a => a.UserName == UserName);

            if (login != null)
            {
                db.um_ResetPassword(UserName, "123");
                ViewBag.MessageType = "success";
                ViewBag.ErrorMessage = "Password changed successfully.";
            }
            else
            {
                ViewBag.MessageType = "error";
                ViewBag.ErrorMessage = "User Name is incorrect.";
            }

            ViewBag.UserName = UserName;

            return RedirectToAction("AdminChangePassword");
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

