using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using CampusManagement.Models;
using Microsoft.Reporting.WebForms;


namespace CampusManagement.Reporting.ReportForms
{
    public partial class ReportViewerExamStudentSubjectReport : System.Web.UI.Page
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
        }



        protected void LoadGetEligibleStudentsForExam(string searchOrPrint)
        {
            try
            {
                // rpt_GetEligibleStudentsForExam
                List<rpt_GetExamStudentSubjectReport_Result> StudentList = new List<rpt_GetExamStudentSubjectReport_Result>() ;
                StudentList =  db.rpt_GetExamStudentSubjectReport(Convert.ToInt32(ddlExam.SelectedValue)).ToList();
                ReportViewer1.ProcessingMode = ProcessingMode.Local;
                ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Reporting/Reports/rpt_GetExamStudentSubjectReport.rdlc");
                ReportDataSource datasource = new ReportDataSource("DataSet1", StudentList);
                ReportViewer1.LocalReport.DataSources.Clear();
                ReportViewer1.LocalReport.DataSources.Add(datasource);

                //ReportViewer1.LocalReport.EnableExternalImages = true;
                //string imagePath = new Uri(Server.MapPath(MvcApplication.CampusLogo)).AbsoluteUri;
                //ReportParameter parameter = new ReportParameter("ImagePath", imagePath);
                //ReportViewer1.LocalReport.SetParameters(parameter);
                ReportViewer1.LocalReport.Refresh();
                ReportViewer1.Visible = true;

                if (searchOrPrint == "print")
                {
                    OpenReportInPDF("rpt_GetExamStudentSubjectReport.pdf");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }




        protected void LoadReport(string searchOrPrint)
        {
            string reportName = Request.QueryString["ReportName"];
            switch (reportName)
            {
                case "rpt_GetEligibleStudentsForExam":
                    lblReportTitle.InnerHtml = "Student Subjects Report";
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
    }
}