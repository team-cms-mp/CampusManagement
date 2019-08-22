using CampusManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Reporting.WebForms;


namespace CampusManagement.Reporting.ReportForms
{
    public partial class ReportViewerBatchProgramFormSearch : System.Web.UI.Page
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
            cmbSubDepartmentNew.DataSource = db.GetSubDepartments_by_HospitalID(1);
            cmbSubDepartmentNew.DataTextField = "SubDept_Name";
            cmbSubDepartmentNew.DataValueField = "SubDept_Id";
            cmbSubDepartmentNew.DataBind();
            cmbSubDepartmentNew.Items.Insert(0, new ListItem("---All---", "0"));

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

        protected void rpt_PaymentCollections(string searchOrPrint)
        {
            try
            {
                List<GetApplicantStudentChallans_Result> lstForms = db.GetApplicantStudentChallans(this.FormNoNew.Value, "Student", "Yes", Convert.ToInt32(cmbSessions.Value), Convert.ToInt32(cmbBatchPrograms.Value)).ToList();
                ReportViewer1.ProcessingMode = ProcessingMode.Local;
                ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Reporting/Reports/rpt_PaymentCollections.rdlc");
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
                    OpenReportInPDF("rpt_PaymentCollections.pdf");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        protected void rpt_PayablesDetail(string searchOrPrint)
        {
            try
            {
                List<GetApplicantStudentChallans_Result> lstForms = db.GetApplicantStudentChallans(this.FormNoNew.Value, "Student", "No", Convert.ToInt32(cmbSessions.Value), Convert.ToInt32(cmbBatchPrograms.Value)).ToList();
                ReportViewer1.ProcessingMode = ProcessingMode.Local;
                ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Reporting/Reports/rpt_PayablesDetail.rdlc");
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
                    OpenReportInPDF("rpt_PayablesDetail.pdf");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        protected void SF_GetApplicantStudentChallansFeeDefaulterSummary(string searchOrPrint)
        {
            try
            {
                List<SF_GetApplicantStudentChallansFeeDefaulterSummary_Result> lstForms = db.SF_GetApplicantStudentChallansFeeDefaulterSummary(this.FormNoNew.Value, "Student", Convert.ToInt32(cmbSessions.Value), Convert.ToInt32(cmbBatchPrograms.Value)).ToList();
                ReportViewer1.ProcessingMode = ProcessingMode.Local;
                ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Reporting/Reports/SF_GetApplicantStudentChallansFeeDefaulterSummary.rdlc");
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
                    OpenReportInPDF("SF_GetApplicantStudentChallansFeeDefaulterSummary.pdf");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        protected void rpt_PaymentSummary(string searchOrPrint)
        {
            try
            {
                List<GetApplicantStudentChallansSummary_Result> lstForms = db.GetApplicantStudentChallansSummary(this.FormNoNew.Value, "Student", Convert.ToInt32(cmbSessions.Value), Convert.ToInt32(cmbBatchPrograms.Value)).ToList();
                ReportViewer1.ProcessingMode = ProcessingMode.Local;
                ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Reporting/Reports/rpt_PaymentSummary.rdlc");
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
                    OpenReportInPDF("rpt_PaymentSummary.pdf");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        protected void rpt_PayableAmount(string searchOrPrint)
        {
            try
            {
                List<SF_GetCollegeServiceWiseAmounts_Result> lstForms = db.SF_GetCollegeServiceWiseAmounts("No", this.FormNoNew.Value, "Student", Convert.ToInt32(cmbSessions.Value), Convert.ToInt32(cmbBatchPrograms.Value)).ToList();
                ReportViewer1.ProcessingMode = ProcessingMode.Local;
                ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Reporting/Reports/rpt_PayableAmount.rdlc");
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
                    OpenReportInPDF("rpt_PayableAmount.pdf");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        protected void rpt_PCollectedAmount(string searchOrPrint)
        {
            try
            {
                List<SF_GetCollegeServiceWiseAmounts_Result> lstForms = db.SF_GetCollegeServiceWiseAmounts("Yes", this.FormNoNew.Value, "Student", Convert.ToInt32(cmbSessions.Value), Convert.ToInt32(cmbBatchPrograms.Value)).ToList();
                ReportViewer1.ProcessingMode = ProcessingMode.Local;
                ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Reporting/Reports/rpt_PCollectedAmount.rdlc");
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
                    OpenReportInPDF("rpt_PCollectedAmount.pdf");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }


        protected void rpt_GetAdmissionApplicationSubmittedDateWiseSummary(string searchOrPrint)
        {
            try
            {
                List<rpt_GetAdmissionApplicationSubmittedDateWiseSummary_Result> lstForms = db.rpt_GetAdmissionApplicationSubmittedDateWiseSummary(Convert.ToInt32(cmbSessions.Value), Convert.ToInt32(cmbBatchPrograms.Value)).ToList();
                ReportViewer1.ProcessingMode = ProcessingMode.Local;
                ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Reporting/Reports/rpt_GetAdmissionApplicationSubmittedDateWiseSummary.rdlc");
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
                    OpenReportInPDF("rpt_GetAdmissionApplicationSubmittedDateWiseSummary.pdf");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        protected void rpt_GetAdmissionApplicationsMissingEntries(string searchOrPrint)
        {
            try
            {
                List<rpt_GetAdmissionApplicationsMissingEntries_Result> lstForms = db.rpt_GetAdmissionApplicationsMissingEntries(Convert.ToInt32(cmbSessions.Value), Convert.ToInt32(cmbBatchPrograms.Value)).ToList();
                ReportViewer1.ProcessingMode = ProcessingMode.Local;
                ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Reporting/Reports/rpt_GetAdmissionApplicationsMissingEntries.rdlc");
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
                    OpenReportInPDF("rpt_GetAdmissionApplicationsMissingEntries.pdf");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        protected void rpt_GetAdmissionApplicantAcademicRecords(string searchOrPrint)
        {
            try
            {
                List<rpt_GetAdmissionApplicantAcademicRecords_Result> lstForms = db.rpt_GetAdmissionApplicantAcademicRecords(FormNoNew.Value, Convert.ToInt32(cmbSessions.Value), Convert.ToInt32(cmbBatchPrograms.Value), Convert.ToInt32(cmbSubDepartmentNew.Value)).ToList();
                ReportViewer1.ProcessingMode = ProcessingMode.Local;
                ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Reporting/Reports/rpt_GetAdmissionApplicantAcademicRecords.rdlc");
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
                    OpenReportInPDF("rpt_GetAdmissionApplicantAcademicRecords.pdf");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }


        protected void rpt_GetAdmissionFormSaleDetail(string searchOrPrint)
        {
            try
            {
                List<rpt_GetAdmissionFormSaleDetail_Result> lstForms = db.rpt_GetAdmissionFormSaleDetail(FormNoNew.Value, Convert.ToInt32(cmbSessions.Value), Convert.ToInt32(cmbBatchPrograms.Value)).ToList();
                ReportViewer1.ProcessingMode = ProcessingMode.Local;
                ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Reporting/Reports/rpt_GetAdmissionFormSaleDetail.rdlc");
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
                    OpenReportInPDF("rpt_GetAdmissionFormSaleDetail.pdf");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        protected void rpt_GetApplicantStudentChallans(string searchOrPrint)
        {
            try
            {
                List<rpt_GetApplicantStudentChallans_Result> lstForms = db.rpt_GetApplicantStudentChallans(FormNoNew.Value, Convert.ToInt32(cmbSessions.Value), Convert.ToInt32(cmbBatchPrograms.Value), 1, "ApplicantStudent", "Yes").ToList();
                ReportViewer1.ProcessingMode = ProcessingMode.Local;
                ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Reporting/Reports/rpt_GetApplicantStudentChallans.rdlc");
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
                    OpenReportInPDF("rpt_GetApplicantStudentChallans.pdf");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        
        protected void sf_GetFeeReceiptSummaryBatchWise(string searchOrPrint)
        {
            try
            {
                string BatchID = (this.cmbSessions.Value == "") ? "0" : this.cmbSessions.Value;
                string BatchProgramID = (this.cmbBatchPrograms.Value == "") ? "0" : this.cmbBatchPrograms.Value;
                List<sf_GetFeeReceiptSummaryBatchWise_Result> lstForms = db.sf_GetFeeReceiptSummaryBatchWise(Convert.ToInt32(BatchID), Convert.ToInt32(BatchProgramID), "Yes").ToList();
                ReportViewer1.ProcessingMode = ProcessingMode.Local;
                ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Reporting/Reports/rpt_sf_GetFeeReceiptSummaryBatchWise.rdlc");
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
                    OpenReportInPDF("rpt_sf_GetFeeReceiptSummaryBatchWise.pdf");
                }

            }
            catch (Exception)
            {
                throw;
            }
        }

        protected void rpt_GetAdmissionApplicantAcademicRecordsPHD(string searchOrPrint)
        {
            try
            {
                List<rpt_GetAdmissionApplicantAcademicRecordsPHD_Result> lstForms = db.rpt_GetAdmissionApplicantAcademicRecordsPHD(FormNoNew.Value, Convert.ToInt32(cmbSessions.Value), Convert.ToInt32(cmbBatchPrograms.Value)).ToList();
                ReportViewer1.ProcessingMode = ProcessingMode.Local;
                ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Reporting/Reports/rpt_GetAdmissionApplicantAcademicRecordsPHD.rdlc");
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
                    OpenReportInPDF("rpt_GetAdmissionApplicantAcademicRecordsPHD.pdf");
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
                case "rpt_PaymentCollections":
                    lblReportTitle.InnerHtml = "Fee Payment History Detail";
                    rpt_PaymentCollections(searchOrPrint);
                    break;
                case "rpt_PayablesDetail":
                    lblReportTitle.InnerHtml = "Balance Due Report";
                    rpt_PayablesDetail(searchOrPrint);
                    break;
                case "SF_GetApplicantStudentChallansFeeDefaulterSummary":
                    lblReportTitle.InnerHtml = "Fee Defaulters History";
                    SF_GetApplicantStudentChallansFeeDefaulterSummary(searchOrPrint);
                    break;
                case "rpt_PaymentSummary":
                    lblReportTitle.InnerHtml = "Payment Summary";
                    rpt_PaymentSummary(searchOrPrint);
                    break;
                case "rpt_PayableAmount":
                    lblReportTitle.InnerHtml = "Service Wise Balance Due Report";
                    rpt_PayableAmount(searchOrPrint);
                    break;
                case "rpt_PCollectedAmount":
                    lblReportTitle.InnerHtml = "Service Wise Fee Payment History";
                    rpt_PCollectedAmount(searchOrPrint);
                    break;
                case "rpt_GetAdmissionApplicationsMissingEntries":
                    lblReportTitle.InnerHtml = "List of Missing Entries";
                    rpt_GetAdmissionApplicationsMissingEntries(searchOrPrint);
                    break;
                case "rpt_GetAdmissionApplicationSubmittedDateWiseSummary":
                    lblReportTitle.InnerHtml = "Date Wise Form Submitted Report";
                    rpt_GetAdmissionApplicationSubmittedDateWiseSummary(searchOrPrint);
                    break;
                case "rpt_GetAdmissionFormSaleDetail":
                    lblReportTitle.InnerHtml = "Cash Collection Report: Prospectus Sale";
                    rpt_GetAdmissionFormSaleDetail(searchOrPrint);
                    break;
                case "rpt_GetApplicantStudentChallans":
                    lblReportTitle.InnerHtml = "Applicant Student Challans";
                    rpt_GetApplicantStudentChallans(searchOrPrint);
                    break;
                case "rpt_GetAdmissionApplicantAcademicRecords":
                    lblReportTitle.InnerHtml = "Applicant’s Academic Details";
                    rpt_GetAdmissionApplicantAcademicRecords(searchOrPrint);
                    break;
                case "rpt_sf_GetFeeReceiptSummaryBatchWise":
                    lblReportTitle.InnerHtml = "Fee Receipt Summary";
                    sf_GetFeeReceiptSummaryBatchWise(searchOrPrint);
                    break;
                case "rpt_GetAdmissionApplicantAcademicRecordsPHD":
                    lblReportTitle.InnerHtml = "Applicant’s Academic Details";
                    rpt_GetAdmissionApplicantAcademicRecordsPHD(searchOrPrint);
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

            Byte[] bytes = ReportViewer1.LocalReport.Render("PDF");
            Response.AddHeader("Content-Disposition", "inline; filename=" + ReportNamePDF);
            Response.ContentType = "application/pdf";
            Response.BinaryWrite(bytes);
            Response.End();
            //}
        }
    }
}