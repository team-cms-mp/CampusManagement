﻿using System;
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
    [Authorize(Roles = "Account Officer,Accounts Officer,Admin Assistant,Admin Officer,Admin.Assistant,Assist. Account Officer,Assist.Technician,Import Manager,Manager Servive & Support,Office Manager,Officer QMS,RSM - Center 2,RSM - South,Sales & Service Executive,Sales Executive,Sales Manager,Sales Representative,Sr.Accounts Officer,Sr.Associate Engineer,Sr.Sales Executive,Sr.Sales Representative,Store Assistant,Store Incharge,Technician")]
    public class ApplicantQualificationsController : Controller
    {
        private ModelCMSContainer db = new ModelCMSContainer();
        ApplicantQualificationViewModel model = new ApplicantQualificationViewModel();

        public ActionResult Index(string FormNo)
        {
            model.ApplicantQualifications = db.ApplicantQualifications.Where(a => a.FormNo == FormNo && !a.Degree.DegreeName.Contains("Interview") && !a.Degree.DegreeName.Contains("Test")).OrderByDescending(a => a.AppQualiID).ToList();
            model.SelectedApplicantQualification = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc");
            ViewBag.DegreeID = new SelectList(db.Degrees.Where(d => !d.DegreeName.Contains("Interview") && !d.DegreeName.Contains("Test")), "DegreeID", "DegreeName");
            ViewBag.InstituteID = new SelectList(db.Institutes, "InstituteID", "InstituteName");
            ViewBag.MessageType = "";
            ViewBag.Message = "";
            ViewBag.FormNo = FormNo;
            return View(model);
        }

        [HttpGet]
        public ActionResult Create(string FormNo)
        {
            model.ApplicantQualifications = db.ApplicantQualifications.Where(a => a.FormNo == FormNo && !a.Degree.DegreeName.Contains("Interview") && !a.Degree.DegreeName.Contains("Test")).OrderByDescending(a => a.AppQualiID).ToList();
            model.SelectedApplicantQualification = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc");
            ViewBag.DegreeID = new SelectList(db.Degrees.Where(d => !d.DegreeName.Contains("Interview") && !d.DegreeName.Contains("Test")), "DegreeID", "DegreeName");
            ViewBag.InstituteID = new SelectList(db.Institutes, "InstituteID", "InstituteName");
            ViewBag.MessageType = "";
            ViewBag.Message = "";
            ViewBag.FormNo = FormNo;
            return View("Index", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ApplicantQualification applicantqualification)
        {
            string ErrorMessage = "";
            int count = 0;
            try
            {
                try
                {
                    if (applicantqualification.UploadDocument != null)
                    {
                        applicantqualification.DegreeDocument = "~/DegreeDocument/" + string.Concat(applicantqualification.FormNo, "_", DateTime.Now.Ticks, "_", applicantqualification.UploadDocument.FileName);
                        applicantqualification.UploadDocument.SaveAs(Server.MapPath("~/DegreeDocument") + "/" + string.Concat(applicantqualification.FormNo, "_", DateTime.Now.Ticks, "_", applicantqualification.UploadDocument.FileName));
                    }
                }
                catch (Exception)
                {
                    ModelState.AddModelError(string.Empty, "Document is not saved.");
                    count++;
                    ErrorMessage += count + "-Document is not saved.<br />";
                }

                applicantqualification.CreatedBy = Convert.ToInt32(Session["emp_id"]);
                applicantqualification.CreatedOn = DateTime.Now;
                db.ApplicantQualifications.Add(applicantqualification);
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
            model.ApplicantQualifications = db.ApplicantQualifications.Where(a => a.FormNo == applicantqualification.FormNo && !a.Degree.DegreeName.Contains("Interview") && !a.Degree.DegreeName.Contains("Test")).OrderByDescending(a => a.AppQualiID).ToList();
            model.SelectedApplicantQualification = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc", applicantqualification.IsActive);
            ViewBag.DegreeID = new SelectList(db.Degrees.Where(d => !d.DegreeName.Contains("Interview") && !d.DegreeName.Contains("Test")), "DegreeID", "DegreeName", applicantqualification.DegreeID);
            ViewBag.InstituteID = new SelectList(db.Institutes, "InstituteID", "InstituteName", applicantqualification.InstituteID);
            ViewBag.FormNo = applicantqualification.FormNo;
            return View("Index", model);
        }

        public ActionResult Update(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ApplicantQualification applicantqualification = db.ApplicantQualifications.Find(id);
            if (applicantqualification == null)
            {
                return HttpNotFound();
            }

            model.ApplicantQualifications = db.ApplicantQualifications.Where(a => a.FormNo == applicantqualification.FormNo && !a.Degree.DegreeName.Contains("Interview") && !a.Degree.DegreeName.Contains("Test")).ToList();
            model.SelectedApplicantQualification = applicantqualification;
            model.DisplayMode = "ReadWrite";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc", applicantqualification.IsActive);
            ViewBag.DegreeID = new SelectList(db.Degrees.Where(d => !d.DegreeName.Contains("Interview") && !d.DegreeName.Contains("Test")), "DegreeID", "DegreeName", applicantqualification.DegreeID);
            ViewBag.InstituteID = new SelectList(db.Institutes, "InstituteID", "InstituteName", applicantqualification.InstituteID);
            ViewBag.MessageType = "";
            ViewBag.Message = "";
            ViewBag.FormNo = applicantqualification.FormNo;
            return View("Index", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ApplicantQualification applicantqualification)
        {
            string ErrorMessage = "";
            int count = 0;
            try
            {
                db.Entry(applicantqualification).State = EntityState.Modified;
                try
                {
                    if (applicantqualification.UploadDocument != null)
                    {
                        applicantqualification.DegreeDocument = "~/DegreeDocument/" + string.Concat(applicantqualification.FormNo, "_", DateTime.Now.Ticks, "_", applicantqualification.UploadDocument.FileName);
                        applicantqualification.UploadDocument.SaveAs(Server.MapPath("~/DegreeDocument") + "/" + string.Concat(applicantqualification.FormNo, "_", DateTime.Now.Ticks, "_", applicantqualification.UploadDocument.FileName));
                    }
                }
                catch (Exception)
                {
                    ModelState.AddModelError(string.Empty, "Document is not saved.");
                    count++;
                    ErrorMessage += count + "-Document is not saved.<br />";
                }

                applicantqualification.ModifiedBy = Convert.ToInt32(Session["emp_id"]);
                applicantqualification.ModifiedOn = DateTime.Now;
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
            model.ApplicantQualifications = db.ApplicantQualifications.Where(a => a.FormNo == applicantqualification.FormNo && !a.Degree.DegreeName.Contains("Interview") && !a.Degree.DegreeName.Contains("Test")).OrderByDescending(a => a.AppQualiID).ToList();
            model.SelectedApplicantQualification = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc", applicantqualification.IsActive);
            ViewBag.DegreeID = new SelectList(db.Degrees.Where(d => !d.DegreeName.Contains("Interview") && !d.DegreeName.Contains("Test")), "DegreeID", "DegreeName", applicantqualification.DegreeID);
            ViewBag.InstituteID = new SelectList(db.Institutes, "InstituteID", "InstituteName", applicantqualification.InstituteID);
            ViewBag.FormNo = applicantqualification.FormNo;
            return View("Index", model);
        }

        public ActionResult Delete(int? id, string FormNo)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicantQualification applicantqualification = db.ApplicantQualifications.Find(id);
            if (applicantqualification == null)
            {
                return HttpNotFound();
            }

            model.ApplicantQualifications = db.ApplicantQualifications.Where(a => a.FormNo == FormNo && !a.Degree.DegreeName.Contains("Interview") && !a.Degree.DegreeName.Contains("Test")).OrderByDescending(a => a.AppQualiID).ToList();
            model.SelectedApplicantQualification = applicantqualification;
            model.DisplayMode = "Delete";
            ViewBag.MessageType = "";
            ViewBag.Message = "";
            ViewBag.FormNo = FormNo;
            return View("Index", model);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id, string FormNo)
        {
            ApplicantQualification applicantqualification = new ApplicantQualification();
            try
            {
                applicantqualification = db.ApplicantQualifications.Find(id);
                ViewBag.FormNo = FormNo;
                db.ApplicantQualifications.Remove(applicantqualification);
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
            model.ApplicantQualifications = db.ApplicantQualifications.Where(a => a.FormNo == FormNo && !a.Degree.DegreeName.Contains("Interview") && !a.Degree.DegreeName.Contains("Test")).OrderByDescending(a => a.AppQualiID).ToList();
            model.SelectedApplicantQualification = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc");
            ViewBag.DegreeID = new SelectList(db.Degrees.Where(d => !d.DegreeName.Contains("Interview") && !d.DegreeName.Contains("Test")), "DegreeID", "DegreeName");
            ViewBag.InstituteID = new SelectList(db.Institutes, "InstituteID", "InstituteName");
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
