using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Windows.Forms;

namespace EmployeeRecords
{
    public partial class ReportsPage : UserControl
    {
        private bool loading;   // true while the filter drop-downs are being filled
        private int printRow;   // the next grid row to print, carried from one printed page to the next

        public ReportsPage()
        {
            InitializeComponent();
        }

        private void ReportsPage_Load(object sender, EventArgs e)
        {
            Theme.StyleGrid(dgvReport);

            try
            {
                loading = true;
                cboStatus.SelectedIndex = 0;
                cboPosition.Items.Add("All");
                foreach (DataRow row in Database.GetTable("SELECT DISTINCT Position FROM Employees WHERE Position Is Not Null ORDER BY Position").Rows)
                {
                    cboPosition.Items.Add(row["Position"].ToString());
                }
                cboPosition.SelectedIndex = 0;
                loading = false;

                LoadReport();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Could not load the report: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Filter_Changed(object sender, EventArgs e)
        {
            if (!loading)
            {
                LoadReport();
            }
        }

        // The complete employee list, narrowed down by the status and position filters.
        private void LoadReport()
        {
            string sql = "SELECT EmployeeNo AS [EMPLOYEE ID], LastName & ', ' & FirstName AS NAME, Position AS POSITION, " +
                         "Department AS DEPARTMENT, HireDate AS [HIRE DATE], EmploymentStatus AS STATUS, Email AS EMAIL, Phone AS PHONE " +
                         "FROM Employees WHERE 1 = 1";
            List<object> values = new List<object>();

            if (cboStatus.Text != "All")
            {
                sql += " AND EmploymentStatus = ?";
                values.Add(cboStatus.Text);
            }
            if (cboPosition.Text != "All")
            {
                sql += " AND Position = ?";
                values.Add(cboPosition.Text);
            }
            sql += " ORDER BY EmployeeNo";

            try
            {
                dgvReport.DataSource = Database.GetTable(sql, values.ToArray());
                dgvReport.Columns["HIRE DATE"].DefaultCellStyle.Format = "MMM d, yyyy";

                // Share the width by how long each column's values are. The printed report uses the same proportions.
                dgvReport.Columns["EMPLOYEE ID"].FillWeight = 97;
                dgvReport.Columns["NAME"].FillWeight = 106;
                dgvReport.Columns["POSITION"].FillWeight = 96;
                dgvReport.Columns["DEPARTMENT"].FillWeight = 104;
                dgvReport.Columns["HIRE DATE"].FillWeight = 80;
                dgvReport.Columns["STATUS"].FillWeight = 57;
                dgvReport.Columns["EMAIL"].FillWeight = 178;
                dgvReport.Columns["PHONE"].FillWeight = 88;

                int count = dgvReport.Rows.Count;
                lblCount.Text = count + (count == 1 ? " employee" : " employees");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Could not load the report: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (dgvReport.Rows.Count == 0)
            {
                MessageBox.Show("There are no employees to print.", "Reports", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                using (PrintDocument document = new PrintDocument())
                using (PrintPreviewDialog preview = new PrintPreviewDialog())
                {
                    document.DocumentName = "Employee Records Report";
                    document.DefaultPageSettings.Landscape = true;
                    document.BeginPrint += (s, args) => printRow = 0;
                    document.PrintPage += Document_PrintPage;

                    // Opens a preview first; its printer button sends the report to the printer.
                    preview.Document = document;
                    preview.Width = 1000;
                    preview.Height = 700;
                    preview.ShowDialog(this);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Could not print: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Draws one page of the report. It is called again for each page while HasMorePages is true.
        private void Document_PrintPage(object sender, PrintPageEventArgs e)
        {
            Graphics g = e.Graphics;
            Rectangle area = e.MarginBounds;
            int y = area.Top;
            int rowHeight = 24;

            int totalWidth = 0;
            foreach (DataGridViewColumn column in dgvReport.Columns)
            {
                totalWidth += column.Width;
            }

            using (Font titleFont = new Font("Segoe UI", 16F, FontStyle.Bold))
            using (Font headerFont = new Font("Segoe UI", 8.5F, FontStyle.Bold))
            using (Font bodyFont = new Font("Segoe UI", 8.5F))
            using (SolidBrush darkGreen = new SolidBrush(Theme.DarkGreen))
            using (SolidBrush headerBack = new SolidBrush(Theme.Green))
            using (SolidBrush headerText = new SolidBrush(Theme.LightGrey))
            using (Pen line = new Pen(Theme.LightGrey))
            using (StringFormat cellFormat = new StringFormat())
            {
                cellFormat.Trimming = StringTrimming.EllipsisCharacter;
                cellFormat.FormatFlags = StringFormatFlags.NoWrap;
                cellFormat.LineAlignment = StringAlignment.Center;

                g.DrawString("Employee Records Report", titleFont, darkGreen, area.Left, y);
                y += 34;
                g.DrawString("Status: " + cboStatus.Text + "     Position: " + cboPosition.Text + "     Printed: " + DateTime.Now.ToString("MMMM d, yyyy h:mm tt"),
                    bodyFont, darkGreen, area.Left, y);
                y += 28;

                // Column headings, sharing the page width the same way the grid shares its width.
                int x = area.Left;
                g.FillRectangle(headerBack, area.Left, y, area.Width, rowHeight);
                foreach (DataGridViewColumn column in dgvReport.Columns)
                {
                    int width = area.Width * column.Width / totalWidth;
                    g.DrawString(column.HeaderText, headerFont, headerText, new RectangleF(x + 4, y, width - 8, rowHeight), cellFormat);
                    x += width;
                }
                y += rowHeight;

                while (printRow < dgvReport.Rows.Count)
                {
                    if (y + rowHeight > area.Bottom)
                    {
                        e.HasMorePages = true;
                        return;
                    }

                    DataGridViewRow row = dgvReport.Rows[printRow];
                    x = area.Left;
                    foreach (DataGridViewColumn column in dgvReport.Columns)
                    {
                        int width = area.Width * column.Width / totalWidth;
                        string text = Convert.ToString(row.Cells[column.Index].FormattedValue);
                        g.DrawString(text, bodyFont, darkGreen, new RectangleF(x + 4, y, width - 8, rowHeight), cellFormat);
                        x += width;
                    }
                    g.DrawLine(line, area.Left, y + rowHeight, area.Right, y + rowHeight);
                    y += rowHeight;
                    printRow++;
                }

                g.DrawString(lblCount.Text, bodyFont, darkGreen, area.Left, y + 8);
            }

            e.HasMorePages = false;
        }
    }
}
