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

namespace CampusManagement.Controllers
{
    [Authorize(Roles = "Account Officer,Accounts Officer,Admin Assistant,Admin Officer,Admin.Assistant,Assist. Account Officer,Assist.Technician,Import Manager,Manager Servive & Support,Office Manager,Officer QMS,RSM - Center 2,RSM - South,Sales & Service Executive,Sales Executive,Sales Manager,Sales Representative,Sr.Accounts Officer,Sr.Associate Engineer,Sr.Sales Executive,Sr.Sales Representative,Store Assistant,Store Incharge,Technician")]
    public class FormSaleDetailsController : Controller
    {
        private ModelCMSContainer db = new ModelCMSContainer();
        private ModelFinanceContainer dbFinance = new ModelFinanceContainer();
        FormSaleDetailsViewModel model = new FormSaleDetailsViewModel();

        public ActionResult Index()
        {
            model.FormSaleDetails = db.FormSaleDetails.Where(f => f.PurchaseDate != null).OrderByDescending(a => a.FormID).ToList();
            model.SelectedFormSaleDetail = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc");
            ViewBag.FormTypeID = new SelectList(db.FormTypes, "FormTypeID", "FormTypeName");
            ViewBag.DepositTypeID = new SelectList(db.DepositTypes, "DepositTypeID", "DepositTypeName");
            ViewBag.BatchProgramID = new SelectList(db.GetBatchProgramNameConcat("", 0), "ID", "Name");
            ViewBag.AccountID = new SelectList(dbFinance.Bank_Account, "Account_ID", "Account_No");
            ViewBag.MessageType = "";
            ViewBag.Message = "";
            return View(model);
        }

        [HttpPost]
        public ActionResult Index(string Search)
        {
            var lst = db.FormSaleDetails.Where(x => x.FormNo.Contains(Search) || x.ApplicantName.Contains(Search)).OrderByDescending(a => a.FormID).ToList();
            model.FormSaleDetails = lst;
            model.SelectedFormSaleDetail = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc");
            ViewBag.FormTypeID = new SelectList(db.FormTypes, "FormTypeID", "FormTypeName");
            ViewBag.DepositTypeID = new SelectList(db.DepositTypes, "DepositTypeID", "DepositTypeName");
            ViewBag.BatchProgramID = new SelectList(db.GetBatchProgramNameConcat("", 0), "ID", "Name");
            ViewBag.AccountID = new SelectList(dbFinance.Bank_Account, "Account_ID", "Account_No");
            ViewBag.MessageType = "";
            ViewBag.Message = "";
            return View(model);
        }

        [HttpGet]
        public ActionResult Create()
        {
            model.FormSaleDetails = db.FormSaleDetails.Where(f => f.PurchaseDate != null).OrderByDescending(a => a.FormID).ToList();
            model.SelectedFormSaleDetail = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc");
            ViewBag.FormTypeID = new SelectList(db.FormTypes, "FormTypeID", "FormTypeName");
            ViewBag.DepositTypeID = new SelectList(db.DepositTypes, "DepositTypeID", "DepositTypeName");
            ViewBag.BatchProgramID = new SelectList(db.GetBatchProgramNameConcat("", 0), "ID", "Name");
            ViewBag.AccountID = new SelectList(dbFinance.Bank_Account, "Account_ID", "Account_No");
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

                    if (formSaleDetail.PurchaseDate == null)
                    {
                        ModelState.AddModelError(string.Empty, "Purchase Date is required.");
                        count++;
                        ErrorMessage += count + "-Purchase Date is required.<br />";
                        ViewBag.MessageType = "error";
                    }

                    if (string.IsNullOrEmpty(ErrorMessage))
                    {
                        try
                        {
                            formSaleDetail.CreatedBy = Convert.ToInt32(Session["emp_id"]);
                            formSaleDetail.ModifiedBy = 0;
                            formSaleDetail.IsReceived = "No";
                            formSaleDetail.ReceiveDate = null;
                            db.InsertFormSaleDetail(formSaleDetail.FormID, formSaleDetail.FormNo
                                , formSaleDetail.FormTypeID, formSaleDetail.FormPrice
                                , formSaleDetail.FormDescription, formSaleDetail.DepositTypeID
                                , formSaleDetail.BatchProgramID, formSaleDetail.ApplicantName
                                , formSaleDetail.FatherName, formSaleDetail.PhoneNo, formSaleDetail.CNIC
                                , formSaleDetail.DepositSlipNo, formSaleDetail.PurchaseDate
                                , formSaleDetail.AccountID, formSaleDetail.CreatedBy, formSaleDetail.IsActive
                                , formSaleDetail.ModifiedBy, formSaleDetail.IsReceived, formSaleDetail.ReceiveDate);
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
                else
                {
                    ModelState.AddModelError(string.Empty, "Form # is already exists.");
                    ViewBag.MessageType = "error";
                    ViewBag.Message = "Form # is already exists.";
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
            model.FormSaleDetails = db.FormSaleDetails.Where(f => f.PurchaseDate != null).OrderByDescending(a => a.FormID).ToList();
            model.SelectedFormSaleDetail = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc", formSaleDetail.IsActive);
            ViewBag.FormTypeID = new SelectList(db.FormTypes, "FormTypeID", "FormTypeName", formSaleDetail.FormTypeID);
            ViewBag.DepositTypeID = new SelectList(db.DepositTypes, "DepositTypeID", "DepositTypeName", formSaleDetail.DepositTypeID);
            ViewBag.BatchProgramID = new SelectList(db.GetBatchProgramNameConcat("", 0), "ID", "Name", formSaleDetail.BatchProgramID);
            ViewBag.AccountID = new SelectList(dbFinance.Bank_Account, "Account_ID", "Account_No", formSaleDetail.AccountID);
            return View("Index", model);
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

            model.FormSaleDetails = db.FormSaleDetails.Where(f => f.PurchaseDate != null).OrderByDescending(a => a.FormID).ToList();
            model.SelectedFormSaleDetail = formSaleDetail;
            model.DisplayMode = "ReadWrite";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc", formSaleDetail.IsActive);
            ViewBag.FormTypeID = new SelectList(db.FormTypes, "FormTypeID", "FormTypeName", formSaleDetail.FormTypeID);
            ViewBag.DepositTypeID = new SelectList(db.DepositTypes, "DepositTypeID", "DepositTypeName", formSaleDetail.DepositTypeID);
            ViewBag.BatchProgramID = new SelectList(db.GetBatchProgramNameConcat("", 0), "ID", "Name", formSaleDetail.BatchProgramID);
            ViewBag.AccountID = new SelectList(dbFinance.Bank_Account, "Account_ID", "Account_No", formSaleDetail.AccountID);
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
                        ErrorMessage += count + "-Please select Bank Account.< br />";
                        ViewBag.MessageType = "error";
                    }

                    if (string.IsNullOrEmpty(formSaleDetail.DepositSlipNo))
                    {
                        ModelState.AddModelError(string.Empty, "Please enter Deposit Slip #.");
                        count++;
                        ErrorMessage += count + "-Please enter Deposit Slip #.< br />";
                        ViewBag.MessageType = "error";
                    }
                }

                if (formSaleDetail.PurchaseDate == null)
                {
                    ModelState.AddModelError(string.Empty, "Purchase Date is required.");
                    count++;
                    ErrorMessage += count + "-Purchase Date is required.<br />";
                    ViewBag.MessageType = "error";
                }

                if (string.IsNullOrEmpty(ErrorMessage))
                {
                    try
                    {
                        db.Entry(formSaleDetail).State = EntityState.Modified;
                        formSaleDetail.ModifiedBy = Convert.ToInt32(Session["emp_id"]);
                        formSaleDetail.IsReceived = "No";
                        formSaleDetail.ReceiveDate = null;
                        db.InsertFormSaleDetail(formSaleDetail.FormID, formSaleDetail.FormNo
                            , formSaleDetail.FormTypeID, formSaleDetail.FormPrice
                            , formSaleDetail.FormDescription, formSaleDetail.DepositTypeID
                            , formSaleDetail.BatchProgramID, formSaleDetail.ApplicantName
                            , formSaleDetail.FatherName, formSaleDetail.PhoneNo, formSaleDetail.CNIC
                            , formSaleDetail.DepositSlipNo, formSaleDetail.PurchaseDate
                            , formSaleDetail.AccountID, formSaleDetail.CreatedBy, formSaleDetail.IsActive
                            , formSaleDetail.ModifiedBy, formSaleDetail.IsReceived, formSaleDetail.ReceiveDate);
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
            model.FormSaleDetails = db.FormSaleDetails.Where(f => f.PurchaseDate != null).OrderByDescending(a => a.FormID).ToList();
            model.SelectedFormSaleDetail = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc", formSaleDetail.IsActive);
            ViewBag.FormTypeID = new SelectList(db.FormTypes, "FormTypeID", "FormTypeName", formSaleDetail.FormTypeID);
            ViewBag.DepositTypeID = new SelectList(db.DepositTypes, "DepositTypeID", "DepositTypeName", formSaleDetail.DepositTypeID);
            ViewBag.BatchProgramID = new SelectList(db.GetBatchProgramNameConcat("", 0), "ID", "Name", formSaleDetail.BatchProgramID);
            ViewBag.AccountID = new SelectList(dbFinance.Bank_Account, "Account_ID", "Account_No", formSaleDetail.AccountID);
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

            model.FormSaleDetails = db.FormSaleDetails.Where(f => f.PurchaseDate != null).OrderByDescending(a => a.FormID).ToList();
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
                db.FormSaleDetails.Remove(formSaleDetail);
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
            model.FormSaleDetails = db.FormSaleDetails.Where(f => f.PurchaseDate != null).OrderByDescending(a => a.FormID).ToList();
            model.SelectedFormSaleDetail = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc");
            ViewBag.FormTypeID = new SelectList(db.FormTypes, "FormTypeID", "FormTypeName");
            ViewBag.DepositTypeID = new SelectList(db.DepositTypes, "DepositTypeID", "DepositTypeName");
            ViewBag.BatchProgramID = new SelectList(db.GetBatchProgramNameConcat("", 0), "ID", "Name");
            ViewBag.AccountID = new SelectList(dbFinance.Bank_Account, "Account_ID", "Account_No");
            return View("Index", model);
        }

        public ActionResult PostForm()
        {
            model.FormSaleDetails = db.FormSaleDetails.OrderByDescending(a => a.FormID).ToList();
            model.SelectedFormSaleDetail = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.MessageType = "";
            ViewBag.Message = "";
            return View(model);
        }

        [HttpPost]
        public ActionResult PostForm(string Search, string StartDate, string EndDate)
        {
            var lst = db.FormSaleDetails.Where(x => x.FormNo.Contains(Search) || x.ApplicantName.Contains(Search)).OrderByDescending(a => a.FormID).ToList();
            if (!string.IsNullOrEmpty(StartDate) && !string.IsNullOrEmpty(EndDate))
            {
                DateTime dt1 = Convert.ToDateTime(StartDate);
                DateTime dt2 = Convert.ToDateTime(EndDate);
                lst = db.FormSaleDetails.Where(i => i.PurchaseDate >= dt1 && i.PurchaseDate <= dt2).ToList();
            }
            model.FormSaleDetails = lst;
            model.SelectedFormSaleDetail = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.MessageType = "";
            ViewBag.Message = "";
            return View(model);
        }

        [HttpPost]
        public ActionResult PostVouchers(FormCollection fc)
        {
            int count = 0;
            if (fc["hdnTotalCount"] != null)
            {
                count = Convert.ToInt32(fc["hdnTotalCount"]);
                for (int i = 1; i < count; i++)
                {
                    if (fc["hdnFormID_" + i] != null)
                    {
                        db.InsertPostedVouchers(Convert.ToInt32(fc["hdnFormID_" + i]));
                    }
                }
                ViewBag.MessageType = "success";
                ViewBag.Message = "Vouchers have been posted successfully.";
            }

            var lst = db.FormSaleDetails.OrderByDescending(a => a.FormID).ToList();
            model.FormSaleDetails = lst;
            model.SelectedFormSaleDetail = null;
            model.DisplayMode = "WriteOnly";
            
            return View("PostForm", model);
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
