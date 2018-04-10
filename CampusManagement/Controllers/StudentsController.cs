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

namespace CampusManagement.Controllers
{
    [Authorize(Roles = "Account Officer,Accounts Officer,Admin Assistant,Admin Officer,Admin.Assistant,Assist. Account Officer,Assist.Technician,Import Manager,Manager Servive & Support,Office Manager,Officer QMS,RSM - Center 2,RSM - South,Sales & Service Executive,Sales Executive,Sales Manager,Sales Representative,Sr.Accounts Officer,Sr.Associate Engineer,Sr.Sales Executive,Sr.Sales Representative,Store Assistant,Store Incharge,Technician")]
    public class StudentsController : Controller
    {
        private ModelCMSContainer db = new ModelCMSContainer();
        string ErrorMessage = "";
        string MessageType = "";
        int count = 0;

        public ActionResult Index()
        {
            return View(db.Students.OrderByDescending(a => a.StudentID));
        }

        [HttpPost]
        public ActionResult Index(string Search)
        {
            return View(db.Students.Where(x => x.FirstName.Contains(Search) || x.Email.Contains(Search) || x.LastName.Contains(Search)).OrderByDescending(a => a.StudentID));
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
            ViewBag.BatchProgramID = new SelectList(db.GetBatchProgramNameConcat("", 0), "ID", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Student student)
        {
            try
            {
                Applicant applicant = db.Applicants.FirstOrDefault(a => a.FormNo == student.FormNo);
                if (applicant != null)
                {
                    Student st = db.Students.FirstOrDefault(a => a.FormNo == student.FormNo);

                    if (st != null)
                    {
                        ModelState.AddModelError(string.Empty, "Form # is already exists.");
                        count++;
                        ErrorMessage += count + "-Form # is already exists.";
                        MessageType = "error";
                    }
                    else
                    {
                        try
                        {
                            if (student.ProfilePicture != null)
                            {
                                student.Picture = "~/ProfilePics/" + string.Concat(student.FormNo, "_", student.ProfilePicture.FileName);
                                student.ProfilePicture.SaveAs(Server.MapPath("~/ProfilePics") + "/" + string.Concat(student.FormNo, "_", student.ProfilePicture.FileName));
                            }
                        }
                        catch (Exception)
                        {
                            ModelState.AddModelError(string.Empty, "Profile pictuer is required.");
                            count++;
                            ErrorMessage += count + "-Profile pictuer is required.<br />";
                            MessageType = "error";
                        }

                        try
                        {
                            student.BatchProgramID = applicant.BatchProgramID;
                            student.CreatedOn = DateTime.Now;
                            student.CreatedBy = Convert.ToInt32(Session["emp_id"]);
                            db.Students.Add(student);
                            db.SaveChanges();
                            ViewBag.MessageType = "success";
                            ViewBag.Message = "Data has been saved successfully.";
                            return RedirectToAction("Index", "Students");
                        }
                        catch (DbUpdateException ex)
                        {
                            MessageType = "error";
                            count++;
                            ErrorMessage += count + "-" + ex.Message;
                            ModelState.AddModelError(string.Empty, ex.Message);
                        }
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "No applicant found against Form #");
                    count++;
                    ErrorMessage += count + "-No applicant found against Form # : " + student.FormNo;
                    MessageType = "error";
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
                MessageType = "error";
            }

            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc", student.IsActive);
            ViewBag.CityID = new SelectList(db.Cities, "CityID", "CityName", student.CityID);
            ViewBag.CountryID = new SelectList(db.Countries, "CountryID", "CountryName", student.CountryID);
            ViewBag.CurrentOccupationID = new SelectList(db.CurrentOccupations, "CurrentOccupationID", "CurrentOccupationName", student.CurrentOccupationID);
            ViewBag.GenderID = new SelectList(db.Genders, "GenderID", "GenderName", student.GenderID);
            ViewBag.MaritalStatusID = new SelectList(db.MaritalStatus, "MaritalStatusID", "MaritalStatusName", student.MaritalStatusID);
            ViewBag.NationalityID = new SelectList(db.Nationalities, "NationalityID", "NationalityName", student.NationalityID);
            ViewBag.ProvinceID = new SelectList(db.Provinces, "ProvinceID", "ProvinceName", student.ProvinceID);
            ViewBag.RelationTypeID = new SelectList(db.RelationTypes, "RelationTypeID", "RelationTypeName", student.RelationTypeID);
            ViewBag.ReligionID = new SelectList(db.Religions, "ReligionID", "ReligionName", student.ReligionID);
            ViewBag.SalutationID = new SelectList(db.Salutations, "SalutationID", "SalutationName", student.SalutationID);
            ViewBag.StatusID = new SelectList(db.Status, "StatusID", "StatusName", student.StatusID);
            ViewBag.BatchProgramID = new SelectList(db.GetBatchProgramNameConcat("", 0), "ID", "Name", student.BatchProgramID);

            if (!string.IsNullOrEmpty(ErrorMessage) && MessageType == "error")
            {
                ViewBag.MessageType = MessageType;
                ViewBag.Message = ErrorMessage;
            }
            return View(student);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }

            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc", student.IsActive);
            ViewBag.CityID = new SelectList(db.Cities, "CityID", "CityName", student.CityID);
            ViewBag.CountryID = new SelectList(db.Countries, "CountryID", "CountryName", student.CountryID);
            ViewBag.CurrentOccupationID = new SelectList(db.CurrentOccupations, "CurrentOccupationID", "CurrentOccupationName", student.CurrentOccupationID);
            ViewBag.GenderID = new SelectList(db.Genders, "GenderID", "GenderName", student.GenderID);
            ViewBag.MaritalStatusID = new SelectList(db.MaritalStatus, "MaritalStatusID", "MaritalStatusName", student.MaritalStatusID);
            ViewBag.NationalityID = new SelectList(db.Nationalities, "NationalityID", "NationalityName", student.NationalityID);
            ViewBag.ProvinceID = new SelectList(db.Provinces, "ProvinceID", "ProvinceName", student.ProvinceID);
            ViewBag.RelationTypeID = new SelectList(db.RelationTypes, "RelationTypeID", "RelationTypeName", student.RelationTypeID);
            ViewBag.ReligionID = new SelectList(db.Religions, "ReligionID", "ReligionName", student.ReligionID);
            ViewBag.SalutationID = new SelectList(db.Salutations, "SalutationID", "SalutationName", student.SalutationID);
            ViewBag.StatusID = new SelectList(db.Status, "StatusID", "StatusName", student.StatusID);
            ViewBag.BatchProgramID = new SelectList(db.GetBatchProgramNameConcat("", 0), "ID", "Name", student.BatchProgramID);

            return View(student);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Student student)
        {
            try
            {
                if (student.ProfilePicture != null)
                {
                    student.Picture = "~/ProfilePics/" + string.Concat(student.FormNo, "_", student.ProfilePicture.FileName);
                    student.ProfilePicture.SaveAs(Server.MapPath("~/ProfilePics") + "/" + string.Concat(student.FormNo, "_", student.ProfilePicture.FileName));
                }
            }
            catch (Exception)
            {
                ModelState.AddModelError(string.Empty, "Profile pictuer is not saved.");
                count++;
                ErrorMessage += count + "-Profile pictuer is not saved.<br />";
                MessageType = "error";
            }

            try
            {
                try
                {
                    db.Entry(student).State = EntityState.Modified;
                    student.ModifiedOn = DateTime.Now;
                    student.ModifiedBy = Convert.ToInt32(Session["emp_id"]);
                    ApplyForProgram applyForProgram = db.ApplyForPrograms.FirstOrDefault(afp => afp.FormNo == student.FormNo);
                    student.BatchProgramID = Convert.ToInt32(applyForProgram.ProgramID);
                    db.SaveChanges();
                    ViewBag.MessageType = "success";
                    ViewBag.Message = "Data has been saved successfully.";
                    return RedirectToAction("Index", "Students");
                }
                catch (DbUpdateException ex)
                {
                    MessageType = "error";
                    count++;
                    ErrorMessage += count + "-" + ex.Message;
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
                MessageType = "error";
            }

            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc", student.IsActive);
            ViewBag.CityID = new SelectList(db.Cities, "CityID", "CityName", student.CityID);
            ViewBag.CountryID = new SelectList(db.Countries, "CountryID", "CountryName", student.CountryID);
            ViewBag.CurrentOccupationID = new SelectList(db.CurrentOccupations, "CurrentOccupationID", "CurrentOccupationName", student.CurrentOccupationID);
            ViewBag.GenderID = new SelectList(db.Genders, "GenderID", "GenderName", student.GenderID);
            ViewBag.MaritalStatusID = new SelectList(db.MaritalStatus, "MaritalStatusID", "MaritalStatusName", student.MaritalStatusID);
            ViewBag.NationalityID = new SelectList(db.Nationalities, "NationalityID", "NationalityName", student.NationalityID);
            ViewBag.ProvinceID = new SelectList(db.Provinces, "ProvinceID", "ProvinceName", student.ProvinceID);
            ViewBag.RelationTypeID = new SelectList(db.RelationTypes, "RelationTypeID", "RelationTypeName", student.RelationTypeID);
            ViewBag.ReligionID = new SelectList(db.Religions, "ReligionID", "ReligionName", student.ReligionID);
            ViewBag.SalutationID = new SelectList(db.Salutations, "SalutationID", "SalutationName", student.SalutationID);
            ViewBag.StatusID = new SelectList(db.Status, "StatusID", "StatusName", student.StatusID);
            ViewBag.BatchProgramID = new SelectList(db.GetBatchProgramNameConcat("", 0), "ID", "Name", student.BatchProgramID);

            if (!string.IsNullOrEmpty(ErrorMessage) && MessageType == "error")
            {
                ViewBag.MessageType = MessageType;
                ViewBag.Message = ErrorMessage;
            }
            return View(student);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                Student student = db.Students.Find(id);
                db.Students.Remove(student);
                db.SaveChanges();
            }
            catch (DbUpdateException ex)
            {
                throw ex;
            }
            return RedirectToAction("Index");
        }

        public JsonResult GetProvinceList(string CountryID)
        {
            List<Province> lstProvince = new List<Province>();
            int cId = Convert.ToInt32(CountryID);

            lstProvince = db.Provinces.Where(p => p.CountryID == cId).ToList();
            var Provinces = lstProvince.Select(S => new {
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
            var Cities = lstCity.Select(S => new {
                CityID = S.CityID,
                CityName = S.CityName,
            });
            string result = JsonConvert.SerializeObject(Cities, Formatting.Indented);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetApplicantsList(string searchValue)
        {
            List<Applicant> lstApplicant = new List<Applicant>();

            lstApplicant = db.Applicants.Where(a => a.FirstName.Contains(searchValue)
            || a.LastName.Contains(searchValue)
            || a.FormNo.Contains(searchValue)
            || a.ACNIC.Contains(searchValue)).ToList();

            var Applicants = lstApplicant.Select(s => new {
                AddAppID = s.AddAppID,
                FirstName = s.FirstName,
                LastName = s.LastName,
                FormNo = s.FormNo,
                ACNIC = s.ACNIC
            });

            string result = JsonConvert.SerializeObject(Applicants, Formatting.Indented);
            return new JsonResult { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult GetApplicant(string FormNo)
        {
            List<Applicant> lstApplicant = new List<Applicant>();
            string expiryDate = "";
            string DOB = "";

            lstApplicant = db.Applicants.Where(a => a.FormNo == FormNo).ToList();
            if(lstApplicant.Count > 0)
            {
                expiryDate = (lstApplicant[0].PassportExpiryDate == null) ? "" : lstApplicant[0].PassportExpiryDate.Value.ToShortDateString();
                DOB = (lstApplicant[0].ApplicantDOB == null) ? "" : lstApplicant[0].ApplicantDOB.Value.ToShortDateString();
            }
            else
            {
                ViewBag.MessageType = "error";
                ViewBag.Message = "No result found.";
                return new JsonResult { Data = "No result found.", JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }

            var Applicants = lstApplicant.Select(s => new
            {
                AddAppID = s.AddAppID,
                FirstName = s.FirstName,
                LastName = s.LastName,
                FormNo = s.FormNo,
                ACNIC = s.ACNIC,
                SalutationID = s.SalutationID,
                FatherName = s.FatherName,
                ApplicantDOB = DOB,
                PlaceOfBirth = s.PlaceOfBirth,
                GenderID = s.GenderID,
                NationalityID = s.NationalityID,
                MaritalStatusID = s.MaritalStatusID,
                PassportNo = s.PassportNo,
                PassportExpiryDate = expiryDate,
                PTCLNO = s.PTCLNO,
                CellNo = s.CellNo,
                Email = s.Email,
                AlternateEmail = s.AlternateEmail,
                GuardianName = s.GuardianName,
                RelationTypeID = s.RelationTypeID,
                GuardianCNIC = s.GuardianCNIC,
                BatchProgramID = s.BatchProgramID,
                Picture = s.Picture,
                CountryID = s.CountryID,
                ProvinceID = s.ProvinceID,
                CityID = s.CityID,
                PresentAddress = s.PresentAddress,
                PermanentAddress = s.PermanentAddress,
                ReligionID = s.ReligionID,
                CurrentOccupationID = s.CurrentOccupationID,
                StatusID = s.StatusID,
                CreatedOn = s.CreatedOn,
                CreatedBy = s.CreatedBy,
                IsActive = s.IsActive,
                ModifiedOn = s.ModifiedOn,
                ModifiedBy = s.ModifiedBy
            });

            string result = JsonConvert.SerializeObject(Applicants, Formatting.Indented);
            return new JsonResult { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
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
