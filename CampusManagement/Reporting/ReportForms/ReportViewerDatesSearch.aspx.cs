using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using CampusManagement.Models;
using Microsoft.Reporting.WebForms;

namespace CampusManagement.Reporting.ReportForms
{
    public partial class ReportViewerDatesSearch : System.Web.UI.Page
    {
        ModelCMSNewContainer db = new ModelCMSNewContainer();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                this.txtStartDate.Value = DateTime.Now.ToShortDateString();
                this.txtEndDate.Value = DateTime.Now.ToShortDateString();
                FillComboBox();
                LoadReport("");
            }
        }
        protected void FillComboBox()
        {
            cmbStatus.DataSource = db.Status.ToList();
            cmbStatus.DataTextField = "StatusName";
            cmbStatus.DataValueField = "StatusID";
            cmbStatus.DataBind();
            cmbStatus.Items.Insert(0, new ListItem("---All---", "0"));

            cmbBatchPrograms.DataSource = db.GetBatchProgramNameConcat("", 0);
            cmbBatchPrograms.DataTextField = "Name";
            cmbBatchPrograms.DataValueField = "ID";
            cmbBatchPrograms.DataBind();
            cmbBatchPrograms.Items.Insert(0, new ListItem("---All---", "0"));

            cmbFaculties.DataSource = db.GetSubDepartments_by_HospitalID(MvcApplication.Hospital_ID);
            cmbFaculties.DataTextField = "SubDept_Name";
            cmbFaculties.DataValueField = "SubDept_Id";
            cmbFaculties.DataBind();
            cmbFaculties.Items.Insert(0, new ListItem("---All---", "0"));
        }

        protected void LoadGetApplicantQualificationsReport(string searchOrPrint)
        {
            try
            {
                List<rptGetApplicantQualifications_Result> lstForms = db.rptGetApplicantQualifications(Convert.ToInt32(this.cmbStatus.Value), txtSearch.Value, Convert.ToDateTime(txtStartDate.Value), Convert.ToDateTime(txtEndDate.Value)).ToList();
                ReportViewer1.ProcessingMode = ProcessingMode.Local;
                ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Reporting/Reports/rptGetApplicantQualifications.rdlc");
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
                    OpenReportInPDF("rptGetApplicantQualifications.pdf");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        protected void LoadApplicantsReport(string searchOrPrint)
        {
            try
            {
                List<GetApplicant_Result> lstForms = db.GetApplicant(this.txtSearch.Value, Convert.ToInt32(this.cmbFaculties.Value), Convert.ToDateTime(this.txtStartDate.Value), Convert.ToDateTime(this.txtEndDate.Value), Convert.ToInt32(this.cmbBatchPrograms.Value), Convert.ToInt32(this.cmbStatus.Value)).ToList();
                ReportViewer1.ProcessingMode = ProcessingMode.Local;
                ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Reporting/Reports/rptApplicantDetail.rdlc");
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
                    OpenReportInPDF("rptApplicantDetail.pdf");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        protected void LoadApplicantSummaryReport(string searchOrPrint)
        {
            try
            {
                List<GetApplicantSummary_Result> lstForms = db.GetApplicantSummary(this.txtSearch.Value, Convert.ToInt32(this.cmbFaculties.Value), Convert.ToDateTime(this.txtStartDate.Value), Convert.ToDateTime(this.txtEndDate.Value), Convert.ToInt32(this.cmbBatchPrograms.Value), Convert.ToInt32(this.cmbStatus.Value)).ToList();
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
                case "rptGetApplicantQualifications":
                    lblReportTitle.InnerHtml = "Academic Record of Aplicants";
                    LoadGetApplicantQualificationsReport(searchOrPrint);
                    break;
                case "rptApplicantDetail":
                    lblReportTitle.InnerHtml = "Program Wise Applicants Detail";
                    LoadApplicantsReport(searchOrPrint);
                    break;
                case "rptApplicantSummary":
                    lblReportTitle.InnerHtml = "Program Wise Applicants Summary";
                    LoadApplicantSummaryReport(searchOrPrint);
                    break;
                default:
                    break;
            }
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            DateTime dt = new DateTime(1900, 01, 01);
            if (string.IsNullOrEmpty(this.txtStartDate.Value) || string.IsNullOrEmpty(this.txtEndDate.Value))
            {
                this.txtStartDate.Value = dt.ToShortDateString();
                this.txtEndDate.Value = dt.ToShortDateString();
            }
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