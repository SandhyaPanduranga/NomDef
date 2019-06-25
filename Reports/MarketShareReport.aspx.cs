using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Globalization;
using NominalDefendant;

public partial class MarketShareReport : System.Web.UI.Page
{

    SqlConnection myConnection = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["NominalConn"].ConnectionString);
    
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void ButtonSearch_Click(object sender, EventArgs e)
    {

        try
        {

            tmpLabel.Text = "";
            
            myConnection.Close();
            myConnection.Open();

            SqlDataReader myReader = null;
            SqlCommand myCommand = new SqlCommand("select * from InsurersMarketShareAudit where Id = '" + DateDropDown.SelectedValue + "'", myConnection);
            myReader = myCommand.ExecuteReader();
            
            bool rowCount = myReader.HasRows;
            if (rowCount)
            {
                OutputLabel.Text = "<strong>Market Share Report for the period commencing " + DateDropDown.SelectedItem + "</strong><br /><br />";
                myReader.Read();
                string[] RawShareSplit = myReader["MarketShareSpread"].ToString().Split(new string[] { "---" }, StringSplitOptions.None);
                myReader.Close();
                OutputLabel.Text += "<table cellpadding=8 cellspacing=3>";
                OutputLabel.Text += "<tr><Td class='tableTitle'>Insurer</td><td class='tableTitle'>Market Share</td></tr>";
                foreach (string ShareSplit in RawShareSplit)
                {
                    if (ShareSplit != "" && ShareSplit != null)
                    {
                        string[] InsurerShareSplit = ShareSplit.Split(new string[] { "XX" }, StringSplitOptions.None);
                        if (InsurerShareSplit[1] != "0.00" && InsurerShareSplit[1] != "0")
                        {
                            myCommand = new SqlCommand("select * from Insurers where InsurerId = '" + InsurerShareSplit[0] + "'", myConnection);
                            myReader = myCommand.ExecuteReader();
                            myReader.Read();
                            OutputLabel.Text += "<tr><td class='tableField' width='200'>" + myReader["InsurerName"].ToString() + "</td><td class='tableField' width='150'>" + InsurerShareSplit[1] + "</td></tr>";
                            myReader.Close();
                        }
                        
                    }
                    
                }
                OutputLabel.Text += "</table>";
            }

            myConnection.Close();

        }
        catch (Exception ex)
        {
            Resources.errorHandling(ex);
        }

    }


}