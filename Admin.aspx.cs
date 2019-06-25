using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using NominalDefendant;

public partial class Admin : System.Web.UI.Page
{
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
                Response.Redirect("Default.aspx?Err=NoAdmin", false);
            }
        }
        catch (Exception ex)
        {
            Resources.errorHandling(ex);
        }

    }
}