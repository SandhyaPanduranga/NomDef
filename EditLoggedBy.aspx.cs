using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using NominalDefendant;

public partial class EditLoggedBy : System.Web.UI.Page
{

    SqlConnection myConnection = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["NominalConn"].ConnectionString);

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!Page.IsPostBack)
            {
                populateFields();
            }
        }
        catch (Exception ex)
        {
            Resources.errorHandling(ex);
        }
    }

    protected void LodgerButtonEdit_Click(object sender, EventArgs e)
    {
        try
        {
            myConnection.Close();

            myConnection.Open();
            String mySQL = "UPDATE [NDF].[dbo].[Lodgers] SET Company = '" + CompanyEdit.Text.Replace("'", "''") + "', Title = '" + TitleEdit.Text.Replace("'", "''") + "', FirstName = '" + FirstNameEdit.Text.Replace("'", "''") + "', LastName = '" + LastNameEdit.Text.Replace("'", "''") + "', Address = '" + AddressEdit.Text.Replace("'", "''") + "', PhoneNumber = '" + PhoneEdit.Text.Replace("'", "''") + "', FaxNumber = '" + FaxEdit.Text.Replace("'", "''") + "', Type = '" + TypeList.Text + "', Suburb = '" + SuburbEdit.Text.Replace("'", "''") + "' WHERE Id = '" + Request["lid"] + "'";
            SqlCommand myCommand = new SqlCommand(mySQL, myConnection);
            myCommand.ExecuteNonQuery();

            string script = "<script language=\"javas" + "cript\">\r\n";
            script += "{\r\n";
            script += "populateLodger('" + AddSlashes(CompanyEdit.Text) + "', '" + AddSlashes(TitleEdit.Text) + " " + AddSlashes(FirstNameEdit.Text) + "', '" + AddSlashes(LastNameEdit.Text) + "', '" + AddSlashes(AddressEdit.Text.Replace(Environment.NewLine, "XXYYXX")) + "', '" + AddSlashes(SuburbEdit.Text) + "', '" + Request["lid"] + "');\r\n";
            script += "}\r\n";
            script += "</" + "script>";

            ClientScript.RegisterClientScriptBlock(this.GetType(), "SaveCloseEditLodger", script);

            myConnection.Close();
        }
        catch (Exception ex)
        {
            Resources.errorHandling(ex);
        }       
    }

    public void populateFields()
    {
        try
        {
            myConnection.Close();
            myConnection.Open();

            String mySQL = "Select * FROM [NDF].[dbo].[Lodgers] WHERE [NDF].[dbo].[Lodgers].Id = '" + Request["lid"] + "'";
            SqlDataReader myReader = null;
            SqlCommand myCommand = new SqlCommand(mySQL, myConnection);
            myReader = myCommand.ExecuteReader();
            bool rowCount = myReader.HasRows;
            if (rowCount)
            {
                myReader.Read();
                CompanyEdit.Text = myReader["Company"].ToString();
                TitleEdit.Text = myReader["Title"].ToString();
                FirstNameEdit.Text = myReader["FirstName"].ToString();
                LastNameEdit.Text = myReader["LastName"].ToString();
                AddressEdit.Text = myReader["Address"].ToString();
                SuburbEdit.Text = myReader["Suburb"].ToString();
                PhoneEdit.Text = myReader["PhoneNumber"].ToString();
                FaxEdit.Text = myReader["FaxNumber"].ToString();
                TypeList.SelectedValue = myReader["Type"].ToString();
            }
            myReader.Close();
            myConnection.Close();
        }
        catch (Exception ex)
        {
            Resources.errorHandling(ex);
        }
        
    }

    protected void CancelEdit_Click(object sender, EventArgs e)
    {
        try
        {
            string script = "<script language=\"javas" + "cript\">\r\n";
            script += "{\r\n";
            script += "location.href='LoggedBy.aspx';\r\n";
            script += "}\r\n";
            script += "</" + "script>";
            ClientScript.RegisterClientScriptBlock(this.GetType(), "CloseEditLodger", script);
        }
        catch (Exception ex)
        {
            Resources.errorHandling(ex);
        }
        
    }

    public string AddSlashes(string InputTxt)
    {
        // List of characters handled:
        // \000 null
        // \010 backspace
        // \011 horizontal tab
        // \012 new line
        // \015 carriage return
        // \032 substitute
        // \042 double quote
        // \047 single quote
        // \134 backslash
        // \140 grave accent

        string Result = InputTxt;

        try
        {
            Result = System.Text.RegularExpressions.Regex.Replace(InputTxt, @"[\000\010\011\012\015\032\042\047\134\140]", "\\$0");
        }
        catch (Exception ex)
        {
            Resources.errorHandling(ex);
        }

        return Result;
    }

}