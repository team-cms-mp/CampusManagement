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
    public partial class MeritListForm : System.Web.UI.Page
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

            cmbBatchPrograms.DataSource = db.GetBatchProgramNameConcat(cmbSessions.Value, 0);
            cmbBatchPrograms.DataTextField = "Name";
            cmbBatchPrograms.DataValueField = "ID";
            cmbBatchPrograms.DataBind();

            cmbEntryTest.DataSource = db.EntryTests.ToList();
            cmbEntryTest.DataTextField = "EntryTestName";
            cmbEntryTest.DataValueField = "EntryTestID";
            cmbEntryTest.DataBind();
            cmbEntryTest.Items.Insert(0, new ListItem("---All---", "0"));

            cmbStatus.DataSource = db.InstituteStatus.ToList();
            cmbStatus.DataTextField = "InstituteStatusName";
            cmbStatus.DataValueField = "InstituteStatusID";
            cmbStatus.DataBind();
        }

        protected void rpt_GetAdmissionApplicantMeritListWeightageWise(string searchOrPrint)
        {
            try
            {
                string BatchID = (this.cmbSessions.Value == "") ? "0" : this.cmbSessions.Value;
                string BatchProgramID = (this.cmbBatchPrograms.Value == "") ? "0" : this.cmbBatchPrograms.Value;
                string EntryTestID = (this.cmbEntryTest.Value == "") ? "0" : this.cmbEntryTest.Value;
                List<rpt_GetAdmissionApplicantMeritList_Result> lstForms = db.rpt_GetAdmissionApplicantMeritList(Convert.ToInt32(cmbDepartments.Value), "", Convert.ToInt32(BatchID), Convert.ToInt32(BatchProgramID), Convert.ToInt32(EntryTestID), cmbStatus.Value, 2).ToList();
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
                List<rpt_GetAdmissionApplicantMeritList_Result> lstForms = db.rpt_GetAdmissionApplicantMeritList(Convert.ToInt32(cmbDepartments.Value), "", Convert.ToInt32(BatchID), Convert.ToInt32(BatchProgramID), Convert.ToInt32(EntryTestID), cmbStatus.Value, 1).ToList();
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

        protected void LoadReport(string searchOrPrint)
        {
            hdnReportName.Value = Request.QueryString["ReportName"];

            switch (hdnReportName.Value)
            {
                case "rpt_GetAdmissionApplicantMeritListWeightageWise":
                    lblReportTitle.InnerHtml = "Merit List";
                    rpt_GetAdmissionApplicantMeritListWeightageWise(searchOrPrint);
                    break;
                case "rpt_GetAdmissionApplicantMeritListFormNoWise":
                    lblReportTitle.InnerHtml = "Merit List";
                    rpt_GetAdmissionApplicantMeritListFormNoWise(searchOrPrint);
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