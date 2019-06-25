using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;
using System.Security.Principal;
using System.Data;
using NominalDefendant;
using System.Net.Mail;
using System.Configuration;

public partial class AddUpdateND : System.Web.UI.Page
{
    
    int passValidation = 0;
    SqlConnection myConnection = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["NominalConn"].ConnectionString);

    protected void Page_Init(object sender, EventArgs e)
    {
        try
        {
            if (myConnection != null && myConnection.State == ConnectionState.Closed)
            {
                myConnection.Open();
            }
            SqlDataReader myReader = null;
            String mySQL = "Select * FROM [NDF].[dbo].[ClaimType] Order By ID ASC";
            SqlCommand myCommand = new SqlCommand(mySQL, myConnection);
            myReader = myCommand.ExecuteReader();
            bool rowCount = myReader.HasRows;
            if (rowCount)
            {
                while (myReader.Read())
                {
                    CheckBox cb1 = new CheckBox();
                    cb1.ID = "ClaimType_" + myReader["ID"].ToString();
                    cb1.Text = myReader["ClaimType"].ToString() + "<br />";
                    ClaimTypes.Controls.Add(cb1);
                }
            }
            myReader.Close();
            myConnection.Close();

        }catch(Exception ex){
            Resources.errorHandling(ex);
        }
        
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                // make claims officer default to logged in user
                SetDefaultClaimsOfficer();
                FirstNameInput.Focus();

                if (Session["Admin"] != null && Session["Admin"].ToString() == "Y")
                {
                    unAllocateButton.Visible = true;
                }
                else
                {
                    unAllocateButton.Visible = false;
                }

            }

            if (Request["NDID"] != null && Request["NDID"] != "")
            {
                if (NDID.Value == "")
                {
                    NDID.Value = Request["NDID"];
                    if (!IsPostBack)
                    {
                        populateFields();
                    }
                }

                if (Request["process"] != null && Request["process"] != "")
                {
                    if (Request["process"] == "Allocate")
                    {
                        if (!IsPostBack)
                        {
                            //showStep(3);
                            AllocateButton_Click(null, null);
                            addSaveAllocated();
                            // need to remove the process from the URL so it doesnt try to allocate it again. 
                        }

                    }
                }
            }
            else
            {
                if (Request["process"] != null && Request["process"] != "")
                {
                    if (Request["process"] == "Related")
                    {
                        if (!IsPostBack)
                        {
                            //RelatedRefNum.Text = Request["RelatedValues"].ToString();
                            // call create related claim 
                            CreateRelatedClaim(Request["RelatedValues"].ToString());
                        }

                    }
                }
            }

            ShowInsurerName();

            DateTime dt = DateTime.Now;
            //if (DateReceivedInput.Text == "")
            //{
            //    DateReceivedInput.Text = dt.Day + "/" + dt.Month + "/" + dt.Year;
            //    DateReceivedInput.Style.Value = "background-color:white";
            //    ReceivedLabel.Text = "";
            //}

            if (CommentActionDate.Text == "")
            {
                CommentActionDate.Text = dt.Day + "/" + dt.Month + "/" + dt.Year;
                CommentActionDate.Style.Value = "background-color:white";
                CommentActionLabel.Text = "";
            }

            ClaimNumberInput.Attributes.Add("onfocus", "blurField(this)");

            LoggedByInput.Attributes.Add("onkeydown", "return RestrictAllEntry(this, event, document.getElementById(\"MainContent_LoggedById\"))");
            LocationInput.Attributes.Add("onkeydown", "return RestrictAllEntry(this, event, document.getElementById(\"MainContent_LocationId\"))");

            CarRegoNumber.Attributes.Add("onkeypress", "return numberPlateEntry(this, event)");
            RelatedRefNum.Attributes.Add("onkeydown", "return relatedClaimEntry(this, event)");
            
            RelatedRefNum.Style.Value = "text-transform:uppercase";
            CarRegoNumber.Style.Value = "text-transform:uppercase";

            CarAddForm.Visible = false;

            //CarRegistrationExpiryInput

            string CurrentStepString = CurrentStepVal.Value;

            if (CurrentStepString == "")
            {
                showStep(1);
                CurrentStepVal.Value = "1";
            }
            else
            {
                int CurrentStep = Convert.ToInt32(CurrentStepVal.Value);
                showStep(CurrentStep);
            }

            showDocuments();
            showVehicles();
            showNotes();

            if (IsPostBack)
            {
                showVehicles();
                showNotes();
            }

        }catch(Exception ex){
            Resources.errorHandling(ex);
        }

    }

    public void CreateRelatedClaim(String RelClaimNum)
    {
       
        try
        {
            myConnection.Close();
            tmpLabel.Text = RelClaimNum;
            string tmpClaimNum = "";
            if (RelClaimNum != "")
            {
                string[] claimArr = RelClaimNum.Split(',');
                tmpClaimNum = claimArr[0];

                String AllRelClaims = "";

                myConnection.Open();
                String mySQL = "Select * FROM [NDF].[dbo].[NominalDefendants] WHERE [NDF].[dbo].[NominalDefendants].ClaimNumber = '" + tmpClaimNum + "'";
                SqlDataReader myReader = null;
                SqlCommand myCommand = new SqlCommand(mySQL, myConnection);
                myReader = myCommand.ExecuteReader();
                bool rowCount = myReader.HasRows;
                if (rowCount)
                {
                    myReader.Read();
                    String tmpClaimId = myReader["ID"].ToString();

                    AllRelClaims = myReader["RelatedRefNum"].ToString();
                    if (myReader["RelatedRefNum"].ToString() == "")
                    {
                        AllRelClaims = myReader["ClaimNumber"].ToString();
                    }else{
                        AllRelClaims += "," + myReader["ClaimNumber"].ToString();
                    }
                    
                    

                    // need to create claim with following fields populated

                    String mySQLAdd = "INSERT INTO [NDF].[dbo].[NominalDefendants](ClaimReceived,ClaimAgainstId,AccidentDate,AccidentLocationId,AllocatedInsurerId,AllocatedDate,ClaimsOfficerId,RelatedRefNum) VALUES ('" + swapDate(myReader["ClaimReceived"].ToString()) + "','" + myReader["ClaimAgainstId"].ToString() + "','" + swapDate(myReader["AccidentDate"].ToString()) + "','" + myReader["AccidentLocationId"].ToString() + "','" + myReader["AllocatedInsurerId"].ToString() + "','" + swapDate(myReader["AllocatedDate"].ToString()) + "','" + myReader["ClaimsOfficerId"].ToString() + "','')";

                    tmpLabel.Text = mySQLAdd;

                    SqlCommand myCommandAdd = new SqlCommand(mySQLAdd, myConnection);
                    myCommandAdd.ExecuteNonQuery();

                    SqlCommand myCommand2 = new SqlCommand("SELECT @@IDENTITY AS 'Identity' FROM [NDF].[dbo].[NominalDefendants]", myConnection);
                    SqlDataReader myReaderAdd = null;
                    myReaderAdd = myCommand2.ExecuteReader();
                    myReaderAdd.Read();
                    //ClaimNumberInput.Text = myReader["Identity"].ToString();
                    NDID.Value = myReaderAdd["Identity"].ToString();
                    myReaderAdd.Close();

                    GenerateNDNumber("Allocate");

                    AllRelClaims += "," + ClaimNumberVal.Value;

                    myConnection.Open();
                    SqlCommand myCommandCars = new SqlCommand("Select * FROM [NDF].[dbo].[VehiclesInvolved] WHERE Ndid = '" + tmpClaimId + "'", myConnection);
                    SqlDataReader myReaderCars = null;
                    myReaderCars = myCommandCars.ExecuteReader();

                    bool rowCountCars = myReaderCars.HasRows;
                    if (rowCountCars)
                    {
                        
                        while (myReaderCars.Read())
                        {
                            
                            string SQLCarAdd = "INSERT INTO [NDF].[dbo].[VehiclesInvolved](RegistrationNumber,AtFault,RegistrationState,RegistrationExpiry,VehicleClass,InsurerId,Ndid) VALUES ('" + myReaderCars["RegistrationNumber"].ToString() + "','" + myReaderCars["AtFault"].ToString() + "','" + myReaderCars["RegistrationState"].ToString() + "','" + swapDate(myReaderCars["RegistrationExpiry"].ToString()) + "','" + myReaderCars["VehicleClass"].ToString() + "','" + myReaderCars["InsurerId"].ToString() + "','" + NDID.Value + "')";
                            SqlCommand myCommandAddCar = new SqlCommand(SQLCarAdd, myConnection);
                            myCommandAddCar.ExecuteNonQuery();
                            
                        }
                        
                    }

                    // Update related claims for each claim that is related to this one
                    string[] claimArrRelAll = AllRelClaims.Split(',');

                    foreach (string relClaimNumber in claimArrRelAll)
                    {
                        string tmprelClaimNumbers = AllRelClaims.Replace(relClaimNumber.ToString(), "");
                        tmprelClaimNumbers = tmprelClaimNumbers.Replace(",,", ",");
                        tmprelClaimNumbers = tmprelClaimNumbers.Trim(',');


                        String mySQLUpdateRC = "UPDATE [NDF].[dbo].[NominalDefendants] set RelatedRefNum = '" + tmprelClaimNumbers + "' where ClaimNumber = '" + relClaimNumber + "'";
                        SqlCommand myCommandUpdateRC = new SqlCommand(mySQLUpdateRC, myConnection);
                        myCommandUpdateRC.ExecuteNonQuery();

                    }

                }
                myReader.Close();
                Response.Redirect("AddUpdateND.aspx?NDID=" + NDID.Value, false);
            }

            
        }
        catch (Exception ex)
        {
            Resources.errorHandling(ex);
        }


    }

    protected void ShowFolderDialog(object sender, EventArgs e)
    {
        try
        {
            
        }
        catch 
        {
            
        }
    }

    public void showDocuments()
    {
        myConnection.Close();
        try
        {
            DocAttachmentsPlaceholder.Text = "";
            DocViewPlaceholder.Controls.Clear();
            myConnection.Open();
            String MyDocLocation = "";
            String mySQL = "Select ClaimDocumentLoc FROM [NDF].[dbo].[NominalDefendants] WHERE [NDF].[dbo].[NominalDefendants].ID = '" + NDID.Value + "'";
            SqlDataReader myReader = null;
            SqlCommand myCommand = new SqlCommand(mySQL, myConnection);
            myReader = myCommand.ExecuteReader();
            bool rowCount = myReader.HasRows;
            if (rowCount)
            {
                myReader.Read();
                if (myReader["ClaimDocumentLoc"].ToString() != "" && myReader["ClaimDocumentLoc"] != null)
                {
                    MyDocLocation = myReader["ClaimDocumentLoc"].ToString();
                    MyDocLocation = MyDocLocation.Replace("G:\\", "\\\\rppmaa1\\common\\");
                    
                    if (Directory.Exists(@MyDocLocation))
                    {
                        string[] filePaths = Directory.GetFiles(@MyDocLocation);
                        foreach (string Filename in filePaths)
                        {
                            if (Path.GetFileName(Filename).ToString() != "Thumbs.db")
                            {
                                DocAttachmentsPlaceholder.Text += "<a href='file://" + Filename + "'>" + Path.GetFileName(Filename).ToString() + "</a><br />";
                            }
                        }

                        Literal viewButton = new Literal();
                        viewButton.Text = "&nbsp;<a href='" + myReader["ClaimDocumentLoc"].ToString() + "'>VIEW</a>";
                        
                        viewButton.ID = "ViewFilesButton";
                        DocViewPlaceholder.Controls.Add(viewButton);

                    }
                    else
                    {
                        DocAttachmentsPlaceholder.Text += "DIRECTORY NOT FOUND: " + MyDocLocation;
                    }


                    
                    
                }
                
            }
        }
        catch (Exception ex)
        {
            Resources.errorHandling(ex);
        }
    }

    public void populateFields()
    {
        myConnection.Close();
        try
        {
            myConnection.Open();

            String mySQL = "Select * FROM [NDF].[dbo].[NominalDefendants] WHERE [NDF].[dbo].[NominalDefendants].ID = '" + NDID.Value + "'";
            SqlDataReader myReader = null;
            SqlCommand myCommand = new SqlCommand(mySQL, myConnection);
            myReader = myCommand.ExecuteReader();
            bool rowCount = myReader.HasRows;
            if (rowCount)
            {
                myReader.Read();
                // Screen 1
                // Change this to be the new NDID val after allocation
                ClaimNumberInput.Text = myReader["ClaimNumber"].ToString();
                ClaimNumberVal.Value = ClaimNumberInput.Text;

                FirstNameInput.Text = myReader["ClaimantFirstName"].ToString();
                LastNameInput.Text = myReader["ClaimantLastName"].ToString();
                if (myReader["ClaimReceived"].ToString() != "")
                {
                    DateReceivedInput.Text = displayDate(myReader["ClaimReceived"].ToString()); 
                }

                

                LoggedById.Value = myReader["ClaimLodgedById"].ToString();
                LodgersRef.Text = myReader["LodgersRef"].ToString();

                if (myReader["ClaimantDOB"].ToString() != "" && myReader["ClaimantDOB"].ToString().IndexOf("1900") == -1)
                {
                    DateOfBirthInput.Text = displayDate(myReader["ClaimantDOB"].ToString());
                }

                if (myReader["ClaimAgainstId"].ToString() != "" && myReader["ClaimAgainstId"].ToString() != "0") { ClaimAgainstInput.SelectedValue = myReader["ClaimAgainstId"].ToString(); }
                if (myReader["ClaimsOfficerId"].ToString() != "") { ClaimsOfficer.SelectedValue = myReader["ClaimsOfficerId"].ToString(); }
                if (myReader["RoleInAccidentId"].ToString() != "" && myReader["RoleInAccidentId"].ToString() != "0") { RoleInAccident.SelectedValue = myReader["RoleInAccidentId"].ToString(); }
                if (myReader["AnomalyId"].ToString() != "" && myReader["AnomalyId"].ToString() != "0") { AccidentAnomaly.SelectedValue = myReader["AnomalyId"].ToString(); }

                if(myReader["ClaimType"].ToString() != ""){
                    String ClaimTypeString = myReader["ClaimType"].ToString();
                    string[] claimTypeArr = ClaimTypeString.Split('X');
                    foreach (string claimTypeVal in claimTypeArr)
                    {
                        if (claimTypeVal != "" && claimTypeVal != null)
                        {
                            CheckBox checkType = FindControl("ctl00$MainContent$ClaimType_" + claimTypeVal) as CheckBox;
                            checkType.Checked = true;
                        }
                    }
                }

                RelatedRefNum.Text = myReader["RelatedRefNum"].ToString();

                if (myReader["AnomalyOther"].ToString() != "")
                {
                    AnomalyOther.Enabled = true;
                    AnomalyOther.Text = myReader["AnomalyOther"].ToString();
                }
                

                if (myReader["ClaimAgainstId"].ToString() == "1")
                {
                    //CarAtFaultYes.Enabled = false;
                    //CarAtFaultNo.Checked = true;
                }

                if (myReader["Rejected"].ToString() == "True")
                {
                    RejectedCheckbox.Checked = true;
                    RejectedReason.Enabled = true;
                    RejectedReason.SelectedValue = myReader["RejectedReason"].ToString();
                    AllocateButton.Enabled = false;
                    ManualAllocateButton.Enabled = false;
                    DateRejected.Enabled = true;
                    if (myReader["DateRejected"].ToString() != "" && myReader["DateRejected"].ToString().IndexOf("1900") == -1)
                    {
                        DateRejected.Text = displayDate(myReader["DateRejected"].ToString());
                    }
                }

                // Screen 2
                
                if (myReader["AccidentDate"].ToString() != "" && myReader["AccidentDate"].ToString().IndexOf("1900") == -1)
                {
                    AccidentDateInput.Text = displayDate(myReader["AccidentDate"].ToString());
                    if (myReader["LateClaim"].ToString() == "True")
                    {
                        LateClaim.Checked = true;
                    }
                }
                

                LocationId.Value = myReader["AccidentLocationId"].ToString();

                // Screen 3
                AllocatedInsurerId.Value = myReader["AllocatedInsurerId"].ToString();
                if (myReader["AllocatedDate"].ToString() != "" && myReader["AllocatedDate"].ToString().IndexOf("1900") == -1) { AllocatedDateInput.Text = displayDate(myReader["AllocatedDate"].ToString()); }

                if (myReader["InvestigationRequired"].ToString().Equals("True")){
                    MAAInvestigationYes.Checked = true;
                }else{
                    MAAInvestigationNo.Checked = true;
                }

                if (myReader["ClaimCheckedDate"].ToString() != "" && myReader["ClaimCheckedDate"].ToString().IndexOf("1900") == -1)
                {
                    dateChecked.Text = displayDate(myReader["ClaimCheckedDate"].ToString());
                }

                if (myReader["ClaimCheckedBy"].ToString() != "" && myReader["ClaimCheckedBy"].ToString() != "0")
                {
                    CheckByOfficer.DataBind();
                    CheckByOfficer.SelectedValue = myReader["ClaimCheckedBy"].ToString();
                    HiddenCheckByOfficer.Value = myReader["ClaimCheckedBy"].ToString();
                }

                //Screen 4

                if (myReader["DateSubpoena"].ToString() != "" && myReader["DateSubpoena"].ToString().IndexOf("1900") == -1)
                {
                    DateSubpoena.Text = displayDate(myReader["DateSubpoena"].ToString());
                }

                if (myReader["DateStatmentClaim"].ToString() != "" && myReader["DateStatmentClaim"].ToString().IndexOf("1900") == -1)
                {
                    DateStatmentClaim.Text = displayDate(myReader["DateStatmentClaim"].ToString());
                }

                if (myReader["ClaimDocumentLoc"].ToString() != "")
                {
                    ClaimDocumentLoc.Text = myReader["ClaimDocumentLoc"].ToString();
                }
                
                // Screen 5
                if (myReader["ClaimStatus"].ToString() != "" && myReader["ClaimStatus"].ToString() != "0")
                {
                    ClaimStatus.DataBind();
                    ClaimStatus.SelectedValue = myReader["ClaimStatus"].ToString();
                    HiddenClaimStatus.Value = myReader["ClaimStatus"].ToString();
                }

                if (myReader["DateClosed"].ToString() != "" && myReader["DateClosed"].ToString().IndexOf("1900") == -1)
                {
                    DateClosed.Text = displayDate(myReader["DateClosed"].ToString());
                }

                if (myReader["CloseType"].ToString() != "" && myReader["CloseType"].ToString() != "0")
                {
                    CloseType.DataBind();
                    CloseType.SelectedValue = myReader["CloseType"].ToString();
                    HiddenCloseType.Value = myReader["CloseType"].ToString();
                }

                if (myReader["Payment_IssuerRef"].ToString() != "") {
                    Payment_IssuerRef.Text = myReader["Payment_IssuerRef"].ToString(); 
                }
                if (myReader["Payment_PaymentsToDate"].ToString() != "") {
                    Payment_PaymentsToDate.Text = String.Format("{0:C}", Convert.ToDecimal(myReader["Payment_PaymentsToDate"].ToString()));
                }
                if (myReader["Payment_OutstandingEstimate"].ToString() != "") { 
                    Payment_OutstandingEstimate.Text = String.Format("{0:C}", Convert.ToDecimal(myReader["Payment_OutstandingEstimate"].ToString()));
                }

                //CommentsActionsInput.Text = myReader["ClaimCommentsActions"].ToString();
                string ClaimLodgedById = myReader["ClaimLodgedById"].ToString();
                string AccidentLocationId = myReader["AccidentLocationId"].ToString();
                string locationTextValue = myReader["LocationText"].ToString();

                myReader.Close();

                // SQL lookups

                mySQL = "Select * FROM [NDF].[dbo].[Lodgers] WHERE Id = '" + ClaimLodgedById + "'";
                myReader = null;
                myCommand = new SqlCommand(mySQL, myConnection);
                myReader = myCommand.ExecuteReader();
                rowCount = myReader.HasRows;
                if (rowCount)
                {
                    myReader.Read();
                    String LoggedBy = "";
                    if (myReader["Company"].ToString()!="") {LoggedBy += myReader["Company"].ToString() + "\r\n"; }
                    if (myReader["FirstName"].ToString() != "") { LoggedBy += myReader["Title"].ToString() + " " + myReader["FirstName"].ToString() + " " + myReader["LastName"].ToString() + "\r\n"; }
                    if (myReader["Address"].ToString() != "") { LoggedBy += myReader["Address"].ToString() + "\r\n"; }
                    if (myReader["Suburb"].ToString() != "") { LoggedBy += myReader["Suburb"].ToString(); }

                    LoggedBy = LoggedBy.Replace("   ", " ");
                    LoggedBy = LoggedBy.Replace("  ", " ");
                    LoggedByInput.Text = LoggedBy;
                }
                myReader.Close();

                if (AccidentLocationId != "" && AccidentLocationId != "0")
                {
                    mySQL = "Select * FROM [NDF].[dbo].[Locations] WHERE id = '" + AccidentLocationId + "'";
                    myReader = null;
                    myCommand = new SqlCommand(mySQL, myConnection);
                    myReader = myCommand.ExecuteReader();
                    rowCount = myReader.HasRows;
                    if (rowCount)
                    {
                        myReader.Read();
                        LocationInput.Text = myReader["Suburb"].ToString() + ", " + myReader["State"].ToString() + ", " + myReader["Postcode"].ToString();
                    }
                    myReader.Close();
                }
                else if (locationTextValue != "" && locationTextValue != null)
                {
                    LocationInput.Text = locationTextValue;
                }

            }
            myConnection.Close();
        }
        catch (Exception ex)
        {
            Resources.errorHandling(ex);
        }
    }

    public void UpdateNote(object sender, EventArgs e)
    {
        myConnection.Close();
        try
        {
            myConnection.Open();
            Button updateButton = (Button)sender;
            int noteId = Convert.ToInt32(updateButton.ID.ToString().Replace("UpdateNoteButton_", ""));
            string notetext = Request["ctl00$MainContent$UpdateNoteText_" + noteId].ToString();
            String mySQL = "UPDATE [NDF].[dbo].[NominalNotes] set Notes ='" + notetext + "', Userid = '" + Page.User.Identity.Name.ToString() +"' WHERE Id = '" + noteId + "'";
            SqlCommand myCommand = new SqlCommand(mySQL, myConnection);
            myCommand.ExecuteNonQuery();
            myConnection.Close();
            showNotes();
        }
        catch (Exception ex)
        {
            Resources.errorHandling(ex);
        }
    }

    public void deleteNote(object sender, EventArgs e)
    {
        myConnection.Close();
        try
        {
            myConnection.Open();
            Button delButton = (Button)sender;
            int noteId = Convert.ToInt32(delButton.ID.ToString().Replace("DelNoteButton_", ""));
            String mySQL = "DELETE FROM [NDF].[dbo].[NominalNotes] WHERE Id = '" + noteId + "'";
            SqlCommand myCommand = new SqlCommand(mySQL, myConnection);
            myCommand.ExecuteNonQuery();
            myConnection.Close();
            showNotes();
        }
        catch (Exception ex)
        {
            Resources.errorHandling(ex);
        }
    }

    public void deleteCar(object sender, EventArgs e)
    {
        myConnection.Close();
        try
        {
            myConnection.Open();
            Button delButton = (Button)sender;
            int carId = Convert.ToInt32(delButton.ID.ToString().Replace("DelCarButton_", ""));
            String mySQL = "DELETE FROM [NDF].[dbo].[VehiclesInvolved] WHERE Id = '" + carId + "'";
            SqlCommand myCommand = new SqlCommand(mySQL, myConnection);
            myCommand.ExecuteNonQuery();
            myConnection.Close();
            showVehicles();
        }
        catch (Exception ex)
        {
            Resources.errorHandling(ex);
        }
    }

    public void CancelUpdateCar(object sender, EventArgs e)
    {   
        try
        {
            showVehicles();

        }
        catch (Exception ex)
        {
            Resources.errorHandling(ex);
        }

    }

    public void CancelupdateNoteButton(object sender, EventArgs e)
    {
        try
        {
            showNotes();
        }
        catch (Exception ex)
        {
            Resources.errorHandling(ex);
        }
    }

    public void updateNoteButton(object sender, EventArgs e)
    {
        try
        {
            showNotes();
            Button updateButton = (Button)sender;
            int noteId = Convert.ToInt32(updateButton.ID.ToString().Replace("EditNoteButton_", ""));
            TableRow rowEdit = (TableRow)PageDisplay.FindControl("rowEditNotes_" + noteId.ToString());
            TableRow rowDisp = (TableRow)PageDisplay.FindControl("rowDispNotes_" + noteId.ToString());

            rowEdit.Style.Value += "display:block";
            rowDisp.Style.Value += "display:none";
        }
        catch (Exception ex)
        {
            Resources.errorHandling(ex);
        }
    }

    public void updateCarButton(object sender, EventArgs e)
    {
        try
        {
            showVehicles();
            Button updateButton = (Button)sender;
            int carId = Convert.ToInt32(updateButton.ID.ToString().Replace("UpdateCarButton_", ""));
            TableRow rowEdit = (TableRow)PageDisplay.FindControl("rowEdit_" + carId.ToString());
            TableRow rowDisp = (TableRow)PageDisplay.FindControl("rowDisp_" + carId.ToString());

            rowEdit.Style.Value += "display:block";
            rowDisp.Style.Value += "display:none";
        }
        catch (Exception ex)
        {
            Resources.errorHandling(ex);
        }
    }
    
    public void updateCar(object sender, EventArgs e)
    {
        myConnection.Close();
        try
        {
            myConnection.Open();
            Button saveCarButton = (Button)sender;
            int carId = Convert.ToInt32(saveCarButton.ID.ToString().Replace("SaveCarButton_", ""));
            // Add Car Details
            String CarRegistrationExpiryDate = "";

            if (Request["ctl00$MainContent$CarRegistrationExpiryInput_" + carId] != "") { CarRegistrationExpiryDate = swapDate(Request["ctl00$MainContent$CarRegistrationExpiryInput_" + carId]); }

            String atFault = "";
            if (Request["ctl00$MainContent$AtFault_" + carId].Contains("Yes")) { atFault = "True"; } else { atFault = "False"; }
            String mySQL = "UPDATE [NDF].[dbo].[VehiclesInvolved] SET RegistrationNumber='" + Request["ctl00$MainContent$CarRegoNumber_" + carId] + "',AtFault='" + atFault + "',RegistrationState='" + Request["ctl00$MainContent$CarRegoState_" + carId] + "',RegistrationExpiry='" + CarRegistrationExpiryDate + "',VehicleClass='" + Request["ctl00$MainContent$CarClass_" + carId] + "',InsurerId='" + Request["ctl00$MainContent$Insurer_" + carId] + "' WHERE Id='" + carId + "'";

            SqlCommand myCommand3 = new SqlCommand(mySQL, myConnection);
            myCommand3.ExecuteNonQuery();

            myConnection.Close();

            showVehicles();

        }
        catch (Exception ex)
        {
            Resources.errorHandling(ex);
        }

    }

    public void ShowInsurerName()
    {
        myConnection.Close();
        try
        {
            myConnection.Open();

            String mySQL = "Select * FROM [NDF].[dbo].[Insurers], [NDF].[dbo].[NominalDefendants] WHERE [NDF].[dbo].[NominalDefendants].ID = '" + NDID.Value + "' AND [NDF].[dbo].[NominalDefendants].AllocatedInsurerId = [NDF].[dbo].[Insurers].Id";
            SqlDataReader myReader = null;

            SqlCommand myCommand = new SqlCommand(mySQL, myConnection);
            myReader = myCommand.ExecuteReader();
            bool rowCount = myReader.HasRows;
            if (rowCount)
            {
                myReader.Read();
                String InsurerName = myReader["InsurerName"].ToString();
                InsurerNamePlaceHolder.Controls.Clear();
                Literal InsurerNameDisp = new Literal();
                InsurerNameDisp.Text = InsurerName;
                InsurerNamePlaceHolder.Controls.Add(InsurerNameDisp);
                RejectedCheckbox.Enabled = false;
                //AllocateButton.Text = "Re-Allocate to Insurer";
                AllocateButton.Visible = false;
                reAllocateButton.Visible = true;

            }

        }
        catch (Exception ex)
        {
            Resources.errorHandling(ex);
        }
    }

    public void showStep(int step)
    {
        try
        {
            PageNumLabel.Text = step.ToString();
            switch (step)
            {
                case 1:
                    Screen1.Visible = true;
                    Screen2.Visible = false;
                    Screen3.Visible = false;
                    Screen4.Visible = false;
                    Screen5.Visible = false;
                    PreviousButton.Visible = false;
                    CompleteButton.Visible = false;
                    StepButton.Text = "Accident/Vehicles >>";
                    Screen1Button.Style.Value = "background-color:pink";
                    Screen2Button.Style.Value = "background-color:#E0E0EB";
                    Screen3Button.Style.Value = "background-color:#E0E0EB";
                    Screen4Button.Style.Value = "background-color:#E0E0EB";
                    Screen5Button.Style.Value = "background-color:#E0E0EB";
                    ScreenTitle.Text = "Claim Details";
                    FirstNameInput.Focus();
                    break;
                case 2:
                    Screen1.Visible = false;
                    Screen2.Visible = true;
                    Screen3.Visible = false;
                    Screen4.Visible = false;
                    Screen5.Visible = false;
                    PreviousButton.Visible = true;
                    PreviousButton.Enabled = true;
                    PreviousButton.Text = "<< Claim Details";
                    StepButton.Text = "Allocation >>";
                    Screen1Button.Style.Value = "background-color:#E0E0EB";
                    Screen2Button.Style.Value = "background-color:pink";
                    Screen3Button.Style.Value = "background-color:#E0E0EB";
                    Screen4Button.Style.Value = "background-color:#E0E0EB";
                    Screen5Button.Style.Value = "background-color:#E0E0EB";
                    ScreenTitle.Text = "Accident/Vehicles";
                    AccidentDateInput.Focus();
                    break;
                case 3:
                    Screen1.Visible = false;
                    Screen2.Visible = false;
                    Screen3.Visible = true;
                    ShowInsurerName();
                    Screen4.Visible = false;
                    Screen5.Visible = false;
                    StepButton.Visible = true;
                    CompleteButton.Visible = false;
                    PreviousButton.Text = "<< Accident/Vehicles";
                    StepButton.Text = "Documents/Correspondence >>";
                    Screen1Button.Style.Value = "background-color:#E0E0EB";
                    Screen2Button.Style.Value = "background-color:#E0E0EB";
                    Screen3Button.Style.Value = "background-color:pink";
                    Screen4Button.Style.Value = "background-color:#E0E0EB";
                    Screen5Button.Style.Value = "background-color:#E0E0EB";
                    ScreenTitle.Text = "Allocation";
                    break;
                case 4:
                    Screen1.Visible = false;
                    Screen2.Visible = false;
                    Screen3.Visible = false;
                    Screen4.Visible = true;
                    Screen5.Visible = false;
                    StepButton.Visible = true;
                    CompleteButton.Visible = false;
                    PreviousButton.Text = "<< Allocation";
                    StepButton.Text = "Litigation/Status >>";
                    Screen1Button.Style.Value = "background-color:#E0E0EB";
                    Screen2Button.Style.Value = "background-color:#E0E0EB";
                    Screen3Button.Style.Value = "background-color:#E0E0EB";
                    Screen4Button.Style.Value = "background-color:pink";
                    Screen5Button.Style.Value = "background-color:#E0E0EB";
                    ScreenTitle.Text = "Documents/Correspondence";

                    break;
                case 5:
                    Screen1.Visible = false;
                    Screen2.Visible = false;
                    Screen3.Visible = false;
                    Screen4.Visible = false;
                    Screen5.Visible = true;
                    StepButton.Visible = false;
                    PreviousButton.Text = "<< Documents/Correspondence";
                    CompleteButton.Visible = true;
                    Screen1Button.Style.Value = "background-color:#E0E0EB";
                    Screen2Button.Style.Value = "background-color:#E0E0EB";
                    Screen3Button.Style.Value = "background-color:#E0E0EB";
                    Screen4Button.Style.Value = "background-color:#E0E0EB";
                    Screen5Button.Style.Value = "background-color:pink";
                    ScreenTitle.Text = "Litigation/Status";
                    DateSubpoena.Focus();
                    break;
            }
        }
        catch (Exception ex)
        {
            Resources.errorHandling(ex);
        }

    }

    public void ChangeAnomoly(object sender, EventArgs e)
    {
        try
        {
            if (AccidentAnomaly.SelectedItem.Text == "Other")
            {
                AnomalyOther.Enabled = true;
            }
            else
            {
                AnomalyOther.Text = "";
                AnomalyOther.Enabled = false;
            }
        }
        catch (Exception ex)
        {
            Resources.errorHandling(ex);
        }

    }

    protected void CarAddButtonCancel_Click(object sender, EventArgs e)
    {
        try
        {
            CarRegoNumber.Text = "";
            CarAtFaultYes.Checked = false;
            CarAtFaultNo.Checked = true;
            CarRegoState.SelectedIndex = 0;
            CarRegistrationExpiryInput.Text = "";
            CarClass.Text = "";
            CarInsurer.SelectedIndex = 0;
            CarAddForm.Visible = false;
        }
        catch (Exception ex)
        {
            Resources.errorHandling(ex);
        }
    }

    protected void ShowScreenClick(object sender, EventArgs e)
    {
        try
        {
            int CurrentStep = Convert.ToInt32(CurrentStepVal.Value);
            Button ClickedButton = (Button)sender;
            String DestScreen = ClickedButton.ID.ToString().Replace("Button", "");
            DestScreen = DestScreen.Replace("Screen", "");

            if (CurrentStep > Convert.ToInt32(DestScreen))
            {
                int i = CurrentStep;
                while (i > Convert.ToInt32(DestScreen))
                {
                    PreviousButton_Click(null, null);
                    i--;
                }
            }
            else if (CurrentStep < Convert.ToInt32(DestScreen))
            {
                int i = CurrentStep;
                while (i < Convert.ToInt32(DestScreen))
                {
                    StepButton_Click(null, null);
                    i++;
                }
            }
            else
            {
                //on correct screen - do nothing   
            }
        }
        catch (Exception ex)
        {
            Resources.errorHandling(ex);
        }

    }

    protected void CarAddButton_Click(object sender, EventArgs e)
    {
        myConnection.Close();
        try
        {
            myConnection.Open();

            checkValidDateFormatAllowFuture(CarRegistrationExpiryInput, null);

            if (CarRegoExpiryLabel.Text == "")
            {
                // Add Car Details
                String CarRegistrationExpiryDate = "";
                if (CarRegistrationExpiryInput.Text != "") { CarRegistrationExpiryDate = swapDate(CarRegistrationExpiryInput.Text); }

                String mySQL2 = "INSERT INTO [NDF].[dbo].[VehiclesInvolved](RegistrationNumber,AtFault,RegistrationState,RegistrationExpiry,VehicleClass,InsurerId,Ndid) VALUES ('" + CarRegoNumber.Text + "','" + CarAtFaultYes.Checked + "','" + CarRegoState.Text + "','" + CarRegistrationExpiryDate + "','" + CarClass.Text + "','" + CarInsurer.Text + "','" + NDID.Value + "')";

                SqlCommand myCommand3 = new SqlCommand(mySQL2, myConnection);
                myCommand3.ExecuteNonQuery();

                myConnection.Close();

                CarRegoNumber.Text = "";
                CarAtFaultYes.Checked = false;
                CarAtFaultNo.Checked = true;
                CarRegoState.SelectedIndex = 0;
                CarRegistrationExpiryInput.Text = "";
                CarClass.Text = "";
                CarInsurer.SelectedIndex = 0;

                CarAddForm.Visible = false;

                showVehicles();
            }

        }
        catch (Exception ex)
        {
            Resources.errorHandling(ex);
        }
    }

    public void showVehicles()
    {
        myConnection.Close();

        if (NDID.Value != null)
        {

            try
            {
                VehiclesPlaceholder.Controls.Clear();

                myConnection.Open();
                SqlDataReader myReader = null;
                SqlCommand myCommand = new SqlCommand("SELECT * FROM VehiclesInvolved LEFT JOIN AustralianStates ON VehiclesInvolved.RegistrationState=AustralianStates.ID LEFT JOIN Insurers ON VehiclesInvolved.InsurerId = Insurers.InsurerId WHERE VehiclesInvolved.Ndid = '" + NDID.Value + "'", myConnection);
                VehicleErrorSpan.Text = ""; // this is for use when vehicle info does not match up with claim type - unidentified and uninsured
                myReader = myCommand.ExecuteReader();
                bool rowCount = myReader.HasRows;
                if (rowCount)
                {

                    

                    Table table = new Table();
                    table.CellPadding = 6;
                    table.CellSpacing = 2;

                    TableRow rowTitle = new TableRow();
                    rowTitle.ID = "RowTitle";
                    rowTitle.Height = 28;

                    TableCell cell1Title = new TableCell(); // Number Plate
                    TableCell cell2Title = new TableCell(); // At Fault
                    TableCell cell3Title = new TableCell(); // Rego State
                    TableCell cell4Title = new TableCell(); // Rego Expiry  
                    TableCell cell5Title = new TableCell(); // Vehicle Class
                    TableCell cell6Title = new TableCell(); // Insurer
                    TableCell cell7Title = new TableCell(); // Buttons

                    Literal cell1TitleText = new Literal();
                    Literal cell2TitleText = new Literal();
                    Literal cell3TitleText = new Literal();
                    Literal cell4TitleText = new Literal();
                    Literal cell5TitleText = new Literal();
                    Literal cell6TitleText = new Literal();
                    Literal cell7TitleText = new Literal();

                    cell1TitleText.Text = "<strong>Vehicle Registration No</strong>";
                    cell1Title.Controls.Add(cell1TitleText);
                    cell1Title.Width = 110;
                    cell1Title.Wrap = false;
                    cell1Title.CssClass = "tableTitle";
                    rowTitle.Cells.Add(cell1Title);

                    cell2TitleText.Text = "<strong>At Fault</strong>";
                    cell2Title.Controls.Add(cell2TitleText);
                    cell2Title.Width = 80;
                    cell2Title.CssClass = "tableTitle";
                    rowTitle.Cells.Add(cell2Title);

                    cell3TitleText.Text = "<strong>Rego State</strong>";
                    cell3Title.Controls.Add(cell3TitleText);
                    cell3Title.Width = 100;
                    cell3Title.CssClass = "tableTitle";
                    rowTitle.Cells.Add(cell3Title);

                    cell4TitleText.Text = "<strong>Rego Expiry</strong>";
                    cell4Title.Controls.Add(cell4TitleText);
                    cell4Title.Width = 120;
                    cell4Title.CssClass = "tableTitle";
                    rowTitle.Cells.Add(cell4Title);

                    cell5TitleText.Text = "<strong>Vehicle Class</strong>";
                    cell5Title.Controls.Add(cell5TitleText);
                    cell5Title.Wrap = false;
                    cell5Title.Width = 110;
                    cell5Title.CssClass = "tableTitle";
                    rowTitle.Cells.Add(cell5Title);

                    cell6TitleText.Text = "<strong>Insurer</strong>";
                    cell6Title.Controls.Add(cell6TitleText);
                    cell6Title.Width = 120;
                    cell6Title.CssClass = "tableTitle";
                    rowTitle.Cells.Add(cell6Title);

                    cell7TitleText.Text = " ";
                    cell7Title.Controls.Add(cell7TitleText);
                    cell7Title.Width = 110;
                    cell7Title.CssClass = "tableTitle";
                    rowTitle.Cells.Add(cell7Title);

                    table.Rows.Add(rowTitle);

                    HiddenField carID = new HiddenField();
                    carID.ID = "updatecarId";
                    carID.Value = "";
                    VehiclesPlaceholder.Controls.Add(carID);

                    HiddenField deleteCarID = new HiddenField();
                    deleteCarID.ID = "deletecarId";
                    deleteCarID.Value = "";
                    VehiclesPlaceholder.Controls.Add(deleteCarID);

                    while (myReader.Read())
                    {

                        // This is the display row.
                        TableRow rowDisp = new TableRow();
                        rowDisp.ID = "rowDisp_" + myReader["Id"].ToString();
                        rowDisp.Height = 28;
                        TableCell cell1Disp = new TableCell(); // Number Plate
                        TableCell cell2Disp = new TableCell(); // At Fault
                        TableCell cell3Disp = new TableCell(); // Rego State
                        TableCell cell4Disp = new TableCell(); // Rego Expiry  
                        TableCell cell5Disp = new TableCell(); // Vehicle Class
                        TableCell cell6Disp = new TableCell(); // Insurer
                        TableCell cell7Disp = new TableCell(); // Buttons

                        Literal cell1DispText = new Literal();
                        cell1DispText.Text = myReader["RegistrationNumber"].ToString();
                        cell1Disp.Style.Value = "text-transform:uppercase";
                        cell1Disp.Controls.Add(cell1DispText);
                        cell1Disp.CssClass = "tableField";
                        rowDisp.Cells.Add(cell1Disp);

                        Literal cell2DispText = new Literal();
                        if (myReader["AtFault"].ToString() == "True"){
                            cell2DispText.Text = "Yes";
                        }else{
                            cell2DispText.Text = "No";
                        }
                        cell2Disp.Controls.Add(cell2DispText);
                        cell2Disp.CssClass = "tableField";
                        rowDisp.Cells.Add(cell2Disp);

                        Literal cell3DispText = new Literal();
                        cell3DispText.Text = myReader["State"].ToString();
                        cell3Disp.Controls.Add(cell3DispText);
                        cell3Disp.CssClass = "tableField";
                        rowDisp.Cells.Add(cell3Disp);

                        Literal cell4DispText = new Literal();
                        if (!myReader["RegistrationExpiry"].ToString().Contains("1900") && myReader["RegistrationExpiry"].ToString() != "" && myReader["RegistrationExpiry"].ToString() != null)
                        {
                            cell4DispText.Text = displayDate(myReader["RegistrationExpiry"].ToString());
                        }else { 
                            cell4DispText.Text = ""; 
                        }
                        cell4Disp.Controls.Add(cell4DispText);
                        cell4Disp.CssClass = "tableField";
                        rowDisp.Cells.Add(cell4Disp);

                        Literal cell5DispText = new Literal();
                        cell5DispText.Text = myReader["VehicleClass"].ToString();
                        cell5Disp.Controls.Add(cell5DispText);
                        cell5Disp.CssClass = "tableField";
                        rowDisp.Cells.Add(cell5Disp);

                        Literal cell6DispText = new Literal();
                        cell6DispText.Text = myReader["InsurerName"].ToString();
                        cell6Disp.Controls.Add(cell6DispText);
                        cell6Disp.CssClass = "tableField";
                        rowDisp.Cells.Add(cell6Disp);
                        rowDisp.Visible = true;
                        table.Rows.Add(rowDisp);

                        Button UpdateButton = new Button();
                        UpdateButton.ID = "UpdateCarButton_" + myReader["Id"].ToString();
                        UpdateButton.Click += new EventHandler(updateCarButton);
                        UpdateButton.CssClass = "buttonCSSNarrow";
                        UpdateButton.Text = "Update";
                        cell7Disp.Controls.Add(UpdateButton);

                        Button delButton = new Button();
                        delButton.ID = "DelCarButton_" + myReader["Id"].ToString();
                        delButton.Click += new EventHandler(deleteCar);
                        delButton.CssClass = "buttonCSSNarrow";
                        delButton.Text = "Delete";
                        cell7Disp.Controls.Add(delButton);

                        cell7Disp.CssClass = "tableField";
                        rowDisp.Cells.Add(cell7Disp);

                        DateTime tmpAccDate = new DateTime();
                        if (AccidentDateInput.Text != "" && AccidentDateInput.Text != null)
                        {
                            CultureInfo provider = new CultureInfo("en-AU");
                            tmpAccDate = DateTime.Parse(AccidentDateInput.Text, provider);
                        }
                        
                        rowDisp.Visible = true;
                        table.Rows.Add(rowDisp);

                        ////////////////////////////////////

                        // This is the edit row.
                        TableRow row = new TableRow();
                        row.ID = "rowEdit_" + myReader["Id"].ToString();
                        row.Height = 28;

                        TableCell cell1 = new TableCell(); // Number Plate
                        TableCell cell2 = new TableCell(); // At Fault
                        TableCell cell3 = new TableCell(); // Rego State
                        TableCell cell4 = new TableCell(); // Rego Expiry  
                        TableCell cell5 = new TableCell(); // Vehicle Class
                        TableCell cell6 = new TableCell(); // Insurer
                        TableCell cell7 = new TableCell(); // Buttons

                        TextBox numberPlate = new TextBox();
                        numberPlate.ID = "CarRegoNumber_" + myReader["Id"].ToString();
                        numberPlate.CssClass = "textboxEntryVeryShort";
                        numberPlate.Text = myReader["RegistrationNumber"].ToString();
                        numberPlate.Style.Value = "text-transform:uppercase";
                        cell1.Width = 110;
                        cell1.Controls.Add(numberPlate);
                        row.Cells.Add(cell1);

                        RadioButton rb1 = new RadioButton();
                        rb1.ID = "CarAtFaultYes_" + myReader["Id"].ToString();
                        rb1.GroupName = "AtFault_" + myReader["Id"].ToString();
                        rb1.Text = "Yes";
                        if (myReader["AtFault"].ToString() == "True") { rb1.Checked = true; }
                        cell2.Controls.Add(rb1);
                        RadioButton rb2 = new RadioButton();
                        rb2.ID = "CarAtFaultNo_" + myReader["Id"].ToString();
                        rb2.GroupName = "AtFault_" + myReader["Id"].ToString();
                        rb2.Text = "No";
                        if (myReader["AtFault"].ToString() == "False") { rb2.Checked = true; }
                        cell2.Controls.Add(rb2);
                        cell2.Width = 80;
                        row.Cells.Add(cell2);

                        DropDownList stateList = new DropDownList();
                        stateList.ID = "CarRegoState_" + myReader["Id"].ToString();
                        stateList.DataSourceID = "AusStateDataSource";
                        stateList.DataTextField = "State";
                        stateList.DataValueField = "Id";
                        if (myReader["RegistrationState"].ToString() != "")
                        {
                            stateList.SelectedValue = myReader["RegistrationState"].ToString();
                        }
                        
                        stateList.CssClass = "dropDownCSSShort";
                        cell3.Controls.Add(stateList);
                        cell3.Width = 100;
                        row.Cells.Add(cell3);

                        TextBox CarRegistrationExpiryInput = new TextBox();
                        CarRegistrationExpiryInput.ID = "CarRegistrationExpiryInput_" + myReader["Id"].ToString();
                        CarRegistrationExpiryInput.CssClass = "textboxEntryVeryShort";
                        CarRegistrationExpiryInput.TextChanged += new EventHandler(checkValidDateFormatRego);
                        CarRegistrationExpiryInput.Attributes.Add("labelParam", "CarRegoExpiryLabel_" + myReader["Id"].ToString());
                        CarRegistrationExpiryInput.AutoPostBack = true;
                        if (!myReader["RegistrationExpiry"].ToString().Contains("1900") && myReader["RegistrationExpiry"].ToString() != "" && myReader["RegistrationExpiry"].ToString() != null) { CarRegistrationExpiryInput.Text = displayDate(myReader["RegistrationExpiry"].ToString()); }
                        cell4.Controls.Add(CarRegistrationExpiryInput);
                        
                        Literal calButton = new Literal();
                        calButton.Text = "<span style='vertical-align:bottom' class='textboxEntryShort'><a style='cursor:pointer' id='ImgUpdateRegoExpiry_" + myReader["Id"].ToString() + "' onclick=\"show_calendar('document.forms[0].ctl00$MainContent$CarRegistrationExpiryInput_" + myReader["Id"].ToString() + "', '');\"><img src='Images/dates.gif' alt='Select Date' /></a></span>";
                        cell4.Controls.Add(calButton);
                        Label regoExpLabel = new Label();
                        regoExpLabel.ID = "CarRegoExpiryLabel_" + myReader["Id"].ToString();
                        regoExpLabel.Attributes.Add("runat", "server");
                        cell4.Controls.Add(regoExpLabel);
                        cell4.Width = 120;
                        row.Cells.Add(cell4);

                        TextBox carClass = new TextBox();
                        carClass.ID = "CarClass_" + myReader["Id"].ToString();
                        carClass.CssClass = "textboxEntryVeryShort";
                        carClass.Text = myReader["VehicleClass"].ToString();
                        cell5.Controls.Add(carClass);
                        cell5.Width = 110;
                        row.Cells.Add(cell5);

                        DropDownList insurers = new DropDownList();
                        insurers.ID = "Insurer_" + myReader["Id"].ToString();
                        insurers.DataSourceID = "InsurerDataSource";
                        insurers.DataTextField = "InsurerName";
                        insurers.DataValueField = "InsurerId";
                        if (myReader["InsurerId"].ToString() != "")
                        {
                            insurers.SelectedValue = myReader["InsurerId"].ToString();
                        }
                        insurers.CssClass = "dropDownCSSInsurer";
                        cell6.Controls.Add(insurers);
                        cell6.Width = 120;
                        row.Cells.Add(cell6);

                        Button CancelButton = new Button();
                        CancelButton.ID = "CancelCarButton_" + myReader["Id"].ToString();
                        CancelButton.Click += new EventHandler(CancelUpdateCar);
                        CancelButton.CssClass = "buttonCSSNarrow";
                        CancelButton.Text = "Cancel";
                        cell7.Controls.Add(CancelButton);


                        Button saveCarButton = new Button();
                        saveCarButton.ID = "SaveCarButton_" + myReader["Id"].ToString();
                        saveCarButton.Click += new EventHandler(updateCar);
                        saveCarButton.CssClass = "buttonCSSNarrow";
                        saveCarButton.Text = "Save";
                        cell7.Controls.Add(saveCarButton);

                        row.Cells.Add(cell7);

                        row.Style.Value = "display:none";
                        table.Rows.Add(row);

                    }
                    VehiclesPlaceholder.Controls.Add(table);
                }

                myReader.Close();
                myConnection.Close();
            }
            catch (Exception ex)
            {
                Resources.errorHandling(ex);
            }
        }

    }

    public void showNotes(){
        myConnection.Close();

        if (NDID.Value != null)
        {

            try
            {
                CommentsPlaceHolder.Controls.Clear();

                myConnection.Open();
                SqlDataReader myReader = null;
                SqlCommand myCommand = new SqlCommand("Select * from [NDF].[dbo].[NominalNotes] WHERE Ndid = '" + NDID.Value + "'", myConnection);
                myReader = myCommand.ExecuteReader();
                bool rowCount = myReader.HasRows;
                if (rowCount)
                {

                    Table table = new Table();
                    table.CellPadding = 4;
                    table.CellSpacing = 0;

                    TableRow rowTitle = new TableRow();
                    rowTitle.ID = "RowTitleNotes";
                    rowTitle.Height = 28;

                    TableCell cell1Title = new TableCell(); // Date
                    TableCell cell2Title = new TableCell(); // Notes/Comments
                    TableCell cell3Title = new TableCell(); // Buttons

                    Literal cell1TitleText = new Literal();
                    Literal cell2TitleText = new Literal();
                    Literal cell3TitleText = new Literal();

                    cell1TitleText.Text = "<strong>Date</strong>";
                    cell1Title.Controls.Add(cell1TitleText);
                    cell1Title.Width = 110;
                    cell1Title.Wrap = false;
                    cell1Title.CssClass = "tableTitle";
                    rowTitle.Cells.Add(cell1Title);

                    cell2TitleText.Text = "<strong>comments</strong>";
                    cell2Title.Controls.Add(cell2TitleText);
                    cell2Title.Width = 300;
                    cell2Title.CssClass = "tableTitle";
                    rowTitle.Cells.Add(cell2Title);

                    cell3TitleText.Text = " ";
                    cell3Title.Controls.Add(cell3TitleText);
                    cell3Title.Width = 110;
                    cell3Title.CssClass = "tableTitle";
                    rowTitle.Cells.Add(cell3Title);

                    table.Rows.Add(rowTitle);

                    HiddenField deleteNoteID = new HiddenField();
                    deleteNoteID.ID = "deleteNoteId";
                    deleteNoteID.Value = "";
                    CommentsPlaceHolder.Controls.Add(deleteNoteID);

                    while (myReader.Read())
                    {

                        // This is the display row.
                        TableRow rowDisp = new TableRow();
                        rowDisp.ID = "rowDispNotes_" + myReader["Id"].ToString();
                        rowDisp.Height = 28;
                        TableCell cell1Disp = new TableCell(); // Date
                        TableCell cell2Disp = new TableCell(); // Notes/Comments
                        TableCell cell3Disp = new TableCell(); // Buttons

                        Literal cell1DispText = new Literal();
                        cell1DispText.Text = myReader["NoteDate"].ToString();
                        cell1Disp.Style.Value = "text-transform:uppercase";
                        cell1Disp.Controls.Add(cell1DispText);
                        cell1Disp.CssClass = "tableField";
                        rowDisp.Cells.Add(cell1Disp);

                        Literal cell2DispText = new Literal();
                        cell2DispText.Text = myReader["Notes"].ToString();
                        cell2Disp.Controls.Add(cell2DispText);
                        cell2Disp.CssClass = "tableField";
                        rowDisp.Cells.Add(cell2Disp);

                        cell3Disp.CssClass = "tableField";

                        Button editNoteButton = new Button();
                        editNoteButton.ID = "EditNoteButton_" + myReader["Id"].ToString();
                        editNoteButton.Click += new EventHandler(updateNoteButton);
                        editNoteButton.CssClass = "buttonCSSNarrow";
                        editNoteButton.Text = "Edit";
                        editNoteButton.UseSubmitBehavior = false;
                        cell3Disp.Controls.Add(editNoteButton);

                        Button delButton = new Button();
                        delButton.Text = "Delete";
                        delButton.ID = "DelNoteButton_" + myReader["Id"].ToString();
                        delButton.UseSubmitBehavior = false;
                        delButton.Click += new EventHandler(deleteNote);
                        delButton.CssClass = "buttonCSSNarrow";
                        cell3Disp.Controls.Add(delButton);
                        
                        rowDisp.Cells.Add(cell3Disp);

                        table.Rows.Add(rowDisp);

                        // This is the edit row.
                        TableRow rowEdit = new TableRow();
                        rowEdit.ID = "rowEditNotes_" + myReader["Id"].ToString();
                        rowEdit.Height = 28;
                        TableCell cell1Edit = new TableCell(); // Date
                        TableCell cell2Edit = new TableCell(); // Notes/Comments
                        TableCell cell3Edit = new TableCell(); // Buttons

                        Literal noteDate = new Literal();
                        noteDate.Text = myReader["NoteDate"].ToString();
                        cell1Edit.Controls.Add(noteDate);
                        cell1Edit.CssClass = "tableField";
                        rowEdit.Cells.Add(cell1Edit);

                        TextBox noteText = new TextBox();
                        noteText.ID = "UpdateNoteText_" + myReader["Id"].ToString();
                        noteText.CssClass = "textboxEntry";
                        noteText.Rows = 5;
                        noteText.Text = myReader["Notes"].ToString();
                        cell2Edit.Controls.Add(noteText);
                        cell2Edit.CssClass = "tableField";
                        rowEdit.Cells.Add(cell2Edit);

                        Button updateNoteBut = new Button();
                        updateNoteBut.Text = "Save";
                        updateNoteBut.ID = "UpdateNoteButton_" + myReader["Id"].ToString();
                        updateNoteBut.Click += new EventHandler(UpdateNote);
                        updateNoteBut.CssClass = "buttonCSSNarrow";
                        updateNoteBut.UseSubmitBehavior = false;

                        Button updateNoteButCancel = new Button();
                        updateNoteButCancel.Text = "Cancel";
                        updateNoteButCancel.ID = "CancelUpdateNoteButton_" + myReader["Id"].ToString();
                        updateNoteButCancel.Click += new EventHandler(CancelupdateNoteButton);
                        updateNoteButCancel.CssClass = "buttonCSSNarrow";
                        updateNoteButCancel.UseSubmitBehavior = false;

                        cell3Edit.Controls.Add(updateNoteButCancel);
                        cell3Edit.Controls.Add(updateNoteBut);
                        
                        cell3Edit.CssClass = "tableField";
                        rowEdit.Cells.Add(cell3Edit);

                        rowEdit.Style.Value = "display:none";

                        table.Rows.Add(rowEdit);


                    }
                    CommentsPlaceHolder.Controls.Add(table);
                }

                myReader.Close();
                myConnection.Close();
            }
            catch (Exception ex)
            {
                Resources.errorHandling(ex);
            }
        }
    }

    protected void ActionSheetButton_Click(object sender, EventArgs e)
    {
        try
        {
            SaveButton_Click(null, null);
            string script = "<script language=\"javas" + "cript\">\r\n";
            script += "{\r\n";
            script += "window.open('printActionSheet.aspx?ndid=" + NDID.Value + "', 'PrintActionSheet', 'width=700px,height=790px,top=0,left=0,scrollbars=0');\r\n";
            script += "}\r\n";
            script += "</" + "script>";
            ClientScript.RegisterClientScriptBlock(this.GetType(), "PrintActionSheet", script);
        }
        catch (Exception ex)
        {
            Resources.errorHandling(ex);
        }

    }

    protected void PreviousButton_Click(object sender, EventArgs e)
    {
        try
        {
            addSave();
            int CurrentStep = Convert.ToInt32(CurrentStepVal.Value);
            CurrentStep = CurrentStep - 1;
            CurrentStepVal.Value = Convert.ToString(CurrentStep);
            showStep(CurrentStep);
        }
        catch (Exception ex)
        {
            Resources.errorHandling(ex);
        }
    }

    protected void CompleteButton_Click(object sender, EventArgs e)
    {
        try
        {
            addSave();
            Response.Redirect("Default.aspx", false);
        }
        catch (Exception ex)
        {
            Resources.errorHandling(ex);
        }
    }

    protected void AddVehicleShowHide_Click(object sender, EventArgs e)
    {
        try
        {
            CarAddForm.Visible = true;
            string javaScript = "<script language=JavaScript>\n" + "hideUpdateCarButton();\n" + "</script>";
            ClientScript.RegisterStartupScript(this.GetType(), "function", javaScript);
        }
        catch (Exception ex)
        {
            Resources.errorHandling(ex);
        }

    }

    protected void AllocateButtonManual_Click(object sender, EventArgs e)
    {
        try
        {
            InsurerNamePlaceHolder.Visible = false;
            ManualAllocateInsurerDropDown.Visible = true;
            ManualAllocateButton.Visible = false;
            ManualAllocateSaveButton.Visible = true;
            ManualAllocateCancelButton.Visible = true;
        }
        catch (Exception ex)
        {
            Resources.errorHandling(ex);
        }
    }

    protected void AllocateButtonManualCancel_Click(object sender, EventArgs e)
    {
        try
        {
            InsurerNamePlaceHolder.Visible = true;
            ManualAllocateInsurerDropDown.Visible = false;
            ManualAllocateButton.Visible = true;
            ManualAllocateSaveButton.Visible = false;
            ManualAllocateCancelButton.Visible = false;
        }
        catch (Exception ex)
        {
            Resources.errorHandling(ex);
        }
    }

    protected void checkByChanged(object sender, EventArgs e)
    {
        try
        {
            HiddenCheckByOfficer.Value = CheckByOfficer.SelectedValue;
            if (CheckByOfficer.SelectedValue == "0")
            {
                dateChecked.Text = "";
            }else{
                DateTime dt = DateTime.Now;
                if (dateChecked.Text == "")
                {
                    dateChecked.Text = dt.Day + "/" + dt.Month + "/" + dt.Year;
                }
            }
        }
        catch (Exception ex)
        {
            Resources.errorHandling(ex);
        }
    }

    protected void AllocateButtonManualSave_Click(object sender, EventArgs e)
    {
        try
        {
            myConnection.Close();
            AllocatedInsurerId.Value = ManualAllocateInsurerDropDown.SelectedValue.ToString();

            myConnection.Open();
            SqlDataReader myReader = null;
            SqlCommand myCommand = new SqlCommand("SELECT * FROM [NDF].[dbo].[Insurers] WHERE Id = '" + AllocatedInsurerId.Value + "'", myConnection);
            myReader = myCommand.ExecuteReader();
            myReader.Read();
            String InsurerName = myReader["InsurerName"].ToString();

            InsurerNamePlaceHolder.Controls.Clear();

            Literal InsurerNameDisp = new Literal();
            InsurerNameDisp.Text = InsurerName;
            InsurerNamePlaceHolder.Controls.Add(InsurerNameDisp);

            AllocateButton.Text = "Re-Allocate to Insurer";

            InsurerNamePlaceHolder.Visible = true;
            ManualAllocateInsurerDropDown.Visible = false;
            ManualAllocateButton.Visible = true;
            ManualAllocateSaveButton.Visible = false;
            ManualAllocateCancelButton.Visible = false;

            myReader.Close();
            myConnection.Close();

            DateTime dt = DateTime.Now;
            if (AllocatedDateInput.Text == "")
            {
                AllocatedDateInput.Text = dt.Day + "/" + dt.Month + "/" + dt.Year;
            }

            GenerateNDNumber("Allocate");
            addSave();
        }
        catch (Exception ex)
        {
            Resources.errorHandling(ex);
        }
    }

    protected void AllocateButton_Click(object sender, EventArgs e)
    {
        myConnection.Close();

        try
        {

            String InsurersExcluded = "-1";
            myConnection.Open();
            SqlDataReader myReader = null;
            SqlCommand myCommand = new SqlCommand("SELECT * FROM [NDF].[dbo].[Insurers] WHERE Id In(Select InsurerId from [NDF].[dbo].[VehiclesInvolved] WHERE Ndid = '" + NDID.Value + "')", myConnection);
            myReader = myCommand.ExecuteReader();
            while (myReader.Read())
            {
                InsurersExcluded += "," + myReader["InsurerId"].ToString();
                if(myReader["Linkings"].ToString() != ""){ 
                    //remove last char from linkings
                    InsurersExcluded += "," + myReader["Linkings"].ToString().Remove(myReader["Linkings"].ToString().Length -1);
                }
            }
            myReader.Close();

            String MandatorInsurer = "";
            if (RelatedRefNum.Text !="")
            {
                string[] RelatedClaims = RelatedRefNum.Text.Split(',');
                foreach (string RelatedClaim in RelatedClaims)
                {
                    myCommand = new SqlCommand("SELECT InsurerId, Linkings FROM [NDF].[dbo].[NominalDefendants], [NDF].[dbo].[Insurers] WHERE NominalDefendants.AllocatedInsurerId = Insurers.Id AND NominalDefendants.ClaimNumber = '" + RelatedClaim + "'", myConnection);
                    myReader = myCommand.ExecuteReader();
                    while (myReader.Read())
                    {
                        MandatorInsurer = myReader["InsurerId"].ToString();
                    }
                    myReader.Close();
                }
            }

            String selectedInsurer = "";
            int SelInsurerLength = 0;

            if (MandatorInsurer !="")
            {
                myCommand = new SqlCommand("SELECT * FROM [NDF].[dbo].[Insurers] WHERE InsurerId ='" + MandatorInsurer + "'", myConnection);
                myReader = myCommand.ExecuteReader();
                while (myReader.Read())
                {
                    selectedInsurer = myReader["Id"].ToString();
                    AllocatedInsurerId.Value = selectedInsurer;
                    SelInsurerLength = 1;
                }
            }

            
            if (selectedInsurer == "")
            {
                myCommand = new SqlCommand("SELECT * FROM [NDF].[dbo].[Insurers] WHERE InsurerId Not In(" + InsurersExcluded + ")", myConnection);
                ArrayList MarketShareList = new ArrayList();

                myReader = myCommand.ExecuteReader();
                while (myReader.Read())
                {
                    int InsurerID = Convert.ToInt32(myReader["Id"].ToString());
                    Decimal MarketShare = Convert.ToDecimal(myReader["MarketShare"].ToString());
                    MarketShare = MarketShare * 100;
                    for (int i = 1; i <= MarketShare; i++)
                    {
                        MarketShareList.Add(InsurerID);
                    }
                }
                myReader.Close();
                // randomise the array and select one of the insurers.
                MarketShareList = ScrambleArrayList(MarketShareList);
                SelInsurerLength = MarketShareList.Count;
                if (SelInsurerLength > 0)
                {
                    selectedInsurer = MarketShareList[0].ToString();
                    AllocatedInsurerId.Value = selectedInsurer;
                }

            }


            if (SelInsurerLength > 0)
            {
                SqlCommand myCommand2 = new SqlCommand("SELECT * FROM [NDF].[dbo].[Insurers] WHERE Id = '" + selectedInsurer + "'", myConnection);
                myReader = myCommand2.ExecuteReader();
                myReader.Read();
                String InsurerName = myReader["InsurerName"].ToString();

                InsurerNamePlaceHolder.Controls.Clear();

                Literal InsurerNameDisp = new Literal();
                InsurerNameDisp.Text = InsurerName;
                InsurerNamePlaceHolder.Controls.Add(InsurerNameDisp);

                AllocateButton.Text = "Re-Allocate to Insurer";

                myReader.Close();
                myConnection.Close();

                DateTime dt = DateTime.Now;
                if (AllocatedDateInput.Text == "")
                {
                    AllocatedDateInput.Text = dt.Day + "/" + dt.Month + "/" + dt.Year;
                }

                GenerateNDNumber("Allocate");
                if (Request["process"] == null || Request["process"] == "")
                {
                    addSave();
                }
            }
            else
            {
                tmpLabel.Text = "<font color=red><strong>No insurers available to allocate this claim to. You must allocate manally.</strong></font>";
            }
        }
        catch (Exception ex)
        {
            Resources.errorHandling(ex);
        }

    }

    protected void UnAllocateButton_Click(object sender, EventArgs e)
    {
        try
        {
            myConnection.Open();
            String mySQL2 = "UPDATE [NDF].[dbo].[NominalDefendants] SET ClaimNumber ='', AllocatedInsurerId='', AllocatedDate='', Rejected='False', RejectedReason=NULL WHERE ID='" + NDID.Value + "'";
            SqlCommand myCommand = new SqlCommand(mySQL2, myConnection);
            myCommand.ExecuteNonQuery();
            myConnection.Close();

            ClaimNumberInput.Text = "";
            InsurerNamePlaceHolder.Controls.Clear();
            AllocateButton.Visible = true;
            reAllocateButton.Visible = false;
            AllocatedInsurerId.Value = "";
            AllocatedDateInput.Text = "";
            RejectedCheckbox.Enabled = true;
            RejectedCheckbox.Checked = false;
            RejectedReason.Enabled = false;
            AllocateButton.Enabled = true;
            ManualAllocateButton.Enabled = true;
            ClaimNumberVal.Value = "";
            addSave();
        }
        catch (Exception ex)
        {
            Resources.errorHandling(ex);
            tmpLabel.Text = ex.ToString();
        }
    }

    public ArrayList ScrambleArrayList(ArrayList AList)
    {
        try
        {
            Random RandomGen = new Random(DateTime.Now.Millisecond);
            ArrayList ScrambledList = new ArrayList();

            Int32 Index;

            while (AList.Count > 0)
            {
                Index = RandomGen.Next(AList.Count);
                ScrambledList.Add(AList[Index]);
                AList.RemoveAt(Index);
            }
            return ScrambledList;
        }
        catch (Exception ex)
        {
            Resources.errorHandling(ex);
            return null;
        }
    }

    public void CheckClaimTypeChecked()
    {
        try
        {
            myConnection.Open();
            SqlDataReader myReader = null;
            String mySQL = "Select * FROM [NDF].[dbo].[ClaimType] Order By ID ASC";
            SqlCommand myCommand = new SqlCommand(mySQL, myConnection);
            myReader = myCommand.ExecuteReader();
            bool rowCount = myReader.HasRows;
            String tmpResult = "";
            if (rowCount)
            {
                while (myReader.Read())
                {
                    CheckBox checkClaimType = FindControl("ctl00$MainContent$ClaimType_" + myReader["ID"].ToString()) as CheckBox;
                    if (checkClaimType.Checked)
                    {
                        tmpResult = "X";
                    }
                }
                myReader.Close();
                if (tmpResult == "")
                {
                    passValidation = 1;
                    myReader = myCommand.ExecuteReader();
                    while (myReader.Read())
                    {
                        CheckBox checkClaimType = FindControl("ctl00$MainContent$ClaimType_" + myReader["ID"].ToString()) as CheckBox;
                        checkClaimType.Style.Value = "background-color:pink";
                    }
                    myReader.Close();
                }
                else
                {
                    myReader = myCommand.ExecuteReader();
                    while (myReader.Read())
                    {
                        CheckBox checkClaimType = FindControl("ctl00$MainContent$ClaimType_" + myReader["ID"].ToString()) as CheckBox;
                        checkClaimType.Style.Value = "background-color:transparent";
                    }
                    myReader.Close();
                }
            }

            myReader.Close();
            myConnection.Close();
        }
        catch (Exception ex)
        {
            Resources.errorHandling(ex);
        }
    }

    protected void SaveExitButton_Click(object sender, EventArgs e)
    {
        try
        {
            SaveButton_Click(null, null);
            if (passValidation == 0)
            {
                Response.Redirect("Default.aspx", false);
            }
        }
        catch (Exception ex)
        {
            Resources.errorHandling(ex);
        }
    }

    protected void SaveButton_Click(object sender, EventArgs e)
    {
        try
        {
            // Do Validation of form
            int CurrentStep = Convert.ToInt32(CurrentStepVal.Value);
            passValidation = 0;
            switch (CurrentStep)
            {
                case 1:
                    checkTextValue(FirstNameInput);
                    checkTextValue(LastNameInput);
                    checkTextValue(DateReceivedInput);
                    checkTextValue(DateOfBirthInput);
                    checkDropDownValue(RoleInAccident);
                    CheckClaimTypeChecked();
                    break;
                case 2:
                    checkTextValue(AccidentDateInput);
                    break;
                case 3:
                    break;
            }

            Literal ErrMessage = new Literal();
            ErrMessage.Text = "<span style='vertical-align:middle;'><strong>Fields highlighted are Mandatory&nbsp;&nbsp;</strong><span>";
            ErrorMessages.Controls.Clear();
            if (passValidation == 0)
            {
                addSave();
                showDocuments();
            }
            else
            {
                ErrorMessages.Controls.Add(ErrMessage);
            }
        }
        catch (Exception ex)
        {
            Resources.errorHandling(ex);
        }
    }

    protected void StepButton_Click(object sender, EventArgs e)
    {
        try
        {
            // Do Validation of form
            int CurrentStep = Convert.ToInt32(CurrentStepVal.Value);
            passValidation = 0;
            switch (CurrentStep)
            {
                case 1:
                    checkTextValue(FirstNameInput);
                    checkTextValue(LastNameInput);
                    checkTextValue(DateReceivedInput);
                    checkTextValue(DateOfBirthInput);
                    checkDropDownValue(RoleInAccident);
                    CheckClaimTypeChecked();
                    break;
                case 2:
                    checkTextValue(AccidentDateInput);
                    break;
                case 3:
                    break;
            }

            Literal ErrMessage = new Literal();
            ErrMessage.Text = "<span style='vertical-align:middle;'><strong>Fields highlighted are Mandatory&nbsp;&nbsp;</strong><span>";

            ErrorMessages.Controls.Clear();

            if (passValidation == 0)
            {
                addSave();
                CurrentStep = CurrentStep + 1;
                CurrentStepVal.Value = Convert.ToString(CurrentStep);
                showStep(CurrentStep);
            }
            else
            {
                ErrorMessages.Controls.Add(ErrMessage);
            }
        }
        catch (Exception ex)
        {
            Resources.errorHandling(ex);
        }

    }

    public void checkTextValue(TextBox fieldEntry)
    {
        try
        {
            if (fieldEntry.Text == "")
            {
                passValidation = 1;
                fieldEntry.Style.Value = "background-color:pink";
            }
            else
            {
                fieldEntry.Style.Value = "background-color:white";
            }
        }
        catch (Exception ex)
        {
            Resources.errorHandling(ex);
        }
    }

    public void checkDropDownValue(DropDownList fieldEntry)
    {
        try
        {
            if (fieldEntry.SelectedValue == "0")
            {
                passValidation = 1;
                fieldEntry.Style.Value = "background-color:pink";
            }
            else
            {
                fieldEntry.Style.Value = "background-color:white";
            }
        }
        catch (Exception ex)
        {
            Resources.errorHandling(ex);
        }
    }

    public void GenerateNDNumber(String AllocateOrReject)
    {
        try
        {
            myConnection.Close();
            tmpLabel.Text = "";
            string CurrentClaimNumber = "";
            string NeworReplaceValue = "New";
            string OriginalClaimNumber = "";
            if (ClaimNumberVal.Value != "")
            {
                CurrentClaimNumber = ClaimNumberVal.Value;
            }

            if (CurrentClaimNumber != "")
            {
                Regex exp = new Regex("[R][0-9]*[A-Z]", RegexOptions.IgnoreCase);
                MatchCollection MatchList = exp.Matches(CurrentClaimNumber);
                if (MatchList.Count > 0)
                {
                    Match FirstMatch = MatchList[0];
                    if (FirstMatch.ToString() != "" && AllocateOrReject == "Allocate")
                    {
                        CurrentClaimNumber = "";
                        NeworReplaceValue = "Replace";
                        OriginalClaimNumber = ClaimNumberVal.Value;
                    }
                }
            }

            if (CurrentClaimNumber == "")
            {
                
                myConnection.Open();
                String mySQLUser = "Select * from [NDF].[dbo].[IDs]";
                SqlCommand myCommand2 = new SqlCommand(mySQLUser, myConnection);
                SqlDataReader myReader = null;
                myReader = myCommand2.ExecuteReader();
                myReader.Read();
                int id = 0;
                String ClaimNumber = "";
                String Numid = "";
                String mySQLUpdate = "";
                Numid = myReader["Id"].ToString();
                if (AllocateOrReject == "Allocate")
                {
                    id = Convert.ToInt32(myReader["NDID"].ToString());
                    
                    //work out letter value here
                    int z = 0;
                    int x = 6;
                    for (int i = 0; i < 5; i++)
                    {
                        z = z + (Convert.ToInt32(id.ToString().Substring(i, 1)) * x);
                        x--;
                    }

                    z = 13 - (z % 13) + 64;
                    char letVal = (char)z;

                    ClaimNumber = id.ToString() + letVal;
                    id = id + 1;
                    mySQLUpdate = "UPDATE [NDF].[dbo].[IDs] SET NDID ='" + id + "' WHERE ID='" + Numid + "'";
                }
                else
                {
                    id = Convert.ToInt32(myReader["RNDID"].ToString());

                    //work out letter value here
                    int z = 0;
                    int x = 6;
                    for (int i = 0; i < 5; i++)
                    {
                        z = z + (Convert.ToInt32(id.ToString().Substring(i, 1)) * x);
                        x--;
                    }

                    z = 13 - (z % 13) + 64;
                    char letVal = (char)z;

                    ClaimNumber = "R" + id.ToString() + letVal;
                    id = id + 1;
                    mySQLUpdate = "UPDATE [NDF].[dbo].[IDs] SET RNDID ='" + id + "' WHERE ID='" + Numid + "'";

                    //ClaimAgainstInput.SelectedIndex = 0; // this is to set the claim against field to be N/A

                }
                myReader.Close();

                SqlCommand myCommandID = new SqlCommand(mySQLUpdate, myConnection);
                myCommandID.ExecuteNonQuery();

                String mySQL2 = "UPDATE [NDF].[dbo].[NominalDefendants] SET ClaimNumber ='" + ClaimNumber + "', ClaimAgainstID='0' WHERE ID='" + NDID.Value + "'";

                SqlCommand myCommand = new SqlCommand(mySQL2, myConnection);
                myCommand.ExecuteNonQuery();
                myConnection.Close();

                ClaimNumberInput.Text = ClaimNumber;
                ClaimNumberVal.Value = ClaimNumber;

            }
        }
        catch (Exception ex)
        {
            Resources.errorHandling(ex);
        }
    }

    public void addSaveAllocated()
    {
        myConnection.Close();
        try
        {
            String AllocatedDate = null;
            myConnection.Open();
            if (AllocatedDateInput.Text != "") { AllocatedDate = swapDate(AllocatedDateInput.Text); }
            String mySQL = "UPDATE [NDF].[dbo].[NominalDefendants] SET AllocatedInsurerId = '" + AllocatedInsurerId.Value + "', AllocatedDate = '" + AllocatedDate + "' WHERE ID = '" + NDID.Value + "'";
            SqlCommand myCommand = new SqlCommand(mySQL, myConnection);
            myCommand.ExecuteNonQuery();
            myConnection.Close();
        }
        catch (Exception ex)
        {
            Resources.errorHandling(ex);
        }
    }

    public void addSave()
    {
        myConnection.Close();
        try
        {
            myConnection.Open();
            SqlDataReader myReader = null;

            // Get Claim Types
            String ClaimTypes = "";
            SqlCommand myCommandTypes = new SqlCommand("SELECT * FROM [NDF].[dbo].[ClaimType] Order By ID ASC", myConnection);
            myReader = myCommandTypes.ExecuteReader();
            while (myReader.Read())
            {
                String claimTypeVal = myReader["ID"].ToString();
                CheckBox checkType = FindControl("ctl00$MainContent$ClaimType_" + claimTypeVal) as CheckBox;
                if (checkType.Checked)
                {
                    ClaimTypes += claimTypeVal + "X";
                }
            }
            myReader.Close();
            myReader = null;

            if (NDID.Value == "")
            {
                // We need to create a record for this entry and get the ID and store it in the session. 

                String ClaimReceived = null;
                String AccidentDate = String.Empty;
                String AllocatedDate = null;
                String RejectionReasonVal = null;
                String DateSubpoenaConv = null;
                String DateStatmentClaimConv = null;
                String DateRejectedConv = null;
                String ClaimantDOB = null;
                String ClaimChecked = null;
                String claimStatus = null;

                if (DateOfBirthInput.Text != "") { ClaimantDOB = swapDate(DateOfBirthInput.Text); }
                if (DateRejected.Text != "") { DateRejectedConv = swapDate(DateRejected.Text); }

                if (DateReceivedInput.Text != "") { ClaimReceived = swapDate(DateReceivedInput.Text); }
                if (AccidentDateInput.Text != "") { AccidentDate = swapDate(AccidentDateInput.Text); }
                if (AllocatedDateInput.Text != "") { AllocatedDate = swapDate(AllocatedDateInput.Text); }

                if (dateChecked.Text != "") { ClaimChecked = swapDate(dateChecked.Text); }

                if (DateSubpoena.Text != "") { DateSubpoenaConv = swapDate(DateSubpoena.Text); }
                if (DateStatmentClaim.Text != "") { DateStatmentClaimConv = swapDate(DateStatmentClaim.Text); }

                if (RejectedCheckbox.Checked)
                {
                    RejectionReasonVal = RejectedReason.SelectedValue;
                }

                claimStatus = ClaimStatus.SelectedValue;

                String tmpAnomalyOther = "";
                tmpAnomalyOther = AnomalyOther.Text;

                String mySQL = "INSERT INTO [NDF].[dbo].[NominalDefendants](ClaimNumber,ClaimantFirstName,ClaimantLastName,ClaimReceived,ClaimLodgedById,LodgersRef,ClaimType,ClaimAgainstId,RoleInAccidentId,AnomalyId,AnomalyOther,AccidentDate,AccidentLocationId,AllocatedInsurerId,AllocatedDate,ClaimsOfficerId,InvestigationRequired,LateClaim,DateClosed,Payment_IssuerRef,Payment_PaymentsToDate,Payment_OutstandingEstimate,Rejected,RejectedReason,ClaimantDOB,DateSubpoena,DateStatmentClaim,RelatedRefNum,ClaimStatus,ClaimDocumentLoc,ClaimCheckedBy,ClaimCheckedDate,DateRejected) VALUES ('" + ClaimNumberInput.Text + "','" + FirstNameInput.Text.Replace("'", "''") + "','" + LastNameInput.Text.Replace("'", "''") + "','" + ClaimReceived + "','" + LoggedById.Value + "','" + LodgersRef.Text.Replace("'", "''") + "', '" + ClaimTypes + "','" + ClaimAgainstInput.SelectedValue + "','" + RoleInAccident.SelectedValue + "','" + AccidentAnomaly.SelectedValue + "','" + AnomalyOther.Text.Replace("'", "''") + "','" + AccidentDate + "','" + LocationId.Value + "','" + AllocatedInsurerId.Value + "','" + AllocatedDate + "','" + ClaimsOfficer.SelectedValue + "','" + MAAInvestigationYes.Checked + "', '" + LateClaim.Checked + "', '" + DateClosed.Text + "', '" + Payment_IssuerRef.Text.Replace("$", "") + "', '" + Payment_PaymentsToDate.Text.Replace("$", "") + "', '" + Payment_OutstandingEstimate.Text.Replace("$", "") + "', '" + RejectedCheckbox.Checked + "', '" + RejectionReasonVal + "', '" + ClaimantDOB + "', '" + DateSubpoenaConv + "', '" + DateStatmentClaimConv + "', '" + RelatedRefNum.Text.Replace("'", "''") + "', '" + claimStatus + "', '" + ClaimDocumentLoc.Text.Replace("'", "''") + "','" + CheckByOfficer.SelectedValue + "','" + ClaimChecked + "', '" + DateRejectedConv + "')";

                SqlCommand myCommand = new SqlCommand(mySQL, myConnection);
                myCommand.ExecuteNonQuery();

                SqlCommand myCommand2 = new SqlCommand("SELECT @@IDENTITY AS 'Identity' FROM [NDF].[dbo].[NominalDefendants]", myConnection);
                myReader = myCommand2.ExecuteReader();
                myReader.Read();
                //ClaimNumberInput.Text = myReader["Identity"].ToString();
                NDID.Value = myReader["Identity"].ToString();
                myReader.Close();
            }
            else
            {
                String ClaimReceived = null;
                String AccidentDate = null;
                String DateRejectedConv = null;
                String AllocatedDate = null;
                String CloseDate = null;
                String RejectionReasonVal = null;
                String DateSubpoenaConv = null;
                String DateStatmentClaimConv = null;
                String ClaimantDOB = null;
                String ClaimChecked = null;

                if (DateOfBirthInput.Text != "") { ClaimantDOB = swapDate(DateOfBirthInput.Text); }
                if (DateRejected.Text != "") { DateRejectedConv = swapDate(DateRejected.Text); }

                if (DateReceivedInput.Text != "") { ClaimReceived = swapDate(DateReceivedInput.Text); }
                if (AccidentDateInput.Text != "") { AccidentDate = swapDate(AccidentDateInput.Text); }
                if (AllocatedDateInput.Text != "") { AllocatedDate = swapDate(AllocatedDateInput.Text); }
                if (DateClosed.Text != "") { CloseDate = swapDate(DateClosed.Text); }

                if (dateChecked.Text != "") { ClaimChecked = swapDate(dateChecked.Text); }

                if (DateSubpoena.Text != "") { DateSubpoenaConv = swapDate(DateSubpoena.Text); }
                if (DateStatmentClaim.Text != "") { DateStatmentClaimConv = swapDate(DateStatmentClaim.Text); }

                if (RejectedCheckbox.Checked)
                {
                    RejectionReasonVal = RejectedReason.SelectedValue;
                }

                String mySQL = "UPDATE [NDF].[dbo].[NominalDefendants] SET ClaimantFirstName = '" + FirstNameInput.Text.Replace("'", "''") + "', ClaimantLastName = '" + LastNameInput.Text.Replace("'", "''") + "', ClaimReceived = '" + ClaimReceived + "', ClaimLodgedById = '" + LoggedById.Value + "', LodgersRef='" + LodgersRef.Text.Replace("'", "''") + "',  ClaimType = '" + ClaimTypes + "', ClaimAgainstId = '" + ClaimAgainstInput.SelectedValue + "', RoleInAccidentId = '" + RoleInAccident.SelectedValue + "' , AnomalyId = '" + AccidentAnomaly.SelectedValue + "' , AnomalyOther='" + AnomalyOther.Text.Replace("'", "''") + "', AccidentDate = '" + AccidentDate + "', AccidentLocationId = '" + LocationId.Value + "', AllocatedInsurerId = '" + AllocatedInsurerId.Value + "', AllocatedDate = '" + AllocatedDate + "', ClaimsOfficerId = '" + ClaimsOfficer.Text + "', InvestigationRequired = '" + MAAInvestigationYes.Checked + "', LateClaim='" + LateClaim.Checked + "', ClaimStatus = '" + ClaimStatus.SelectedValue + "', DateClosed = '" + CloseDate + "', CloseType = '" + HiddenCloseType.Value + "', Payment_IssuerRef = '" + Payment_IssuerRef.Text.Replace("'", "''") + "', Payment_PaymentsToDate = '" + Payment_PaymentsToDate.Text.Replace("$", "") + "', Payment_OutstandingEstimate = '" + Payment_OutstandingEstimate.Text.Replace("$", "") + "', Rejected='" + RejectedCheckbox.Checked + "', RejectedReason='" + RejectionReasonVal + "', ClaimantDOB ='" + ClaimantDOB + "', DateSubpoena ='" + DateSubpoenaConv + "', DateStatmentClaim ='" + DateStatmentClaimConv + "', RelatedRefNum = '" + RelatedRefNum.Text.Replace("'", "''") + "', ClaimDocumentLoc = '" + ClaimDocumentLoc.Text + "', ClaimCheckedBy='" + HiddenCheckByOfficer.Value + "', ClaimCheckedDate = '" + ClaimChecked + "', DateRejected='" + DateRejectedConv + "' WHERE ID = '" + NDID.Value + "'";

                SqlCommand myCommand = new SqlCommand(mySQL, myConnection);
                myCommand.ExecuteNonQuery();
            }

            myConnection.Close();

        }
        catch (Exception ex)
        {
            Resources.errorHandling(ex);
        }

    }

    public void checkValidDateFormatRego(object sender, EventArgs e)
    {
        try
        {
            TextBox dateValue = (TextBox)sender;

            if (dateValue.Text != "")
            {
                DateTime todayDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day);
                CultureInfo provider = new CultureInfo("en-AU");
                Label errLabel = (Label)PageDisplay.FindControl(dateValue.Attributes["labelParam"].ToString());
                DateTime outDate;

                string[] formats = { "dd/MM/yyyy", "d/M/yyyy", "dd/M/yyyy", "d/MM/yyyy", "d/M/yy", "dd/M/yy", "dd/MM/yy", "d/MM/yy" };
                //string[] formats = { "dd/MM/yyyy" };
                if (!DateTime.TryParseExact(dateValue.Text, formats, provider, DateTimeStyles.None, out outDate))
                {
                    dateValue.Style.Value = "background-color:pink";
                    errLabel.Text = " Date is not in correct format";
                    dateValue.Text = "";
                }
                else
                {
                    dateValue.Style.Value = "background-color:white";
                    errLabel.Text = "";
                }

            }

            int carId = Convert.ToInt32(dateValue.ID.ToString().Replace("CarRegistrationExpiryInput_", ""));
            TableRow rowEdit = (TableRow)PageDisplay.FindControl("rowEdit_" + carId.ToString());
            TableRow rowDisp = (TableRow)PageDisplay.FindControl("rowDisp_" + carId.ToString());

            rowEdit.Style.Value += "display:block";
            rowDisp.Style.Value += "display:none";
        }
        catch (Exception ex)
        {
            Resources.errorHandling(ex);
        }

    }

    public void checkValidDateFormatAllowFuture(object sender, EventArgs e)
    {
        try
        {
            TextBox dateValue = (TextBox)sender;

            if (dateValue.Text != "")
            {
                DateTime todayDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day);
                CultureInfo provider = new CultureInfo("en-AU");
                Label errLabel = (Label)PageDisplay.FindControl(dateValue.Attributes["labelParam"].ToString());
                DateTime outDate;

                string[] formats = { "dd/MM/yyyy", "d/M/yyyy", "dd/M/yyyy", "d/MM/yyyy", "d/M/yy", "dd/M/yy", "dd/MM/yy", "d/MM/yy" };
                //string[] formats = { "dd/MM/yyyy" };
                if (!DateTime.TryParseExact(dateValue.Text, formats, provider, DateTimeStyles.None, out outDate))
                {
                    dateValue.Style.Value = "background-color:pink";
                    errLabel.Text = " Date is not in correct format";
                    dateValue.Text = "";
                }
                else
                {
                    dateValue.Style.Value = "background-color:white";
                    errLabel.Text = "";
                }

                if (dateValue.ID == "CarRegistrationExpiryInput")
                {
                    AddVehicleShowHide_Click(null, null);
                }

            }
        }
        catch (Exception ex)
        {
            Resources.errorHandling(ex);
        }

    }

    public void checkValidDateFormatLong(object sender, EventArgs e)
    {
        try
        {
            TextBox dateValue = (TextBox)sender;

            if (dateValue.Text != "")
            {
                DateTime todayDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day);
                CultureInfo provider = new CultureInfo("en-AU");
                Label errLabel = (Label)PageDisplay.FindControl(dateValue.Attributes["labelParam"].ToString());
                DateTime outDate;

                string[] formats = { "dd/MM/yyyy", "d/M/yyyy", "dd/M/yyyy", "d/MM/yyyy"};
                if (!DateTime.TryParseExact(dateValue.Text, formats, provider, DateTimeStyles.None, out outDate))
                {
                    dateValue.Style.Value = "background-color:pink";
                    errLabel.Text = " Date is not in correct format. Must be dd/mm/yyyy";
                    dateValue.Text = "";
                }
                else
                {
                    if (outDate > todayDate)
                    {
                        dateValue.Style.Value = "background-color:pink";
                        errLabel.Text = " Date cannot be in the future";
                        dateValue.Text = "";
                    }
                    else
                    {
                        dateValue.Style.Value = "background-color:white";
                        errLabel.Text = "";
                    }

                }

            }
        }
        catch (Exception ex)
        {
            Resources.errorHandling(ex);
        }

    }

    public void checkValidDateFormat(object sender, EventArgs e)
    {
        try
        {
            TextBox dateValue = (TextBox)sender;

            if (dateValue.Text != "")
            {
                DateTime todayDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day);
                CultureInfo provider = new CultureInfo("en-AU");
                Label errLabel = (Label)PageDisplay.FindControl(dateValue.Attributes["labelParam"].ToString());
                DateTime outDate;

                string[] formats = { "dd/MM/yyyy", "d/M/yyyy", "dd/M/yyyy", "d/MM/yyyy", "d/M/yy", "dd/M/yy", "dd/MM/yy", "d/MM/yy" };
                if (!DateTime.TryParseExact(dateValue.Text, formats, provider, DateTimeStyles.None, out outDate))
                {
                    dateValue.Style.Value = "background-color:pink";
                    errLabel.Text = " Date is not in correct format";
                    dateValue.Text = "";
                }
                else
                {
                    if (outDate > todayDate)
                    {
                        dateValue.Style.Value = "background-color:pink";
                        errLabel.Text = " Date cannot be in the future";
                        dateValue.Text = "";
                    }
                    else
                    {
                        dateValue.Style.Value = "background-color:white";
                        errLabel.Text = "";

                        if (dateValue.ID == "AccidentDateInput")
                        {
                            checkAccidentDate(null, null);
                        }

                    }

                }

            }
        }
        catch (Exception ex)
        {
            Resources.errorHandling(ex);
        }

    }

    public void checkAccidentDate(object sender, EventArgs e)
    {
        try
        {
            if (AccidentDateInput.Text != "")
            {
                CultureInfo provider = new CultureInfo("en-AU");
                DateTime accDate = DateTime.Parse(AccidentDateInput.Text, provider);
                DateTime recDate = DateTime.Parse(DateReceivedInput.Text, provider);

                if (accDate > recDate)
                {
                    AccidentDateInput.Style.Value = "background-color:pink";
                    AccDateLabel.Text = "   Accident Date must occur before the date the claim was received - Claim received on: " + DateReceivedInput.Text;
                    AccidentDateInput.Text = "";
                }
                else
                {
                    AccidentDateInput.Style.Value = "background-color:white";
                    AccDateLabel.Text = "";
                }

                addSave();
            }
        }
        catch (Exception ex)
        {
            Resources.errorHandling(ex);
        }
    }

    protected void AddComment_Click(object sender, EventArgs e)
    {
        myConnection.Close();
        try
        {
            myConnection.Open();
            String CommentDate = null;
            DateTime dt = DateTime.Now;
            if (CommentActionDate.Text != "")
            {
                CommentDate = swapDate(CommentActionDate.Text);
                CommentDate = CommentDate + String.Format(" {0:HH:mm:ss}", dt);
            }
            else
            {
                CommentDate = String.Format("{0:yyyy/MM/dd HH:mm:ss}", dt);
            }

            String mySQL = "INSERT INTO [NDF].[dbo].[NominalNotes](Notes,Ndid,NoteDate,Userid) VALUES ('" + CommentsActionsInput.Text.Replace("'", "''") + "','" + NDID.Value + "','" + CommentDate + "', '" + Page.User.Identity.Name.ToString() +"')";
            SqlCommand myCommand = new SqlCommand(mySQL, myConnection);
            myCommand.ExecuteNonQuery();
            myConnection.Close();

            CommentsActionsInput.Text = "";
            showNotes();

        }
        catch (Exception ex)
        {
            Resources.errorHandling(ex);
        }
    }

    protected void SetClosed(object sender, EventArgs e)
    {
        try
        {
            HiddenClaimStatus.Value = ClaimStatus.SelectedValue;
        }
        catch (Exception ex)
        {
            Resources.errorHandling(ex);
        }
    }

    protected void SetCloseType(object sender, EventArgs e)
    {
        try
        {
            HiddenCloseType.Value = CloseType.SelectedValue;
        }
        catch (Exception ex)
        {
            Resources.errorHandling(ex);
        }
    }

    protected void SetRejected(object sender, EventArgs e)
    {
        try
        {
            if (RejectedCheckbox.Checked)
            {
                ClaimAgainstInput.SelectedIndex = 0; // this is to set the claim against field to be N/A
                
                DateTime dt = DateTime.Now;
                if (DateRejected.Text == "")
                {
                    DateRejected.Text = dt.Day + "/" + dt.Month + "/" + dt.Year;
                }

                addSave();
                RejectedReason.Enabled = true;
                AllocateButton.Enabled = false;
                ManualAllocateButton.Enabled = false;
                DateRejected.Enabled = true;
                GenerateNDNumber("Rejected");
            }
            else
            {
                DateRejected.Text = "";
                addSave();
                RejectedReason.Enabled = false;
                AllocateButton.Enabled = true;
                ManualAllocateButton.Enabled = true;
                DateRejected.Enabled = false;
                UnAllocateButton_Click(null, null);
            }
        }
        catch (Exception ex)
        {
            Resources.errorHandling(ex);
        }
    }

    public String displayDate(String dateToChange)
    {
        try
        {
            DateTime Displaydate = DateTime.Parse(dateToChange);
            String dateToReturn = Displaydate.Day.ToString() + "/" + Displaydate.Month.ToString() + "/" + Displaydate.Year.ToString();
            return dateToReturn;
        }
        catch (Exception ex)
        {
            Resources.errorHandling(ex);
            return null;
        }
    }

    public String swapDate(String dateToChange)
    {
        try
        {
            if (dateToChange != null)
            {
                CultureInfo provider = new CultureInfo("en-AU");
                DateTime Displaydate = DateTime.Parse(dateToChange, provider);
                String dateToReturn = Displaydate.Year.ToString() + "/" + Displaydate.Month.ToString() + "/" + Displaydate.Day.ToString();
                return dateToReturn;
            }
            else
            {
                return null;
            }
        }
        catch (Exception ex)
        {
            Resources.errorHandling(ex);
            return null;
        }

    }

    public void SetDefaultClaimsOfficer()
    {
        myConnection.Close();
        try
        {

            string windowsLogin = Page.User.Identity.Name.ToString();
            int hasDomain = windowsLogin.IndexOf(@"\");

            if (hasDomain > 0)
            {
                windowsLogin = windowsLogin.Remove(0, hasDomain + 1);
            }

            myConnection.Open();
            String mySQLUser = "Select * from [NDF].[dbo].[Users] WHERE Username = '" + windowsLogin + "'";
            SqlCommand myCommand = new SqlCommand(mySQLUser, myConnection);
            SqlDataReader myReader = null;
            myReader = myCommand.ExecuteReader();
            myReader.Read();
            ClaimsOfficer.SelectedValue = myReader["Id"].ToString();
            myConnection.Close();
        }
        catch (Exception ex)
        {
            Resources.errorHandling(ex);
        }

        
    }

}