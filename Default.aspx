<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" Inherits="_Default" Codebehind="Default.aspx.cs" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent"></asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <h2>Nominal Defendant</h2>
    <p>
        Welcome to the Nominal Defendant Database.
    </p>
    
    <h1><strong><asp:Label ID="tmpLabel" runat="server"></asp:Label></strong></h1>

    <p>
    <ul>
        <li><a href='AddUpdateND.aspx'>Add a new Nominal Defendant Claim</a></li>
        <li><a href='Search.aspx'>Search and update Nominal Defendant Claims</a></li>
        <li><a href='Reports.aspx'>Reports</a></li>
    </ul>
    <asp:PlaceHolder ID="PlaceHolder1" runat="server"></asp:PlaceHolder>
    <p>&nbsp;</p>

</asp:Content>
