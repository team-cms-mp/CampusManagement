using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using CampusManagement.Models;
using Microsoft.Reporting.WebForms;

namespace CampusManagement.Reporting.ReportForms
{
    public partial class ReportViewerExamEligibleStudents : System.Web.UI.Page
    {
        ModelCMSNewContainer db = new ModelCMSNewContainer();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                FillComboBox();
                LoadReport("");
            }

        }


        protected void FillComboBox()
        {


            ddlExam.DataSource = db.GetExam().ToList();
            ddlExam.DataTextField = "ExamDetail";
            ddlExam.DataValueField = "ExamID";
            ddlExam.DataBind();
            ddlExam.Items.Insert(0, new ListItem("---All---", "0"));

            List<EligibleModel> EligibleModelList = new List<EligibleModel>();
            EligibleModelList = GenrateEligibleModel();
            ddlEligible.DataSource = EligibleModelList;
            ddlEligible.DataTextField = "EligibleText";
            ddlEligible.DataValueField = "IsEligible";
            ddlEligible.DataBind();
            ddlEligible.Items.Insert(0, new ListItem("---All---", "0"));

            List<GetBatchProgramNameConcat_Result> BatchList = new List<GetBatchProgramNameConcat_Result>();
            BatchList = db.GetBatchProgramNameConcat("", 10).ToList();
            ddlBatch.DataSource = BatchList;
            ddlBatch.DataTextField = "Name";
            ddlBatch.DataValueField = "ID";
            ddlBatch.DataBind();

            List<GetPrograms_by_FacultyLevelBatch_Result> lstPrograms = new List<GetPrograms_by_FacultyLevelBatch_Result>();
            lstPrograms = GetPrograms_by_FacultyLevelBatch(null, null);
            ddlProgram.DataSource = lstPrograms;
            ddlProgram.DataTextField = "ProgramName";
            ddlProgram.DataValueField = "BatchProgramID";
            ddlProgram.DataBind();
            ddlProgram.Items.Insert(0, new ListItem("--All--", "0"));

            List<Semester> SemesterList = new List<Semester>();
            SemesterList = db.Semesters.ToList();

            List<BatchProgramSemester> lstSemester = new List<BatchProgramSemester>();
            lstSemester = GetBatchProgramSemesterList(null);
            ddlSemester.DataSource = lstSemester;
            ddlSemester.DataTextField = "YearSemesterNo";
            ddlSemester.DataValueField = "YearSemesterNo";
            ddlSemester.DataBind();
            ddlSemester.Items.Insert(0, new ListItem("--All--", "0"));
        }



        protected void LoadGetEligibleStudentsForExam(string searchOrPrint)
        {
            try
            {
                // rpt_GetEligibleStudentsForExam
                List<rpt_GetEligibleStudentsForExam_Result> StudentList = new List<rpt_GetEligibleStudentsForExam_Result>();
                StudentList = db.rpt_GetEligibleStudentsForExam(Convert.ToInt32(this.ddlExam.SelectedValue), 0, 0, Convert.ToInt32(this.ddlEligible.SelectedValue), Convert.ToInt32(this.ddlBatch.SelectedValue), Convert.ToInt32(this.ddlProgram.SelectedValue), Convert.ToInt32(this.ddlSemester.SelectedValue)).ToList();
                List<GetApplicantSummary_Result> lstForms = db.GetApplicantSummary("", Convert.ToInt32(this.ddlProgram.SelectedItem), DateTime.Now, DateTime.Now, Convert.ToInt32(this.ddlEligible.SelectedItem), Convert.ToInt32(this.ddlExam.SelectedItem)).ToList();

                ReportViewer1.ProcessingMode = ProcessingMode.Local;
                ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Reporting/Reports/rptApplicantSummary.rdlc");
                ReportDataSource datasource = new ReportDataSource("DataSet1", lstForms);
                ReportViewer1.LocalReport.DataSources.Clear();
                ReportViewer1.LocalReport.DataSources.Add(datasource);

                ReportViewer1.LocalReport.EnableExternalImages = true;
                string imagePath = new Uri(Server.MapPath(MvcApplication.CampusLogo)).AbsoluteUri;
                ReportParameter parameter = new ReportParameter("ImagePath", imagePath);
                ReportViewer1.LocalReport.SetParameters(parameter);
                ReportViewer1.LocalReport.Refresh();
                ReportViewer1.Visible = true;

                if (searchOrPrint == "print")
                {
                    OpenReportInPDF("rptApplicantSummary.pdf");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }




        protected void LoadReport(string searchOrPrint)
        {
            string reportName = Request.QueryString["ReportName"];
            switch (reportName)
            {
                case "rpt_GetEligibleStudentsForExam":
                    lblReportTitle.InnerHtml = "Program Wise Applicants Summary";
                    LoadGetEligibleStudentsForExam(searchOrPrint);
                    break;

                default:
                    break;
            }
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {

            LoadReport("");
        }
        protected void btnPrint_Click(object sender, EventArgs e)
        {
            LoadReport("print");

        }

        protected void OpenReportInPDF(string ReportNamePDF)
        {
            //if (Request.Browser.Browser == "Chrome")
            //{
            Byte[] bytes = ReportViewer1.LocalReport.Render("PDF");
            Response.AddHeader("Content-Disposition", "inline; filename=" + ReportNamePDF);
            Response.ContentType = "application/pdf";
            Response.BinaryWrite(bytes);
            Response.End();
            //}
        }

        private class EligibleModel
        {
            public int IsEligible { get; set; }
            public string EligibleText { get; set; }

        }



        private List<EligibleModel> GenrateEligibleModel()
        {
            List<EligibleModel> List = new List<EligibleModel>();
            EligibleModel Obj1 = new EligibleModel();
            Obj1.IsEligible = -1;
            Obj1.EligibleText = "--Please Select--";
            EligibleModel Obj2 = new EligibleModel();
            Obj2.IsEligible = 1;
            Obj2.EligibleText = "Eligible";
            EligibleModel Obj3 = new EligibleModel();
            Obj3.IsEligible = 0;
            Obj3.EligibleText = "Not Eligible";
            List.Add(Obj1);
            List.Add(Obj2);
            List.Add(Obj3);
            return List;
        }

        protected void ddlBatch_SelectedIndexChanged(object sender, EventArgs e)
        {
            List<GetPrograms_by_FacultyLevelBatch_Result> lstPrograms = new List<GetPrograms_by_FacultyLevelBatch_Result>();
            lstPrograms = GetPrograms_by_FacultyLevelBatch(null, ddlBatch.SelectedValue);
            ddlProgram.DataSource = lstPrograms;
            ddlProgram.DataTextField = "ProgramName";
            ddlProgram.DataValueField = "BatchProgramID";
            ddlProgram.DataBind();
            ddlProgram.Items.Insert(0, new ListItem("--Please Select--", "0"));

            List<BatchProgramSemester> lstSemester = new List<BatchProgramSemester>();
            lstSemester = GetBatchProgramSemesterList(ddlProgram.SelectedValue);
            ddlSemester.DataSource = lstSemester;
            ddlSemester.DataTextField = "YearSemesterNo";
            ddlSemester.DataValueField = "YearSemesterNo";
            ddlSemester.DataBind();
            ddlSemester.Items.Insert(0, new ListItem("--Please Select--", "0"));
        }

        protected void ddlProgram_SelectedIndexChanged(object sender, EventArgs e)
        {
            List<BatchProgramSemester> lstSemester = new List<BatchProgramSemester>();
            lstSemester = GetBatchProgramSemesterList(ddlProgram.SelectedValue);
            ddlSemester.DataSource = lstSemester;
            ddlSemester.DataTextField = "YearSemesterNo";
            ddlSemester.DataValueField = "YearSemesterNo";
            ddlSemester.DataBind();
            ddlSemester.Items.Insert(0, new ListItem("--Please Select--", "0"));
        }


        public List<GetPrograms_by_FacultyLevelBatch_Result> GetPrograms_by_FacultyLevelBatch(string FacultyID, string BatchID)
        {
            List<GetPrograms_by_FacultyLevelBatch_Result> lstPrograms = new List<GetPrograms_by_FacultyLevelBatch_Result>();
            lstPrograms = db.GetPrograms_by_FacultyLevelBatch(Convert.ToInt32(FacultyID), 0, Convert.ToInt32(BatchID), 0).ToList();
            return lstPrograms;
        }

        public List<BatchProgramSemester> GetBatchProgramSemesterList(string BatchProgramID)
        {
            List<BatchProgramSemester> lstSemester = new List<BatchProgramSemester>();
            int bpId = Convert.ToInt32(BatchProgramID);
            lstSemester = db.BatchProgramSemesters.Where(s => s.BatchProgramID == bpId).ToList();
            return lstSemester;
        }
    }
}