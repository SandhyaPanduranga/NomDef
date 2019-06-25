<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" Inherits="Admin" Codebehind="Admin.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">

Admin Home Page


<ul>
    <li><a href="Insurers.aspx">Insurer Admin</a></li>
    <li><a href="UserAdmin.aspx">User Admin</a></li>
    <li><a href="ClaimAdministration.aspx">Claim Administration</a></li>
</ul>

Table Administration

<ul>
    <li><a href="adminUpdates.aspx?type=Anomalies">Anomalies</a></li>
    <li><a href="adminUpdates.aspx?type=ClaimAgainst">Claim Against</a></li>
    <li><a href="adminUpdates.aspx?type=ClaimStatus">Claim Status</a></li>
    <li><a href="adminUpdates.aspx?type=ClaimType">Claim Type</a></li>
    <li><a href="adminUpdates.aspx?type=CloseType">Close Type</a></li>
    <li><a href="adminUpdates.aspx?type=RejectionReason">Rejection Reason</a></li>
    <li><a href="adminUpdates.aspx?type=RoleInAccident">Role in Accident</a></li>
</ul>

</asp:Content>

