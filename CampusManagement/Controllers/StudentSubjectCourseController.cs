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
    public class StudentSubjectCourseController : Controller
    {
        private ModelCMSNewContainer db = new ModelCMSNewContainer();
        BatchProgramCourseViewModel model = new BatchProgramCourseViewModel();
        

        public ActionResult Index( )
        {
           



            model.BatchProgramCourses = db.BatchProgramCourses.OrderByDescending(a=>a.BatchProgramID).ToList();
            model.StudentBatchProgramCourses = db.StudentBatchProgramCourses.OrderByDescending(a => a.BatchProgramID).ToList();
            model.DisplayMode = "WriteOnly";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc");
            ViewBag.BatchProgramID = new SelectList(db.GetBatchProgramNameConcat("", 0), "ID", "Name");
            ViewBag.Semester = new SelectList(db.Semesters, "SemesterID", "SemesterName");
            ViewBag.StatusID = new SelectList(db.Status.Where(s => s.StatusName == "Closed" || s.StatusName == "Pending Further Investigation" || s.StatusName == "Under Investigation"), "StatusID", "StatusName");
            ViewBag.MessageType = "";
            ViewBag.Message = "";
            return View(model);
        }

        [HttpGet]
        public ActionResult Create()
        {
            model.BatchProgramCourses = db.BatchProgramCourses.OrderByDescending(a => a.BatchProgramID).ToList();
            model.StudentBatchProgramCourses = db.StudentBatchProgramCourses.OrderByDescending(a => a.BatchProgramID).ToList();
            model.BatchProgramCourses = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc");

            ViewBag.BatchProgramID = new SelectList(db.GetBatchProgramNameConcat("", 0), "ID", "Name");
            ViewBag.Semester = new SelectList(db.Semesters, "SemesterID", "SemesterName");
            ViewBag.StatusID = new SelectList(db.Status.Where(s => s.StatusName == "Closed" || s.StatusName == "Pending Further Investigation" || s.StatusName == "Under Investigation"), "StatusID", "StatusName");
            ViewBag.MessageType = "";
            ViewBag.Message = "";
            return View("Index", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Discipline discipline)
        {
            try
            {
                Discipline d = db.Disciplines.FirstOrDefault(de => de.DisciplineDate == discipline.DisciplineDate);
                if (d == null)
                {
                    discipline.CreatedBy = Convert.ToInt32(Session["emp_id"]);
                    discipline.CreatedOn = DateTime.Now;
                    db.Disciplines.Add(discipline);
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
                    ModelState.AddModelError(string.Empty, "Discipline Name already exists.");
                    ViewBag.MessageType = "error";
                    ViewBag.Message = "Discipline Name already exists.";
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

            model.BatchProgramCourses = db.BatchProgramCourses.OrderByDescending(a => a.BatchProgramID).ToList();
            model.BatchProgramCourses = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc");

            ViewBag.BatchProgramID = new SelectList(db.GetBatchProgramNameConcat("", 0), "ID", "Name");
            ViewBag.Semester = new SelectList(db.Semesters, "SemesterID", "SemesterName");

            return RedirectToAction("Index", model);
        }
       
        public ActionResult AddSubject(StudentBatchProgramCourse sbpc, string  CourseName, int BatchProgramID, int ProgramCourseID, int CourseID, int YearSemesterNo)
        {
            try
            {
                StudentBatchProgramCourse d = db.StudentBatchProgramCourses.FirstOrDefault(de => de.BatchProgramID == sbpc.BatchProgramID && de.CourseID == CourseID);

                if (d == null)
                {
                    sbpc.CreatedBy = Convert.ToInt32(Session["emp_id"]);
                    sbpc.CreatedOn = DateTime.Now;
                    db.StudentBatchProgramCourses.Add(sbpc);
                    ViewBag.MessageType = "success";
                    ViewBag.Message = "Record has been removed successfully.";
                    try
                    {
                        db.SaveChanges();
                      
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
                    ViewBag.MessageType = "error";
                    ViewBag.Message = "Subject is already assigned.";
                    ModelState.AddModelError(string.Empty, "Subject is already assigned.");
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

            model.BatchProgramCourses = db.BatchProgramCourses.OrderByDescending(a => a.BatchProgramID).ToList();
            model.BatchProgramCourses = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.Message = "";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc");

            
            return RedirectToAction("Index", model);
        }

        public ActionResult DeleteSubject(int StudentBatchProgramCourseID)
        {
            try
            {
                StudentBatchProgramCourse sbsc = db.StudentBatchProgramCourses.FirstOrDefault(s => s.StudentBatchProgramCourseID == StudentBatchProgramCourseID);
                db.StudentBatchProgramCourses.Remove(sbsc);

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

            model.BatchProgramCourses = db.BatchProgramCourses.OrderByDescending(a => a.BatchProgramID).ToList();
            model.StudentBatchProgramCourses = db.StudentBatchProgramCourses.OrderByDescending(a => a.BatchProgramID).ToList();
            model.DisplayMode = "WriteOnly";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc");

            ViewBag.BatchProgramID = new SelectList(db.GetBatchProgramNameConcat("", 0), "ID", "Name");
            ViewBag.Semester = new SelectList(db.Semesters, "SemesterID", "SemesterName");
            return View("Index",model);
        }
        public ActionResult Update(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            StudentBatchProgramCourse discipline = db.StudentBatchProgramCourses.Find(id);
            if (discipline == null)
            {
                return HttpNotFound();
            }


            model.BatchProgramCourses = db.BatchProgramCourses.OrderByDescending(a => a.BatchProgramID).ToList();
            model.StudentBatchProgramCourses = db.StudentBatchProgramCourses.OrderByDescending(a => a.BatchProgramID).ToList();
            model.DisplayMode = "ReadWrite";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc");
            
            ViewBag.DisciplineCategoryID = new SelectList(db.DisciplineCategories, "DisciplineCategoryID", "DisciplineCategoryName", discipline.StudentBatchProgramCourseID);
            ViewBag.DisciplineCommittedID = new SelectList(db.DisciplineCommitteds, "DisciplineCommittedID", "DisciplineCommittedName", discipline.StudentID);
            ViewBag.StatusID = new SelectList(db.Status.Where(s => s.StatusName == "Closed" || s.StatusName == "Pending Further Investigation" || s.StatusName == "Under Investigation"), "StatusID", "StatusName", discipline.CourseID);
            ViewBag.MessageType = "";
            ViewBag.Message = "";
            return View("Index", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Discipline discipline)
        {
            try
            {
                db.Entry(discipline).State = EntityState.Modified;
                discipline.ModifiedBy = Convert.ToInt32(Session["emp_id"]);
                discipline.ModifiedOn = DateTime.Now;
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

            model.BatchProgramCourses = db.BatchProgramCourses.OrderByDescending(a => a.BatchProgramID).ToList();
            model.BatchProgramCourses = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc");
            
            ViewBag.DisciplineCategoryID = new SelectList(db.DisciplineCategories, "DisciplineCategoryID", "DisciplineCategoryName", discipline.DisciplineCategoryID);
            ViewBag.DisciplineCommittedID = new SelectList(db.DisciplineCommitteds, "DisciplineCommittedID", "DisciplineCommittedName", discipline.DisciplineCommittedID);
            ViewBag.StatusID = new SelectList(db.Status.Where(s => s.StatusName == "Closed" || s.StatusName == "Pending Further Investigation" || s.StatusName == "Under Investigation"), "StatusID", "StatusName", discipline.StatusID);
            return RedirectToAction("Index", model);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StudentBatchProgramCourse discipline = db.StudentBatchProgramCourses.Find(id);
            if (discipline == null)
            {
                return HttpNotFound();
            }

            model.BatchProgramCourses = db.BatchProgramCourses.OrderByDescending(a => a.BatchProgramID).ToList();
            model.BatchProgramCourses = null;
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
                Discipline discipline = db.Disciplines.Find(id);
                db.Disciplines.Remove(discipline);
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

            model.BatchProgramCourses = db.BatchProgramCourses.OrderByDescending(a => a.BatchProgramID).ToList();
            model.BatchProgramCourses = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc");
            
            ViewBag.DisciplineCategoryID = new SelectList(db.DisciplineCategories, "DisciplineCategoryID", "DisciplineCategoryName");
            ViewBag.DisciplineCommittedID = new SelectList(db.DisciplineCommitteds, "DisciplineCommittedID", "DisciplineCommittedName");
            ViewBag.StatusID = new SelectList(db.Status.Where(s => s.StatusName == "Closed" || s.StatusName == "Pending Further Investigation" || s.StatusName == "Under Investigation"), "StatusID", "StatusName");

            return View("Index", model);
        }
        public JsonResult GetStudentsList(string searchValue)
        {
            List<Student> lstStudent = new List<Student>();

            lstStudent = db.Students.Where(a => a.FirstName.Contains(searchValue)
            || a.LastName.Contains(searchValue)
            || a.FormNo.Contains(searchValue)
            || a.ACNIC.Contains(searchValue)).ToList();

            var Students = lstStudent.Select(s => new
            {
                AddAppID = s.StudentID,
                FirstName = s.FirstName,
                LastName = s.LastName,
                FormNo = s.FormNo,
                ACNIC = s.ACNIC
            });

            string result = JsonConvert.SerializeObject(Students, Formatting.Indented);
            return new JsonResult { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult GetSubjects(string FormNo)
        {
            List<Applicant> lstApplicant = new List<Applicant>();
            string expiryDate = "";
            string DOB = "";

            lstApplicant = db.Applicants.Where(a => a.FormNo == FormNo).ToList();
            if (lstApplicant.Count > 0)
            {
                expiryDate = (lstApplicant[0].PassportExpiryDate == null) ? "" : lstApplicant[0].PassportExpiryDate;
                DOB = (lstApplicant[0].ApplicantDOB == null) ? "" : lstApplicant[0].ApplicantDOB;
            }
            else
            {
                ViewBag.MessageType = "error";
                ViewBag.Message = "No result found.";
                return new JsonResult { Data = "No result found.", JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }

            var Applicants = lstApplicant.Select(s => new
            {
                AddAppID = s.AddAppID,
                FirstName = s.FirstName,
                LastName = s.LastName,
                FormNo = s.FormNo,
                ACNIC = s.ACNIC,
                SalutationID = s.SalutationID,
                SalutationName = db.Salutations.FirstOrDefault(sa => sa.SalutationID == s.SalutationID).SalutationName,
                FatherName = s.FatherName,
                ApplicantDOB = DOB,
                PlaceOfBirth = s.PlaceOfBirth,
                GenderID = s.GenderID,
                GenderName = db.Genders.FirstOrDefault(g => g.GenderID == s.GenderID).GenderName,
                NationalityID = s.NationalityID,
                MaritalStatusID = s.MaritalStatusID,
                MaritalStatusName = db.MaritalStatus.FirstOrDefault(m => m.MaritalStatusID == s.MaritalStatusID).MaritalStatusName,
                PassportNo = s.PassportNo,
                PassportExpiryDate = expiryDate,
                PTCLNO = s.PTCLNO,
                CellNo = s.CellNo,
                Email = s.Email,
                AlternateEmail = s.AlternateEmail,
                GuardianName = s.GuardianName,
                RelationTypeID = s.RelationTypeID,
                RelationTypeName = db.RelationTypes.FirstOrDefault(r => r.RelationTypeID == s.RelationTypeID).RelationTypeName,
                GuardianCNIC = s.GuardianCNIC,
                BatchProgramID = s.BatchProgramID,
                Picture = s.Picture,
                CountryID = s.CountryID,
                ProvinceID = s.ProvinceID,
                CityID = s.CityID,
                PresentAddress = s.PresentAddress,
                PermanentAddress = s.PermanentAddress,
                ReligionID = s.ReligionID,
                CurrentOccupationID = s.CurrentOccupationID,
                StatusID = s.StatusID,
                StatusName = db.Status.FirstOrDefault(st => st.StatusID == s.StatusID).StatusName,
                CreatedOn = s.CreatedOn,
                CreatedBy = s.CreatedBy,
                IsActive = s.IsActive,
                ModifiedOn = s.ModifiedOn,
                ModifiedBy = s.ModifiedBy
            });

            string result = JsonConvert.SerializeObject(Applicants, Formatting.Indented);
            return new JsonResult { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
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
