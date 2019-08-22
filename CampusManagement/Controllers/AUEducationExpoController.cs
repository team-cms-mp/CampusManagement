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
    public class AUEducationExpoController : Controller
    {
        private ModelCMSNewContainer db = new ModelCMSNewContainer();
        AlumniEducationExpoViewModel model = new AlumniEducationExpoViewModel();

        public ActionResult Index()
        {
            int CurrentUserID = Convert.ToInt32(Session["CurrentUserID"]);
            model.AlumniEducationExpos = db.AlumniEducationExpoes.OrderByDescending(a => a.AlumniEducationExpoID).ToList();
            model.SelectedAlumniEducationExpos = null;
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
            model.AlumniEducationExpos = db.AlumniEducationExpoes.OrderByDescending(a => a.AlumniEducationExpoID).ToList();
            model.SelectedAlumniEducationExpos = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.AlumniUserID = CurrentUserID;
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc");
            ViewBag.MessageType = "";
            ViewBag.Message = "";
            return View("Index", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(AlumniEducationExpo AUR, string Start_Date, string End_Date, string Start_Time, string End_Time)
        {
            try
            {
                AlumniEducationExpo d = db.AlumniEducationExpoes.FirstOrDefault(de => de.EducationExpoTitle == AUR.EducationExpoTitle);

                if (d == null)
                {
                    var combinedStartDateTime = (Start_Date + " " + Start_Time);
                    var combinedEndDateTime = (End_Date + " " + End_Time);
                    AUR.EducationExpoStartDateTime = Convert.ToDateTime(combinedStartDateTime);
                    AUR.EducationExpoEndDateTime = Convert.ToDateTime(combinedEndDateTime);
                    //AUR.CreatedBy = Convert.ToInt32(Session["emp_id"]);
                    AUR.CreatedBy = Convert.ToInt32(Session["CurrentUserID"]);
                    AUR.CreatedOn = DateTime.Now;
                    db.AlumniEducationExpoes.Add(AUR);
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
            model.AlumniEducationExpos = db.AlumniEducationExpoes.OrderByDescending(a => a.AlumniUserID).ToList();
            model.SelectedAlumniEducationExpos = null;
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

            AlumniEducationExpo AUR = db.AlumniEducationExpoes.Find(id);
            if (AUR == null)
            {
                return HttpNotFound();
            }

            model.AlumniEducationExpos = db.AlumniEducationExpoes.OrderByDescending(a => a.AlumniUserID).ToList();
            string strDate = AUR.EducationExpoStartDateTime.Value.ToShortDateString();
            ViewBag.strDate = strDate;
           // string mtest = AUR.EducationExpoEndDateTime.Value.ToString("HH:mm");
            string endDate = AUR.EducationExpoEndDateTime.Value.ToShortDateString();
            ViewBag.endDate = endDate;
            string strTime = AUR.EducationExpoStartDateTime.Value.ToString("HH:mm");
            ViewBag.strTime = strTime;
            string endTime = AUR.EducationExpoEndDateTime.Value.ToString("HH:mm");
            ViewBag.endTime = endTime.Replace(" PM", "").Replace(" AM", ""); ;
            model.SelectedAlumniEducationExpos = AUR;

            //model.AlumniEducationExpos = db.AlumniEducationExpoes.OrderByDescending(a => a.AlumniUserID).ToList();
            //string strDate = AUR.EducationExpoStartDateTime.Value.ToShortDateString();
            //ViewBag.strDate = strDate;
            //string endDate = AUR.EducationExpoEndDateTime.Value.ToShortDateString();
            //ViewBag.endDate = endDate;
            //string strTime = AUR.EducationExpoStartDateTime.Value.ToShortTimeString();
            //ViewBag.strTime = strTime.Replace(" PM", "").Replace(" AM", "");
            //string endTime = AUR.EducationExpoEndDateTime.Value.ToShortTimeString();
            //ViewBag.endTime = endTime.Replace(" PM", "").Replace(" AM", ""); ;
            //model.SelectedAlumniEducationExpos = AUR;

            model.DisplayMode = "ReadWrite";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc", AUR.IsActive);
            ViewBag.AlumniUserID = AUR.AlumniUserID;
            ViewBag.MessageType = "";
            ViewBag.Message = "";
            return View("Index", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(AlumniEducationExpo AUR, string Start_Date, string End_Date, string Start_Time, string End_Time)
        {
            try
            {
                var combinedStartDateTime = (Start_Date + " " + Start_Time);
                var combinedEndDateTime = (End_Date + " " + End_Time);
                AUR.EducationExpoStartDateTime = Convert.ToDateTime(combinedStartDateTime);
                AUR.EducationExpoEndDateTime = Convert.ToDateTime(combinedEndDateTime);
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
            model.AlumniEducationExpos = db.AlumniEducationExpoes.OrderByDescending(a => a.AlumniUserID).ToList();
            model.SelectedAlumniEducationExpos = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc", AUR.IsActive);

            return View("Index", model);
        }


        public ActionResult GetAlumniEducationExpo(GetAlumniEducationExpo_Result alumi)
        {
            GetAlumniEducationExpo_ResultViewModel model = new GetAlumniEducationExpo_ResultViewModel();
            //model.GetAlumniEducationExpo_Results = db.GetAlumniEducationExpo.OrderByDescending(a => a.AlumniEducationExpoID).ToList();
            model.GetAlumniEducationExpo_Results = db.GetAlumniEducationExpo().ToList();
            model.SelectedGetAlumniEducationExpo_Results = null;
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