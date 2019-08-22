using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CampusManagement.Models;
using Newtonsoft.Json;

namespace CampusManagement.Controllers
{
    [Authorize]
    public class StudentProgramEnrollmentsController : Controller
    {
        private ModelCMSNewContainer db = new ModelCMSNewContainer();
        StudentProgramEnrollmentsViewModel model = new StudentProgramEnrollmentsViewModel();

        public ActionResult Index()
        {
            model.StudentProgramEnrollments = db.StudentProgramEnrollments.OrderByDescending(e => e.EnrollmentID).ToList();
            model.SelectedStudentProgramEnrollment = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc");
            ViewBag.BatchProgramID = new SelectList(db.GetBatchProgramNameConcat("", 0), "ID", "Name");
            ViewBag.DegreeCompleted = new SelectList(db.Options, "OptionDesc", "OptionDesc");
            ViewBag.MessageType = "";
            ViewBag.Message = "";
            return View(model);
        }

        [HttpGet]
        public ActionResult Create()
        {
            model.StudentProgramEnrollments = db.StudentProgramEnrollments.OrderByDescending(e => e.EnrollmentID).ToList();
            model.SelectedStudentProgramEnrollment = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc");
            ViewBag.BatchProgramID = new SelectList(db.GetBatchProgramNameConcat("", 0), "ID", "Name");
            ViewBag.DegreeCompleted = new SelectList(db.Options, "OptionDesc", "OptionDesc");
            ViewBag.MessageType = "";
            ViewBag.Message = "";
            return View("Index", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(StudentProgramEnrollment studentProgramEnrollment)
        {
            StudentProgramEnrollment en = db.StudentProgramEnrollments.FirstOrDefault(e => e.StudentID == studentProgramEnrollment.StudentID);
            if (en == null)
            {
                studentProgramEnrollment.CreatedBy = Convert.ToInt32(Session["emp_id"]);
                try
                {
                    if (studentProgramEnrollment.EnrollmentNo != null && studentProgramEnrollment.EnrollmentNo != "")
                    {
                        db.Insert_StudentEnrollment(studentProgramEnrollment.StudentID
                            , studentProgramEnrollment.BatchProgramID
                            , studentProgramEnrollment.AdmissionDate
                            , studentProgramEnrollment.EnrollmentNo
                            , studentProgramEnrollment.RegistrationNo
                            , studentProgramEnrollment.DegreeCompleted
                            , studentProgramEnrollment.DegreeCompletionDate
                            , studentProgramEnrollment.CreatedBy
                            , studentProgramEnrollment.IsActive);

                        ViewBag.MessageType = "success";
                        ViewBag.Message = "Data has been saved successfully.";
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Enrollment already exists.");
                        ViewBag.MessageType = "error";
                        ViewBag.Message = "Enrollment No is required.";
                    }
                }
                catch (SqlException ex)
                {
                    ViewBag.MessageType = "error";
                    ViewBag.Message = ex.Message;
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Enrollment already exists.");
                ViewBag.MessageType = "error";
                ViewBag.Message = "Enrollment already exists.";
            }

            model.StudentProgramEnrollments = db.StudentProgramEnrollments.OrderByDescending(e => e.EnrollmentID).ToList();
            model.SelectedStudentProgramEnrollment = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc", studentProgramEnrollment.IsActive);
            ViewBag.BatchProgramID = new SelectList(db.GetBatchProgramNameConcat("", 0), "ID", "Name", studentProgramEnrollment.BatchProgramID);
            ViewBag.DegreeCompleted = new SelectList(db.Options, "OptionDesc", "OptionDesc");
            return View("Index", model);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StudentProgramEnrollment studentProgramEnrollment = db.StudentProgramEnrollments.Find(id);
            if (studentProgramEnrollment == null)
            {
                return HttpNotFound();
            }

            model.StudentProgramEnrollments = db.StudentProgramEnrollments.OrderByDescending(e => e.EnrollmentID).ToList();
            model.SelectedStudentProgramEnrollment = studentProgramEnrollment;
            model.DisplayMode = "Delete";
            ViewBag.MessageType = "";
            ViewBag.Message = "";
            return View("Index", model);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                StudentProgramEnrollment studentProgramEnrollment = db.StudentProgramEnrollments.Find(id);
                db.StudentProgramEnrollments.Remove(studentProgramEnrollment);
                db.SaveChanges();
                ViewBag.MessageType = "success";
                ViewBag.Message = "Record has been removed successfully.";
            }
            catch (DbUpdateException ex)
            {
                ViewBag.MessageType = "error";
                ViewBag.Message = ex.Message;
                ModelState.AddModelError(string.Empty, ex.Message);
            }
            model.StudentProgramEnrollments = db.StudentProgramEnrollments.OrderByDescending(e => e.EnrollmentID).ToList();
            model.SelectedStudentProgramEnrollment = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc");
            ViewBag.BatchProgramID = new SelectList(db.GetBatchProgramNameConcat("", 0), "ID", "Name");
            ViewBag.DegreeCompleted = new SelectList(db.Options, "OptionDesc", "OptionDesc");

            return View("Index", model);
        }

        public JsonResult GetStudentsList(string searchValue)
        {
            List<Student> lstStudent = new List<Student>();

            lstStudent = db.Students.Where(a => a.FirstName.Contains(searchValue)
            || a.LastName.Contains(searchValue)
            || a.FormNo.Contains(searchValue)
            || a.ACNIC.Contains(searchValue)).ToList();

            var Students = lstStudent.Select(s => new
            {
                AddAppID = s.StudentID,
                FirstName = s.FirstName,
                LastName = s.LastName,
                FormNo = s.FormNo,
                ACNIC = s.ACNIC
            });

            string result = JsonConvert.SerializeObject(Students, Formatting.Indented);
            return new JsonResult { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult GetBatchProgramsByFormNo(string FormNo)
        {
            List<GetBatchProgramNameConcat_Result> lstBP = new List<GetBatchProgramNameConcat_Result>();

            lstBP = db.GetBatchProgramNameConcat(FormNo, 6).ToList();
            if (lstBP.Count == 0)
            {
                ViewBag.MessageType = "error";
                ViewBag.Message = "No Programs found for the selected student.";
                return new JsonResult { Data = "No result found.", JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }

            var BatchPrograms = lstBP.Select(s => new
            {
                ID = s.ID,
                Name = s.Name
            });

            string result = JsonConvert.SerializeObject(BatchPrograms, Formatting.Indented);
            return new JsonResult { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
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
