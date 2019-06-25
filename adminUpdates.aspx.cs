using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using NominalDefendant;

public partial class adminUpdates : System.Web.UI.Page
{
    SqlConnection myConnection = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["NominalConn"].ConnectionString);

    protected void Page_Load(object sender, EventArgs e)
    {

        try
        {
            String updateType = Request["type"];

            if (Session["Admin"] != null && Session["Admin"].ToString() == "Y")
            {
                // allow users to remain on page
                showValues(updateType);
                PageTitle.Text = updateType;
            }
            else
            {
                Response.Redirect("Default.aspx");
            }
        }
        catch (Exception ex)
        {
            Resources.errorHandling(ex);
        }

        
    }

    protected void showValues(String updateType)
    {

        try
        {
            myConnection.Close();
            myConnection.Open();

            // String structure = Table Name XX ID Field XX Display Name

            String Anomalies = "AnomaliesXIdXAnomaly";
            String ClaimAgainst = "ClaimAgainstXIdXClaimAgainst";
            String ClaimStatus = "ClaimStatusXIdXClaimStatus";
            String ClaimType = "ClaimTypeXIdXClaimType";
            String CloseType = "CloseTypeXIdXCloseType";
            String RejectionReason = "RejectionReasonsXIdXRejectionReason";
            String RoleInAccident = "RoleInAccidentXIDXRoleInAccident";

            int arrPos = 0;
            switch (updateType)
            {
                case "Anomalies":
                    arrPos = 0;
                    break;
                case "ClaimAgainst":
                    arrPos = 1;
                    break;
                case "ClaimStatus":
                    arrPos = 2;
                    break;
                case "ClaimType":
                    arrPos = 3;
                    break;
                case "CloseType":
                    arrPos = 4;
                    break;
                case "RejectionReason":
                    arrPos = 5;
                    break;
                case "RoleInAccident":
                    arrPos = 6;
                    break;
            }

            string[] arr1 = new string[] { Anomalies, ClaimAgainst, ClaimStatus, ClaimType, CloseType, RejectionReason, RoleInAccident };
            string[] arr2 = arr1[arrPos].Split('X');

            String mySQL = "Select * FROM [NDF].[dbo].[" + arr2[0] + "] Order By " + arr2[2] + " ASC";


            SqlDataReader myReader = null;
            SqlCommand myCommand = new SqlCommand(mySQL, myConnection);
            myReader = myCommand.ExecuteReader();
            bool rowCount = myReader.HasRows;
            if (rowCount)
            {
                AdminPlaceholder.Controls.Clear();

                Table table = new Table();
                table.CellPadding = 4;
                table.CellSpacing = 0;

                while (myReader.Read())
                {
                    // This is the display row.
                    TableRow rowDisp = new TableRow();
                    rowDisp.Height = 28;
                    TableCell cell1Disp = new TableCell(); // Username
                    TableCell cell4Disp = new TableCell(); // Buttons  

                    Literal cell1DispText = new Literal();
                    cell1DispText.Text = myReader[arr2[2]].ToString();
                    cell1Disp.Controls.Add(cell1DispText);
                    rowDisp.Cells.Add(cell1Disp);

                    Literal cell4DispText = new Literal();
                    //cell4DispText.Text = "<input type='button' value='Delete' name='DeleteUserButton' class='buttonCSSNarrow' onclick='deleteUser(\"" + myReader["Id"].ToString() + "\")' />";
                    cell4DispText.Text = "";
                    cell4Disp.Controls.Add(cell4DispText);
                    rowDisp.Cells.Add(cell4Disp);

                    rowDisp.Visible = true;
                    table.Rows.Add(rowDisp);
                }
                AdminPlaceholder.Controls.Add(table);
                myReader.Close();
            }
            myConnection.Close();
        }
        catch (Exception ex)
        {
            Resources.errorHandling(ex);
        }

    }
 
}