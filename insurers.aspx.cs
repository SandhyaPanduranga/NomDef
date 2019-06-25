using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using NominalDefendant;

public partial class insurers : System.Web.UI.Page
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

            Session["NDID"] = null;
            Session["CurrentStep"] = null;

            myConnection.Open();
            SqlDataReader myReader = null;
            SqlCommand myCommand = new SqlCommand("Select * from Insurers ORDER BY InsurerName ASC", myConnection);

            Table table = new Table();
            table.ID = "TableInsurers";
            table.CellPadding = 5;
            InsurersPlaceholder.Controls.Add(table);

            TableRow rowTitle = new TableRow();
            TableCell cell0Title = new TableCell();
            TableCell cell1Title = new TableCell();
            TableCell cell2Title = new TableCell();
            TableCell cell3Title = new TableCell();

            Literal Message1Title = new Literal();
            Literal Message2Title = new Literal();

            Message1Title.Text = "<strong>Insurer</strong>";
            Message2Title.Text = "<strong>Market Share (%)</strong>";

            cell1Title.Controls.Add(Message1Title);
            cell2Title.Controls.Add(Message2Title);

            rowTitle.Cells.Add(cell0Title);
            rowTitle.Cells.Add(cell1Title);
            rowTitle.Cells.Add(cell2Title);
            rowTitle.Cells.Add(cell3Title);

            table.Rows.Add(rowTitle);

            myReader = myCommand.ExecuteReader();
            while (myReader.Read())
            {
                TableRow row = new TableRow();
                TableCell cell0 = new TableCell();
                TableCell cell1 = new TableCell();
                TableCell cell2 = new TableCell();
                Literal Message0 = new Literal();
                Literal Message1 = new Literal();
                Literal Message2 = new Literal();
                cell1.Height = 30;
                Message0.Text = "<input type=button value='Linking' onclick='javascript:location.href=\"insurerLinking.aspx?iid=" + myReader["InsurerId"].ToString() + "\"' class='buttonCSSNarrow' />";
                Message1.Text = "(" + myReader["InsurerId"].ToString() + ") " + myReader["InsurerName"].ToString();
                Message2.Text = myReader["MarketShare"].ToString() + "%";
                Message2.ID = "MarketShareText_" + myReader["Id"].ToString();

                cell0.Controls.Add(Message0);
                cell1.Controls.Add(Message1);
                cell2.Controls.Add(Message2);

                cell2.ID = "MarketShare_" + myReader["Id"].ToString();

                row.Cells.Add(cell0);
                row.Cells.Add(cell1);
                row.Cells.Add(cell2);

                TableCell cell3 = new TableCell();
                TextBox tb = new TextBox();

                tb.ID = "Insurer_" + myReader["Id"].ToString();
                tb.Width = 60;
                tb.Text = myReader["MarketShare"].ToString();
                tb.Attributes.Add("onkeypress", "return NumberDecimalOnlyEntry(this, event);");
                tb.Attributes.Add("onchange", "updateMarketShareTotal()");

                cell3.Controls.Add(tb);
                cell3.ID = "MarketShareCell_" + myReader["Id"].ToString();
                cell3.Visible = false;
                row.Cells.Add(cell3);
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

    protected void UpdateShareButton_Click(object sender, EventArgs e)
    {
        try
        {
            UpdateShareButton.Visible = false;
            CancelUpdateButton.Visible = true;
            SaveShareButton.Visible = true;

            myConnection.Open();
            SqlDataReader myReader = null;
            SqlCommand myCommand = new SqlCommand("Select * from Insurers ORDER BY InsurerName ASC", myConnection);

            myReader = myCommand.ExecuteReader();
            ContentPlaceHolder mpContentPlaceHolder;
            mpContentPlaceHolder = (ContentPlaceHolder)Master.FindControl("MainContent");
            while (myReader.Read())
            {
                TableCell cell1;
                TableCell cell2;
                if (mpContentPlaceHolder != null)
                {
                    cell1 = mpContentPlaceHolder.FindControl("MarketShare_" + myReader["Id"].ToString()) as TableCell;
                    cell2 = mpContentPlaceHolder.FindControl("MarketShareCell_" + myReader["Id"].ToString()) as TableCell;

                    Literal tmpMessage = cell1.FindControl("MarketShareText_" + myReader["Id"].ToString()) as Literal;

                    TextBox tmpMessageBox = cell2.FindControl("Insurer_" + myReader["Id"].ToString()) as TextBox;
                    tmpMessageBox.Text = myReader["MarketShare"].ToString();

                    if (cell1 != null){cell1.Visible = false;}
                    if (cell2 != null){cell2.Visible = true;}
                }
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
            TotalSharePlaceholder.Visible = false;
            myConnection.Open();
            SqlDataReader myReader = null;
            SqlCommand myCommand = new SqlCommand("Select * from Insurers ORDER BY InsurerName ASC", myConnection);

            myReader = myCommand.ExecuteReader();
            ContentPlaceHolder mpContentPlaceHolder;
            mpContentPlaceHolder = (ContentPlaceHolder)Master.FindControl("MainContent");
            while (myReader.Read())
            {
                TableCell cell1;
                TableCell cell2;
                if (mpContentPlaceHolder != null)
                {
                    cell1 = mpContentPlaceHolder.FindControl("MarketShare_" + myReader["Id"].ToString()) as TableCell;
                    cell2 = mpContentPlaceHolder.FindControl("MarketShareCell_" + myReader["Id"].ToString()) as TableCell;
                    if (cell1 != null) { cell1.Visible = true; }
                    if (cell2 != null) { cell2.Visible = false; }
                }
            }

            myReader.Close();
            myConnection.Close();

            UpdateShareButton.Visible = true;
            CancelUpdateButton.Visible = false;
            SaveShareButton.Visible = false;
            //HiddenTotal.Value = "100";
        }
        catch (Exception ex)
        {
            Resources.errorHandling(ex);
        }    
    }

    protected void SaveShareButton_Click(object sender, EventArgs e)
    {
        try
        {
            decimal totalField = Convert.ToDecimal(HiddenTotal.Value);
            if (totalField < 99 || totalField > 101)
            {
                TotalSharePlaceholder.Visible = true;

            }
            else
            {
                TotalSharePlaceholder.Visible = false;
                // loop through all rows and hide display cells and show edit cells.

                myConnection.Open();
                SqlDataReader myReader = null;
                SqlCommand myCommand = new SqlCommand("Select * from Insurers ORDER BY InsurerName ASC", myConnection);
                String InsurerID = "";
                String MarketShareAudit = "";
                myReader = myCommand.ExecuteReader();
                ContentPlaceHolder mpContentPlaceHolder;
                mpContentPlaceHolder = (ContentPlaceHolder)Master.FindControl("MainContent");
                while (myReader.Read())
                {
                    TextBox marketShare;
                    TableCell cell1;
                    TableCell cell2;
                    if (mpContentPlaceHolder != null)
                    {
                        marketShare = mpContentPlaceHolder.FindControl("Insurer_" + myReader["Id"].ToString()) as TextBox;
                        InsurerID = myReader["InsurerId"].ToString();
                        SqlCommand myCommandUpdate = new SqlCommand("UPDATE Insurers SET MarketShare='" + marketShare.Text.ToString() + "' WHERE ID = '" + myReader["Id"].ToString() + "'", myConnection);
                        myCommandUpdate.ExecuteScalar();
                        cell1 = mpContentPlaceHolder.FindControl("MarketShare_" + myReader["Id"].ToString()) as TableCell;
                        cell2 = mpContentPlaceHolder.FindControl("MarketShareCell_" + myReader["Id"].ToString()) as TableCell;
                        Literal tmpMessage = cell1.FindControl("MarketShareText_" + myReader["Id"].ToString()) as Literal;
                        tmpMessage.Text = marketShare.Text.ToString();
                        if (cell1 != null) { cell1.Visible = true; }
                        if (cell2 != null) { cell2.Visible = false; }
                        MarketShareAudit += InsurerID + "XX" + marketShare.Text.ToString() + "---";
                    }
                }

                myReader.Close();

                SqlCommand myCommandUpdateAudit = new SqlCommand("Insert into InsurersMarketShareAudit (MarketShareSpread, AuditDate) VALUES ('" + MarketShareAudit + "', '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')", myConnection);
                myCommandUpdateAudit.ExecuteScalar();

                myConnection.Close();

                UpdateShareButton.Visible = true;
                CancelUpdateButton.Visible = false;
                SaveShareButton.Visible = false;


            }

        }
        catch (Exception ex)
        {
            Resources.errorHandling(ex);
        }
    }
}