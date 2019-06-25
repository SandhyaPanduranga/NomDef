using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using NominalDefendant;

public partial class _Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (Request["Err"] != null && Request["Err"] == "NoAdmin")
            {
                tmpLabel.Text = "You have been directed here because you attempted to access a restricted page.<br/><br/>You require admin rights to see the page you attempted to access";
            }
        }
        catch (Exception ex)
        {
            Resources.errorHandling(ex);
        }
    }
}