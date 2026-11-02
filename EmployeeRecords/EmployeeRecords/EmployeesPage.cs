using System;
using System.Drawing;
using System.Windows.Forms;

namespace EmployeeRecords
{
    public partial class EmployeesPage : UserControl
    {
        public EmployeesPage()
        {
            InitializeComponent();
        }

        private void EmployeesPage_Load(object sender, EventArgs e)
        {
            Theme.StyleGrid(dgvEmployees);
            LoadEmployees();
        }

        // Fills the list, keeping only employees whose name or employee ID contains the search text.
        private void LoadEmployees()
        {
            string search = "%" + txtSearch.Text.Trim() + "%";

            try
            {
                dgvEmployees.DataSource = Database.GetTable(
                    "SELECT EmployeeID, EmployeeNo AS [EMPLOYEE ID], LastName & ', ' & FirstName AS NAME, Position AS POSITION, " +
                    "Department AS DEPARTMENT, Email AS EMAIL, Phone AS PHONE, EmploymentStatus AS STATUS FROM Employees " +
                    "WHERE EmployeeNo LIKE ? OR FirstName & ' ' & LastName LIKE ? OR LastName & ', ' & FirstName LIKE ? ORDER BY EmployeeNo",
                    search, search, search);
                dgvEmployees.Columns["EmployeeID"].Visible = false;
                // Share the width by how long each column's values are (100 is the default).
                dgvEmployees.Columns["EMPLOYEE ID"].FillWeight = 100;
                dgvEmployees.Columns["NAME"].FillWeight = 118;
                dgvEmployees.Columns["POSITION"].FillWeight = 108;
                dgvEmployees.Columns["DEPARTMENT"].FillWeight = 123;
                dgvEmployees.Columns["EMAIL"].FillWeight = 220;
                dgvEmployees.Columns["PHONE"].FillWeight = 98;
                dgvEmployees.Columns["STATUS"].FillWeight = 66;

                int count = dgvEmployees.Rows.Count;
                lblCount.Text = count + (count == 1 ? " employee" : " employees");
                UpdateStatusButton();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Could not load employees: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // The EmployeeID of the highlighted row, or 0 when nothing is selected.
        private int SelectedEmployeeId()
        {
            if (dgvEmployees.CurrentRow == null)
            {
                return 0;
            }
            return Convert.ToInt32(dgvEmployees.CurrentRow.Cells["EmployeeID"].Value);
        }

        private void SelectEmployee(int employeeId)
        {
            foreach (DataGridViewRow row in dgvEmployees.Rows)
            {
                if (Convert.ToInt32(row.Cells["EmployeeID"].Value) == employeeId)
                {
                    dgvEmployees.CurrentCell = row.Cells["EMPLOYEE ID"];
                    return;
                }
            }
        }

        // The status button offers the opposite of the selected employee's current status.
        private void UpdateStatusButton()
        {
            if (!dgvEmployees.Columns.Contains("STATUS"))
            {
                return;
            }
            bool inactive = dgvEmployees.CurrentRow != null && dgvEmployees.CurrentRow.Cells["STATUS"].Value.ToString() == "Inactive";
            btnStatus.Text = inactive ? "Set Active" : "Set Inactive";
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            LoadEmployees();
        }

        private void dgvEmployees_SelectionChanged(object sender, EventArgs e)
        {
            UpdateStatusButton();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            using (EmployeeForm form = new EmployeeForm(0, false))
            {
                if (form.ShowDialog(this) == DialogResult.OK)
                {
                    LoadEmployees();
                    SelectEmployee(form.EmployeeId);
                }
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            OpenEmployee(false);
        }

        private void btnView_Click(object sender, EventArgs e)
        {
            OpenEmployee(true);
        }

        private void dgvEmployees_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                OpenEmployee(true);
            }
        }

        private void OpenEmployee(bool viewOnly)
        {
            int employeeId = SelectedEmployeeId();
            if (employeeId == 0)
            {
                MessageBox.Show("Please select an employee first.", "Employee Records", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using (EmployeeForm form = new EmployeeForm(employeeId, viewOnly))
            {
                form.ShowDialog(this);
            }

            // A profile can be switched to editing, so reload either way.
            LoadEmployees();
            SelectEmployee(employeeId);
        }

        private void btnStatus_Click(object sender, EventArgs e)
        {
            int employeeId = SelectedEmployeeId();
            if (employeeId == 0)
            {
                MessageBox.Show("Please select an employee first.", "Employee Records", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string name = dgvEmployees.CurrentRow.Cells["NAME"].Value.ToString();
            string newStatus = btnStatus.Text == "Set Active" ? "Active" : "Inactive";
            string question = newStatus == "Inactive"
                ? "Set " + name + " as Inactive? Their login account, if they have one, will be deactivated too."
                : "Set " + name + " as Active?";

            if (MessageBox.Show(question, "Update Status", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
            {
                return;
            }

            try
            {
                Database.Execute("UPDATE Employees SET EmploymentStatus = ? WHERE EmployeeID = ?", newStatus, employeeId);
                if (newStatus == "Inactive")
                {
                    Database.Execute("UPDATE Users SET IsActive = False WHERE EmployeeID = ?", employeeId);
                }
                LoadEmployees();
                SelectEmployee(employeeId);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Could not update the status: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Inactive employees are shown in grey so they stand out from active ones.
        private void dgvEmployees_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex >= 0 && dgvEmployees.Rows[e.RowIndex].Cells["STATUS"].Value.ToString() == "Inactive")
            {
                e.CellStyle.ForeColor = Color.Gray;
            }
        }
    }
}
