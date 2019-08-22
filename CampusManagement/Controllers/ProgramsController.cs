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
    public class ProgramsController : Controller
    {
        private ModelCMSNewContainer db = new ModelCMSNewContainer();
        ProgramsViewModel model = new ProgramsViewModel();

        public ActionResult Index()
        {
            model.Programs = db.SP_ProgramForPage("").ToList();
            model.SelectedProgram = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.FacultyID = new SelectList(db.GetSubDepartments_by_HospitalID(MvcApplication.Hospital_ID), "SubDept_Id", "SubDept_Name");
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc");
            ViewBag.ProgramTypeID = new SelectList(db.ProgramTypes, "ProgramTypeID", "ProgramTypeName");
            ViewBag.LevelID = new SelectList(db.Levels, "LevelID", "LevelName");
            ViewBag.MessageType = "";
            ViewBag.Message = "";
            return View(model);
        }

        [HttpGet]
        public ActionResult Create()
        {
            model.Programs = db.SP_ProgramForPage("").ToList();
            model.SelectedProgram = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.FacultyID = new SelectList(db.GetSubDepartments_by_HospitalID(MvcApplication.Hospital_ID), "SubDept_Id", "SubDept_Name");
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc");
            ViewBag.ProgramTypeID = new SelectList(db.ProgramTypes, "ProgramTypeID", "ProgramTypeName");
            ViewBag.LevelID = new SelectList(db.Levels, "LevelID", "LevelName");
            ViewBag.MessageType = "";
            ViewBag.Message = "";
            return View("Index", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Program program)
        {
            string ErrorMessage = "";
            int count = 0;
            try
            {
                Program p = new Program();
                p = db.Programs.FirstOrDefault(c => c.ProgramName == program.ProgramName);
                if (p != null)
                {
                    ModelState.AddModelError(string.Empty, "Program Name already exists.");
                    count++;
                    ErrorMessage += count + "-" + "Program Name already exists." + "<br />";
                }

                p = db.Programs.FirstOrDefault(c => c.ProgramCode == program.ProgramCode);
                if (p != null)
                {
                    ModelState.AddModelError(string.Empty, "Program Code already exists.");
                    count++;
                    ErrorMessage += count + "-" + "Program Code already exists." + "<br />";
                }

                if (!string.IsNullOrEmpty(ErrorMessage))
                {
                    ViewBag.MessageType = "error";
                    ViewBag.Message = ErrorMessage;
                }

                if (p == null)
                {
                    program.CreatedBy = Convert.ToInt32(Session["emp_id"]);
                    program.CreatedOn = DateTime.Now;
                    db.Programs.Add(program);
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
             model.Programs = db.SP_ProgramForPage("").ToList();
            model.SelectedProgram = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.FacultyID = new SelectList(db.GetSubDepartments_by_HospitalID(MvcApplication.Hospital_ID), "SubDept_Id", "SubDept_Name", program.FacultyID);
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc", program.IsActive);
            ViewBag.ProgramTypeID = new SelectList(db.ProgramTypes, "ProgramTypeID", "ProgramTypeName", program.ProgramTypeID);
            ViewBag.LevelID = new SelectList(db.Levels, "LevelID", "LevelName", program.LevelID);

            return View("Index", model);
        }

        public ActionResult Update(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            SP_ProgramForPage_Result program = db.SP_ProgramForPage("").FirstOrDefault(s => s.ProgramID == id);
            if (program == null)
            {
                return HttpNotFound();
            }
            model.Programs = db.SP_ProgramForPage("").ToList();
            model.SelectedProgram = program;
            model.DisplayMode = "ReadWrite";
            ViewBag.FacultyID = new SelectList(db.GetSubDepartments_by_HospitalID(MvcApplication.Hospital_ID), "SubDept_Id", "SubDept_Name", program.FacultyID);
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc", program.IsActive);
            ViewBag.ProgramTypeID = new SelectList(db.ProgramTypes, "ProgramTypeID", "ProgramTypeName", program.ProgramTypeID);
            ViewBag.LevelID = new SelectList(db.Levels, "LevelID", "LevelName", program.LevelID);
            ViewBag.MessageType = "";
            ViewBag.Message = "";
            return View("Index", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Program program)
        {
            try
            {
                db.Entry(program).State = EntityState.Modified;
                program.ModifiedBy = Convert.ToInt32(Session["emp_id"]);
                program.ModifiedOn = DateTime.Now;
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
            model.Programs = db.SP_ProgramForPage("").ToList();
           
            model.SelectedProgram = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.FacultyID = new SelectList(db.GetSubDepartments_by_HospitalID(MvcApplication.Hospital_ID), "SubDept_Id", "SubDept_Name", program.FacultyID);
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc", program.IsActive);
            ViewBag.ProgramTypeID = new SelectList(db.ProgramTypes, "ProgramTypeID", "ProgramTypeName", program.ProgramTypeID);
            ViewBag.LevelID = new SelectList(db.Levels, "LevelID", "LevelName", program.LevelID);
            return View("Index", model);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            SP_ProgramForPage_Result program = db.SP_ProgramForPage("").FirstOrDefault(s => s.ProgramID == id);
            if(program==null)
            {
                return HttpNotFound();
            }

            model.Programs = db.SP_ProgramForPage("").ToList();
            model.SelectedProgram = program;
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
                Program program = db.Programs.Find(id);
                db.Programs.Remove(program);
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
            model.Programs = db.SP_ProgramForPage("").ToList();
            model.SelectedProgram = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.FacultyID = new SelectList(db.GetSubDepartments_by_HospitalID(MvcApplication.Hospital_ID), "SubDept_Id", "SubDept_Name");
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc");
            ViewBag.ProgramTypeID = new SelectList(db.ProgramTypes, "ProgramTypeID", "ProgramTypeName");
            ViewBag.LevelID = new SelectList(db.Levels, "LevelID", "LevelName");
            return View("Index", model);
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
