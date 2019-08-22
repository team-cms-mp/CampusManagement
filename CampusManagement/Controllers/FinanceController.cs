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

namespace CampusManagement.Controllers
{
    [Authorize]
    public class FinanceController : Controller
    {
        ModelCMSNewContainer db = new ModelCMSNewContainer();
        StudentDiscountRequestsViewModel model = new StudentDiscountRequestsViewModel();
        public ActionResult Invoicing()
        {
            return View();
        }

        public ActionResult Collection()
        {
            return View();
        }

        public ActionResult Discount()
        {
            return View();
        }

        public ActionResult FinanceReports()
        {
            return View();
        }

        public ActionResult PaymentCollection()
        {
            List<GetApplicantStudentChallans_Result> lst = db.GetApplicantStudentChallans("", "Student", "Yes", 0, 0).ToList();
            return View(lst);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PaymentCollection(string FormNo, int? BatchID, int? BatchProgramID)
        {
            ViewBag.FormNo = FormNo;
            ViewBag.BatchID = BatchID;
            ViewBag.BatchProgramID = BatchProgramID;
            List<GetApplicantStudentChallans_Result> lst = db.GetApplicantStudentChallans(FormNo, "Student", "Yes", BatchID, BatchProgramID).ToList();
            return View(lst);
        }

        public ActionResult PayablesFromStudents()
        {
            List<GetApplicantStudentChallans_Result> lst = db.GetApplicantStudentChallans("", "Student", "No", 0, 0).ToList();
            return View(lst);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PayablesFromStudents(string FormNo, int? BatchID, int? BatchProgramID)
        {
            ViewBag.FormNo = FormNo;
            ViewBag.BatchID = BatchID;
            ViewBag.BatchProgramID = BatchProgramID;
            List<GetApplicantStudentChallans_Result> lst = db.GetApplicantStudentChallans(FormNo, "Student", "No", BatchID, BatchProgramID).ToList();
            return View(lst);
        }

        public ActionResult PaymentCollectionSummary()
        {
            List<GetApplicantStudentChallansSummary_Result> lst = db.GetApplicantStudentChallansSummary("", "Student", 0, 0).ToList();
            return View(lst);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PaymentCollectionSummary(string FormNo, int? BatchID, int? BatchProgramID)
        {
            ViewBag.FormNo = FormNo;
            ViewBag.BatchID = BatchID;
            ViewBag.BatchProgramID = BatchProgramID;
            List<GetApplicantStudentChallansSummary_Result> lst = db.GetApplicantStudentChallansSummary(FormNo, "Student", BatchID, BatchProgramID).ToList();
            return View(lst);
        }

        public ActionResult StudentDiscountRequests()
        {
            model.StudentDiscountRequests = db.GetStudentDiscountRequests("").OrderBy(d => d.StudentRequestStatusID).ToList();
            return View(model);
        }

        public ActionResult CollegeServiceWiseAmounts()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CollegeServiceWiseAmounts(string FormNo, int? BatchID, int? BatchProgramID)
        {
            ViewBag.FormNo = FormNo;
            ViewBag.BatchID = BatchID;
            ViewBag.BatchProgramID = BatchProgramID;
            return View();
        }

        public ActionResult FeeDefaultersHistory()
        {
            List<SF_GetApplicantStudentChallansFeeDefaulterSummary_Result> lst = db.SF_GetApplicantStudentChallansFeeDefaulterSummary("", "Student", 0, 0).ToList();
            return View(lst);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult FeeDefaultersHistory(string FormNo, int? BatchID, int? BatchProgramID)
        {
            ViewBag.FormNo = FormNo;
            ViewBag.BatchID = BatchID;
            ViewBag.BatchProgramID = BatchProgramID;
            List<SF_GetApplicantStudentChallansFeeDefaulterSummary_Result> lst = db.SF_GetApplicantStudentChallansFeeDefaulterSummary(FormNo, "Student", BatchID, BatchProgramID).ToList();
            return View(lst);
        }

        public ActionResult StudentFeeDefaultersHistory()
        {
            if(Session["FormNo"] == null)
            {
                return RedirectToAction("Login2", "Home");
            }
            string FormNo = Session["FormNo"].ToString();
            List<SF_GetApplicantStudentChallansFeeDefaulterSummary_Result> lst = db.SF_GetApplicantStudentChallansFeeDefaulterSummary(FormNo, "Student", 0, 0).ToList();
            return View(lst);
        }

        public ActionResult StudentPaymentCollection()
        {
            if (Session["FormNo"] == null)
            {
                return RedirectToAction("Login2", "Home");
            }
            string formno = Convert.ToString(Session["FormNo"]);
            List<GetApplicantStudentChallans_Result> lst = db.GetApplicantStudentChallans(formno, "Student", "Yes", 0, 0).ToList();
            return View(lst);
        }

        public ActionResult StudentPayablesFromStudents()
        {
            if (Session["FormNo"] == null)
            {
                return RedirectToAction("Login2", "Home");
            }
            string formno = Convert.ToString(Session["FormNo"]);
            List<GetApplicantStudentChallans_Result> lst = db.GetApplicantStudentChallans(formno, "Student", "No", 0, 0).ToList();
            return View(lst);
        }

        public ActionResult StudentFeeDetail()
        {
            return View();
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

        public JsonResult ApproveDisapproveDiscounts(int StudentDiscountRequestID, string FormNo, string RequestTypeName, string StudentRequestStatusName, string DiscountPercentage)
        {
            int EmpID = Convert.ToInt32(Session["emp_id"]);

            try
            {
                db.Update_StudentDiscountRequest(StudentDiscountRequestID, FormNo, RequestTypeName, StudentRequestStatusName, DiscountPercentage, EmpID);
                return Json("success", JsonRequestBehavior.AllowGet);
            }
            catch (EntityCommandExecutionException)
            {
                return Json("error", JsonRequestBehavior.AllowGet);
            }
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
