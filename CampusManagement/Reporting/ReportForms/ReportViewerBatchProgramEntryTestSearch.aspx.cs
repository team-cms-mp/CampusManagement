using CampusManagement.Models;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CampusManagement.Reporting.ReportForms
{
    public partial class ReportViewerBatchProgramEntryTestSearch : System.Web.UI.Page
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
            cmbDepartments.DataSource = db.GetSubDepartments_by_HospitalID(MvcApplication.CampusSettingID);
            cmbDepartments.DataTextField = "SubDept_Name";
            cmbDepartments.DataValueField = "SubDept_Id";
            cmbDepartments.DataBind();
            cmbDepartments.Items.Insert(0, new ListItem("---All---", "0"));

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

            cmbEntryTest.DataSource = db.EntryTests.ToList();
            cmbEntryTest.DataTextField = "EntryTestName";
            cmbEntryTest.DataValueField = "EntryTestID";
            cmbEntryTest.DataBind();
            cmbEntryTest.Items.Insert(0, new ListItem("---All---", "0"));
        }

        protected void rpt_GetAdmissionApplicationSummary(string searchOrPrint)
        {
            try
            {
                List<rpt_GetAdmissionApplicationSummary_Result> lstForms = db.rpt_GetAdmissionApplicationSummary(Convert.ToInt32(cmbDepartments.Value), Convert.ToInt32(cmbSessions.Value), Convert.ToInt32(cmbBatchPrograms.Value), Convert.ToInt32(cmbEntryTest.Value)).ToList();
                ReportViewer1.ProcessingMode = ProcessingMode.Local;
                ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Reporting/Reports/rpt_GetAdmissionApplicationSummary.rdlc");
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
                    OpenReportInPDF("rpt_GetAdmissionApplicationSummary.pdf");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        protected void rpt_GetAdmissionApplicationDetail(string searchOrPrint)
        {
            try
            {
                List<rpt_GetAdmissionApplicationDetail_Result> lstForms = db.rpt_GetAdmissionApplicationDetail(Convert.ToInt32(cmbDepartments.Value), Convert.ToInt32(cmbSessions.Value), Convert.ToInt32(cmbBatchPrograms.Value), Convert.ToInt32(cmbEntryTest.Value)).ToList();
                ReportViewer1.ProcessingMode = ProcessingMode.Local;
                ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Reporting/Reports/rpt_GetAdmissionApplicationDetail.rdlc");
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
                    OpenReportInPDF("rpt_GetAdmissionApplicationDetail.pdf");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        protected void rpt_GetAdmissionApplicationDetailEntryTest(string searchOrPrint)
        {
            try
            {
                List<rpt_GetAdmissionApplicationDetail_Result> lstForms = db.rpt_GetAdmissionApplicationDetail(Convert.ToInt32(cmbDepartments.Value), Convert.ToInt32(cmbSessions.Value), Convert.ToInt32(cmbBatchPrograms.Value), Convert.ToInt32(cmbEntryTest.Value)).ToList();
                ReportViewer1.ProcessingMode = ProcessingMode.Local;
                ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Reporting/Reports/rpt_GetAdmissionApplicationDetailEntryTest.rdlc");
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
                    OpenReportInPDF("rpt_GetAdmissionApplicationDetailEntryTest.pdf");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        protected void rpt_GetMeritListOnlyPercentage(string searchOrPrint)
        {
            try
            {
                string BatchProgramID = (this.cmbBatchPrograms.Value == "") ? "0" : this.cmbBatchPrograms.Value;
                string EntryTestID = (this.cmbEntryTest.Value == "") ? "0" : this.cmbEntryTest.Value;
                List<rpt_GetMeritListOnlyPercentage_Result> lstForms = db.rpt_GetMeritListOnlyPercentage(Convert.ToInt32(BatchProgramID), Convert.ToInt32(EntryTestID), 1).ToList();
                ReportViewer1.ProcessingMode = ProcessingMode.Local;
                ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Reporting/Reports/rpt_GetMeritListOnlyPercentage.rdlc");
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
                    OpenReportInPDF("rpt_GetMeritListOnlyPercentage.pdf");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        protected void rpt_GetMeritListOnlyPercentage_WeightageWise(string searchOrPrint)
        {
            try
            {
                string BatchProgramID = (this.cmbBatchPrograms.Value == "") ? "0" : this.cmbBatchPrograms.Value;
                string EntryTestID = (this.cmbEntryTest.Value == "") ? "0" : this.cmbEntryTest.Value;
                List<rpt_GetMeritListOnlyPercentage_FormWise_Result> lstForms = db.rpt_GetMeritListOnlyPercentage_FormWise(Convert.ToInt32(BatchProgramID), Convert.ToInt32(EntryTestID), 1).ToList();
                ReportViewer1.ProcessingMode = ProcessingMode.Local;
                ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Reporting/Reports/rpt_GetMeritListOnlyPercentage_WeightageWise.rdlc");
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
                    OpenReportInPDF("rpt_GetMeritListOnlyPercentage_WeightageWise.pdf");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        protected void rpt_GetMeritListOnlyPercentage_WeightageWiseUnselected(string searchOrPrint)
        {
            try
            {
                string BatchProgramID = (this.cmbBatchPrograms.Value == "") ? "0" : this.cmbBatchPrograms.Value;
                string EntryTestID = (this.cmbEntryTest.Value == "") ? "0" : this.cmbEntryTest.Value;
                List<rpt_GetMeritListOnlyPercentage_FormWise_Result> lstForms = db.rpt_GetMeritListOnlyPercentage_FormWise(Convert.ToInt32(BatchProgramID), Convert.ToInt32(EntryTestID), 1).ToList();
                ReportViewer1.ProcessingMode = ProcessingMode.Local;
                ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Reporting/Reports/rpt_GetMeritListOnlyPercentage_WeightageWiseUnselected.rdlc");
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
                    OpenReportInPDF("rpt_GetMeritListOnlyPercentage_WeightageWiseUnselected.pdf");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        protected void rpt_GetMeritListOnlyPercentage_FormWise(string searchOrPrint)
        {
            try
            {
                string BatchProgramID = (this.cmbBatchPrograms.Value == "") ? "0" : this.cmbBatchPrograms.Value;
                string EntryTestID = (this.cmbEntryTest.Value == "") ? "0" : this.cmbEntryTest.Value;
                List<rpt_GetMeritListOnlyPercentage_FormWise_Result> lstForms = db.rpt_GetMeritListOnlyPercentage_FormWise(Convert.ToInt32(BatchProgramID), Convert.ToInt32(EntryTestID), 2).ToList();
                ReportViewer1.ProcessingMode = ProcessingMode.Local;
                ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Reporting/Reports/rpt_GetMeritListOnlyPercentage_FormWise.rdlc");
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
                    OpenReportInPDF("rpt_GetMeritListOnlyPercentage_FormWise.pdf");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        protected void rpt_GetMeritListOnlyPercentage_FormWiseUnselected(string searchOrPrint)
        {
            try
            {
                string BatchProgramID = (this.cmbBatchPrograms.Value == "") ? "0" : this.cmbBatchPrograms.Value;
                string EntryTestID = (this.cmbEntryTest.Value == "") ? "0" : this.cmbEntryTest.Value;
                List<rpt_GetMeritListOnlyPercentage_FormWise_Result> lstForms = db.rpt_GetMeritListOnlyPercentage_FormWise(Convert.ToInt32(BatchProgramID), Convert.ToInt32(EntryTestID), 2).ToList();
                ReportViewer1.ProcessingMode = ProcessingMode.Local;
                ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Reporting/Reports/rpt_GetMeritListOnlyPercentage_FormWiseUnselected.rdlc");
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
                    OpenReportInPDF("rpt_GetMeritListOnlyPercentage_FormWiseUnselected.pdf");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        protected void rpt_GetMeritListOnlyPercentage_ResultUnselected(string searchOrPrint)
        {
            try
            {
                string BatchProgramID = (this.cmbBatchPrograms.Value == "") ? "0" : this.cmbBatchPrograms.Value;
                string EntryTestID = (this.cmbEntryTest.Value == "") ? "0" : this.cmbEntryTest.Value;
                List<rpt_GetMeritListOnlyPercentage_Result> lstForms = db.rpt_GetMeritListOnlyPercentage(Convert.ToInt32(BatchProgramID), Convert.ToInt32(EntryTestID), 2).ToList();
                ReportViewer1.ProcessingMode = ProcessingMode.Local;
                ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Reporting/Reports/rpt_GetMeritListOnlyPercentage_ResultUnselected.rdlc");
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
                    OpenReportInPDF("rpt_GetMeritListOnlyPercentage_ResultUnselected.pdf");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        protected void rpt_GetAdmissionApplicantMeritListWeightageWise(string searchOrPrint)
        {
            try
            {
                string BatchID = (this.cmbSessions.Value == "") ? "0" : this.cmbSessions.Value;
                string BatchProgramID = (this.cmbBatchPrograms.Value == "") ? "0" : this.cmbBatchPrograms.Value;
                string EntryTestID = (this.cmbEntryTest.Value == "") ? "0" : this.cmbEntryTest.Value;
                List<rpt_GetAdmissionApplicantMeritList_Result> lstForms = db.rpt_GetAdmissionApplicantMeritList(Convert.ToInt32(cmbDepartments.Value), "", Convert.ToInt32(BatchID), Convert.ToInt32(BatchProgramID), Convert.ToInt32(EntryTestID), "", 2).ToList();
                ReportViewer1.ProcessingMode = ProcessingMode.Local;
                ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Reporting/Reports/rpt_GetAdmissionApplicantMeritListWeightageWise.rdlc");
                ReportDataSource datasource = new ReportDataSource("DataSet1", lstForms);
                ReportViewer1.LocalReport.DataSources.Clear();

                ReportViewer1.LocalReport.DataSources.Add(datasource);

                ReportViewer1.LocalReport.EnableExternalImages = true;
                string imagePath = new Uri(Server.MapPath(MvcApplication.CampusLogo)).AbsoluteUri;
                ReportParameterCollection reportParameters = new ReportParameterCollection();
                reportParameters.Add(new ReportParameter("ImagePath", imagePath));
                reportParameters.Add(new ReportParameter("rpApplicantStatus", "All"));
                ReportViewer1.LocalReport.SetParameters(reportParameters);
                ReportViewer1.LocalReport.Refresh();
                ReportViewer1.Visible = true;

                if (searchOrPrint == "print")
                {
                    OpenReportInPDF("rpt_GetAdmissionApplicantMeritListWeightageWise.pdf");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        protected void rpt_GetAdmissionApplicantMeritListFormNoWise(string searchOrPrint)
        {
            try
            {
                string BatchID = (this.cmbSessions.Value == "") ? "0" : this.cmbSessions.Value;
                string BatchProgramID = (this.cmbBatchPrograms.Value == "") ? "0" : this.cmbBatchPrograms.Value;
                string EntryTestID = (this.cmbEntryTest.Value == "") ? "0" : this.cmbEntryTest.Value;
                List<rpt_GetAdmissionApplicantMeritList_Result> lstForms = db.rpt_GetAdmissionApplicantMeritList(Convert.ToInt32(cmbDepartments.Value), "", Convert.ToInt32(BatchID), Convert.ToInt32(BatchProgramID), Convert.ToInt32(EntryTestID), "", 1).ToList();
                ReportViewer1.ProcessingMode = ProcessingMode.Local;
                ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Reporting/Reports/rpt_GetAdmissionApplicantMeritListFormNoWise.rdlc");
                ReportDataSource datasource = new ReportDataSource("DataSet1", lstForms);
                ReportViewer1.LocalReport.DataSources.Clear();

                ReportViewer1.LocalReport.DataSources.Add(datasource);

                ReportViewer1.LocalReport.EnableExternalImages = true;
                string imagePath = new Uri(Server.MapPath(MvcApplication.CampusLogo)).AbsoluteUri;
                ReportParameterCollection reportParameters = new ReportParameterCollection();
                reportParameters.Add(new ReportParameter("ImagePath", imagePath));
                reportParameters.Add(new ReportParameter("rpApplicantStatus", "All"));
                ReportViewer1.LocalReport.SetParameters(reportParameters);
                ReportViewer1.LocalReport.Refresh();
                ReportViewer1.Visible = true;

                if (searchOrPrint == "print")
                {
                    OpenReportInPDF("rpt_GetAdmissionApplicantMeritListFormNoWise.pdf");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        protected void rpt_GetEntrytestDrawingTest(string searchOrPrint)
        {
            try
            {
                string FacultyID = (this.cmbDepartments.Value == "") ? "0" : this.cmbDepartments.Value;
                string BatchID = (this.cmbSessions.Value == "") ? "0" : this.cmbSessions.Value;
                string BatchProgramID = (this.cmbBatchPrograms.Value == "") ? "0" : this.cmbBatchPrograms.Value;
                string EntryTestID = (this.cmbEntryTest.Value == "") ? "0" : this.cmbEntryTest.Value;
                List<rpt_GetEntrytestDrawingTest_Result> lstForms = db.rpt_GetEntrytestDrawingTest(Convert.ToInt32(FacultyID), "", Convert.ToInt32(BatchID), Convert.ToInt32(BatchProgramID), Convert.ToInt32(EntryTestID)).ToList();
              
                ReportViewer1.ProcessingMode = ProcessingMode.Local;
                ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Reporting/Reports/rpt_GetEntrytestDrawingTest.rdlc");
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
                    OpenReportInPDF("rpt_GetEntrytestDrawingTest.pdf");
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
                case "rpt_GetAdmissionApplicationSummary":
                    lblReportTitle.InnerHtml = "Admission Applications Summary";
                    rpt_GetAdmissionApplicationSummary(searchOrPrint);
                    break;
                case "rpt_GetAdmissionApplicationDetail":
                    lblReportTitle.InnerHtml = "List of Applicants Test";
                    rpt_GetAdmissionApplicationDetail(searchOrPrint);
                    break;
                case "rpt_GetAdmissionApplicationDetailEntryTest":
                    lblReportTitle.InnerHtml = "List of Entry Test Applicants";
                    rpt_GetAdmissionApplicationDetailEntryTest(searchOrPrint);
                    break;
                case "rpt_GetMeritListOnlyPercentage":
                    lblReportTitle.InnerHtml = "Merit List (Selected)";
                    rpt_GetMeritListOnlyPercentage(searchOrPrint);
                    break;
                case "rpt_GetMeritListOnlyPercentage_WeightageWise":
                    lblReportTitle.InnerHtml = "Merit List Weightage Wise (Selected)";
                    rpt_GetMeritListOnlyPercentage_WeightageWise(searchOrPrint);
                    break;
                case "rpt_GetMeritListOnlyPercentage_FormWise":
                    lblReportTitle.InnerHtml = "Merit List Form # Wise (Selected)";
                    rpt_GetMeritListOnlyPercentage_FormWise(searchOrPrint);
                    break;
                case "rpt_GetMeritListOnlyPercentage_ResultUnselected":
                    lblReportTitle.InnerHtml = "Merit List Form # Wise (Unselected)";
                    rpt_GetMeritListOnlyPercentage_ResultUnselected(searchOrPrint);
                    break;
                case "rpt_GetMeritListOnlyPercentage_FormWiseUnselected":
                    lblReportTitle.InnerHtml = "Merit List Form # Wise (Unselected)";
                    rpt_GetMeritListOnlyPercentage_FormWiseUnselected(searchOrPrint);
                    break;
                case "rpt_GetMeritListOnlyPercentage_WeightageWiseUnselected":
                    lblReportTitle.InnerHtml = "Merit List Weightage Wise (Unselected)";
                    rpt_GetMeritListOnlyPercentage_WeightageWiseUnselected(searchOrPrint);
                    break;
                case "rpt_GetAdmissionApplicantMeritListWeightageWise":
                    lblReportTitle.InnerHtml = "Merit List";
                    rpt_GetAdmissionApplicantMeritListWeightageWise(searchOrPrint);
                    break;
                case "rpt_GetAdmissionApplicantMeritListFormNoWise":
                    lblReportTitle.InnerHtml = "Merit List";
                    rpt_GetAdmissionApplicantMeritListFormNoWise(searchOrPrint);
                    break;
                case "rpt_GetEntrytestDrawingTest":
                    lblReportTitle.InnerHtml = "Entry Test / Drawing Test Marks Entered";
                    rpt_GetEntrytestDrawingTest(searchOrPrint);
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