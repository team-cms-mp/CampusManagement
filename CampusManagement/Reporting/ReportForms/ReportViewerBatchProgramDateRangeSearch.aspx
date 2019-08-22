<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ReportViewerBatchProgramDateRangeSearch.aspx.cs" Inherits="CampusManagement.Reporting.ReportForms.ReportViewerBatchProgramDateRangeSearch" %>

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
        <div class="row>">
            <div class="col-md-12">
                <h3>
                    <label id="lblReportTitle" runat="server"></label>
                </h3>
                <input type="hidden" id="hdnReportName" class="form-control" runat="server" />
                <input type="hidden" id="hdnEDate" class="form-control" runat="server" />
                <input type="hidden" id="hdnSDate" class="form-control" runat="server" />
            </div>
        </div>
        <hr />
        <table style="width: 100%;">
            <tr>
                <td style="text-align: right;">
                    <label class="control-label">Text Search</label>
                </td>
                <td>
                    <input id="txtSearch" class="form-control" runat="server" />
                </td>
                <td style="text-align: right;">
                    <label class="control-label">Department</label>
                </td>
                <td>
                    <select id="cmbSubDepartment" class="form-control" runat="server"></select>
                </td>
                <td style="text-align: right;">
                    <label class="control-label">Session</label>
                </td>
                <td>
                    <select id="cmbSessions" class="form-control" runat="server"></select>
                </td>
                <td style="text-align: right;">
                    <label class="control-label">Program</label>
                </td>
                <td>
                    <select id="cmbBatchPrograms" class="form-control" runat="server"></select>
                </td>
            </tr>
            <tr>
                <td style="text-align: right;">
                    <label class="control-label">Start Date</label>
                </td>
                <td>
                    <input id="txtStartDate" class="form-control" runat="server" type="date" />
                </td>
                <td style="text-align: right;">
                    <label class="control-label">End Date</label>
                </td>
                <td>
                    <input id="txtEndDate" class="form-control" runat="server" type="date" />
                </td>
            </tr>
            <tr>
                <td></td>
                <td colspan="7">
                    <asp:Button ID="btnSearch" CssClass="btn btn-primary" runat="server" Text="Search" OnClick="btnSearch_Click" />
                    <asp:Button ID="btnPrint" CssClass="btn btn-primary" runat="server" Text="Print" OnClick="btnPrint_Click" OnClientClick="SetTarget();" />
                    <a href="../../Home/Index" target="_self" class="btn btn-primary">Back</a>
                </td>
            </tr>
        </table>
        <hr />
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <rsweb:ReportViewer ID="ReportViewer1" runat="server" Height="1000px" Width="100%"></rsweb:ReportViewer>
    </form>
</body>
</html>

<script type="text/javascript">

    Date.prototype.toDateInputValue = (function () {
        var local = new Date(this);
        local.setMinutes(this.getMinutes() - this.getTimezoneOffset());
        return local.toJSON().slice(0, 10);
    });

    document.getElementById("txtStartDate").value = new Date().toDateInputValue();
    document.getElementById("txtEndDate").value = new Date().toDateInputValue();

    if (document.getElementById("hdnSDate").value != "") {
        document.getElementById("txtStartDate").value = new Date(document.getElementById("hdnSDate").value).toDateInputValue();
        document.getElementById("txtEndDate").value = new Date(document.getElementById("hdnEDate").value).toDateInputValue();
    }

</script>
