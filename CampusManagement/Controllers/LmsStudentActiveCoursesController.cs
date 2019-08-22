using CampusManagement.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace CampusManagement.Controllers
{
    public class LmsStudentActiveCoursesController : Controller
    {
        int count = 0;
        string ErrorMessage = "";
        private ModelCMSNewContainer db = new ModelCMSNewContainer();
        LMSSubjectLearningViewModel model = new LMSSubjectLearningViewModel();
        List<GetQuizByID_Result> Listglobal = new List<GetQuizByID_Result>();



        public ActionResult StudentActiveCourses()
        {
            ViewBag.BatchID = new SelectList(db.Batches, "BatchID", "BatchName");
            ViewBag.BatchProgramID = new SelectList(db.GetBatchProgramNameConcat("", 0).ToList(), "ID", "Name");
            ViewBag.YearSemesterNo = new SelectList(db.Semesters, "YearSemesterNo", "YearSemesterNo");

            ViewBag.MessageType = "";
            ViewBag.Message = "";
            return View();
        }

        public ActionResult Index(int? LMSSubjectLearningID, string LSTitle)
        {
            LMSWeekViewModel model = new LMSWeekViewModel();
            model.LMSWeeks = db.LMSWeeks.Where(x => x.LMSSubjectLearningID == LMSSubjectLearningID).OrderByDescending(a => a.LMSSubjectLearningID).ToList();
            model.SelectedLMSWeeks = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc");
            ViewBag.MessageType = "";
            ViewBag.LMSSubjectLearningID = LMSSubjectLearningID;
            ViewBag.LSTitle = LSTitle;
            ViewBag.Message = "";
            return View(model);
        }

        public ActionResult WelcomeQuizPage(int? LMSQuizID, int? LMSSubjectLearningID, string LSTitle, int? IsValidAttempts)
        {
            Session["GetQuizByID_List"] = null;
            if (LMSQuizID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (IsValidAttempts == -1)
            {
                 ViewBag.MessageType = "error";
                 ViewBag.Message = "Your Attempts are over.";
               
            }
            else if (IsValidAttempts == -2)
            {

                ViewBag.MessageType = "error";
                ViewBag.Message = "Your Date To attempt test are over.";
                
            }
            LMSQuiz Quiz = db.LMSQuizs.Find(LMSQuizID);
            if (Quiz == null)
            {
                return HttpNotFound();
            }
            ViewBag.LMSQuizID = LMSQuizID;
            ViewBag.LMSSubjectLearningID = LMSSubjectLearningID;
            ViewBag.LSTitle = LSTitle;

            return View(Quiz);
        }


        public ActionResult StudentViewQuizAfterVarify(int? LMSQuizID, int? LMSSubjectLearningID, string LSTitle,int? currentindex, int? IsValidAttempts, int? LMSStudentQuizAttemptID)
        {
            // LMSQuizIQuestionViewModel model = new LMSQuizIQuestionViewModel();
            QuizQuestionAttemptViewModel model = new QuizQuestionAttemptViewModel();
            List<GetQuizByID_Result> List = new List<GetQuizByID_Result>();
            if (Session["GetQuizByID_List"] != null) {
                List = (List<GetQuizByID_Result>)Session["GetQuizByID_List"];
            }
            

            InsertOrValidateStudentQuizAttempt_Result InsertOrValidateStudentQuizAttempt_ResultObj = new InsertOrValidateStudentQuizAttempt_Result();
            int StudentID = Convert.ToInt32(Session["CurrentUserID"]);
            int CurrentUserID = Convert.ToInt32(Session["CurrentUserID"]);
            //int CurrentUserID = Convert.ToInt32(Session["emp_id"]);
            try
            {
                if (currentindex == null)
                {
                    InsertOrValidateStudentQuizAttempt_ResultObj = db.InsertOrValidateStudentQuizAttempt(LMSQuizID, StudentID, CurrentUserID, CurrentUserID).FirstOrDefault();
                }
                else {
                    InsertOrValidateStudentQuizAttempt_ResultObj.IsValidAttempts = IsValidAttempts;
                    InsertOrValidateStudentQuizAttempt_ResultObj.LMSStudentQuizAttemptID = LMSStudentQuizAttemptID;
                }
                if (InsertOrValidateStudentQuizAttempt_ResultObj.IsValidAttempts == 1)
                {
                    if (currentindex == null )
                    {
                        ViewBag.Iindex = 0;
                       
                    }
                    else {

                        ViewBag.Iindex = currentindex;
                    }

                    ViewBag.LMSSubjectLearningID = LMSSubjectLearningID;
                    ViewBag.LSTitle = LSTitle;
                    ViewBag.IsValidAttempts = InsertOrValidateStudentQuizAttempt_ResultObj.IsValidAttempts;
                    ViewBag.LMSStudentQuizAttemptID = InsertOrValidateStudentQuizAttempt_ResultObj.LMSStudentQuizAttemptID;
                    if (List.Count  == 0)
                    {
                        model.QuizQuestionList = db.GetQuizByID(LMSQuizID).ToList();
                       
                    }
                    else {
                        model.QuizQuestionList = List;
                    }
                    

                   // model.QuizQuestionMCQList = List.Where(x => x.LMSQuestionTypeID == 1).ToList();
                   // model.QuizQuestionSCQList = List.Where(x => x.LMSQuestionTypeID == 2).ToList();
                  //  model.QuizQuestionTrueFalseList = List.Where(x => x.LMSQuestionTypeID == 3).ToList();

                  
                    ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc");
                    ViewBag.LMSQuestionTypeID = new SelectList(db.LMSQuestionTypes, "LMSQuestionTypeID", "QuestionTypeName");

                }
                else if (InsertOrValidateStudentQuizAttempt_ResultObj.IsValidAttempts == -1)
                {
                  
                    return RedirectToAction("WelcomeQuizPage", new { LMSQuizID, LMSSubjectLearningID, LSTitle, InsertOrValidateStudentQuizAttempt_ResultObj.IsValidAttempts });
                  
                }
                else if (InsertOrValidateStudentQuizAttempt_ResultObj.IsValidAttempts == -2)
                {
                    return RedirectToAction("WelcomeQuizPage" , new {LMSQuizID, LMSSubjectLearningID,LSTitle, InsertOrValidateStudentQuizAttempt_ResultObj.IsValidAttempts });
                    
                }

               
            }
            catch (DbUpdateException ex)
            {
                ViewBag.MessageType = "error";
                ViewBag.Message = ex.Message;
                ModelState.AddModelError(string.Empty, ex.Message);
            }
            return View(model);
        }
        [HttpPost]
        public ActionResult StudentsQuizAttempt(QuizQuestionAttemptViewModel ListObj,FormCollection fc)
        {
            
            int currentindex = Convert.ToInt32(fc["Iindex"]);
            int LMSSubjectLearningID = Convert.ToInt32(fc["LMSSubjectLearningID"]);
            int IsValidAttempts = Convert.ToInt32(fc["IsValidAttempts"]);
            int LMSStudentQuizAttemptID = Convert.ToInt32(fc["LMSStudentQuizAttemptID"]);
            string  LSTitle = fc["LSTitle"];
            string LMSQuizID_str = "QuizQuestionList[" + currentindex + "].LMSQuizID";
            string LMSQuestionTypeID_str = "QuizQuestionList[" + currentindex + "].LMSQuestionTypeID";
            string LMSQuizIQuestionD_str = "QuizQuestionList[" + currentindex + "].LMSQuizIQuestionD";
            string QuestionMarks_str = "QuizQuestionList[" + currentindex + "].QuestionMarks";
            
            string LMSStudentQuizAttemptID_str = "QuizQuestionList[" + currentindex + "].LMSStudentQuizAttemptID";
            string AnswerMCQ1_str = "QuizQuestionList[" + currentindex + "].AnswerMCQ1";
            string AnswerMCQ2_str = "QuizQuestionList[" + currentindex + "].AnswerMCQ2";
            string AnswerMCQ3_str = "QuizQuestionList[" + currentindex + "].AnswerMCQ3";
            string AnswerMCQ4_str = "QuizQuestionList[" + currentindex + "].AnswerMCQ4";

            string AnswerSCQ1_str = "QuizQuestionList[" + currentindex + "].AnswerSCQ1";
            string AnswerSCQ2_str = "QuizQuestionList[" + currentindex + "].AnswerSCQ2";
            string AnswerSCQ3_str = "QuizQuestionList[" + currentindex + "].AnswerSCQ3";
            string AnswerSCQ4_str = "QuizQuestionList[" + currentindex + "].AnswerSCQ4";

            string AnswerTrueFalse_str = "QuizQuestionList[" + currentindex + "].AnswerTrueFalse";

           

            //QuizQuestionList[2].AnswerSCQ1

            // var AnswerSCQ = fc["AnswerSCQ"];

            int LMSQuizID = Convert.ToInt32(fc[LMSQuizID_str]);
            int LMSQuizIQuestionD = Convert.ToInt32(fc[LMSQuizIQuestionD_str]);
            //int LMSStudentQuizAttemptID = Convert.ToInt32(fc[LMSStudentQuizAttemptID_str]);
            int LMSQuestionTypeID = Convert.ToInt32(fc[LMSQuestionTypeID_str]);
            decimal QuestionMarks = Convert.ToDecimal(fc[QuestionMarks_str]);
            bool AnswerMCQ1 = false;
            bool AnswerMCQ2 = false;
            bool AnswerMCQ3 = false;
            bool AnswerMCQ4 = false;

            bool AnswerSCQ1 = false;
            bool AnswerSCQ2 = false;
            bool AnswerSCQ3 = false;
            bool AnswerSCQ4 = false;

            bool AnswerTrueFalse = false;



            string AnsMCQ1 = fc[AnswerMCQ1_str];
            string[] AnsMCQ1array;
            if (AnsMCQ1 != null) {
                AnsMCQ1array = AnsMCQ1.Split(',');
                AnswerMCQ1 = Convert.ToBoolean(AnsMCQ1array[0]);
            }
            
            
            string AnsMCQ2 = fc[AnswerMCQ2_str];
            string[] AnsMCQ2array;
            if (AnsMCQ2 != null)
            {
                AnsMCQ2array = AnsMCQ2.Split(',');
                AnswerMCQ2 = Convert.ToBoolean(AnsMCQ2array[0]);
            }
           
            string AnsMCQ3 = fc[AnswerMCQ3_str];
            string[] AnsMCQ3array;
            if (AnsMCQ3 != null)
            {
                AnsMCQ3array = AnsMCQ3.Split(',');
                AnswerMCQ3 = Convert.ToBoolean(AnsMCQ3array[0]);
            }
            
            string AnsMCQ4 = fc[AnswerMCQ4_str];
            string[] AnsMCQ4array;
            if (AnsMCQ4 != null)
            {
                AnsMCQ4array = AnsMCQ4.Split(',');
                AnswerMCQ4 = Convert.ToBoolean(AnsMCQ4array[0]);
            }




            string AnsSCQ1 = fc[AnswerSCQ1_str];
            string[] AnsSCQ1array;
            if (AnsSCQ1 != null)
            {
                AnsSCQ1array = AnsSCQ1.Split(',');
                AnswerSCQ1 = Convert.ToBoolean(AnsSCQ1array[0]);
            }


            string AnsSCQ2 = fc[AnswerSCQ2_str];
            string[] AnsSCQ2array;
            if (AnsSCQ2 != null)
            {
                AnsSCQ2array = AnsSCQ2.Split(',');
                AnswerSCQ2 = Convert.ToBoolean(AnsSCQ2array[0]);
            }

            string AnsSCQ3 = fc[AnswerSCQ3_str];
            string[] AnsSCQ3array;
            if (AnsSCQ3 != null)
            {
                AnsSCQ3array = AnsSCQ3.Split(',');
                AnswerSCQ3 = Convert.ToBoolean(AnsSCQ3array[0]);
            }

            string AnsSCQ4 = fc[AnswerSCQ4_str];
            string[] AnsSCQ4array;
            if (AnsSCQ4 != null)
            {
                AnsSCQ4array = AnsSCQ4.Split(',');
                AnswerSCQ4 = Convert.ToBoolean(AnsSCQ4array[0]);
            }


            string AnsTrueFalse = fc[AnswerTrueFalse_str];
            string[] AnsTrueFalsearray;
            if (AnsTrueFalse != null)
            {
                AnsTrueFalsearray = AnsTrueFalse.Split(',');
                AnswerTrueFalse = Convert.ToBoolean(AnsTrueFalsearray[0]);
            }

            // int LMSQuizID = 0;

            if (Session["GetQuizByID_List"] == null)
            {
                Listglobal = db.GetQuizByID(LMSQuizID).ToList();
            }
            else {
                Listglobal = (List<GetQuizByID_Result>)Session["GetQuizByID_List"];
            }
                
            GetQuizByID_Result QuizObj = new GetQuizByID_Result();
            QuizObj.LMSQuizID = LMSQuizID;
            QuizObj.LMSQuizIQuestionD = LMSQuizIQuestionD;
            QuizObj.LMSStudentQuizAttemptID = LMSStudentQuizAttemptID;
            QuizObj.LMSQuestionTypeID = LMSQuestionTypeID;
            QuizObj.AnswerMCQ1 = AnswerMCQ1;
            QuizObj.AnswerMCQ2 = AnswerMCQ2;
            QuizObj.AnswerMCQ3 = AnswerMCQ3;
            QuizObj.AnswerMCQ4 = AnswerMCQ4;
            QuizObj.AnswerSCQ1 = AnswerSCQ1;
            QuizObj.AnswerSCQ2 = AnswerSCQ2;
            QuizObj.AnswerSCQ3 = AnswerSCQ3;
            QuizObj.AnswerSCQ4 = AnswerSCQ4;
            QuizObj.AnswerTrueFalse = AnswerTrueFalse;
            QuizObj.QuestionMarks = QuestionMarks;
            Listglobal[currentindex] = QuizObj;

            //foreach (GetQuizByID_Result Obj in ListObj.QuizQuestionList )
            //{
            //    List = db.GetQuizByID(Obj.LMSQuizID).ToList();
            //    //LMSQuizID = Obj.LMSQuizID;
            //    List[currentindex] = Obj;

            //}
            currentindex = currentindex + 1;
            Session["GetQuizByID_List"] = Listglobal;
            // return RedirectToAction("StudentAttendanceSummary", new { ProgramCourseID = ListObj.ProgramCourseID });
            return RedirectToAction("StudentViewQuizAfterVarify", new { LMSQuizID,LMSSubjectLearningID, LSTitle, currentindex, IsValidAttempts, LMSStudentQuizAttemptID });
        }

        [HttpPost]
        public ActionResult StudentsFinishQuizAttempt()
        {
            LMSQuizIQuestion LMSQuizIQuestionObj  ;
            List<GetQuizByID_Result> FilterdList = new List<GetQuizByID_Result>();
            GetQuizByID_Result GetQuizByID_ResultObj = new GetQuizByID_Result();
            if (Session["GetQuizByID_List"] != null)
            {
                Listglobal = (List<GetQuizByID_Result>)Session["GetQuizByID_List"];
            }
            // int EmpID = Convert.ToInt32(Session["emp_id"]);
            int EmpID = Convert.ToInt32(Session["CurrentUserID"]);
            int StudentID = Convert.ToInt32(Session["CurrentUserID"]);
            GetQuizByID_ResultObj = Listglobal[0];
            db.DeleteLMSStudentQuizAttemptDetail(GetQuizByID_ResultObj.LMSStudentQuizAttemptID, GetQuizByID_ResultObj.LMSQuizID, StudentID);

            foreach (GetQuizByID_Result Obj in Listglobal) {
                if (Obj.LMSQuestionTypeID == 1) {
                    LMSQuizIQuestionObj = new LMSQuizIQuestion();
                    LMSQuizIQuestionObj = db.LMSQuizIQuestions.Where(x => x.LMSQuizIQuestionD == Obj.LMSQuizIQuestionD).FirstOrDefault();
                    if (Obj.AnswerMCQ1 == LMSQuizIQuestionObj.AnswerMCQ1 && Obj.AnswerMCQ2 == LMSQuizIQuestionObj.AnswerMCQ2 && Obj.AnswerMCQ3 == LMSQuizIQuestionObj.AnswerMCQ3 && Obj.AnswerMCQ4 == LMSQuizIQuestionObj.AnswerMCQ4) {

                        db.InsertLMSStudentQuizAttemptDetail(
                                                            Obj.LMSStudentQuizAttemptID,
                                                            Obj.LMSQuizIQuestionD,
                                                            Obj.LMSQuizID,
                                                            Obj.LMSQuestionTypeID,
                                                            StudentID,
                                                            false,
                                                            Obj.AnswerMCQ1,
                                                            Obj.AnswerMCQ2,
                                                            Obj.AnswerMCQ3,
                                                            Obj.AnswerMCQ4,
                                                            Obj.AnswerTrueFalse,
                                                            Obj.AnswerSCQ1,
                                                            Obj.AnswerSCQ2,
                                                            Obj.AnswerSCQ3,
                                                            Obj.AnswerSCQ4,
                                                            true,
                                                            Obj.QuestionMarks,
                                                            Obj.QuestionMarks,
                                                            DateTime.Now,
                                                            EmpID,
                                                            "Yes",
                                                            DateTime.Now,
                                                            EmpID);
                        // Insert Mark Function Here 
                    }
                    else
                    {
                        db.InsertLMSStudentQuizAttemptDetail(
                                                           Obj.LMSStudentQuizAttemptID,
                                                           Obj.LMSQuizIQuestionD,
                                                           Obj.LMSQuizID,
                                                           Obj.LMSQuestionTypeID,
                                                           StudentID,
                                                           false,
                                                           Obj.AnswerMCQ1,
                                                           Obj.AnswerMCQ2,
                                                           Obj.AnswerMCQ3,
                                                           Obj.AnswerMCQ4,
                                                           Obj.AnswerTrueFalse,
                                                           Obj.AnswerSCQ1,
                                                           Obj.AnswerSCQ2,
                                                           Obj.AnswerSCQ3,
                                                           Obj.AnswerSCQ4,
                                                           false,
                                                           Obj.QuestionMarks,
                                                           0,
                                                           DateTime.Now,
                                                           EmpID,
                                                           "Yes",
                                                           DateTime.Now,
                                                           EmpID);
                    }

                }
                else if(Obj.LMSQuestionTypeID == 2) {
                    LMSQuizIQuestionObj = new LMSQuizIQuestion();
                    LMSQuizIQuestionObj = db.LMSQuizIQuestions.Where(x => x.LMSQuizIQuestionD == Obj.LMSQuizIQuestionD).FirstOrDefault();
                    if (Obj.AnswerSCQ1 == LMSQuizIQuestionObj.AnswerSCQ1 && Obj.AnswerSCQ2 == LMSQuizIQuestionObj.AnswerSCQ2 && Obj.AnswerSCQ3 == LMSQuizIQuestionObj.AnswerSCQ3 && Obj.AnswerSCQ4 == LMSQuizIQuestionObj.AnswerSCQ4)
                    {
                        db.InsertLMSStudentQuizAttemptDetail(
                                                          Obj.LMSStudentQuizAttemptID,
                                                          Obj.LMSQuizIQuestionD,
                                                          Obj.LMSQuizID,
                                                          Obj.LMSQuestionTypeID,
                                                          StudentID,
                                                          false,
                                                          Obj.AnswerMCQ1,
                                                          Obj.AnswerMCQ2,
                                                          Obj.AnswerMCQ3,
                                                          Obj.AnswerMCQ4,
                                                          Obj.AnswerTrueFalse,
                                                          Obj.AnswerSCQ1,
                                                          Obj.AnswerSCQ2,
                                                          Obj.AnswerSCQ3,
                                                          Obj.AnswerSCQ4,
                                                          true,
                                                          Obj.QuestionMarks,
                                                           Obj.QuestionMarks,
                                                          DateTime.Now,
                                                          EmpID,
                                                          "Yes",
                                                          DateTime.Now,
                                                          EmpID);
                    }
                    else
                    {
                        db.InsertLMSStudentQuizAttemptDetail(
                                                       Obj.LMSStudentQuizAttemptID,
                                                       Obj.LMSQuizIQuestionD,
                                                       Obj.LMSQuizID,
                                                       Obj.LMSQuestionTypeID,
                                                       StudentID,
                                                       false,
                                                       Obj.AnswerMCQ1,
                                                       Obj.AnswerMCQ2,
                                                       Obj.AnswerMCQ3,
                                                       Obj.AnswerMCQ4,
                                                       Obj.AnswerTrueFalse,
                                                       Obj.AnswerSCQ1,
                                                       Obj.AnswerSCQ2,
                                                       Obj.AnswerSCQ3,
                                                       Obj.AnswerSCQ4,
                                                       false,
                                                       Obj.QuestionMarks,
                                                       0,
                                                       DateTime.Now,
                                                       EmpID,
                                                       "Yes",
                                                       DateTime.Now,
                                                       EmpID);
                    }

                }
                else if (Obj.LMSQuestionTypeID == 3)
                {
                    LMSQuizIQuestionObj = new LMSQuizIQuestion();
                    LMSQuizIQuestionObj = db.LMSQuizIQuestions.Where(x => x.LMSQuizIQuestionD == Obj.LMSQuizIQuestionD).FirstOrDefault();
                    if (Obj.AnswerTrueFalse == LMSQuizIQuestionObj.AnswerTrueFalse)
                    {
                        db.InsertLMSStudentQuizAttemptDetail(
                                                       Obj.LMSStudentQuizAttemptID,
                                                       Obj.LMSQuizIQuestionD,
                                                       Obj.LMSQuizID,
                                                       Obj.LMSQuestionTypeID,
                                                       StudentID,
                                                       false,
                                                       Obj.AnswerMCQ1,
                                                       Obj.AnswerMCQ2,
                                                       Obj.AnswerMCQ3,
                                                       Obj.AnswerMCQ4,
                                                       Obj.AnswerTrueFalse,
                                                       Obj.AnswerSCQ1,
                                                       Obj.AnswerSCQ2,
                                                       Obj.AnswerSCQ3,
                                                       Obj.AnswerSCQ4,
                                                       true,
                                                       Obj.QuestionMarks,
                                                       Obj.QuestionMarks,
                                                       DateTime.Now,
                                                       EmpID,
                                                       "Yes",
                                                       DateTime.Now,
                                                       EmpID);
                    }
                    else {
                        
                        db.InsertLMSStudentQuizAttemptDetail(
                                                       Obj.LMSStudentQuizAttemptID,
                                                       Obj.LMSQuizIQuestionD,
                                                       Obj.LMSQuizID,
                                                       Obj.LMSQuestionTypeID,
                                                       StudentID,
                                                       false,
                                                       Obj.AnswerMCQ1,
                                                       Obj.AnswerMCQ2,
                                                       Obj.AnswerMCQ3,
                                                       Obj.AnswerMCQ4,
                                                       Obj.AnswerTrueFalse,
                                                       Obj.AnswerSCQ1,
                                                       Obj.AnswerSCQ2,
                                                       Obj.AnswerSCQ3,
                                                       Obj.AnswerSCQ4,
                                                       false,
                                                       Obj.QuestionMarks,
                                                       0,
                                                       DateTime.Now,
                                                       EmpID,
                                                       "Yes",
                                                       DateTime.Now,
                                                       EmpID);
                    }


                }
            }
           
             Session["GetQuizByID_List"] = null;  //ToDo Yet

            return View();
        }
        
        public ActionResult StudentQuizAttempt(int? LMSQuizID)
        {
            LMSQuizIQuestionViewModel model = new LMSQuizIQuestionViewModel();
            model.LMSQuizIQuestions = db.LMSQuizIQuestions.Where(x => x.LMSQuizID == LMSQuizID).OrderBy(a => a.LMSQuestionTypeID).ToList();
            model.SelectedLMSQuizIQuestions = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc");
            ViewBag.LMSQuestionTypeID = new SelectList(db.LMSQuestionTypes, "LMSQuestionTypeID", "QuestionTypeName");
            ViewBag.MessageType = "";
            ViewBag.LMSQuizID = LMSQuizID;
            ViewBag.Message = "";
            return View(model);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(LMSSubjectLearning LMS)
        {
            try
            {
                LMSSubjectLearning d = db.LMSSubjectLearnings.FirstOrDefault(de => de.Title == LMS.Title);

                if (d == null)
                {
                    try
                    {
                        LMS.FilePath = string.Concat("~/DegreeDocument/", DateTime.Now.Ticks, "_", LMS.UploadFiles.FileName.Replace(" ", ""));
                        LMS.UploadFiles.SaveAs(Server.MapPath(LMS.FilePath));
                    }
                    catch (Exception)
                    {
                        ModelState.AddModelError(string.Empty, "-Please attach the document.");
                        count++;
                        ErrorMessage += count + "-Please attach the document.<br />";
                    }

                    // LMS.CreatedBy = Convert.ToInt32(Session["emp_id"]);
                    LMS.CreatedBy = Convert.ToInt32(Session["CurrentUserID"]);
                    LMS.CreatedOn = DateTime.Now;
                    db.LMSSubjectLearnings.Add(LMS);
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
                    ModelState.AddModelError(string.Empty, "Name already exists.");
                    ViewBag.MessageType = "error";
                    ViewBag.Message = "Name already exists.";
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
            model.LMSSubjectLearnings = db.LMSSubjectLearnings.Where(x => x.ProgramCourseID == LMS.ProgramCourseID).OrderByDescending(a => a.LMSSubjectLearningID).ToList();
            model.SelectedLMSSubjectLearning = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc", LMS.IsActive);

            return View("Index", model);
        }


        [HttpPost]
        public JsonResult StudentAttemptDone(string LMSWeekDetailID, string LMSWeekID, string WeekListTypeID, string LMSWeekDetailAttemptID, Boolean IsChecked)
        {
            
            //int EmpID = Convert.ToInt32(Session["emp_id"]);
            int EmpID = Convert.ToInt32(Session["CurrentUserID"]);
            int StudentID = Convert.ToInt32(Session["CurrentUserID"]); ;
            int? ID = db.InsertLMSStudentQuizAttempt(Convert.ToInt32(LMSWeekDetailAttemptID), Convert.ToInt32(LMSWeekDetailID), Convert.ToInt32(LMSWeekID), Convert.ToInt32(WeekListTypeID), StudentID, IsChecked, EmpID, EmpID,"Yes").FirstOrDefault();
            return Json(new { success = true, responseText = ID }, JsonRequestBehavior.AllowGet);
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