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
    public class ScholarshipDocumentsController : Controller
    {
        private ModelCMSNewContainer db = new ModelCMSNewContainer();
        ScholarshipDocumentViewModel model = new ScholarshipDocumentViewModel();

        public ActionResult Index(int? ScholarshipOpportunitiesID)
        {

            int EmpTeacherID = Convert.ToInt32(Session["emp_id"]);

            model.ScholarshipDocuments = db.ScholarshipDocuments.Where(x => x.ScholarshipOpportunitiesID == ScholarshipOpportunitiesID ).ToList();

            model.ScholarshipDocuments = db.ScholarshipDocuments.ToList();
            model.SelectedScholarshipDocument = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc");
            ViewBag.MessageType = "";
            ViewBag.ScholarshipOpportunitiesID = ScholarshipOpportunitiesID;
            ViewBag.Message = "";
            return View(model);
        }


        [HttpGet]
        public ActionResult Create(int? ScholarshipOpportunitiesID)
        {
            model.ScholarshipDocuments = db.ScholarshipDocuments.ToList();
            model.SelectedScholarshipDocument = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc");
            ViewBag.ScholarshipOpportunitiesID = ScholarshipOpportunitiesID;
            ViewBag.MessageType = "";
            ViewBag.Message = "";
            return View("Index", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ScholarshipDocument ScholarshipDocument)
        {
            try
            {
                ScholarshipDocument d = db.ScholarshipDocuments.FirstOrDefault(de => de.ScholarshipDocumentName == ScholarshipDocument.ScholarshipDocumentName);
                if (d == null)
                {
                    ScholarshipDocument.CreatedBy = Convert.ToInt32(Session["emp_id"]);
                    ScholarshipDocument.CreatedOn = DateTime.Now;
                    db.ScholarshipDocuments.Add(ScholarshipDocument);
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
                    ModelState.AddModelError(string.Empty, "Scholarship Document already exists.");
                    ViewBag.MessageType = "error";
                    ViewBag.Message = "Scholarship Document already exists.";
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
            model.ScholarshipDocuments = db.ScholarshipDocuments.ToList();
            model.SelectedScholarshipDocument = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.ScholarshipOpportunitiesID = ScholarshipDocument.ScholarshipOpportunitiesID;
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc");
            return View("Index", model);
        }

        public ActionResult Update(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ScholarshipDocument ScholarshipDocument = db.ScholarshipDocuments.Find(id);
            if (ScholarshipDocument == null)
            {
                return HttpNotFound();
            }

            model.ScholarshipDocuments = db.ScholarshipDocuments.ToList();
            model.SelectedScholarshipDocument = ScholarshipDocument;
            model.DisplayMode = "ReadWrite";

            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc");
            ViewBag.MessageType = "";
            ViewBag.Message = "";
            return View("Index", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ScholarshipDocument ScholarshipDocument)
        {
            try
            {
                db.Entry(ScholarshipDocument).State = EntityState.Modified;
                ScholarshipDocument.ModifiedBy = Convert.ToInt32(Session["emp_id"]);
                ScholarshipDocument.ModifiedOn = DateTime.Now;
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
            model.ScholarshipDocuments = db.ScholarshipDocuments.ToList();
            model.SelectedScholarshipDocument = null;
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
