<%@ Page Title="Add and Update Page" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" Inherits="AddUpdateND" ValidateRequest="false" Codebehind="AddUpdateND.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
<asp:PlaceHolder ID="PageDisplay" runat="server">
    <script src="Scripts/nominal.js" type="text/javascript"></script>
    <script src="Scripts/ts_picker.js" type="text/javascript"></script>

    <asp:HiddenField ID="NDID" runat="server" />
    <asp:HiddenField ID="CurrentStepVal" runat="server" />
    <asp:HiddenField ID="ClaimNumberVal" runat="server" />
    
    <asp:Label ID="tmpLabel" runat="server"></asp:Label>
    <asp:PlaceHolder ID="mySQLPH" runat="server"></asp:PlaceHolder>
    <table width="900px">
        <tr>
            <td width="50%" align="left"><h2><asp:Label ID="ScreenTitle" runat="server"></asp:Label></h2></td>
            <td width="40%" nowrap="nowrap">Screen <asp:Label ID="PageNumLabel" runat="server" Text=""></asp:Label> of 5</td>
            <td nowrap="nowrap"><strong>Claim Number</strong></td>
            <td nowrap="nowrap"><asp:TextBox ID="ClaimNumberInput" runat="server" CssClass="textboxEntryVeryShort" style="border:0px;"></asp:TextBox></td>
        </tr>
    </table>
    <br />
        <table width="900px">
        <tr>
            <td width="20%" align="left"><asp:Button ID="Screen1Button" runat="server" Text="CLAIM DETAILS" CssClass="buttonCSS" onclick="ShowScreenClick" UseSubmitBehavior="False" /></td>
            <td width="20%" align="center"><asp:Button ID="Screen2Button" runat="server" Text="ACCIDENT/VEHICLES" CssClass="buttonCSS" onclick="ShowScreenClick" UseSubmitBehavior="False" /></td>
            <td width="20%" align="center"><asp:Button ID="Screen3Button" runat="server" Text="ALLOCATION" CssClass="buttonCSS" onclick="ShowScreenClick" UseSubmitBehavior="False" /></td>
            <td width="20%" align="center"><asp:Button ID="Screen4Button" runat="server" Text="DOCO/CORRO" CssClass="buttonCSS" onclick="ShowScreenClick" UseSubmitBehavior="False" /></td>
            <td width="20%" align="right"><asp:Button ID="Screen5Button" runat="server" Text="LITIGATION/STATUS" CssClass="buttonCSS" onclick="ShowScreenClick" UseSubmitBehavior="False" /></td>
        </tr>
        </table>

        <asp:PlaceHolder ID="Screen1" runat="server">
        <div style="min-height:460px">
        <table cellpadding="8" cellspacing="3" width="900px" style="min-height:450px">
            <tr>
                <td colspan="2" style="font-weight:bold" class="tableTitle">Claimant Details</td>
                <td colspan="2" style="font-weight:bold" class="tableTitle">Lodger Details</td>
            </tr>
            <tr>
                <td width="15%" class="tableTitle">First Name <span class="manadtoryText">*</span></td>
                <td class="tableField" width="35%"><asp:TextBox ID="FirstNameInput" runat="server" CssClass="textboxEntryShort" TabIndex="1"></asp:TextBox></td>
                <td class="tableTitle" rowspan="3">Lodged By</td>
                <td valign="top" class="tableField" rowspan="3">
                    <asp:HiddenField ID="LoggedById" runat="server" Value="" />
                    <span><asp:TextBox ID="LoggedByInput" runat="server" Height="60px" TextMode="MultiLine" CssClass="textboxMultiLine"></asp:TextBox>&nbsp;</span>
                    <br /><br />
                    <span><input type="button" class="buttonCSS" id="ButtonLoggedBy" value="Search" onclick="OpenWindowLoggedBy('LoggedBy.aspx')" tabindex="5" UseSubmitBehavior="False" /></span>
                </td>
            </tr>
            <tr>
                <td class="tableTitle">Last Name <span class="manadtoryText">*</span></td>
                <td class="tableField"><asp:TextBox ID="LastNameInput" runat="server" CssClass="textboxEntryShort" TabIndex="2"></asp:TextBox></td>
            </tr>
            <tr>
                <td class="tableTitle">Date of Birth <span class="manadtoryText">*</span></td>
                <td class="tableField">
                    <span>
                        <asp:TextBox ID="DateOfBirthInput" runat="server" CssClass="textboxEntryShort" OnTextChanged="checkValidDateFormatLong" labelParam="DateOfBirthLabel" TabIndex="3"></asp:TextBox></span>
                        <span style="vertical-align:bottom" class="textboxEntryShort"><a style="cursor:pointer" onclick="show_calendar('document.forms[0].ctl00$MainContent$DateOfBirthInput', '');"><img src="Images/dates.gif" alt="Select Date" id="Img5" /></a></span><asp:Label ID="DateOfBirthLabel" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="tableTitle">Date Received <span class="manadtoryText">*</span></td>
                <td class="tableField">
                    <span>
                        <asp:TextBox ID="DateReceivedInput" runat="server" CssClass="textboxEntryShort" OnTextChanged="checkValidDateFormatLong" labelParam="ReceivedLabel" TabIndex="4"></asp:TextBox></span>
                        <span style="vertical-align:bottom" class="textboxEntryShort"><a style="cursor:pointer" onclick="show_calendar('document.forms[0].ctl00$MainContent$DateReceivedInput', '');"><img src="Images/dates.gif" alt="Select Date" id="DateReceivedButton" /></a></span><asp:Label ID="ReceivedLabel" runat="server"></asp:Label>
                </td>
                <td class="tableTitle">Lodgement Ref Number</td>
                <td class="tableField"><asp:TextBox ID="LodgersRef" runat="server" CssClass="textboxEntryShort" TabIndex="6" MaxLength="50"></asp:TextBox></td>
            </tr>
            <tr>
                <td colspan="4">&nbsp;</td>
            </tr>
            <tr>
                <td colspan="4" style="font-weight:bold" class="tableTitle">Claim Details</td>
            </tr>
            <tr>
                <td class="tableTitle" rowspan="2">Claim Type <span class="manadtoryText">*</span></td>
                <td class="tableField" rowspan="2">
                    <asp:PlaceHolder ID="ClaimTypes" runat="server"></asp:PlaceHolder>
                </td>
                <td class="tableTitle">Related Claim Ref Number(s)</td>
                <td class="tableField">
                    <asp:TextBox ID="RelatedRefNum" runat="server" CssClass="textboxEntryShort" TabIndex="10" MaxLength="100"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="tableTitle">Claim Against</td>
                <td class="tableField">
                    <asp:DropDownList ID="ClaimAgainstInput" runat="server" 
                        DataSourceID="ClaimAgainstDataSource" DataTextField="ClaimAgainst" 
                        DataValueField="Id" CssClass="dropDownCSSMedium" TabIndex="11">
                    </asp:DropDownList>
                    <asp:SqlDataSource ID="ClaimAgainstDataSource" runat="server" 
                        ConnectionString="<%$ ConnectionStrings:NominalConn %>" 
                        SelectCommand="SELECT * FROM [ClaimAgainst]"></asp:SqlDataSource>
                </td>
            </tr>
            <tr>
                <td class="tableTitle">Claims Officer <span class="manadtoryText">*</span></td>
                <td class="tableField">
                    
                    <asp:DropDownList ID="ClaimsOfficer" runat="server" CssClass="dropDownCSSMedium"
                    DataSourceID="ClaimsOfficerDataSource" DataTextField="Name" DataValueField="Id" TabIndex="8">
                    </asp:DropDownList>

                    <asp:SqlDataSource ID="ClaimsOfficerDataSource" runat="server" 
                        ConnectionString="<%$ ConnectionStrings:NominalConn %>" 
                        SelectCommand="SELECT * FROM [Users]"></asp:SqlDataSource>
                </td>
                <td class="tableTitle">Role in Accident <span class="manadtoryText">*</span></td>
                <td class="tableField">
                    <asp:DropDownList ID="RoleInAccident" runat="server" CssClass="dropDownCSSMedium"
                    DataSourceID="RoleInAccidentDatasource" DataTextField="RoleInAccident" DataValueField="Id" TabIndex="13">
                    </asp:DropDownList>

                    <asp:SqlDataSource ID="RoleInAccidentDatasource" runat="server" 
                        ConnectionString="<%$ ConnectionStrings:NominalConn %>" 
                        SelectCommand="SELECT * FROM [RoleInAccident] ORDER BY ID ASC"></asp:SqlDataSource>
                </td>
            </tr>
            <tr valign="top">
                <td class="tableTitle">Anomaly</td>
                <td class="tableField">
                    <asp:DropDownList ID="AccidentAnomaly" runat="server" CssClass="dropDownCSSMedium"
                    DataSourceID="AnomalyDatasource" DataTextField="Anomaly" DataValueField="Id" OnSelectedIndexChanged="ChangeAnomoly" AutoPostBack="true" TabIndex="9">
                    </asp:DropDownList>

                    <asp:SqlDataSource ID="AnomalyDatasource" runat="server" 
                        ConnectionString="<%$ ConnectionStrings:NominalConn %>" 
                        SelectCommand="SELECT * FROM [Anomalies] ORDER BY Id ASC"></asp:SqlDataSource>
                        
                        <br />
                        <asp:TextBox ID="AnomalyOther" runat="server" CssClass="textboxEntryShort" Enabled="false"></asp:TextBox>
                </td>
                <td class="tableTitle">Claim Rejected</td>
                <td class="tableField">
                    <asp:CheckBox ID="RejectedCheckbox" runat="server" OnCheckedChanged="SetRejected" AutoPostBack="true" />
                    &nbsp;<span><asp:TextBox ID="DateRejected" runat="server" OnTextChanged="checkValidDateFormat" labelParam="DateRejectedLabel" CssClass="textboxEntryShort" Enabled="false"></asp:TextBox></span>
                    <span style="vertical-align:bottom" class="textboxEntryShort"><a style="cursor:pointer" onclick="show_calendar('document.forms[0].ctl00$MainContent$DateRejected', '');"><img src="Images/dates.gif" alt="Select Date" id="Img8" /></a> </span><span style="vertical-align:middle;">(dd/mm/yyyy)</span><asp:Label ID="DateRejectedLabel" runat="server"></asp:Label>
                    <br /><br />
                    <asp:DropDownList ID="RejectedReason" runat="server" CssClass="dropDownCSSMedium"
                    DataSourceID="RejectedReasonDatasource" DataTextField="RejectionReason" DataValueField="Id" Enabled="false">
                    </asp:DropDownList>
                    
                    <asp:SqlDataSource ID="RejectedReasonDatasource" runat="server" 
                        ConnectionString="<%$ ConnectionStrings:NominalConn %>" 
                        SelectCommand="SELECT * FROM [RejectionReasons]"></asp:SqlDataSource>

                </td>
            </tr>
        </table>
        </div>
        </asp:PlaceHolder>

        <asp:PlaceHolder ID="Screen2" runat="server">
        <div style="">
        <table cellpadding="8" cellspacing="3" width="900px" style="min-height:450px">
            <tr>
                <td colspan="2" style="font-weight:bold" class="tableTitle">Accident Details</td>
            </tr>
            <tr>
                <td class="tableTitle">Accident Date <span class="manadtoryText">*</span></td>
                <td class="tableField">
                    <span><asp:TextBox ID="AccidentDateInput" runat="server" CssClass="textboxEntryShort" OnTextChanged="checkValidDateFormat" labelParam="AccDateLabel"></asp:TextBox></span>
                    <span style="vertical-align:bottom" class="textboxEntryShort"><a style="cursor:pointer" onclick="show_calendar('document.forms[0].ctl00$MainContent$AccidentDateInput', '');"><img src="Images/dates.gif" alt="Select Date" id="AccidentDateButton" /></a> </span><span style="vertical-align:middle;"></span><asp:Label ID="AccDateLabel" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="tableTitle">Late Claim</td>
                <td class="tableField">
                    <span>
                        <asp:CheckBox ID="LateClaim" runat="server" Enabled="true" />
                        <asp:Label ID="LateClaimLabel" runat="server" Text="" CssClass="textboxEntryNP"></asp:Label>
                    </span>
                </td>
            </tr>
            <tr valign="top">
                <td class="tableTitle">Accident Location</td>
                <td class="tableField">
                    <asp:HiddenField ID="LocationId" runat="server" Value="" />
                    <asp:TextBox ID="LocationInput" runat="server" CssClass="textboxEntry"></asp:TextBox>&nbsp;
                    <input type="button" class="buttonCSS" id="ButtonLocation" value="Location search" onclick="OpenWindow('Locations.aspx')" UseSubmitBehavior="False" />
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td colspan="2" style="font-weight:bold" class="tableTitle">Vehicles Involved &nbsp;&nbsp;<asp:Button ID="AddVehicleShowHide" runat="server" Text="Add Vehicle" onclick="AddVehicleShowHide_Click" CssClass="buttonCSS" UseSubmitBehavior="False" /><asp:Label ID="VehicleErrorSpan" runat="server" Text=""></asp:Label></td>
            </tr>
            <tr valign="top">
                <td colspan="2">
                    <asp:PlaceHolder ID="VehiclesPlaceholder" runat="server"></asp:PlaceHolder>
                    <asp:PlaceHolder ID="CarAddForm" runat="server" Visible="false">
                    <div id="VehiclesAddForm" style="padding-top:10px; display:block">
                        <table cellpadding="6" cellspacing="2">
                            <tr>
                                <td class="tableTitle"><strong>Vehicle Registration No</strong></td>
                                <td class="tableTitle"><strong>At Fault</strong></td>
                                <td class="tableTitle"><strong>Rego State</strong></td>
                                <td class="tableTitle"><strong>Rego Expiry</strong></td>
                                <td class="tableTitle"><strong>Vehicle Class</strong></td>
                                <td class="tableTitle"><strong>Insurer</strong></td>
                                <td class="tableTitle">&nbsp;</td>
                            </tr>
                            <tr>
                                <td style="width:110px" class="tableField"><asp:TextBox ID="CarRegoNumber" runat="server" CssClass="textboxEntryVeryShort" MaxLength="10"></asp:TextBox></td>
                                
                                <td style="width:80px" nowrap="nowrap" class="tableField">
                                    <asp:RadioButton ID="CarAtFaultYes" runat="server" GroupName="AtFault"  />Yes<asp:RadioButton ID="CarAtFaultNo" runat="server" GroupName="AtFault" Checked="true" />No
                                </td>

                                <td style="width:100px" class="tableField">
                                    <asp:DropDownList ID="CarRegoState" runat="server"
                                        DataSourceID="AusStateDataSource" DataTextField="State"
                                        DataValueField="Id" CssClass="dropDownCSSShort"></asp:DropDownList>
                                    <asp:SqlDataSource ID="AusStateDataSource" runat="server" 
                                        ConnectionString="<%$ ConnectionStrings:NominalConn %>" 
                                        SelectCommand="SELECT * FROM [AustralianStates]"></asp:SqlDataSource>    
                                </td>
                                <td style="width:120px" nowrap="nowrap" class="tableField">
                                    <span><asp:TextBox ID="CarRegistrationExpiryInput" runat="server" CssClass="textboxEntryVeryShort"  labelParam="CarRegoExpiryLabel"></asp:TextBox></span>
                                    <span style="vertical-align:bottom" class="textboxEntryShort"><a style="cursor:pointer" onclick="show_calendar('document.forms[0].ctl00$MainContent$CarRegistrationExpiryInput', '');"><img src="Images/dates.gif" alt="Select Date" id="Img1" /></a></span>
                                    <br /><asp:Label ID="CarRegoExpiryLabel" runat="server"></asp:Label>
                                </td>
                                <td style="width:110px" class="tableField"><asp:TextBox ID="CarClass" runat="server" CssClass="textboxEntryVeryShort" MaxLength="10"></asp:TextBox></td>
                                <td style="width:120px" class="tableField">
                                    <asp:DropDownList ID="CarInsurer" runat="server"
                                        DataSourceID="InsurerDataSource" DataTextField="InsurerName"
                                        DataValueField="InsurerId" CssClass="dropDownCSSInsurer"></asp:DropDownList>
                                    <asp:SqlDataSource ID="InsurerDataSource" runat="server" 
                                        ConnectionString="<%$ ConnectionStrings:NominalConn %>" 
                                        SelectCommand="SELECT * FROM [Insurers]"></asp:SqlDataSource>
                                </td>
                                <td class="tableField">
                                    <asp:Button ID="CarAddButtonCancel" runat="server" Text="Cancel" CssClass="buttonCSSNarrow" 
                                        onclick="CarAddButtonCancel_Click" UseSubmitBehavior="False" />&nbsp;<asp:Button ID="CarAddButton" runat="server" Text="Save" CssClass="buttonCSSNarrow" 
                                        onclick="CarAddButton_Click" UseSubmitBehavior="False" />
                                </td>
                            </tr>
                        </table>
                    </div>
                    </asp:PlaceHolder>
                    <asp:PlaceHolder ID="vehiclesInvolved" runat="server" Visible="false"></asp:PlaceHolder>
                </td>
            </tr>
        </table>
        </div>
        </asp:PlaceHolder>

        <asp:PlaceHolder ID="Screen3" runat="server">
        <div style="">
        <table cellpadding="8" cellspacing="3" width="900px" style="min-height:450px">
            <tr>
                <td colspan="2" style="font-weight:bold" class="tableTitle">
                    <div style="float:left">Insurer Allocation&nbsp;&nbsp;<asp:Button ID="AllocateButton" OnClick="AllocateButton_Click" CssClass="buttonCSS" runat="server" Text="Allocate to Insurer" UseSubmitBehavior="False" />&nbsp;<asp:button id="reAllocateButton" runat="server" Text="Re-Allocate to Insurer" CssClass="buttonCSS" OnClientClick="return confirm('Are you sure you wish to re-allocate?');" OnClick="AllocateButton_Click" Visible="false" UseSubmitBehavior="True" /></div>
                    <div style="float:right"><asp:button id="unAllocateButton" runat="server" Text="Un-Allocate Claim" CssClass="buttonCSS" OnClientClick="return confirm('Are you sure you wish to un-allocate this claim?');" OnClick="UnAllocateButton_Click" Visible="true" UseSubmitBehavior="True" /></div>
                </td>
            </tr>
            <tr>
                <td width="25%" class="tableTitle">Insurer</td>
                <td class="tableField">
                    <asp:HiddenField ID="AllocatedInsurerId" runat="server" Value="" />
                    <asp:PlaceHolder ID="InsurerNamePlaceHolder" runat="server"></asp:PlaceHolder>

                    <asp:DropDownList ID="ManualAllocateInsurerDropDown" runat="server" CssClass="dropDownCSSInsurer"
                    DataSourceID="ManualAllocateInsurerDataSource" DataTextField="InsurerName" DataValueField="Id" Visible="false" >
                    </asp:DropDownList>
                    <asp:SqlDataSource ID="ManualAllocateInsurerDataSource" runat="server" 
                                        ConnectionString="<%$ ConnectionStrings:NominalConn %>" 
                                        SelectCommand="SELECT * FROM [Insurers] WHERE MarketShare > 0"></asp:SqlDataSource>
                    &nbsp;&nbsp;
                    <asp:Button ID="ManualAllocateButton" OnClick="AllocateButtonManual_Click" CssClass="buttonCSS" runat="server" Text="Manually Allocate" UseSubmitBehavior="False" />
                    <asp:Button ID="ManualAllocateSaveButton" OnClick="AllocateButtonManualSave_Click" CssClass="buttonCSS" runat="server" Text="Allocate" Visible="false" UseSubmitBehavior="False" />
                    <asp:Button ID="ManualAllocateCancelButton" OnClick="AllocateButtonManualCancel_Click" CssClass="buttonCSS" runat="server" Text="Cancel" Visible="false" UseSubmitBehavior="False" />
                </td>
            </tr>
            <tr>
                <td class="tableTitle">Date Allocated</td>
                <td class="tableField">
                    <span><asp:TextBox ID="AllocatedDateInput" runat="server" CssClass="textboxEntryShort"  OnTextChanged="checkValidDateFormat" labelParam="AllocatedLabel"></asp:TextBox></span>
                    <span style="vertical-align:bottom" class="textboxEntryShort"><a style="cursor:pointer" onclick="show_calendar('document.forms[0].ctl00$MainContent$AllocatedDateInput', '');"><img src="Images/dates.gif" alt="Select Date" id="ButtonAllocatedDate" /></a> </span><span style="vertical-align:middle;">(dd/mm/yyyy)</span><asp:Label ID="AllocatedLabel" runat="server"></asp:Label>
                </td>
            </tr>
            
            <tr>
                <td nowrap="nowrap" class="tableTitle">MAA Investigation Required</td>
                <td class="tableField">
                    Yes <asp:RadioButton ID="MAAInvestigationYes" runat="server" GroupName="MAAInvestigation" />&nbsp;&nbsp;
                    No <asp:RadioButton ID="MAAInvestigationNo" runat="server" GroupName="MAAInvestigation" />
                </td>
            </tr>
            <tr>
                <td class="tableTitle">Checked By</td>
                <td class="tableField">
                    
                    <asp:DropDownList ID="CheckByOfficer" runat="server" CssClass="dropDownCSSMedium"
                    DataSourceID="CheckbyDataSource" DataTextField="Name" DataValueField="Id" AppendDataBoundItems="true" OnSelectedIndexChanged="checkByChanged" AutoPostBack="true">
                    <asp:ListItem Text="Not Checked" Value="0" />
                    </asp:DropDownList>
                    <asp:HiddenField ID="HiddenCheckByOfficer" runat="server" />
                    <asp:SqlDataSource ID="CheckbyDataSource" runat="server" 
                        ConnectionString="<%$ ConnectionStrings:NominalConn %>" 
                        SelectCommand="SELECT * FROM [Users]"></asp:SqlDataSource>

                </td>
            </tr>
            <tr>
                <td class="tableTitle">Date Checked</td>
                <td class="tableField">
                    <span><asp:TextBox ID="dateChecked" runat="server" OnTextChanged="checkValidDateFormat" labelParam="dateCheckedLabel" CssClass="textboxEntryShort"></asp:TextBox></span>
                    <span style="vertical-align:bottom" class="textboxEntryShort"><a style="cursor:pointer" onclick="show_calendar('document.forms[0].ctl00$MainContent$dateChecked', '');"><img src="Images/dates.gif" alt="Select Date" id="Img7" /></a> </span><span style="vertical-align:middle;">(dd/mm/yyyy)</span><asp:Label ID="dateCheckedLabel" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td colspan="2">&nbsp;</td>
            </tr>
            

        </table>
        </div>
        </asp:PlaceHolder>

        <asp:PlaceHolder ID="Screen4" runat="server">
        <div style="">
        <table cellpadding="8" cellspacing="3" width="900px" style="min-height:450px">
            <tr>
                <td colspan="2" style="font-weight:bold" class="tableTitle">
                    <div style="float:left">Documents</div>
                    <div style="float:right"> 
                        <span><asp:TextBox ID="ClaimDocumentLoc" runat="server" CssClass="textboxEntryLong" MaxLength="150"></asp:TextBox></span>
                        <input type="button" class="buttonCSS" id="ButtonBrowse" value="BROWSE" onclick="this.form.ctl00$MainContent$ClaimDocumentLoc.value = getFolder()" tabindex="5" UseSubmitBehavior="False" />
                        <asp:PlaceHolder ID="DocViewPlaceholder" runat="server"></asp:PlaceHolder>
                    </div>
                </td>
            </tr>
            <tr valign="top">
                <td colspan="2" style="min-height:50px;">
                    <asp:Label ID="DocAttachmentsPlaceholder" runat="server"></asp:Label>
                </td>
            </tr>
        </table>
        </div>
        </asp:PlaceHolder>

        <asp:PlaceHolder ID="Screen5" runat="server">
        <div style="min-height:460px">
        <table cellpadding="8" cellspacing="3" width="900px" style="min-height:450px">
            <tr>
                <td colspan="2" style="font-weight:bold" class="tableTitle">
                    <div style="float:left">Litigation</div>
                </td>
            </tr>
            <tr>
                <td class="tableTitle">Subpoena Received</td>
                <td class="tableField">
                    <span><asp:TextBox ID="DateSubpoena" runat="server" OnTextChanged="checkValidDateFormat" labelParam="SubpoenaLabel" CssClass="textboxEntryShort"></asp:TextBox></span>
                    <span style="vertical-align:bottom" class="textboxEntryShort"><a style="cursor:pointer" onclick="show_calendar('document.forms[0].ctl00$MainContent$DateSubpoena', '');"><img src="Images/dates.gif" alt="Select Date" id="Img3" /></a> </span><span style="vertical-align:middle;">(dd/mm/yyyy)</span><asp:Label ID="SubpoenaLabel" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="tableTitle">Statement of Claim Received</td>
                <td class="tableField">
                    <span><asp:TextBox ID="DateStatmentClaim" runat="server" OnTextChanged="checkValidDateFormat" labelParam="StatementLabel" CssClass="textboxEntryShort"></asp:TextBox></span>
                    <span style="vertical-align:bottom" class="textboxEntryShort"><a style="cursor:pointer" onclick="show_calendar('document.forms[0].ctl00$MainContent$DateStatmentClaim', '');"><img src="Images/dates.gif" alt="Select Date" id="Img4" /></a> </span><span style="vertical-align:middle;">(dd/mm/yyyy)</span><asp:Label ID="StatementLabel" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td colspan="2" style="font-weight:bold" class="tableTitle">
                    <div style="float:left">Status</div>
                </td>
            </tr>
            <tr>
                <td width="25%" class="tableTitle">Date Closed</td>
                <td class="tableField">
                    <span><asp:TextBox ID="DateClosed" runat="server" OnTextChanged="checkValidDateFormat" labelParam="ClosedLabel" CssClass="textboxEntryShort"></asp:TextBox></span>
                    <span style="vertical-align:bottom" class="textboxEntryShort"><a style="cursor:pointer" onclick="show_calendar('document.forms[0].ctl00$MainContent$DateClosed', '');"><img src="Images/dates.gif" alt="Select Date" id="Img2" /></a></span><span style="vertical-align:middle;">(dd/mm/yyyy)</span><asp:Label ID="ClosedLabel" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td width="25%" class="tableTitle">Close Type</td>
                <td class="tableField">
                
                    <asp:DropDownList ID="CloseType" runat="server"
                        DataSourceID="CloseTypeDataSource" DataTextField="CloseType" OnSelectedIndexChanged="SetCloseType"
                        DataValueField="Id" CssClass="dropDownCSSMedium" AutoPostBack="true"></asp:DropDownList>
                    <asp:SqlDataSource ID="CloseTypeDataSource" runat="server" 
                        ConnectionString="<%$ ConnectionStrings:NominalConn %>" 
                        SelectCommand="SELECT * FROM [CloseType]"></asp:SqlDataSource>
                    <asp:HiddenField ID="HiddenCloseType" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="tableTitle">Claim Status <span class="manadtoryText">*</span></td>
                <td class="tableField">
                    <asp:DropDownList ID="ClaimStatus" runat="server"
                        DataSourceID="ClaimStatusDataSource" DataTextField="ClaimStatus" OnSelectedIndexChanged="SetClosed" 
                        DataValueField="Id" CssClass="dropDownCSSInsurer" TabIndex="12" AutoPostBack="true"></asp:DropDownList>
                    <asp:SqlDataSource ID="ClaimStatusDataSource" runat="server" 
                        ConnectionString="<%$ ConnectionStrings:NominalConn %>" 
                        SelectCommand="SELECT * FROM [ClaimStatus]"></asp:SqlDataSource>
                    <asp:HiddenField ID="HiddenClaimStatus" runat="server" />
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td colspan="2" style="font-weight:bold" class="tableTitle">
                    <div style="float:left">Payments</div>
                </td>
            </tr>
            <tr>
                <td width="25%" class="tableTitle">Insurer Reference</td>
                <td class="tableField"><asp:TextBox ID="Payment_IssuerRef" runat="server" CssClass="textboxEntryShort"></asp:TextBox></td>
            </tr>
            <tr>
                <td width="25%" class="tableTitle">Payments to Date</td>
                <td class="tableField"><asp:TextBox ID="Payment_PaymentsToDate" runat="server" CssClass="textboxEntryShort"></asp:TextBox></td>
            </tr>
            <tr>
                <td width="25%" class="tableTitle">Outstanding Estimate</td>
                <td class="tableField"><asp:TextBox ID="Payment_OutstandingEstimate" runat="server" CssClass="textboxEntryShort"></asp:TextBox></td>
            </tr>
        </table>
        </div>
        </asp:PlaceHolder>
        

        <table cellpadding="8" cellspacing="3" width="900px" >
            <tr>
                <td colspan="2" style="font-weight:bold" class="tableTitle">Comments / Actions</td>
            </tr>
            <tr valign="top">
                <td width="25%" class="tableTitle">Comment/Action Date:</td>
                <td class="tableField">
                    <span><asp:TextBox ID="CommentActionDate" runat="server" CssClass="textboxEntryShort"  OnTextChanged="checkValidDateFormat" labelParam="CommentActionLabel"></asp:TextBox></span>
                    <span style="vertical-align:bottom" class="textboxEntryShort"><a style="cursor:pointer" onclick="show_calendar('document.forms[0].ctl00$MainContent$CommentActionDate', '');"><img src="Images/dates.gif" alt="Select Date" id="Img6" /></a> </span><span style="vertical-align:middle;">(dd/mm/yyyy)</span><asp:Label ID="CommentActionLabel" runat="server"></asp:Label>
                </td>
            </tr>
            <tr valign="top">
                <td width="25%" class="tableTitle">Comments/Action:</td>
                <td class="tableField">
                    <asp:TextBox ID="CommentsActionsInput" runat="server" Height="60px" TextMode="MultiLine" MaxLength="500"></asp:TextBox><br />
                    <asp:Button ID="CommentsAddButton" runat="server" Text="Add Comment" OnClick="AddComment_Click" CssClass="buttonCSS" UseSubmitBehavior="False" />
                </td>
            </tr>
            <tr>
                <td colspan="2" style="line-height:5px;"></td>
            </tr>
            <tr>
                <td colspan="2" class="tableField">
                   <asp:PlaceHolder ID="CommentsPlaceHolder" runat="server"></asp:PlaceHolder>
                </td>
            </tr>
        </table>

        <table cellpadding="5" cellspacing="5" width="900px">
             <tr valign="top">
                <td colspan="2" align="left">
                    <asp:Button ID="ActionSheetButton" runat="server" Text="Print Action Sheet" CssClass="buttonCSS" onclick="ActionSheetButton_Click" UseSubmitBehavior="False" />
                </td>
                <td colspan="2" align="right">
                
                <asp:label ID="lblStatus" runat="server"></asp:label>
                    <asp:Button ID="SaveExitButton" runat="server" Text="Save and Exit" 
                        CssClass="buttonCSS" onclick="SaveExitButton_Click" UseSubmitBehavior="False" />
                        &nbsp;&nbsp;
                    <asp:Button ID="SaveButton" runat="server" Text="Save" 
                        CssClass="buttonCSS" onclick="SaveButton_Click" UseSubmitBehavior="False" />
                        &nbsp;&nbsp;
                    <asp:Button ID="PreviousButton" runat="server" Text="Previous" 
                        CssClass="buttonCSS" onclick="PreviousButton_Click" UseSubmitBehavior="False" />
                        &nbsp;&nbsp;
                    <asp:Button ID="StepButton" runat="server" Text="Next" 
                        onclick="StepButton_Click" CssClass="buttonCSS" UseSubmitBehavior="False" />
                    <asp:Button ID="CompleteButton" runat="server" Text="Complete" 
                        CssClass="buttonCSS" onclick="CompleteButton_Click" UseSubmitBehavior="False" />
                    <br />
                        <asp:PlaceHolder ID="ErrorMessages" runat="server"></asp:PlaceHolder>
                </td>
            </tr>
        </table>
        </asp:PlaceHolder>
</asp:Content>