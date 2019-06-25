using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Security.Principal;
using System.Data.SqlClient;
using NominalDefendant;

public partial class Logout : System.Web.UI.Page
{

    protected void Page_Load(object sender, EventArgs e)
    {
            
            try
            {
                Session["Auth"] = null;
                Session["Admin"] = null;
            }
            catch (Exception ex)
            {
                Resources.errorHandling(ex);
            }

    }
}