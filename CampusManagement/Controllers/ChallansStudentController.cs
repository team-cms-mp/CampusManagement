using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CampusManagement.Models;
using Newtonsoft.Json;
using System.Data.SqlClient;

namespace CampusManagement.Controllers
{
    [Authorize]
    public class ChallansStudentController : Controller
    {
        private ModelCMSNewContainer db = new ModelCMSNewContainer();
        ChallansViewModel model = new ChallansViewModel();

        public ActionResult Index()
        {
            model.Challans = db.GetAllChallans(0, "", 0, "Student").ToList();
            model.SelectedChallan = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.BatchID = new SelectList(db.Batches, "BatchID", "BatchName");
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc");
            ViewBag.BatchProgramID = new SelectList(db.GetBatchProgramNameConcat("", 0), "ID", "Name");
            ViewBag.YearSemesterNo = new SelectList(db.Semesters, "YearSemesterNo", "YearSemesterNo");
            ViewBag.AccountID = new SelectList(db.Finance_Bank_Accounts, "Account_ID", "Account_No");

            ViewBag.MessageType = "";
            ViewBag.Message = "";
            return View(model);
        }

        [HttpGet]
        public ActionResult Create()
        {
            model.Challans = db.GetAllChallans(0, "", 0, "Student").ToList();
            model.SelectedChallan = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc");
            ViewBag.BatchID = new SelectList(db.Batches, "BatchID", "BatchName");
            ViewBag.BatchProgramID = new SelectList(db.GetBatchProgramNameConcat("", 0), "ID", "Name");
            ViewBag.YearSemesterNo = new SelectList(db.Semesters, "YearSemesterNo", "YearSemesterNo");
            ViewBag.AccountID = new SelectList(db.Finance_Bank_Accounts, "Account_ID", "Account_No");

            ViewBag.MessageType = "";
            ViewBag.Message = "";
            return View("Index", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Challan challan, FormCollection fc)
        {
            string ErrorMessage = "";
            int count = 0;
            try
            {
                if (string.IsNullOrEmpty(challan.FormNo))
                {
                    challan.FormNo = "";
                }
                challan.IsDeposited = "No";

                if (challan.BatchProgramID == 0 || challan.BatchProgramID == null)
                {
                    count++;
                    ErrorMessage += count + "-" + string.Concat("Please select Program.") + "<br />";
                    ViewBag.MessageType = "error";
                }

                if (challan.YearSemesterNo == 0 || challan.YearSemesterNo == null)
                {
                    count++;
                    ErrorMessage += count + "-" + string.Concat("Please select Year/Semester.") + "<br />";
                    ViewBag.MessageType = "error";
                }

                if (challan.IssueDate == null)
                {
                    count++;
                    ErrorMessage += count + "-" + string.Concat("Please enter valid Issue Date.") + "<br />";
                    ViewBag.MessageType = "error";
                }

                if (challan.LastDate == null)
                {
                    count++;
                    ErrorMessage += count + "-" + string.Concat("Please enter valid Last Date.") + "<br />";
                    ViewBag.MessageType = "error";
                }

                if (challan.AccountID == null || challan.AccountID == 0)
                {
                    count++;
                    ErrorMessage += count + "-" + string.Concat("Please select Bank Account.") + "<br />";
                    ViewBag.MessageType = "error";
                }

                if (string.IsNullOrEmpty(ErrorMessage))
                {
                    string Data = "<DATA>";
                    Status status = db.Status.FirstOrDefault(st => st.StatusName == "Selected");
                    List<Applicant> lstApplicants = db.Applicants.Where(s => s.BatchProgramID == challan.BatchProgramID && s.StatusID == status.StatusID).ToList();
                    if (lstApplicants.Count > 0)
                    {
                        foreach (var item in lstApplicants)
                        {
                            Challan ch = new Challan();
                            ch.FormNo = item.FormNo;
                            ch.BatchProgramID = item.BatchProgramID;
                            ch.IssueDate = challan.IssueDate;
                            ch.LastDate = challan.LastDate;
                            ch.AccountID = challan.AccountID;
                            ch.IsDeposited = "No";
                            ch.YearSemesterNo = 1;
                            ch.Voucher_Trans_ID = 0;
                            ch.CreatedBy = Convert.ToInt32(Session["emp_id"]);
                            ch.CreatedOn = DateTime.Now;
                            ch.IsActive = "Yes";
                            ch.ChallanType = "Student";
                            ch.IsBatchProgramWise = "ProgramWise";
                            db.Challans.Add(ch);
                            db.SaveChanges();
                            //Getting Scope Identity
                            challan.ChallanID = ch.ChallanID;
                            Data = Data + "<row><ChallanID>" + ch.ChallanID.ToString() + "</ChallanID></row>";
                            //Inserting ChallanServices
                            int totalRows = Convert.ToInt32(fc["TotalRows"]);
                            for (int i = 1; i <= totalRows; i++)
                            {
                                if (fc["Amount_" + i] != null && fc["Amount_" + i] != "" && fc["Amount_" + i] != "0")
                                {
                                    try
                                    {
                                        int? Quantity = Convert.ToInt32(fc["Quantity_" + i]);
                                        if (Quantity == 0 || Quantity == null)
                                        {
                                            challan.Quantity = 1;
                                        }
                                        else
                                        {
                                            challan.Quantity = Quantity;
                                        }

                                        int CollegeServiceID = Convert.ToInt32(fc["CollegeServiceID_" + i]);
                                        ChallanService chs = new ChallanService();
                                        chs.ChallanID = challan.ChallanID;
                                        chs.CollegeServiceID = CollegeServiceID;
                                        chs.Amount = Convert.ToInt32(fc["Amount_" + i]);
                                        chs.Quantity = challan.Quantity;
                                        db.ChallanServices.Add(chs);
                                        db.SaveChanges();
                                    }
                                    catch (EntityCommandExecutionException)
                                    {
                                        ViewBag.MessageType = "success";
                                        ViewBag.Message = "Data has been saved successfully.";
                                    }
                                }
                            }
                        }
                    }

                    Data = Data + "</DATA>";
                    if (Data != "<DATA></DATA>")
                    {
                        db.Post_FinancialImpact_Challan(Data, Convert.ToInt32(Session["emp_id"])
                            , Convert.ToInt32(Session["SubDeptid"]), Convert.ToInt32(Session["dept_id"]), "Student");
                    }

                    ViewBag.MessageType = "success";
                    ViewBag.Message = "Data has been saved successfully.";
                }
                else
                {
                    ViewBag.Message = ErrorMessage;
                }
            }
            catch (Exception ex)
            {
                ViewBag.MessageType = "error";
                ViewBag.Message = ex.Message;
            }

            model.Challans = db.GetAllChallans(challan.BatchProgramID, challan.FormNo, challan.YearSemesterNo, "Student").ToList();
            model.SelectedChallan = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.BatchID = new SelectList(db.Batches, "BatchID", "BatchName", challan.BatchID);
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc", challan.IsActive);
            ViewBag.BatchProgramID = new SelectList(db.GetBatchProgramNameConcat("", 0), "ID", "Name", challan.BatchProgramID);
            ViewBag.YearSemesterNo = new SelectList(db.Semesters, "YearSemesterNo", "YearSemesterNo", challan.YearSemesterNo);
            ViewBag.AccountID = new SelectList(db.Finance_Bank_Accounts, "Account_ID", "Account_No", challan.AccountID);

            return View("Index", model);
        }

        [HttpGet]
        public ActionResult Search()
        {
            model.Challans = db.GetAllChallans(0, "", 0, "Student").ToList();
            model.SelectedChallan = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.BatchID = new SelectList(db.Batches, "BatchID", "BatchName");
            ViewBag.BatchProgramID = new SelectList(db.GetBatchProgramNameConcat("", 0), "ID", "Name");
            ViewBag.YearSemesterNo = new SelectList(db.Semesters, "YearSemesterNo", "YearSemesterNo");

            ViewBag.MessageType = "";
            ViewBag.Message = "";
            return View("ApproveChallans", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Search(Challan challan)
        {
            if (string.IsNullOrEmpty(challan.FormNo))
            {
                challan.FormNo = "";
            }

            model.Challans = db.GetAllChallans(challan.BatchProgramID, challan.FormNo, challan.YearSemesterNo, "Student").ToList();
            model.SelectedChallan = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.BatchID = new SelectList(db.Batches, "BatchID", "BatchName", challan.BatchID);
            ViewBag.BatchProgramID = new SelectList(db.GetBatchProgramNameConcat("", 0), "ID", "Name", challan.BatchProgramID);
            ViewBag.YearSemesterNo = new SelectList(db.Semesters, "YearSemesterNo", "YearSemesterNo", challan.YearSemesterNo);
            if (model.Challans.Count == 0)
            {
                ViewBag.MessageType = "error";
                ViewBag.Message = "No record found.";
            }
            ViewBag.hdnBatchID = challan.BatchID;
            ViewBag.hdnBatchProgramID = challan.BatchProgramID;
            ViewBag.hdnYearSemesterNo = challan.YearSemesterNo;
            return View("ApproveChallans", model);
        }

        [HttpGet]
        public ActionResult ApproveChallans()
        {
            model.Challans = db.GetAllChallans(0, "", 0, "Student").ToList();
            model.SelectedChallan = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.BatchID = new SelectList(db.Batches, "BatchID", "BatchName");
            ViewBag.BatchProgramID = new SelectList(db.GetBatchProgramNameConcat("", 0), "ID", "Name");
            ViewBag.YearSemesterNo = new SelectList(db.Semesters, "YearSemesterNo", "YearSemesterNo");

            ViewBag.MessageType = "";
            ViewBag.Message = "";
            return View("ApproveChallans", model);
        }

        public JsonResult ApproveStudentChallan(int? ChallanID, string depositDate)
        {
            string ErrorMessage = "";
            int EmpID = Convert.ToInt32(Session["emp_id"]);
            try
            {
                if (ChallanID == null)
                {
                    ErrorMessage += string.Concat("Challan ID is null.") + "<br />";
                }

                if (string.IsNullOrEmpty(depositDate))
                {
                    ErrorMessage += string.Concat("Required") + "<br />";
                }
                else
                {
                    DateTime ddate;
                    bool isValid = DateTime.TryParse(depositDate, out ddate);
                    if (!isValid)
                    {
                        ErrorMessage += string.Concat("Invalid") + "<br />";
                    }
                }

                if (string.IsNullOrEmpty(ErrorMessage))
                {
                    db.ApproveStudentChallan(Convert.ToDateTime(depositDate), ChallanID, EmpID, "Student");
                    return Json("success", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(ErrorMessage, JsonRequestBehavior.AllowGet);
                }
            }
            catch (EntityCommandExecutionException)
            {
                return Json("error", JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult SingleStudentChallan()
        {
            ViewBag.objES = null;
            ViewBag.AccountID = new SelectList(db.Finance_Bank_Accounts, "Account_ID", "Account_No");
            ViewBag.YearSemesterNo = new SelectList(db.Semesters, "YearSemesterNo", "YearSemesterNo");
            ViewBag.MessageType = "";
            ViewBag.Message = "";
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SingleStudentChallan(string FormNo)
        {
            GetEnrolledStudent_Result objES = db.GetEnrolledStudent(FormNo, "Student").FirstOrDefault();
            ViewBag.objES = objES;
            ViewBag.AccountID = new SelectList(db.Finance_Bank_Accounts, "Account_ID", "Account_No");
            ViewBag.YearSemesterNo = new SelectList(db.Semesters, "YearSemesterNo", "YearSemesterNo");
            ViewBag.MessageType = "";
            ViewBag.Message = "";
            ViewBag.FormNo = FormNo;
            return View();
        }

        public ActionResult InsertStudentChallan(string FormNo, FormCollection fc)
        {
            GetEnrolledStudent_Result objES = db.GetEnrolledStudent(FormNo, "Student").FirstOrDefault();

            string ErrorMessage = "";
            int count = 0;
            int? QuantityMain = 0;
            if (objES != null)
            {
                try
                {
                    objES.IssueDate = fc["IssueDate"];
                    objES.LastDate = fc["LastDate"];
                    objES.YearSemesterNo = Convert.ToInt32(fc["YearSemesterNo"]);
                    objES.AccountID = fc["AccountID"];

                    if (objES.YearSemesterNo == 0)
                    {
                        count++;
                        ErrorMessage += count + "-" + string.Concat("Please select Year/Semester.") + "<br />";
                        ViewBag.MessageType = "error";
                    }

                    if (string.IsNullOrEmpty(objES.IssueDate))
                    {
                        count++;
                        ErrorMessage += count + "-" + string.Concat("Please enter valid Issue Date.") + "<br />";
                        ViewBag.MessageType = "error";
                    }

                    if (string.IsNullOrEmpty(objES.LastDate))
                    {
                        count++;
                        ErrorMessage += count + "-" + string.Concat("Please enter valid Last Date.") + "<br />";
                        ViewBag.MessageType = "error";
                    }

                    if (string.IsNullOrEmpty(objES.AccountID))
                    {
                        count++;
                        ErrorMessage += count + "-" + string.Concat("Please select Bank Account.") + "<br />";
                        ViewBag.MessageType = "error";
                    }

                    string Data = "<DATA>";
                    if (string.IsNullOrEmpty(ErrorMessage))
                    {
                        Challan ch = new Challan();
                        ch.FormNo = objES.FormNo;
                        ch.BatchProgramID = objES.BatchProgramID;
                        ch.IssueDate = objES.IssueDate;
                        ch.LastDate = objES.LastDate;
                        ch.AccountID = Convert.ToInt32(objES.AccountID);
                        ch.IsDeposited = "No";
                        ch.YearSemesterNo = objES.YearSemesterNo;
                        ch.Voucher_Trans_ID = 0;
                        ch.CreatedBy = Convert.ToInt32(Session["emp_id"]);
                        ch.CreatedOn = DateTime.Now;
                        ch.IsActive = "Yes";
                        ch.ChallanType = "Student";
                        ch.IsBatchProgramWise = "Single";
                        db.Challans.Add(ch);
                        db.SaveChanges();
                        //Getting Scope Identity
                        int ChallanID = ch.ChallanID;
                        Data = Data + "<row><ChallanID>" + ch.ChallanID.ToString() + "</ChallanID></row>";

                        //Inserting ChallanServices
                        int totalRows = Convert.ToInt32(fc["TotalRows"]);
                        for (int i = 1; i <= totalRows; i++)
                        {
                            if (fc["Amount_" + i] != null && fc["Amount_" + i] != "" && fc["Amount_" + i] != "0")
                            {
                                try
                                {
                                    int? Quantity = Convert.ToInt32(fc["Quantity_" + i]);
                                    if (Quantity == 0 || Quantity == null)
                                    {
                                        QuantityMain = 1;
                                    }
                                    else
                                    {
                                        QuantityMain = Quantity;
                                    }

                                    int CollegeServiceID = Convert.ToInt32(fc["CollegeServiceID_" + i]);
                                    ChallanService chs = new ChallanService();
                                    chs.ChallanID = ChallanID;
                                    chs.CollegeServiceID = CollegeServiceID;
                                    chs.Amount = Convert.ToInt32(fc["Amount_" + i]);
                                    chs.Quantity = QuantityMain;
                                    db.ChallanServices.Add(chs);
                                    db.SaveChanges();
                                }
                                catch (EntityCommandExecutionException)
                                {

                                }
                            }
                        }
                        ViewBag.MessageType = "success";
                        ViewBag.Message = "Data has been saved successfully.";
                    }
                    else
                    {
                        ViewBag.Message = ErrorMessage;
                    }

                    Data = Data + "</DATA>";
                    if (Data != "<DATA></DATA>")
                    {
                        db.Post_FinancialImpact_Challan(Data, Convert.ToInt32(Session["emp_id"])
                            , Convert.ToInt32(Session["SubDeptid"]), Convert.ToInt32(Session["dept_id"]), "Student");
                    }
                }
                catch (Exception ex)
                {
                    ViewBag.MessageType = "error";
                    ViewBag.Message = ex.Message;
                }
            }
            else
            {
                ViewBag.MessageType = "error";
                ViewBag.Message = "No student found to add challan.";
            }

            ViewBag.objES = objES;
            ViewBag.AccountID = new SelectList(db.Finance_Bank_Accounts, "Account_ID", "Account_No");
            ViewBag.YearSemesterNo = new SelectList(db.Semesters, "YearSemesterNo", "YearSemesterNo");
            ViewBag.FormNo = FormNo;

            return View("SingleStudentChallan");
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