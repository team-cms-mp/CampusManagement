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
    public class DegreeTitlesController : Controller
    {
        private ModelCMSNewContainer db = new ModelCMSNewContainer();
        DegreeTitleViewModel model = new DegreeTitleViewModel();

        public ActionResult Index()
        {
            model.DegreeTitles = db.SP_DegreeTitleForPage("").ToList();
            model.SelectedDegreeTitle = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc");
            ViewBag.DegreeID = new SelectList(db.GetDegreesList(1), "DegreeID", "DegreeName");
            ViewBag.MessageType = "";
            ViewBag.Message = "";
            return View(model);
        }

        [HttpGet]
        public ActionResult Create()
        {
            model.DegreeTitles = db.SP_DegreeTitleForPage("").ToList();
            model.SelectedDegreeTitle = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc");
            ViewBag.DegreeID = new SelectList(db.GetDegreesList(1), "DegreeID", "DegreeName");
            ViewBag.MessageType = "";
            ViewBag.Message = "";
            return View("Index", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(DegreeTitle degreeTitle)
        {
            try
            {
                if (string.IsNullOrEmpty(degreeTitle.DegreeTitleName))
                {
                    ViewBag.MessageType = "error";
                    ViewBag.Message = "Degree Title is required.";
                }
                else
                {
                    DegreeTitle ba = db.DegreeTitles.FirstOrDefault(bac => bac.DegreeTitleName == degreeTitle.DegreeTitleName);
                    if (ba == null)
                    {
                        degreeTitle.CreatedBy = Convert.ToInt32(Session["emp_id"]);
                        degreeTitle.CreatedOn = DateTime.Now;
                        db.DegreeTitles.Add(degreeTitle);
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
                        ViewBag.MessageType = "error";
                        ViewBag.Message = "Degree Title already exists.";
                    }
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
            model.DegreeTitles = db.SP_DegreeTitleForPage("").ToList();
            model.SelectedDegreeTitle = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc", degreeTitle.IsActive);
            ViewBag.DegreeID = new SelectList(db.GetDegreesList(1), "DegreeID", "DegreeName", degreeTitle.DegreeID);

            return View("Index", model);
        }

        public ActionResult Update(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            SP_DegreeTitleForPage_Result degreeTitle = db.SP_DegreeTitleForPage("").FirstOrDefault(s=> s.DegreeTitleID == id);
            if (degreeTitle == null)
            {
                return HttpNotFound();
            }

            model.DegreeTitles = db.SP_DegreeTitleForPage("").ToList();
            model.SelectedDegreeTitle = degreeTitle;
            model.DisplayMode = "ReadWrite";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc", degreeTitle.IsActive);
            ViewBag.DegreeID = new SelectList(db.GetDegreesList(1), "DegreeID", "DegreeName", degreeTitle.DegreeID);
            ViewBag.MessageType = "";
            ViewBag.Message = "";
            return View("Index", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(DegreeTitle degreeTitle)
        {
            try
            {
                db.Entry(degreeTitle).State = EntityState.Modified;
                degreeTitle.ModifiedBy = Convert.ToInt32(Session["emp_id"]);
                degreeTitle.ModifiedOn = DateTime.Now;
                degreeTitle.IsActive = "Yes";
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
            model.DegreeTitles = db.SP_DegreeTitleForPage("").ToList();
            model.SelectedDegreeTitle = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc", degreeTitle.IsActive);
            ViewBag.DegreeID = new SelectList(db.GetDegreesList(1), "DegreeID", "DegreeName", degreeTitle.DegreeID);

            return View("Index", model);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SP_DegreeTitleForPage_Result degreeTitle = db.SP_DegreeTitleForPage("").FirstOrDefault(s => s.DegreeTitleID == id);
            if (degreeTitle == null)
            {
                return HttpNotFound();
            }

            model.DegreeTitles = db.SP_DegreeTitleForPage("").ToList();
            model.SelectedDegreeTitle = degreeTitle;
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
                DegreeTitle degreeTitle = db.DegreeTitles.Find(id);
                db.DegreeTitles.Remove(degreeTitle);
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
            model.DegreeTitles = db.SP_DegreeTitleForPage("").ToList();
            model.SelectedDegreeTitle = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc");
            ViewBag.DegreeID = new SelectList(db.GetDegreesList(1), "DegreeID", "DegreeName");

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
