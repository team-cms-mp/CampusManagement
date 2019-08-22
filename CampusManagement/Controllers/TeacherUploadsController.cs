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
    public class TeacherUploadsController : Controller
    {
        int count = 0;
        string ErrorMessage = "";
        private ModelCMSNewContainer db = new ModelCMSNewContainer();
        TeacherUploadViewModel model = new TeacherUploadViewModel();

        public ActionResult Index()
        {
            int EmpID = Convert.ToInt32(Session["emp_id"]);
            model.TeacherUploads = db.TeacherUploads.Where(t => t.EmpID == EmpID).OrderByDescending(a=>a.TeacherUploadID).ToList();
            model.SelectedTeacherUpload = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc");
            ViewBag.MessageType = "";
            ViewBag.Message = "";
            return View(model);
        }
        
        [HttpGet]
        public ActionResult Create()
        {
            int EmpID = Convert.ToInt32(Session["emp_id"]);
            model.TeacherUploads = db.TeacherUploads.Where(t => t.EmpID == EmpID).OrderByDescending(a => a.TeacherUploadID).ToList();
            model.SelectedTeacherUpload = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc");
            ViewBag.MessageType = "";
            ViewBag.Message = "";
            return View("Index", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(TeacherUpload teacherUpload)
        {
            try
            {
                teacherUpload.CreatedBy = Convert.ToInt32(Session["emp_id"]);
                teacherUpload.EmpID = Convert.ToInt32(Session["emp_id"]);
                TeacherUpload d = db.TeacherUploads.FirstOrDefault(de => de.TeacherUploadName == teacherUpload.TeacherUploadName && de.CreatedBy == teacherUpload.CreatedBy);
                if (d == null)
                {
                    try
                    {
                        teacherUpload.UploadPath = "~/DegreeDocument/" + string.Concat(DateTime.Now.Ticks, "_", teacherUpload.TeacherUploadFile.FileName.Replace(" ", ""));
                        teacherUpload.TeacherUploadFile.SaveAs(Server.MapPath("~/DegreeDocument") + "/" + string.Concat(DateTime.Now.Ticks, "_", teacherUpload.TeacherUploadFile.FileName.Replace(" ", "")));
                    }
                    catch (Exception)
                    {
                        ModelState.AddModelError(string.Empty, "-Please attach the document.");
                        count++;
                        ErrorMessage += count + "-Please attach the document.<br />";
                    }

                    teacherUpload.CreatedOn = DateTime.Now;
                    db.TeacherUploads.Add(teacherUpload);
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
                else
                {
                    ModelState.AddModelError(string.Empty, "Assignment already exists.");
                    ViewBag.MessageType = "error";
                    ViewBag.Message = "Assignment already exists.";
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
            int EmpID = Convert.ToInt32(Session["emp_id"]);
            model.TeacherUploads = db.TeacherUploads.Where(t => t.EmpID == EmpID).OrderByDescending(a => a.TeacherUploadID).ToList();
            model.SelectedTeacherUpload = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc", teacherUpload.IsActive);

            return View("Index", model);
        }

        public ActionResult Update(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            TeacherUpload teacherUpload = db.TeacherUploads.Find(id);
            if (teacherUpload == null)
            {
                return HttpNotFound();
            }

            int EmpID = Convert.ToInt32(Session["emp_id"]);
            model.TeacherUploads = db.TeacherUploads.Where(t => t.EmpID == EmpID).OrderByDescending(a => a.TeacherUploadID).ToList();
            model.SelectedTeacherUpload = teacherUpload;
            model.DisplayMode = "ReadWrite";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc", teacherUpload.IsActive);
            ViewBag.MessageType = "";
            ViewBag.Message = "";
            return View("Index", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(TeacherUpload teacherUpload)
        {
            try
            {
                db.Entry(teacherUpload).State = EntityState.Modified;
                teacherUpload.ModifiedBy = Convert.ToInt32(Session["emp_id"]);
                teacherUpload.ModifiedOn = DateTime.Now;
                try
                {
                    teacherUpload.UploadPath = "~/DegreeDocument/" + string.Concat(DateTime.Now.Ticks, "_", teacherUpload.TeacherUploadFile.FileName.Replace(" ", ""));
                    teacherUpload.TeacherUploadFile.SaveAs(Server.MapPath("~/DegreeDocument") + "/" + string.Concat(DateTime.Now.Ticks, "_", teacherUpload.TeacherUploadFile.FileName.Replace(" ", "")));
                }
                catch (Exception)
                {
                    ModelState.AddModelError(string.Empty, "-Please attach the document.");
                    count++;
                    ErrorMessage += count + "-Please attach the document.<br />";
                }

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
            int EmpID = Convert.ToInt32(Session["emp_id"]);
            model.TeacherUploads = db.TeacherUploads.Where(t => t.EmpID == EmpID).OrderByDescending(a => a.TeacherUploadID).ToList();
            model.SelectedTeacherUpload = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc", teacherUpload.IsActive);

            return View("Index", model);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TeacherUpload teacherUpload = db.TeacherUploads.Find(id);
            if (teacherUpload == null)
            {
                return HttpNotFound();
            }

            int EmpID = Convert.ToInt32(Session["emp_id"]);
            model.TeacherUploads = db.TeacherUploads.Where(t => t.EmpID == EmpID).OrderByDescending(a => a.TeacherUploadID).ToList();
            model.SelectedTeacherUpload = teacherUpload;
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
                TeacherUpload teacherUpload = db.TeacherUploads.Find(id);
                db.TeacherUploads.Remove(teacherUpload);
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
            int EmpID = Convert.ToInt32(Session["emp_id"]);
            model.TeacherUploads = db.TeacherUploads.Where(t => t.EmpID == EmpID).OrderByDescending(a => a.TeacherUploadID).ToList();
            model.SelectedTeacherUpload = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc");

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
