using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using NominalDefendant;

public partial class insurerLinking : System.Web.UI.Page
{
    SqlConnection myConnection = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["NominalConn"].ConnectionString);
    
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {

            if (Session["Admin"] != null && Session["Admin"].ToString() == "Y")
            {
                // allow users to remain on page
            }
            else
            {
                Response.Redirect("Default.aspx?Err=NoAdmin");
            }

            myConnection.Open();
            SqlDataReader myReader = null;
            SqlCommand myCommand = new SqlCommand("Select * from Insurers WHERE InsurerId='" + Request["iid"] + "'", myConnection);
            myReader = myCommand.ExecuteReader();
            myReader.Read();
            InsurerNameLabel.Text = "<strong>" + myReader["InsurerName"].ToString() + " - Current Linkings</strong><br />";
            String[] insurersLinking = myReader["Linkings"].ToString().Split(',');
            myReader.Close();

            Table table = new Table();
            table.ID = "TableInsurerLinking";
            table.CellPadding = 5;
            InsurerLinkingPlaceholder.Controls.Add(table);

            myCommand = new SqlCommand("Select * from Insurers ORDER BY InsurerName ASC", myConnection);
            myReader = myCommand.ExecuteReader();
            while (myReader.Read())
            {
                TableRow row = new TableRow();
                TableCell cell1 = new TableCell();
                TableCell cell2 = new TableCell();
                Literal Message1 = new Literal();

                cell1.Height = 30;
                Message1.Text = myReader["InsurerName"].ToString();
                cell1.Controls.Add(Message1);

                CheckBox checka = new CheckBox();
                checka.ID = "Link_" + myReader["InsurerId"].ToString();
                if (Array.IndexOf(insurersLinking, myReader["InsurerId"].ToString()) != -1)
                {
                    checka.Checked = true;
                }

                if (myReader["InsurerId"].ToString() == Request["iid"])
                {
                    checka.Enabled = false;
                }

                cell2.Controls.Add(checka);

                row.Cells.Add(cell1);
                row.Cells.Add(cell2);
                table.Rows.Add(row);
            }

            myReader.Close();
            myConnection.Close();
        }
        catch (Exception ex)
        {
            Resources.errorHandling(ex);
        }

    }

    protected void CancelUpdateButton_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("insurers.aspx", false);
        }
        catch (Exception ex)
        {
            Resources.errorHandling(ex);
        }
    }

    protected void SaveLinkingButton_Click(object sender, EventArgs e)
    {

        try
        {
            myConnection.Open();
            SqlDataReader myReader = null;
            SqlCommand myCommand = new SqlCommand("Select * from Insurers ORDER BY InsurerName ASC", myConnection);

            myReader = myCommand.ExecuteReader();
            ContentPlaceHolder mpContentPlaceHolder;
            mpContentPlaceHolder = (ContentPlaceHolder)Master.FindControl("MainContent");

            String linking = "-1";
            CheckBox marketShare = new CheckBox();
            while (myReader.Read())
            {
                if (mpContentPlaceHolder != null)
                {
                    marketShare = mpContentPlaceHolder.FindControl("Link_" + myReader["InsurerId"].ToString()) as CheckBox;

                    //update other insurers Linking
                    SqlDataReader myReaderLinking = null;
                    SqlCommand myCommandLinking = new SqlCommand("Select Linkings from Insurers Where InsurerId='" + myReader["InsurerId"].ToString() + "'", myConnection);
                    myReaderLinking = myCommandLinking.ExecuteReader();
                    myReaderLinking.Read();
                    String linkingInsurer = myReaderLinking["Linkings"].ToString();
                    myReaderLinking.Close();

                    if (marketShare.Checked)
                    {
                        linking += "," + myReader["InsurerId"].ToString();

                        if (!linkingInsurer.Contains("," + Request["iid"] + ","))
                        {
                            //Update linking Values
                            linkingInsurer += Request["iid"] + ",";
                            SqlCommand myCommandUpdateLinked = new SqlCommand("UPDATE Insurers SET Linkings='" + linkingInsurer + "'WHERE InsurerId='" + myReader["InsurerId"].ToString() + "'", myConnection);
                            myCommandUpdateLinked.ExecuteScalar();
                        }

                    }
                    else
                    {
                        if (linkingInsurer.Contains("," + Request["iid"]))
                        {
                            //Update linking Values
                            linkingInsurer = linkingInsurer.Replace("," + Request["iid"] + ",", ",");
                            SqlCommand myCommandUpdateLinked = new SqlCommand("UPDATE Insurers SET Linkings='" + linkingInsurer + "'WHERE InsurerId='" + myReader["InsurerId"].ToString() + "'", myConnection);
                            myCommandUpdateLinked.ExecuteScalar();
                        }
                    }
                }
            }

            linking += ",";
            SqlCommand myCommandUpdate = new SqlCommand("UPDATE Insurers SET Linkings='" + linking + "'WHERE InsurerId='" + Request["iid"] + "'", myConnection);
            myCommandUpdate.ExecuteScalar();

            myReader.Close();
            myConnection.Close();
            Response.Redirect("insurers.aspx", false);
        }
        catch (Exception ex)
        {
            Resources.errorHandling(ex);
        }
    }
}