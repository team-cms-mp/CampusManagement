<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ReportViewerExamStudentSubjectClash.aspx.cs" Inherits="CampusManagement.Reporting.ReportForms.ReportViewerExamStudentSubjectClash" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=12.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="~/Content/bootstrap.css" rel="stylesheet" />
    <script type="text/javascript">
        function SetTarget() {
            document.forms[0].target = "_blank";
        }
    </script>
</head>
<body>

    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <asp:UpdatePanel ID="updatePaned" runat="server" UpdateMode="Always">

            <ContentTemplate>
                <div class="row>">
                    <div class="col-md-12">
                        <h3>
                            <label id="lblReportTitle" runat="server"></label>
                        </h3>
                        <input type="hidden" id="hdnReportName" class="form-control" runat="server" />
                    </div>
                </div>
                <hr />
                <table style="width: 40%;">
                    <tr>
                        <td style="text-align: right;">
                            <label class="control-label">Exam</label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlExam" runat="server" class="form-control">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td></td>
                        <td>
                            <asp:Button ID="btnSearch" CssClass="btn btn-primary" runat="server" Text="Search" OnClick="btnSearch_Click" />
                            <asp:Button ID="btnPrint" CssClass="btn btn-primary" runat="server" Text="Print" OnClick="btnPrint_Click" OnClientClick="SetTarget();" />
                            <a href="../../Home/Index" target="_self" class="btn btn-primary">Back</a>
                        </td>
                    </tr>
                </table>
                <hr />
            </ContentTemplate>
        </asp:UpdatePanel>
        <br />
        <rsweb:ReportViewer ID="ReportViewer1" runat="server" Height="1000px" Width="100%"></rsweb:ReportViewer>
    </form>
</body>
</html>
