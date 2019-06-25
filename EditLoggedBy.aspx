<%@ Page Language="C#" AutoEventWireup="True" Inherits="EditLoggedBy" Codebehind="EditLoggedBy.aspx.cs" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="Scripts/nominal.js" type="text/javascript"></script>
    <link href="Styles/Site.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
        <center>
            <br />
            <table cellpadding="5">
                <tr>
                    <td align="right" style="color:White; font-weight:bold">Company</td>
                    <td align="left"><asp:TextBox ID="CompanyEdit" runat="server" CssClass="textboxEntry" MaxLength="50"></asp:TextBox></td>
                </tr>
                <tr>
                    <td align="right" style="color:White; font-weight:bold">Title</td>
                    <td align="left"><asp:TextBox ID="TitleEdit" runat="server" MaxLength="10" CssClass="textboxEntry"></asp:TextBox></td>
                </tr>
                <tr>
                    <td align="right" style="color:White; font-weight:bold">First Name</td>
                    <td align="left"><asp:TextBox ID="FirstNameEdit" runat="server" MaxLength="50" CssClass="textboxEntry"></asp:TextBox></td>
                </tr>
                <tr>
                    <td align="right" style="color:White; font-weight:bold">Last Name</td>
                    <td align="left"><asp:TextBox ID="LastNameEdit" runat="server" MaxLength="50" CssClass="textboxEntry"></asp:TextBox></td>
                </tr>
                <tr>
                    <td align="right" style="color:White; font-weight:bold">Address</td>
                    <td align="left"><asp:TextBox ID="AddressEdit" runat="server" MaxLength="255" Height="50px" TextMode="MultiLine" CssClass="textboxMultiLine"></asp:TextBox></td>
                </tr>
                <tr>
                    <td align="right" style="color:White; font-weight:bold">Suburb</td>
                    <td align="left"><asp:TextBox ID="SuburbEdit" runat="server" MaxLength="150" CssClass="textboxEntry"></asp:TextBox></td>
                </tr>
                <tr>
                    <td align="right" style="color:White; font-weight:bold">Phone Number</td>
                    <td align="left"><asp:TextBox ID="PhoneEdit" runat="server" MaxLength="30" CssClass="textboxEntry"></asp:TextBox></td>
                </tr>
                <tr>
                    <td align="right" style="color:White; font-weight:bold">Fax Number</td>
                    <td align="left"><asp:TextBox ID="FaxEdit" runat="server" MaxLength="30" CssClass="textboxEntry"></asp:TextBox></td>
                </tr>
                <tr>
                    <td align="right" style="color:White; font-weight:bold">Type</td>
                    <td align="left">
                        <asp:DropDownList ID="TypeList" runat="server" DataSourceID="LodgerType" 
                            CssClass="dropDownCSSInsurer" DataTextField="LodgerType" DataValueField="LodgerType">
                        </asp:DropDownList>
                        <asp:SqlDataSource ID="LodgerType" runat="server" 
                            ConnectionString="<%$ ConnectionStrings:NominalConn %>" 
                            SelectCommand="SELECT * FROM [LodgerType] ORDER BY LodgerType ASC"></asp:SqlDataSource>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" align="right">
                        <asp:Button ID="CancelButton" runat="server" Text="Cancel" CssClass="buttonCSS" onclick="CancelEdit_Click" />
                            &nbsp;
                        <asp:Button ID="LodgerButtonEdit" runat="server" Text="Save and Populate" CssClass="buttonCSS" onclick="LodgerButtonEdit_Click" />

                    </td>
                </tr>
            </table>
            
        </center>
        <asp:PlaceHolder ID="EPH" runat="server"></asp:PlaceHolder>
    </form>
</body>
</html>
