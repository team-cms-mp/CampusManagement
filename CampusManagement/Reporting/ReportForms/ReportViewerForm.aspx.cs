using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CampusManagement.Models;

namespace CampusManagement.Reporting.ReportForms
{
    public partial class ReportViewerForm : System.Web.UI.Page
    {
        ModelCMSContainer db = new ModelCMSContainer();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                LoadReport();
            }
        }

        protected void LoadFormSaleSlip(string ReportName)
        {
            if (Request.QueryString["FormID"] != "" && Request.QueryString["FormID"] != null)
            {
                try
                {
                    List<rpt_GetFormSaleSlip_Result> lstForms = db.rpt_GetFormSaleSlip(Convert.ToInt32(Request.QueryString["FormID"])).ToList();
                    ReportViewer1.ProcessingMode = ProcessingMode.Local;
                    ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Reporting/Reports/rptFormSaleSlip.rdlc");
                    ReportDataSource datasource = new ReportDataSource("DataSet1", lstForms);
                    ReportViewer1.LocalReport.DataSources.Clear();
                    ReportViewer1.LocalReport.DataSources.Add(datasource);

                    ReportViewer1.LocalReport.EnableExternalImages = true;
                    string imagePath = new Uri(Server.MapPath("~/assets/img/CampusConnectLogo.png")).AbsoluteUri;
                    ReportParameter parameter = new ReportParameter("ImagePath", imagePath);
                    ReportViewer1.LocalReport.SetParameters(parameter);
                    ReportViewer1.LocalReport.Refresh();
                    //if (Request.Browser.Browser == "Chrome")
                    //{
                    //    Byte[] bytes = ReportViewer1.LocalReport.Render("PDF");
                    //    Response.AddHeader("Content-Disposition", "inline; filename=MyReport.pdf");
                    //    Response.ContentType = "application/pdf";
                    //    Response.BinaryWrite(bytes);
                    //    Response.End();
                    //}
                    //else
                    //{
                        ReportViewer1.Visible = true;
                    //}
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        protected void LoadFormReceiveSlip(string ReportName)
        {
            if (Request.QueryString["FormID"] != "" && Request.QueryString["FormID"] != null)
            {
                try
                {
                    List<rpt_GetFormSaleSlip_Result> lstForms = db.rpt_GetFormSaleSlip(Convert.ToInt32(Request.QueryString["FormID"])).ToList();
                    ReportViewer1.ProcessingMode = ProcessingMode.Local;
                    ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Reporting/Reports/rptFormReceiveSlip.rdlc");
                    ReportDataSource datasource = new ReportDataSource("DataSet1", lstForms);
                    ReportViewer1.LocalReport.DataSources.Clear();
                    ReportViewer1.LocalReport.DataSources.Add(datasource);

                    ReportViewer1.LocalReport.EnableExternalImages = true;
                    string imagePath = new Uri(Server.MapPath("~/assets/img/CampusConnectLogo.png")).AbsoluteUri;
                    ReportParameter parameter = new ReportParameter("ImagePath", imagePath);
                    ReportViewer1.LocalReport.SetParameters(parameter);
                    ReportViewer1.LocalReport.Refresh();
                    ReportViewer1.Visible = true;
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        public void LoadStudentChallan()
        {
            try
            {
                if (Request.QueryString["ChallanID"] != "" && Request.QueryString["ChallanID"] != null)
                {
                    List<GetChallan_Result> lstChallan = db.GetChallan(Convert.ToInt32(Request.QueryString["ChallanID"])).ToList();
                    List<GetChallanService_Result> lstCS = db.GetChallanService(Convert.ToInt32(Request.QueryString["ChallanID"])).ToList();
                    ReportViewer1.ProcessingMode = ProcessingMode.Local;
                    ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Reporting/Reports/rptGetChallan.rdlc");
                    ReportDataSource datasource = new ReportDataSource("DataSet1", lstChallan);
                    ReportDataSource datasource2 = new ReportDataSource("DataSet2", lstCS);
                    ReportViewer1.LocalReport.DataSources.Clear();
                    ReportViewer1.LocalReport.DataSources.Add(datasource);
                    ReportViewer1.LocalReport.DataSources.Add(datasource2);

                    ReportViewer1.LocalReport.EnableExternalImages = true;
                    string imagePath = new Uri(Server.MapPath("~/assets/img/CampusConnectLogo.png")).AbsoluteUri;
                    ReportParameter parameter = new ReportParameter("ImagePath", imagePath);
                    ReportViewer1.LocalReport.SetParameters(parameter);
                    ReportViewer1.LocalReport.Refresh();
                    ReportViewer1.DataBind();
                }
            }
            catch (Exception ex)
            {
                Response.Write(ex.ToString());
            }
        }

        public void GetFormSaleDetail()
        {
            try
            {
                if (Request.QueryString["ChallanID"] != "" && Request.QueryString["ChallanID"] != null)
                {
                    List<GetFormSaleDetail_Result> lstForms = db.GetFormSaleDetail("", "").Where(f => f.PurchaseDate != null).ToList();
                    ReportViewer1.ProcessingMode = ProcessingMode.Local;
                    ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Reporting/Reports/rptAdmissionFormSale.rdlc");
                    ReportDataSource datasource = new ReportDataSource("DataSet1", lstForms);
                    ReportViewer1.LocalReport.DataSources.Clear();
                    ReportViewer1.LocalReport.DataSources.Add(datasource);

                    ReportViewer1.LocalReport.EnableExternalImages = true;
                    string imagePath = new Uri(Server.MapPath("~/assets/img/CampusConnectLogo.png")).AbsoluteUri;
                    ReportParameter parameter = new ReportParameter("ImagePath", imagePath);
                    ReportViewer1.LocalReport.SetParameters(parameter);
                    ReportViewer1.LocalReport.Refresh();
                    ReportViewer1.DataBind();
                }
            }
            catch (Exception ex)
            {
                Response.Write(ex.ToString());
            }
        }

        public void GetMeritList()
        {
            try
            {
                if ((Request.QueryString["IsBlank"] != "" && Request.QueryString["IsBlank"] != null)
                    && (Request.QueryString["BatchProgramID"] != "" && Request.QueryString["BatchProgramID"] != null))
                {
                    string reportName = "";
                    bool IsBlank = Convert.ToBoolean(Request.QueryString["IsBlank"]);
                    if (IsBlank)
                    {
                        reportName = "~/Reporting/Reports/rpt_MeritListBlank.rdlc";
                    }
                    else
                    {
                        reportName = "~/Reporting/Reports/rpt_MeritList.rdlc";
                    }
                    List<rpt_GetMeritList_Result> lstForms = db.rpt_GetMeritList(Convert.ToInt32(Request.QueryString["BatchProgramID"])).ToList();
                    ReportViewer1.ProcessingMode = ProcessingMode.Local;
                    ReportViewer1.LocalReport.ReportPath = Server.MapPath(reportName);
                    ReportDataSource datasource = new ReportDataSource("DataSet1", lstForms);
                    ReportViewer1.LocalReport.DataSources.Clear();
                    ReportViewer1.LocalReport.DataSources.Add(datasource);

                    ReportViewer1.LocalReport.EnableExternalImages = true;
                    string imagePath = new Uri(Server.MapPath("~/assets/img/CampusConnectLogo.png")).AbsoluteUri;
                    ReportParameter parameter = new ReportParameter("ImagePath", imagePath);
                    ReportViewer1.LocalReport.SetParameters(parameter);

                    ReportViewer1.LocalReport.Refresh();
                    ReportViewer1.DataBind();
                }
            }
            catch (Exception ex)
            {
                Response.Write(ex.ToString());
            }
        }

        protected void LoadReport()
        {
            string reportName = Request.QueryString["ReportName"];
            switch (reportName)
            {
                case "rptFormSaleSlip":
                    LoadFormSaleSlip(reportName);
                    break;
                case "rptFormReceiveSlip":
                    LoadFormReceiveSlip(reportName);
                    break;
                case "rptGetChallan":
                    LoadStudentChallan();
                    break;
                case "rptAdmissionFormSale":
                    GetFormSaleDetail();
                    break;
                case "rpt_MeritList":
                    GetMeritList();
                    break;
                default:
                    break;
            }
        }
    }
}