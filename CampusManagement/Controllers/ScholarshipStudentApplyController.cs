using CampusManagement.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.IO;

namespace CampusManagement.Controllers
{
    public class ScholarshipStudentApplyController : Controller
    {
        int count = 0;
        string ErrorMessage = "";
        private ModelCMSNewContainer db = new ModelCMSNewContainer();
        ScholarshipApplicantViewModel model = new ScholarshipApplicantViewModel();
        ScholarshipApplicantDocumentViewModel model1 = new ScholarshipApplicantDocumentViewModel();
        public ActionResult Index(int? id)
        {
          int CurrentUserID =  Convert.ToInt32(Session["CurrentUserID"]);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            model.SO = db.ScholarshipOpportunities.Find(id);
            model.SA = db.ScholarshipApplicants.Where(x=> x.ScholarshipOpportunitiesID == id && x.StudentID == CurrentUserID).FirstOrDefault();
            model.DocumentList = db.GetScholarshipDocumentByStudentID(id, CurrentUserID).ToList();
            
            ViewBag.ScholarshipOpportunitiesID = id;

            if (model.SO == null)
            {
                return HttpNotFound();
            }

            return View(model);
        }
        public ActionResult UploadDocuments(int? ScholarshipApplicantID, int? ScholarshipDocumentID, int? ScholarshipOpportunitiesID)
        {

            model1.ScholarshipApplicantDocuments = db.ScholarshipApplicantDocuments.Where(de => de.ScholarshipApplicantID == ScholarshipApplicantID && de.ScholarshipDocumentID== ScholarshipDocumentID).ToList();
            model1.SelectedScholarshipApplicantDocument = null;
            model1.DisplayMode = "WriteOnly";
            ViewBag.MessageType = "";
            ViewBag.Message = "";
            ViewBag.ScholarshipDocumentID = ScholarshipDocumentID;
            ViewBag.ScholarshipApplicantID = ScholarshipApplicantID;
            ViewBag.ScholarshipOpportunitiesID = ScholarshipOpportunitiesID;
            return View(model1);
        }

        [HttpGet]
        public ActionResult Create(int? ScholarshipApplicantID, int? ScholarshipDocumentID, int? ScholarshipOpportunitiesID)
        {
            model1.ScholarshipApplicantDocuments = db.ScholarshipApplicantDocuments.ToList();
            model1.SelectedScholarshipApplicantDocument = null;
            model1.DisplayMode = "WriteOnly";
            ViewBag.ScholarshipDocumentID = ScholarshipDocumentID;
            ViewBag.ScholarshipApplicantID = ScholarshipApplicantID;
            ViewBag.ScholarshipOpportunitiesID = ScholarshipOpportunitiesID;
            ViewBag.MessageType = "";
            ViewBag.Message = "";
            return View("UploadDocuments", model1);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ScholarshipApplicantDocument ScholarshipApplicantDocument,  FormCollection fc)
        {
            int? ScholarshipApplicantID = 0;
            int? ScholarshipDocumentID = 0;
            int? ScholarshipOpportunitiesID = 0;

            try
            {
                ScholarshipApplicantID = Convert.ToInt32(fc["ScholarshipApplicantID"]);
                ScholarshipDocumentID = Convert.ToInt32(fc["ScholarshipDocumentID"]);
                ScholarshipOpportunitiesID = Convert.ToInt32(fc["ScholarshipOpportunitiesID"]);
                ViewBag.ScholarshipDocumentID = ScholarshipDocumentID;
                ViewBag.ScholarshipApplicantID = ScholarshipApplicantID;
                ViewBag.ScholarshipOpportunitiesID = ScholarshipOpportunitiesID;
                ScholarshipApplicantDocument d = db.ScholarshipApplicantDocuments.FirstOrDefault(de => de.ScholarshipApplicantDocumentID == ScholarshipApplicantDocument.ScholarshipApplicantDocumentID);
                if (d == null)

                {
                    try
                    {
                        ScholarshipApplicantDocument.DocumentPath = string.Concat("~/DegreeDocument/", DateTime.Now.Ticks, "_", ScholarshipApplicantDocument.UploadFiles.FileName.Replace(" ", ""));
                        ScholarshipApplicantDocument.UploadFiles.SaveAs(Server.MapPath(ScholarshipApplicantDocument.DocumentPath));
                    }
                    catch (Exception)
                    {
                        ModelState.AddModelError(string.Empty, "-Please attach the document.");
                        count++;
                        ErrorMessage += count + "-Please attach the document.<br />";
                    }
                    int CurrentUserID = Convert.ToInt32(Session["CurrentUserID"]);
                    // ScholarshipApplicantDocument.CreatedBy =  Convert.ToInt32(Session["emp_id"]);
                    ScholarshipApplicantDocument.CreatedBy = CurrentUserID;
                    ScholarshipApplicantDocument.CreatedOn = DateTime.Now;
                    db.ScholarshipApplicantDocuments.Add(ScholarshipApplicantDocument);
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
                    ModelState.AddModelError(string.Empty, "Scholarship opportunity already exists.");
                    ViewBag.MessageType = "error";
                    ViewBag.Message = "Scholarship opportunity already exists.";
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
            model1.ScholarshipApplicantDocuments = db.ScholarshipApplicantDocuments.Where(de => de.ScholarshipApplicantID == ScholarshipApplicantID && de.ScholarshipDocumentID == ScholarshipDocumentID ).ToList();
            model1.SelectedScholarshipApplicantDocument = null;
            model1.DisplayMode = "WriteOnly";

            return View("UploadDocuments", model1);
        }

        public ActionResult Delete(int? id, int? ScholarshipApplicantID, int? ScholarshipDocumentID, int? ScholarshipOpportunitiesID)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ScholarshipApplicantDocument ScholarshipApplicantDocument = db.ScholarshipApplicantDocuments.Find(id);
            if (ScholarshipApplicantDocument == null)
            {
                return HttpNotFound();
            }

            model1.ScholarshipApplicantDocuments = db.ScholarshipApplicantDocuments.Where(de => de.ScholarshipApplicantID == ScholarshipApplicantID && de.ScholarshipDocumentID == ScholarshipDocumentID).OrderByDescending(a => a.ScholarshipApplicantDocumentID).ToList();
            model1.SelectedScholarshipApplicantDocument = ScholarshipApplicantDocument;
            model1.DisplayMode = "Delete";
            ViewBag.MessageType = "";
            ViewBag.Message = "";
            ViewBag.ScholarshipDocumentID = ScholarshipDocumentID;
            ViewBag.ScholarshipApplicantID = ScholarshipApplicantID;
            ViewBag.ScholarshipOpportunitiesID = ScholarshipOpportunitiesID;
            return View("UploadDocuments", model1);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id, int? ScholarshipApplicantID, int? ScholarshipDocumentID, int? ScholarshipOpportunitiesID)
        {
            try
            {
                ViewBag.ScholarshipDocumentID = ScholarshipDocumentID;
                ViewBag.ScholarshipApplicantID = ScholarshipApplicantID;
                ViewBag.ScholarshipOpportunitiesID = ScholarshipOpportunitiesID;
                ScholarshipApplicantDocument ScholarshipApplicantDocument = db.ScholarshipApplicantDocuments.Find(id);
                db.ScholarshipApplicantDocuments.Remove(ScholarshipApplicantDocument);
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
            model1.ScholarshipApplicantDocuments = db.ScholarshipApplicantDocuments.Where(de => de.ScholarshipApplicantID == ScholarshipApplicantID && de.ScholarshipDocumentID == ScholarshipDocumentID ).OrderByDescending(a => a.ScholarshipApplicantDocumentID).ToList();
            model1.SelectedScholarshipApplicantDocument = null;
            model1.DisplayMode = "WriteOnly";


            return View("UploadDocuments", model1);
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