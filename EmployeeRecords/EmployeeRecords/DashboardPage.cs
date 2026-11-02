using System;
using System.Windows.Forms;

namespace EmployeeRecords
{
    public partial class DashboardPage : UserControl
    {
        public DashboardPage()
        {
            InitializeComponent();
        }

        private void DashboardPage_Load(object sender, EventArgs e)
        {
            Theme.StyleGrid(dgvByPosition);
            Theme.StyleGrid(dgvRecent);

            try
            {
                int total = Database.GetNumber("SELECT COUNT(*) FROM Employees");
                int active = Database.GetNumber("SELECT COUNT(*) FROM Employees WHERE EmploymentStatus = 'Active'");
                lblTotal.Text = total.ToString();
                lblActive.Text = active.ToString();
                lblInactive.Text = (total - active).ToString();

                dgvByPosition.DataSource = Database.GetTable(
                    "SELECT Position AS POSITION, SUM(IIf(EmploymentStatus = 'Active', 1, 0)) AS ACTIVE, " +
                    "SUM(IIf(EmploymentStatus = 'Active', 0, 1)) AS INACTIVE, COUNT(*) AS TOTAL " +
                    "FROM Employees GROUP BY Position ORDER BY Position");

                dgvRecent.DataSource = Database.GetTable(
                    "SELECT TOP 10 FirstName & ' ' & LastName AS NAME, HireDate AS HIRED FROM Employees ORDER BY HireDate DESC");
                dgvRecent.Columns["HIRED"].DefaultCellStyle.Format = "MMM d, yyyy";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Could not load the dashboard: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
