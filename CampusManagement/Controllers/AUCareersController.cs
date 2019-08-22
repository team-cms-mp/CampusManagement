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
    public class AUCareersController : Controller
    {
        private ModelCMSNewContainer db = new ModelCMSNewContainer();
        AlumniUserCareerViewModel model = new AlumniUserCareerViewModel();

        public ActionResult Index()
        {
            int CurrentUserID = Convert.ToInt32(Session["CurrentUserID"]);
            model.AlumniUserCareers = db.AlumniUserCareers.OrderByDescending(a => a.AlumniUserCareerID).ToList();
            model.SelectedAlumniUserCareers = null;
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
            model.AlumniUserCareers = db.AlumniUserCareers.OrderByDescending(a => a.AlumniUserCareerID).ToList();
            model.SelectedAlumniUserCareers = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.AlumniUserID = CurrentUserID;
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc");
            ViewBag.MessageType = "";
            ViewBag.Message = "";
            return View("Index", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(AlumniUserCareer AUR)
        {
            try
            {
                AlumniUserCareer d = db.AlumniUserCareers.FirstOrDefault(de => de.CareerTitle == AUR.CareerTitle);

                if (d == null)
                {
                    
                    //AUR.CreatedBy = Convert.ToInt32(Session["emp_id"]);
                    AUR.CreatedBy = Convert.ToInt32(Session["CurrentUserID"]);
                    AUR.CreatedOn = DateTime.Now;
                    db.AlumniUserCareers.Add(AUR);
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
            }
            ViewBag.AlumniUserID = AUR.AlumniUserID;
            model.AlumniUserCareers = db.AlumniUserCareers.OrderByDescending(a => a.AlumniUserID).ToList();
            model.SelectedAlumniUserCareers = null;
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

            AlumniUserCareer AUR = db.AlumniUserCareers.Find(id);
            if (AUR == null)
            {
                return HttpNotFound();
            }

            model.AlumniUserCareers = db.AlumniUserCareers.OrderByDescending(a => a.AlumniUserID).ToList();
            model.SelectedAlumniUserCareers = AUR;
            model.DisplayMode = "ReadWrite";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc", AUR.IsActive);
            ViewBag.AlumniUserID = AUR.AlumniUserID;
            ViewBag.MessageType = "";
            ViewBag.Message = "";
            return View("Index", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(AlumniUserCareer AUR)
        {
            try
            {
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
            model.AlumniUserCareers = db.AlumniUserCareers.OrderByDescending(a => a.AlumniUserID).ToList();
            model.SelectedAlumniUserCareers = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc", AUR.IsActive);

            return View("Index", model);
        }


        public ActionResult GetAlumniUserCareer(GetAlumniUserCareer_Result alumi)
        {
            GetAlumniUserCareer_ResultViewModel model = new GetAlumniUserCareer_ResultViewModel();
            //model.GetAlumniUserCareer_Results = db.GetAlumniUserCareer().ToList();
            model.SelectedGetAlumniUserCareer_Results = null;
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