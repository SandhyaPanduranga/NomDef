<%@ Page Language="C#" AutoEventWireup="true" Inherits="LoggedBy" Codebehind="LoggedBy.aspx.cs" %>

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
        <table>
              <tr>
                <td colspan="2">
                    <table cellpadding="0" cellspacing="0">
                        <tr valign="middle">
                            <td><img src="Images/fancy_title_left.png" alt="" /></td>
                            <td style="background-image:url(Images/fancy_title_main.png); background-repeat:repeat-x; color:White; font-weight:bold; padding-bottom:6px; width:200px;">Claim Lodger Search</td>
                            <td><img src="Images/fancy_title_right.png" alt="" /></td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td align="right" style="color:White; font-weight:bold">Company</td>
                <td>
                    <asp:TextBox ID="CompanyInput" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td align="right" style="color:White; font-weight:bold">Firstname</td>
                <td>
                    <asp:TextBox ID="FirstNameInput" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td align="right" style="color:White; font-weight:bold">Lastname</td>
                <td>
                    <asp:TextBox ID="LastnameInput" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td align="right" style="color:White; font-weight:bold">Address</td>
                <td>
                    <asp:TextBox ID="AddressInput" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td align="right" style="color:White; font-weight:bold">Type</td>
                <td>
                     <asp:DropDownList ID="LodgerTypeList" runat="server" 
                        DataSourceID="LodgerTypeDataSource" DataTextField="LodgerType" 
                        DataValueField="LodgerType" CssClass="dropDownCSSMedium">
                    </asp:DropDownList>
                    <asp:SqlDataSource ID="LodgerTypeDataSource" runat="server" 
                        ConnectionString="<%$ ConnectionStrings:NominalConn %>" 
                        SelectCommand="select * from [LodgerType]"></asp:SqlDataSource>
                </td>
            </tr>
            <tr>
                <td colspan="2" align="right">
                    <asp:Button ID="StartSearchbutton" runat="server" Text="Find" onclick="StartSearchbutton_Click" />
                </td>
            </tr>
        </table>
        <asp:PlaceHolder ID="LodgersResultsPlaceholder" runat="server"></asp:PlaceHolder>
        
        <div id="addLodgerForm" style="display:none">
            <table cellpadding="5">
                <tr>
                    <td align="right" style="color:White; font-weight:bold">Company</td>
                    <td align="left"><asp:TextBox ID="CompanyAdd" runat="server" CssClass="textboxEntry" MaxLength="50"></asp:TextBox></td>
                </tr>
                <tr>
                    <td align="right" style="color:White; font-weight:bold">Title</td>
                    <td align="left"><asp:TextBox ID="TitleAdd" runat="server" MaxLength="10" CssClass="textboxEntry"></asp:TextBox></td>
                </tr>
                <tr>
                    <td align="right" style="color:White; font-weight:bold">First Name</td>
                    <td align="left"><asp:TextBox ID="FirstNameAdd" runat="server" MaxLength="50" CssClass="textboxEntry"></asp:TextBox></td>
                </tr>
                <tr>
                    <td align="right" style="color:White; font-weight:bold">Last Name</td>
                    <td align="left"><asp:TextBox ID="LastNameAdd" runat="server" MaxLength="50" CssClass="textboxEntry"></asp:TextBox></td>
                </tr>
                <tr>
                    <td align="right" style="color:White; font-weight:bold">Address</td>
                    <td align="left"><asp:TextBox ID="AddressAdd" runat="server" MaxLength="255" Height="50px" TextMode="MultiLine" CssClass="textboxMultiLine"></asp:TextBox></td>
                </tr>
                <tr>
                    <td align="right" style="color:White; font-weight:bold">Suburb</td>
                    <td align="left"><asp:TextBox ID="SuburbAdd" runat="server" MaxLength="100" CssClass="textboxEntry"></asp:TextBox></td>
                </tr>
                <tr>
                    <td align="right" style="color:White; font-weight:bold">Phone Number</td>
                    <td align="left"><asp:TextBox ID="PhoneAdd" runat="server" MaxLength="30" CssClass="textboxEntry"></asp:TextBox></td>
                </tr>
                <tr>
                    <td align="right" style="color:White; font-weight:bold">Fax Number</td>
                    <td align="left"><asp:TextBox ID="FaxAdd" runat="server" MaxLength="30" CssClass="textboxEntry"></asp:TextBox></td>
                </tr>
                <tr>
                    <td align="right" style="color:White; font-weight:bold">Type</td>
                    <td align="left">
                        <asp:DropDownList ID="TypeList" runat="server" DataSourceID="LodgerTypeDataSource" 
                            CssClass="dropDownCSSInsurer" DataTextField="LodgerType" DataValueField="LodgerType">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" align="right">
                        <asp:Button ID="AddLodgerButton" runat="server" Text="Add Lodger" 
                            CssClass="buttonCSS" onclick="AddLodgerButton_Click" />
                    </td>
                </tr>
            </table>
        </div>
            

        </center>
        <asp:PlaceHolder ID="EPH" runat="server"></asp:PlaceHolder>
    </form>
</body>
</html>
