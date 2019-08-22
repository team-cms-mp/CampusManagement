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
    public class BatchProgramCoursesController : Controller
    {
        private ModelCMSNewContainer db = new ModelCMSNewContainer();
        GetBatchProgramCourses_ResultViewModel model = new GetBatchProgramCourses_ResultViewModel();

        public ActionResult Index()
        {
            model.BatchProgramCourses = db.GetBatchProgramCourses("").ToList();
            model.SelectedBatchProgramCourse = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.BatchID = new SelectList(db.Batches, "BatchID", "BatchName");
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc");
            ViewBag.CourseID = new SelectList(db.Courses, "CourseID", "CourseName");
            ViewBag.BatchProgramID = new SelectList(db.GetBatchProgramNameConcat("", 0).ToList(), "ID", "Name");
            ViewBag.CourseTypeID = new SelectList(db.CourseTypes, "CourseTypeID", "CourseTypeName");
           
            ViewBag.SectionID = new SelectList(db.Sections.Where(a => a.IsActive == "Yes"), "SectionID", "SectionName");
            ViewBag.LectureRoomID = new SelectList(db.Rooms.Where(l => l.RoomTypeID == 1), "RoomID", "RoomName");
            ViewBag.LabRoomID = new SelectList(db.Rooms.Where(l => l.RoomTypeID == 6), "RoomID", "RoomName");
            ViewBag.MessageType = "";
            ViewBag.Message = "";
            return View(model);
        }

        [HttpGet]
        public ActionResult Create()
        {
            model.BatchProgramCourses = db.GetBatchProgramCourses("").ToList();
            model.SelectedBatchProgramCourse = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.BatchID = new SelectList(db.Batches, "BatchID", "BatchName");
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc");
            ViewBag.CourseID = new SelectList(db.Courses, "CourseID", "CourseName");
            ViewBag.BatchProgramID = new SelectList(db.GetBatchProgramNameConcat("", 0).ToList(), "ID", "Name");
            ViewBag.CourseTypeID = new SelectList(db.CourseTypes, "CourseTypeID", "CourseTypeName");
            ViewBag.SectionID = new SelectList(db.Sections.Where(a => a.IsActive == "Yes"), "SectionID", "SectionName");

            ViewBag.LectureRoomID = new SelectList(db.Rooms.Where(l => l.RoomTypeID == 1), "RoomID", "RoomName");
            ViewBag.LabRoomID = new SelectList(db.Rooms.Where(l => l.RoomTypeID == 6), "RoomID", "RoomName");
            ViewBag.MessageType = "";
            ViewBag.Message = "";
            return View("Index", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(BatchProgramCourse batchProgramCourse)
        {
            string ErrorMessage = "";
            int count = 0;
            try
            {
                if (batchProgramCourse.YearSemesterNo == 0)
                {
                    ViewBag.MessageType = "error";
                    ViewBag.Message = "Semester # should not be 0.";
                    ModelState.AddModelError(string.Empty, "Semester # should not be 0.");
                }
                else
                {
                    BatchProgramCourse pc = db.BatchProgramCourses.FirstOrDefault(
                        p => p.BatchProgramID == batchProgramCourse.BatchProgramID
                        && p.CourseID == batchProgramCourse.CourseID
                        && p.YearSemesterNo == batchProgramCourse.YearSemesterNo
                        && p.LectureRoomID == batchProgramCourse.LectureRoomID
                        );


                    if (pc != null)
                    {
                        ViewBag.MessageType = "error";
                        ViewBag.Message = "Selected Course already exists against the same Program , Semester # and Room.";
                        ModelState.AddModelError(string.Empty, "Selected Course already exists against the same Program , Semester # and Room");
                    }
                    else
                    {
                        batchProgramCourse.CreatedBy = Convert.ToInt32(Session["emp_id"]);
                        batchProgramCourse.CreatedOn = DateTime.Now;
                        db.BatchProgramCourses.Add(batchProgramCourse);
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
            model.BatchProgramCourses = db.GetBatchProgramCourses("").ToList();
            model.SelectedBatchProgramCourse = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.BatchID = new SelectList(db.Batches, "BatchID", "BatchName", batchProgramCourse.BatchID);
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc", batchProgramCourse.IsActive);
            ViewBag.CourseID = new SelectList(db.Courses, "CourseID", "CourseName", batchProgramCourse.CourseID);
            ViewBag.BatchProgramID = new SelectList(db.GetBatchProgramNameConcat("", 0).ToList(), "ID", "Name", batchProgramCourse.BatchProgramID);
            ViewBag.CourseTypeID = new SelectList(db.CourseTypes, "CourseTypeID", "CourseTypeName", batchProgramCourse.CourseTypeID);
            ViewBag.SectionID = new SelectList(db.Sections.Where(a => a.IsActive == "Yes"), "SectionID", "SectionName", batchProgramCourse.SectionID);
            ViewBag.LectureRoomID = new SelectList(db.Rooms.Where(l => l.RoomTypeID == 1), "RoomID", "RoomName", batchProgramCourse.LectureRoomID);
            ViewBag.LabRoomID = new SelectList(db.Rooms.Where(l => l.RoomTypeID == 6), "RoomID", "RoomName", batchProgramCourse.LabRoomID);
            return View("Index", model);
        }

        public ActionResult Update(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            GetBatchProgramCourses_Result batchProgramCourse = db.GetBatchProgramCourses("").FirstOrDefault(bp => bp.ProgramCourseID == id);
            if (batchProgramCourse == null)
            {
                return HttpNotFound();
            }

            model.BatchProgramCourses = db.GetBatchProgramCourses("").ToList();
            model.SelectedBatchProgramCourse = batchProgramCourse;
            model.DisplayMode = "ReadWrite";
            ViewBag.BatchID = new SelectList(db.Batches, "BatchID", "BatchName", batchProgramCourse.BatchID);
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc", batchProgramCourse.IsActive);
            ViewBag.CourseID = new SelectList(db.Courses, "CourseID", "CourseName", batchProgramCourse.CourseID);
            ViewBag.BatchProgramID = new SelectList(db.GetBatchProgramNameConcat("", 0).ToList(), "ID", "Name", batchProgramCourse.BatchProgramID);
            ViewBag.CourseTypeID = new SelectList(db.CourseTypes, "CourseTypeID", "CourseTypeName", batchProgramCourse.CourseTypeID);
            ViewBag.SectionID = new SelectList(db.Sections.Where(a => a.IsActive == "Yes"), "SectionID", "SectionName", batchProgramCourse.SectionID);
            ViewBag.LectureRoomID = new SelectList(db.Rooms.Where(l => l.RoomTypeID == 1), "RoomID", "RoomName", batchProgramCourse.LectureRoomID);
            ViewBag.LabRoomID = new SelectList(db.Rooms.Where(l => l.RoomTypeID == 6), "RoomID", "RoomName", batchProgramCourse.LabRoomID);
            ViewBag.MessageType = "";
            ViewBag.Message = "";
            return View("Index", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(BatchProgramCourse batchProgramCourse)
        {
            try
            {
                if (batchProgramCourse.YearSemesterNo == 0)
                {
                    ViewBag.MessageType = "error";
                    ViewBag.Message = "Semester # should not be 0.";
                    ModelState.AddModelError(string.Empty, "Semester # should not be 0.");
                }
                else
                {
                    db.Entry(batchProgramCourse).State = EntityState.Modified;
                    batchProgramCourse.ModifiedBy = Convert.ToInt32(Session["emp_id"]);
                    batchProgramCourse.ModifiedOn = DateTime.Now;
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
            model.BatchProgramCourses = db.GetBatchProgramCourses("").ToList();
            model.SelectedBatchProgramCourse = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.BatchID = new SelectList(db.Batches, "BatchID", "BatchName", batchProgramCourse.BatchID);
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc", batchProgramCourse.IsActive);
            ViewBag.CourseID = new SelectList(db.Courses, "CourseID", "CourseName", batchProgramCourse.CourseID);
            ViewBag.BatchProgramID = new SelectList(db.GetBatchProgramNameConcat("", 0).ToList(), "ID", "Name", batchProgramCourse.BatchProgramID);
            ViewBag.CourseTypeID = new SelectList(db.CourseTypes, "CourseTypeID", "CourseTypeName", batchProgramCourse.CourseTypeID);
            ViewBag.SectionID = new SelectList(db.Sections.Where(a => a.IsActive == "Yes"), "SectionID", "SectionName", batchProgramCourse.SectionID);
            ViewBag.LectureRoomID = new SelectList(db.Rooms.Where(l => l.RoomTypeID == 1), "RoomID", "RoomName", batchProgramCourse.LectureRoomID);
            ViewBag.LabRoomID = new SelectList(db.Rooms.Where(l => l.RoomTypeID == 6), "RoomID", "RoomName", batchProgramCourse.LabRoomID);

            return View("Index", model);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GetBatchProgramCourses_Result batchProgramCourse = db.GetBatchProgramCourses("").FirstOrDefault(bp => bp.ProgramCourseID == id);
            if (batchProgramCourse == null)
            {
                return HttpNotFound();
            }

            model.BatchProgramCourses = db.GetBatchProgramCourses("").ToList();
            model.SelectedBatchProgramCourse = batchProgramCourse;
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
                BatchProgramCourse batchProgramCourse = db.BatchProgramCourses.Find(id);
                db.BatchProgramCourses.Remove(batchProgramCourse);
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
            model.BatchProgramCourses = db.GetBatchProgramCourses("").ToList();
            model.SelectedBatchProgramCourse = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.BatchID = new SelectList(db.Batches, "BatchID", "BatchName");
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc");
            ViewBag.CourseID = new SelectList(db.Courses, "CourseID", "CourseName");
            ViewBag.BatchProgramID = new SelectList(db.GetBatchProgramNameConcat("", 0).ToList(), "ID", "Name");
            ViewBag.CourseTypeID = new SelectList(db.CourseTypes, "CourseTypeID", "CourseTypeName");
            ViewBag.SectionID = new SelectList(db.Sections.Where(a => a.IsActive == "Yes"), "SectionID", "SectionName");
            ViewBag.LectureRoomID = new SelectList(db.Rooms.Where(l => l.RoomTypeID == 1), "RoomID", "RoomName");
            ViewBag.LabRoomID = new SelectList(db.Rooms.Where(l => l.RoomTypeID == 6), "RoomID", "RoomName");
            return View("Index", model);
        }

        // GEt programs by Faculty and Batch.
        public JsonResult GetPrograms_by_FacultyLevelBatch(string FacultyID, string BatchID)
        {
            List<GetPrograms_by_FacultyLevelBatch_Result> lstPrograms = new List<GetPrograms_by_FacultyLevelBatch_Result>();

            lstPrograms = db.GetPrograms_by_FacultyLevelBatch(Convert.ToInt32(FacultyID), 0, Convert.ToInt32(BatchID), 0).ToList();
            var programs = lstPrograms.Select(p => new
            {
                BatchProgramID = p.BatchProgramID,
                ProgramName = p.ProgramName
            });
            string result = JsonConvert.SerializeObject(programs, Formatting.Indented);
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
