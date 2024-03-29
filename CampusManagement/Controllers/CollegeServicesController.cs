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
    public class CollegeServicesController : Controller
    {
        private ModelCMSContainer db = new ModelCMSContainer();
        CollegeServicesViewModel model = new CollegeServicesViewModel();

        public ActionResult Index()
        {
            model.CollegeServices = db.CollegeServices.OrderByDescending(a=>a.CollegeServiceID).ToList();
            model.SelectedCollegeService = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc");
            ViewBag.IsEffectProgramFee = new SelectList(db.Options, "OptionDesc", "OptionDesc");
            ViewBag.MessageType = "";
            ViewBag.Message = "";
            return View(model);
        }

        [HttpGet]
        public ActionResult Create()
        {
            model.CollegeServices = db.CollegeServices.OrderByDescending(a=>a.CollegeServiceID).ToList();
            model.SelectedCollegeService = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc");
            ViewBag.IsEffectProgramFee = new SelectList(db.Options, "OptionDesc", "OptionDesc");
            ViewBag.MessageType = "";
            ViewBag.Message = "";
            return View("Index", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CollegeService collegeService)
        {
            try
            {
                CollegeService cs = db.CollegeServices.FirstOrDefault(c => c.CollegeServiceName == collegeService.CollegeServiceName);
                if (cs == null)
                {
                    collegeService.CreatedBy = Convert.ToInt32(Session["emp_id"]);
                    collegeService.CreatedOn = DateTime.Now;
                    db.CollegeServices.Add(collegeService);
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
                    ModelState.AddModelError(string.Empty, "Service Name is already exists.");
                    ViewBag.MessageType = "error";
                    ViewBag.Message = "Service Name is already exists.";
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
            model.CollegeServices = db.CollegeServices.OrderByDescending(a=>a.CollegeServiceID).ToList();
            model.SelectedCollegeService = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc", collegeService.IsActive);
            ViewBag.IsEffectProgramFee = new SelectList(db.Options, "OptionDesc", "OptionDesc", collegeService.CollegeServiceID);
            return View("Index", model);
        }

        public ActionResult Update(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            CollegeService collegeService = db.CollegeServices.Find(id);
            if (collegeService == null)
            {
                return HttpNotFound();
            }

            model.CollegeServices = db.CollegeServices.OrderByDescending(a=>a.CollegeServiceID).ToList();
            model.SelectedCollegeService = collegeService;
            model.DisplayMode = "ReadWrite";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc", collegeService.IsActive);
            ViewBag.IsEffectProgramFee = new SelectList(db.Options, "OptionDesc", "OptionDesc", collegeService.CollegeServiceID);
            ViewBag.MessageType = "";
            ViewBag.Message = "";
            return View("Index", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CollegeService collegeService)
        {
            try
            {
                db.Entry(collegeService).State = EntityState.Modified;
                collegeService.ModifiedBy = Convert.ToInt32(Session["emp_id"]);
                collegeService.ModifiedOn = DateTime.Now;
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
            model.CollegeServices = db.CollegeServices.OrderByDescending(a=>a.CollegeServiceID).ToList();
            model.SelectedCollegeService = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc", collegeService.IsActive);
            ViewBag.IsEffectProgramFee = new SelectList(db.Options, "OptionDesc", "OptionDesc", collegeService.CollegeServiceID);

            return View("Index", model);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CollegeService collegeService = db.CollegeServices.Find(id);
            if (collegeService == null)
            {
                return HttpNotFound();
            }

            model.CollegeServices = db.CollegeServices.OrderByDescending(a=>a.CollegeServiceID).ToList();
            model.SelectedCollegeService = collegeService;
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
                CollegeService collegeService = db.CollegeServices.Find(id);
                db.CollegeServices.Remove(collegeService);
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
            model.CollegeServices = db.CollegeServices.OrderByDescending(a=>a.CollegeServiceID).ToList();
            model.SelectedCollegeService = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc");
            ViewBag.IsEffectProgramFee = new SelectList(db.Options, "OptionDesc", "OptionDesc");

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
