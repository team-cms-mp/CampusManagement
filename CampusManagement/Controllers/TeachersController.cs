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
    public class TeachersController : Controller
    {
        private ModelCMSNewContainer db = new ModelCMSNewContainer();
        TeachersViewModel model = new TeachersViewModel();

        public ActionResult Index()
        {
            model.Teachers = db.Teachers.OrderByDescending(a => a.TeacherID).ToList();
            model.SelectedTeacher = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc");
            ViewBag.DesignationID = new SelectList(db.Designations, "DesignationID", "DesignationName");
            ViewBag.MessageType = "";
            ViewBag.Message = "";
            return View(model);
        }

        [HttpPost]
        public ActionResult Index(string Search)
        {
            var lst = db.Teachers.Where(x => x.CNIC.Contains(Search) || x.Email.Contains(Search) || x.TeacherName.Contains(Search)).OrderByDescending(a => a.TeacherID).ToList();
            model.Teachers = lst;
            model.SelectedTeacher = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc");
            ViewBag.DesignationID = new SelectList(db.Designations, "DesignationID", "DesignationName");
            ViewBag.MessageType = "";
            ViewBag.Message = "";
            return View(model);
        }

        public ActionResult LecturerProfile()
        {
            model.Teachers = db.Teachers.OrderByDescending(a => a.TeacherID).ToList();
            model.SelectedTeacher = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc");
            ViewBag.DesignationID = new SelectList(db.Designations, "DesignationID", "DesignationName");
            ViewBag.MessageType = "";
            ViewBag.Message = "";
            return View("LecturerProfile", model);
        }

        [HttpPost]
        public ActionResult LecturerProfile(string Search)
        {
            var lst = db.Teachers.Where(x => x.CNIC.Contains(Search) || x.Email.Contains(Search) || x.TeacherName.Contains(Search)).ToList();
            model.Teachers = lst;
            model.SelectedTeacher = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc");
            ViewBag.DesignationID = new SelectList(db.Designations, "DesignationID", "DesignationName");
            ViewBag.MessageType = "";
            ViewBag.Message = "";
            return View("LecturerProfile", model);
        }

        public ActionResult ViewLecturerProfile(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Teacher teacher = db.Teachers.Find(id);
            if (teacher == null)
            {
                return HttpNotFound();
            }

            model.Teachers = db.Teachers.OrderByDescending(a => a.TeacherID).ToList();
            model.SelectedTeacher = teacher;
            model.DisplayMode = "Delete";
            ViewBag.MessageType = "";
            ViewBag.Message = "";
            return View("ViewLecturerProfile", model);
        }

        [HttpGet]
        public ActionResult Create()
        {
            model.Teachers = db.Teachers.OrderByDescending(a => a.TeacherID).ToList();
            model.SelectedTeacher = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc");
            ViewBag.DesignationID = new SelectList(db.Designations, "DesignationID", "DesignationName");
            ViewBag.MessageType = "";
            ViewBag.Message = "";
            return View("Index", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Teacher teacher)
        {
            string ErrorMessage = "";
            int count = 0;
            try
            {
                Teacher t = new Teacher();
                t = db.Teachers.FirstOrDefault(te => te.CNIC == teacher.CNIC);
                if (t != null)
                {
                    ModelState.AddModelError(string.Empty, "CNIC already exists.");
                    count++;
                    ErrorMessage += count + "-CNIC already exists.<br />";
                }
                t = db.Teachers.FirstOrDefault(te => te.Email == teacher.Email);
                if (t != null)
                {
                    ModelState.AddModelError(string.Empty, "Email already exists.");
                    count++;
                    ErrorMessage += count + "-Email already exists.<br />";
                }
                t = db.Teachers.FirstOrDefault(te => te.EmployeeNo == teacher.EmployeeNo);
                if (t != null)
                {
                    ModelState.AddModelError(string.Empty, "Employee # already exists.");
                    count++;
                    ErrorMessage += count + "-Employee # already exists.<br />";
                }

                if(!string.IsNullOrEmpty(ErrorMessage))
                {
                    ViewBag.MessageType = "error";
                    ViewBag.Message = ErrorMessage;
                }

                if (t == null)
                {
                    teacher.CreatedBy = Convert.ToInt32(Session["emp_id"]);
                    teacher.CreatedOn = DateTime.Now;
                    db.Teachers.Add(teacher);
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
            model.Teachers = db.Teachers.OrderByDescending(a => a.TeacherID).ToList();
            model.SelectedTeacher = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc", teacher.IsActive);
            ViewBag.DesignationID = new SelectList(db.Designations, "DesignationID", "DesignationName", teacher.DesignationID);
            return View("Index", model);
        }

        public ActionResult Update(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Teacher teacher = db.Teachers.Find(id);
            if (teacher == null)
            {
                return HttpNotFound();
            }

            model.Teachers = db.Teachers.OrderByDescending(a => a.TeacherID).ToList();
            model.SelectedTeacher = teacher;
            model.DisplayMode = "ReadWrite";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc", teacher.IsActive);
            ViewBag.DesignationID = new SelectList(db.Designations, "DesignationID", "DesignationName", teacher.DesignationID);
            ViewBag.MessageType = "";
            ViewBag.Message = "";
            return View("Index", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Teacher teacher)
        {
            try
            {
                db.Entry(teacher).State = EntityState.Modified;
                teacher.ModifiedBy = Convert.ToInt32(Session["emp_id"]);
                teacher.ModifiedOn = DateTime.Now;
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
            catch (DbEntityValidationException ex)
            {
                string ErrorMessage = "";
                int count = 0;
                foreach (DbEntityValidationResult validationResult in ex.EntityValidationErrors)
                {
                    string entityName = validationResult.Entry.Entity.GetType().Name;
                    foreach (DbValidationError error in validationResult.ValidationErrors)
                    {
                        ModelState.AddModelError(string.Empty, error.ErrorMessage);
                        count++;
                        ErrorMessage += string.Concat(count, "-", error.ErrorMessage, "\n");
                    }
                }
                ViewBag.MessageType = "error";
                ViewBag.Message = ErrorMessage;
            }
            model.Teachers = db.Teachers.OrderByDescending(a => a.TeacherID).ToList();
            model.SelectedTeacher = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc", teacher.IsActive);
            ViewBag.DesignationID = new SelectList(db.Designations, "DesignationID", "DesignationName", teacher.DesignationID);

            return View("Index", model);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Teacher teacher = db.Teachers.Find(id);
            if (teacher == null)
            {
                return HttpNotFound();
            }

            model.Teachers = db.Teachers.OrderByDescending(a => a.TeacherID).ToList();
            model.SelectedTeacher = teacher;
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
                Teacher teacher = db.Teachers.Find(id);
                db.Teachers.Remove(teacher);
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
            model.Teachers = db.Teachers.OrderByDescending(a => a.TeacherID).ToList();
            model.SelectedTeacher = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc");
            ViewBag.DesignationID = new SelectList(db.Designations, "DesignationID", "DesignationName");

            return View("Index", model);
        }

        public ActionResult TeacherProfile()
        {
            return View();
        }

        public ActionResult TeacherTimeTable()
        {
            return View();
        }

        public ActionResult TeacherSubjects()
        {
            return View();
        }

        public ActionResult UploadAssignments()
        {
            return View();
        }

        public ActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ChangePassword(string OldPassword, string UserName, string CnfrmPassword, string NewPassword)
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
