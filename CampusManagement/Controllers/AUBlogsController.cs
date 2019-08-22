using CampusManagement.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace CampusManagement.Controllers
{
    public class AUBlogsController : Controller
    {
        private ModelCMSNewContainer db = new ModelCMSNewContainer();
        AlumniUserBlogViewModel model = new AlumniUserBlogViewModel();

        public ActionResult Index()
        {
            int CurrentUserID = Convert.ToInt32(Session["CurrentUserID"]);
            model.AlumniUserBlogs = db.AlumniUserBlogs.OrderByDescending(a => a.AlumniUserBlogID).ToList();
            model.SelectedAlumniUserBlogs = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc");
            ViewBag.MessageType = "";
            ViewBag.AlumniUserID = CurrentUserID;
            ViewBag.Message = "";
            return View(model);
        }

        [HttpGet]
        public ActionResult Create()
        {
            int CurrentUserID = Convert.ToInt32(Session["CurrentUserID"]);
            model.AlumniUserBlogs = db.AlumniUserBlogs.OrderByDescending(a => a.AlumniUserBlogID).ToList();
            model.SelectedAlumniUserBlogs = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.AlumniUserID = CurrentUserID;
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc");
            ViewBag.MessageType = "";
            ViewBag.Message = "";
            return View("Index", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(AlumniUserBlog AUR)
        {
            try
            {
                AlumniUserBlog d = db.AlumniUserBlogs.FirstOrDefault(de => de.BlogTitle == AUR.BlogTitle);

                if (d == null)
                {

                    // AUR.CreatedBy = Convert.ToInt32(Session["emp_id"]);
                    AUR.CreatedBy = Convert.ToInt32(Session["CurrentUserID"]);
                    AUR.CreatedOn = DateTime.Now;
                    db.AlumniUserBlogs.Add(AUR);
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
                    ModelState.AddModelError(string.Empty, "Name already exists.");
                    ViewBag.MessageType = "error";
                    ViewBag.Message = "Name already exists.";
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
            ViewBag.AlumniUserID = AUR.AlumniUserID;
            model.AlumniUserBlogs = db.AlumniUserBlogs.OrderByDescending(a => a.AlumniUserID).ToList();
            model.SelectedAlumniUserBlogs = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc", AUR.IsActive);

            return View("Index", model);
        }

        public ActionResult Update(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            AlumniUserBlog AUR = db.AlumniUserBlogs.Find(id);
            if (AUR == null)
            {
                return HttpNotFound();
            }

            model.AlumniUserBlogs = db.AlumniUserBlogs.OrderByDescending(a => a.AlumniUserID).ToList();
            model.SelectedAlumniUserBlogs = AUR;
            model.DisplayMode = "ReadWrite";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc", AUR.IsActive);
            ViewBag.AlumniUserID = AUR.AlumniUserID;
            ViewBag.MessageType = "";
            ViewBag.Message = "";
            return View("Index", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(AlumniUserBlog AUR)
        {
            try
            {
                db.Entry(AUR).State = EntityState.Modified;
                //AUR.ModifiedBy = Convert.ToInt32(Session["emp_id"]);
                AUR.ModifiedBy = Convert.ToInt32(Session["CurrentUserID"]);
                AUR.ModifiedOn = DateTime.Now;
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
            model.AlumniUserBlogs = db.AlumniUserBlogs.OrderByDescending(a => a.AlumniUserID).ToList();
            model.SelectedAlumniUserBlogs = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc", AUR.IsActive);

            return View("Index", model);
        }


        public ActionResult GetAlumniUserBlogs(GetAlumniUserBlog_Result alumi)
        {
            GetAlumniUserBlog_ResultViewModel model = new GetAlumniUserBlog_ResultViewModel();
            model.GetAlumniUserBlog_Results = db.GetAlumniUserBlog().ToList();
            model.SelectedGetAlumniUserBlog_Results = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.MessageType = "";
            ViewBag.Message = "";
            return View(model);
        }

        public ActionResult GetAlumniUserBlogCommentByBlogID(int AlumniUserBlogID)
        {
            GetAlumniUserBlogCommentByBlogID_ResultViewModel model = new GetAlumniUserBlogCommentByBlogID_ResultViewModel();
            model.AlumniUserBlogObj = db.GetAlumniUserBlogByID(AlumniUserBlogID).FirstOrDefault();
            model.GetAlumniUserBlogCommentByBlogID_Results = db.GetAlumniUserBlogCommentByBlogID(AlumniUserBlogID).OrderByDescending(x => x.CreatedOn).ToList();
            model.SelectedGetAlumniUserBlogCommentByBlogID_Results = null;
            ViewBag.AlumniUserBlogID = AlumniUserBlogID;
            model.AlumniUserBlogID = AlumniUserBlogID;
            model.DisplayMode = "WriteOnly";
            ViewBag.MessageType = "";
            ViewBag.Message = "";
            return View(model);
        }

        [HttpPost]
        public ActionResult GetAlumniUserBlogCommentByBlogID(AlumniUserBlogComment GAUC, int AlumniUserBlogID, string txtBlogCommet)
        {

            try
            {
                AlumniUserBlogComment d = db.AlumniUserBlogComments.FirstOrDefault(de => de.BlogComment == GAUC.BlogComment);

                if (d == null)
                {
                    GAUC.BlogComment = txtBlogCommet;
                    //GAUC.CreatedBy = Convert.ToInt32(Session["emp_id"]);
                    //GAUC.CommentUserID = Convert.ToInt32(Session["emp_id"]);
                    GAUC.CreatedBy = Convert.ToInt32(Session["CurrentUserID"]);
                    GAUC.CommentUserID = Convert.ToInt32(Session["CurrentUserID"]);
                    GAUC.CreatedOn = DateTime.Now;
                    GAUC.IsActive = "Yes";
                    db.AlumniUserBlogComments.Add(GAUC);
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
                    ModelState.AddModelError(string.Empty, "Name already exists.");
                    ViewBag.MessageType = "error";
                    ViewBag.Message = "Name already exists.";
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
            GetAlumniUserBlogCommentByBlogID_ResultViewModel model = new GetAlumniUserBlogCommentByBlogID_ResultViewModel();
            model.AlumniUserBlogObj = db.GetAlumniUserBlogByID(GAUC.AlumniUserBlogID).FirstOrDefault();
            model.GetAlumniUserBlogCommentByBlogID_Results = db.GetAlumniUserBlogCommentByBlogID(GAUC.AlumniUserBlogID).OrderByDescending(x => x.CreatedOn).ToList();
            model.SelectedGetAlumniUserBlogCommentByBlogID_Results = null;
            ViewBag.AlumniUserBlogID = AlumniUserBlogID;
            model.DisplayMode = "WriteOnly";
            ViewBag.MessageType = "";
            ViewBag.Message = "";
            return View(model);
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