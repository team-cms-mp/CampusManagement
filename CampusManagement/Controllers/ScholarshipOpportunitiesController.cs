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
    public class ScholarshipOpportunitiesController : Controller

    {
        int count = 0;
        string ErrorMessage = "";
        private ModelCMSNewContainer db = new ModelCMSNewContainer();
        ScholarshipOpportunitieViewModel model = new ScholarshipOpportunitieViewModel();

        public ActionResult Index()
        {

            model.ScholarshipOpportunities = db.ScholarshipOpportunities.ToList();
            model.SelectedScholarshipOpportunitie = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.ScholarshipCatagoryID = new SelectList(db.ScholarshipCatagories, "ScholarshipCatagoryID", "ScholarshipCatagoryName");
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc");
            ViewBag.MessageType = "";
            ViewBag.Message = "";
            return View(model);
        }


        [HttpGet]
        public ActionResult Create()
        {
            model.ScholarshipOpportunities = db.ScholarshipOpportunities.ToList();
            model.SelectedScholarshipOpportunitie = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.ScholarshipCatagoryID = new SelectList(db.ScholarshipCatagories, "ScholarshipCatagoryID", "ScholarshipCatagoryName");
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc");

            ViewBag.MessageType = "";
            ViewBag.Message = "";
            return View("Index", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ScholarshipOpportunitie ScholarshipOpportunitie)
        {
            try
            {
                ScholarshipOpportunitie d = db.ScholarshipOpportunities.FirstOrDefault(de => de.ScholarshipCatagoryID == ScholarshipOpportunitie.ScholarshipCatagoryID);
                if (d == null)

                {
                    if (Session["emp_id"] != null)
                    {
                        try
                    {
                        ScholarshipOpportunitie.ScholarshipImagePath = string.Concat("~/DegreeDocument/", DateTime.Now.Ticks, "_", ScholarshipOpportunitie.UploadFiles.FileName.Replace(" ", ""));
                        ScholarshipOpportunitie.UploadFiles.SaveAs(Server.MapPath(ScholarshipOpportunitie.ScholarshipImagePath));
                    }
                    catch (Exception)
                    {
                        ModelState.AddModelError(string.Empty, "-Please attach the document.");
                        count++;
                        ErrorMessage += count + "-Please attach the document.<br />";
                    }
                    ScholarshipOpportunitie.CreatedBy = Convert.ToInt32(Session["emp_id"]);
                    ScholarshipOpportunitie.CreatedOn = DateTime.Now;
                    db.ScholarshipOpportunities.Add(ScholarshipOpportunitie);
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
                    ModelState.AddModelError(string.Empty, "Scholarship opportunity already exists.");
                    ViewBag.MessageType = "error";
                    ViewBag.Message = "Scholarship opportunity already exists.";
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
            model.ScholarshipOpportunities = db.ScholarshipOpportunities.ToList();
            model.SelectedScholarshipOpportunitie = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.ScholarshipCatagoryID = new SelectList(db.ScholarshipCatagories, "ScholarshipCatagoryID", "ScholarshipCatagoryName");
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc");
            return View("Index", model);
        }

        public ActionResult Update(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ScholarshipOpportunitie ScholarshipOpportunitie = db.ScholarshipOpportunities.Find(id);
            if (ScholarshipOpportunitie == null)
            {
                return HttpNotFound();
            }

            model.ScholarshipOpportunities = db.ScholarshipOpportunities.ToList();
            model.SelectedScholarshipOpportunitie = ScholarshipOpportunitie;
            model.DisplayMode = "ReadWrite";
            ViewBag.ScholarshipCatagoryID = new SelectList(db.ScholarshipCatagories, "ScholarshipCatagoryID", "ScholarshipCatagoryName");
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc");
            ViewBag.MessageType = "";
            ViewBag.Message = "";
            return View("Index", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ScholarshipOpportunitie ScholarshipOpportunitie)
        {
            if (Session["emp_id"] != null)
            {
                try
                {
                    db.Entry(ScholarshipOpportunitie).State = EntityState.Modified;
                    ScholarshipOpportunitie.ModifiedBy = Convert.ToInt32(Session["emp_id"]);
                    ScholarshipOpportunitie.ModifiedOn = DateTime.Now;
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
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Your session expires please login again.");
                ViewBag.MessageType = "error";
                ViewBag.Message = "Your session expires please login again.";
            }
            model.ScholarshipOpportunities = db.ScholarshipOpportunities.ToList();
            model.SelectedScholarshipOpportunitie = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.ScholarshipCatagoryID = new SelectList(db.ScholarshipCatagories, "ScholarshipCatagoryID", "ScholarshipCatagoryName");
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
