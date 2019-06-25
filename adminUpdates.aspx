<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" Inherits="adminUpdates" Codebehind="adminUpdates.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
<script src="Scripts/nominal.js" type="text/javascript"></script>
<h2><asp:Label ID="PageTitle" runat="server"></asp:Label></h2>

<br />
<asp:PlaceHolder ID="AdminPlaceholder" runat="server" Visible="true">


</asp:PlaceHolder>
</asp:Content>

