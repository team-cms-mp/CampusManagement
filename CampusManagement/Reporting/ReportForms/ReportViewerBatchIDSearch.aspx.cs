
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using CampusManagement.Models;
using Microsoft.Reporting.WebForms;

namespace CampusManagement.Reporting.ReportForms
{
    public partial class ReportViewerBatchIDSearch : System.Web.UI.Page
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
            List<GetBatchProgramNameConcat_Result> BatchList = new List<GetBatchProgramNameConcat_Result>();
            BatchList = db.GetBatchProgramNameConcat("", 10).ToList();
            cmbBatch.DataSource = BatchList;
            cmbBatch.DataTextField = "Name";
            cmbBatch.DataValueField = "ID";
            cmbBatch.DataBind();
        }

        protected void sf_GetFeeRefundExistingDetailBatchWise(string searchOrPrint)
        {
            try
            {
                string BatchID = (this.cmbBatch.Value == "") ? "0" : this.cmbBatch.Value;
                List<sf_GetFeeRefundExistingDetailBatchWise_Result> lstForms = db.sf_GetFeeRefundExistingDetailBatchWise(Convert.ToInt32(BatchID), "Yes").ToList();
                ReportViewer1.ProcessingMode = ProcessingMode.Local;
                ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Reporting/Reports/rpt_sf_GetFeeRefundExistingDetailBatchWise.rdlc");
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
                    OpenReportInPDF("rpt_sf_GetFeeRefundExistingDetailBatchWise.pdf");
                }

            }
            catch (Exception)
            {
                throw;
            }
        }

        protected void sf_GetFeeCompleteDetailAllStudentBatchWise(string searchOrPrint)
        {
            try
            {
                string BatchID = (this.cmbBatch.Value == "") ? "0" : this.cmbBatch.Value;
                List<sf_GetFeeCompleteDetailAllStudentBatchWise_Result> lstForms = db.sf_GetFeeCompleteDetailAllStudentBatchWise(Convert.ToInt32(BatchID), "Yes").ToList();
                ReportViewer1.ProcessingMode = ProcessingMode.Local;
                ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Reporting/Reports/rpt_sf_GetFeeCompleteDetailAllStudentBatchWise.rdlc");
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
                    OpenReportInPDF("rpt_sf_GetFeeCompleteDetailAllStudentBatchWise.pdf");
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
                case "rpt_sf_GetFeeRefundExistingDetailBatchWise":
                    lblReportTitle.InnerHtml = "All Students Fee Receipt Detail";
                    sf_GetFeeRefundExistingDetailBatchWise(searchOrPrint);
                    break;
                case "rpt_sf_GetFeeCompleteDetailAllStudentBatchWise":
                    lblReportTitle.InnerHtml = "Fee Detail of All Student";
                    sf_GetFeeCompleteDetailAllStudentBatchWise(searchOrPrint);
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

            Byte[] bytes = ReportViewer1.LocalReport.Render("PDF");
            Response.AddHeader("Content-Disposition", "inline; filename=" + ReportNamePDF);
            Response.ContentType = "application/pdf";
            Response.BinaryWrite(bytes);
            Response.End();
            //}
        }
    }
}