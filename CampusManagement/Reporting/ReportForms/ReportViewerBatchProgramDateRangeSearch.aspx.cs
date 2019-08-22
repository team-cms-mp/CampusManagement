using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using CampusManagement.Models;
using Microsoft.Reporting.WebForms;

namespace CampusManagement.Reporting.ReportForms
{
    public partial class ReportViewerBatchProgramDateRangeSearch : System.Web.UI.Page
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

            List<GetBatchProgramNameConcat_Result> BatchList = new List<GetBatchProgramNameConcat_Result>();
            BatchList = db.GetBatchProgramNameConcat("", 10).ToList();
            cmbSessions.DataSource = BatchList;
            cmbSessions.DataTextField = "Name";
            cmbSessions.DataValueField = "ID";
            cmbSessions.DataBind();

            cmbBatchPrograms.DataSource = db.GetBatchProgramNameConcat("", 0);
            cmbBatchPrograms.DataTextField = "Name";
            cmbBatchPrograms.DataValueField = "ID";
            cmbBatchPrograms.DataBind();
            cmbBatchPrograms.Items.Insert(0, new ListItem("---All---", "0"));
        }

        protected void LoadGetFormSaleDetail(string searchOrPrint)
        {
            try
            {
                List<GetFormSaleDetail_Result> lstForms = new List<GetFormSaleDetail_Result>();
                string BatchID = (this.cmbSessions.Value == "") ? "0" : this.cmbSessions.Value;
                string BatchProgramID = (this.cmbBatchPrograms.Value == "") ? "0" : this.cmbBatchPrograms.Value;
                string StartDate = (this.txtStartDate.Value == "") ? null : this.txtStartDate.Value;
                string EndDate = (this.txtEndDate.Value == "") ? null : this.txtEndDate.Value;
                if (StartDate == null || EndDate == null)
                {
                    lstForms = db.GetFormSaleDetail(this.txtSearch.Value, "Yes", Convert.ToInt32(BatchID), Convert.ToInt32(BatchProgramID), DateTime.Now, DateTime.Now, Convert.ToInt32(cmbSubDepartment.Value)).ToList();
                }
                else
                {
                    DateTime dtS = Convert.ToDateTime(StartDate);
                    DateTime dtE = Convert.ToDateTime(EndDate);
                    lstForms = db.GetFormSaleDetail(this.txtSearch.Value, "Yes", Convert.ToInt32(BatchID), Convert.ToInt32(BatchProgramID), dtS, dtE, Convert.ToInt32(cmbSubDepartment.Value)).ToList();
                    this.hdnSDate.Value = dtS.Date.ToString("yyyy/MM/dd");
                    this.hdnEDate.Value = dtE.Date.ToString("yyyy/MM/dd");
                }
                ReportViewer1.ProcessingMode = ProcessingMode.Local;
                ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Reporting/Reports/rptReceivedFormsDetail.rdlc");
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
                    OpenReportInPDF("rptReceivedFormsDetail.pdf");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        protected void GetFormSaleDetail(string searchOrPrint)
        {
            try
            {
                List<GetFormSaleDetail_Result> lstForms = new List<GetFormSaleDetail_Result>();
                string BatchID = (this.cmbSessions.Value == "") ? "0" : this.cmbSessions.Value;
                string BatchProgramID = (this.cmbBatchPrograms.Value == "") ? "0" : this.cmbBatchPrograms.Value;
                string StartDate = (this.txtStartDate.Value == "") ? null : this.txtStartDate.Value;
                string EndDate = (this.txtEndDate.Value == "") ? null : this.txtEndDate.Value;
                if (StartDate == null || EndDate == null)
                {
                    lstForms = db.GetFormSaleDetail(this.txtSearch.Value, "Yes", Convert.ToInt32(BatchID), Convert.ToInt32(BatchProgramID), DateTime.Now, DateTime.Now, Convert.ToInt32(cmbSubDepartment.Value)).ToList();
                }
                else
                {
                    DateTime dtS = Convert.ToDateTime(StartDate);
                    DateTime dtE = Convert.ToDateTime(EndDate);
                    lstForms = db.GetFormSaleDetail(this.txtSearch.Value, "Yes", Convert.ToInt32(BatchID), Convert.ToInt32(BatchProgramID), dtS, dtE, Convert.ToInt32(cmbSubDepartment.Value)).ToList();
                    this.hdnSDate.Value = dtS.Date.ToString("yyyy/MM/dd");
                    this.hdnEDate.Value = dtE.Date.ToString("yyyy/MM/dd");
                }
                ReportViewer1.ProcessingMode = ProcessingMode.Local;
                ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Reporting/Reports/rpt_GetFormSaleDetail.rdlc");
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
                    OpenReportInPDF("rpt_GetFormSaleDetail.pdf");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        protected void sf_GetFeeReceiptDetailBatchWise(string searchOrPrint)
        {
            try
            {
                List<sf_GetFeeReceiptDetailBatchWise_Result> lstForms = new List<sf_GetFeeReceiptDetailBatchWise_Result>();
                string BatchID = (this.cmbSessions.Value == "") ? "0" : this.cmbSessions.Value;
                string BatchProgramID = (this.cmbBatchPrograms.Value == "") ? "0" : this.cmbBatchPrograms.Value;
                string StartDate = (this.txtStartDate.Value == "") ? null : this.txtStartDate.Value;
                string EndDate = (this.txtEndDate.Value == "") ? null : this.txtEndDate.Value;
                if (StartDate == null || EndDate == null)
                {
                    lstForms = db.sf_GetFeeReceiptDetailBatchWise(Convert.ToInt32(BatchID), Convert.ToInt32(BatchProgramID), "Yes", DateTime.Now, DateTime.Now).ToList();
                }
                else
                {
                    DateTime dtS = Convert.ToDateTime(StartDate);
                    DateTime dtE = Convert.ToDateTime(EndDate);
                    lstForms = db.sf_GetFeeReceiptDetailBatchWise(Convert.ToInt32(BatchID), Convert.ToInt32(BatchProgramID), "Yes", dtS, dtE).ToList();
                    this.hdnSDate.Value = dtS.Date.ToString("yyyy/MM/dd");
                    this.hdnEDate.Value = dtE.Date.ToString("yyyy/MM/dd");
                }
                ReportViewer1.ProcessingMode = ProcessingMode.Local;
                ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Reporting/Reports/rpt_sf_GetFeeReceiptDetailBatchWise.rdlc");
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
                    OpenReportInPDF("rpt_sf_GetFeeReceiptDetailBatchWise.pdf");
                }

            }
            catch (Exception)
            {
                throw;
            }
        }

        protected void sf_GetFeeReceiptStudentDetailBatchWise(string searchOrPrint)
        {
            try
            {
                List<sf_GetFeeReceiptDetailBatchWise_Result> lstForms = new List<sf_GetFeeReceiptDetailBatchWise_Result>();
                string BatchID = (this.cmbSessions.Value == "") ? "0" : this.cmbSessions.Value;
                string BatchProgramID = (this.cmbBatchPrograms.Value == "") ? "0" : this.cmbBatchPrograms.Value;
                string StartDate = (this.txtStartDate.Value == "") ? null : this.txtStartDate.Value;
                string EndDate = (this.txtEndDate.Value == "") ? null : this.txtEndDate.Value;
                if (StartDate == null || EndDate == null)
                {
                    lstForms = db.sf_GetFeeReceiptDetailBatchWise(Convert.ToInt32(BatchID), Convert.ToInt32(BatchProgramID), "Yes", DateTime.Now, DateTime.Now).ToList();
                }
                else
                {
                    DateTime dtS = Convert.ToDateTime(StartDate);
                    DateTime dtE = Convert.ToDateTime(EndDate);
                    lstForms = db.sf_GetFeeReceiptDetailBatchWise(Convert.ToInt32(BatchID), Convert.ToInt32(BatchProgramID), "Yes", dtS, dtE).ToList();
                    this.hdnSDate.Value = dtS.Date.ToString("yyyy/MM/dd");
                    this.hdnEDate.Value = dtE.Date.ToString("yyyy/MM/dd");
                }
                ReportViewer1.ProcessingMode = ProcessingMode.Local;
                ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Reporting/Reports/rpt_sf_GetFeeReceiptStudentDetailBatchWise.rdlc");
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
                    OpenReportInPDF("rpt_sf_GetFeeReceiptStudentDetailBatchWise.pdf");
                }

            }
            catch (Exception)
            {
                throw;
            }
        }

        protected void rpt_ApplicantMissingDetail(string searchOrPrint)
        {
            try
            {
                List<rpt_ApplicantMissingDetail_Result> lstForms = new List<rpt_ApplicantMissingDetail_Result>();
                string BatchID = (this.cmbSessions.Value == "") ? "0" : this.cmbSessions.Value;
                string BatchProgramID = (this.cmbBatchPrograms.Value == "") ? "0" : this.cmbBatchPrograms.Value;
                string Search = (this.txtSearch.Value == "") ? "" : this.txtSearch.Value;
                string Datefrom = (this.txtStartDate.Value == "") ? null : this.txtStartDate.Value;
                string DateTo = (this.txtEndDate.Value == "") ? null : this.txtEndDate.Value;
                if (Datefrom == null || DateTo == null)
                {
                    lstForms = db.rpt_ApplicantMissingDetail(Convert.ToInt32(BatchID), Convert.ToInt32(BatchProgramID), Search, DateTime.Now, DateTime.Now).ToList();
                }
                else
                {
                    DateTime dtS = Convert.ToDateTime(Datefrom);
                    DateTime dtE = Convert.ToDateTime(DateTo);
                    lstForms = db.rpt_ApplicantMissingDetail(Convert.ToInt32(BatchID), Convert.ToInt32(BatchProgramID), Search, dtS, dtE).ToList();
                    this.hdnSDate.Value = dtS.Date.ToString("yyyy/MM/dd");
                    this.hdnEDate.Value = dtE.Date.ToString("yyyy/MM/dd");
                }
                ReportViewer1.ProcessingMode = ProcessingMode.Local;
                ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Reporting/Reports/rpt_ApplicantMissingDetail.rdlc");
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
                    OpenReportInPDF("rpt_ApplicantMissingDetail.pdf");
                }

            }
            catch (Exception)
            {
                throw;
            }
        }

        protected void rpt_GetAdmissionApplicantsSummery(string searchOrPrint)
        {
            try
            {
                List<rpt_GetAdmissionApplicantsSummery_Result> lstForms = new List<rpt_GetAdmissionApplicantsSummery_Result>();
                string BatchID = (this.cmbSessions.Value == "") ? "0" : this.cmbSessions.Value;
                string BatchProgramID = (this.cmbBatchPrograms.Value == "") ? "0" : this.cmbBatchPrograms.Value;
                string Search = (this.cmbSessions.Value == "") ? "0" : this.cmbSessions.Value;
                string Datefrom = (this.txtStartDate.Value == "") ? null : this.txtStartDate.Value;
                string DateTo = (this.txtEndDate.Value == "") ? null : this.txtEndDate.Value;
                if (Datefrom == null || DateTo == null)
                {
                    lstForms = db.rpt_GetAdmissionApplicantsSummery(Convert.ToInt32(BatchID), Convert.ToInt32(BatchProgramID), Search, DateTime.Now, DateTime.Now).ToList();
                }
                else
                {
                    DateTime dtS = Convert.ToDateTime(Datefrom);
                    DateTime dtE = Convert.ToDateTime(DateTo);
                    lstForms = db.rpt_GetAdmissionApplicantsSummery(Convert.ToInt32(BatchID), Convert.ToInt32(BatchProgramID), Search, dtS, dtE).ToList();
                    this.hdnSDate.Value = dtS.Date.ToString("yyyy/MM/dd");
                    this.hdnEDate.Value = dtE.Date.ToString("yyyy/MM/dd");
                }
                ReportViewer1.ProcessingMode = ProcessingMode.Local;
                ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Reporting/Reports/rpt_GetAdmissionApplicantsSummery.rdlc");
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
                    OpenReportInPDF("rpt_GetAdmissionApplicantsSummery.pdf");
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
                case "rptReceivedFormsDetail":
                    lblReportTitle.InnerHtml = "Admission Form Receive Detail";
                    LoadGetFormSaleDetail(searchOrPrint);
                    break;
                case "rpt_GetFormSaleDetail":
                    lblReportTitle.InnerHtml = "Date Wise Fee Receipt Report";
                    GetFormSaleDetail(searchOrPrint);
                    break;
                case "rpt_sf_GetFeeReceiptDetailBatchWise":
                    lblReportTitle.InnerHtml = "All Students Fee Receipt Detail";
                    sf_GetFeeReceiptDetailBatchWise(searchOrPrint);
                    break;
                case "rpt_sf_GetFeeReceiptStudentDetailBatchWise":
                    lblReportTitle.InnerHtml = "All Students Fee Receipt Detail";
                    sf_GetFeeReceiptStudentDetailBatchWise(searchOrPrint);
                    break;
                case "rpt_ApplicantMissingDetail":
                    lblReportTitle.InnerHtml = "Applicant's Missing Detail";
                    rpt_ApplicantMissingDetail(searchOrPrint);
                    break;
                case "rpt_GetAdmissionApplicantsSummery":
                    lblReportTitle.InnerHtml = "Admission Applicant's Summary";
                    rpt_GetAdmissionApplicantsSummery(searchOrPrint);
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