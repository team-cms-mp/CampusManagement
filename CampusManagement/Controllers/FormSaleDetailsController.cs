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
using CampusManagement.App_Code;
using Newtonsoft.Json;
using PagedList;

namespace CampusManagement.Controllers
{
    [Authorize]
    public class FormSaleDetailsController : Controller
    {
        private ModelCMSNewContainer db = new ModelCMSNewContainer();
        FormSaleDetailsViewModel model = new FormSaleDetailsViewModel();

        string BatchID = "0";

        public FormSaleDetailsController()
        {
            Batch batch = db.Batches.FirstOrDefault(b => b.IsActive == "Current");
            if (batch != null)
            {
                BatchID = batch.BatchID.ToString();
            }
        }

        public ActionResult Index(int? page, int? pageSize)
        {
            if (pageSize == null)
            {
                pageSize = 10;
            }
            ViewBag.pageSize = pageSize;
            int pageNumber = (page ?? 1);
            ViewBag.TotalRecords = db.sp_GetFormSaleDatailForPage("", Convert.ToInt32(BatchID)).ToList().Count();
            model.plFormsale = db.sp_GetFormSaleDatailForPage("", Convert.ToInt32(BatchID)).ToList().ToPagedList(pageNumber, Convert.ToInt32(pageSize));
            model.SelectedFormSaleDetail = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc");
            ViewBag.FormTypeID = new SelectList(db.FormTypes, "FormTypeID", "FormTypeName");
            ViewBag.DepositTypeID = new SelectList(db.DepositTypes.Where(d => d.DepositTypeName != "Online"), "DepositTypeID", "DepositTypeName");

            ViewBag.BatchProgramID = new SelectList(db.GetBatchProgramNameConcat(BatchID, 0).ToList(), "ID", "Name");
            ViewBag.BatchProgramID1 = new SelectList(db.GetBatchProgramNameConcat(BatchID, 0).ToList(), "ID", "Name");
            ViewBag.BatchProgramID2 = new SelectList(db.GetBatchProgramNameConcat(BatchID, 0).ToList(), "ID", "Name");
            ViewBag.BatchProgramID3 = new SelectList(db.GetBatchProgramNameConcat(BatchID, 0).ToList(), "ID", "Name");
            ViewBag.BatchProgramID4 = new SelectList(db.GetBatchProgramNameConcat(BatchID, 0).ToList(), "ID", "Name");
            ViewBag.AccountID = new SelectList(db.Finance_Bank_Accounts, "Account_ID", "Account_No");
            ViewBag.CountryCode = new SelectList(db.Countries, "CountryCode", "CountryCode");
            ViewBag.DegreeID = new SelectList(db.GetDegrees(2), "DegreeID", "DegreeName");
            ViewBag.Wavier_Discount = new SelectList(db.Options, "OptionDesc", "OptionDesc", "No");
            ViewBag.LevelID = new SelectList(db.Levels, "LevelID", "LevelName");

            ViewBag.MessageType = "";
            ViewBag.Message = "";
            return View(model);
        }

        [HttpPost]
        public ActionResult Index(string Search, int? page, int? pageSize)
        {
            if (pageSize == null)
            {
                pageSize = 10;
            }
            ViewBag.pageSize = pageSize;
            int pageNumber = (page ?? 1);
            ViewBag.TotalRecords = db.sp_GetFormSaleDatailForPage(Search, 0).ToList().Count();
            model.plFormsale = db.sp_GetFormSaleDatailForPage(Search, 0).ToList().ToPagedList(pageNumber, Convert.ToInt32(pageSize));
            model.SelectedFormSaleDetail = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc");
            ViewBag.FormTypeID = new SelectList(db.FormTypes, "FormTypeID", "FormTypeName");
            ViewBag.DepositTypeID = new SelectList(db.DepositTypes.Where(d => d.DepositTypeName != "Online"), "DepositTypeID", "DepositTypeName");
            ViewBag.BatchProgramID = new SelectList(db.GetBatchProgramNameConcat(BatchID, 0), "ID", "Name");
            ViewBag.BatchProgramID1 = new SelectList(db.GetBatchProgramNameConcat(BatchID, 0), "ID", "Name");
            ViewBag.BatchProgramID2 = new SelectList(db.GetBatchProgramNameConcat(BatchID, 0), "ID", "Name");
            ViewBag.BatchProgramID3 = new SelectList(db.GetBatchProgramNameConcat(BatchID, 0), "ID", "Name");
            
            ViewBag.AccountID = new SelectList(db.Finance_Bank_Accounts, "Account_ID", "Account_No");
            ViewBag.CountryCode = new SelectList(db.Countries, "CountryCode", "CountryCode");
            ViewBag.DegreeID = new SelectList(db.GetDegrees(2), "DegreeID", "DegreeName");
            ViewBag.Wavier_Discount = new SelectList(db.Options, "OptionDesc", "OptionDesc", "No");
            ViewBag.LevelID = new SelectList(db.Levels, "LevelID", "LevelName");

            ViewBag.MessageType = "";
            ViewBag.Message = "";
            return View(model);
        }

        [HttpGet]
        public ActionResult Create(int? page, int? pageSize)
        {
            if (pageSize == null)
            {
                pageSize = 10;
            }
            ViewBag.pageSize = pageSize;
            int pageNumber = (page ?? 1);
            ViewBag.TotalRecords = db.sp_GetFormSaleDatailForPage("", Convert.ToInt32(BatchID)).ToList().Count();
            model.plFormsale = db.sp_GetFormSaleDatailForPage("", Convert.ToInt32(BatchID)).ToList().ToPagedList(pageNumber, Convert.ToInt32(pageSize));
            
            model.SelectedFormSaleDetail = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc");
            ViewBag.FormTypeID = new SelectList(db.FormTypes, "FormTypeID", "FormTypeName");
            ViewBag.DepositTypeID = new SelectList(db.DepositTypes.Where(d => d.DepositTypeName != "Online"), "DepositTypeID", "DepositTypeName");
            ViewBag.BatchProgramID = new SelectList(db.GetBatchProgramNameConcat(BatchID, 0), "ID", "Name");
            ViewBag.BatchProgramID1 = new SelectList(db.GetBatchProgramNameConcat(BatchID, 0), "ID", "Name");
            ViewBag.BatchProgramID2 = new SelectList(db.GetBatchProgramNameConcat(BatchID, 0), "ID", "Name");
            ViewBag.BatchProgramID3 = new SelectList(db.GetBatchProgramNameConcat(BatchID, 0), "ID", "Name");
            
            ViewBag.AccountID = new SelectList(db.Finance_Bank_Accounts, "Account_ID", "Account_No");
            ViewBag.CountryCode = new SelectList(db.Countries, "CountryCode", "CountryCode");
            ViewBag.DegreeID = new SelectList(db.GetDegrees(2), "DegreeID", "DegreeName");
            ViewBag.Wavier_Discount = new SelectList(db.Options, "OptionDesc", "OptionDesc", "No");
            ViewBag.LevelID = new SelectList(db.Levels, "LevelID", "LevelName");
            ViewBag.MessageType = "";
            ViewBag.Message = "";
            return View("Index", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(FormSaleDetail formSaleDetail, int? page, int? pageSize)
        {
            if (pageSize == null)
            {
                pageSize = 10;
            }
            ViewBag.pageSize = pageSize;
            int pageNumber = (page ?? 1);
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
                    count++;
                    ErrorMessage += count + "-Purchase Date is required.<br />";
                    ViewBag.MessageType = "error";
                }

                if (!ComparePriceWithWaiveOff(formSaleDetail.BatchProgramID1, formSaleDetail.Wavier_Discount))
                {
                    count++;
                    ErrorMessage += count + "-Waive Off/Discount should be less then Prospectus Fee.<br />";
                    ViewBag.MessageType = "error";
                }

                if (string.IsNullOrEmpty(ErrorMessage))
                {
                    try
                    {
                        formSaleDetail.CreatedBy = Convert.ToInt32(Session["emp_id"]);
                        formSaleDetail.ModifiedBy = 0;
                        formSaleDetail.IsReceived = "Yes";
                        formSaleDetail.ReceiveDate = DateTime.Now.ToShortDateString();
                        formSaleDetail.FormPrice = "1500";

                        formSaleDetail.FormNo = db.GetMaxFormNo().FirstOrDefault();

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
                    }
                    catch (EntityCommandExecutionException ex)
                    {
                        ViewBag.MessageType = "error";
                        ViewBag.Message = string.Concat(ex.Message, ", Inner Exception: " + ex.InnerException);
                        ModelState.AddModelError(string.Empty, ex.Message);
                    }
                }
                else
                {
                    ViewBag.MessageType = "error";
                    ViewBag.Message = ErrorMessage;
                }

                ApplyForProgram afp = new ApplyForProgram();
                ApplyForProgram afpCheck = new ApplyForProgram();
                List<ApplyForProgram> lstPrograms = db.ApplyForPrograms.Where(p => p.FormNo == formSaleDetail.FormNo).ToList();
                if (lstPrograms.Count > 0)
                {
                    db.ApplyForPrograms.RemoveRange(lstPrograms);
                    db.SaveChanges();
                }

                afp.FormNo = formSaleDetail.FormNo;
                afp.IsActive = "Yes";
                afp.CreatedBy = Convert.ToInt32(Session["emp_id"]);
                afp.CreatedOn = DateTime.Now;

                afpCheck = db.ApplyForPrograms.FirstOrDefault(a => a.ProgramID == formSaleDetail.BatchProgramID1 && a.FormNo == formSaleDetail.FormNo);

                if (formSaleDetail.BatchProgramID1 != null)
                {
                    if (formSaleDetail.BatchProgramID1 != 0)
                    {
                        afpCheck = db.ApplyForPrograms.FirstOrDefault(a => a.ProgramID == formSaleDetail.BatchProgramID1 && a.FormNo == formSaleDetail.FormNo);
                        if (afpCheck == null)
                        {
                            afp.ProgramID = formSaleDetail.BatchProgramID1;
                            afp.ProgramPriority = 1;
                            db.ApplyForPrograms.Add(afp);

                            //Program Preference 1 will be the Applicant program
                            FormSaleDetail app = db.FormSaleDetails.FirstOrDefault(a => a.FormNo == formSaleDetail.FormNo);
                            app.BatchProgramID = Convert.ToInt32(formSaleDetail.BatchProgramID1);
                            db.Entry(app).State = EntityState.Modified;
                            db.SaveChanges();
                        }

                        if (formSaleDetail.BatchProgramID2 != null && formSaleDetail.BatchProgramID2 != 0)
                        {
                            if (formSaleDetail.BatchProgramID1 != formSaleDetail.BatchProgramID2)
                            {
                                afpCheck = db.ApplyForPrograms.FirstOrDefault(a => a.ProgramID == formSaleDetail.BatchProgramID2 && a.FormNo == formSaleDetail.FormNo);
                                if (afpCheck == null)
                                {
                                    afp.ProgramID = formSaleDetail.BatchProgramID2;
                                    afp.ProgramPriority = 2;
                                    db.ApplyForPrograms.Add(afp);
                                    db.SaveChanges();
                                }
                            }
                        }

                        if (formSaleDetail.BatchProgramID3 != null && formSaleDetail.BatchProgramID3 != 0)
                        {
                            if (formSaleDetail.BatchProgramID1 != formSaleDetail.BatchProgramID3 && formSaleDetail.BatchProgramID2 != formSaleDetail.BatchProgramID3)
                            {
                                afpCheck = db.ApplyForPrograms.FirstOrDefault(a => a.ProgramID == formSaleDetail.BatchProgramID3 && a.FormNo == formSaleDetail.FormNo);
                                if (afpCheck == null)
                                {
                                    afp.ProgramID = formSaleDetail.BatchProgramID3;
                                    afp.ProgramPriority = 3;
                                    db.ApplyForPrograms.Add(afp);
                                    db.SaveChanges();
                                }
                            }
                        }
                    }
                    else
                    {
                        ViewBag.MessageType = "error";
                        ViewBag.Message = "Please select Program Preference 1";
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
            ViewBag.TotalRecords = db.sp_GetFormSaleDatailForPage("", Convert.ToInt32(BatchID)).ToList().Count();
            model.plFormsale = db.sp_GetFormSaleDatailForPage("", Convert.ToInt32(BatchID)).ToList().ToPagedList(pageNumber, Convert.ToInt32(pageSize));
            
            model.SelectedFormSaleDetail = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc", formSaleDetail.IsActive);
            ViewBag.FormTypeID = new SelectList(db.FormTypes, "FormTypeID", "FormTypeName", formSaleDetail.FormTypeID);
            ViewBag.DepositTypeID = new SelectList(db.DepositTypes.Where(d => d.DepositTypeName != "Online"), "DepositTypeID", "DepositTypeName", formSaleDetail.DepositTypeID);
            ViewBag.BatchProgramID = new SelectList(db.GetBatchProgramNameConcat(BatchID, 0), "ID", "Name", formSaleDetail.BatchProgramID);
            ViewBag.BatchProgramID1 = new SelectList(db.GetBatchProgramNameConcat(BatchID, 0), "ID", "Name", formSaleDetail.BatchProgramID1);
            ViewBag.BatchProgramID2 = new SelectList(db.GetBatchProgramNameConcat(BatchID, 0), "ID", "Name", formSaleDetail.BatchProgramID2);
            ViewBag.BatchProgramID3 = new SelectList(db.GetBatchProgramNameConcat(BatchID, 0), "ID", "Name", formSaleDetail.BatchProgramID3);
            
            ViewBag.AccountID = new SelectList(db.Finance_Bank_Accounts, "Account_ID", "Account_No", formSaleDetail.AccountID);
            ViewBag.CountryCode = new SelectList(db.Countries, "CountryCode", "CountryCode");
            ViewBag.DegreeID = new SelectList(db.GetDegrees(2), "DegreeID", "DegreeName", formSaleDetail.DegreeID);
            ViewBag.Wavier_Discount = new SelectList(db.Options, "OptionDesc", "OptionDesc", formSaleDetail.Wavier_Discount);
            ViewBag.LevelID = new SelectList(db.Levels, "LevelID", "LevelName", formSaleDetail.LevelID);

            return View("Index", model);
        }

        public ActionResult Update(int? id, int? page, int? pageSize)
        {
            if (pageSize == null)
            {
                pageSize = 10;
            }
            ViewBag.pageSize = pageSize;
            int pageNumber = (page ?? 1);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            sp_GetFormSaleDatailForPage_Result formSaleDetail = db.sp_GetFormSaleDatailForPage(id.ToString(), 0).FirstOrDefault();
            if (formSaleDetail == null)
            {
                return HttpNotFound();
            }

            ViewBag.TotalRecords = db.sp_GetFormSaleDatailForPage("", Convert.ToInt32(BatchID)).ToList().Count();
            model.plFormsale = db.sp_GetFormSaleDatailForPage("", Convert.ToInt32(BatchID)).ToList().ToPagedList(pageNumber, Convert.ToInt32(pageSize));
            model.SelectedFormSaleDetail = formSaleDetail;
            model.DisplayMode = "ReadWrite";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc", formSaleDetail.IsActive);
            ViewBag.FormTypeID = new SelectList(db.FormTypes, "FormTypeID", "FormTypeName", formSaleDetail.FormTypeID);
            ViewBag.DepositTypeID = new SelectList(db.DepositTypes.Where(d => d.DepositTypeName != "Online"), "DepositTypeID", "DepositTypeName", formSaleDetail.DepositTypeID);
            ViewBag.BatchProgramID = new SelectList(db.GetBatchProgramNameConcat(BatchID, 0), "ID", "Name", formSaleDetail.BatchProgramID);
            ViewBag.BatchProgramID1 = new SelectList(db.GetBatchProgramNameConcat(BatchID, 0), "ID", "Name", formSaleDetail.BatchProgramID1);
            ViewBag.BatchProgramID2 = new SelectList(db.GetBatchProgramNameConcat(BatchID, 0), "ID", "Name", formSaleDetail.BatchProgramID2);
            ViewBag.BatchProgramID3 = new SelectList(db.GetBatchProgramNameConcat(BatchID, 0), "ID", "Name", formSaleDetail.BatchProgramID3);

            ViewBag.AccountID = new SelectList(db.Finance_Bank_Accounts, "Account_ID", "Account_No", formSaleDetail.AccountID);
            ViewBag.CountryCode = new SelectList(db.Countries, "CountryCode", "CountryCode");
            ViewBag.DegreeID = new SelectList(db.GetDegrees(2), "DegreeID", "DegreeName", formSaleDetail.DegreeID);
            ViewBag.Wavier_Discount = new SelectList(db.Options, "OptionDesc", "OptionDesc", formSaleDetail.Wavier_Discount);
            ViewBag.LevelID = new SelectList(db.Levels, "LevelID", "LevelName", formSaleDetail.LevelID);
            ViewBag.MessageType = "";
            ViewBag.Message = "";
            return View("Index", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(FormSaleDetail formSaleDetail, int? page, int? pageSize)
        {
            if (pageSize == null)
            {
                pageSize = 10;
            }
            ViewBag.pageSize = pageSize;
            int pageNumber = (page ?? 1);

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

                if (!ComparePriceWithWaiveOff(formSaleDetail.BatchProgramID1, formSaleDetail.Wavier_Discount))
                {
                    count++;
                    ErrorMessage += count + "-Waive Off/Discount should be less then Prospectus Fee.<br />";
                    ViewBag.MessageType = "error";
                }

                if (string.IsNullOrEmpty(ErrorMessage))
                {
                    try
                    {
                        db.Entry(formSaleDetail).State = EntityState.Modified;
                        formSaleDetail.ModifiedBy = Convert.ToInt32(Session["emp_id"]);
                        formSaleDetail.IsReceived = "Yes";

                        formSaleDetail.ReceiveDate = DateTime.Now.ToString();
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
                        ViewBag.Message = string.Concat(ex.Message, ", Inner Exception: " + ex.InnerException);
                        ModelState.AddModelError(string.Empty, ex.Message);
                    }
                }
                else
                {
                    ViewBag.MessageType = "error";
                    ViewBag.Message = ErrorMessage;
                }
                ApplyForProgram afp = new ApplyForProgram();
                ApplyForProgram afpCheck = new ApplyForProgram();
                List<ApplyForProgram> lstPrograms = db.ApplyForPrograms.Where(p => p.FormNo == formSaleDetail.FormNo).ToList();
                if (lstPrograms.Count > 0)
                {
                    db.ApplyForPrograms.RemoveRange(lstPrograms);
                    db.SaveChanges();
                }

                afp.FormNo = formSaleDetail.FormNo;
                afp.IsActive = "Yes";
                afp.CreatedBy = Convert.ToInt32(Session["emp_id"]);
                afp.CreatedOn = DateTime.Now;

                afpCheck = db.ApplyForPrograms.FirstOrDefault(a => a.ProgramID == formSaleDetail.BatchProgramID1 && a.FormNo == formSaleDetail.FormNo);

                if (formSaleDetail.BatchProgramID1 != null)
                {
                    if (formSaleDetail.BatchProgramID1 != 0)
                    {
                        afpCheck = db.ApplyForPrograms.FirstOrDefault(a => a.ProgramID == formSaleDetail.BatchProgramID1 && a.FormNo == formSaleDetail.FormNo);
                        if (afpCheck == null)
                        {
                            afp.ProgramID = formSaleDetail.BatchProgramID1;
                            afp.ProgramPriority = 1;
                            db.ApplyForPrograms.Add(afp);

                            //Program Preference 1 will be the Applicant program
                            FormSaleDetail app = db.FormSaleDetails.FirstOrDefault(a => a.FormNo == formSaleDetail.FormNo);
                            app.BatchProgramID = Convert.ToInt32(afp.ProgramID);
                            db.Entry(app).State = EntityState.Modified;
                            db.SaveChanges();
                        }

                        if (formSaleDetail.BatchProgramID2 != null && formSaleDetail.BatchProgramID2 != 0)
                        {
                            if (formSaleDetail.BatchProgramID1 != formSaleDetail.BatchProgramID2)
                            {
                                afpCheck = db.ApplyForPrograms.FirstOrDefault(a => a.ProgramID == formSaleDetail.BatchProgramID2 && a.FormNo == formSaleDetail.FormNo);
                                if (afpCheck == null)
                                {
                                    afp.ProgramID = formSaleDetail.BatchProgramID2;
                                    afp.ProgramPriority = 2;
                                    db.ApplyForPrograms.Add(afp);
                                    db.SaveChanges();
                                }
                            }
                        }

                        if (formSaleDetail.BatchProgramID3 != null && formSaleDetail.BatchProgramID3 != 0)
                        {
                            if (formSaleDetail.BatchProgramID1 != formSaleDetail.BatchProgramID3 && formSaleDetail.BatchProgramID2 != formSaleDetail.BatchProgramID3)
                            {
                                afpCheck = db.ApplyForPrograms.FirstOrDefault(a => a.ProgramID == formSaleDetail.BatchProgramID3 && a.FormNo == formSaleDetail.FormNo);
                                if (afpCheck == null)
                                {
                                    afp.ProgramID = formSaleDetail.BatchProgramID3;
                                    afp.ProgramPriority = 3;
                                    db.ApplyForPrograms.Add(afp);
                                    db.SaveChanges();
                                }
                            }
                        }
                    }
                    else
                    {
                        ViewBag.MessageType = "error";
                        ViewBag.Message = "Please select Program Preference 1";
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
                        ErrorMessage += string.Concat(count, "-", error.ErrorMessage, "\n");
                    }
                }
                ViewBag.MessageType = "error";
                ViewBag.Message = ErrorMessage;
            }
            ViewBag.TotalRecords = db.sp_GetFormSaleDatailForPage("", Convert.ToInt32(BatchID)).ToList().Count();
            model.plFormsale = db.sp_GetFormSaleDatailForPage("", Convert.ToInt32(BatchID)).ToList().ToPagedList(pageNumber, Convert.ToInt32(pageSize));
            model.SelectedFormSaleDetail = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc", formSaleDetail.IsActive);
            ViewBag.FormTypeID = new SelectList(db.FormTypes, "FormTypeID", "FormTypeName", formSaleDetail.FormTypeID);
            ViewBag.DepositTypeID = new SelectList(db.DepositTypes.Where(d => d.DepositTypeName != "Online"), "DepositTypeID", "DepositTypeName", formSaleDetail.DepositTypeID);
            ViewBag.BatchProgramID = new SelectList(db.GetBatchProgramNameConcat(BatchID, 0), "ID", "Name", formSaleDetail.BatchProgramID);
            ViewBag.BatchProgramID1 = new SelectList(db.GetBatchProgramNameConcat(BatchID, 0), "ID", "Name", formSaleDetail.BatchProgramID1);
            ViewBag.BatchProgramID2 = new SelectList(db.GetBatchProgramNameConcat(BatchID, 0), "ID", "Name", formSaleDetail.BatchProgramID2);
            ViewBag.BatchProgramID3 = new SelectList(db.GetBatchProgramNameConcat(BatchID, 0), "ID", "Name", formSaleDetail.BatchProgramID3);

            ViewBag.AccountID = new SelectList(db.Finance_Bank_Accounts, "Account_ID", "Account_No", formSaleDetail.AccountID);
            ViewBag.CountryCode = new SelectList(db.Countries, "CountryCode", "CountryCode");
            ViewBag.DegreeID = new SelectList(db.GetDegrees(2), "DegreeID", "DegreeName", formSaleDetail.DegreeID);
            ViewBag.Wavier_Discount = new SelectList(db.Options, "OptionDesc", "OptionDesc", formSaleDetail.Wavier_Discount);
            ViewBag.LevelID = new SelectList(db.Levels, "LevelID", "LevelName", formSaleDetail.LevelID);
            return View("Index", model);
        }

        public ActionResult Delete(int? id, int? page, int? pageSize)
        {
            if (pageSize == null)
            {
                pageSize = 10;
            }
            ViewBag.pageSize = pageSize;
            int pageNumber = (page ?? 1);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            sp_GetFormSaleDatailForPage_Result formSaleDetail = db.sp_GetFormSaleDatailForPage(id.ToString(), 0).FirstOrDefault();

            if (formSaleDetail == null)
            {
                return HttpNotFound();
            }

            ViewBag.TotalRecords = db.sp_GetFormSaleDatailForPage("", Convert.ToInt32(BatchID)).ToList().Count();
            model.plFormsale = db.sp_GetFormSaleDatailForPage("", Convert.ToInt32(BatchID)).ToList().ToPagedList(pageNumber, Convert.ToInt32(pageSize));
            model.SelectedFormSaleDetail = formSaleDetail;
            model.DisplayMode = "Delete";
            ViewBag.MessageType = "";
            ViewBag.Message = "";
            return View("Index", model);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id, int? page, int? pageSize)
        {
            if (pageSize == null)
            {
                pageSize = 10;
            }
            ViewBag.pageSize = pageSize;
            int pageNumber = (page ?? 1);
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
            
            ViewBag.TotalRecords = db.sp_GetFormSaleDatailForPage("", Convert.ToInt32(BatchID)).ToList().Count();
            model.plFormsale = db.sp_GetFormSaleDatailForPage("", Convert.ToInt32(BatchID)).ToList().ToPagedList(pageNumber, Convert.ToInt32(pageSize));
            model.SelectedFormSaleDetail = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc");
            ViewBag.FormTypeID = new SelectList(db.FormTypes, "FormTypeID", "FormTypeName");
            ViewBag.DepositTypeID = new SelectList(db.DepositTypes.Where(d => d.DepositTypeName != "Online"), "DepositTypeID", "DepositTypeName");
            ViewBag.BatchProgramID = new SelectList(db.GetBatchProgramNameConcat(BatchID, 0), "ID", "Name");
            ViewBag.BatchProgramID1 = new SelectList(db.GetBatchProgramNameConcat(BatchID, 0), "ID", "Name");
            ViewBag.BatchProgramID2 =new SelectList(db.GetBatchProgramNameConcat(BatchID, 0), "ID", "Name");
            ViewBag.BatchProgramID3 =new SelectList(db.GetBatchProgramNameConcat(BatchID, 0), "ID", "Name");
            ViewBag.BatchProgramID4 =new SelectList(db.GetBatchProgramNameConcat(BatchID, 0), "ID", "Name");
            ViewBag.AccountID = new SelectList(db.Finance_Bank_Accounts, "Account_ID", "Account_No");
            ViewBag.CountryCode = new SelectList(db.Countries, "CountryCode", "CountryCode");
            ViewBag.DegreeID = new SelectList(db.GetDegrees(2), "DegreeID", "DegreeName");
            ViewBag.Wavier_Discount = new SelectList(db.Options, "OptionDesc", "OptionDesc", "No");
            ViewBag.LevelID = new SelectList(db.Levels, "LevelID", "LevelName");
            return View("Index", model);
        }

        public ActionResult PostForm()
        {
            model.FormSaleDetails = db.sp_GetFormSaleDatailForPage("", Convert.ToInt32(BatchID)).ToList();
            model.SelectedFormSaleDetail = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.MessageType = "";
            ViewBag.Message = "";
            return View(model);
        }

        [HttpPost]
        public ActionResult PostForm(string Search, string StartDate, string EndDate)
        {
            model.FormSaleDetails = db.sp_GetFormSaleDatailForPage("", Convert.ToInt32(BatchID)).ToList();
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

            model.FormSaleDetails = db.sp_GetFormSaleDatailForPage("", Convert.ToInt32(BatchID)).ToList();
            model.SelectedFormSaleDetail = null;
            model.DisplayMode = "WriteOnly";

            return View("PostForm", model);
        }

        public ActionResult UpdateReciveForm(int? id)
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

            string ErrorMessage = "";
            int count = 0;
            try
            {

                db.Entry(formSaleDetail).State = EntityState.Modified;

                formSaleDetail.ModifiedBy = Convert.ToInt32(Session["emp_id"]);
                formSaleDetail.ReceiveDate = DateTime.Now.ToString("dd-MM-yyyy");
                formSaleDetail.IsReceived = "Yes";
                db.SaveChanges();

                //formSaleDetail.FormPrice = "1500";
                //InsertFormDetail(formSaleDetail, ref ErrorMessage, ref count);
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
            model.FormSaleDetails = db.sp_GetFormSaleDatailForPage("", Convert.ToInt32(BatchID)).ToList();

            model.SelectedFormSaleDetail = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc", formSaleDetail.IsActive);
            ViewBag.FormTypeID = new SelectList(db.FormTypes, "FormTypeID", "FormTypeName", formSaleDetail.FormTypeID);
            ViewBag.DepositTypeID = new SelectList(db.DepositTypes.Where(d => d.DepositTypeName != "Online"), "DepositTypeID", "DepositTypeName", formSaleDetail.DepositTypeID);
            ViewBag.BatchProgramID = new SelectList(db.GetBatchProgramNameConcat(BatchID, 0), "ID", "Name", formSaleDetail.BatchProgramID);
            ViewBag.BatchProgramID1 = new SelectList(db.GetBatchProgramNameConcat(BatchID, 0), "ID", "Name");
            ViewBag.BatchProgramID2 = new SelectList(db.GetBatchProgramNameConcat(BatchID, 0), "ID", "Name");
            ViewBag.BatchProgramID3 = new SelectList(db.GetBatchProgramNameConcat(BatchID, 0), "ID", "Name");
            
            ViewBag.AccountID = new SelectList(db.Finance_Bank_Accounts, "Account_ID", "Account_No", formSaleDetail.AccountID);
            ViewBag.CountryCode = new SelectList(db.Countries, "CountryCode", "CountryCode");
            ViewBag.DegreeID = new SelectList(db.GetDegrees(2), "DegreeID", "DegreeName", formSaleDetail.DegreeID);
            ViewBag.Wavier_Discount = new SelectList(db.Options, "OptionDesc", "OptionDesc", "No");
            ViewBag.LevelID = new SelectList(db.Levels, "LevelID", "LevelName", formSaleDetail.LevelID);
            return RedirectToAction("Index");
        }

        public JsonResult GetPrograms_by_FacultyLevelBatch(string LevelID)
        {
            List<GetPrograms_by_FacultyLevelBatch_Result> lstPrograms = new List<GetPrograms_by_FacultyLevelBatch_Result>();
            Batch batch = db.Batches.FirstOrDefault(b => b.IsActive == "Current");
            if (batch != null)
            {
                BatchID = batch.BatchID.ToString();
            }

            lstPrograms = db.GetPrograms_by_FacultyLevelBatch(0, Convert.ToInt32(LevelID), Convert.ToInt32(BatchID), 0).ToList();
            var programs = lstPrograms.Select(p => new
            {
                BatchProgramID = p.BatchProgramID,
                ProgramName = p.ProgramName
            });
            string result = JsonConvert.SerializeObject(programs, Formatting.Indented);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetSelectedPrograms(string FormNo)
        {
            List<ApplyForProgram> lstPrograms = new List<ApplyForProgram>();
            lstPrograms = db.ApplyForPrograms.Where(p => p.FormNo == FormNo).ToList();
            var programs = lstPrograms.Select(afp => new
            {
                ProgramPriority = afp.ProgramPriority,
                ProgramID = afp.ProgramID
            });

            string result = JsonConvert.SerializeObject(programs, Formatting.Indented);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public bool ComparePriceWithWaiveOff(int? BatchProgramID, string waiveOff)
        {
            bool flag = true;
            BatchProgram bp = db.BatchPrograms.FirstOrDefault(bprog => bprog.BatchProgramID == BatchProgramID);
            Program p = db.Programs.FirstOrDefault(prog => prog.ProgramID == bp.ProgramID);
            if(string.IsNullOrEmpty(waiveOff))
            {
                waiveOff = "0";
            }

            if(Convert.ToInt32(waiveOff) > Convert.ToInt32(p.ProspectusFee))
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