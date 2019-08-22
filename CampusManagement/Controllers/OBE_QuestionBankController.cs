using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CampusManagement.Models;
using Newtonsoft.Json;
using PagedList;
using System.Globalization;

using CampusManagement.App_Code;
using System.Configuration;
using System.IO;
using System.Net.Mail;


namespace CampusManagement.Controllers
{
    public class OBE_QuestionBankController : Controller
    {
        private ModelCMSContainer db = new ModelCMSContainer();
        QuestionBank ModelObj = new QuestionBank();


        // GET: ObeProgramPLO
        [HttpGet]
        public ActionResult Index(int? page, int? pageSize, string QuestionID, string QuestionName,string QuestionTypeName, string LevelName,  string message)
        {
            if (pageSize == null)
            {
                pageSize = 10;
            }
            ViewBag.pageSize = pageSize;
            int pageNumber = (page ?? 1);

            if (QuestionID == null)
            {
                QuestionID = "0";
            }
            if (LevelName == null || LevelName == "")
            {
                LevelName = "";
            }

            if (QuestionTypeName == null || QuestionTypeName == "")
            {
                QuestionTypeName = "";
            }
            if (QuestionName == null || QuestionName == "")
            {
                QuestionName = "";
            }



            //ViewBag.CourseID = new SelectList(GetCourceslist(), "CourseID", "CourseName", CourseID);
            ViewBag.LevelID = new SelectList(db.OBE_Level, "LevelID", "LevelName");
            ViewBag.QuestionTypeID = new SelectList(db.OBE_QuestionType, "QuestionTypeID", "QuestionTypeName");
            ViewBag.hdnQuestionID = QuestionID;
            ViewBag.hdnQuestionName = QuestionName;
            return View(db.GetAllOBE_QuestionBank(Convert.ToInt32(QuestionID), LevelName, QuestionTypeName, QuestionName, "").OrderByDescending(a=>a.QuestionID).ToList().ToPagedList(pageNumber, Convert.ToInt32(pageSize)));
        }

        [HttpPost]
        public ActionResult Index(int? page, string QuestionID, string QuestionName, string QuestionTypeName, string LevelName, string CLOCode)
        {
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            if (QuestionID == null)
            {
                QuestionID = "0";
            }
            if (LevelName == null || LevelName == "")
            {
                LevelName = "";
            }

            if (QuestionTypeName == null || QuestionTypeName == "")
            {
                QuestionTypeName = "";
            }

            if (QuestionName == null || QuestionName == "")
            {
                QuestionName = "";
            }

            
            ViewBag.hdnQuestionID = QuestionID;
            ViewBag.hdnQuestionName = QuestionName;
            ViewBag.LevelID = new SelectList(db.OBE_Level, "LevelID", "LevelName");
            ViewBag.QuestionTypeID = new SelectList(db.OBE_QuestionType, "QuestionTypeID", "QuestionTypeName");
            //ViewBag.CourseID = new SelectList(GetCourceslist(), "CourseID", "CourseName", CourseID);
            return View(db.GetAllOBE_QuestionBank(Convert.ToInt32(QuestionID), LevelName, QuestionTypeName, QuestionName, "").OrderByDescending(a => a.QuestionID).ToList().ToPagedList(pageNumber, Convert.ToInt32(pageSize)));

        }

        [HttpGet]
        public ActionResult Create(int? QuestionID)
        {

            //ViewBag.CourseID = new SelectList(GetCourcesWithOutAll(), "CourseID", "CourseName");
            ViewBag.LevelID = new SelectList(db.OBE_Level, "LevelID", "LevelName");
            ViewBag.QuestionTypeID = new SelectList(db.OBE_QuestionType, "QuestionTypeID", "QuestionTypeName");
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc");
            return View(ModelObj);
        }

        [HttpPost]
        public ActionResult Create(QuestionBank obj)
        {
            //ViewBag.CourseID = new SelectList(GetCourcesWithOutAll(), "CourseID", "CourseName", obj.CourseID);
            ViewBag.LevelID = new SelectList(db.OBE_Level, "LevelID", "LevelName", obj.LevelID);
            ViewBag.QuestionTypeID = new SelectList(db.OBE_QuestionType, "QuestionTypeID", "QuestionTypeName", obj.QuestionTypeID);
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc", obj.IsActive);
            int QuestionID = Convert.ToInt32(db.InsertOrUpdateOBE_QuestionBank(
           obj.QuestionID,
           obj.LevelID,
           obj.QuestionTypeID,
           obj.QuestionName,
        
           obj.Description,
           null,
           Convert.ToInt32(Session["emp_id"]),
           obj.IsActive,
           null,
           Convert.ToInt32(Session["emp_id"])).FirstOrDefault());
            if (QuestionID > 0)
            {
                ViewBag.MessageType = "success";
                ViewBag.Message = "Data has been saved successfully.";
            }
            else
            {
                ViewBag.MessageType = "error";
                ViewBag.Message = "Error while executing query, please try again";
            }

            return View(obj);
        }



        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            QuestionBank questionbank = db.QuestionBanks.Find(id);
            if (questionbank == null)
            {
                return HttpNotFound();
            }
           
            else
            {
                ModelObj = db.QuestionBanks.Where(c => c.QuestionID == id).FirstOrDefault();
            }
            ViewBag.LevelID = new SelectList(db.OBE_Level, "LevelID", "LevelName");
            ViewBag.QuestionTypeID = new SelectList(db.OBE_QuestionType, "QuestionTypeID", "QuestionTypeName");
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc");
            ViewBag.MessageType = "";
            ViewBag.Message = "";
            return View( ModelObj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(QuestionBank questionbank)
        {
            try
            {
                db.Entry(questionbank).State = EntityState.Modified;
                questionbank.ModifiedBy = Convert.ToInt32(Session["emp_id"]);
                questionbank.ModifiedOn = DateTime.Now;
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
            ViewBag.LevelID = new SelectList(db.OBE_Level, "LevelID", "LevelName",questionbank.LevelID);
            ViewBag.QuestionTypeID = new SelectList(db.OBE_QuestionType, "QuestionTypeID", "QuestionTypeName",questionbank.QuestionTypeID);
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc", questionbank.IsActive);
            return RedirectToAction("Index", ModelObj);
            //return View("Index", model);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            QuestionBank questionbank = db.QuestionBanks.Find(id);
            if (questionbank == null)
            {
                return HttpNotFound();
            }
            else
            {
                ModelObj= db.QuestionBanks.Where(c => c.QuestionID == id).FirstOrDefault();
            }

            //model.BankAccounts = db.BankAccounts.OrderByDescending(a => a.BankAccountID).ToList();
            //model.SelectedBankAccount = bankaccount;
            //model.DisplayMode = "Delete";
            ViewBag.LevelID = new SelectList(db.OBE_Level, "LevelID", "LevelName", questionbank.LevelID);
            ViewBag.QuestionTypeID = new SelectList(db.OBE_QuestionType, "QuestionTypeID", "QuestionTypeName", questionbank.QuestionTypeID);
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc" ,questionbank.IsActive);

            ViewBag.MessageType = "";
            ViewBag.Message = "";
            return View(ModelObj);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                QuestionBank questionbank = db.QuestionBanks.Find(id);
                db.QuestionBanks.Remove(questionbank);
                db.SaveChanges();
                ViewBag.MessageType = "success";
                ViewBag.Message = "Record has been removed successfully.";
                ViewBag.LevelID = new SelectList(db.OBE_Level, "LevelID", "LevelName", questionbank.LevelID);
                ViewBag.QuestionTypeID = new SelectList(db.OBE_QuestionType, "QuestionTypeID", "QuestionTypeName", questionbank.QuestionTypeID);
                ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc", questionbank.IsActive);
            }
            catch (DbUpdateException ex)
            {
                ViewBag.MessageType = "error";
                ViewBag.Message = ex.Message;
                ModelState.AddModelError(string.Empty, ex.Message);
            }
            //model.BankAccounts = db.BankAccounts.OrderByDescending(a => a.BankAccountID).ToList();
            //model.SelectedBankAccount = null;
            //model.DisplayMode = "WriteOnly";

            return RedirectToAction("Index", ModelObj);
            //return View("Index", model);
        }

        public JsonResult GetCources()
        {
            List<Course> lstcources = new List<Course>();
            Course CourseObj = new Course();
            CourseObj.CourseID = 0;
            CourseObj.CourseName = "--ALL--";
            lstcources = db.Courses.ToList();
            lstcources.Insert(0, CourseObj);
            var Courses = lstcources.Select(p => new
            {
                CourseID = p.CourseID,
                CourseName = p.CourseName
            });
            string result = JsonConvert.SerializeObject(Courses, Formatting.Indented);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public List<Course> GetCourceslist()
        {
            List<Course> lstPrograms = new List<Course>();
            Course CourseObj = new Course();
            CourseObj.CourseID = 0;
            CourseObj.CourseName = "--ALL--";
            lstPrograms = db.Courses.ToList();
            lstPrograms.Insert(0, CourseObj);

            return lstPrograms;
        }

        public List<Course> GetCourcesWithOutAll()
        {
            List<Course> lstCourse = new List<Course>();
            lstCourse = db.Courses.ToList();
            return lstCourse;
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
