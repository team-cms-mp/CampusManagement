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
using System.Data.Entity.Core;
using Newtonsoft.Json;
using CampusManagement.App_Code;

namespace CampusManagement.Controllers
{
    [Authorize]
    public class StudentDiscountsController : Controller
    {
        private ModelCMSNewContainer db = new ModelCMSNewContainer();
        SF_GetStudentApplicantDiscounts_ResultViewModel model = new SF_GetStudentApplicantDiscounts_ResultViewModel();

        [HttpGet]
        public ActionResult StudentsForDiscount()
        {
            ViewBag.MessageType = "";
            ViewBag.Message = "";
            List<GetStudentsToMoveToNextSemester_Result> lstStudents = db.GetStudentsToMoveToNextSemester(0, 0, 0).ToList();
            return View(lstStudents);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult StudentsForDiscount(string paramSearch, int? BatchID, int? BatchProgramID, int? YearSemesterNo, int? StatusID)
        {
            if (BatchID == null)
            {
                BatchID = 0;
            }

            if (BatchProgramID == null)
            {
                BatchProgramID = 0;
            }

            if (YearSemesterNo == null)
            {
                YearSemesterNo = 0;
            }

            if (StatusID == null)
            {
                StatusID = 0;
            }

            ViewBag.hdnBatchID = BatchID;
            ViewBag.hdnBatchProgramID = BatchProgramID;
            ViewBag.hdnYearSemesterNo = YearSemesterNo;
            ViewBag.hdnStatusID = StatusID;

            List<GetStudentsToMoveToNextSemester_Result> lstStudents = db.GetStudentsToMoveToNextSemester(BatchProgramID, YearSemesterNo, 0).ToList();
            return View(lstStudents);
        }

        public ActionResult Index(int? StudentID)
        {
            model.SF_GetStudentApplicantDiscounts_Results = db.SF_GetStudentApplicantDiscounts(StudentID, 1).ToList();
            model.SelectedSF_GetStudentApplicantDiscounts_Result = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.DiscountTypeID = new SelectList(db.SF_StudentDiscountType, "DiscountTypeID", "DiscountTypeName");
            ViewBag.MessageType = "";
            ViewBag.Message = "";
            ViewBag.StudentID = StudentID;
            return View(model);
        }

        [HttpGet]
        public ActionResult Create(int? StudentID)
        {
            model.SF_GetStudentApplicantDiscounts_Results = db.SF_GetStudentApplicantDiscounts(StudentID, 1).ToList();
            model.SelectedSF_GetStudentApplicantDiscounts_Result = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.DiscountTypeID = new SelectList(db.SF_StudentDiscountType, "DiscountTypeID", "DiscountTypeName");
            ViewBag.MessageType = "";
            ViewBag.Message = "";
            ViewBag.StudentID = StudentID;
            return View("Index", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(SF_StudentDiscount discount)
        {
            string ErrorMessage = "";
            int count = 0;
            try
            {
                SF_StudentDiscount dis = db.SF_StudentDiscount.FirstOrDefault(d => d.StudentID == discount.StudentID && d.YearSemesterNo == 1 && d.DiscountTypeID == discount.DiscountTypeID);
                if (dis != null)
                {
                    count++;
                    ErrorMessage += count + "-" + "Discount already exists." + "<br />";
                }

                if (discount.DiscountTypeID == null)
                {
                    count++;
                    ErrorMessage += count + "-" + "Please select Discount Type." + "<br />";
                }

                if (discount.PercentageInstallmentValue == null || Convert.ToDecimal(discount.PercentageInstallmentValue) <= 0)
                {
                    count++;
                    ErrorMessage += count + "-" + "Please select Percentage/Installments." + "<br />";
                }

                if (!string.IsNullOrEmpty(ErrorMessage))
                {
                    ViewBag.MessageType = "error";
                    ViewBag.Message = ErrorMessage;
                }
                else
                {
                    discount.CreatedBy = Convert.ToInt32(Session["emp_id"]);
                    discount.CreatedOn = DateTime.Now;
                    discount.StatusID = 1007;
                    discount.IsActive = "Yes";
                    db.SF_StudentDiscount.Add(discount);
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
            model.SF_GetStudentApplicantDiscounts_Results = db.SF_GetStudentApplicantDiscounts(discount.StudentID, 1).ToList();
            model.SelectedSF_GetStudentApplicantDiscounts_Result = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.DiscountTypeID = new SelectList(db.SF_StudentDiscountType, "DiscountTypeID", "DiscountTypeName");
            ViewBag.MessageType = "";
            ViewBag.Message = "";
            ViewBag.StudentID = discount.StudentID;

            return View("Index", model);
        }

        public ActionResult ApproveUnapprove(int? id, string type, int? StudentID)
        {
            try
            {
                Student stu = db.Students.FirstOrDefault(s => s.StudentID == StudentID);
                int EmpID = Convert.ToInt32(Session["emp_id"]);
                if (type == "Approved")
                {
                    db.SF_Insert_StudentService(0, stu.FormNo, StudentID, stu.BatchProgramID, stu.CurrentSemesterNo, EmpID, 9, id, 0);
                }
                else if (type == "Unapproved")
                {
                    db.SF_Insert_StudentService(0, "", StudentID, stu.BatchProgramID, stu.CurrentSemesterNo, EmpID, 7, id, 0);
                }
            }
            catch (EntityCommandExecutionException ex)
            {
                ViewBag.MessageType = "error";
                ViewBag.Message = string.Concat(ex.Message, ", Inner Exception: " + ex.InnerException);
                ModelState.AddModelError(string.Empty, ex.Message);
            }

            return RedirectToAction("Index", "StudentDiscounts", new { StudentID = StudentID });
        }

        // Get Active Batches
        public JsonResult GetActiveBatches()
        {
            List<Batch> lstBatches = new List<Batch>();

            lstBatches = db.Batches.Where(b => b.IsActive == "Yes").ToList();
            var batches = lstBatches.Select(b => new
            {
                BatchID = b.BatchID,
                BatchName = b.BatchName,
                BatchSession = b.BatchSession,
                BatchCode = b.BatchCode
            });
            string result = JsonConvert.SerializeObject(batches, Formatting.Indented);
            return Json(result, JsonRequestBehavior.AllowGet);
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

        public JsonResult GetBatchProgramSemesterList(string BatchProgramID)
        {
            List<BatchProgramSemester> lstSemester = new List<BatchProgramSemester>();
            int bpId = Convert.ToInt32(BatchProgramID);

            lstSemester = db.BatchProgramSemesters.Where(s => s.BatchProgramID == bpId).ToList();
            var semesters = lstSemester.Select(S => new
            {
                YearSemesterNo = S.YearSemesterNo
            });
            string result = JsonConvert.SerializeObject(semesters, Formatting.Indented);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        // GEt Status
        public JsonResult GetApplicantStudentStatus(string StatusType, int QueryID)
        {
            List<GetApplicantStudentStatus_Result> lstStatus = CommonFunctions.GetApplicantStudentStatus("student", QueryID);
            var status = lstStatus.Select(s => new
            {
                StatusID = s.StatusID,
                StatusName = s.StatusName,
                StatusType = s.StatusType
            });
            string result = JsonConvert.SerializeObject(status, Formatting.Indented);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        // GEt Discount Values
        public JsonResult GetDiscountValues(int DiscountTypeID)
        {
            List<SF_StudentPercentageInstallment> values = db.SF_StudentPercentageInstallment.Where(d => d.DiscountTypeID == DiscountTypeID).ToList();
            var value = values.Select(s => new
            {
                PercentageInstallmentDisplay = s.PercentageInstallmentDisplay,
                PercentageInstallmentValue = s.PercentageInstallmentValue
            });
            string result = JsonConvert.SerializeObject(value, Formatting.Indented);
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
