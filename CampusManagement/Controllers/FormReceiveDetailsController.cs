using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core;
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
    [Authorize]
    public class FormReceiveDetailsController : Controller
    {
        private ModelCMSContainer db = new ModelCMSContainer();
        FormSaleDetailsViewModel model = new FormSaleDetailsViewModel();

        public ActionResult Index()
        {
            model.FormSaleDetails = db.FormSaleDetails.Where(f => f.IsReceived == "Yes" && f.DepositTypeID != 4).OrderByDescending(a => a.FormID).ToList();
            model.SelectedFormSaleDetail = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc");
            ViewBag.IsReceived = new SelectList(db.Options, "OptionDesc", "OptionDesc");
            ViewBag.FormTypeID = new SelectList(db.FormTypes, "FormTypeID", "FormTypeName");
            ViewBag.DepositTypeID = new SelectList(db.DepositTypes.Where(d => d.DepositTypeName != "Online"), "DepositTypeID", "DepositTypeName");
            ViewBag.BatchProgramID = new SelectList(db.GetBatchProgramNameConcat("", 0), "ID", "Name");
            ViewBag.AccountID = new SelectList(db.Finance_Bank_Accounts, "Account_ID", "Account_No");
            ViewBag.MessageType = "";
            ViewBag.Message = "";
            return View(model);
        }

        [HttpGet]
        public ActionResult Create()
        {
            model.FormSaleDetails = db.FormSaleDetails.Where(f => f.IsReceived == "Yes" && f.DepositTypeID != 4).OrderByDescending(a => a.FormID).ToList();
            model.SelectedFormSaleDetail = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc");
            ViewBag.IsReceived = new SelectList(db.Options, "OptionDesc", "OptionDesc");
            ViewBag.FormTypeID = new SelectList(db.FormTypes, "FormTypeID", "FormTypeName");
            ViewBag.DepositTypeID = new SelectList(db.DepositTypes.Where(d => d.DepositTypeName != "Online"), "DepositTypeID", "DepositTypeName");
            ViewBag.BatchProgramID = new SelectList(db.GetBatchProgramNameConcat("", 0), "ID", "Name");
            ViewBag.AccountID = new SelectList(db.Finance_Bank_Accounts, "Account_ID", "Account_No");
            ViewBag.MessageType = "";
            ViewBag.Message = "";
            return View("Index", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(FormSaleDetail formSaleDetail)
        {
            string ErrorMessage = "";
            int count = 0;

            try
            {
                FormSaleDetail fsd = db.FormSaleDetails.FirstOrDefault(f => f.FormNo == formSaleDetail.FormNo);
                Applicant applicant = db.Applicants.FirstOrDefault(f => f.FormNo == formSaleDetail.FormNo);
                if (fsd == null && applicant == null)
                {
                    formSaleDetail.CreatedBy = Convert.ToInt32(Session["emp_id"]);
                    formSaleDetail.ModifiedBy = null;
                    formSaleDetail.IsReceived = "Yes";
                    formSaleDetail.PurchaseDate = null;
                    formSaleDetail.AccountID = null;
                    formSaleDetail.DepositSlipNo = null;
                    InsertFormDetail(formSaleDetail, ref ErrorMessage, ref count);
                }
                else if (fsd != null)
                {
                    formSaleDetail.ModifiedBy = Convert.ToInt32(Session["emp_id"]);
                    formSaleDetail.IsReceived = "Yes";
                    InsertFormDetail(formSaleDetail, ref ErrorMessage, ref count);
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
            model.FormSaleDetails = db.FormSaleDetails.Where(f => f.IsReceived == "Yes" && f.DepositTypeID != 4).OrderByDescending(a => a.FormID).ToList();
            model.SelectedFormSaleDetail = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc", formSaleDetail.IsActive);
            ViewBag.FormTypeID = new SelectList(db.FormTypes, "FormTypeID", "FormTypeName", formSaleDetail.FormTypeID);
            ViewBag.DepositTypeID = new SelectList(db.DepositTypes.Where(d => d.DepositTypeName != "Online"), "DepositTypeID", "DepositTypeName", formSaleDetail.DepositTypeID);
            ViewBag.BatchProgramID = new SelectList(db.GetBatchProgramNameConcat("", 0), "ID", "Name", formSaleDetail.BatchProgramID);
            ViewBag.AccountID = new SelectList(db.Finance_Bank_Accounts, "Account_ID", "Account_No", formSaleDetail.AccountID);
            return View("Index", model);
        }

        private void InsertFormDetail(FormSaleDetail formSaleDetail, ref string ErrorMessage, ref int count)
        {
            DepositType dt = db.DepositTypes.FirstOrDefault(d => d.DepositTypeID == formSaleDetail.DepositTypeID);
            string depositeType = "";
            if (dt != null)
            {
                depositeType = dt.DepositTypeName;
            }
            if (depositeType.ToLower() == "cash")
            {
                formSaleDetail.AccountID = null;
                formSaleDetail.DepositSlipNo = null;
            }
            else if (depositeType.ToLower() == "bank")
            {
                if (formSaleDetail.AccountID == null || formSaleDetail.AccountID == 0)
                {
                    ModelState.AddModelError(string.Empty, "Please select Bank Account.");
                    count++;
                    ErrorMessage += count + "-Please select Bank Account.<br />";
                    ViewBag.MessageType = "error";
                }

                if (string.IsNullOrEmpty(formSaleDetail.DepositSlipNo))
                {
                    ModelState.AddModelError(string.Empty, "Please enter Deposit Slip #.");
                    count++;
                    ErrorMessage += count + "-Please enter Deposit Slip #.<br />";
                    ViewBag.MessageType = "error";
                }
            }

            if (formSaleDetail.ReceiveDate == null)
            {
                ModelState.AddModelError(string.Empty, "Receive Date is required.");
                count++;
                ErrorMessage += count + "-Receive Date is required.<br />";
                ViewBag.MessageType = "error";
            }

            if (string.IsNullOrEmpty(ErrorMessage))
            {
                try
                {
                    db.InsertFormSaleDetail(formSaleDetail.FormID, formSaleDetail.FormNo
                        , formSaleDetail.FormTypeID, formSaleDetail.FormPrice
                        , formSaleDetail.FormDescription, formSaleDetail.DepositTypeID
                        , formSaleDetail.BatchProgramID
                        , formSaleDetail.FatherName, formSaleDetail.PhoneNo, formSaleDetail.CNIC
                        , formSaleDetail.DepositSlipNo, formSaleDetail.PurchaseDate
                        , formSaleDetail.AccountID, formSaleDetail.CreatedBy, formSaleDetail.IsActive
                        , formSaleDetail.ModifiedBy, formSaleDetail.IsReceived, formSaleDetail.ReceiveDate
                        , formSaleDetail.FirstName, formSaleDetail.LastName, formSaleDetail.DegreeID, formSaleDetail.Wavier_Discount);
                    ViewBag.MessageType = "success";
                    ViewBag.Message = "Data has been saved successfully.";
                }
                catch (EntityCommandExecutionException ex)
                {
                    ViewBag.MessageType = "error";
                    ViewBag.Message = ex.Message;
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }
            else
            {
                ViewBag.MessageType = "error";
                ViewBag.Message = ErrorMessage;
            }
        }

        public ActionResult Update(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            FormSaleDetail formSaleDetail = db.FormSaleDetails.Find(id);
            if (formSaleDetail == null)
            {
                return HttpNotFound();
            }

            model.FormSaleDetails = db.FormSaleDetails.Where(f => f.IsReceived == "Yes" && f.DepositTypeID != 4).OrderByDescending(a => a.FormID).ToList();
            model.SelectedFormSaleDetail = formSaleDetail;
            model.DisplayMode = "ReadWrite";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc", formSaleDetail.IsActive);
            ViewBag.FormTypeID = new SelectList(db.FormTypes, "FormTypeID", "FormTypeName", formSaleDetail.FormTypeID);
            ViewBag.DepositTypeID = new SelectList(db.DepositTypes.Where(d => d.DepositTypeName != "Online"), "DepositTypeID", "DepositTypeName", formSaleDetail.DepositTypeID);
            ViewBag.BatchProgramID = new SelectList(db.GetBatchProgramNameConcat("", 0), "ID", "Name", formSaleDetail.BatchProgramID);
            ViewBag.AccountID = new SelectList(db.Finance_Bank_Accounts, "Account_ID", "Account_No", formSaleDetail.AccountID);
            ViewBag.MessageType = "";
            ViewBag.Message = "";
            return View("Index", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(FormSaleDetail formSaleDetail)
        {
            string ErrorMessage = "";
            int count = 0;
            try
            {
                formSaleDetail.ModifiedBy = Convert.ToInt32(Session["emp_id"]);
                formSaleDetail.IsReceived = "Yes";
                //formSaleDetail.FormPrice = "1500";
                InsertFormDetail(formSaleDetail, ref ErrorMessage, ref count);
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
            model.FormSaleDetails = db.FormSaleDetails.Where(f => f.IsReceived == "Yes" && f.DepositTypeID != 4).OrderByDescending(a => a.FormID).ToList();
            model.SelectedFormSaleDetail = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc", formSaleDetail.IsActive);
            ViewBag.FormTypeID = new SelectList(db.FormTypes, "FormTypeID", "FormTypeName", formSaleDetail.FormTypeID);
            ViewBag.DepositTypeID = new SelectList(db.DepositTypes.Where(d => d.DepositTypeName != "Online"), "DepositTypeID", "DepositTypeName", formSaleDetail.DepositTypeID);
            ViewBag.BatchProgramID = new SelectList(db.GetBatchProgramNameConcat("", 0), "ID", "Name", formSaleDetail.BatchProgramID);
            ViewBag.AccountID = new SelectList(db.Finance_Bank_Accounts, "Account_ID", "Account_No", formSaleDetail.AccountID);
            return View("Index", model);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FormSaleDetail formSaleDetail = db.FormSaleDetails.Find(id);
            if (formSaleDetail == null)
            {
                return HttpNotFound();
            }

            model.FormSaleDetails = db.FormSaleDetails.Where(f => f.IsReceived == "Yes" && f.DepositTypeID != 4).OrderByDescending(a => a.FormID).ToList();
            model.SelectedFormSaleDetail = formSaleDetail;
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
                FormSaleDetail formSaleDetail = db.FormSaleDetails.Find(id);
                db.Entry(formSaleDetail).State = EntityState.Modified;
                formSaleDetail.IsReceived = "No";
                formSaleDetail.ReceiveDate = null;
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
            model.FormSaleDetails = db.FormSaleDetails.Where(f => f.IsReceived == "Yes" && f.DepositTypeID != 4).OrderByDescending(a => a.FormID).ToList();
            model.SelectedFormSaleDetail = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc");
            ViewBag.IsReceived = new SelectList(db.Options, "OptionDesc", "OptionDesc");
            ViewBag.FormTypeID = new SelectList(db.FormTypes, "FormTypeID", "FormTypeName");
            ViewBag.DepositTypeID = new SelectList(db.DepositTypes.Where(d => d.DepositTypeName != "Online"), "DepositTypeID", "DepositTypeName");
            ViewBag.BatchProgramID = new SelectList(db.GetBatchProgramNameConcat("", 0), "ID", "Name");
            ViewBag.AccountID = new SelectList(db.Finance_Bank_Accounts, "Account_ID", "Account_No");
            return View("Index", model);
        }

        public JsonResult GetFormSaleDetail()
        {
            List<FormSaleDetail> lstFS = new List<FormSaleDetail>();
            string pDate = "";
            string cDate = "";
            string mDate = "";
            string FormNo = Request.QueryString["FormNo"];

            lstFS = db.FormSaleDetails.Where(f => f.FormNo == FormNo && f.PurchaseDate != null && (f.IsReceived == "No" || f.IsReceived == null) && f.DepositTypeID != 4).OrderByDescending(a => a.FormID).ToList();
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
                ApplicantName = s.ApplicantName,
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
