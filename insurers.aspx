<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" Inherits="insurers" Codebehind="insurers.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <script src="Scripts/nominal.js" type="text/javascript"></script>
    <h2>Nominal Defendant Insurers</h2>
    <asp:Label ID="tmpLabelError" runat="server" />
    <asp:PlaceHolder ID="ErrPH" runat="server"></asp:PlaceHolder>
    <table>
    <tr valign="top">
        <td>
            <asp:PlaceHolder ID="InsurersPlaceholder" runat="server"></asp:PlaceHolder>
        </td>
        <asp:HiddenField ID="HiddenTotal" runat="server" Value="100" />
        <td align="left" style="padding-left:30px; padding-top:50px">    
            <h1>Total Market Share - <span id="TotalMarketShare"><%=HiddenTotal.Value %>%</span></h1>
            <asp:PlaceHolder ID="TotalSharePlaceholder" runat="server" Visible="false"> 
                <h2 style="color:Maroon; font-weight:bold">Total Market share must be between 99 and 101%<br />or changes will not be saved</h2>
            </asp:PlaceHolder>
        </td>
    </tr>
    </table>
    
    <br /><br />
    <asp:Button ID="UpdateShareButton" runat="server" Text="Update Market Share" 
        CssClass="buttonCSS" onclick="UpdateShareButton_Click" />
    <asp:Button ID="CancelUpdateButton" runat="server" Text="Cancel Update" 
        CssClass="buttonCSS" Visible="false" onclick="CancelUpdateButton_Click" />&nbsp;
    <asp:Button ID="SaveShareButton" runat="server" Text="Save Changes" 
        CssClass="buttonCSS" Visible="false" onclick="SaveShareButton_Click" />

</asp:Content>

