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
    [Authorize(Roles = "Account Officer,Accounts Officer,Admin Assistant,Admin Officer,Admin.Assistant,Assist. Account Officer,Assist.Technician,Import Manager,Manager Servive & Support,Office Manager,Officer QMS,RSM - Center 2,RSM - South,Sales & Service Executive,Sales Executive,Sales Manager,Sales Representative,Sr.Accounts Officer,Sr.Associate Engineer,Sr.Sales Executive,Sr.Sales Representative,Store Assistant,Store Incharge,Technician")]
    public class DepartmentsController : Controller
    {
        private ModelCMSContainer db = new ModelCMSContainer();
        DepartmentsViewModel model = new DepartmentsViewModel();

        public ActionResult Index()
        {
            model.Departments = db.Departments.OrderByDescending(a=>a.DepartmentID).ToList();
            model.SelectedDepartment = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc");
            ViewBag.MessageType = "";
            ViewBag.Message = "";
            return View(model);
        }

        [HttpPost]
        public ActionResult Index(string Search)
        {
            var lst = db.Departments.Where(x => x.DepartmentName.Contains(Search)).OrderByDescending(a => a.DepartmentID).ToList();
            model.Departments = lst;
            model.SelectedDepartment = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc");
            ViewBag.MessageType = "";
            ViewBag.Message = "";
            return View(model);
        }

        [HttpGet]
        public ActionResult Create()
        {
            model.Departments = db.Departments.OrderByDescending(a=>a.DepartmentID).ToList();
            model.SelectedDepartment = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc");
            ViewBag.MessageType = "";
            ViewBag.Message = "";
            return View("Index", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Department department)
        {
            try
            {
                Department d = db.Departments.FirstOrDefault(de => de.DepartmentName == department.DepartmentName);
                if (d == null)
                {
                    department.CreatedBy = Convert.ToInt32(Session["emp_id"]);
                    department.CreatedOn = DateTime.Now;
                    db.Departments.Add(department);
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
                    ModelState.AddModelError(string.Empty, "Department Name is already exists.");
                    ViewBag.MessageType = "error";
                    ViewBag.Message = "Department Name is already exists.";
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
            model.Departments = db.Departments.OrderByDescending(a=>a.DepartmentID).ToList();
            model.SelectedDepartment = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc", department.IsActive);

            return View("Index", model);
        }

        public ActionResult Update(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Department department = db.Departments.Find(id);
            if (department == null)
            {
                return HttpNotFound();
            }

            model.Departments = db.Departments.OrderByDescending(a=>a.DepartmentID).ToList();
            model.SelectedDepartment = department;
            model.DisplayMode = "ReadWrite";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc", department.IsActive);
            ViewBag.MessageType = "";
            ViewBag.Message = "";
            return View("Index", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Department department)
        {
            try
            {
                db.Entry(department).State = EntityState.Modified;
                department.ModifiedBy = Convert.ToInt32(Session["emp_id"]);
                department.ModifiedOn = DateTime.Now;
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
            model.Departments = db.Departments.OrderByDescending(a=>a.DepartmentID).ToList();
            model.SelectedDepartment = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc", department.IsActive);

            return View("Index", model);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Department department = db.Departments.Find(id);
            if (department == null)
            {
                return HttpNotFound();
            }

            model.Departments = db.Departments.OrderByDescending(a=>a.DepartmentID).ToList();
            model.SelectedDepartment = department;
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
                Department department = db.Departments.Find(id);
                db.Departments.Remove(department);
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
            model.Departments = db.Departments.OrderByDescending(a=>a.DepartmentID).ToList();
            model.SelectedDepartment = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc");

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
