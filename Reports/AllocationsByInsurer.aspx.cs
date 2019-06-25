using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Globalization;
using NominalDefendant;

public partial class AllocationsByInsurer : System.Web.UI.Page
{

    SqlConnection myConnection = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["NominalConn"].ConnectionString);
    
    protected void Page_Load(object sender, EventArgs e)
    {
        
    }

    protected void ButtonSearch_Click(object sender, EventArgs e)
    {

        try
        {

            if (AllocationFromDate.Text == "" || AllocationToDate.Text == "")
            {
                tmpLabel.Text = "<strong>Please enter both From and To dates</strong>";
            }
            else
            {
                tmpLabel.Text = "";
                myConnection.Close();
                myConnection.Open();

                decimal TotalAllocations = 0;

                SqlDataReader myReaderCount = null;
                SqlDataReader myReader = null;

                CultureInfo provider = new CultureInfo("en-AU");
                DateTime AllocatedFrom = DateTime.Parse(AllocationFromDate.Text, provider);
                DateTime AllocatedTo = DateTime.Parse(AllocationToDate.Text, provider);

                String AllocatedFromText = AllocatedFrom.Month.ToString() + "/" + AllocatedFrom.Day.ToString() + "/" + AllocatedFrom.Year.ToString();
                String AllocatedToText = AllocatedTo.Month.ToString() + "/" + AllocatedTo.Day.ToString() + "/" + AllocatedTo.Year.ToString();

                SqlCommand myCommandCount = new SqlCommand("select COUNT(*) as NumberAllocated from NominalDefendants where AllocatedDate >= '" + AllocatedFromText + "' AND AllocatedDate <= '" + AllocatedToText + "' AND AllocatedInsurerId != '0'", myConnection);
                SqlCommand myCommand = new SqlCommand("select COUNT(*) as NumberAllocated, InsurerName from NominalDefendants, Insurers where AllocatedDate >= '" + AllocatedFromText + "' AND AllocatedDate <= '" + AllocatedToText + "' AND AllocatedInsurerId != '0' and NominalDefendants.AllocatedInsurerId = Insurers.InsurerId Group by InsurerName", myConnection);

                myReaderCount = myCommandCount.ExecuteReader();
                myReader = myCommand.ExecuteReader();

                bool rowCount = myReader.HasRows;
                if (rowCount)
                {
                    InsurersPlaceholder.Controls.Clear();
                   
                    Label TitleLabel = new Label();
                    TitleLabel.Text = "<strong> Allocations By Insurer</strong><br /><br />";
                    TitleLabel.Text += "Claims allocated between " + AllocatedFrom.Day.ToString() + "/" + AllocatedFrom.Month.ToString() + "/" + AllocatedFrom.Year.ToString() + " and " + AllocatedTo.Day.ToString() + "/" + AllocatedTo.Month.ToString() + "/" + AllocatedTo.Year.ToString() + "<br /><br />";
                    InsurersPlaceholder.Controls.Add(TitleLabel);
                    
                    myReaderCount.Read();
                    TotalAllocations = Convert.ToDecimal(myReaderCount["NumberAllocated"]);
                    

                    Table table = new Table();
                    table.CellPadding = 8;
                    table.CellSpacing = 3;

                    TableRow rowTitle = new TableRow();
                    rowTitle.ID = "RowTitle";
                    rowTitle.Height = 28;

                    TableCell cell1Title = new TableCell(); // Insurer
                    TableCell cell2Title = new TableCell(); // Number Claims
                    TableCell cell3Title = new TableCell(); // % of Industry

                    Literal cell1TitleText = new Literal();
                    Literal cell2TitleText = new Literal();
                    Literal cell3TitleText = new Literal();

                    cell1TitleText.Text = "<strong>Insurer</strong>";
                    cell1Title.Controls.Add(cell1TitleText);
                    cell1Title.Width = 200;
                    cell1Title.Wrap = false;
                    cell1Title.CssClass = "tableTitle";
                    rowTitle.Cells.Add(cell1Title);

                    cell2TitleText.Text = "<strong>No of Claims</strong>";
                    cell2Title.Controls.Add(cell2TitleText);
                    cell2Title.Width = 150;
                    cell2Title.CssClass = "tableTitle";
                    rowTitle.Cells.Add(cell2Title);

                    cell3TitleText.Text = "<strong>% of Industry</strong>";
                    cell3Title.Controls.Add(cell3TitleText);
                    cell3Title.Width = 150;
                    cell3Title.CssClass = "tableTitle";
                    rowTitle.Cells.Add(cell3Title);

                    table.Rows.Add(rowTitle);

                    while (myReader.Read())
                    {

                        // This is the display row.
                        TableRow rowDisp = new TableRow();
                        rowDisp.ID = "rowDisp";
                        rowDisp.Height = 28;
                        TableCell cell1Disp = new TableCell();
                        TableCell cell2Disp = new TableCell();
                        TableCell cell3Disp = new TableCell();

                        Literal cell1DispText = new Literal();
                        cell1DispText.Text = myReader["InsurerName"].ToString();
                        cell1Disp.Style.Value = "text-transform:uppercase";
                        cell1Disp.Controls.Add(cell1DispText);
                        cell1Disp.CssClass = "tableField";
                        rowDisp.Cells.Add(cell1Disp);

                        Literal cell2DispText = new Literal();
                        cell2DispText.Text = myReader["NumberAllocated"].ToString();
                        cell2Disp.Controls.Add(cell2DispText);
                        cell2Disp.CssClass = "tableField";
                        rowDisp.Cells.Add(cell2Disp);

                        Literal cell3DispText = new Literal();

                        decimal allocatedAmount = Convert.ToInt16(myReader["NumberAllocated"].ToString());
                        decimal percentAllocated = (allocatedAmount / TotalAllocations) * 100;
                        cell3DispText.Text = decimal.Round(percentAllocated, 2).ToString() + "%";
                        cell3Disp.Controls.Add(cell3DispText);
                        cell3Disp.CssClass = "tableField";
                        rowDisp.Cells.Add(cell3Disp);

                        table.Rows.Add(rowDisp);

                    }

                    TableRow rowTotal = new TableRow();
                    rowTotal.ID = "RowTotal";
                    rowTotal.Height = 28;

                    TableCell cell1Total = new TableCell(); // Insurer
                    TableCell cell2Total = new TableCell(); // Number Claims
                    TableCell cell3Total = new TableCell(); // % of Industry

                    Literal cell1TotalText = new Literal();
                    Literal cell2TotalText = new Literal();
                    Literal cell3TotalText = new Literal();

                    cell1TotalText.Text = "<strong>Totals for Period</strong>";
                    cell1Total.Controls.Add(cell1TotalText);
                    cell1Total.Width = 200;
                    cell1Total.Wrap = false;
                    cell1Total.CssClass = "tableTitle";
                    rowTotal.Cells.Add(cell1Total);

                    cell2TotalText.Text = TotalAllocations.ToString();
                    cell2Total.Controls.Add(cell2TotalText);
                    cell2Total.Width = 150;
                    cell2Total.CssClass = "tableTitle";
                    rowTotal.Cells.Add(cell2Total);

                    cell3TotalText.Text = " ";
                    cell3Total.Controls.Add(cell3TotalText);
                    cell3Total.Width = 150;
                    cell3Total.CssClass = "tableTitle";
                    rowTotal.Cells.Add(cell3Total);

                    table.Rows.Add(rowTotal);

                    InsurersPlaceholder.Controls.Add(table);
                }
                myReaderCount.Close();
                myReader.Close();
                myConnection.Close();

            }
        }
        catch (Exception ex)
        {
            Resources.errorHandling(ex);
        }

    }

}