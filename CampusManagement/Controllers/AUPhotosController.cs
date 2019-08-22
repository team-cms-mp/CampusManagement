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
    public class AUPhotosController : Controller
    {
        int count = 0;
        string ErrorMessage = "";
        private ModelCMSNewContainer db = new ModelCMSNewContainer();
        AlumniUserPhotoViewModel model = new AlumniUserPhotoViewModel();

        public ActionResult Index()
        {
            int CurrentUserID = Convert.ToInt32(Session["CurrentUserID"]);
            model.AlumniUserPhotos = db.AlumniUserPhotoes.OrderByDescending(a => a.AlumniUserPhotoID).ToList();
            model.SelectedAlumniUserPhotos = null;
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
            model.AlumniUserPhotos = db.AlumniUserPhotoes.OrderByDescending(a => a.AlumniUserPhotoID).ToList();
            model.SelectedAlumniUserPhotos = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.AlumniUserID = CurrentUserID;
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc");
            ViewBag.MessageType = "";
            ViewBag.Message = "";
            return View("Index", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(AlumniUserPhoto AUR)
        {
            try
            {
                //AlumniUserPhoto d = db.AlumniUserPhotoes.FirstOrDefault(de => de.AlumniUserID == AUR.AlumniUserID);
                AUR.PhotoPath = string.Concat("~/DegreeDocument/", DateTime.Now.Ticks, "_", AUR.UploadFiles.FileName.Replace(" ", ""));
                AlumniUserPhoto d = db.AlumniUserPhotoes.FirstOrDefault(de => de.PhotoPath == AUR.PhotoPath);

                if (d == null)
                {
                    try
                    {
                        //AUR.PhotoPath = string.Concat("~/DegreeDocument/", DateTime.Now.Ticks, "_", AUR.UploadFiles.FileName.Replace(" ", ""));
                        AUR.UploadFiles.SaveAs(Server.MapPath(AUR.PhotoPath));
                    }
                    catch (Exception)
                    {
                        ModelState.AddModelError(string.Empty, "-Please attach the document.");
                        count++;
                        ErrorMessage += count + "-Please attach the document.<br />";
                    }

                    int CurrentUserID = Convert.ToInt32(Session["CurrentUserID"]);
                    AUR.CreatedBy = CurrentUserID;
                    //AUR.CreatedBy = Convert.ToInt32(Session["emp_id"]);
                    AUR.CreatedOn = DateTime.Now;
                    db.AlumniUserPhotoes.Add(AUR);
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
            model.AlumniUserPhotos = db.AlumniUserPhotoes.OrderByDescending(a => a.AlumniUserID).ToList();
            model.SelectedAlumniUserPhotos = null;
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

            AlumniUserPhoto AUR = db.AlumniUserPhotoes.Find(id);
            if (AUR == null)
            {
                return HttpNotFound();
            }

            model.AlumniUserPhotos = db.AlumniUserPhotoes.OrderByDescending(a => a.AlumniUserID).ToList();
            model.SelectedAlumniUserPhotos = AUR;
            model.DisplayMode = "ReadWrite";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc", AUR.IsActive);
            ViewBag.AlumniUserID = AUR.AlumniUserID;
            ViewBag.MessageType = "";
            ViewBag.Message = "";
            return View("Index", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(AlumniUserPhoto AUR)
        {
            try
            {
                try
                {
                    if (AUR.UploadFiles != null)
                    {
                        AUR.PhotoPath = string.Concat("~/DegreeDocument/", DateTime.Now.Ticks, "_", AUR.UploadFiles.FileName.Replace(" ", ""));
                        AUR.UploadFiles.SaveAs(Server.MapPath(AUR.PhotoPath));
                    }
                }
                catch (Exception)
                {
                    ModelState.AddModelError(string.Empty, "-Please attach the document.");
                    count++;
                    ErrorMessage += count + "-Please attach the document.<br />";
                }
                db.Entry(AUR).State = EntityState.Modified;
                // AUR.ModifiedBy = Convert.ToInt32(Session["emp_id"]);
                int CurrentUserID = Convert.ToInt32(Session["CurrentUserID"]);
                AUR.ModifiedBy = CurrentUserID;
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
            model.AlumniUserPhotos = db.AlumniUserPhotoes.OrderByDescending(a => a.AlumniUserID).ToList();
            model.SelectedAlumniUserPhotos = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc", AUR.IsActive);

            return View("Index", model);
        }


        public ActionResult GetAlumniUserPhoto_Result()
        {
            GetAlumniUserPhoto_ResultViewModel model = new GetAlumniUserPhoto_ResultViewModel();
            model.GetAlumniUserPhoto_Results = db.GetAlumniUserPhoto().ToList();
            model.SelectedGetAlumniUserPhoto_Results = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.MessageType = "";
            ViewBag.Message = "";
            return View(model);
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