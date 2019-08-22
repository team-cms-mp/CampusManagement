using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using CampusManagement.Models;

namespace CampusManagement.Reporting.ReportForms
{
    public partial class ReportViewerForm : System.Web.UI.Page
    {
        ModelCMSNewContainer db = new ModelCMSNewContainer();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                LoadReport();
            }
        }

        protected void LoadFormSaleSlip()
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
                    string imagePath = new Uri(Server.MapPath(MvcApplication.CampusLogo)).AbsoluteUri;
                    ReportParameter parameter = new ReportParameter("ImagePath", imagePath);
                    ReportViewer1.LocalReport.SetParameters(parameter);
                    ReportViewer1.LocalReport.Refresh();
                    ReportViewer1.Visible = true;

                    OpenReportInPDF("rptFormSaleSlip.pdf");
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        protected void LoadFormReceiveSlip()
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
                    string imagePath = new Uri(Server.MapPath(MvcApplication.CampusLogo)).AbsoluteUri;
                    ReportParameter parameter = new ReportParameter("ImagePath", imagePath);
                    ReportViewer1.LocalReport.SetParameters(parameter);
                    ReportViewer1.LocalReport.Refresh();
                    ReportViewer1.Visible = true;

                    OpenReportInPDF("rptFormReceiveSlip.pdf");
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
                    string Type = Request.QueryString["Type"];
                    List<GetChallan_Result> lstChallan = db.GetChallan(Convert.ToInt32(Request.QueryString["ChallanID"]), Type).ToList();
                    List<GetChallanService_Result> lstCS = db.GetChallanService(Convert.ToInt32(Request.QueryString["ChallanID"])).ToList();
                    ReportViewer1.ProcessingMode = ProcessingMode.Local;
                    ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Reporting/Reports/rptGetChallan.rdlc");
                    ReportDataSource datasource = new ReportDataSource("DataSet1", lstChallan);
                    ReportDataSource datasource2 = new ReportDataSource("DataSet2", lstCS);
                    ReportViewer1.LocalReport.DataSources.Clear();
                    ReportViewer1.LocalReport.DataSources.Add(datasource);
                    ReportViewer1.LocalReport.DataSources.Add(datasource2);

                    ReportViewer1.LocalReport.EnableExternalImages = true;
                    string imagePath = new Uri(Server.MapPath(MvcApplication.CampusLogo)).AbsoluteUri;
                    ReportParameter parameter = new ReportParameter("ImagePath", imagePath);
                    ReportViewer1.LocalReport.SetParameters(parameter);
                    ReportViewer1.LocalReport.Refresh();
                    ReportViewer1.DataBind();

                    OpenReportInPDF("rptGetChallan.pdf");
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
                    List<GetFormSaleDetail_Result> lstForms = db.GetFormSaleDetail("", "", 0, 0, null, null,0).Where(f => f.PurchaseDate != null).ToList();
                    ReportViewer1.ProcessingMode = ProcessingMode.Local;
                    ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Reporting/Reports/rptAdmissionFormSale.rdlc");
                    ReportDataSource datasource = new ReportDataSource("DataSet1", lstForms);
                    ReportViewer1.LocalReport.DataSources.Clear();
                    ReportViewer1.LocalReport.DataSources.Add(datasource);

                    ReportViewer1.LocalReport.EnableExternalImages = true;
                    string imagePath = new Uri(Server.MapPath(MvcApplication.CampusLogo)).AbsoluteUri;
                    ReportParameter parameter = new ReportParameter("ImagePath", imagePath);
                    ReportViewer1.LocalReport.SetParameters(parameter);
                    ReportViewer1.LocalReport.Refresh();
                    ReportViewer1.DataBind();

                    OpenReportInPDF("rptAdmissionFormSale.pdf");
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
                        reportName = "~/Reporting/Reports/rpt_GetMeritListDynamicColumnBlank.rdlc";
                    }
                    else
                    {
                        reportName = "~/Reporting/Reports/rpt_GetMeritListDynamicColumn.rdlc";
                    }
                    List<rpt_GetMeritList_Result> lstForms = db.rpt_GetMeritList(Convert.ToInt32(Request.QueryString["BatchProgramID"])).ToList();
                    ReportViewer1.ProcessingMode = ProcessingMode.Local;
                    ReportViewer1.LocalReport.ReportPath = Server.MapPath(reportName);
                    ReportDataSource datasource = new ReportDataSource("DataSet1", lstForms);
                    ReportViewer1.LocalReport.DataSources.Clear();
                    ReportViewer1.LocalReport.DataSources.Add(datasource);

                    ReportViewer1.LocalReport.EnableExternalImages = true;
                    string imagePath = new Uri(Server.MapPath(MvcApplication.CampusLogo)).AbsoluteUri;
                    ReportParameter parameter = new ReportParameter("ImagePath", imagePath);
                    ReportViewer1.LocalReport.SetParameters(parameter);

                    ReportViewer1.LocalReport.Refresh();
                    ReportViewer1.DataBind();

                    OpenReportInPDF("rpt_GetMeritListDynamicColumnBlank.pdf");
                    OpenReportInPDF("rpt_GetMeritListDynamicColumn.pdf");
                }
            }
            catch (Exception ex)
            {
                Response.Write(ex.ToString());
            }
        }

        public void GetApplicantProfileReport()
        {
            try
            {
                if (Request.QueryString["FormNo"] != "" && Request.QueryString["FormNo"] != null)
                {
                    string FormNo = Request.QueryString["FormNo"];
                    List<GetApplicantPersonalInfo_Result> lstAD = db.GetApplicantPersonalInfo(FormNo).ToList();
                    List<GetApplyForPrograms_Result> lstAFP = db.GetApplyForPrograms(FormNo).ToList();
                    List<GetQualification_Result> lstQ = db.GetQualification(FormNo).ToList();
                    List<GetEntranceTests_Result> lstE = db.GetEntranceTests(FormNo).ToList();
                    ReportViewer1.ProcessingMode = ProcessingMode.Local;
                    ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Reporting/Reports/rptCandidateProfile.rdlc");
                    ReportDataSource datasource = new ReportDataSource("DataSet1", lstAD);
                    ReportDataSource datasource2 = new ReportDataSource("DataSet3", lstAFP);
                    ReportDataSource datasource3 = new ReportDataSource("DataSet2", lstQ);
                    ReportDataSource datasource4 = new ReportDataSource("DataSet4", lstE);
                    ReportViewer1.LocalReport.DataSources.Clear();
                    ReportViewer1.LocalReport.DataSources.Add(datasource);
                    ReportViewer1.LocalReport.DataSources.Add(datasource2);
                    ReportViewer1.LocalReport.DataSources.Add(datasource3);
                    ReportViewer1.LocalReport.DataSources.Add(datasource4);

                    ReportViewer1.LocalReport.EnableExternalImages = true;
                    string imagePath = new Uri(Server.MapPath(lstAD[0].Picture)).AbsoluteUri;
                    ReportParameter parameter = new ReportParameter("ImagePath", imagePath);
                    ReportViewer1.LocalReport.SetParameters(parameter);

                    ReportViewer1.LocalReport.EnableExternalImages = true;
                    string imagePath2 = new Uri(Server.MapPath(MvcApplication.CampusLogo)).AbsoluteUri;
                    ReportParameter parameter2 = new ReportParameter("ImagePath2", imagePath2);
                    ReportViewer1.LocalReport.SetParameters(parameter2);

                    ReportViewer1.LocalReport.Refresh();
                    ReportViewer1.DataBind();

                    OpenReportInPDF("rptCandidateProfile.pdf");
                }
            }
            catch (Exception ex)
            {
                Response.Write(ex.ToString());
            }
        }

        public void GetStudentProfileReport()
        {
            try
            {
                if (Request.QueryString["FormNo"] != "" && Request.QueryString["FormNo"] != null)
                {
                    string FormNo = Request.QueryString["FormNo"];
                    List<GetStudentPersonalInfo_Result> lstSPI = db.GetStudentPersonalInfo(FormNo).ToList();
                    List<GetApplyForPrograms_Result> lstAFP = db.GetApplyForPrograms(FormNo).ToList();
                    List<GetQualification_Result> lstQ = db.GetQualification(FormNo).ToList();
                    List<GetEntranceTests_Result> lstE = db.GetEntranceTests(FormNo).ToList();
                    ReportViewer1.ProcessingMode = ProcessingMode.Local;
                    ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Reporting/Reports/rptStudentProfile.rdlc");
                    ReportDataSource datasource = new ReportDataSource("DataSet1", lstSPI);
                    ReportDataSource datasource2 = new ReportDataSource("DataSet3", lstAFP);
                    ReportDataSource datasource3 = new ReportDataSource("DataSet2", lstQ);
                    ReportDataSource datasource4 = new ReportDataSource("DataSet4", lstE);
                    ReportViewer1.LocalReport.DataSources.Clear();
                    ReportViewer1.LocalReport.DataSources.Add(datasource);
                    ReportViewer1.LocalReport.DataSources.Add(datasource2);
                    ReportViewer1.LocalReport.DataSources.Add(datasource3);
                    ReportViewer1.LocalReport.DataSources.Add(datasource4);

                    ReportViewer1.LocalReport.EnableExternalImages = true;
                    string imagePath = new Uri(Server.MapPath(lstSPI[0].Picture)).AbsoluteUri;
                    ReportParameter parameter = new ReportParameter("ImagePath", imagePath);
                    ReportViewer1.LocalReport.SetParameters(parameter);

                    ReportViewer1.LocalReport.EnableExternalImages = true;
                    string imagePath2 = new Uri(Server.MapPath(MvcApplication.CampusLogo)).AbsoluteUri;
                    ReportParameter parameter2 = new ReportParameter("ImagePath2", imagePath2);
                    ReportViewer1.LocalReport.SetParameters(parameter2);

                    ReportViewer1.LocalReport.Refresh();
                    ReportViewer1.DataBind();
                    OpenReportInPDF("rptStudentProfile.pdf");
                }
            }
            catch (Exception ex)
            {
                Response.Write(ex.ToString());
            }
        }

        public void GetStudentChallanDetailReport()
        {
            try
            {
                if (Request.QueryString["FormNo"] != "" && Request.QueryString["FormNo"] != null)
                {
                    string FormNo = Request.QueryString["FormNo"];
                    List<rpt_StudentFinanceDetail_Result> lstForms = db.rpt_StudentFinanceDetail().Where(s => s.FormNo == FormNo).ToList();
                    ReportViewer1.ProcessingMode = ProcessingMode.Local;
                    ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Reporting/Reports/rpt_StudentChallanDetail.rdlc");
                    ReportDataSource datasource = new ReportDataSource("DataSet1", lstForms);
                    ReportViewer1.LocalReport.DataSources.Clear();
                    ReportViewer1.LocalReport.DataSources.Add(datasource);

                    ReportViewer1.LocalReport.EnableExternalImages = true;
                    string imagePath = new Uri(Server.MapPath(MvcApplication.CampusLogo)).AbsoluteUri;
                    ReportParameter parameter = new ReportParameter("ImagePath", imagePath);
                    ReportViewer1.LocalReport.SetParameters(parameter);
                    ReportViewer1.LocalReport.Refresh();
                    ReportViewer1.DataBind();

                    OpenReportInPDF("rpt_StudentChallanDetail.pdf");
                }
            }
            catch (Exception ex)
            {
                Response.Write(ex.ToString());
            }
        }
        public void GetStudentBatchProgramCoursesReport()
        {
            try
            {
                if (Request.QueryString["FormNo"] != "" && Request.QueryString["FormNo"] != null)
                {
                    string FormNo = Request.QueryString["FormNo"];
                    List<rpt_StudentBatchProgramCourses_Result> lstForms = db.rpt_StudentBatchProgramCourses(FormNo).ToList();
                    ReportViewer1.ProcessingMode = ProcessingMode.Local;
                    ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Reporting/Reports/rpt_StudentBatchProgramCourses.rdlc");
                    ReportDataSource datasource = new ReportDataSource("DataSet1", lstForms);
                    ReportViewer1.LocalReport.DataSources.Clear();
                    ReportViewer1.LocalReport.DataSources.Add(datasource);

                    ReportViewer1.LocalReport.EnableExternalImages = true;
                    string imagePath = new Uri(Server.MapPath(MvcApplication.CampusLogo)).AbsoluteUri;
                    ReportParameter parameter = new ReportParameter("ImagePath", imagePath);
                    ReportViewer1.LocalReport.SetParameters(parameter);
                    ReportViewer1.LocalReport.Refresh();
                    ReportViewer1.DataBind();

                    OpenReportInPDF("rpt_StudentBatchProgramCourses.pdf");
                }
            }
            catch (Exception ex)
            {
                Response.Write(ex.ToString());
            }
        }

        protected void LoadEmptyChallan()
        {
            try
            {
                List<rptGetChallanEmpty_Result> lstForms = db.rptGetChallanEmpty().ToList();
                ReportViewer1.ProcessingMode = ProcessingMode.Local;
                ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Reporting/Reports/rptGetChallanEmpty.rdlc");
                ReportDataSource datasource = new ReportDataSource("DataSet1", lstForms);
                ReportViewer1.LocalReport.DataSources.Clear();
                ReportViewer1.LocalReport.DataSources.Add(datasource);

                ReportViewer1.LocalReport.EnableExternalImages = true;
                string imagePath = new Uri(Server.MapPath(MvcApplication.CampusLogo)).AbsoluteUri;
                ReportParameter parameter = new ReportParameter("ImagePath", imagePath);
                ReportViewer1.LocalReport.SetParameters(parameter);
                ReportViewer1.LocalReport.Refresh();
                ReportViewer1.Visible = true;

                OpenReportInPDF("rptGetChallanEmpty.pdf");
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void LoadBeliTrustForm()
        {
            try
            {
                if (Request.QueryString["FormNo"] != "" && Request.QueryString["FormNo"] != null)
                {
                    List<rpt_GetBeliTrust_Result> lstBeliTrust = db.rpt_GetBeliTrust(Request.QueryString["FormNo"]).ToList();
                    List<rpt_GetBeliTrustAffidavit_Result> lstBeliTrustAffidavit = db.rpt_GetBeliTrustAffidavit(Request.QueryString["FormNo"]).ToList();
                    List<rpt_GetBeliTrustFamilyMembers_Result> lstBeliTrustFamilyMembers = db.rpt_GetBeliTrustFamilyMembers(Request.QueryString["FormNo"]).ToList();
                    List<rpt_GetBeliTrustFinancialSupport_Result> lstBeliTrustFinancialSupports = db.rpt_GetBeliTrustFinancialSupport(Request.QueryString["FormNo"]).ToList();
                    List<GetQualification_Result> lstQualifications = db.GetQualification(Request.QueryString["FormNo"]).ToList();
                    ReportViewer1.ProcessingMode = ProcessingMode.Local;
                    ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Reporting/Reports/rptBeliTrust.rdlc");
                    ReportDataSource datasource1 = new ReportDataSource("DataSet1", lstBeliTrust);
                    ReportDataSource datasource2 = new ReportDataSource("DataSet2", lstBeliTrustAffidavit);
                    ReportDataSource datasource3 = new ReportDataSource("DataSet3", lstBeliTrustFamilyMembers);
                    ReportDataSource datasource4 = new ReportDataSource("DataSet4", lstBeliTrustFinancialSupports);
                    ReportDataSource datasource5 = new ReportDataSource("DataSet5", lstQualifications);
                    ReportViewer1.LocalReport.DataSources.Clear();
                    ReportViewer1.LocalReport.DataSources.Add(datasource1);
                    ReportViewer1.LocalReport.DataSources.Add(datasource2);
                    ReportViewer1.LocalReport.DataSources.Add(datasource3);
                    ReportViewer1.LocalReport.DataSources.Add(datasource4);
                    ReportViewer1.LocalReport.DataSources.Add(datasource5);

                    //ReportViewer1.LocalReport.EnableExternalImages = true;
                    //string imagePath = new Uri(Server.MapPath(MvcApplication.CampusLogo)).AbsoluteUri;
                    //ReportParameter parameter = new ReportParameter("ImagePath", imagePath);
                    //ReportViewer1.LocalReport.SetParameters(parameter);
                    ReportViewer1.LocalReport.Refresh();
                    ReportViewer1.DataBind();

                    OpenReportInPDF("rptBeliTrust.pdf");
                }
            }
            catch (Exception ex)
            {
                Response.Write(ex.ToString());
            }
        }

        public void GetAdmitCard()
        {
            try
            {
                if (Request.QueryString["FormNo"] != "" && Request.QueryString["FormNo"] != null)
                {
                    List<rptGetAdmitCard_Result> lstForms = db.rptGetAdmitCard(Request.QueryString["FormNo"]).ToList();
                    List<GetApplyForPrograms_Result> LstAfp = db.GetApplyForPrograms(Request.QueryString["FormNo"]).ToList();
                    ReportViewer1.ProcessingMode = ProcessingMode.Local;
                    ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Reporting/Reports/rptGetAdmitCard.rdlc");
                    ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Reporting/Reports/rptGetAdmitCard.rdlc");
                    ReportDataSource datasource = new ReportDataSource("DataSet1", lstForms);
                    ReportDataSource datasource1 = new ReportDataSource("DataSet2", LstAfp);
                    ReportViewer1.LocalReport.DataSources.Clear();
                    ReportViewer1.LocalReport.DataSources.Add(datasource);
                    ReportViewer1.LocalReport.DataSources.Add(datasource1);

                    ReportViewer1.LocalReport.EnableExternalImages = true;
                    string imagePath = new Uri(Server.MapPath(MvcApplication.CampusLogo)).AbsoluteUri;
                    ReportParameter parameter = new ReportParameter("ImagePath", imagePath);
                    ReportViewer1.LocalReport.SetParameters(parameter);

                    string appPic = "~/ProfilePics/noimage.png";
                    if (lstForms.Count > 0)
                    {
                        appPic = lstForms[0].Picture;
                    }

                    string imagePath2 = new Uri(Server.MapPath(appPic)).AbsoluteUri;
                    ReportParameter parameter2 = new ReportParameter("ImagePath2", imagePath2);
                    ReportViewer1.LocalReport.SetParameters(parameter2);

                    string imagePathQRCode = new Uri(Server.MapPath("~/Content/images/QrCode.jpg")).AbsoluteUri;
                    ReportParameter paramimagePathQRCode = new ReportParameter("ImagePathQRCode", imagePathQRCode);
                    ReportViewer1.LocalReport.SetParameters(paramimagePathQRCode);

                    string imagePathQRCode2 = new Uri(Server.MapPath("~/Content/images/QrCode.jpg")).AbsoluteUri;
                    ReportParameter paramimagePathQRCode2 = new ReportParameter("ImagePathQRCode2", imagePathQRCode2);
                    ReportViewer1.LocalReport.SetParameters(paramimagePathQRCode2);

                    ReportViewer1.LocalReport.Refresh();
                    ReportViewer1.DataBind();

                    OpenReportInPDF("rptGetAdmitCard.pdf");
                }
            }
            catch (Exception ex)
            {
                Response.Write(ex.ToString());
            }
        }

        public void rptGetAdmitCardFormReceived()
        {
            try
            {
                if (Request.QueryString["FormNo"] != "" && Request.QueryString["FormNo"] != null)
                {
                    List<rptGetAdmitCardFormReceived_Result> lstForms = db.rptGetAdmitCardFormReceived(Request.QueryString["FormNo"]).ToList();
                    List<GetApplyForPrograms_Result> LstAfp = db.GetApplyForPrograms(Request.QueryString["FormNo"]).ToList();
                    ReportViewer1.ProcessingMode = ProcessingMode.Local;
                    ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Reporting/Reports/rptGetAdmitCardFormReceived.rdlc");
                    ReportDataSource datasource = new ReportDataSource("DataSet1", lstForms);
                    ReportDataSource datasource1 = new ReportDataSource("DataSet2", LstAfp);

                    ReportViewer1.LocalReport.DataSources.Clear();
                    ReportViewer1.LocalReport.DataSources.Add(datasource);
                    ReportViewer1.LocalReport.DataSources.Add(datasource1);

                    ReportViewer1.LocalReport.EnableExternalImages = true;
                    string imagePath = new Uri(Server.MapPath(MvcApplication.CampusLogo)).AbsoluteUri;
                    ReportParameter parameter = new ReportParameter("ImagePath", imagePath);
                    ReportViewer1.LocalReport.SetParameters(parameter);

                    string imagePathQRCode = new Uri(Server.MapPath("~/Content/images/QrCode.jpg")).AbsoluteUri;
                    ReportParameter paramimagePathQRCode = new ReportParameter("ImagePathQRCode", imagePathQRCode);
                    ReportViewer1.LocalReport.SetParameters(paramimagePathQRCode);

                    string imagePathQRCode2 = new Uri(Server.MapPath("~/Content/images/QrCode.jpg")).AbsoluteUri;
                    ReportParameter paramimagePathQRCode2 = new ReportParameter("ImagePathQRCode2", imagePathQRCode2);
                    ReportViewer1.LocalReport.SetParameters(paramimagePathQRCode2);

                    ReportViewer1.LocalReport.Refresh();
                    ReportViewer1.DataBind();

                    OpenReportInPDF("rptGetAdmitCardFormReceived.pdf");
                }
            }
            catch (Exception ex)
            {
                Response.Write(ex.ToString());
            }
        }

        protected void rpt_GetMeritListOnlyPercentage()
        {
            try
            {
                if ((Request.QueryString["BatchProgramID"] != "" && Request.QueryString["BatchProgramID"] != null) && (Request.QueryString["EntryTestID"] != "" && Request.QueryString["EntryTestID"] != null))
                {
                    List<rpt_GetMeritListOnlyPercentage_Result> lstForms = db.rpt_GetMeritListOnlyPercentage(Convert.ToInt32(Request.QueryString["BatchProgramID"]), Convert.ToInt32(Request.QueryString["EntryTestID"]), 1).ToList();
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

                    OpenReportInPDF("rpt_GetMeritListOnlyPercentage.pdf");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        protected void rpt_GetMeritListOnlyPercentageUnselected()
        {
            try
            {
                if ((Request.QueryString["BatchProgramID"] != "" && Request.QueryString["BatchProgramID"] != null) && (Request.QueryString["EntryTestID"] != "" && Request.QueryString["EntryTestID"] != null))
                {
                    List<rpt_GetMeritListOnlyPercentage_Result> lstForms = db.rpt_GetMeritListOnlyPercentage(Convert.ToInt32(Request.QueryString["BatchProgramID"]), Convert.ToInt32(Request.QueryString["EntryTestID"]), 1).ToList();
                    ReportViewer1.ProcessingMode = ProcessingMode.Local;
                    ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Reporting/Reports/rpt_GetMeritListOnlyPercentageUnselected.rdlc");
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

                    OpenReportInPDF("rpt_GetMeritListOnlyPercentageUnselected.pdf");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }


        public void rpt_SF_GetCollegeServiceWiseAmounts()
        {
            try
            {

                List<SF_GetCollegeServiceWiseAmounts_Result> lstForms = db.SF_GetCollegeServiceWiseAmounts("Yes", "FormNo", "Student", 0, 0).ToList();
                ReportViewer1.ProcessingMode = ProcessingMode.Local;
                ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Reporting/Reports/rpt_SF_GetCollegeServiceWiseAmounts.rdlc");
                ReportDataSource datasource = new ReportDataSource("DataSet1", lstForms);
                ReportViewer1.LocalReport.DataSources.Clear();
                ReportViewer1.LocalReport.DataSources.Add(datasource);

                ReportViewer1.LocalReport.EnableExternalImages = true;
                string imagePath = new Uri(Server.MapPath(MvcApplication.CampusLogo)).AbsoluteUri;
                ReportParameter parameter = new ReportParameter("ImagePath", imagePath);
                ReportViewer1.LocalReport.SetParameters(parameter);
                ReportViewer1.LocalReport.Refresh();
                ReportViewer1.DataBind();

                OpenReportInPDF("rpt_SF_GetCollegeServiceWiseAmounts.pdf");
            }
            catch (Exception ex)
            {
                Response.Write(ex.ToString());
            }
        }

        public void rpt_SF_PayableServiceWiseAmounts()
        {
            try
            {

                List<SF_GetCollegeServiceWiseAmounts_Result> lstForms = db.SF_GetCollegeServiceWiseAmounts("No", "FormNo", "Student", 0, 0).ToList();
                ReportViewer1.ProcessingMode = ProcessingMode.Local;
                ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Reporting/Reports/rpt_SF_PayableServiceWiseAmounts.rdlc");
                ReportDataSource datasource = new ReportDataSource("DataSet1", lstForms);
                ReportViewer1.LocalReport.DataSources.Clear();
                ReportViewer1.LocalReport.DataSources.Add(datasource);

                ReportViewer1.LocalReport.EnableExternalImages = true;
                string imagePath = new Uri(Server.MapPath(MvcApplication.CampusLogo)).AbsoluteUri;
                ReportParameter parameter = new ReportParameter("ImagePath", imagePath);
                ReportViewer1.LocalReport.SetParameters(parameter);
                ReportViewer1.LocalReport.Refresh();
                ReportViewer1.DataBind();

                OpenReportInPDF("rpt_SF_PayableServiceWiseAmounts.pdf");
            }
            catch (Exception ex)
            {
                Response.Write(ex.ToString());
            }
        }

        protected void rpt_PaymentSummary()
        {
            try
            {
                List<GetApplicantStudentChallansSummary_Result> lstForms = db.GetApplicantStudentChallansSummary("FormNo", "Student", 0, 0).ToList();
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

                OpenReportInPDF("rpt_PaymentSummary.pdf");
            }
            catch (Exception)
            {
                throw;
            }
        }

        protected void rpt_PayableAmount()
        {
            try
            {
                List<SF_GetCollegeServiceWiseAmounts_Result> lstForms = db.SF_GetCollegeServiceWiseAmounts("No", "FormNo", "Student", 0, 0).ToList();
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

                OpenReportInPDF("rpt_PayableAmount.pdf");
            }
            catch (Exception)
            {
                throw;
            }
        }

        protected void rpt_PCollectedAmount()
        {
            try
            {
                List<SF_GetCollegeServiceWiseAmounts_Result> lstForms = db.SF_GetCollegeServiceWiseAmounts("Yes", "FormNo", "Student", 0, 0).ToList();
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

                OpenReportInPDF("rpt_PCollectedAmount.pdf");
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void GetApplicantCompleteDetails()
        {
            try
            {
                if (Request.QueryString["FormNo"] != "" && Request.QueryString["FormNo"] != null)
                {
                    string FormNo = Request.QueryString["FormNo"];
                    List<rpt_GetApplicantDetails_Result> lstAD = db.rpt_GetApplicantDetails(FormNo).ToList();
                    List<GetApplyForPrograms_Result> lstAFP = db.GetApplyForPrograms(FormNo).ToList();
                    List<GetQualification_Result> lstQ = db.GetQualification(FormNo).ToList();
                    List<GetEntranceTests_Result> lstE = db.GetEntranceTests(FormNo).ToList();
                    List<GetInstituteStatus_Result> lstStatus = db.GetInstituteStatus(FormNo, 1).ToList();
                    ReportViewer1.ProcessingMode = ProcessingMode.Local;
                    ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Reporting/Reports/rptApplicantCompleteDetails.rdlc");
                    ReportDataSource datasource = new ReportDataSource("DataSet1", lstAD);
                    ReportDataSource datasource2 = new ReportDataSource("DataSet3", lstAFP);
                    ReportDataSource datasource3 = new ReportDataSource("DataSet2", lstQ);
                    ReportDataSource datasource4 = new ReportDataSource("DataSet4", lstE);
                    ReportDataSource datasource5 = new ReportDataSource("DataSet5", lstStatus);
                    ReportViewer1.LocalReport.DataSources.Clear();
                    ReportViewer1.LocalReport.DataSources.Add(datasource);
                    ReportViewer1.LocalReport.DataSources.Add(datasource2);
                    ReportViewer1.LocalReport.DataSources.Add(datasource3);
                    ReportViewer1.LocalReport.DataSources.Add(datasource4);
                    ReportViewer1.LocalReport.DataSources.Add(datasource5);

                    ReportViewer1.LocalReport.EnableExternalImages = true;
                    string imagePath = new Uri(Server.MapPath(lstAD[0].Picture)).AbsoluteUri;
                    ReportParameter parameter = new ReportParameter("ImagePath", imagePath);
                    ReportViewer1.LocalReport.SetParameters(parameter);

                    ReportViewer1.LocalReport.EnableExternalImages = true;
                    string imagePath2 = new Uri(Server.MapPath(MvcApplication.CampusLogo)).AbsoluteUri;
                    ReportParameter parameter2 = new ReportParameter("ImagePath2", imagePath2);
                    ReportViewer1.LocalReport.SetParameters(parameter2);

                    ReportViewer1.LocalReport.Refresh();
                    ReportViewer1.DataBind();

                    OpenReportInPDF("rptApplicantCompleteDetails.pdf");
                }
            }
            catch (Exception ex)
            {
                Response.Write(ex.ToString());
            }
        }

        public void rpt_FormSaleSummary()
        {
            try
            {
                List<rpt_FormSaleSummary_Result> lstAD = db.rpt_FormSaleSummary(0,0).ToList();
                ReportViewer1.ProcessingMode = ProcessingMode.Local;
                ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Reporting/Reports/rpt_FormSaleSummary.rdlc");
                ReportDataSource datasource = new ReportDataSource("DataSet1", lstAD);
                ReportViewer1.LocalReport.DataSources.Clear();
                ReportViewer1.LocalReport.DataSources.Add(datasource);

                ReportViewer1.LocalReport.EnableExternalImages = true;
                string imagePath2 = new Uri(Server.MapPath(MvcApplication.CampusLogo)).AbsoluteUri;
                ReportParameter parameter2 = new ReportParameter("ImagePath", imagePath2);
                ReportViewer1.LocalReport.SetParameters(parameter2);

                ReportViewer1.LocalReport.Refresh();
                ReportViewer1.DataBind();

                OpenReportInPDF("rpt_FormSaleSummary.pdf");
            }
            catch (Exception ex)
            {
                Response.Write(ex.ToString());
            }
        }

        public void um_rpt_GetIncompleteApplicantDetail()
        {
            try
            {
                List<um_rpt_GetIncompleteApplicantDetail_Result> lstAD = db.um_rpt_GetIncompleteApplicantDetail("").ToList();
                ReportViewer1.ProcessingMode = ProcessingMode.Local;
                ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Reporting/Reports/um_rpt_GetIncompleteApplicantDetail.rdlc");
                ReportDataSource datasource = new ReportDataSource("DataSet1", lstAD);
                ReportViewer1.LocalReport.DataSources.Clear();
                ReportViewer1.LocalReport.DataSources.Add(datasource);

                ReportViewer1.LocalReport.EnableExternalImages = true;
                string imagePath2 = new Uri(Server.MapPath(MvcApplication.CampusLogo)).AbsoluteUri;
                ReportParameter parameter2 = new ReportParameter("ImagePath", imagePath2);
                ReportViewer1.LocalReport.SetParameters(parameter2);

                ReportViewer1.LocalReport.Refresh();
                ReportViewer1.DataBind();

                OpenReportInPDF("um_rpt_GetIncompleteApplicantDetail.pdf");
            }
            catch (Exception ex)
            {
                Response.Write(ex.ToString());
            }
        }

        protected void rpt_GetAdmissionApplicantMeritListWeightageWise()
        {
            try
            {
                if ((Request.QueryString["BatchID"] != "" && Request.QueryString["BatchID"] != null)
                    && (Request.QueryString["BatchProgramID"] != "" && Request.QueryString["BatchProgramID"] != null)
                    && (Request.QueryString["EntryTestID"] != "" && Request.QueryString["EntryTestID"] != null)
                    && (Request.QueryString["QueryID"] != "" && Request.QueryString["QueryID"] != null))
                {
                    List<rpt_GetAdmissionApplicantMeritList_Result> lstForms = db.rpt_GetAdmissionApplicantMeritList(0, "", Convert.ToInt32(Request.QueryString["BatchID"]), Convert.ToInt32(Request.QueryString["BatchProgramID"]), Convert.ToInt32(Request.QueryString["EntryTestID"]), "", Convert.ToInt32(Request.QueryString["QueryID"])).ToList();
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

                    OpenReportInPDF("rpt_GetAdmissionApplicantMeritListWeightageWise.pdf");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        protected void LoadReport()
        {
            string reportName = Request.QueryString["ReportName"];
            switch (reportName)
            {
                case "rptFormSaleSlip":
                    LoadFormSaleSlip();
                    break;
                case "rptFormReceiveSlip":
                    LoadFormReceiveSlip();
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
                case "rptCandidateProfile":
                    GetApplicantProfileReport();
                    break;
                case "rptStudentProfile":
                    GetStudentProfileReport();
                    break;
                case "rpt_StudentChallanDetail":
                    GetStudentChallanDetailReport();
                    break;
                case "rpt_StudentBatchProgramCourses":
                    GetStudentBatchProgramCoursesReport();
                    break;
                case "rptBeliTrust":
                    LoadBeliTrustForm();
                    break;
                case "rptGetAdmitCard":
                    GetAdmitCard();
                    break;
                case "rptGetAdmitCardFormReceived":
                    rptGetAdmitCardFormReceived();
                    break;
                case "rpt_GetMeritListOnlyPercentage":
                    rpt_GetMeritListOnlyPercentage();
                    break;
                case "rpt_GetMeritListOnlyPercentageUnselected":
                    rpt_GetMeritListOnlyPercentageUnselected();
                    break;
                case "rpt_PaymentSummary":
                    rpt_PaymentSummary();
                    break;
                case "rpt_PayableAmount":
                    rpt_PayableAmount();
                    break;
                case "rpt_GetAdmissionApplicantMeritListWeightageWise":
                    rpt_GetAdmissionApplicantMeritListWeightageWise();
                    break;
                case "rpt_PCollectedAmount":
                    rpt_PCollectedAmount();
                    break;
                case "rpt_SF_GetCollegeServiceWiseAmounts":
                    rpt_SF_GetCollegeServiceWiseAmounts();
                    break;
                case "rpt_SF_PayableServiceWiseAmounts":
                    rpt_SF_PayableServiceWiseAmounts();
                    break;
                case "rptApplicantCompleteDetails":
                    GetApplicantCompleteDetails();
                    break;
                case "um_rpt_GetIncompleteApplicantDetail":
                    um_rpt_GetIncompleteApplicantDetail();
                    break;
                case "rpt_FormSaleSummary":
                    rpt_FormSaleSummary();
                    break;
            }
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