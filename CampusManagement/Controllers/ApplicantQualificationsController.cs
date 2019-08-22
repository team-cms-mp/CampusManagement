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
    public class ApplicantQualificationsController : Controller
    {

        private ModelCMSNewContainer db = new ModelCMSNewContainer();
        ApplicantQualificationViewModel model = new ApplicantQualificationViewModel();
        string ErrorMessage = "";
        int count = 0;

        public ActionResult Index()
        {
            if (Session["FormNo"] == null)
            {
                TempData["messageType"] = "error";
                TempData["message"] = "Please select applicant from Applicants list.";
                return RedirectToAction("Create", "Applicants");
            }

            int EmpID = Convert.ToInt32(Session["emp_id"]);
            string FormNo = Session["FormNo"].ToString();

            Applicant applicant = db.Applicants.FirstOrDefault(a => a.FormNo == FormNo);
            if (applicant != null)
            {
                FormNo = applicant.FormNo;
            }
            else
            {
                TempData["messageType"] = "error";
                TempData["message"] = "Please fill Personal Detail.";
                return RedirectToAction("Create", "Applicants");
            }

            if (Session["error"] != null)
            {
                ViewBag.MessageType = "error";
                ViewBag.Message = Session["error"].ToString();
                Session.Remove("error");
            }
            ApplyForProgram appap = db.ApplyForPrograms.FirstOrDefault(a => a.FormNo == FormNo);
            if (appap == null)
            {
                TempData["messageType"] = "error";
                TempData["message"] = "Please Choose Programs.";
                return RedirectToAction("ChooseProgram", "ApplicantQualifications");
            }

            if (!string.IsNullOrEmpty(FormNo))
            {
                //Eligibility Code
                List<ApplicantQualification> lstAQ = db.ApplicantQualifications.Where(q => q.FormNo == FormNo).ToList();
                bool isNotEligible = EligiblityCriteria();

                if (isNotEligible == true)
                {
                    db.ApplicantQualifications.RemoveRange(lstAQ);
                    db.SaveChanges();

                    model.SelectedApplicantQualification = null;
                    model.DisplayMode = "WriteOnly";
                    ViewBag.FormNo = FormNo;
                    ViewBag.FacultyID = new SelectList(db.GetSubDepartments_by_HospitalID(MvcApplication.Hospital_ID), "SubDept_Id", "SubDept_Name");
                    ViewBag.FacultyID1 = new SelectList(db.GetSubDepartments_by_HospitalID(MvcApplication.Hospital_ID), "SubDept_Id", "SubDept_Name");
                    ViewBag.FacultyID2 = new SelectList(db.GetSubDepartments_by_HospitalID(MvcApplication.Hospital_ID), "SubDept_Id", "SubDept_Name");
                    ViewBag.LevelID = new SelectList(db.Levels.Where(l => l.IsActive == "Yes"), "LevelID", "LevelName");
                    ViewBag.BatchID = new SelectList(db.Batches, "BatchID", "BatchName");
                    ViewBag.BatchProgramID1 = new SelectList(db.GetBatchProgramNameConcat("", 0), "ID", "Name");
                    ViewBag.BatchProgramID2 = new SelectList(db.GetBatchProgramNameConcat("", 0), "ID", "Name");
                    ViewBag.BatchProgramID3 = new SelectList(db.GetBatchProgramNameConcat("", 0), "ID", "Name");
                    ViewBag.BatchProgramID4 = new SelectList(db.GetBatchProgramNameConcat("", 0), "ID", "Name");
                    return View(model);
                }
            }
            else
            {
                return RedirectToAction("Login2", "Home");
            }
            model.SelectedApplicantQualification = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.FormNo = FormNo;
            ViewBag.FacultyID = new SelectList(db.GetSubDepartments_by_HospitalID(MvcApplication.Hospital_ID), "SubDept_Id", "SubDept_Name");
            ViewBag.FacultyID1 = new SelectList(db.GetSubDepartments_by_HospitalID(MvcApplication.Hospital_ID), "SubDept_Id", "SubDept_Name");
            ViewBag.FacultyID2 = new SelectList(db.GetSubDepartments_by_HospitalID(MvcApplication.Hospital_ID), "SubDept_Id", "SubDept_Name");
            ViewBag.LevelID = new SelectList(db.Levels.Where(l => l.IsActive == "Yes"), "LevelID", "LevelName");
            ViewBag.BatchID = new SelectList(db.Batches, "BatchID", "BatchName");
            ViewBag.BatchProgramID1 = new SelectList(db.GetBatchProgramNameConcat("", 0), "ID", "Name");
            ViewBag.BatchProgramID2 = new SelectList(db.GetBatchProgramNameConcat("", 0), "ID", "Name");
            ViewBag.BatchProgramID3 = new SelectList(db.GetBatchProgramNameConcat("", 0), "ID", "Name");
            ViewBag.BatchProgramID4 = new SelectList(db.GetBatchProgramNameConcat("", 0), "ID", "Name");
            return View(model);
        }

        [HttpGet]
        public ActionResult Create()
        {
            if (Session["FormNo"] == null)
            {
                TempData["messageType"] = "error";
                TempData["message"] = "Please select applicant from Applicants list.";
                return RedirectToAction("Create", "Applicants");
            }

            int EmpID = Convert.ToInt32(Session["emp_id"]);
            string FormNo = Session["FormNo"].ToString();

            model.SelectedApplicantQualification = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.FormNo = FormNo;
            ViewBag.FacultyID = new SelectList(db.GetSubDepartments_by_HospitalID(MvcApplication.Hospital_ID), "SubDept_Id", "SubDept_Name");
            ViewBag.FacultyID1 = new SelectList(db.GetSubDepartments_by_HospitalID(MvcApplication.Hospital_ID), "SubDept_Id", "SubDept_Name");
            ViewBag.FacultyID2 = new SelectList(db.GetSubDepartments_by_HospitalID(MvcApplication.Hospital_ID), "SubDept_Id", "SubDept_Name");
            ViewBag.LevelID = new SelectList(db.Levels.Where(l => l.IsActive == "Yes"), "LevelID", "LevelName");
            ViewBag.BatchID = new SelectList(db.Batches, "BatchID", "BatchName");
            ViewBag.BatchProgramID1 = new SelectList(db.GetBatchProgramNameConcat("", 0), "ID", "Name");
            ViewBag.BatchProgramID2 = new SelectList(db.GetBatchProgramNameConcat("", 0), "ID", "Name");
            ViewBag.BatchProgramID3 = new SelectList(db.GetBatchProgramNameConcat("", 0), "ID", "Name");
            ViewBag.BatchProgramID4 = new SelectList(db.GetBatchProgramNameConcat("", 0), "ID", "Name");

            return View("Index", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ApplicantQualification aq, FormCollection fc, string FormNo, string SubmitType, string ApplicantStatus)
        {
            if (Session["FormNo"] == null)
            {
                TempData["messageType"] = "error";
                TempData["message"] = "Please select applicant from Applicants list.";
                return RedirectToAction("Create", "Applicants");
            }

            int EmpID = Convert.ToInt32(Session["emp_id"]);
            FormNo = Session["FormNo"].ToString();

            if (SubmitType == "AddQualification")
            {
                InsertQualification(fc, "hdnRowCount");
                InsertQualification(fc, "hdnRowCount1");
                model.SelectedApplicantQualification = null;
                model.DisplayMode = "WriteOnly";
            }

            if (SubmitType == "AddProgram")
            {
                ApplyForProgram afp = new ApplyForProgram();
                ApplyForProgram afpCheck = new ApplyForProgram();
                List<ApplyForProgram> lstPrograms = db.ApplyForPrograms.Where(p => p.FormNo == FormNo).ToList();
                if(lstPrograms.Count > 0)
                {
                    db.ApplyForPrograms.RemoveRange(lstPrograms);
                    db.SaveChanges();
                }
                
                afp.FormNo = aq.FormNo;
                afp.IsActive = "Yes";
                afp.CreatedBy = Convert.ToInt32(Session["emp_id"]);
                afp.CreatedOn = DateTime.Now;

                afpCheck = db.ApplyForPrograms.FirstOrDefault(a => a.ProgramID == aq.BatchProgramID1 && a.FormNo == FormNo);

                if (aq.BatchProgramID1 != null)
                {
                    if (aq.BatchProgramID1 != 0)
                    {
                        afpCheck = db.ApplyForPrograms.FirstOrDefault(a => a.ProgramID == aq.BatchProgramID1 && a.FormNo == FormNo);
                        if (afpCheck == null)
                        {
                            afp.ProgramID = aq.BatchProgramID1;
                            afp.ProgramPriority = 1;
                            db.ApplyForPrograms.Add(afp);
                         
                            //Program Preference 1 will be the Applicant program
                            Applicant app = db.Applicants.FirstOrDefault(a => a.FormNo == FormNo);
                            app.BatchProgramID = Convert.ToInt32(afp.ProgramID);
                            db.Entry(app).State = EntityState.Modified;
                            db.SaveChanges();
                        }

                        if (aq.BatchProgramID2 != null && aq.BatchProgramID2 != 0)
                        {
                            if (aq.BatchProgramID1 != aq.BatchProgramID2)
                            {
                                afpCheck = db.ApplyForPrograms.FirstOrDefault(a => a.ProgramID == aq.BatchProgramID2 && a.FormNo == FormNo);
                                if (afpCheck == null)
                                {
                                    afp.ProgramID = aq.BatchProgramID2;
                                    afp.ProgramPriority = 2;
                                    db.ApplyForPrograms.Add(afp);
                                    db.SaveChanges();
                                }
                            }
                        }

                        if (aq.BatchProgramID3 != null && aq.BatchProgramID3 != 0)
                        {
                            if (aq.BatchProgramID1 != aq.BatchProgramID3 && aq.BatchProgramID2 != aq.BatchProgramID3)
                            {
                                afpCheck = db.ApplyForPrograms.FirstOrDefault(a => a.ProgramID == aq.BatchProgramID3 && a.FormNo == FormNo);
                                if (afpCheck == null)
                                {
                                    afp.ProgramID = aq.BatchProgramID3;
                                    afp.ProgramPriority = 3;
                                    db.ApplyForPrograms.Add(afp);
                                    db.SaveChanges();
                                }
                            }
                        }

                        if (aq.BatchProgramID4 != null && aq.BatchProgramID4 != 0)
                        {
                            if (aq.BatchProgramID1 != aq.BatchProgramID4 && aq.BatchProgramID2
                                != aq.BatchProgramID4 && aq.BatchProgramID3 != aq.BatchProgramID4)
                            {
                                afpCheck = db.ApplyForPrograms.FirstOrDefault(a => a.ProgramID == aq.BatchProgramID4 && a.FormNo == FormNo);
                                if (afpCheck == null)
                                {
                                    afp.ProgramID = aq.BatchProgramID4;
                                    afp.ProgramPriority = 4;
                                    db.ApplyForPrograms.Add(afp);
                                    db.SaveChanges();
                                }
                            }
                        }
                    }
                    else
                    {
                        ViewBag.MessageType = "error";
                        ViewBag.Message = "Please select Program Preference 1";
                    }
                }

                Insert_ApplicantInstituteStatus(FormNo, ApplicantStatus);

                ViewBag.FacultyID = new SelectList(db.GetSubDepartments_by_HospitalID(MvcApplication.Hospital_ID), "SubDept_Id", "SubDept_Name");
                ViewBag.FacultyID1 = new SelectList(db.GetSubDepartments_by_HospitalID(MvcApplication.Hospital_ID), "SubDept_Id", "SubDept_Name");
                ViewBag.FacultyID2 = new SelectList(db.GetSubDepartments_by_HospitalID(MvcApplication.Hospital_ID), "SubDept_Id", "SubDept_Name");
                ViewBag.LevelID = new SelectList(db.Levels.Where(l => l.IsActive == "Yes"), "LevelID", "LevelName");
                ViewBag.BatchID = new SelectList(db.Batches, "BatchID", "BatchName");
                ViewBag.BatchProgramID1 = new SelectList(db.GetBatchProgramNameConcat("", 0), "ID", "Name");
                ViewBag.BatchProgramID2 = new SelectList(db.GetBatchProgramNameConcat("", 0), "ID", "Name");
                ViewBag.BatchProgramID3 = new SelectList(db.GetBatchProgramNameConcat("", 0), "ID", "Name");
                ViewBag.BatchProgramID4 = new SelectList(db.GetBatchProgramNameConcat("", 0), "ID", "Name");

                return RedirectToAction("Index", "ApplicantQualifications");
            }

            ViewBag.FormNo = FormNo;
           
            if (SubmitType == "AddQualification")
            {
                if (Session["error"] != null || EligiblityCriteria())
                {
                    return RedirectToAction("Index", "ApplicantQualifications");
                }
                else
                {
                    return RedirectToAction("ApplicantStatus", "Applicants");
                }
            }
            return RedirectToAction("Index", "ApplicantQualifications");
        }

        private void Insert_ApplicantInstituteStatus(string FormNo, string ApplicantStatus)
        {
            if (!string.IsNullOrEmpty(ApplicantStatus))
            {
                List<ApplicantInstituteStatu> lstStatus = db.ApplicantInstituteStatus.Where(p => p.FormNo == FormNo).ToList();
                if (lstStatus.Count > 0)
                {
                    db.ApplicantInstituteStatus.RemoveRange(lstStatus);
                    db.SaveChanges();
                }

                List<ApplicantInstituteStatu> lst = new List<ApplicantInstituteStatu>();
                string[] ArrApplicantStatus = ApplicantStatus.Split(',');
                db.UpdateStatusByFormNo(0, FormNo, Convert.ToInt32(Session["emp_id"]), 2);
                for (int i = 0; i < ArrApplicantStatus.Length; i++)
                {
                    ApplicantInstituteStatu obj = new ApplicantInstituteStatu();
                    obj.InstituteStatusID = Convert.ToInt32(ArrApplicantStatus[i]);
                    obj.FormNo = FormNo;
                    obj.IsActive = "Yes";
                    obj.CreatedBy = Convert.ToInt32(Session["emp_id"]);
                    obj.CreatedOn = DateTime.Now;
                    lst.Add(obj);

                    if(obj.InstituteStatusID == 7)
                    {
                        // Setting the ApplicantStatus when Result Awaited is selected
                        db.UpdateStatusByFormNo(0, FormNo, obj.CreatedBy, 3);
                    }
                }

                db.ApplicantInstituteStatus.AddRange(lst);
                db.SaveChanges();
            }
        }

        public bool EligiblityCriteria()
        {
            string FormNo = Convert.ToString(Session["FormNo"]);
            bool isNotEligible = false;

            if (!string.IsNullOrEmpty(FormNo))
            {
                //Eligibility Code
                List<ApplicantQualification> lstAQ = db.ApplicantQualifications.Where(q => q.FormNo == FormNo).ToList();
                GetApplicantLevel_Result level = db.GetApplicantLevel(FormNo).FirstOrDefault();
                int? levelID = 0;
                if (level != null)
                {
                    levelID = level.LevelID;
                }

                if (lstAQ.Count > 0)
                {
                    foreach (var item in lstAQ)
                    {
                        Degree d = db.Degrees.FirstOrDefault(deg => deg.DegreeID == item.DegreeID);
                        if (d.DegreeName == "HSSC/A-Level" && item.Percentage < Convert.ToDecimal(45) && levelID == 1)
                        {
                            ViewBag.MessageType = "error";
                            ViewBag.Message = "You are not eligible to apply because your percentage is less than 45 in " + d.DegreeName;
                            isNotEligible = true;
                            break;
                        }
                        else if (d.DegreeName == "Bachelors" && item.Percentage < Convert.ToDecimal(62.5) && levelID == 2)
                        {
                            ViewBag.MessageType = "error";
                            ViewBag.Message = "You are not eligible to apply because your percentage is less than 62.5 in " + d.DegreeName;
                            isNotEligible = true;
                            break;
                        }
                        else if ((d.DegreeName == "Masters" || d.DegreeName == "MS/MPhil") && item.Percentage < Convert.ToDecimal(75) && (levelID == 3 || levelID == 4))
                        {
                            ViewBag.MessageType = "error";
                            ViewBag.Message = "You are not eligible to apply because your percentage is less than 75 in " + d.DegreeName;
                            isNotEligible = true;
                            break;
                        }
                    }
                }
            }
            return isNotEligible;
        }

        private void InsertQualification(FormCollection fc, string hdnRowCount)
        {
            string ErrorMessage = "";
            int messageCount = 0;
            int count = 1;
            string FormNo = Session["FormNo"].ToString();
            string DegreeName = "";

            if (!string.IsNullOrEmpty(FormNo))
            {
                int rowCount = Convert.ToInt32(fc[hdnRowCount]);

                List<ApplicantQualification> lstAQ = new List<ApplicantQualification>();
                if (hdnRowCount == "hdnRowCount1")
                {
                    rowCount = 100 + rowCount;
                    count = 100;
                }

                for (var i = count; i <= rowCount; i++)
                {
                    int DegreeID = Convert.ToInt32(fc["DegreeID_" + i]);
                    ApplicantQualification stds = new ApplicantQualification();

                    stds.DegreeID = DegreeID;
                    DegreeName = fc["DegreeName_" + i];
                    stds.InstituteID = (fc["InstituteID_" + i] == "null") ? 0 : Convert.ToInt32(fc["InstituteID_" + i]);
                    stds.YearQualification = (fc["YearQualification_" + i] == "") ? 0 : Convert.ToInt32(fc["YearQualification_" + i]);
                    stds.TotalMarks = (fc["TotalMarks_" + i] == "") ? 0 : Convert.ToDouble(fc["TotalMarks_" + i]);
                    stds.ObtainedMarks = (fc["ObtainedMarks_" + i] == "") ? 0 : Convert.ToDouble(fc["ObtainedMarks_" + i]);
                    stds.Percentage = (fc["Percentage_" + i] == "") ? 0 : Convert.ToDecimal(fc["Percentage_" + i]);
                    stds.InstituteCity = "Default";
                    stds.InstituteName = fc["InstituteName_" + i];
                    
                    stds.CreatedOn = DateTime.Now;
                    stds.CreatedBy = Convert.ToInt32(Session["emp_id"]);
                    stds.IsActive = "Yes";
                    stds.FormNo = FormNo;
                    stds.DegreeTitle = fc["DegreeTitle_" + i];
                    
                    stds.RollNO = fc["RollNumber_" + i];
                    stds.ValidUpTo = fc["ValidUpTo_" + i];
                    stds.CompleteResultAwaitedID = (fc["CompleteResultAwaitedID_" + i] == "null") ? 0 : Convert.ToInt32(fc["CompleteResultAwaitedID_" + i]);

                    if (stds.InstituteID == 0)
                    {
                        stds.InstituteName = "";
                    }
                    else
                    {
                        stds.InstituteName = db.Institutes.FirstOrDefault(ins => ins.InstituteID == stds.InstituteID).InstituteName;
                        if (stds.InstituteName == "Other")
                        {
                            stds.InstituteName = fc["OtherInstitute_" + i];
                        }                       
                    }


                    if (stds.DegreeTitle == "")
                    {
                        stds.DegreeTitle = "";
                    }
                    else
                    {
                        if (stds.DegreeTitle != null)
                        {
                            stds.DegreeTitle = db.DegreeTitles.FirstOrDefault(ins => ins.DegreeTitleName == stds.DegreeTitle).DegreeTitleName;
                            if (stds.DegreeTitle == "Others")
                            {
                                stds.DegreeTitle = fc["OtherDegree_" + i];
                            }
                        }
                    }

                    if (stds.TotalMarks > 0 && stds.ObtainedMarks > 0)
                    {
                        if (stds.TotalMarks > stds.ObtainedMarks)
                        {
                            stds.Percentage = Convert.ToDecimal((stds.ObtainedMarks / stds.TotalMarks) * 100);

                            if (stds.InstituteName == "" && hdnRowCount == "hdnRowCount")
                            {
                                messageCount++;
                                ErrorMessage += messageCount + "-Please Select Institute for Degree Name : " + DegreeName + ".<br />";
                            }
                        }
                        else
                        {
                            count++;
                            ErrorMessage += count + "-Total Marks must be greater than Obtained Marks.<br />";
                        }
                    }

                    if (string.IsNullOrEmpty(ErrorMessage))
                    {
                        if (stds.TotalMarks > 0 && stds.ObtainedMarks > 0 && stds.Percentage > 0)
                        {
                            if (hdnRowCount == "hdnRowCount1")
                            {
                                stds.InstituteID = null;
                            }
                            ApplicantQualification aQualification = db.ApplicantQualifications.Where(aq => aq.FormNo == FormNo && aq.DegreeID == DegreeID).FirstOrDefault();
                            if (aQualification != null)
                            {
                                aQualification.DegreeID = stds.DegreeID;
                                aQualification.InstituteID = stds.InstituteID;
                                aQualification.YearQualification = stds.YearQualification;
                                aQualification.TotalMarks = stds.TotalMarks;
                                aQualification.ObtainedMarks = stds.ObtainedMarks;
                                aQualification.Percentage = stds.Percentage;
                                aQualification.InstituteCity = stds.InstituteCity;
                                aQualification.InstituteName = stds.InstituteName;
                                aQualification.ModifiedOn = DateTime.Now;
                                aQualification.ModifiedBy = Convert.ToInt32(Session["emp_id"]);
                                aQualification.IsActive = "Yes";
                                aQualification.FormNo = FormNo;
                                aQualification.RollNO = stds.RollNO;
                                aQualification.ValidUpTo = stds.ValidUpTo;
                                aQualification.CompleteResultAwaitedID = stds.CompleteResultAwaitedID;
                                db.Entry(aQualification).State = EntityState.Modified;
                                db.SaveChanges();
                            }
                            else
                            {
                                lstAQ.Add(stds);
                            }
                        }
                    }
                    else
                    {
                        ViewBag.MessageType = "error";
                        ViewBag.Message = ErrorMessage;
                        ViewBag.FormNo = FormNo;
                    }
                }

                if (lstAQ.Count > 0)
                {
                    try
                    {
                        db.ApplicantQualifications.AddRange(lstAQ);
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

                if (!string.IsNullOrEmpty(ErrorMessage))
                {
                    Session.Add("error", ErrorMessage);
                }
            }
            else
            {
                ViewBag.MessageType = "error";
                ViewBag.Message = "Form # is required.";
            }
            ViewBag.FormNo = FormNo;
        }

        [HttpGet]
        public ActionResult ChooseProgram()
        {
            if(Session["FormNo"] == null)
            {
                TempData["messageType"] = "error";
                TempData["message"] = "Please select applicant from Applicants list.";
                return RedirectToAction("Create", "Applicants");
            }

            int EmpID = Convert.ToInt32(Session["emp_id"]);
            string FormNo = Session["FormNo"].ToString();

            if (EmpID == 0)
            {
                return RedirectToAction("Login2", "Home");
            }
            else
            {
                if (!string.IsNullOrEmpty(FormNo))
                {
                    Applicant app = db.Applicants.FirstOrDefault(a => a.FormNo == FormNo);
                    if (app == null)
                    {
                        TempData["messageType"] = "error";
                        TempData["message"] = "Please fill Personal Detail.";
                        return RedirectToAction("Create", "Applicants");
                    }
                }
                else
                {
                    TempData["messageType"] = "error";
                    TempData["message"] = "Please fill Personal Detail.";
                    return RedirectToAction("Create", "Applicants");
                }
            }

            model.SelectedApplicantQualification = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.MessageType = "";
            ViewBag.Message = "";

            ViewBag.FormNo = FormNo;
            ViewBag.FacultyID = new SelectList(db.GetSubDepartments_by_HospitalID(MvcApplication.Hospital_ID), "SubDept_Id", "SubDept_Name");
            ViewBag.FacultyID1 = new SelectList(db.GetSubDepartments_by_HospitalID(MvcApplication.Hospital_ID), "SubDept_Id", "SubDept_Name");
            ViewBag.FacultyID2 = new SelectList(db.GetSubDepartments_by_HospitalID(MvcApplication.Hospital_ID), "SubDept_Id", "SubDept_Name");
            ViewBag.LevelID = new SelectList(db.Levels.Where(l => l.IsActive == "Yes"), "LevelID", "LevelName");
            ViewBag.BatchID = new SelectList(db.Batches, "BatchID", "BatchName");
            ViewBag.BatchProgramID1 = new SelectList(db.GetBatchProgramNameConcat("", 0), "ID", "Name");
            ViewBag.BatchProgramID2 = new SelectList(db.GetBatchProgramNameConcat("", 0), "ID", "Name");
            ViewBag.BatchProgramID3 = new SelectList(db.GetBatchProgramNameConcat("", 0), "ID", "Name");
            ViewBag.BatchProgramID4 = new SelectList(db.GetBatchProgramNameConcat("", 0), "ID", "Name");
            return View();
        }

        public ActionResult Declaration()
        {
            int EmpID = Convert.ToInt32(Session["emp_id"]);
            if (EmpID == 0)
            {
                return RedirectToAction("Login2", "Home");
            }
            else
            {
                if (Session["FormNo"] == null)
                {
                    TempData["messageType"] = "error";
                    TempData["message"] = "Please select applicant from Applicants list.";
                    return RedirectToAction("Create", "Applicants");
                }

                string FormNo = Session["FormNo"].ToString();

                ViewBag.FormNo = FormNo;

                if (!string.IsNullOrEmpty(FormNo))
                {
                    ApplicantQualification appq = db.ApplicantQualifications.FirstOrDefault(a => a.FormNo == FormNo);
                    if (appq == null)
                    {
                        TempData["messageType"] = "error";
                        TempData["message"] = "Please fill Academic History.";
                        return RedirectToAction("Index", "ApplicantQualifications");
                    }

                    //ApplicantUpload appu = db.ApplicantUploads.FirstOrDefault(a => a.FormNo == FormNo);
                    //if (appu == null)
                    //{
                    //    return RedirectToAction("Index", "ApplicantUploadsApplicants");
                    //}

                    ApplyForProgram appap = db.ApplyForPrograms.FirstOrDefault(a => a.FormNo == FormNo);
                    if (appap == null)
                    {
                        TempData["messageType"] = "error";
                        TempData["message"] = "Please Choose Programs.";
                        return RedirectToAction("ChooseProgram", "ApplicantQualifications");
                    }
                    Applicant ap = db.Applicants.FirstOrDefault(a => a.FormNo == FormNo);
                    if (ap.DisciplinaryIssue == null)
                    {
                        TempData["error"] = "-- Disciplinary Issues is required --";
                        TempData["messageType"] = "error";
                        TempData["message"] = "Please fill Applicant Status.";
                        return RedirectToAction("ApplicantStatus", "Applicants");
                    }
                }
                else
                {
                    TempData["messageType"] = "error";
                    TempData["message"] = "Please fill Personal Detail.";
                    return RedirectToAction("Create", "Applicants");
                }
            }
            return View();
        }

        [HttpPost]
        public ActionResult Declaration(string FormNo)
        {
            if (Session["FormNo"] == null)
            {
                TempData["messageType"] = "error";
                TempData["message"] = "Please select applicant from Applicants list.";
                return RedirectToAction("Create", "Applicants");
            }

            FormNo = Session["FormNo"].ToString();

            string ChallanPath = "";
            ViewBag.FormNo = FormNo;
            int EmpID = Convert.ToInt32(Session["emp_id"]);
            if (EmpID == 0)
            {
                return RedirectToAction("Login2", "Home");
            }
            else
            {
                Applicant app = db.Applicants.FirstOrDefault(a => a.FormNo == FormNo);
                if (app != null)
                {
                    if (!string.IsNullOrEmpty(app.ChallanImagePath))
                    {
                        ChallanPath = app.ChallanImagePath;
                    }
                }

                db.Entry(app).State = EntityState.Modified;
                app.ModifiedOn = DateTime.Now;
                app.ModifiedBy = Convert.ToInt32(Session["emp_id"]);
                app.StatusID = 8;
                //try
                //{
                //    if (string.IsNullOrEmpty(ChallanPath))
                //    {
                //        ViewBag.MessageType = "error";
                //        ViewBag.Message = "Please upload bank challan first.";
                //    }
                //    else
                //    {
                //        db.SaveChanges();
                //        ViewBag.MessageType = "success";
                //        ViewBag.Message = "Application has been submitted successfully.";
                //    }
                //}
                try
                {
                    db.SaveChanges();
                    ViewBag.MessageType = "success";
                    ViewBag.Message = "Application has been submitted successfully.";
                }
                catch (DbUpdateException ex)
                {
                    ViewBag.MessageType = "error";
                    ViewBag.Message = ex.Message;
                }
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UploadChallan(string FormNo, HttpPostedFileBase ChallanImage)
        {
            FormNo = Session["FormNo"].ToString();
            int EmpID = Convert.ToInt32(Session["emp_id"]);
            if (EmpID == 0)
            {
                return RedirectToAction("Login2", "Home");
            }
            else
            {
                Applicant applicant = db.Applicants.FirstOrDefault(a => a.FormNo == FormNo);

                db.Entry(applicant).State = EntityState.Modified;
                applicant.ModifiedOn = DateTime.Now;
                applicant.ModifiedBy = Convert.ToInt32(Session["emp_id"]);
                try
                {
                    if (ChallanImage != null)
                    {
                        applicant.ChallanImagePath = string.Concat("~/ChallanImages/", applicant.FormNo, "_", ChallanImage.FileName);
                        ChallanImage.SaveAs(Server.MapPath(applicant.ChallanImagePath));
                        Session["ChallanImage"] = ChallanImage;
                    }
                    else if (Session["ChallanImage"] != null)
                    {
                        applicant.ChallanImage = (HttpPostedFileBase)Session["ChallanImage"];
                        applicant.ChallanImagePath = string.Concat("~/ChallanImages/", applicant.FormNo, "_", applicant.ChallanImage.FileName);
                    }
                    else
                    {
                        count++;
                        ErrorMessage += count + "-Bank Challan Slip is required.<br />";
                    }
                }
                catch (Exception)
                {
                    count++;
                    ErrorMessage += count + "-Bank Challan Slip is required.<br />";
                }

                try
                {
                    if (string.IsNullOrEmpty(ErrorMessage))
                    {
                        db.SaveChanges();
                        ViewBag.MessageType = "success";
                        ViewBag.Message = "Challan has been uploaded successfully.";
                        return RedirectToAction("Declaration", "ApplicantQualifications");
                    }
                    else
                    {
                        ViewBag.MessageType = "error";
                        ViewBag.Message = ErrorMessage;
                        return RedirectToAction("Declaration", "ApplicantQualifications");
                    }
                }
                catch (DbUpdateException ex)
                {
                    ViewBag.MessageType = "error";
                    ViewBag.Message = ex.Message;
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }

            return RedirectToAction("Declaration", "ApplicantQualifications");
        }

        // Get programs by Faculty and Batch.
        public JsonResult GetPrograms_by_FacultyLevelBatch(string FacultyID, string BatchID, string LevelID, string QueryID)
        {
            List<GetPrograms_by_FacultyLevelBatch_Result> lstPrograms = new List<GetPrograms_by_FacultyLevelBatch_Result>();
            Batch batch = db.Batches.FirstOrDefault(b => b.IsActive == "Current");
            if (batch != null)
            {
                BatchID = batch.BatchID.ToString();
            }

            lstPrograms = db.GetPrograms_by_FacultyLevelBatch(Convert.ToInt32(FacultyID), Convert.ToInt32(LevelID), Convert.ToInt32(BatchID), Convert.ToInt32(QueryID)).ToList();
            var programs = lstPrograms.Select(p => new
            {
                BatchProgramID = p.BatchProgramID,
                ProgramName = p.ProgramName
            });
            string result = JsonConvert.SerializeObject(programs, Formatting.Indented);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetPrograms_by_FacultyLevelBatchFilter(string FacultyID, string BatchID, string LevelID, string QueryID, string BatchProgramID)
        {
            List<GetPrograms_by_FacultyLevelBatch_Result> lstPrograms = new List<GetPrograms_by_FacultyLevelBatch_Result>();
            Batch batch = db.Batches.FirstOrDefault(b => b.IsActive == "Current");
            if (batch != null)
            {
                BatchID = batch.BatchID.ToString();
            }

            string[] array = null;
            if (!string.IsNullOrEmpty(BatchProgramID))
            {
                array = BatchProgramID.Split(',');
            }

            if (array != null)
            {
                if (array.Length == 1)
                {
                    lstPrograms = db.GetPrograms_by_FacultyLevelBatch(Convert.ToInt32(FacultyID), Convert.ToInt32(LevelID), Convert.ToInt32(BatchID), Convert.ToInt32(QueryID)).Where(x => x.BatchProgramID != Convert.ToInt32(array[0])).ToList();
                }

                if (array.Length == 2)
                {
                    lstPrograms = db.GetPrograms_by_FacultyLevelBatch(Convert.ToInt32(FacultyID), Convert.ToInt32(LevelID), Convert.ToInt32(BatchID), Convert.ToInt32(QueryID)).Where(x => x.BatchProgramID != Convert.ToInt32(array[0])).ToList();
                    if (array[1] != "null")
                    {
                        lstPrograms = lstPrograms.Where(x => x.BatchProgramID != Convert.ToInt32(array[1])).ToList();
                    }
                }
            }

            var programs = lstPrograms.Select(p => new
            {
                BatchProgramID = p.BatchProgramID,
                ProgramName = p.ProgramName
            });
            string result = JsonConvert.SerializeObject(programs, Formatting.Indented);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetSelectedPrograms()
        {
            string FormNo = Session["FormNo"].ToString();
            List<GetSelectedProgramsOfApplicant_Result> Ap = db.GetSelectedProgramsOfApplicant(FormNo).ToList();
            var programs = Ap.Select(p => new
            {
                BatchProgramID = p.BatchProgramID,
                FacultyID = p.FacultyID,
                ProgramPriority = p.ProgramPriority
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
