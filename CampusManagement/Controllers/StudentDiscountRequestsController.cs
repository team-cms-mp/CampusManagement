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
    public class StudentDiscountRequestsController : Controller
    {
        private ModelCMSNewContainer db = new ModelCMSNewContainer();
        StudentDiscountRequestsViewModel model = new StudentDiscountRequestsViewModel();

        public ActionResult Index()
        {
            model.StudentDiscountRequests = db.GetStudentDiscountRequests("").OrderByDescending(a => a.StudentDiscountRequestID).ToList();
            model.SelectedStudentDiscountRequest = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc");
            ViewBag.StudentRequestTypeID = new SelectList(db.StudentRequestTypes, "StudentRequestTypeID", "RequestDescription");
            ViewBag.StudentRequestStatusID = new SelectList(db.StudentRequestStatus, "StudentRequestStatusID", "StudentRequestStatusName");
            ViewBag.MessageType = "";
            ViewBag.Message = "";
            return View(model);
        }

        [HttpGet]
        public ActionResult Create()
        {
            model.StudentDiscountRequests = db.GetStudentDiscountRequests("").OrderByDescending(a => a.StudentDiscountRequestID).ToList();
            model.SelectedStudentDiscountRequest = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc");
            ViewBag.StudentRequestTypeID = new SelectList(db.StudentRequestTypes, "StudentRequestTypeID", "RequestDescription");
            ViewBag.StudentRequestStatusID = new SelectList(db.StudentRequestStatus, "StudentRequestStatusID", "StudentRequestStatusName");
            ViewBag.MessageType = "";
            ViewBag.Message = "";
            return View("Index", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(StudentDiscountRequest studentDiscountRequest)
        {
            try
            {
                studentDiscountRequest.CreatedBy = Convert.ToInt32(Session["emp_id"]);
                studentDiscountRequest.CreatedOn = DateTime.Now;
                Student st = db.Students.FirstOrDefault(s => s.CreatedBy == studentDiscountRequest.CreatedBy);
                studentDiscountRequest.FormNo = st.FormNo;
                db.StudentDiscountRequests.Add(studentDiscountRequest);
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
                        ErrorMessage += count + "-" + string.Concat(error.PropertyName, " is required.") + "<br />";
                    }
                }
                ViewBag.MessageType = "error";
                ViewBag.Message = ErrorMessage;
            }
            model.StudentDiscountRequests = db.GetStudentDiscountRequests("").OrderByDescending(a => a.StudentDiscountRequestID).ToList();
            model.SelectedStudentDiscountRequest = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc");
            ViewBag.StudentRequestTypeID = new SelectList(db.StudentRequestTypes, "StudentRequestTypeID", "RequestDescription");
            ViewBag.StudentRequestStatusID = new SelectList(db.StudentRequestStatus, "StudentRequestStatusID", "StudentRequestStatusName");
            return View("Index", model);
        }

        public ActionResult Update(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            GetStudentDiscountRequests_Result studentDiscountRequest = db.GetStudentDiscountRequests("").FirstOrDefault( x=>x.StudentDiscountRequestID == id);
            if (studentDiscountRequest == null)
            {
                return HttpNotFound();
            }

            model.StudentDiscountRequests = db.GetStudentDiscountRequests("").OrderByDescending(a => a.StudentDiscountRequestID).ToList();
            model.SelectedStudentDiscountRequest = studentDiscountRequest;
            model.DisplayMode = "ReadWrite";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc");
            ViewBag.StudentRequestTypeID = new SelectList(db.StudentRequestTypes, "StudentRequestTypeID", "RequestDescription");
            ViewBag.StudentRequestStatusID = new SelectList(db.StudentRequestStatus, "StudentRequestStatusID", "StudentRequestStatusName");
            ViewBag.MessageType = "";
            ViewBag.Message = "";
            return View("Index", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(StudentDiscountRequest studentDiscountRequest)
        {
            try
            {
                db.Entry(studentDiscountRequest).State = EntityState.Modified;
                studentDiscountRequest.Modifiedby = Convert.ToInt32(Session["emp_id"]);
                Student st = db.Students.FirstOrDefault(s => s.CreatedBy == studentDiscountRequest.CreatedBy);
                studentDiscountRequest.FormNo = st.FormNo;
                studentDiscountRequest.ModifiedOn = DateTime.Now;
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
            model.StudentDiscountRequests = db.GetStudentDiscountRequests("").OrderByDescending(a => a.StudentDiscountRequestID).ToList();
            model.SelectedStudentDiscountRequest = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc");
            ViewBag.StudentRequestTypeID = new SelectList(db.StudentRequestTypes, "StudentRequestTypeID", "RequestDescription");
            ViewBag.StudentRequestStatusID = new SelectList(db.StudentRequestStatus, "StudentRequestStatusID", "StudentRequestStatusName");

            return View("Index", model);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GetStudentDiscountRequests_Result studentDiscountRequest = db.GetStudentDiscountRequests("").FirstOrDefault(x => x.StudentDiscountRequestID == id);
            if (studentDiscountRequest == null)
            {
                return HttpNotFound();
            }

            model.StudentDiscountRequests = db.GetStudentDiscountRequests("").OrderByDescending(a => a.StudentDiscountRequestID).ToList();
            model.SelectedStudentDiscountRequest = studentDiscountRequest;
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
                StudentDiscountRequest studentDiscountRequest = db.StudentDiscountRequests.Find(id);
                db.StudentDiscountRequests.Remove(studentDiscountRequest);
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
            model.StudentDiscountRequests = db.GetStudentDiscountRequests("").OrderByDescending(a => a.StudentDiscountRequestID).ToList();
            model.SelectedStudentDiscountRequest = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc");
            ViewBag.StudentRequestTypeID = new SelectList(db.StudentRequestTypes, "StudentRequestTypeID", "RequestDescription");
            ViewBag.StudentRequestStatusID = new SelectList(db.StudentRequestStatus, "StudentRequestStatusID", "StudentRequestStatusName");

            return View("Index", model);
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