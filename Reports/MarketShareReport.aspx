<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="True" Inherits="MarketShareReport" Codebehind="MarketShareReport.aspx.cs" %>

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
            Market Share Date Change: 
            <asp:DropDownList ID="DateDropDown" runat="server" CssClass="dropDownCSS" Width="250"
                    DataSourceID="DateDataSource" DataTextField="AuditDate" DataValueField="ID">
           </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td colspan="2" align="right">
                <br /><asp:Button ID="ButtonSearch" runat="server" Text="Display Market Share" 
                    onclick="ButtonSearch_Click" CssClass="buttonCSS" />
            </td>
        </tr>
    </table>

    <asp:SqlDataSource ID="DateDataSource" runat="server" 
        ConnectionString="<%$ ConnectionStrings:NominalConn %>" 
        SelectCommand="select * from [InsurersMarketShareAudit] Order by AuditDate Desc"></asp:SqlDataSource>

        <br />
        <asp:PlaceHolder ID="MarketSharePlaceholder" runat="server" Visible="true"></asp:PlaceHolder>
        <asp:Label ID="OutputLabel" runat="server"></asp:Label>
</asp:Content>

