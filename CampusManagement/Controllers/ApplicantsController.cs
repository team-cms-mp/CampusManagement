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
    [Authorize(Roles = "Account Officer,Accounts Officer,Admin Assistant,Admin Officer,Admin.Assistant,Assist. Account Officer,Assist.Technician,Import Manager,Manager Servive & Support,Office Manager,Officer QMS,RSM - Center 2,RSM - South,Sales & Service Executive,Sales Executive,Sales Manager,Sales Representative,Sr.Accounts Officer,Sr.Associate Engineer,Sr.Sales Executive,Sr.Sales Representative,Store Assistant,Store Incharge,Technician")]
    public class ApplicantsController : Controller
    {
        private ModelCMSContainer db = new ModelCMSContainer();
        string ErrorMessage = "";
        int count = 0;
       
        public ActionResult Index()
        {
            return View(db.Applicants.OrderByDescending(a => a.AddAppID));
        }

        [HttpPost]
        public ActionResult Index(string Search)
        {
            return View(db.Applicants.Where(x => x.FirstName.Contains(Search) || x.Email.Contains(Search) || x.LastName.Contains(Search)).OrderByDescending(a => a.AddAppID));
        }

        public ActionResult Create()
        {
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc");
            ViewBag.CityID = new SelectList(db.Cities, "CityID", "CityName");
            ViewBag.CountryID = new SelectList(db.Countries, "CountryID", "CountryName");
            ViewBag.CurrentOccupationID = new SelectList(db.CurrentOccupations, "CurrentOccupationID", "CurrentOccupationName");
            ViewBag.GenderID = new SelectList(db.Genders, "GenderID", "GenderName");
            ViewBag.MaritalStatusID = new SelectList(db.MaritalStatus, "MaritalStatusID", "MaritalStatusName");
            ViewBag.NationalityID = new SelectList(db.Nationalities, "NationalityID", "NationalityName");
            ViewBag.ProvinceID = new SelectList(db.Provinces, "ProvinceID", "ProvinceName");
            ViewBag.RelationTypeID = new SelectList(db.RelationTypes, "RelationTypeID", "RelationTypeName");
            ViewBag.ReligionID = new SelectList(db.Religions, "ReligionID", "ReligionName");
            ViewBag.SalutationID = new SelectList(db.Salutations, "SalutationID", "SalutationName");
            ViewBag.StatusID = new SelectList(db.Status, "StatusID", "StatusName");
            ViewBag.SelectedPrograms = new SelectList(db.GetBatchProgramNameConcat("", 0), "ID", "Name");
            ViewBag.FormNo = Convert.ToInt32(db.Applicants.Max(a => a.FormNo)) + 1;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Applicant applicant)
        {
            applicant.CreatedOn = DateTime.Now;
            applicant.CreatedBy = Convert.ToInt32(Session["emp_id"]);
            try
            {
                applicant.Picture = "~/ProfilePics/" + string.Concat(applicant.FormNo, "_", applicant.ProfilePicture.FileName);
                applicant.ProfilePicture.SaveAs(Server.MapPath("~/ProfilePics") + "/" + string.Concat(applicant.FormNo, "_", applicant.ProfilePicture.FileName));
            }
            catch (Exception)
            {
                ModelState.AddModelError(string.Empty, "Profile pictuer is required.");
                count++;
                ErrorMessage += count + "-Profile pictuer is required.<br />";
            }

            try
            {
                db.Applicants.Add(applicant);
                try
                {
                    for (int i = 0; i < applicant.SelectedPrograms.Length; i++)
                    {
                        applicant.BatchProgramID = Convert.ToInt32(applicant.SelectedPrograms[i]);

                        ApplyForProgram afp = new ApplyForProgram();
                        afp.FormNo = applicant.FormNo;
                        afp.ProgramID = Convert.ToInt32(applicant.SelectedPrograms[i]); //Using for BatchProgramID
                        afp.IsActive = applicant.IsActive;
                        afp.CreatedBy = Convert.ToInt32(Session["emp_id"]);
                        afp.CreatedOn = DateTime.Now;
                        db.ApplyForPrograms.Add(afp);
                    }
                }
                catch (Exception)
                {
                    count++;
                    ErrorMessage += count + "-Please select Program(s).<br />";
                }

                try
                {
                    if (string.IsNullOrEmpty(ErrorMessage))
                    {
                        applicant.FormNo = Convert.ToString(Convert.ToInt32(db.Applicants.Max(a => a.FormNo)) + 1);
                        db.SaveChanges();
                        ViewBag.MessageType = "success";
                        ViewBag.Message = "Data has been saved successfully.";
                        return RedirectToAction("Index", "Applicants");
                    }
                    else
                    {
                        ViewBag.MessageType = "error";
                        ViewBag.Message = ErrorMessage;
                    }
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

            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc", applicant.IsActive);
            ViewBag.CityID = new SelectList(db.Cities, "CityID", "CityName", applicant.CityID);
            ViewBag.CountryID = new SelectList(db.Countries, "CountryID", "CountryName", applicant.CountryID);
            ViewBag.CurrentOccupationID = new SelectList(db.CurrentOccupations, "CurrentOccupationID", "CurrentOccupationName", applicant.CurrentOccupationID);
            ViewBag.GenderID = new SelectList(db.Genders, "GenderID", "GenderName", applicant.GenderID);
            ViewBag.MaritalStatusID = new SelectList(db.MaritalStatus, "MaritalStatusID", "MaritalStatusName", applicant.MaritalStatusID);
            ViewBag.NationalityID = new SelectList(db.Nationalities, "NationalityID", "NationalityName", applicant.NationalityID);
            ViewBag.ProvinceID = new SelectList(db.Provinces, "ProvinceID", "ProvinceName", applicant.ProvinceID);
            ViewBag.RelationTypeID = new SelectList(db.RelationTypes, "RelationTypeID", "RelationTypeName", applicant.RelationTypeID);
            ViewBag.ReligionID = new SelectList(db.Religions, "ReligionID", "ReligionName", applicant.ReligionID);
            ViewBag.SalutationID = new SelectList(db.Salutations, "SalutationID", "SalutationName", applicant.SalutationID);
            ViewBag.StatusID = new SelectList(db.Status, "StatusID", "StatusName", applicant.StatusID);
            ViewBag.SelectedPrograms = new SelectList(db.GetBatchProgramNameConcat("", 0), "ID", "Name", applicant.BatchProgramID);
            ViewBag.FormNo = applicant.FormNo;
            return View(applicant);
        }

        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Applicant applicant = db.Applicants.Find(id);
            if (applicant == null)
            {
                return HttpNotFound();
            }

            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc", applicant.IsActive);
            ViewBag.CityID = new SelectList(db.Cities, "CityID", "CityName", applicant.CityID);
            ViewBag.CountryID = new SelectList(db.Countries, "CountryID", "CountryName", applicant.CountryID);
            ViewBag.CurrentOccupationID = new SelectList(db.CurrentOccupations, "CurrentOccupationID", "CurrentOccupationName", applicant.CurrentOccupationID);
            ViewBag.GenderID = new SelectList(db.Genders, "GenderID", "GenderName", applicant.GenderID);
            ViewBag.MaritalStatusID = new SelectList(db.MaritalStatus, "MaritalStatusID", "MaritalStatusName", applicant.MaritalStatusID);
            ViewBag.NationalityID = new SelectList(db.Nationalities, "NationalityID", "NationalityName", applicant.NationalityID);
            ViewBag.ProvinceID = new SelectList(db.Provinces, "ProvinceID", "ProvinceName", applicant.ProvinceID);
            ViewBag.RelationTypeID = new SelectList(db.RelationTypes, "RelationTypeID", "RelationTypeName", applicant.RelationTypeID);
            ViewBag.ReligionID = new SelectList(db.Religions, "ReligionID", "ReligionName", applicant.ReligionID);
            ViewBag.SalutationID = new SelectList(db.Salutations, "SalutationID", "SalutationName", applicant.SalutationID);
            ViewBag.StatusID = new SelectList(db.Status, "StatusID", "StatusName", applicant.StatusID);
            ViewBag.SelectedPrograms = new SelectList(db.GetBatchProgramNameConcat(applicant.FormNo, 6), "ID", "Name");
            ViewBag.FormNo = applicant.FormNo;
            return View(applicant);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Applicant applicant)
        {
            try
            {
                if (applicant.ProfilePicture != null)
                {
                    applicant.Picture = "~/ProfilePics/" + string.Concat(applicant.FormNo, "_", applicant.ProfilePicture.FileName);
                    applicant.ProfilePicture.SaveAs(Server.MapPath("~/ProfilePics") + "/" + string.Concat(applicant.FormNo, "_", applicant.ProfilePicture.FileName));
                }
            }
            catch (Exception)
            {
                ModelState.AddModelError(string.Empty, "Profile pictuer is not saved.");
                count++;
                ErrorMessage += count + "-Profile pictuer is not saved.<br />";
            }

            try
            {
                db.Entry(applicant).State = EntityState.Modified;
                applicant.ModifiedOn = DateTime.Now;
                applicant.ModifiedBy = Convert.ToInt32(Session["emp_id"]);

                List<ApplyForProgram> afpList = db.ApplyForPrograms.ToList().Where(a => a.FormNo == applicant.FormNo).ToList();
                db.ApplyForPrograms.RemoveRange(afpList);

                try
                {
                    for (int i = 0; i < applicant.SelectedPrograms.Length; i++)
                    {
                        applicant.BatchProgramID = Convert.ToInt32(applicant.SelectedPrograms[i]);

                        ApplyForProgram afp = new ApplyForProgram();
                        afp.FormNo = applicant.FormNo;
                        afp.ProgramID = Convert.ToInt32(applicant.SelectedPrograms[i]); //Using for BatchProgramID
                        afp.IsActive = applicant.IsActive;
                        afp.CreatedBy = Convert.ToInt32(Session["emp_id"]);
                        afp.CreatedOn = DateTime.Now;
                        db.ApplyForPrograms.Add(afp);
                    }
                }
                catch (Exception)
                {
                    count++;
                    ErrorMessage += count + "-Please select Program(s).<br />";
                }

                try
                {
                    db.SaveChanges();
                    ViewBag.MessageType = "success";
                    ViewBag.Message = "Data has been saved successfully.";
                    return RedirectToAction("Index", "Applicants");
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

            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc", applicant.IsActive);
            ViewBag.CityID = new SelectList(db.Cities, "CityID", "CityName", applicant.CityID);
            ViewBag.CountryID = new SelectList(db.Countries, "CountryID", "CountryName", applicant.CountryID);
            ViewBag.CurrentOccupationID = new SelectList(db.CurrentOccupations, "CurrentOccupationID", "CurrentOccupationName", applicant.CurrentOccupationID);
            ViewBag.GenderID = new SelectList(db.Genders, "GenderID", "GenderName", applicant.GenderID);
            ViewBag.MaritalStatusID = new SelectList(db.MaritalStatus, "MaritalStatusID", "MaritalStatusName", applicant.MaritalStatusID);
            ViewBag.NationalityID = new SelectList(db.Nationalities, "NationalityID", "NationalityName", applicant.NationalityID);
            ViewBag.ProvinceID = new SelectList(db.Provinces, "ProvinceID", "ProvinceName", applicant.ProvinceID);
            ViewBag.RelationTypeID = new SelectList(db.RelationTypes, "RelationTypeID", "RelationTypeName", applicant.RelationTypeID);
            ViewBag.ReligionID = new SelectList(db.Religions, "ReligionID", "ReligionName", applicant.ReligionID);
            ViewBag.SalutationID = new SelectList(db.Salutations, "SalutationID", "SalutationName", applicant.SalutationID);
            ViewBag.StatusID = new SelectList(db.Status, "StatusID", "StatusName", applicant.StatusID);
            ViewBag.SelectedPrograms = new SelectList(db.GetBatchProgramNameConcat(applicant.FormNo, 6), "ID", "Name");
            ViewBag.FormNo = applicant.FormNo;
            return View(applicant);
        }

        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Applicant applicant = db.Applicants.Find(id);
            if (applicant == null)
            {
                return HttpNotFound();
            }
            return View(applicant);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Applicant applicant = db.Applicants.Find(id);
            List<ApplyForProgram> afpList = db.ApplyForPrograms.ToList().Where(a => a.FormNo == applicant.FormNo).ToList();
            db.ApplyForPrograms.RemoveRange(afpList);
            List<ApplicantQualification> aqList = db.ApplicantQualifications.ToList().Where(a => a.FormNo == applicant.FormNo).ToList();
            db.ApplicantQualifications.RemoveRange(aqList);
            db.Applicants.Remove(applicant);
            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException ex)
            {
                throw ex;
            }
            return RedirectToAction("Index");
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
