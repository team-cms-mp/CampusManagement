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
    [Authorize]
    public class ApplicantUploadsAdminController : Controller
    {
        int count = 0;
        string ErrorMessage = "";
        private ModelCMSNewContainer db = new ModelCMSNewContainer();
        ApplicantUploadViewModel model = new ApplicantUploadViewModel();

        public ActionResult Index(string FormNo)
        {
            int EmpID = Convert.ToInt32(Session["emp_id"]);
            if (EmpID != 0)
            {
                FormNo = Session["FormNo"].ToString();
                Applicant app = db.Applicants.FirstOrDefault(a => a.FormNo == FormNo);
                if (app == null)
                {
                    return RedirectToAction("Create", "OnlineApply");
                }

                ApplicantQualification appq = db.ApplicantQualifications.FirstOrDefault(a => a.FormNo == FormNo);
                if (appq == null)
                {
                    return RedirectToAction("Index", "OnlineApplyQualifications");
                }
            }
            else
            {
                return RedirectToAction("Login2", "Home");
            }

            model.ApplicantUploads = db.ApplicantUploads.Where(b => b.FormNo == FormNo).OrderByDescending(a => a.ApplicantUploadID).ToList();
            model.SelectedApplicantUpload = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc");
            ViewBag.DocumentName = new SelectList(db.DocumentCheckLists, "CheckListDescription", "CheckListDescription");
            ViewBag.MessageType = "";
            ViewBag.FormNo = FormNo;
            ViewBag.Message = "";
            return View(model);
        }

        [HttpGet]
        public ActionResult Create(string FormNo)
        {
            int EmpID = Convert.ToInt32(Session["emp_id"]);
            if(EmpID == 0)
            {
                return RedirectToAction("Login2", "Home");
            }

            FormNo = Session["FormNo"].ToString();
            
            model.ApplicantUploads = db.ApplicantUploads.Where(b => b.FormNo == FormNo).OrderByDescending(a => a.ApplicantUploadID).ToList();
            model.SelectedApplicantUpload = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc");
            ViewBag.DocumentName = new SelectList(db.DocumentCheckLists, "CheckListDescription", "CheckListDescription");
            ViewBag.MessageType = "";
            ViewBag.FormNo = FormNo;
            ViewBag.Message = "";
            return View("Index", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ApplicantUpload ApplicantUpload, string FormNo)
        {
            int EmpID = Convert.ToInt32(Session["emp_id"]);
            if (EmpID == 0)
            {
                return RedirectToAction("Login2", "Home");
            }

            FormNo = Session["FormNo"].ToString();

            try
            {
                ApplicantUpload.UploadPath = string.Concat("~/DegreeDocument/", DateTime.Now.Ticks, "_", ApplicantUpload.ApplicantUploadFile.FileName.Replace(" ", ""));
                ApplicantUpload.ApplicantUploadFile.SaveAs(Server.MapPath(ApplicantUpload.UploadPath));
            }
            catch (Exception)
            {
                ModelState.AddModelError(string.Empty, "-Please attach the document.");
                count++;
                ErrorMessage += count + "-Please attach the document.<br />";
            }

            try
            {
                ApplicantUpload ba = db.ApplicantUploads.FirstOrDefault(bac => bac.DocumentName == ApplicantUpload.DocumentName && bac.FormNo == FormNo);
                if (ba == null)
                {
                    ApplicantUpload.CreatedBy = Convert.ToInt32(Session["emp_id"]);
                    ApplicantUpload.CreatedOn = DateTime.Now;
                    ApplicantUpload.FormNo = FormNo;
                    db.ApplicantUploads.Add(ApplicantUpload);
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
                        ViewBag.Message = ex.Message;
                        ModelState.AddModelError(string.Empty, ex.Message);
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Degree Name already exists.");
                    ViewBag.MessageType = "error";
                    ViewBag.Message = "Degree Name already exists.";
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
                ViewBag.FormNo = FormNo;
                ViewBag.Message = ErrorMessage;
            }
            model.ApplicantUploads = db.ApplicantUploads.Where(b => b.FormNo == FormNo).OrderByDescending(a => a.ApplicantUploadID).ToList();
            model.SelectedApplicantUpload = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc", ApplicantUpload.IsActive);
            ViewBag.DocumentName = new SelectList(db.DocumentCheckLists, "CheckListDescription", "CheckListDescription", ApplicantUpload.DocumentName);
            ViewBag.FormNo = FormNo;
            return View("Index", model);
        }

        public ActionResult Update(int? id)
        {
            string FormNo = "";

            int EmpID = Convert.ToInt32(Session["emp_id"]);
            if (EmpID == 0)
            {
                return RedirectToAction("Login2", "Home");
            }

            FormNo = Session["FormNo"].ToString();

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ApplicantUpload ApplicantUpload = db.ApplicantUploads.Find(id);
            if (ApplicantUpload == null)
            {
                return HttpNotFound();
            }

            model.ApplicantUploads = db.ApplicantUploads.Where(b => b.FormNo == ApplicantUpload.FormNo).OrderByDescending(a => a.ApplicantUploadID).ToList();
            model.SelectedApplicantUpload = ApplicantUpload;
            model.DisplayMode = "ReadWrite";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc", ApplicantUpload.IsActive);
            ViewBag.DocumentName = new SelectList(db.DocumentCheckLists, "CheckListDescription", "CheckListDescription", ApplicantUpload.DocumentName);
            ViewBag.MessageType = "";
            ViewBag.FormNo = ApplicantUpload.FormNo;
            ViewBag.Message = "";
            return View("Index", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ApplicantUpload ApplicantUpload, string FormNo)
        {
            int EmpID = Convert.ToInt32(Session["emp_id"]);
            if (EmpID == 0)
            {
                return RedirectToAction("Login2", "Home");
            }

            FormNo = Session["FormNo"].ToString();

            try
            {
                if (ApplicantUpload.ApplicantUploadFile != null)
                {
                    ApplicantUpload.UploadPath = string.Concat("~/DegreeDocument/", DateTime.Now.Ticks, "_", ApplicantUpload.ApplicantUploadFile.FileName.Replace(" ", ""));
                    ApplicantUpload.ApplicantUploadFile.SaveAs(Server.MapPath(ApplicantUpload.UploadPath));
                }
            }
            catch (Exception)
            {
                ModelState.AddModelError(string.Empty, "-Please attach the document.");
                count++;
                ErrorMessage += count + "-Please attach the document.<br />";
            }

            try
            {
                db.Entry(ApplicantUpload).State = EntityState.Modified;
                ApplicantUpload.ModifiedBy = Convert.ToInt32(Session["emp_id"]);
                ApplicantUpload.ModifiedOn = DateTime.Now;
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
            model.ApplicantUploads = db.ApplicantUploads.Where(b => b.FormNo == FormNo).OrderByDescending(a => a.ApplicantUploadID).ToList();
            model.SelectedApplicantUpload = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.FormNo = FormNo;
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc", ApplicantUpload.IsActive);
            ViewBag.DocumentName = new SelectList(db.DocumentCheckLists, "CheckListDescription", "CheckListDescription", ApplicantUpload.DocumentName);

            return View("Index", model);
        }

        public ActionResult Delete(int? id)
        {
            int EmpID = Convert.ToInt32(Session["emp_id"]);
            if (EmpID == 0)
            {
                return RedirectToAction("Login2", "Home");
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ApplicantUpload ApplicantUpload = db.ApplicantUploads.Find(id);
            if (ApplicantUpload == null)
            {
                return HttpNotFound();
            }

            model.ApplicantUploads = db.ApplicantUploads.Where(b => b.FormNo == ApplicantUpload.FormNo).OrderByDescending(a => a.ApplicantUploadID).ToList();
            model.SelectedApplicantUpload = ApplicantUpload;
            model.DisplayMode = "Delete";
            ViewBag.MessageType = "";
            ViewBag.Message = "";
            ViewBag.FormNo = ApplicantUpload.FormNo;
            return View("Index", model);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ApplicantUpload applicantUpload = db.ApplicantUploads.Find(id);
            try
            {
                db.ApplicantUploads.Remove(applicantUpload);
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
            model.ApplicantUploads = db.ApplicantUploads.Where(b => b.FormNo == applicantUpload.FormNo).OrderByDescending(a => a.ApplicantUploadID).ToList();
            model.SelectedApplicantUpload = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.FormNo = applicantUpload.FormNo;
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc");
            ViewBag.DocumentName = new SelectList(db.DocumentCheckLists, "CheckListDescription", "CheckListDescription");

            return View("Index", model);
        }


        public ActionResult DocumentChecklist(string FormNo)
        {
            ViewBag.FormNo = FormNo;

            return View();
        }

        [HttpPost]
        public ActionResult DocumentChecklist(FormCollection fc)
        {
            int count = Convert.ToInt32(fc["hdnRowCount"]);
            int DocumentCheckListID = 0;
            string chkIsChecked = "";
            string FormNo = fc["FormNo"];

            List<ApplicantDocumentCheckList> lstADCL = new List<ApplicantDocumentCheckList>();

            for (int i = 1; i <= count; i++)
            {
                ApplicantDocumentCheckList documentCheckList = new ApplicantDocumentCheckList();
                DocumentCheckListID = Convert.ToInt32(fc["DocumentCheckListID_" + i]);
                chkIsChecked = fc["chkIsChecked_" + i];

                ApplicantDocumentCheckList documentList = db.ApplicantDocumentCheckLists.FirstOrDefault(x => x.DocumentCheckListID == DocumentCheckListID && x.FormNo == FormNo);
                if (chkIsChecked != null)
                {
                    documentCheckList.FormNo = FormNo;
                    documentCheckList.DocumentCheckListID = Convert.ToInt32(DocumentCheckListID);
                    documentCheckList.IsChecked = "Yes";
                    documentCheckList.CreatedOn = DateTime.Now;
                    documentCheckList.CreatedBy = Convert.ToInt32(Session["emp_id"]);
                    documentCheckList.IsActive = "Yes";
                    if (documentList == null)
                    {
                        lstADCL.Add(documentCheckList);
                    }
                    else
                    {
                        db.Entry(documentList).State = EntityState.Modified;
                        documentList.ModifiedOn = DateTime.Now;
                        documentList.ModifiedBy = Convert.ToInt32(Session["emp_id"]);
                        documentList.IsChecked = "Yes";
                        db.SaveChanges();
                    }
                }
                else
                {
                    if (documentList != null)
                    {
                        db.Entry(documentList).State = EntityState.Modified;
                        documentList.ModifiedOn = DateTime.Now;
                        documentList.ModifiedBy = Convert.ToInt32(Session["emp_id"]);
                        documentList.IsChecked = "No";
                        db.SaveChanges();
                    }
                }
            }

            db.ApplicantDocumentCheckLists.AddRange(lstADCL);
            db.SaveChanges();
            ViewBag.FormNo = FormNo;
            return View();
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
