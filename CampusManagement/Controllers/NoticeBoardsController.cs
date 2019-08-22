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
    [Authorize]
    public class NoticeBoardsController : Controller
    {
        private ModelCMSNewContainer db = new ModelCMSNewContainer();
        NoticeBoardViewModel model = new NoticeBoardViewModel();

        public ActionResult Index()
        {
            model.NoticeBoards = db.GetNoticeBoardDetails("").OrderByDescending(a => a.NoticeBoardID).ToList();
            model.SelectedNoticeBoard = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.FacultyID = new SelectList(db.GetSubDepartments_by_HospitalID(MvcApplication.Hospital_ID), "SubDept_Id", "SubDept_Name");
            ViewBag.NoticeTypeID = new SelectList(db.NoticeTypes, "NoticeTypeID", "NoticeTypeName");
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc");
            ViewBag.MessageType = "";
            ViewBag.Message = "";
            return View(model);
        }

        [HttpGet]
        public ActionResult Create()
        {
            model.NoticeBoards = db.GetNoticeBoardDetails("").OrderByDescending(a => a.NoticeBoardID).ToList();
            model.SelectedNoticeBoard = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.FacultyID = new SelectList(db.GetSubDepartments_by_HospitalID(MvcApplication.Hospital_ID), "SubDept_Id", "SubDept_Name");
            ViewBag.NoticeTypeID = new SelectList(db.NoticeTypes, "NoticeTypeID", "NoticeTypeName");
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc");
            ViewBag.MessageType = "";
            ViewBag.Message = "";
            return View("Index", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(NoticeBoard NoticeBoard)
        {
            try
            {
                NoticeBoard ba = db.NoticeBoards.FirstOrDefault(bac => bac.NoticeBoardID == NoticeBoard.NoticeBoardID);
                if (ba == null)
                {
                    NoticeBoard.CreatedBy = Convert.ToInt32(Session["emp_id"]);
                    NoticeBoard.CreatedOn = DateTime.Now;
                    db.NoticeBoards.Add(NoticeBoard);
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
                    ModelState.AddModelError(string.Empty, "Notice # already exists.");
                    ViewBag.MessageType = "error";
                    ViewBag.Message = "Notice # already exists.";
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
            model.NoticeBoards = db.GetNoticeBoardDetails("").OrderByDescending(a => a.NoticeBoardID).ToList();
            model.SelectedNoticeBoard = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.FacultyID = new SelectList(db.GetSubDepartments_by_HospitalID(MvcApplication.Hospital_ID), "SubDept_Id", "SubDept_Name");
            ViewBag.NoticeTypeID = new SelectList(db.NoticeTypes, "NoticeTypeID", "NoticeTypeName");
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc", NoticeBoard.IsActive);

            return View("Index", model);
        }

        public ActionResult Update(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            GetNoticeBoardDetails_Result NoticeBoard = db.GetNoticeBoardDetails("").FirstOrDefault(x=> x.NoticeBoardID == id);
            if (NoticeBoard == null)
            {
                return HttpNotFound();
            }

            model.NoticeBoards = db.GetNoticeBoardDetails("").OrderByDescending(a => a.NoticeBoardID).ToList();
            model.SelectedNoticeBoard = NoticeBoard;
            model.DisplayMode = "ReadWrite";
            ViewBag.FacultyID = new SelectList(db.GetSubDepartments_by_HospitalID(MvcApplication.Hospital_ID), "SubDept_Id", "SubDept_Name");
            ViewBag.NoticeTypeID = new SelectList(db.NoticeTypes, "NoticeTypeID", "NoticeTypeName");
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc", NoticeBoard.IsActive);
            ViewBag.MessageType = "";
            ViewBag.Message = "";
            return View("Index", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(NoticeBoard NoticeBoard)
        {
            try
            {
                db.Entry(NoticeBoard).State = EntityState.Modified;
                NoticeBoard.ModifiedBy = Convert.ToInt32(Session["emp_id"]);
                NoticeBoard.ModifiedOn = DateTime.Now;
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
            model.NoticeBoards = db.GetNoticeBoardDetails("").OrderByDescending(a => a.NoticeBoardID).ToList();
            model.SelectedNoticeBoard = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc", NoticeBoard.IsActive);
            ViewBag.FacultyID = new SelectList(db.GetSubDepartments_by_HospitalID(MvcApplication.Hospital_ID), "SubDept_Id", "SubDept_Name");
            ViewBag.NoticeTypeID = new SelectList(db.NoticeTypes, "NoticeTypeID", "NoticeTypeName");
            return View("Index", model);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GetNoticeBoardDetails_Result NoticeBoard = db.GetNoticeBoardDetails("").FirstOrDefault(x => x.NoticeBoardID == id);
            if (NoticeBoard == null)
            {
                return HttpNotFound();
            }

            model.NoticeBoards = db.GetNoticeBoardDetails("").OrderByDescending(a => a.NoticeBoardID).ToList();
            model.SelectedNoticeBoard = NoticeBoard;
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
                NoticeBoard NoticeBoard = db.NoticeBoards.Find(id);
                db.NoticeBoards.Remove(NoticeBoard);
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
            model.NoticeBoards = db.GetNoticeBoardDetails("").OrderByDescending(a => a.NoticeBoardID).ToList();
            model.SelectedNoticeBoard = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc");
            ViewBag.FacultyID = new SelectList(db.GetSubDepartments_by_HospitalID(MvcApplication.Hospital_ID), "SubDept_Id", "SubDept_Name");
            ViewBag.NoticeTypeID = new SelectList(db.NoticeTypes, "NoticeTypeID", "NoticeTypeName");
            return View("Index", model);
        }

        public ActionResult ShowNoticeBoard()
        {
            ViewBag.NoticeTypeID = new SelectList(db.NoticeTypes, "NoticeTypeID", "NoticeTypeName");
            ViewBag.FacultyID = new SelectList(db.GetSubDepartments_by_HospitalID(MvcApplication.Hospital_ID), "SubDept_Id", "SubDept_Name");

            return View(db.GetNoticBoardLists(0, 0).OrderByDescending(x => x.NoticeTypeName).ToList());
        }

        [HttpPost]
        public ActionResult ShowNoticeBoard(int? NoticeTypeID, int? FacultyID)
        {
            ViewBag.NoticeTypeID = new SelectList(db.NoticeTypes, "NoticeTypeID", "NoticeTypeName", NoticeTypeID);
            ViewBag.FacultyID = new SelectList(db.GetSubDepartments_by_HospitalID(MvcApplication.Hospital_ID), "SubDept_Id", "SubDept_Name", FacultyID);
            return View(db.GetNoticBoardLists(NoticeTypeID, FacultyID).OrderByDescending(x => x.NoticeTypeName).ToList());
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