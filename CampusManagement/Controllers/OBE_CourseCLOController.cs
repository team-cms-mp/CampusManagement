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
    public class OBE_CourseCLOController : Controller
    {
        private ModelCMSNewContainer db = new ModelCMSNewContainer();
        OBE_CourseCLO ModelObj = new OBE_CourseCLO();


        // GET: ObeProgramPLO
        [HttpGet]
        public ActionResult Index(int? page, int? pageSize, string CourseID, string CLOName, string CLOCode, string message)
        {
            if (pageSize == null)
            {
                pageSize = 10;
            }
            ViewBag.pageSize = pageSize;
            int pageNumber = (page ?? 1);

            if (CourseID == null)
            {
                CourseID = "0";
            }
            if (CLOName == null || CLOName == "")
            {
                CLOName = "";
            }

            if (CLOCode == null || CLOCode == "")
            {
                CLOCode = "";
            }

            ViewBag.CourseID = new SelectList(GetCourceslist(), "CourseID", "CourseName", CourseID);
            ViewBag.hdnCourseID = CourseID;
            ViewBag.hdnCLOName = CLOName;
            ViewBag.hdnCLOCode = CLOCode;
            return View(db.GetAllOBE_CourseCLO(Convert.ToInt32(CourseID), CLOCode, CLOName, "").ToList().ToPagedList(pageNumber, Convert.ToInt32(pageSize)));
        }

        [HttpPost]
        public ActionResult Index(int? page, string CourseID, string CLOName, string CLOCode)
        {
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            if (CourseID == null)
            {
                CourseID = "0";
            }
            if (CLOName == null || CLOName == "")
            {
                CLOName = "";
            }

            if (CLOCode == null || CLOCode == "")
            {
                CLOCode = "";
            }
            ViewBag.hdnCourseID = CourseID;
            ViewBag.hdnCLOName = CLOName;
            ViewBag.hdnCLOCode = CLOCode;
            ViewBag.CourseID = new SelectList(GetCourceslist(), "CourseID", "CourseName", CourseID);
            return View(db.GetAllOBE_CourseCLO(Convert.ToInt32(CourseID), CLOCode, CLOName, "").ToList().ToPagedList(pageNumber, Convert.ToInt32(pageSize)));

        }

        [HttpGet]
        public ActionResult Create(int? CLOID)
        {
            if (CLOID == null || CLOID == 0) {
            } else {
                ModelObj = db.OBE_CourseCLO.Where(c => c.CLOID == CLOID).FirstOrDefault();
            }
            ViewBag.CourseID = new SelectList(GetCourcesWithOutAll(), "CourseID", "CourseName");
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc");
            return View(ModelObj);
        }

        [HttpPost]
        public ActionResult Create(OBE_CourseCLO obj)
        {
            ViewBag.CourseID = new SelectList(GetCourcesWithOutAll(), "CourseID", "CourseName", obj.CourseID);
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc", obj.IsActive);
            //InsertOrUpdateOBE_CourseCLO_Result objresult = new InsertOrUpdateOBE_CourseCLO_Result();
            int CLOID = Convert.ToInt32 (db.InsertOrUpdateOBE_CourseCLO(obj.CLOID,
            obj.CourseID,
            obj.CLOCode,
            obj.CLOName,

            obj.Description,

            null,
            Convert.ToInt32(Session["emp_id"]),
            obj.IsActive,
            null,
            Convert.ToInt32(Session["emp_id"])).FirstOrDefault());
            if (CLOID > 0)
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

            OBE_CourseCLO obe_course = db.OBE_CourseCLO.Find(id);
            if (obe_course == null)
            {
                return HttpNotFound();
            }

            else
            {
                ModelObj = db.OBE_CourseCLO.Where(c => c.CLOID == id).FirstOrDefault();
            }
            ViewBag.CourseID = new SelectList(GetCourcesWithOutAll(), "CourseID", "CourseName", obe_course.CourseID);
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc", obe_course.IsActive);
            ViewBag.MessageType = "";
            ViewBag.Message = "";
            return View(ModelObj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(OBE_CourseCLO obe_course)
        {
            try
            {
                db.Entry(obe_course).State = EntityState.Modified;
                obe_course.ModifiedBy = Convert.ToInt32(Session["emp_id"]);
                obe_course.ModifiedOn = DateTime.Now;
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
            ViewBag.CourseID = new SelectList(GetCourcesWithOutAll(), "CourseID", "CourseName", obe_course.CourseID);
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc", obe_course.IsActive);
            return RedirectToAction("Index", ModelObj);
            //return View("Index", model);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OBE_CourseCLO obe_course = db.OBE_CourseCLO.Find(id);
            if (obe_course == null)
            {
                return HttpNotFound();
            }
            else
            {
                ModelObj = db.OBE_CourseCLO.Where(c => c.CLOID == id).FirstOrDefault();
            }

            //model.BankAccounts = db.BankAccounts.OrderByDescending(a => a.BankAccountID).ToList();
            //model.SelectedBankAccount = bankaccount;
            //model.DisplayMode = "Delete";
            ViewBag.CourseID = new SelectList(GetCourcesWithOutAll(), "CourseID", "CourseName", obe_course.CourseID);
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc", obe_course.IsActive);

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
                OBE_CourseCLO obe_course = db.OBE_CourseCLO.Find(id);
                db.OBE_CourseCLO.Remove(obe_course);
                db.SaveChanges();
                ViewBag.MessageType = "success";
                ViewBag.Message = "Record has been removed successfully.";
                ViewBag.CourseID = new SelectList(GetCourcesWithOutAll(), "CourseID", "CourseName", obe_course.CLOID);
                ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc", obe_course.IsActive);
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
