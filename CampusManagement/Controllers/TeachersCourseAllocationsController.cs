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
using CampusManagement.App_Code;
using Newtonsoft.Json;

namespace CampusManagement.Controllers
{
    [Authorize]
    public class TeachersCourseAllocationsController : Controller
    {
        private ModelCMSNewContainer db = new ModelCMSNewContainer();
        TeachersCourseAllocationsViewModel model = new TeachersCourseAllocationsViewModel();

        public ActionResult Index()
        {
            model.TeachersCourseAllocations = db.TeachersCourseAllocations.OrderByDescending(t => t.TCourseAllocationID).ToList();
            model.SelectedTeachersCourseAllocation = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc");
            ViewBag.ProgramCourseID = new SelectList(db.GetBatchProgramNameConcat("", 1), "ID", "Name");
            
            ViewBag.MessageType = "";
            ViewBag.Message = "";
            return View(model);
        }

        [HttpGet]
        public ActionResult Create()
        {
            model.TeachersCourseAllocations = db.TeachersCourseAllocations.OrderByDescending(t => t.TCourseAllocationID).ToList();
            model.SelectedTeachersCourseAllocation = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc");
            ViewBag.ProgramCourseID = new SelectList(db.GetBatchProgramNameConcat("", 1), "ID", "Name");
            
            ViewBag.MessageType = "";
            ViewBag.Message = "";
            return View("Index", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(TeachersCourseAllocation teachersCourseAllocation)
        {
            string ErrorMessage = "";
            int count = 0;
            try
            {
                TeachersCourseAllocation tc = db.TeachersCourseAllocations.FirstOrDefault(
                    p => p.ProgramCourseID == teachersCourseAllocation.ProgramCourseID
                    && p.TeacherID == teachersCourseAllocation.TeacherID);

                if (tc != null)
                {
                    ViewBag.MessageType = "error";
                    ViewBag.Message = "Selected Subject is already assigned to the selected Lecturer.";
                }
                else
                {
                    teachersCourseAllocation.CreatedBy = Convert.ToInt32(Session["emp_id"]);
                    teachersCourseAllocation.CreatedOn = DateTime.Now;
                    db.TeachersCourseAllocations.Add(teachersCourseAllocation);
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
            model.TeachersCourseAllocations = db.TeachersCourseAllocations.OrderByDescending(t => t.TCourseAllocationID).ToList();
            model.SelectedTeachersCourseAllocation = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc", teachersCourseAllocation.IsActive);
            ViewBag.ProgramCourseID = new SelectList(db.GetBatchProgramNameConcat("", 1), "ID", "Name", teachersCourseAllocation.ProgramCourseID);
            return View("Index", model);
        }

        public ActionResult Update(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            TeachersCourseAllocation teachersCourseAllocation = db.TeachersCourseAllocations.Find(id);
            if (teachersCourseAllocation == null)
            {
                return HttpNotFound();
            }

            model.TeachersCourseAllocations = db.TeachersCourseAllocations.OrderByDescending(t => t.TCourseAllocationID).ToList();
            model.SelectedTeachersCourseAllocation = teachersCourseAllocation;
            model.DisplayMode = "ReadWrite";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc", teachersCourseAllocation.IsActive);
            ViewBag.ProgramCourseID = new SelectList(db.GetBatchProgramNameConcat("", 1), "ID", "Name", teachersCourseAllocation.ProgramCourseID);

            ViewBag.MessageType = "";
            ViewBag.Message = "";
            return View("Index", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(TeachersCourseAllocation teachersCourseAllocation)
        {
            try
            {
                db.Entry(teachersCourseAllocation).State = EntityState.Modified;
                teachersCourseAllocation.ModifiedBy = Convert.ToInt32(Session["emp_id"]);
                teachersCourseAllocation.ModifiedOn = DateTime.Now;
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
            model.TeachersCourseAllocations = db.TeachersCourseAllocations.OrderByDescending(t => t.TCourseAllocationID).ToList();
            model.SelectedTeachersCourseAllocation = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc", teachersCourseAllocation.IsActive);
            ViewBag.ProgramCourseID = new SelectList(db.GetBatchProgramNameConcat("", 1), "ID", "Name", teachersCourseAllocation.ProgramCourseID);
            return View("Index", model);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TeachersCourseAllocation teachersCourseAllocation = db.TeachersCourseAllocations.Find(id);
            if (teachersCourseAllocation == null)
            {
                return HttpNotFound();
            }

            model.TeachersCourseAllocations = db.TeachersCourseAllocations.OrderByDescending(t => t.TCourseAllocationID).ToList();
            model.SelectedTeachersCourseAllocation = teachersCourseAllocation;
            ViewBag.ProgramCourseID = new SelectList(db.GetBatchProgramNameConcat("", 1), "ID", "Name", teachersCourseAllocation.ProgramCourseID);
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
                TeachersCourseAllocation teachersCourseAllocation = db.TeachersCourseAllocations.Find(id);
                db.TeachersCourseAllocations.Remove(teachersCourseAllocation);
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
            model.TeachersCourseAllocations = db.TeachersCourseAllocations.OrderByDescending(t => t.TCourseAllocationID).ToList();
            model.SelectedTeachersCourseAllocation = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc");
            ViewBag.ProgramCourseID = new SelectList(db.GetBatchProgramNameConcat("", 1), "ID", "Name");
            
            return View("Index", model);
        }

        public JsonResult GetSubDepartment()
        {
            List<GetSubDepartments_by_HospitalID_Result> lstDept = new List<GetSubDepartments_by_HospitalID_Result>();

            lstDept = CommonFunctions.GetSubDepartments(MvcApplication.Hospital_ID).ToList();
            var depts = lstDept.Select(S => new
            {
                SubDept_Id = S.SubDept_Id,
                SubDept_Name = S.SubDept_Name,
            });
            string result = JsonConvert.SerializeObject(depts, Formatting.Indented);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult um_GetEmployeesForSubjectAllocation(int SubDeptId)
        {
            List<um_GetEmployeesForSubjectAllocation_Result> lstEmp = new List<um_GetEmployeesForSubjectAllocation_Result>();

            lstEmp = db.um_GetEmployeesForSubjectAllocation(1).Where(e => e.SubDeptId == SubDeptId).ToList();
            var emps = lstEmp.Select(S => new
            {
                EmpID = S.EmpID,
                EmployeeName = S.EmployeeName,
            });
            string result = JsonConvert.SerializeObject(emps, Formatting.Indented);
            return Json(result, JsonRequestBehavior.AllowGet);
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
