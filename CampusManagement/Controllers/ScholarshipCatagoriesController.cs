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
    public class ScholarshipCatagoriesController : Controller
    {
        private ModelCMSNewContainer db = new ModelCMSNewContainer();
        ScholarshipCatagoryViewModel model = new ScholarshipCatagoryViewModel();

        public ActionResult Index()
        {

            model.ScholarshipCatagories = db.ScholarshipCatagories.ToList();
            model.SelectedScholarshipCatagory = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc");
            ViewBag.MessageType = "";
            ViewBag.Message = "";
            return View(model);
        }


        [HttpGet]
        public ActionResult Create()
        {
            model.ScholarshipCatagories = db.ScholarshipCatagories.ToList();
            model.SelectedScholarshipCatagory = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc");
           
            ViewBag.MessageType = "";
            ViewBag.Message = "";
            return View("Index", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ScholarshipCatagory ScholarshipCatagory)
        {
            try
            {
                ScholarshipCatagory d = db.ScholarshipCatagories.FirstOrDefault(de => de.ScholarshipCatagoryName == ScholarshipCatagory.ScholarshipCatagoryName);
                if (d == null)
                {
                    if (Session["emp_id"] != null)
                    {
                        ScholarshipCatagory.CreatedBy = Convert.ToInt32(Session["emp_id"]);
                        ScholarshipCatagory.CreatedOn = DateTime.Now;
                        db.ScholarshipCatagories.Add(ScholarshipCatagory);
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
                    else {
                        ModelState.AddModelError(string.Empty, "Your session expires please login again.");
                        ViewBag.MessageType = "error";
                        ViewBag.Message = "Your session expires please login again.";
                    }
                   
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Scholarship Category already exists.");
                    ViewBag.MessageType = "error";
                    ViewBag.Message = "Scholarship Category already exists.";
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
            model.ScholarshipCatagories = db.ScholarshipCatagories.ToList();
            model.SelectedScholarshipCatagory = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc");
            return View("Index", model);
        }

        public ActionResult Update(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ScholarshipCatagory ScholarshipCatagory = db.ScholarshipCatagories.Find(id);
            if (ScholarshipCatagory == null)
            {
                return HttpNotFound();
            }

            model.ScholarshipCatagories = db.ScholarshipCatagories.ToList();
            model.SelectedScholarshipCatagory = ScholarshipCatagory;
            model.DisplayMode = "ReadWrite";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc");
            ViewBag.MessageType = "";
            ViewBag.Message = "";
            return View("Index", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ScholarshipCatagory ScholarshipCatagory)
        {
            try
            {
                db.Entry(ScholarshipCatagory).State = EntityState.Modified;
                ScholarshipCatagory.ModifiedBy = Convert.ToInt32(Session["emp_id"]);
                ScholarshipCatagory.ModifiedOn = DateTime.Now;
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
            model.ScholarshipCatagories = db.ScholarshipCatagories.ToList();
            model.SelectedScholarshipCatagory = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc");

            return View("Index", model);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ScholarshipCatagory ScholarshipCatagory = db.ScholarshipCatagories.Find(id);
            if (ScholarshipCatagory == null)
            {
                return HttpNotFound();
            }

            db.ScholarshipCatagories.Remove(ScholarshipCatagory);
            db.SaveChanges();

            model.ScholarshipCatagories = db.ScholarshipCatagories.ToList();
            model.SelectedScholarshipCatagory = ScholarshipCatagory;
            model.DisplayMode = "ReadWrite";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc");
            ViewBag.MessageType = "";
            ViewBag.Message = "";
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
