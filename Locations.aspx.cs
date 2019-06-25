using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using NominalDefendant;

public partial class Locations : System.Web.UI.Page
{

    SqlConnection myConnection = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["NominalConn"].ConnectionString);
    
    protected void Page_Load(object sender, EventArgs e)
    {
        SuburbInput.Focus();
    }

    protected void StartSearchbutton_Click(object sender, EventArgs ev)
    {

        try
        {
            Literal Message = new Literal();
            if (SuburbInput.Text == "" && PostcodeInput.Text == "")
            {
                // no input provided - add warning message
                Message.Text = "<strong>Please enter some search criteria</strong>";
                LocationResultsPlaceholder.Controls.Add(Message);
            }
            else
            {


                myConnection.Open();
                SqlDataReader myReader = null;
                Literal sqlQuery = new Literal();
                sqlQuery.Text = "Select * from Locations";

                int whereStarted = 0;
                if (SuburbInput.Text != "")
                {
                    sqlQuery.Text = sqlQuery.Text + " WHERE Suburb like '%" + SuburbInput.Text + "%'";
                    whereStarted = 1;
                }
                if (PostcodeInput.Text != "")
                {
                    if (whereStarted == 1)
                    {
                        sqlQuery.Text = sqlQuery.Text + " AND Postcode like '%" + PostcodeInput.Text + "%'";
                    }
                    else
                    {
                        sqlQuery.Text = sqlQuery.Text + " WHERE Postcode like '%" + PostcodeInput.Text + "%'";
                    }
                }

                sqlQuery.Text = sqlQuery.Text + " ORDER BY Suburb ASC";

                //LocationResultsPlaceholder.Controls.Add(sqlQuery);

                SqlCommand myCommand = new SqlCommand(sqlQuery.Text, myConnection);
                myReader = myCommand.ExecuteReader();
                Message.Text = "<center><table cellpassing='5' cellspacing='5'><tr><td align=left><strong>Postcode</strong></td><td align=left><strong>Suburb</strong></td></tr>";
                while (myReader.Read())
                {
                    Message.Text = Message.Text + "<tr style='cursor:pointer' onclick='populateSuburb(" + myReader["Postcode"].ToString() + ", \"" + myReader["Suburb"].ToString() + "\", \"" + myReader["Id"].ToString() + "\")'><td align=left style='color:White; font-weight:bold'>" + myReader["Postcode"].ToString() + "</td><td align=left style='color:White; font-weight:bold'>" + myReader["Suburb"].ToString() + "</td></tr>";
                }
                Message.Text = Message.Text + "</table></center>";
                LocationResultsPlaceholder.Controls.Add(Message);
                myConnection.Close();

            }
        }
        catch (Exception ex)
        {
            Resources.errorHandling(ex);
        }
        
    }

}