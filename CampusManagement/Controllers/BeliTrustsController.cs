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
    [Authorize]
    public class BeliTrustsController : Controller
    {
        private ModelCMSNewContainer db = new ModelCMSNewContainer();
        string ErrorMessage = "";
        int count = 0;

        public ActionResult Index()
        {
            return View(db.BeliTrusts.OrderByDescending(a => a.BeliTrustID));
        }

        [HttpPost]
        public ActionResult Index(string Search)
        {
            return View(db.BeliTrusts.Where(x => x.FirstName.Contains(Search) || x.Email.Contains(Search) || x.LastName.Contains(Search)).OrderByDescending(a => a.BeliTrustID));
        }

        public ActionResult Create()
        {
            ViewBag.BeliTrustCategoryID = new SelectList(db.BeliTrustCategories, "BeliTrustCategoryID", "BeliTrustCategoryName");
            ViewBag.FormNo = "";
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(BeliTrust beliTrust, FormCollection fc)
        {
            BeliTrust app = db.BeliTrusts.FirstOrDefault(a => a.FormNo == beliTrust.FormNo);
            if (app == null)
            {
                beliTrust.CreatedOn = DateTime.Now;
                beliTrust.CreatedBy = Convert.ToInt32(Session["emp_id"]);

                try
                {
                    db.BeliTrusts.Add(beliTrust);

                    try
                    {
                        if (string.IsNullOrEmpty(ErrorMessage))
                        {
                            db.SaveChanges();
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
                        ViewBag.Message = "Duplicate CNIC/Email";
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
                app.ModifiedOn = DateTime.Now;
                app.ModifiedBy = Convert.ToInt32(Session["emp_id"]);
                app.NameOfOrganization = beliTrust.NameOfOrganization;
                app.DesignationName = beliTrust.DesignationName;
                app.MonthlySalary = beliTrust.MonthlySalary;
                app.DateOfRetirement = beliTrust.DateOfRetirement;
                app.RetiredOrganization = beliTrust.RetiredOrganization;
                app.RetiredPosition = beliTrust.RetiredPosition;
                app.RetiredLastDrawnGrossSalary = beliTrust.RetiredLastDrawnGrossSalary;
                app.NatureOfBusiness = beliTrust.NatureOfBusiness;
                app.BeliTrustCategoryID = beliTrust.BeliTrustCategoryID;
                app.MonthlyIncomeBus = beliTrust.MonthlyIncomeBus;
                app.NatureOfSource = beliTrust.NatureOfSource;
                app.MonthlyIncomeSource = beliTrust.MonthlyIncomeSource;
                app.TypeOfResidence = beliTrust.TypeOfResidence;
                app.ResidenceText = beliTrust.ResidenceText;
                app.MonthlyRent = beliTrust.MonthlyRent;
                app.TotalTuitionFee = beliTrust.TotalTuitionFee;
                app.RequestForConcession = beliTrust.RequestForConcession;
                app.RemarksByPrincipal = beliTrust.RemarksByPrincipal;
                app.RemarksByHOD = beliTrust.RemarksByHOD;
                db.Entry(app).State = EntityState.Modified;
                db.SaveChanges();
            }

            for (var i = 0; i <= 6; i++)
            {
                int FamilyMemberID = Convert.ToInt32(fc["FamilyMemberID_" + i]);
                FamilyMember fml = db.FamilyMembers.FirstOrDefault(f => f.FamilyMemberID == FamilyMemberID);
                fml.FamilyMemberName = fc["FamilyMemberName_" + i];
                fml.FamilyMemberAge = fc["FamilyMemberAge_" + i];
                fml.RelationWithStudent = fc["RelationWithStudent_" + i];
                fml.MaritalStatus = fc["MaritalStatus_" + i];
                fml.MonthlyEducationExpense = fc["MonthlyEducationExpense_" + i];
                fml.MedicalExpense = fc["MedicalExpense_" + i];
                fml.TotalFamilyMember = fc["TotalFamilyMember"];
                fml.ModifiedOn = DateTime.Now;
                fml.Modifiedby = Convert.ToInt32(Session["emp_id"]);
                fml.IsActive = "Yes";
                fml.FormNo = beliTrust.FormNo;
                db.Entry(fml).State = EntityState.Modified;
                db.SaveChanges();
            }

            for (var i = 0; i <= 3; i++)
            {
                int FinancialSupportID = Convert.ToInt32(fc["FinancialSupportID_" + i]);
                FinancialSupport fin = db.FinancialSupports.FirstOrDefault(f => f.FinancialSupportID == FinancialSupportID);
                fin.NameOfOrganization = fc["NameOfOrganization_" + i];
                fin.AmountAppliedFor = fc["AmountAppliedFor_" + i];
                fin.Outcome = fc["Outcome_" + i];
                fin.ModifiedOn = DateTime.Now;
                fin.Modifiedby = Convert.ToInt32(Session["emp_id"]);
                fin.IsActive = "Yes";
                fin.FormNo = beliTrust.FormNo;
                db.Entry(fin).State = EntityState.Modified;
                db.SaveChanges();
            }
            ViewBag.BeliTrustCategoryID = new SelectList(db.BeliTrustCategories, "BeliTrustCategoryID", "BeliTrustCategoryName");
            ViewBag.MessageType = "success";
            ViewBag.Message = "Data has been saved successfully.";
            ViewBag.FormNo = beliTrust.FormNo;
            return View(beliTrust);
        }


        public ActionResult Affidavit()
        {
            ViewBag.FormNo = "";
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Affidavit(BeliTrustAffidavit beliTrustAffidavit)
        {
            BeliTrustAffidavit app = db.BeliTrustAffidavits.FirstOrDefault(a => a.FormNo == beliTrustAffidavit.FormNo);
            if (app == null)
            {
                beliTrustAffidavit.CreatedOn = DateTime.Now;
                beliTrustAffidavit.CreatedBy = Convert.ToInt32(Session["emp_id"]);

                try
                {
                    db.BeliTrustAffidavits.Add(beliTrustAffidavit);

                    try
                    {
                        if (string.IsNullOrEmpty(ErrorMessage))
                        {
                            db.SaveChanges();
                            ViewBag.MessageType = "success";
                            ViewBag.Message = "Data has been saved successfully.";
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
                        ViewBag.Message = "Duplicate CNIC/Email";
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
                app.ModifiedOn = DateTime.Now;
                app.Modifiedby = Convert.ToInt32(Session["emp_id"]);
                app.ApplicantName = beliTrustAffidavit.ApplicantName;
                app.FatherName = beliTrustAffidavit.FatherName;
                app.ACNIC = beliTrustAffidavit.ACNIC;
                app.Address = beliTrustAffidavit.Address;
                app.CastName = beliTrustAffidavit.CastName;
                app.FamilyIncome = beliTrustAffidavit.FamilyIncome;
                app.DeponentName = beliTrustAffidavit.DeponentName;
                app.DeponentFatherName = beliTrustAffidavit.DeponentFatherName;
                app.DeponentCNIC = beliTrustAffidavit.DeponentCNIC;
                app.DeponentAddress = beliTrustAffidavit.DeponentAddress;
                app.Witness1Name = beliTrustAffidavit.Witness1Name;
                app.Witness1FatherName = beliTrustAffidavit.Witness1FatherName;
                app.Witness1CNIC = beliTrustAffidavit.Witness1CNIC;
                app.Witness1Address = beliTrustAffidavit.Witness1Address;
                app.Witness2Name = beliTrustAffidavit.Witness2Name;
                app.Witness2FatherName = beliTrustAffidavit.Witness2FatherName;
                app.Witness2CNIC = beliTrustAffidavit.Witness2CNIC;
                app.Witness2Address = beliTrustAffidavit.Witness2Address;

                db.Entry(app).State = EntityState.Modified;
                db.SaveChanges();
            }



            ViewBag.FormNo = beliTrustAffidavit.FormNo;
            return View(beliTrustAffidavit);
        }

        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BeliTrust beliTrust = db.BeliTrusts.Find(id);
            if (beliTrust == null)
            {
                return HttpNotFound();
            }

            ViewBag.FormNo = beliTrust.FormNo;
            return View(beliTrust);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(BeliTrust beliTrust)
        {
            try
            {
                db.Entry(beliTrust).State = EntityState.Modified;
                beliTrust.ModifiedOn = DateTime.Now;
                beliTrust.ModifiedBy = Convert.ToInt32(Session["emp_id"]);

                try
                {
                    db.SaveChanges();
                    ViewBag.MessageType = "success";
                    ViewBag.Message = "Data has been saved successfully.";
                    return RedirectToAction("Index", "BeliTrusts");
                }
                catch (DbUpdateException ex)
                {
                    ViewBag.MessageType = "error";
                    ViewBag.Message = "Duplicate CNIC/Email";
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

            ViewBag.FormNo = beliTrust.FormNo;
            return View(beliTrust);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BeliTrust beliTrust = db.BeliTrusts.Find(id);
            if (beliTrust == null)
            {
                return HttpNotFound();
            }
            return View(beliTrust);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int? id)
        {
            BeliTrust beliTrust = db.BeliTrusts.Find(id);
            db.BeliTrusts.Remove(beliTrust);
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

        public JsonResult GetApplicant(string FormNo)
        {
            List<Applicant> lstApplicant = new List<Applicant>();
            string expiryDate = "";
            string DOB = "";

            lstApplicant = db.Applicants.Where(a => a.FormNo == FormNo).ToList();
            if (lstApplicant.Count > 0)
            {
                expiryDate = (lstApplicant[0].PassportExpiryDate == null) ? "" : lstApplicant[0].PassportExpiryDate;
                DOB = (lstApplicant[0].ApplicantDOB == null) ? "" : lstApplicant[0].ApplicantDOB;
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
                SalutationName = db.Salutations.FirstOrDefault(sa => sa.SalutationID == s.SalutationID).SalutationName,
                FatherName = s.FatherName,
                ApplicantDOB = DOB,
                PlaceOfBirth = s.PlaceOfBirth,
                GenderID = s.GenderID,
                GenderName = db.Genders.FirstOrDefault(g => g.GenderID == s.GenderID).GenderName,
                NationalityID = s.NationalityID,
                MaritalStatusID = s.MaritalStatusID,
                MaritalStatusName = db.MaritalStatus.FirstOrDefault(m => m.MaritalStatusID == s.MaritalStatusID).MaritalStatusName,
                PassportNo = s.PassportNo,
                PassportExpiryDate = expiryDate,
                PTCLNO = s.PTCLNO,
                CellNo = s.CellNo,
                Email = s.Email,
                AlternateEmail = s.AlternateEmail,
                GuardianName = s.GuardianName,
                RelationTypeID = s.RelationTypeID,
                RelationTypeName = db.RelationTypes.FirstOrDefault(r => r.RelationTypeID == s.RelationTypeID).RelationTypeName,
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
                StatusName = db.Status.FirstOrDefault(st => st.StatusID == s.StatusID).StatusName,
                CreatedOn = s.CreatedOn,
                CreatedBy = s.CreatedBy,
                IsActive = s.IsActive,
                ModifiedOn = s.ModifiedOn,
                ModifiedBy = s.ModifiedBy
            });

            string result = JsonConvert.SerializeObject(Applicants, Formatting.Indented);
            return new JsonResult { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult GetBeliTrustInfo(string FormNo)
        {
            List<BeliTrust> lstBeliTrust = new List<BeliTrust>();

            lstBeliTrust = db.BeliTrusts.Where(a => a.FormNo == FormNo).ToList();

            var BeliTrustInfo = lstBeliTrust.Select(s => new
            {
                NameOfOrganization = s.NameOfOrganization,
                DesignationName = s.DesignationName,
                MonthlySalary = s.MonthlySalary,
                DateOfRetirement = s.DateOfRetirement,
                RetiredOrganization = s.RetiredOrganization,
                RetiredPosition = s.RetiredPosition,
                RetiredLastDrawnGrossSalary = s.RetiredLastDrawnGrossSalary,
                NatureOfBusiness = s.NatureOfBusiness,
                MonthlyIncomeBus = s.MonthlyIncomeBus,
                NatureOfSource = s.NatureOfSource,
                MonthlyIncomeSource = s.MonthlyIncomeSource,
                TypeOfResidence = s.TypeOfResidence,
                ResidenceText = s.ResidenceText,
                MonthlyRent = s.MonthlyRent,
                BeliTrustCategoryID = s.BeliTrustCategoryID,
                TotalTuitionFee = s.TotalTuitionFee,
                RequestForConcession = s.RequestForConcession,
                RemarksByPrincipal = s.RemarksByPrincipal,
                RemarksByHOD = s.RemarksByHOD
            });

            string result = JsonConvert.SerializeObject(BeliTrustInfo, Formatting.Indented);
            return new JsonResult { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult GetAffidavitDetails(string FormNo)
        {
            List<BeliTrustAffidavit> lstBeliTrust = new List<BeliTrustAffidavit>();

            lstBeliTrust = db.BeliTrustAffidavits.Where(a => a.FormNo == FormNo).ToList();

            var BeliAffidavitInfo = lstBeliTrust.Select(s => new
            {
                ApplicantName = s.ApplicantName,
                FatherName = s.FatherName,
                CastName = s.CastName,
                ACNIC = s.ACNIC,
                FamilyIncome = s.FamilyIncome,
                Address = s.Address,
                DeponentName = s.DeponentName,
                DeponentFatherName = s.DeponentFatherName,
                DeponentCNIC = s.DeponentCNIC,
                DeponentAddress = s.DeponentAddress,
                Witness1Name = s.Witness1Name,
                Witness1FatherName = s.Witness1FatherName,
                Witness1CNIC = s.Witness1CNIC,
                Witness1Address = s.Witness1Address,
                Witness2Name = s.Witness2Name,
                Witness2FatherName = s.Witness2FatherName,
                Witness2CNIC = s.Witness2CNIC,
                Witness2Address = s.Witness2Address,
                ReverseOf = s.ReverseOf,
                DeponentDeclareOn = s.DeponentDeclareOn,
                DeponentDeclareAt = s.DeponentDeclareAt,
                DeponentDeclareOnOath = s.DeponentDeclareOnOath,
            });

            string result = JsonConvert.SerializeObject(BeliAffidavitInfo, Formatting.Indented);
            return new JsonResult { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public JsonResult GetFamilyMembers(string FormNo)
        {
            List<GetFamilyMembers_Result> lstFM = new List<GetFamilyMembers_Result>();

            lstFM = db.GetFamilyMembers(FormNo).ToList();
            if (lstFM.Count == 0)
            {
                ViewBag.MessageType = "error";
                ViewBag.Message = "No result found.";
                return new JsonResult { Data = "No result found.", JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }

            var family = lstFM.Select(s => new
            {
                FamilyMemberID = s.FamilyMemberID,
                FamilyMemberName = s.FamilyMemberName,
                FamilyMemberAge = s.FamilyMemberAge,
                RelationWithStudent = s.RelationWithStudent,
                MaritalStatus = s.MaritalStatus,
                MonthlyEducationExpense = s.MonthlyEducationExpense,
                MedicalExpense = s.MedicalExpense,
                TotalFamilyMember = s.TotalFamilyMember
            });

            string result = JsonConvert.SerializeObject(family, Formatting.Indented);
            return new JsonResult { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult GetFinancialSupports(string FormNo)
        {
            List<GetFinancialSupports_Result> lstFS = new List<GetFinancialSupports_Result>();

            lstFS = db.GetFinancialSupports(FormNo).ToList();
            if (lstFS.Count == 0)
            {
                ViewBag.MessageType = "error";
                ViewBag.Message = "No result found.";
                return new JsonResult { Data = "No result found.", JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }

            var fs = lstFS.Select(s => new
            {
                FinancialSupportID = s.FinancialSupportID,
                NameOfOrganization = s.NameOfOrganization,
                AmountAppliedFor = s.AmountAppliedFor,
                Outcome = s.Outcome
            });

            string result = JsonConvert.SerializeObject(fs, Formatting.Indented);
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
