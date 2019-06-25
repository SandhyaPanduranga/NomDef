using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Globalization;
using NominalDefendant;

public partial class Test : System.Web.UI.Page
{

    SqlConnection myConnection = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["NominalConn"].ConnectionString);
    
    protected void Page_Load(object sender, EventArgs e)
    {
        
    }

    protected void ButtonSearch_Click(object sender, EventArgs e)
    {

        try
        {

            if 
                 (AccidentFromDate.Text ==""  ||  AccidentToDate.Text == "")        
                {
                tmpLabel.Text = "<strong>Please enter both From and To dates</strong>";
            }
            else
            {
                tmpLabel.Text = "";
                myConnection.Close();
                myConnection.Open();

                decimal TotalClaims = 0;

                SqlDataReader myReaderCount = null;
                SqlDataReader myReader = null;

                CultureInfo provider = new CultureInfo("en-AU");
                DateTime AccidentFrom = DateTime.Parse(AccidentFromDate.Text, provider);
                DateTime AccidentTo = DateTime.Parse(AccidentToDate.Text, provider);

                String AccidentFromText = AccidentFrom.Month.ToString() + "/" + AccidentFrom.Day.ToString() + "/" + AccidentFrom.Year.ToString();
                String AccidentToText = AccidentTo.Month.ToString() + "/" + AccidentTo.Day.ToString() + "/" + AccidentTo.Year.ToString();

                SqlCommand myCommand = new SqlCommand("SELECT ND.ClaimantFirstName + ' ' + ND.ClaimantLastName as Claimant,ND.ClaimNumber as NDReferenceNo, ND.AccidentDate as DateofAccident, Ins1.InsurerName as NSWManagingInsurerName, UPPER(VehInv.RegistrationNumber) as RegistrationNo,AUS.State as RegistrationState,Ins2.InsurerName as InterstateCTPInsurerName  FROM NDF.dbo.NominalDefendants ND,NDF.dbo.VehiclesInvolved VehInv,NDF.dbo.Insurers Ins1,NDF.dbo.Insurers Ins2,NDF.dbo.AustralianStates AUS WHERE ND.AllocatedInsurerId = Ins1.InsurerId AND ND.ID = VehInv.Ndid AND AUS.ID = VehInv.RegistrationState AND VehInv.InsurerId = Ins2.InsurerId AND VehInv.RegistrationState not in (1, 2) AND VehInv.AtFault = 'True' AND ND.AccidentDate >= '" + AccidentFromText + "' AND ND.AccidentDate <= '" + AccidentToText + "' ORDER BY dateofAccident,Claimant", myConnection);
                SqlCommand myCommandCount = new SqlCommand("SELECT COUNT(*) as NumberofClaims FROM NDF.dbo.NominalDefendants ND,NDF.dbo.VehiclesInvolved VehInv,NDF.dbo.Insurers Ins1,NDF.dbo.Insurers Ins2,NDF.dbo.AustralianStates AUS WHERE ND.AllocatedInsurerId = Ins1.InsurerId AND ND.ID = VehInv.Ndid AND AUS.ID = VehInv.RegistrationState AND VehInv.InsurerId = Ins2.InsurerId AND VehInv.RegistrationState not in (1, 2) AND VehInv.AtFault = 'True' AND ND.AccidentDate >= '" + AccidentFromText + "' AND ND.AccidentDate <= '" + AccidentToText + "' ", myConnection);

                
                myReader = myCommand.ExecuteReader();
                myReaderCount = myCommandCount.ExecuteReader();

                bool rowCount = myReader.HasRows;
                if (rowCount)
                {
                    TestPlaceholder.Controls.Clear();
                   
                    Label TitleLabel = new Label();
                    TitleLabel.Text = "<strong> Interstate Claims</strong><br /><br />";
                    TitleLabel.Text += "Claims Accident between " + AccidentFrom.Day.ToString() + "/" + AccidentFrom.Month.ToString() + "/" + AccidentFrom.Year.ToString() + " and " + AccidentTo.Day.ToString() + "/" + AccidentTo.Month.ToString() + "/" + AccidentTo.Year.ToString() + "<br /><br />";
                    TestPlaceholder.Controls.Add(TitleLabel);
                    
                    myReaderCount.Read();
                    TotalClaims = Convert.ToDecimal(myReaderCount["NumberClaims"]);
                    

                    Table table = new Table();
                    table.CellPadding = 8;
                    table.CellSpacing = 3;

                    TableRow rowTitle = new TableRow();
                    rowTitle.ID = "RowTitle";
                    rowTitle.Height = 28;

                    TableCell cell1Title = new TableCell(); // Claimant FirstName
                    TableCell cell2Title = new TableCell(); // Claimant LastName
                    TableCell cell3Title = new TableCell(); // ND Reference Number
                    TableCell cell4Title = new TableCell(); // Date of Accident
                    TableCell cell5Title = new TableCell(); // NSW ManagingInsurerName
                    TableCell cell6Title = new TableCell(); // Registration No
                    TableCell cell7Title = new TableCell(); // Interstate CTP InsurerName

                    Literal cell1TitleText = new Literal();
                    Literal cell2TitleText = new Literal();
                    Literal cell3TitleText = new Literal();
                    Literal cell4TitleText = new Literal();
                    Literal cell5TitleText = new Literal();
                    Literal cell6TitleText = new Literal();
                    Literal cell7TitleText = new Literal();

                    cell1TitleText.Text = "<strong>Claimant First Name</strong>";
                    cell1Title.Controls.Add(cell1TitleText);
                    cell1Title.Width = 200;
                    cell1Title.Wrap = false;
                    cell1Title.CssClass = "tableTitle";
                    rowTitle.Cells.Add(cell1Title);

                    cell2TitleText.Text = "<strong>Claimant Last Name</strong>";
                    cell2Title.Controls.Add(cell2TitleText);
                    cell2Title.Width = 150;
                    cell2Title.CssClass = "tableTitle";
                    rowTitle.Cells.Add(cell2Title);

                    cell3TitleText.Text = "<strong>ND Reference Number</strong>";
                    cell3Title.Controls.Add(cell3TitleText);
                    cell3Title.Width = 150;
                    cell3Title.CssClass = "tableTitle";
                    rowTitle.Cells.Add(cell3Title);

                    cell4TitleText.Text = "<strong>Date of Accident</strong>";
                    cell4Title.Controls.Add(cell4TitleText);
                    cell4Title.Width = 150;
                    cell4Title.CssClass = "tableTitle";
                    rowTitle.Cells.Add(cell4Title);

                    cell5TitleText.Text = "<strong>NSW ManagingInsurerName</strong>";
                    cell5Title.Controls.Add(cell5TitleText);
                    cell5Title.Width = 150;
                    cell5Title.CssClass = "tableTitle";
                    rowTitle.Cells.Add(cell5Title);

                    cell6TitleText.Text = "<strong>Registration No</strong>";
                    cell6Title.Controls.Add(cell6TitleText);
                    cell6Title.Width = 150;
                    cell6Title.CssClass = "tableTitle";
                    rowTitle.Cells.Add(cell6Title);

                    cell7TitleText.Text = "<strong>Interstate CTP InsurerName</strong>";
                    cell7Title.Controls.Add(cell6TitleText);
                    cell7Title.Width = 150;
                    cell7Title.CssClass = "tableTitle";
                    rowTitle.Cells.Add(cell7Title);

                    table.Rows.Add(rowTitle);

                    while (myReader.Read())
                    {

                        // This is the display row.
                        TableRow rowDisp = new TableRow();
                        rowDisp.ID = "rowDisp";
                        rowDisp.Height = 28;
                        TableCell cell1Disp = new TableCell();
                        TableCell cell2Disp = new TableCell();
                        TableCell cell3Disp = new TableCell();
                        TableCell cell4Disp = new TableCell();
                        TableCell cell5Disp = new TableCell();
                        TableCell cell6Disp = new TableCell();
                        TableCell cell7Disp = new TableCell();

                        Literal cell1DispText = new Literal();
                        cell1DispText.Text = myReader["ClaimantFirstName"].ToString();
                        cell1Disp.Style.Value = "text-transform:uppercase";
                        cell1Disp.Controls.Add(cell1DispText);
                        cell1Disp.CssClass = "tableField";
                        rowDisp.Cells.Add(cell1Disp);

                        Literal cell2DispText = new Literal();
                        cell2DispText.Text = myReader["ClaimantLastName"].ToString();
                        cell2Disp.Controls.Add(cell2DispText);
                        cell2Disp.CssClass = "tableField";
                        rowDisp.Cells.Add(cell2Disp);

                        Literal cell3DispText = new Literal();
                        cell3DispText.Text = myReader["NDReferenceNo"].ToString();
                        cell3Disp.Controls.Add(cell3DispText);
                        cell3Disp.CssClass = "tableField";
                        rowDisp.Cells.Add(cell3Disp);


                        Literal cell4DispText = new Literal();
                        cell4DispText.Text = myReader["DateofAccident"].ToString();
                        cell4Disp.Controls.Add(cell4DispText);
                        cell4Disp.CssClass = "tableField";
                        rowDisp.Cells.Add(cell4Disp);

                        Literal cell5DispText = new Literal();
                        cell5DispText.Text = myReader["NSWManagingInsurerName"].ToString();
                        cell5Disp.Controls.Add(cell5DispText);
                        cell5Disp.CssClass = "tableField";
                        rowDisp.Cells.Add(cell5Disp);

                        Literal cell6DispText = new Literal();
                        cell6DispText.Text = myReader["RegistrationNo"].ToString();
                        cell6Disp.Controls.Add(cell6DispText);
                        cell6Disp.CssClass = "tableField";
                        rowDisp.Cells.Add(cell6Disp);

                        Literal cell7DispText = new Literal();
                        cell7DispText.Text = myReader["RegistrationNo"].ToString();
                        cell7Disp.Controls.Add(cell7DispText);
                        cell7Disp.CssClass = "tableField";
                        rowDisp.Cells.Add(cell7Disp);

                        table.Rows.Add(rowDisp);

                    }

                    TableRow rowTotal = new TableRow();
                    rowTotal.ID = "RowTotal";
                    rowTotal.Height = 28;

                    TableCell cell1Total = new TableCell(); 
                    TableCell cell2Total = new TableCell();
                    TableCell cell3Total = new TableCell();
                    TableCell cell4Total = new TableCell();
                    TableCell cell5Total = new TableCell();
                    TableCell cell6Total = new TableCell();
                    TableCell cell7Total = new TableCell();


                    Literal cell1TotalText = new Literal();
                    Literal cell2TotalText = new Literal();
                    Literal cell3TotalText = new Literal();
                    Literal cell4TotalText = new Literal();
                    Literal cell5TotalText = new Literal();
                    Literal cell6TotalText = new Literal();
                    Literal cell7TotalText = new Literal();

                    cell1TotalText.Text = "<strong>Totals for Period</strong>";
                    cell1Total.Controls.Add(cell1TotalText);
                    cell1Total.Width = 200;
                    cell1Total.Wrap = false;
                    cell1Total.CssClass = "tableTitle";
                    rowTotal.Cells.Add(cell1Total);

                    cell2TotalText.Text = TotalClaims.ToString();
                    cell2Total.Controls.Add(cell2TotalText);
                    cell2Total.Width = 150;
                    cell2Total.CssClass = "tableTitle";
                    rowTotal.Cells.Add(cell2Total);

                    cell3TotalText.Text = " ";
                    cell3Total.Controls.Add(cell3TotalText);
                    cell3Total.Width = 150;
                    cell3Total.CssClass = "tableTitle";
                    rowTotal.Cells.Add(cell3Total);

                    cell4TotalText.Text = " ";
                    cell4Total.Controls.Add(cell4TotalText);
                    cell4Total.Width = 150;
                    cell4Total.CssClass = "tableTitle";
                    rowTotal.Cells.Add(cell4Total);

                    cell5TotalText.Text = " ";
                    cell5Total.Controls.Add(cell5TotalText);
                    cell5Total.Width = 150;
                    cell5Total.CssClass = "tableTitle";
                    rowTotal.Cells.Add(cell5Total);

                    cell6TotalText.Text = " ";
                    cell6Total.Controls.Add(cell6TotalText);
                    cell6Total.Width = 150;
                    cell6Total.CssClass = "tableTitle";
                    rowTotal.Cells.Add(cell6Total);

                    cell7TotalText.Text = " ";
                    cell7Total.Controls.Add(cell7TotalText);
                    cell7Total.Width = 150;
                    cell7Total.CssClass = "tableTitle";
                    rowTotal.Cells.Add(cell7Total);

                    table.Rows.Add(rowTotal);

                    TestPlaceholder.Controls.Add(table);
                }
                myReaderCount.Close();
                myReader.Close();
                myConnection.Close();

            }
        }
        catch (Exception ex)
        {
            Resources.errorHandling(ex);
        }

    }

}