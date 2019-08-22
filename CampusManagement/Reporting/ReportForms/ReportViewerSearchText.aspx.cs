using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using CampusManagement.Models;
using Microsoft.Reporting.WebForms;

namespace CampusManagement.Reporting.ReportForms
{
    public partial class ReportViewerSearchText : System.Web.UI.Page
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
            cmbSubDepartment.DataSource = db.GetSubDepartments_by_HospitalID(1);
            cmbSubDepartment.DataTextField = "SubDept_Name";
            cmbSubDepartment.DataValueField = "SubDept_Id";
            cmbSubDepartment.DataBind();
            cmbSubDepartment.Items.Insert(0, new ListItem("---All---", "0"));

      
        }
        protected void LoadBatchesReport(string searchOrPrint)
        {
            try
            {
                List<rpt_Batches_Result> lstForms = db.rpt_Batches(this.txtSearch.Value).ToList();
                ReportViewer1.ProcessingMode = ProcessingMode.Local;
                ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Reporting/Reports/rpt_Batches.rdlc");
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
                    OpenReportInPDF("rpt_Batches.pdf");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        protected void LoadProgramsReport(string searchOrPrint)
        {
            try
            {
                List<rpt_Programs_Result> lstForms = db.rpt_Programs(this.txtSearch.Value, Convert.ToInt32(cmbSubDepartment.Value)).ToList();
                ReportViewer1.ProcessingMode = ProcessingMode.Local;
                ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Reporting/Reports/rpt_Programs.rdlc");
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
                    OpenReportInPDF("rpt_Programs.pdf");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        protected void LoadCoursesReport(string searchOrPrint)
        {
            try
            {
                List<rpt_Courses_Result> lstForms = db.rpt_Courses(this.txtSearch.Value).ToList();
                ReportViewer1.ProcessingMode = ProcessingMode.Local;
                ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Reporting/Reports/rpt_Courses.rdlc");
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
                    OpenReportInPDF("rpt_Courses.pdf");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        protected void LoadBatchProgramStudentSummaryReport(string searchOrPrint)
        {
            try
            {
                List<rpt_GetBatchProgramStudentSummary_Result> lstForms = db.rpt_GetBatchProgramStudentSummary(this.txtSearch.Value, Convert.ToInt32(cmbSubDepartment.Value)).ToList();
                ReportViewer1.ProcessingMode = ProcessingMode.Local;
                ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Reporting/Reports/rpt_GetBatchProgramStudentSummary.rdlc");
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
                    OpenReportInPDF("rpt_GetBatchProgramStudentSummary.pdf");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        protected void Loadrpt_GetStudentSummary(string searchOrPrint)
        {
            try
            {
                List<rpt_GetStudentSummary_Result> lstForms = db.rpt_GetStudentSummary(this.txtSearch.Value).ToList();
                ReportViewer1.ProcessingMode = ProcessingMode.Local;
                ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Reporting/Reports/rpt_GetStudentSummary.rdlc");
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
                    OpenReportInPDF("rpt_GetStudentSummary.pdf");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        protected void LoadReport(string searchOrPrint)
        {
            hdnReportName.Value = Request.QueryString["ReportName"];

            switch (hdnReportName.Value)
            {
                case "rpt_Batches":
                    lblReportTitle.InnerHtml = "Sessions";
                    LoadBatchesReport(searchOrPrint);
                    break;
                case "rpt_Programs":
                    lblReportTitle.InnerHtml = "Programs";
                    LoadProgramsReport(searchOrPrint);
                    break;
                case "rpt_Courses":
                    lblReportTitle.InnerHtml = "Subjects";
                    LoadCoursesReport(searchOrPrint);
                    break;
                case "rpt_GetBatchProgramStudentSummary":
                    lblReportTitle.InnerHtml = "Program Student Summary";
                    LoadBatchProgramStudentSummaryReport(searchOrPrint);
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