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
using Newtonsoft.Json;
using CampusManagement.App_Code;

namespace CampusManagement.Controllers
{
    [Authorize(Roles = "Applicant")]
    public class OnlineApplyController : Controller
    {
        private ModelCMSNewContainer db = new ModelCMSNewContainer();
        string ErrorMessage = "";
        int count = 0;

        public ActionResult Index()
        {
            int EmpID = Convert.ToInt32(Session["emp_id"]);
            return View(db.Applicants.Where(a => a.CreatedBy == EmpID).ToList());
        }

        public ActionResult Create()
        {
            string FormNo = "";
            int EmpID = Convert.ToInt32(Session["emp_id"]);
            FormNo = Convert.ToString(Session["FormNo"]);
            //if (string.IsNullOrEmpty(FormNo))
            //{
            //    Applicant app = db.Applicants.FirstOrDefault(a => a.FormNo == FormNo);
            //    if (app != null)
            //    {
            //        return RedirectToAction("Edit", "OnlineApply", new { id = FormNo });
            //    }
            //}
            if (!string.IsNullOrEmpty(FormNo))
            {
                Applicant app = db.Applicants.FirstOrDefault(a => a.FormNo == FormNo);
                if (app != null)
                {
                    return RedirectToAction("Edit", "OnlineApply", new { id = FormNo });
                }
            }

            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc");
            ViewBag.CityID = new SelectList(db.Cities, "CityID", "CityName");
            ViewBag.CountryID = new SelectList(db.Countries, "CountryID", "CountryName");
            ViewBag.CountryCode = new SelectList(db.Countries, "CountryCode", "CountryCode");
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

            ViewBag.FormNo = CommonFunctions.GetRandomString(100);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Applicant applicant)
        {
            applicant.CreatedOn = DateTime.Now;
            applicant.CreatedBy = Convert.ToInt32(Session["emp_id"]);
            applicant.StatusID = 1007;
            applicant.BatchProgramID = db.BatchPrograms.FirstOrDefault().BatchProgramID;
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
                    //applicant.ProfilePicture.SaveAs(Server.MapPath("~/ProfilePics") + "/" + string.Concat(applicant.FormNo, "_", applicant.ProfilePicture.FileName));
                }
                else
                {
                    count++;
                    ErrorMessage += count + "-Profile Picture is required.<br />";
                }
            }
            catch (Exception)
            {
                ModelState.AddModelError(string.Empty, "Profile Picture is required.");
                count++;
                ErrorMessage += count + "-Profile Picture is required.<br />";
            }

            //try
            //{
            //    if (applicant.ChallanImage != null)
            //    {
            //        applicant.ChallanImagePath = "~/ChallanImages/" + string.Concat(applicant.FormNo, "_", applicant.ChallanImage.FileName);
            //        applicant.ChallanImage.SaveAs(Server.MapPath("~/ChallanImages") + "/" + string.Concat(applicant.FormNo, "_", applicant.ChallanImage.FileName));
            //        Session["ChallanImage"] = applicant.ChallanImage;
            //    }
            //    else if (Session["ChallanImage"] != null)
            //    {
            //        applicant.ChallanImage = (HttpPostedFileBase)Session["ChallanImage"];
            //        applicant.ChallanImagePath = "~/ChallanImages/" + string.Concat(applicant.FormNo, "_", applicant.ChallanImage.FileName);

            //    }
            //    else
            //    {
            //        count++;
            //        ErrorMessage += count + "-Bank Challan Slip is required.<br />";
            //    }
            //}
            //catch (Exception)
            //{
            //    ModelState.AddModelError(string.Empty, "Bank Challan Slip is required.");
            //    count++;
            //    ErrorMessage += count + "-Bank Challan Slip is required.<br />";
            //}

            try
            {
                try
                {
                    if (string.IsNullOrEmpty(ErrorMessage))
                    {
                        if (applicant.EntryTestID == null || applicant.EntryTestID == 0)
                        {
                            applicant.EntryTestID = db.EntryTests.FirstOrDefault(et => et.IsActive == "Yes") == null ? 1 : db.EntryTests.FirstOrDefault(et => et.IsActive == "Yes").EntryTestID;
                        }
                        applicant.FormNo = db.GetMaxFormNo().FirstOrDefault();
                        db.Applicants.Add(applicant);
                        db.SaveChanges();
                        
                        Session.Add("FormNo", applicant.FormNo);
                        FormSaleDetail formSaleDetail = new FormSaleDetail();
                        formSaleDetail.CreatedBy = Convert.ToInt32(Session["emp_id"]);
                        formSaleDetail.ModifiedBy = 0;
                        formSaleDetail.IsReceived = "Yes";
                        formSaleDetail.ReceiveDate = DateTime.Now.ToShortDateString();
                        formSaleDetail.FormPrice = "1500";

                        formSaleDetail.FormNo = applicant.FormNo;
                        formSaleDetail.Wavier_Discount = "0";

                        db.InsertFormSaleDetail(formSaleDetail.FormID, formSaleDetail.FormNo
                            , formSaleDetail.FormTypeID, formSaleDetail.FormPrice
                            , formSaleDetail.FormDescription, formSaleDetail.DepositTypeID
                            , formSaleDetail.BatchProgramID1
                            , formSaleDetail.FatherName, formSaleDetail.PhoneNo, formSaleDetail.CNIC
                            , formSaleDetail.DepositSlipNo, formSaleDetail.PurchaseDate
                            , formSaleDetail.AccountID, formSaleDetail.CreatedBy, formSaleDetail.IsActive
                            , formSaleDetail.ModifiedBy, formSaleDetail.IsReceived, formSaleDetail.ReceiveDate
                            , formSaleDetail.FirstName, formSaleDetail.LastName, formSaleDetail.DegreeID
                            , formSaleDetail.Wavier_Discount);
                        ViewBag.MessageType = "success";
                        ViewBag.Message = "Data has been saved successfully.";

                        ViewBag.MessageType = "success";
                        ViewBag.Message = "Data has been saved successfully.";
                        ViewBag.scripCall = "EraseAllRecords();";
                        return RedirectToAction("ChooseProgram", "OnlineApplyQualifications");
                    }
                    else
                    {
                        ViewBag.MessageType = "error";
                        ViewBag.Message = ErrorMessage;
                    }
                }
                catch (DbUpdateException ex)
                {
                    if (ex.InnerException.ToString().Contains("Email"))
                    {
                        ViewBag.MessageType = "error";
                        ViewBag.Message = "Email already exists.";
                    }
                    else if (ex.InnerException.ToString().Contains("CNIC"))
                    {
                        ViewBag.MessageType = "error";
                        ViewBag.Message = "CNIC already exists.";
                    }
                    else
                    {
                        ViewBag.MessageType = "error";
                        ViewBag.Message = ex.InnerException.ToString();
                    }
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
             ViewBag.CountryID = new SelectList(db.Countries, "CountryID", "CountryName", applicant.CountryID);  ViewBag.CountryCode = new SelectList(db.Countries, "CountryCode", "CountryCode");
            ViewBag.CountryCode = new SelectList(db.Countries, "CountryCode", "CountryCode");
            ViewBag.CurrentOccupationID = new SelectList(db.CurrentOccupations, "CurrentOccupationID", "CurrentOccupationName", applicant.CurrentOccupationID);
            ViewBag.GenderID = new SelectList(db.Genders, "GenderID", "GenderName", applicant.GenderID);
            ViewBag.MaritalStatusID = new SelectList(db.MaritalStatus, "MaritalStatusID", "MaritalStatusName", applicant.MaritalStatusID);
            ViewBag.NationalityID = new SelectList(db.Nationalities, "NationalityID", "NationalityName",applicant.NationalityID);
            ViewBag.NationalityID2 = new SelectList(db.Nationalities.Where(n => n.NationalityID != 1), "NationalityID", "NationalityName", applicant.NationalityID2);
            ViewBag.ProvinceID = new SelectList(db.Provinces, "ProvinceID", "ProvinceName", applicant.ProvinceID);
            ViewBag.RelationTypeID = new SelectList(db.RelationTypes, "RelationTypeID", "RelationTypeName", applicant.RelationTypeID);
            ViewBag.DesignationID = new SelectList(db.Designations, "DesignationID", "DesignationName");
            ViewBag.ReligionID = new SelectList(db.Religions, "ReligionID", "ReligionName", applicant.ReligionID);
            ViewBag.SalutationID = new SelectList(db.Salutations, "SalutationID", "SalutationName", applicant.SalutationID);
            ViewBag.StatusID = new SelectList(db.Status, "StatusID", "StatusName", applicant.StatusID);
            ViewBag.BatchProgramID = new SelectList(db.GetBatchProgramNameConcat("", 0), "ID", "Name", applicant.BatchProgramID);
            ViewBag.Domicile = new SelectList(db.Provinces.Where(p => p.CountryID == 1 && p.IsActive == "Yes"), "ProvinceID", "ProvinceName", applicant.Domicile);
            ViewBag.FormNo = applicant.FormNo;
            ViewBag.hdnProvinceID = applicant.ProvinceID;
            ViewBag.hdnCityID = applicant.CityID;
            ViewBag.hdnApplicantDistrictName = applicant.ApplicantDistrictName;

            Session.Add("FormNo", applicant.FormNo);
            // JavaScript("EraseAllRecords()");
            // ViewBag.IsCallEraseFunction = "1";
            


            return View(applicant);
        }



        public ActionResult Edit(string id)
        {
            string FormNo = "";
            int EmpID = Convert.ToInt32(Session["emp_id"]);
            FormNo = Convert.ToString(Session["FormNo"]);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Applicant applicant = db.Applicants.Find(id);
            if (applicant == null)
            {
                return HttpNotFound();
            }


            if (string.IsNullOrEmpty(applicant.ChallanImagePath))
            {
                applicant.ChallanImagePath = "~/ChallanImages/nochallan.png";
            }

            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc", applicant.IsActive);
            ViewBag.CityID = new SelectList(db.Cities, "CityID", "CityName", applicant.CityID);
             ViewBag.CountryID = new SelectList(db.Countries, "CountryID", "CountryName", applicant.CountryID);
            ViewBag.CountryCode = new SelectList(db.Countries, "CountryCode", "CountryCode");
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
            ViewBag.BatchProgramID = new SelectList(db.GetBatchProgramNameConcat(applicant.FormNo, 6), "ID", "Name");
            ViewBag.Domicile = new SelectList(db.Provinces.Where(p => p.CountryID == 1 && p.IsActive == "Yes"), "ProvinceID", "ProvinceName", applicant.Domicile);
            ViewBag.FormNo = applicant.FormNo;

            return View(applicant);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Applicant applicant)
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

            //try
            //{
            //    if (applicant.ChallanImage != null)
            //    {
            //        applicant.ChallanImagePath = "~/ChallanImages/" + string.Concat(applicant.FormNo, "_", applicant.ChallanImage.FileName);
            //        applicant.ChallanImage.SaveAs(Server.MapPath("~/ChallanImages") + "/" + string.Concat(applicant.FormNo, "_", applicant.ChallanImage.FileName));
            //    }
            //}
            //catch (Exception)
            //{
            //    ModelState.AddModelError(string.Empty, "Bank Challan Slip is not saved.");
            //    count++;
            //    ErrorMessage += count + "-Bank Challan Slip is not saved.<br />";
            //}

            try
            {
                db.Entry(applicant).State = EntityState.Modified;
                applicant.ModifiedOn = DateTime.Now;
                applicant.ModifiedBy = Convert.ToInt32(Session["emp_id"]);

                try
                {
                    Applicant ap = db.Applicants.FirstOrDefault(a => a.FormNo == applicant.FormNo);
                    if (ap != null)
                    {
                        applicant.StatusID = ap.StatusID;
                        applicant.ApplicantStatus = ap.ApplicantStatus;
                        applicant.DisciplinaryIssue = ap.DisciplinaryIssue;
                    }

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
                    ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc", applicant.IsActive);
                    ViewBag.CityID = new SelectList(db.Cities, "CityID", "CityName", applicant.CityID);
                     ViewBag.CountryID = new SelectList(db.Countries, "CountryID", "CountryName", applicant.CountryID);
                    ViewBag.CountryCode = new SelectList(db.Countries, "CountryCode", "CountryCode");
                    ViewBag.CurrentOccupationID = new SelectList(db.CurrentOccupations, "CurrentOccupationID", "CurrentOccupationName", applicant.CurrentOccupationID);
                    ViewBag.GenderID = new SelectList(db.Genders, "GenderID", "GenderName", applicant.GenderID);
                    ViewBag.MaritalStatusID = new SelectList(db.MaritalStatus, "MaritalStatusID", "MaritalStatusName", applicant.MaritalStatusID);
                    ViewBag.NationalityID = new SelectList(db.Nationalities, "NationalityID", "NationalityName",  applicant.NationalityID);
                    ViewBag.NationalityID2 = new SelectList(db.Nationalities.Where(n => n.NationalityID != 1), "NationalityID", "NationalityName", applicant.NationalityID2);
                    ViewBag.ProvinceID = new SelectList(db.Provinces, "ProvinceID", "ProvinceName", applicant.ProvinceID);
                    ViewBag.RelationTypeID = new SelectList(db.RelationTypes, "RelationTypeID", "RelationTypeName", applicant.RelationTypeID);
                    ViewBag.DesignationID = new SelectList(db.Designations, "DesignationID", "DesignationName");
                    ViewBag.ReligionID = new SelectList(db.Religions, "ReligionID", "ReligionName", applicant.ReligionID);
                    ViewBag.SalutationID = new SelectList(db.Salutations, "SalutationID", "SalutationName", applicant.SalutationID);
                    ViewBag.StatusID = new SelectList(db.Status, "StatusID", "StatusName", applicant.StatusID);
                    ViewBag.BatchProgramID = new SelectList(db.GetBatchProgramNameConcat(applicant.FormNo, 6), "ID", "Name");
                    ViewBag.Domicile = new SelectList(db.Provinces.Where(p => p.CountryID == 1 && p.IsActive == "Yes"), "ProvinceID", "ProvinceName", applicant.Domicile);
                    ViewBag.FormNo = applicant.FormNo;
                    ViewBag.hdnProvinceID = applicant.ProvinceID;
                    ViewBag.hdnCityID = applicant.CityID;
                    return View(applicant);

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
            ViewBag.CountryCode = new SelectList(db.Countries, "CountryCode", "CountryCode");
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
            ViewBag.BatchProgramID = new SelectList(db.GetBatchProgramNameConcat(applicant.FormNo, 6), "ID", "Name");
            ViewBag.Domicile = new SelectList(db.Provinces.Where(p => p.CountryID == 1 && p.IsActive == "Yes"), "ProvinceID", "ProvinceName", applicant.Domicile);
            ViewBag.FormNo = applicant.FormNo;
            ViewBag.hdnProvinceID = applicant.ProvinceID;
            ViewBag.hdnCityID = applicant.CityID;
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

            if (string.IsNullOrEmpty(applicant.ChallanImagePath))
            {
                applicant.ChallanImagePath = "~/ChallanImages/nochallan.png";
            }

            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc", applicant.IsActive);
            ViewBag.CityID = new SelectList(db.Cities, "CityID", "CityName", applicant.CityID);
            ViewBag.CountryID = new SelectList(db.Countries, "CountryID", "CountryName", applicant.CountryID);
            ViewBag.CountryCode = new SelectList(db.Countries, "CountryCode", "CountryCode");
            ViewBag.CurrentOccupationID = new SelectList(db.CurrentOccupations, "CurrentOccupationID", "CurrentOccupationName", applicant.CurrentOccupationID);
            ViewBag.GenderID = new SelectList(db.Genders, "GenderID", "GenderName", applicant.GenderID);
            ViewBag.MaritalStatusID = new SelectList(db.MaritalStatus, "MaritalStatusID", "MaritalStatusName", applicant.MaritalStatusID);
            ViewBag.NationalityID = new SelectList(db.Nationalities, "NationalityID", "NationalityName",  applicant.NationalityID);
            ViewBag.NationalityID2 = new SelectList(db.Nationalities.Where(n => n.NationalityID != 1), "NationalityID", "NationalityName", applicant.NationalityID2);
            ViewBag.ProvinceID = new SelectList(db.Provinces, "ProvinceID", "ProvinceName", applicant.ProvinceID);
            ViewBag.RelationTypeID = new SelectList(db.RelationTypes, "RelationTypeID", "RelationTypeName", applicant.RelationTypeID);
            ViewBag.DesignationID = new SelectList(db.Designations, "DesignationID", "DesignationName");
            ViewBag.ReligionID = new SelectList(db.Religions, "ReligionID", "ReligionName", applicant.ReligionID);
            ViewBag.SalutationID = new SelectList(db.Salutations, "SalutationID", "SalutationName", applicant.SalutationID);
            ViewBag.StatusID = new SelectList(db.Status, "StatusID", "StatusName", applicant.StatusID);
            ViewBag.BatchProgramID = new SelectList(db.GetBatchProgramNameConcat(applicant.FormNo, 6), "ID", "Name");
            ViewBag.Domicile = new SelectList(db.Provinces.Where(p => p.CountryID == 1 && p.IsActive == "Yes"), "ProvinceID", "ProvinceName", applicant.Domicile);
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

        [HttpGet]
        public ActionResult ApplicantStatus()
        {
            ViewData["error"] = TempData["error"];
            string FormNo = Session["FormNo"].ToString();
            ViewBag.FormNo = FormNo;

            Applicant appp = db.Applicants.FirstOrDefault(a => a.FormNo == FormNo);
            try
            {
                if (appp == null)
                {
                    TempData["messageType"] = "error";
                    TempData["message"] = "Please fill Personal Detail.";
                    return RedirectToAction("Create", "OnlineApply");
                }

                ApplyForProgram appap = db.ApplyForPrograms.FirstOrDefault(a => a.FormNo == FormNo);
                if (appap == null)
                {
                    TempData["messageType"] = "error";
                    TempData["message"] = "Please Choose Programs.";
                    return RedirectToAction("ChooseProgram", "OnlineApplyQualifications");
                }

                ApplicantQualification appq = db.ApplicantQualifications.FirstOrDefault(a => a.FormNo == FormNo);
                if (appq == null)
                {
                    TempData["messageType"] = "error";
                    TempData["message"] = "Please fill Academic History.";
                    return RedirectToAction("Index", "OnlineApplyQualifications");
                }
            }
            catch (Exception ex)
            {
                ViewBag.MessageType = "error";
                ViewBag.Message = ex.Message;
            }

            if (appp != null)
            {
                ViewBag.ApplicantStatus = appp.ApplicantStatus;
            }
            ViewBag.ResultAwaitingOf = new SelectList(db.GetApplicantQualifications("", 4), "DegreeName", "DegreeName", appp.ResultAwaitingOf);
            ViewBag.ResultAwaitingBoardUniversity = new SelectList(db.Institutes, "InstituteName", "InstituteName", appp.ResultAwaitingBoardUniversity);
            return View(appp);
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
                    return RedirectToAction("Declaration", "OnlineApplyQualifications");
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

        public JsonResult GetLoginUserInfo()
        {
            List<Login> lstLogin = new List<Login>();
            int EmpID = Convert.ToInt32(Session["emp_id"]);

            lstLogin = db.Logins.Where(l => l.EmpID == EmpID).ToList();
            var logins = lstLogin.Select(S => new
            {
                Email = S.Email,
                CNIC = S.CNIC,
                MobileNumber = S.MobileNumber,
                EmpID = EmpID
            });
            string result = JsonConvert.SerializeObject(logins, Formatting.Indented);
            return Json(result, JsonRequestBehavior.AllowGet);
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