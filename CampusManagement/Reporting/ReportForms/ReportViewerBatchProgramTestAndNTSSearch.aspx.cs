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
    public partial class ReportViewerBatchProgramTestAndNTSSearch : System.Web.UI.Page
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

            int[] lstDegreeID = new[] { 22, 2020, 2021, 2022, 2023, 2024 };
            cmbDegreeID.DataSource = db.Degrees.Where(d => lstDegreeID.Contains(d.DegreeID)).ToList();
            cmbDegreeID.DataTextField = "DegreeName";
            cmbDegreeID.DataValueField = "DegreeID";
            cmbDegreeID.DataBind();
            cmbDegreeID.Items.Insert(0, new ListItem("---All---", "0"));
        }

        protected void rptGetNTSnonNTSStudents(string searchOrPrint)
        {
            try
            {
                string BatchID = (this.cmbSessions.Value == "") ? "0" : this.cmbSessions.Value;
                string BatchProgramID = (this.cmbBatchPrograms.Value == "") ? "0" : this.cmbBatchPrograms.Value;
                string DegreeID = (this.cmbDegreeID.Value == "") ? "0" : this.cmbDegreeID.Value;
                List<rptGetNTSnonNTSStudents_Result> lstForms = db.rptGetNTSnonNTSStudents(Convert.ToInt32(BatchID), Convert.ToInt32(BatchProgramID), Convert.ToInt32(DegreeID), Convert.ToInt32(cmbSubDepartment.Value)).ToList();
                ReportViewer1.ProcessingMode = ProcessingMode.Local;
                ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Reporting/Reports/rptGetNTSnonNTSStudents.rdlc");
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
                    OpenReportInPDF("rptGetNTSnonNTSStudents.pdf");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        protected void StudentsAppliedViaNtsAndEntryTest(string searchOrPrint)
        {
            try
            {
                string BatchID = (this.cmbSessions.Value == "") ? "0" : this.cmbSessions.Value;
                string BatchProgramID = (this.cmbBatchPrograms.Value == "") ? "0" : this.cmbBatchPrograms.Value;
                string DegreeID = (this.cmbDegreeID.Value == "") ? "0" : this.cmbDegreeID.Value;
                List<rptGetListOfStudentsAppliedViaNtsAndEntryTest_Result> lstForms = db.rptGetListOfStudentsAppliedViaNtsAndEntryTest(Convert.ToInt32(BatchID), Convert.ToInt32(BatchProgramID), Convert.ToInt32(DegreeID), Convert.ToInt32(cmbSubDepartment.Value)).ToList();
                ReportViewer1.ProcessingMode = ProcessingMode.Local;
                ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Reporting/Reports/StudentsAppliedViaNtsAndEntryTest.rdlc");
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
                    OpenReportInPDF("StudentsAppliedViaNtsAndEntryTest.pdf");
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
                case "rptGetNTSnonNTSStudents":
                    lblReportTitle.InnerHtml = "NTS or Non-NTS Student Summary";
                    rptGetNTSnonNTSStudents(searchOrPrint);
                    break;
                case "StudentsAppliedViaNtsAndEntryTest":
                    lblReportTitle.InnerHtml = "Students Applied Via NTS and Entry Test Detail";
                    StudentsAppliedViaNtsAndEntryTest(searchOrPrint);
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