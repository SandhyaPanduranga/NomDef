<%@ Page Language="C#" AutoEventWireup="True" Inherits="Locations" Codebehind="Locations.aspx.cs" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="Scripts/nominal.js" type="text/javascript"></script>
    <link href="Styles/Site.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="formLocations" runat="server">
    <div>
        <center>
        <br />
        <table>
            <tr>
                <td colspan="2">
                    <table cellpadding="0" cellspacing="0">
                        <tr valign="middle">
                            <td><img src="Images/fancy_title_left.png" alt="" /></td>
                            <td style="background-image:url(Images/fancy_title_main.png); background-repeat:repeat-x; color:White; font-weight:bold; padding-bottom:6px; width:200px;">Accident Location Search</td>
                            <td><img src="Images/fancy_title_right.png" alt="" /></td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td align="right" style="color:White; font-weight:bold">Suburb</td>
                <td>
                    <asp:TextBox ID="SuburbInput" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td align="right" style="color:White; font-weight:bold">Postcode</td>
                <td>
                    <asp:TextBox ID="PostcodeInput" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td colspan="2" align="right">
                    <asp:Button ID="StartSearchbutton" runat="server" Text="Find" onclick="StartSearchbutton_Click" />
                </td>
            </tr>
            <tr>
                <td colspan="2" align="center">
                    <asp:PlaceHolder ID="LocationResultsPlaceholder" runat="server"></asp:PlaceHolder>
                </td>
            </tr>
        </table>
        </center>
    </div>
    </form>
</body>
</html>
