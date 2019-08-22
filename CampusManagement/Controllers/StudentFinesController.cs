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
using System.Data.Entity.Core;

namespace CampusManagement.Controllers
{
    [Authorize]
    public class StudentFinesController : Controller
    {
        private ModelCMSNewContainer db = new ModelCMSNewContainer();
        SF_GetStudentFines_ResultViewModel model = new SF_GetStudentFines_ResultViewModel();

        public ActionResult Index(int? StudentID)
        {
            model.SF_GetStudentFines_Results = db.SF_GetStudentFines(StudentID, 0).ToList();
            model.SelectedSF_GetStudentFines_Result = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.StudentFineTypeID = new SelectList(db.SF_StudentFineType, "StudentFineTypeID", "StudentFineTypeName");
            ViewBag.MessageType = "";
            ViewBag.Message = "";
            ViewBag.StudentID = StudentID;
            return View(model);
        }

        [HttpGet]
        public ActionResult Create(int? StudentID)
        {
            model.SF_GetStudentFines_Results = db.SF_GetStudentFines(StudentID, 0).ToList();
            model.SelectedSF_GetStudentFines_Result = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.StudentFineTypeID = new SelectList(db.SF_StudentFineType, "StudentFineTypeID", "StudentFineTypeName");
            ViewBag.MessageType = "";
            ViewBag.Message = "";
            ViewBag.StudentID = StudentID;
            return View("Index", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(SF_StudentFine fine)
        {
            string ErrorMessage = "";
            int count = 0;
            try
            {
                SF_StudentFine fin = db.SF_StudentFine.FirstOrDefault(d => d.StudentID == fine.StudentID && d.YearSemesterNo == fine.YearSemesterNo && d.StudentFineTypeID == fine.StudentFineTypeID);
                if (fin != null)
                {
                    count++;
                    ErrorMessage += count + "-" + "Fine already exists." + "<br />";
                }

                if (fine.StudentFineTypeID == null)
                {
                    count++;
                    ErrorMessage += count + "-" + "Please select Fine Type." + "<br />";
                }

                if (!string.IsNullOrEmpty(ErrorMessage))
                {
                    ViewBag.MessageType = "error";
                    ViewBag.Message = ErrorMessage;
                }
                else
                {
                    fine.CreatedBy = Convert.ToInt32(Session["emp_id"]);
                    fine.CreatedOn = DateTime.Now;
                    fine.StatusID = 1007;
                    fine.IsActive = "Yes";
                    db.SF_StudentFine.Add(fine);
                    try
                    {
                        db.SaveChanges();
                        ViewBag.MessageType = "success";
                        ViewBag.Message = "Data has been saved successfully.";
                    }
                    catch (DbUpdateException ex)
                    {
                        ViewBag.MessageType = "error";
                        ViewBag.Message = ex.Message;
                        ModelState.AddModelError(string.Empty, ex.Message);
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                foreach (DbEntityValidationResult validationResult in ex.EntityValidationErrors)
                {
                    string entityName = validationResult.Entry.Entity.GetType().Name;
                    foreach (DbValidationError error in validationResult.ValidationErrors)
                    {
                        ModelState.AddModelError(string.Empty, error.ErrorMessage);
                        count++;
                        ErrorMessage += count + "-" + string.Concat(error.PropertyName, " is required.") + "<br />";
                    }
                }
                ViewBag.MessageType = "error";
                ViewBag.Message = ErrorMessage;
            }
            model.SF_GetStudentFines_Results = db.SF_GetStudentFines(fine.StudentID, 0).ToList();
            model.SelectedSF_GetStudentFines_Result = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.StudentFineTypeID = new SelectList(db.SF_StudentFineType, "StudentFineTypeID", "StudentFineTypeName");
            ViewBag.MessageType = "";
            ViewBag.Message = "";
            ViewBag.StudentID = fine.StudentID;

            return View("Index", model);
        }

        public ActionResult ApproveUnapprove(int? id, string type, int? StudentID)
        {
            try
            {
                Student stu = db.Students.FirstOrDefault(s => s.StudentID == StudentID);
                int EmpID = Convert.ToInt32(Session["emp_id"]);
                if (type == "Approved")
                {
                    db.SF_Insert_StudentService(0, stu.FormNo, StudentID, stu.BatchProgramID, stu.CurrentSemesterNo, EmpID, 11, 0, id);
                }
                else if (type == "Unapproved")
                {
                    db.SF_Insert_StudentService(0, "", StudentID, 0, 0, EmpID, 13, 0, id);
                }
            }
            catch (EntityCommandExecutionException ex)
            {
                ViewBag.MessageType = "error";
                ViewBag.Message = string.Concat(ex.Message, ", Inner Exception: " + ex.InnerException);
                ModelState.AddModelError(string.Empty, ex.Message);
            }

            return RedirectToAction("Index", "StudentFines", new { StudentID = StudentID });
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
