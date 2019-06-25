<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="True" Inherits="InterstateClaims" Codebehind="InterstateClaims.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
<script src="/Scripts/nominal.js" type="text/javascript"></script>
    <script src="/Scripts/ts_picker.js" type="text/javascript"></script>
<h2>Reporting</h2>
<asp:Label ID="tmpLabel" runat="server"></asp:Label>
    <table>
        <tr>
            <td>
            From: <asp:TextBox ID="AccidentFromDate" runat="server" CssClass="textboxEntryShort"></asp:TextBox>
            <span style="vertical-align:bottom" class="textboxEntryShort"><a style="cursor:pointer" onclick="show_calendar('document.forms[0].ctl00$MainContent$AccidentFromDate', '');"><img src="/Images/dates.gif" alt="Select Date" id="Img12" /></a></span>
            </td>
            <td>To: <asp:TextBox ID="AccidentToDate" runat="server" CssClass="textboxEntryShort"></asp:TextBox>
            <span style="vertical-align:bottom" class="textboxEntryShort"><a style="cursor:pointer" onclick="show_calendar('document.forms[0].ctl00$MainContent$AccidentToDate', '');"><img src="/Images/dates.gif" alt="Select Date" id="Img13" /></a></span> 
            </td>
         </tr>
        <tr>
            <td colspan="2" align="right">
                <br /><asp:Button ID="ButtonSearch" runat="server" Text="Search" 
                    onclick="ButtonSearch_Click" CssClass="buttonCSS" />
            </td>
        </tr>
    </table>

    <asp:SqlDataSource ID="TestDataSource" runat="server" 
        ConnectionString="<%$ ConnectionStrings:NominalConn %>" 
        SelectCommand="SELECT * FROM [NominalDefendants]">
    </asp:SqlDataSource>
     <br />
        <asp:PlaceHolder ID="TestPlaceholder" runat="server" Visible="true"></asp:PlaceHolder>

</asp:Content>

