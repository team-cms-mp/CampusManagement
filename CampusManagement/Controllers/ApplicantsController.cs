using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CampusManagement.Models;
using Newtonsoft.Json;
using PagedList;
using System.Globalization;

namespace CampusManagement.Controllers
{
    [Authorize]
    public class ApplicantsController : Controller
    {
        private ModelCMSNewContainer db = new ModelCMSNewContainer();
        string ErrorMessage = "";
        int count = 0;
        int[] arrDegreeID = { 21, 22, 4020 };

        int? _BatchID = 0;

        public ApplicantsController()
        {
            Batch batch = db.Batches.FirstOrDefault(b => b.IsActive == "Current");
            if (batch != null)
            {
                _BatchID = batch.BatchID;
            }
        }

        [HttpGet]
        public ActionResult Index(int? page, int? pageSize, int? BatchID)
        {
            BatchID = _BatchID;
            if (pageSize == null)
            {
                pageSize = 10;
            }
            ViewBag.pageSize = pageSize;
            int pageNumber = (page ?? 1);

            ViewBag.BatchIDAssigned = BatchID;
            ViewBag.BatchID = new SelectList(db.GetBatchProgramNameConcat("", 10), "ID", "Name", BatchID);

            ViewBag.TotalRecords = db.GetApplicantListSearch("", BatchID, null, null).ToList().Count;
            return View(db.GetApplicantListSearch("", BatchID, null, null).ToList().ToPagedList(pageNumber, Convert.ToInt32(pageSize)));
        }

        [HttpPost]
        public ActionResult Index(string Search, int BatchID, string daterange, int? page, int? pageSize)
        {
            if (pageSize == null)
            {
                pageSize = 10;
            }
            ViewBag.pageSize = pageSize;
            int pageNumber = (page ?? 1);

            ViewBag.BatchID = new SelectList(db.GetBatchProgramNameConcat("", 10), "ID", "Name", BatchID);
            ViewBag.BatchIDAssigned = BatchID;
            //Code will be commented if date search needed
            daterange = "";
            if (daterange == "")
            {
                ViewBag.daterange = daterange;
                ViewBag.Search = Search;
                ViewBag.TotalRecords = db.GetApplicantListSearch(Search, BatchID, null, null).ToList().Count;
                return View(db.GetApplicantListSearch(Search, BatchID, null, null).ToList().ToPagedList(pageNumber, Convert.ToInt32(pageSize)));
            }
            else
            {
                DateTime dtStartDate = new DateTime(2019, 1, 25);
                DateTime dtEndDate = new DateTime(2019, 1, 25);
                var split = daterange.Split('-');
                if (split.Length == 2)
                {
                    ViewBag.daterange = daterange;
                    ViewBag.Search = Search;
                    dtStartDate = DateTime.ParseExact(split[0].Trim(), "mm/dd/yyyy", CultureInfo.InvariantCulture);
                    dtEndDate = DateTime.ParseExact(split[1].Trim(), "mm/dd/yyyy", CultureInfo.InvariantCulture);

                }
                ViewBag.TotalRecords = db.GetApplicantListSearch(Search, BatchID, dtStartDate, dtEndDate).ToList().Count;
                return View(db.GetApplicantListSearch(Search, BatchID, dtStartDate, dtEndDate).ToList().ToPagedList(pageNumber, Convert.ToInt32(pageSize)));
            }
        }

        public ActionResult Create(string From)
        {
            if (From != "Express")
            {
                if (Session["FormNo"] != null)
                {
                    string FormNo = Session["FormNo"].ToString();
                    if (!string.IsNullOrEmpty(FormNo))
                    {
                        return RedirectToAction("Edit", "Applicants", new { id = FormNo });
                    }
                }
            }
            else
            {
                Session.Remove("FormNo");
            }

            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc");
            ViewBag.CityID = new SelectList(db.Cities, "CityID", "CityName");
            ViewBag.CountryCode = new SelectList(db.Countries, "CountryCode", "CountryCode");
            ViewBag.CountryID = new SelectList(db.Countries, "CountryID", "CountryName");
            ViewBag.CurrentOccupationID = new SelectList(db.CurrentOccupations, "CurrentOccupationID", "CurrentOccupationName");
            ViewBag.GenderID = new SelectList(db.Genders, "GenderID", "GenderName");
            ViewBag.MaritalStatusID = new SelectList(db.MaritalStatus, "MaritalStatusID", "MaritalStatusName");
            ViewBag.NationalityID = new SelectList(db.Nationalities, "NationalityID", "NationalityName", 1);
            ViewBag.NationalityID2 = new SelectList(db.Nationalities.Where(n => n.NationalityID != 1), "NationalityID", "NationalityName");
            ViewBag.ProvinceID = new SelectList(db.Provinces, "ProvinceID", "ProvinceName");
            ViewBag.RelationTypeID = new SelectList(db.RelationTypes, "RelationTypeID", "RelationTypeName");
            ViewBag.DesignationID = new SelectList(db.Designations, "DesignationID", "DesignationName");
            ViewBag.ReligionID = new SelectList(db.Religions, "ReligionID", "ReligionName");
            ViewBag.SalutationID = new SelectList(db.Salutations, "SalutationID", "SalutationName");
            ViewBag.StatusID = new SelectList(db.Status, "StatusID", "StatusName");
            ViewBag.BatchProgramID = new SelectList(db.GetBatchProgramNameConcat("", 0), "ID", "Name");
            ViewBag.Domicile = new SelectList(db.Provinces.Where(p => p.CountryID == 1 && p.IsActive == "Yes"), "ProvinceID", "ProvinceName");
            ViewBag.FormNo = new SelectList(db.GetFormsNotEnteredInSystem(), "FormNo", "ApplicantText");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Applicant applicant)
        {
            FormSaleDetail fsd = db.FormSaleDetails.FirstOrDefault(a => a.FormNo == applicant.FormNo);
            if (fsd != null)
            {
                Applicant app = db.Applicants.FirstOrDefault(a => a.FormNo == applicant.FormNo);
                applicant.BatchProgramID = db.BatchPrograms.FirstOrDefault().BatchProgramID;
                if (app == null)
                {
                    string HKR = "";
                    if (applicant.Newspaper != null)
                    {
                        HKR += applicant.Newspaper;
                    }

                    if (applicant.Internet != null)
                    {
                        HKR += "," + applicant.Internet;
                    }
                    if (applicant.Other != null)
                    {
                        HKR += "," + applicant.Other;
                    }

                    if (applicant.FriendsRelatives != null)
                    {
                        HKR += "," + applicant.FriendsRelatives;
                    }
                    if (applicant.StudentAtIqra != null)
                    {
                        HKR += "," + applicant.StudentAtIqra;
                    }

                    applicant.HowToKnowAbout = HKR.Trim(',');

                    applicant.CreatedOn = DateTime.Now;
                    applicant.CreatedBy = Convert.ToInt32(Session["emp_id"]);
                    try
                    {
                        if (applicant.ProfilePicture != null)
                        {
                            applicant.Picture = string.Concat("~/ProfilePics/", applicant.FormNo, "_", applicant.ProfilePicture.FileName);
                            applicant.ProfilePicture.SaveAs(Server.MapPath(applicant.Picture));
                            Session["ProfilePicture"] = applicant.ProfilePicture;
                            Session["Picture"] = applicant.Picture;
                        }
                        else if (Session["ProfilePicture"] != null)
                        {
                            applicant.ProfilePicture = (HttpPostedFileBase)Session["ProfilePicture"];
                            applicant.Picture = string.Concat("~/ProfilePics/", applicant.FormNo, "_", applicant.ProfilePicture.FileName);
                        }
                        else
                        {
                            count++;
                            ErrorMessage += count + "-Profile picture is required.<br />";
                        }
                    }
                    catch (Exception)
                    {
                        ModelState.AddModelError(string.Empty, "Profile picture is required.");
                        count++;
                        ErrorMessage += count + "-Profile picture is required.<br />";
                    }

                    try
                    {
                        if (applicant.EntryTestID == null || applicant.EntryTestID == 0)
                        {
                            applicant.EntryTestID = db.EntryTests.FirstOrDefault(et => et.IsActive == "Yes") == null ? 1 : db.EntryTests.FirstOrDefault(et => et.IsActive == "Yes").EntryTestID;
                        }
                        db.Applicants.Add(applicant);

                        try
                        {
                            if (string.IsNullOrEmpty(ErrorMessage))
                            {
                                db.SaveChanges();
                                ViewBag.scripCall = "EraseAllRecords();";
                                ViewBag.MessageType = "success";
                                ViewBag.Message = "Data has been saved successfully.";
                                Session["FormNo"] = applicant.FormNo;
                                //Add Applicant Login
                                Login login = new Login();
                                login.CNIC = applicant.ACNIC;
                                login.Email = applicant.Email;
                                login.UserName = applicant.Email;
                                login.Password = "123";
                                login.MobileNumber = applicant.CellNo;
                                Session.Remove("Picture");
                                Session.Remove("ProfilePicture");
                                return RedirectToAction("ChooseProgram", "ApplicantQualifications");
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
                            ViewBag.Message = ex.InnerException.ToString();
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
                }
                else
                {
                    ViewBag.MessageType = "error";
                    ViewBag.Message = "This Form # already exists.";
                }
            }
            else
            {
                ViewBag.MessageType = "error";
                ViewBag.Message = "This Form # did not received yet.";
            }

            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc", applicant.IsActive);
            ViewBag.CityID = new SelectList(db.Cities, "CityID", "CityName", applicant.CityID);
            ViewBag.CountryCode = new SelectList(db.Countries, "CountryCode", "CountryCode");
            ViewBag.CountryID = new SelectList(db.Countries, "CountryID", "CountryName", applicant.CountryID);
            ViewBag.CurrentOccupationID = new SelectList(db.CurrentOccupations, "CurrentOccupationID", "CurrentOccupationName", applicant.CurrentOccupationID);
            ViewBag.GenderID = new SelectList(db.Genders, "GenderID", "GenderName", applicant.GenderID);
            ViewBag.MaritalStatusID = new SelectList(db.MaritalStatus, "MaritalStatusID", "MaritalStatusName", applicant.MaritalStatusID);
            ViewBag.NationalityID = new SelectList(db.Nationalities, "NationalityID", "NationalityName", applicant.NationalityID);
            ViewBag.NationalityID2 = new SelectList(db.Nationalities.Where(n => n.NationalityID != 1), "NationalityID", "NationalityName", applicant.NationalityID2);
            ViewBag.ProvinceID = new SelectList(db.Provinces, "ProvinceID", "ProvinceName", applicant.ProvinceID);
            ViewBag.RelationTypeID = new SelectList(db.RelationTypes, "RelationTypeID", "RelationTypeName", applicant.RelationTypeID);
            ViewBag.DesignationID = new SelectList(db.Designations, "DesignationID", "DesignationName");
            ViewBag.ReligionID = new SelectList(db.Religions, "ReligionID", "ReligionName", applicant.ReligionID);
            ViewBag.SalutationID = new SelectList(db.Salutations, "SalutationID", "SalutationName", applicant.SalutationID);
            ViewBag.StatusID = new SelectList(db.Status, "StatusID", "StatusName", applicant.StatusID);
            ViewBag.BatchProgramID = new SelectList(db.GetBatchProgramNameConcat("", 0), "ID", "Name", applicant.BatchProgramID);
            ViewBag.Domicile = new SelectList(db.Provinces.Where(p => p.CountryID == 1 && p.IsActive == "Yes"), "ProvinceID", "ProvinceName", applicant.Domicile);
            ViewBag.hdnProvinceID = applicant.Domicile;
            ViewBag.hdnApplicantDistrictName = applicant.ApplicantDistrictName;
            ViewBag.FormNo = new SelectList(db.GetFormsNotEnteredInSystem(), "FormNo", "ApplicantText");
            ViewBag.hdnCityID = applicant.CityID;

            return View(applicant);
        }

        public ActionResult Edit(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                id = Session["FormNo"].ToString();
            }
            else
            {
                Session["FormNo"] = id;
            }

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
            ViewBag.CountryCode = new SelectList(db.Countries, "CountryCode", "CountryCode");
            ViewBag.CountryID = new SelectList(db.Countries, "CountryID", "CountryName", applicant.CountryID);
            ViewBag.CurrentOccupationID = new SelectList(db.CurrentOccupations, "CurrentOccupationID", "CurrentOccupationName", applicant.CurrentOccupationID);
            ViewBag.GenderID = new SelectList(db.Genders, "GenderID", "GenderName", applicant.GenderID);
            ViewBag.MaritalStatusID = new SelectList(db.MaritalStatus, "MaritalStatusID", "MaritalStatusName", applicant.MaritalStatusID);
            ViewBag.NationalityID = new SelectList(db.Nationalities, "NationalityID", "NationalityName", applicant.NationalityID);
            ViewBag.NationalityID2 = new SelectList(db.Nationalities.Where(n => n.NationalityID != 1), "NationalityID", "NationalityName", applicant.NationalityID2);
            ViewBag.ProvinceID = new SelectList(db.Provinces, "ProvinceID", "ProvinceName", applicant.ProvinceID);
            ViewBag.RelationTypeID = new SelectList(db.RelationTypes, "RelationTypeID", "RelationTypeName", applicant.RelationTypeID);
            ViewBag.ReligionID = new SelectList(db.Religions, "ReligionID", "ReligionName", applicant.ReligionID);
            ViewBag.SalutationID = new SelectList(db.Salutations, "SalutationID", "SalutationName", applicant.SalutationID);
            ViewBag.StatusID = new SelectList(db.Status, "StatusID", "StatusName", applicant.StatusID);
            ViewBag.BatchProgramID = new SelectList(db.GetBatchProgramNameConcat(applicant.FormNo, 6), "ID", "Name", applicant.BatchProgramID);
            ViewBag.Domicile = new SelectList(db.Provinces.Where(p => p.CountryID == 1 && p.IsActive == "Yes"), "ProvinceID", "ProvinceName", applicant.Domicile);
            ViewBag.FormNo = applicant.FormNo;
            ViewBag.hdnProvinceID = applicant.Domicile;
            ViewBag.hdnApplicantDistrictName = applicant.ApplicantDistrictName;
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
                    applicant.Picture = string.Concat("~/ProfilePics/", applicant.FormNo, "_", applicant.ProfilePicture.FileName);
                    applicant.ProfilePicture.SaveAs(Server.MapPath(applicant.Picture));
                    Session["ProfilePicture"] = applicant.ProfilePicture;
                    Session["Picture"] = applicant.Picture;
                }
                else if (Session["ProfilePicture"] != null)
                {
                    applicant.ProfilePicture = (HttpPostedFileBase)Session["ProfilePicture"];
                    applicant.Picture = string.Concat("~/ProfilePics/", applicant.FormNo, "_", applicant.ProfilePicture.FileName);
                }
            }
            catch (Exception)
            {
                ModelState.AddModelError(string.Empty, "Profile picture is not saved.");
                count++;
                ErrorMessage += count + "-Profile picture is not saved.<br />";
            }

            try
            {
                db.Entry(applicant).State = EntityState.Modified;
                applicant.ModifiedOn = DateTime.Now;
                applicant.ModifiedBy = Convert.ToInt32(Session["emp_id"]);

                Applicant ap = db.Applicants.FirstOrDefault(a => a.FormNo == applicant.FormNo);
                if (ap != null)
                {
                    applicant.StatusID = ap.StatusID;
                    applicant.ApplicantStatus = ap.ApplicantStatus;
                    applicant.DisciplinaryIssue = ap.DisciplinaryIssue;
                }

                try
                {
                    string HKR = "";
                    if (applicant.Newspaper != null)
                    {
                        HKR += applicant.Newspaper;
                    }

                    if (applicant.Internet != null)
                    {
                        HKR += "," + applicant.Internet;
                    }
                    if (applicant.Other != null)
                    {
                        HKR += "," + applicant.Other;
                    }

                    if (applicant.FriendsRelatives != null)
                    {
                        HKR += "," + applicant.FriendsRelatives;
                    }
                    if (applicant.StudentAtIqra != null)
                    {
                        HKR += "," + applicant.StudentAtIqra;
                    }

                    applicant.HowToKnowAbout = HKR.Trim(',');

                    if (applicant.EntryTestID == null || applicant.EntryTestID == 0)
                    {
                        applicant.EntryTestID = db.EntryTests.FirstOrDefault(et => et.IsActive == "Yes") == null ? 1 : db.EntryTests.FirstOrDefault(et => et.IsActive == "Yes").EntryTestID;
                    }

                    db.SaveChanges();
                    ViewBag.MessageType = "success";
                    ViewBag.Message = "Data has been saved successfully.";
                    Session.Remove("Picture");
                    Session.Remove("ProfilePicture");
                    return RedirectToAction("Edit", "Applicants", new { id = ap.FormNo });
                }
                catch (DbUpdateException ex)
                {
                    ViewBag.MessageType = "error";
                    ViewBag.Message = ex.InnerException;
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
            ViewBag.CountryCode = new SelectList(db.Countries, "CountryCode", "CountryCode");
            ViewBag.CountryID = new SelectList(db.Countries, "CountryID", "CountryName", applicant.CountryID);
            ViewBag.CurrentOccupationID = new SelectList(db.CurrentOccupations, "CurrentOccupationID", "CurrentOccupationName", applicant.CurrentOccupationID);
            ViewBag.GenderID = new SelectList(db.Genders, "GenderID", "GenderName", applicant.GenderID);
            ViewBag.MaritalStatusID = new SelectList(db.MaritalStatus, "MaritalStatusID", "MaritalStatusName", applicant.MaritalStatusID);
            ViewBag.NationalityID = new SelectList(db.Nationalities, "NationalityID", "NationalityName", applicant.NationalityID);
            ViewBag.NationalityID2 = new SelectList(db.Nationalities.Where(n => n.NationalityID != 1), "NationalityID", "NationalityName", applicant.NationalityID2);
            ViewBag.ProvinceID = new SelectList(db.Provinces, "ProvinceID", "ProvinceName", applicant.ProvinceID);
            ViewBag.RelationTypeID = new SelectList(db.RelationTypes, "RelationTypeID", "RelationTypeName", applicant.RelationTypeID);
            ViewBag.ReligionID = new SelectList(db.Religions, "ReligionID", "ReligionName", applicant.ReligionID);
            ViewBag.SalutationID = new SelectList(db.Salutations, "SalutationID", "SalutationName", applicant.SalutationID);
            ViewBag.StatusID = new SelectList(db.Status, "StatusID", "StatusName", applicant.StatusID);
            ViewBag.BatchProgramID = new SelectList(db.GetBatchProgramNameConcat(applicant.FormNo, 6), "ID", "Name", applicant.BatchProgramID);
            ViewBag.Domicile = new SelectList(db.Provinces.Where(p => p.CountryID == 1 && p.IsActive == "Yes"), "ProvinceID", "ProvinceName", applicant.Domicile);
            ViewBag.FormNo = applicant.FormNo;
            ViewBag.hdnProvinceID = applicant.ProvinceID;
            ViewBag.hdnCityID = applicant.CityID;
            ViewBag.hdnProvinceID = applicant.Domicile;
            ViewBag.hdnApplicantDistrictName = applicant.ApplicantDistrictName;
            return View(applicant);
        }

        public ActionResult Details(string id)
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
            ViewBag.CountryCode = new SelectList(db.Countries, "CountryCode", "CountryCode");
            ViewBag.CountryID = new SelectList(db.Countries, "CountryID", "CountryName", applicant.CountryID);
            ViewBag.CurrentOccupationID = new SelectList(db.CurrentOccupations, "CurrentOccupationID", "CurrentOccupationName", applicant.CurrentOccupationID);
            ViewBag.GenderID = new SelectList(db.Genders, "GenderID", "GenderName", applicant.GenderID);
            ViewBag.MaritalStatusID = new SelectList(db.MaritalStatus, "MaritalStatusID", "MaritalStatusName", applicant.MaritalStatusID);
            ViewBag.NationalityID = new SelectList(db.Nationalities, "NationalityID", "NationalityName", applicant.NationalityID);
            ViewBag.NationalityID2 = new SelectList(db.Nationalities.Where(n => n.NationalityID != 1), "NationalityID", "NationalityName", applicant.NationalityID2);
            ViewBag.ProvinceID = new SelectList(db.Provinces, "ProvinceID", "ProvinceName", applicant.ProvinceID);
            ViewBag.RelationTypeID = new SelectList(db.RelationTypes, "RelationTypeID", "RelationTypeName", applicant.RelationTypeID);
            ViewBag.ReligionID = new SelectList(db.Religions, "ReligionID", "ReligionName", applicant.ReligionID);
            ViewBag.SalutationID = new SelectList(db.Salutations, "SalutationID", "SalutationName", applicant.SalutationID);
            ViewBag.StatusID = new SelectList(db.Status, "StatusID", "StatusName", applicant.StatusID);
            ViewBag.BatchProgramID = new SelectList(db.GetBatchProgramNameConcat(applicant.FormNo, 6), "ID", "Name", applicant.BatchProgramID);
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
            //List<ApplyForProgram> afpList = db.ApplyForPrograms.ToList().Where(a => a.FormNo == applicant.FormNo).ToList();
            //db.ApplyForPrograms.RemoveRange(afpList);
            //List<ApplicantQualification> aqList = db.ApplicantQualifications.ToList().Where(a => a.FormNo == applicant.FormNo).ToList();
            //db.ApplicantQualifications.RemoveRange(aqList);
            //db.Applicants.Remove(applicant);

            if (applicant == null)
            {
                return HttpNotFound();
            }

            try
            {
                db.Entry(applicant).State = EntityState.Modified;
                applicant.ModifiedOn = DateTime.Now;
                applicant.ModifiedBy = Convert.ToInt32(Session["emp_id"]);
                applicant.IsActive = "Deleted";
                db.SaveChanges();
            }
            catch (DbUpdateException ex)
            {
                ViewBag.MessageType = "error";
                ViewBag.Message = ex.Message;
            }
            return RedirectToAction("Index");
        }

        public ActionResult ShowAdmitCardToApplicant(string id)
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

            try
            {
                db.Entry(applicant).State = EntityState.Modified;
                applicant.ModifiedOn = DateTime.Now;
                applicant.ModifiedBy = Convert.ToInt32(Session["emp_id"]);

                Status CurrentStatus = db.Status.FirstOrDefault(s => s.StatusID == applicant.StatusID);
                Status status = db.Status.FirstOrDefault(s => s.StatusName == "Show Admit Card");
                if (CurrentStatus.StatusName == "Submitted")
                {
                    if (status != null)
                    {
                        applicant.StatusID = status.StatusID;
                    }
                }

                db.SaveChanges();
            }
            catch (DbUpdateException ex)
            {
                ViewBag.MessageType = "error";
                ViewBag.Message = ex.Message;
            }
            return RedirectToAction("Index", "Applicants");
        }

        public ActionResult ApplicantStatus()
        {
            if (Session["FormNo"] == null)
            {
                TempData["messageType"] = "error";
                TempData["message"] = "Please select applicant from Applicants list.";
                return RedirectToAction("Create", "Applicants");
            }

            int EmpID = Convert.ToInt32(Session["emp_id"]);
            string FormNo = Session["FormNo"].ToString();

            ViewData["error"] = TempData["error"];
            ViewBag.FormNo = FormNo;

            if (!string.IsNullOrEmpty(FormNo))
            {
                Applicant appp = db.Applicants.FirstOrDefault(a => a.FormNo == FormNo);
                ViewBag.applicantStatus = appp.ApplicantStatus;
                if (appp == null)
                {
                    TempData["messageType"] = "error";
                    TempData["message"] = "Please fill Personal Detail.";
                    return RedirectToAction("Create", "Applicants");
                }

                ApplyForProgram appap = db.ApplyForPrograms.FirstOrDefault(a => a.FormNo == FormNo);
                if (appap == null)
                {
                    TempData["messageType"] = "error";
                    TempData["message"] = "Please Choose Programs.";
                    return RedirectToAction("ChooseProgram", "ApplicantQualifications");
                }

                ApplicantQualification appq = db.ApplicantQualifications.FirstOrDefault(a => a.FormNo == FormNo);
                if (appq == null)
                {
                    TempData["messageType"] = "error";
                    TempData["message"] = "Please fill Academic History.";
                    return RedirectToAction("Index", "ApplicantQualifications");
                }

                ViewBag.ResultAwaitingOf = new SelectList(db.GetApplicantQualifications("", 4), "DegreeName", "DegreeName", appp.ResultAwaitingOf);
                ViewBag.ResultAwaitingBoardUniversity = new SelectList(db.Institutes, "InstituteName", "InstituteName", appp.ResultAwaitingBoardUniversity);
                return View(appp);
            }
            else
            {
                TempData["messageType"] = "error";
                TempData["message"] = "Please fill Personal Detail.";
                return RedirectToAction("Create", "Applicants");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ApplicantStatus(Applicant applicant)
        {
            try
            {
                if (string.IsNullOrEmpty(ErrorMessage))
                {
                    applicant.FormNo = Session["FormNo"].ToString();
                    applicant.ModifiedBy = Convert.ToInt32(Session["emp_id"]);
                    db.UpdateApplicantFields(applicant.ExCampus, applicant.ExProgram, applicant.ExRegistrationNo, applicant.ExReasonForLeaving, applicant.TransferInstituteAttendend, applicant.TransferProgram, applicant.TransferCGPA, applicant.TransferCreditHoursCompleted, applicant.TransferReasonForLeaving, applicant.ResultAwaitingOf, applicant.ResultAwaitingRollNo, applicant.ResultAwaitingBoardUniversity, applicant.ResultAwaitingYear, applicant.ApplicantStatus, applicant.ModifiedBy, applicant.DisciplinaryIssue, applicant.FormNo);
                    ViewBag.MessageType = "success";
                    ViewBag.Message = "Data has been saved successfully.";
                    return RedirectToAction("Declaration", "ApplicantQualifications");
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
            ViewBag.ResultAwaitingOf = new SelectList(db.GetApplicantQualifications("", 4), "DegreeName", "DegreeName", applicant.ResultAwaitingOf);
            ViewBag.ResultAwaitingBoardUniversity = new SelectList(db.Institutes, "InstituteName", "InstituteName", applicant.ResultAwaitingBoardUniversity);
            return View(applicant);
        }

        public ActionResult ApplicantListForAttendance()
        {
            ViewBag.BatchID = new SelectList(db.Batches, "BatchID", "BatchName");
            ViewBag.BatchProgramID = new SelectList(db.GetBatchProgramNameConcat("", 0).ToList(), "ID", "Name");
            ViewBag.DegreeID = new SelectList(db.Degrees.Where(d => d.IsActive == "Yes" && arrDegreeID.Contains(d.DegreeID)).ToList(), "DegreeID", "DegreeName");
            ViewBag.EntryTestID = new SelectList(db.EntryTests.Where(et => et.IsActive == "Yes").ToList(), "EntryTestID", "EntryTestName");
            ViewBag.hdnBatchID = 0;
            ViewBag.hdnBatchProgramID = 0;
            ViewBag.hdnDegreeID = 0;
            ViewBag.hdnEntryTestID = 0;
            return View();
        }

        [HttpPost]
        public ActionResult ApplicantListForAttendance(int? BatchID, int? BatchProgramID, int? DegreeID, int? EntryTestID)
        {
            ViewBag.BatchID = new SelectList(db.Batches, "BatchID", "BatchName");
            ViewBag.BatchProgramID = new SelectList(db.GetBatchProgramNameConcat("", 0).ToList(), "ID", "Name");
            ViewBag.DegreeID = new SelectList(db.Degrees.Where(d => d.IsActive == "Yes" && arrDegreeID.Contains(d.DegreeID)).ToList(), "DegreeID", "DegreeName");
            ViewBag.EntryTestID = new SelectList(db.EntryTests.Where(et => et.IsActive == "Yes").ToList(), "EntryTestID", "EntryTestName");
            if (BatchProgramID == 0)
            {
                ViewBag.MessageType = "error";
                ViewBag.Message = "Please select program.";
            }

            ViewBag.hdnBatchID = BatchID;
            ViewBag.hdnBatchProgramID = BatchProgramID;
            ViewBag.hdnDegreeID = DegreeID;
            ViewBag.hdnEntryTestID = EntryTestID;
            return View();
        }

        [HttpPost]
        public ActionResult ApplicantListForAttendanceSave(FormCollection fc)
        {
            Applicant applicant = new Applicant();
            int hdnCount = Convert.ToInt32(fc["hdnCount"]);
            int TestInterviewID = Convert.ToInt32(fc["hdnEntryTestID"]);
            int DegreeID = Convert.ToInt32(fc["hdnDegreeID"]);
            string FormNo = "";
            string PresentAbsent = "";

            if (hdnCount > 0)
            {
                for (int i = 1; i <= hdnCount; i++)
                {
                    FormNo = Convert.ToString(fc["FormNo_" + i]);
                    applicant = db.Applicants.FirstOrDefault(x => x.FormNo == FormNo);
                    PresentAbsent = Convert.ToString(fc["PresentAbsent_" + i]);

                    if (PresentAbsent == "1")
                    {
                        if (DegreeID == 21)
                        {
                            if (applicant.InterviewID == null || applicant.InterviewID == 0)
                            {
                                applicant.InterviewID = TestInterviewID;
                                db.Entry(applicant).State = EntityState.Modified;
                                db.SaveChanges();
                            }
                        }
                        else if (DegreeID == 22)
                        {
                            if (applicant.EntryTestID == null || applicant.EntryTestID == 0)
                            {
                                applicant.EntryTestID = TestInterviewID;
                                db.Entry(applicant).State = EntityState.Modified;
                                db.SaveChanges();
                            }
                        }
                        else if (DegreeID == 4020)
                        {
                            if (applicant.DrawingEntryTestID == null || applicant.DrawingEntryTestID == 0)
                            {
                                applicant.DrawingEntryTestID = TestInterviewID;
                                db.Entry(applicant).State = EntityState.Modified;
                                db.SaveChanges();
                            }
                        }
                    }
                }
            }
            TempData["messageType"] = "success";
            TempData["message"] = "Data has been saved successfully.";

            return RedirectToAction("ApplicantListForAttendance");
        }

        [HttpGet]
        public ActionResult ApplicantListSearch_ToApprove(int? page, int? pageSize, int? BatchID)
        {
            BatchID = _BatchID;
            if (pageSize == null)
            {
                pageSize = 10;
            }
            ViewBag.pageSize = pageSize;
            int pageNumber = (page ?? 1);

            ViewBag.BatchIDAssigned = BatchID;
            ViewBag.BatchID = new SelectList(db.GetBatchProgramNameConcat("", 10), "ID", "Name", BatchID);

            ViewBag.TotalRecords = db.GetApplicantListSearch_ToApprove("", BatchID, 4).ToList().Count;
            return View(db.GetApplicantListSearch_ToApprove("", BatchID, 4).ToList().ToPagedList(pageNumber, Convert.ToInt32(pageSize)));
        }

        [HttpPost]
        public ActionResult ApplicantListSearch_ToApprove(string Search, int BatchID, int? page, int? pageSize)
        {
            if (pageSize == null)
            {
                pageSize = 10;
            }
            ViewBag.pageSize = pageSize;
            int pageNumber = (page ?? 1);

            ViewBag.BatchID = new SelectList(db.GetBatchProgramNameConcat("", 10), "ID", "Name", BatchID);
            ViewBag.BatchIDAssigned = BatchID;
            ViewBag.TotalRecords = db.GetApplicantListSearch_ToApprove(Search, BatchID, 4).ToList().Count;
            return View(db.GetApplicantListSearch_ToApprove(Search, BatchID, 4).ToList().ToPagedList(pageNumber, Convert.ToInt32(pageSize)));
        }

        [HttpGet]
        public ActionResult ApproveApplicant(string id, int BatchID, int? page, int? pageSize)
        {
            int pageNumber = (page ?? 1);
            if (string.IsNullOrEmpty(id))
            {
                return HttpNotFound();
            }

            db.UpdateStatusByFormNo(1025, id, Convert.ToInt32(Session["emp_id"]), 1);

            return RedirectToAction("ApplicantListSearch_ToApprove", new { page = pageNumber, pageSize = pageSize, BatchID = BatchID });
        }

        public JsonResult GetFormSaleDetail()
        {
            List<FormSaleDetail> lstFS = new List<FormSaleDetail>();
            string pDate = "";
            string cDate = "";
            string mDate = "";
            string FormNo = Request.QueryString["FormNo"];

            lstFS = db.FormSaleDetails.Where(a => a.FormNo == FormNo && a.IsReceived == "Yes").ToList();
            if (lstFS.Count > 0)
            {
                pDate = (lstFS[0].PurchaseDate == null) ? "" : lstFS[0].PurchaseDate;
                cDate = (lstFS[0].CreatedOn == null) ? "" : lstFS[0].CreatedOn.Value.ToShortDateString();
                mDate = (lstFS[0].ModifiedOn == null) ? "" : lstFS[0].ModifiedOn.Value.ToShortDateString();
            }
            else
            {
                ViewBag.MessageType = "error";
                ViewBag.Message = "No result found.";
                return new JsonResult { Data = "No result found.", JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }

            var fs = lstFS.Select(s => new
            {
                FormID = s.FormID,
                FormTypeID = s.FormTypeID,
                FormPrice = s.FormPrice,
                FormDescription = s.FormDescription,
                DepositTypeID = s.DepositTypeID,
                BatchProgramID = s.BatchProgramID,
                FirstName = s.FirstName,
                LastName = s.LastName,
                FatherName = s.FatherName,
                PhoneNo = s.PhoneNo,
                CNIC = s.CNIC,
                DepositSlipNo = s.DepositSlipNo,
                PurchaseDate = pDate,
                AccountID = s.AccountID,
                CreatedBy = s.CreatedBy,
                CreatedOn = cDate,
                IsActive = s.IsActive,
                ModifiedOn = mDate,
                ModifiedBy = s.ModifiedBy
            });

            string result = JsonConvert.SerializeObject(fs, Formatting.Indented);
            return new JsonResult { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult GetProvinceList(string CountryID)
        {
            List<Province> lstProvince = new List<Province>();
            int cId = Convert.ToInt32(CountryID);

            lstProvince = db.Provinces.Where(p => p.CountryID == cId).ToList();
            var Provinces = lstProvince.Select(S => new
            {
                ProvinceID = S.ProvinceID,
                ProvinceName = S.ProvinceName,
            });
            string result = JsonConvert.SerializeObject(Provinces, Formatting.Indented);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetCitiesList(string ProvinceID)
        {
            List<City> lstCity = new List<City>();
            int pId = Convert.ToInt32(ProvinceID);

            lstCity = db.Cities.Where(p => p.ProvinceID == pId).ToList();
            var Cities = lstCity.Select(S => new
            {
                CityID = S.CityID,
                CityName = S.CityName,
            });
            string result = JsonConvert.SerializeObject(Cities, Formatting.Indented);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetDistrictList(string ProvinceID)
        {
            List<District> lstDistrict = new List<District>();
            int pId = Convert.ToInt32(ProvinceID);

            lstDistrict = db.Districts.Where(p => p.ProvinceID == pId).ToList();
            var Districts = lstDistrict.Select(S => new
            {
                DistrictID = S.DistrictID,
                DistrictName = S.DistrictName,
            });
            string result = JsonConvert.SerializeObject(Districts, Formatting.Indented);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetPrograms_by_FacultyLevelBatch(string FacultyID, string BatchID)
        {
            List<GetPrograms_by_FacultyLevelBatch_Result> lstPrograms = new List<GetPrograms_by_FacultyLevelBatch_Result>();

            lstPrograms = db.GetPrograms_by_FacultyLevelBatch(Convert.ToInt32(FacultyID), 0, Convert.ToInt32(BatchID), 0).ToList();
            var programs = lstPrograms.Select(p => new
            {
                BatchProgramID = p.BatchProgramID,
                ProgramName = p.ProgramName
            });
            string result = JsonConvert.SerializeObject(programs, Formatting.Indented);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetEntryTestsByDegreeID(string DegreeID)
        {
            List<EntryTest> lstEntryTests = new List<EntryTest>();
            int dID = Convert.ToInt32(DegreeID);

            lstEntryTests = db.EntryTests.Where(d => d.DegreeID == dID).ToList();
            var EntryTests = lstEntryTests.Select(et => new
            {
                DegreeID = et.DegreeID,
                EntryTestID = et.EntryTestID,
                EntryTestName = et.EntryTestName
            });
            string result = JsonConvert.SerializeObject(EntryTests, Formatting.Indented);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        protected bool MatchFatherNameWithGuardianName(string FatherCellNo, string GuardianCellNo)
        {
            bool flag = true;
            if(FatherCellNo.Trim() == GuardianCellNo.Trim())
            {
                flag = false;
            }
            return flag;
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