using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using NominalDefendant;

public partial class ClaimAdministration : System.Web.UI.Page
{
    SqlConnection myConnection = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["NominalConn"].ConnectionString);    

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (Session["Admin"] != null && Session["Admin"].ToString() == "Y")
            {
                // allow user to remain on screen
            }
            else
            {
                Response.Redirect("Default.aspx?Err=NoAdmin");
            }
        }
        catch (Exception ex)
        {
            Resources.errorHandling(ex);
        }
    }

    public void deleteClaim(int claimId)
    {
        myConnection.Close();
        try
        {
            myConnection.Open();
            String mySQL = "";

            // Delete from NominalDefendants where ID = ''
            // Delete from VehiclesInvolved where Ndid = ''
            // Delete from NominalNotes where Ndid = ''

            SqlCommand myCommand = new SqlCommand(mySQL, myConnection);
            myCommand.ExecuteNonQuery();
            myConnection.Close();
        }
        catch (Exception ex)
        {
            Resources.errorHandling(ex);
        }
    }

}