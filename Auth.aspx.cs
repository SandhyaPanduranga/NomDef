using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Security.Principal;
using System.Data.SqlClient;
using NominalDefendant;

public partial class Auth : System.Web.UI.Page
{

    SqlConnection myConnection = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["NominalConn"].ConnectionString);
    
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            string windowsLogin = Page.User.Identity.Name.ToString();
            int hasDomain = windowsLogin.IndexOf(@"\");

            if (hasDomain > 0)
            {
                windowsLogin = windowsLogin.Remove(0, hasDomain + 1);
            }

            myConnection.Open();

            String mySQL = "Select * FROM [NDF].[dbo].[Users] where Username = '" + windowsLogin + "' AND IsActive='Y'";
            SqlDataReader myReader = null;
            SqlCommand myCommand = new SqlCommand(mySQL, myConnection);
            myReader = myCommand.ExecuteReader();
            bool rowCount = myReader.HasRows;
            if (rowCount)
            {
                myReader.Read();
                Session["Auth"] = windowsLogin;
                Session["Admin"] = myReader["IsAdmin"].ToString();
                Response.Redirect("Default.aspx", false);
            }

            myConnection.Close();
        }
        catch (Exception ex)
        {
            Resources.errorHandling(ex);
        }

    }
}