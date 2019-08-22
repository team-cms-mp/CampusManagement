using CampusManagement.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace CampusManagement.Controllers
{
    public class AUResumeController : Controller
    {
        int count = 0;
        string ErrorMessage = "";
        private ModelCMSNewContainer db = new ModelCMSNewContainer();
        AlumniUserResumeViewModel model = new AlumniUserResumeViewModel();

        public ActionResult Index()
        {
            int CurrentUserID = Convert.ToInt32(Session["CurrentUserID"]);
            model.AlumniUserResumes = db.AlumniUserResumes.OrderByDescending(a => a.AlumniUserResumeID).ToList();
            model.SelectedAlumniUserResumes = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc");
            ViewBag.MessageType = "";
            ViewBag.AlumniUserID = CurrentUserID;
            ViewBag.Message = "";
            return View(model);
        }

        [HttpGet]
        public ActionResult Create()
        {
            int CurrentUserID = Convert.ToInt32(Session["CurrentUserID"]);
            model.AlumniUserResumes = db.AlumniUserResumes.OrderByDescending(a => a.AlumniUserResumeID).ToList();
            model.SelectedAlumniUserResumes = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.AlumniUserID = CurrentUserID;
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc");
            ViewBag.MessageType = "";
            ViewBag.Message = "";
            return View("Index", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(AlumniUserResume AUR)
        {
            try
            {
                AUR.ResumeDocumentPath = string.Concat("~/DegreeDocument/", DateTime.Now.Ticks, "_", AUR.UploadFiles.FileName.Replace(" ", ""));
                AlumniUserResume d = db.AlumniUserResumes.FirstOrDefault(de => de.ResumeDocumentPath == AUR.ResumeDocumentPath);

                if (d == null)
                {
                    try
                    {
                       
                        AUR.UploadFiles.SaveAs(Server.MapPath(AUR.ResumeDocumentPath));
                    }
                    catch (Exception)
                    {
                        ModelState.AddModelError(string.Empty, "-Please attach the document.");
                        count++;
                        ErrorMessage += count + "-Please attach the document.<br />";
                    }

                    // AUR.CreatedBy = Convert.ToInt32(Session["emp_id"]);
                    AUR.CreatedBy = Convert.ToInt32(Session["CurrentUserID"]);
                    AUR.CreatedOn = DateTime.Now;
                    db.AlumniUserResumes.Add(AUR);
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
                    ModelState.AddModelError(string.Empty, "Name already exists.");
                    ViewBag.MessageType = "error";
                    ViewBag.Message = "Name already exists.";
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
                ViewBag.AlumniUserID = AUR.AlumniUserID;
            }
            model.AlumniUserResumes = db.AlumniUserResumes.OrderByDescending(a => a.AlumniUserID).ToList();
            model.SelectedAlumniUserResumes = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc", AUR.IsActive);

            return View("Index", model);
        }

        public ActionResult Update(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            AlumniUserResume AUR = db.AlumniUserResumes.Find(id);
            if (AUR == null)
            {
                return HttpNotFound();
            }

            model.AlumniUserResumes = db.AlumniUserResumes.OrderByDescending(a => a.AlumniUserID).ToList();
            model.SelectedAlumniUserResumes = AUR;
            model.DisplayMode = "ReadWrite";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc", AUR.IsActive);
            ViewBag.AlumniUserID = AUR.AlumniUserID;
            ViewBag.MessageType = "";
            ViewBag.Message = "";
            return View("Index", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(AlumniUserResume AUR)
        {
            try
            {
                try
                {
                    if (AUR.UploadFiles != null)
                    {
                        AUR.ResumeDocumentPath = string.Concat("~/DegreeDocument/", DateTime.Now.Ticks, "_", AUR.UploadFiles.FileName.Replace(" ", ""));
                        AUR.UploadFiles.SaveAs(Server.MapPath(AUR.ResumeDocumentPath));
                    }
                }
                catch (Exception)
                {
                    ModelState.AddModelError(string.Empty, "-Please attach the document.");
                    count++;
                    ErrorMessage += count + "-Please attach the document.<br />";
                }
                db.Entry(AUR).State = EntityState.Modified;
                //AUR.ModifiedBy = Convert.ToInt32(Session["emp_id"]);
                AUR.ModifiedBy = Convert.ToInt32(Session["CurrentUserID"]);
                AUR.ModifiedOn = DateTime.Now;
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
            model.AlumniUserResumes = db.AlumniUserResumes.OrderByDescending(a => a.AlumniUserID).ToList();
            model.SelectedAlumniUserResumes = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc", AUR.IsActive);

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