using CampusManagement.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace CampusManagement.Controllers
{
    public class AUReunionsDinnerController : Controller
    {
        private ModelCMSNewContainer db = new ModelCMSNewContainer();
        AlumniReunionsDinnerViewModel model = new AlumniReunionsDinnerViewModel();

        public ActionResult Index()
        {
            int CurrentUserID = Convert.ToInt32(Session["CurrentUserID"]);
            model.AlumniReunionsDinners = db.AlumniReunionsDinners.OrderByDescending(a => a.AlumniReunionsDinnerID).ToList();
            model.SelectedAlumniReunionsDinners = null;
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
            model.AlumniReunionsDinners = db.AlumniReunionsDinners.OrderByDescending(a => a.AlumniReunionsDinnerID).ToList();
            model.SelectedAlumniReunionsDinners = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.AlumniUserID = CurrentUserID;
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc");
            ViewBag.MessageType = "";
            ViewBag.Message = "";
            return View("Index", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(AlumniReunionsDinner AUR, string Start_Date, string End_Date, string Start_Time, string End_Time)
        {
            try
            {
                AlumniReunionsDinner d = db.AlumniReunionsDinners.FirstOrDefault(de => de.ReunionsDinnerTitle == AUR.ReunionsDinnerTitle);

                if (d == null)
                {
                    var combinedStartDateTime = (Start_Date + " " + Start_Time);
                    var combinedEndDateTime = (End_Date + " " + End_Time);
                    AUR.ReunionsDinnerStartDateTime = Convert.ToDateTime(combinedStartDateTime);
                    AUR.ReunionsDinnerEndDateTime = Convert.ToDateTime(combinedEndDateTime);
                    
                    //AUR.CreatedBy = Convert.ToInt32(Session["emp_id"]);
                    AUR.CreatedBy = Convert.ToInt32(Session["CurrentUserID"]);
                    AUR.CreatedOn = DateTime.Now;
                    db.AlumniReunionsDinners.Add(AUR);
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
            model.AlumniReunionsDinners = db.AlumniReunionsDinners.OrderByDescending(a => a.AlumniUserID).ToList();
            model.SelectedAlumniReunionsDinners = null;
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

            AlumniReunionsDinner AUR = db.AlumniReunionsDinners.Find(id);
            if (AUR == null)
            {
                return HttpNotFound();
            }

            model.AlumniReunionsDinners = db.AlumniReunionsDinners.OrderByDescending(a => a.AlumniUserID).ToList();
            string strDate = AUR.ReunionsDinnerStartDateTime.Value.ToShortDateString();
            ViewBag.strDate = strDate;
           // string mtest = AUR.EducationExpoEndDateTime.Value.ToString("HH:mm");
            string endDate = AUR.ReunionsDinnerEndDateTime.Value.ToShortDateString();
            ViewBag.endDate = endDate;
            string strTime = AUR.ReunionsDinnerStartDateTime.Value.ToString("HH:mm");
            ViewBag.strTime = strTime;
            string endTime = AUR.ReunionsDinnerEndDateTime.Value.ToString("HH:mm");
            ViewBag.endTime = endTime.Replace(" PM", "").Replace(" AM", ""); ;
            model.SelectedAlumniReunionsDinners = AUR;


            model.DisplayMode = "ReadWrite";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc", AUR.IsActive);
            ViewBag.AlumniUserID = AUR.AlumniUserID;
            ViewBag.MessageType = "";
            ViewBag.Message = "";
            return View("Index", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(AlumniReunionsDinner AUR, string Start_Date, string End_Date, string Start_Time, string End_Time)
        {
            try
            {
                var combinedStartDateTime = (Start_Date + " " + Start_Time);
                var combinedEndDateTime = (End_Date + " " + End_Time);
                AUR.ReunionsDinnerStartDateTime = Convert.ToDateTime(combinedStartDateTime);
                AUR.ReunionsDinnerEndDateTime = Convert.ToDateTime(combinedEndDateTime);
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
            model.AlumniReunionsDinners = db.AlumniReunionsDinners.OrderByDescending(a => a.AlumniUserID).ToList();
            model.SelectedAlumniReunionsDinners = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc", AUR.IsActive);

            return View("Index", model);
        }


        public ActionResult GetAlumniReunionsDinner(GetAlumniReunionsDinner_Result alumi)
        {
            GetAlumniReunionsDinner_ResultViewModel model = new GetAlumniReunionsDinner_ResultViewModel();
            //model.GetAlumniEducationExpo_Results = db.GetAlumniEducationExpo.OrderByDescending(a => a.AlumniEducationExpoID).ToList();
            model.GetAlumniReunionsDinner_Results = db.GetAlumniReunionsDinner().ToList();
            model.SelectedGetAlumniReunionsDinner_Results = null;
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