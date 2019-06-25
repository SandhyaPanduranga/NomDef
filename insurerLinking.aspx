<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" Inherits="insurerLinking" Codebehind="insurerLinking.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <script src="Scripts/nominal.js" type="text/javascript"></script>
    <h2>Nominal Defendant Insurer Linking</h2>
    <asp:PlaceHolder ID="ErrPH" runat="server"></asp:PlaceHolder>
    <table>
    <tr valign="top">
        <td>
            <asp:Label ID="InsurerNameLabel" runat="server" Text="Label"></asp:Label>
            <asp:PlaceHolder ID="InsurerLinkingPlaceholder" runat="server"></asp:PlaceHolder>
        </td>
    </tr>
    </table>
    
    <br /><br />
    <asp:Button ID="CancelLinkingButton" runat="server" Text="Cancel" 
        CssClass="buttonCSS" Visible="True" onclick="CancelUpdateButton_Click" />&nbsp;
    <asp:Button ID="SaveLinkingButton" runat="server" Text="Save Changes" 
        CssClass="buttonCSS" Visible="True" onclick="SaveLinkingButton_Click" />

</asp:Content>

