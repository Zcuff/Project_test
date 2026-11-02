using System;
using System.Drawing;
using System.Windows.Forms;

namespace EmployeeRecords
{
    public partial class UsersPage : UserControl
    {
        public UsersPage()
        {
            InitializeComponent();
        }

        private void UsersPage_Load(object sender, EventArgs e)
        {
            Theme.StyleGrid(dgvUsers);
            LoadUsers();
        }

        private void LoadUsers()
        {
            try
            {
                dgvUsers.DataSource = Database.GetTable(
                    "SELECT u.UserID, u.Username AS USERNAME, u.FullName AS [FULL NAME], u.UserRole AS ROLE, e.EmployeeNo AS [EMPLOYEE ID], " +
                    "IIf(u.IsActive, 'Active', 'Inactive') AS STATUS, u.DateCreated AS CREATED " +
                    "FROM Users AS u LEFT JOIN Employees AS e ON u.EmployeeID = e.EmployeeID ORDER BY u.Username");
                dgvUsers.Columns["UserID"].Visible = false;
                dgvUsers.Columns["CREATED"].DefaultCellStyle.Format = "MMM d, yyyy";

                int count = dgvUsers.Rows.Count;
                lblCount.Text = count + (count == 1 ? " account" : " accounts");
                UpdateToggleButton();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Could not load user accounts: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // The UserID of the highlighted row, or 0 when nothing is selected.
        private int SelectedUserId()
        {
            if (dgvUsers.CurrentRow == null)
            {
                return 0;
            }
            return Convert.ToInt32(dgvUsers.CurrentRow.Cells["UserID"].Value);
        }

        private void SelectUser(int userId)
        {
            foreach (DataGridViewRow row in dgvUsers.Rows)
            {
                if (Convert.ToInt32(row.Cells["UserID"].Value) == userId)
                {
                    dgvUsers.CurrentCell = row.Cells["USERNAME"];
                    return;
                }
            }
        }

        private void UpdateToggleButton()
        {
            if (!dgvUsers.Columns.Contains("STATUS"))
            {
                return;
            }
            bool inactive = dgvUsers.CurrentRow != null && dgvUsers.CurrentRow.Cells["STATUS"].Value.ToString() == "Inactive";
            btnToggle.Text = inactive ? "Activate" : "Deactivate";
        }

        private void dgvUsers_SelectionChanged(object sender, EventArgs e)
        {
            UpdateToggleButton();
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            using (UserAccountForm form = new UserAccountForm(0))
            {
                if (form.ShowDialog(this) == DialogResult.OK)
                {
                    LoadUsers();
                    SelectUser(form.UserId);
                }
            }
        }

        private void btnToggle_Click(object sender, EventArgs e)
        {
            int userId = SelectedUserId();
            if (userId == 0)
            {
                MessageBox.Show("Please select an account first.", "User Management", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string username = dgvUsers.CurrentRow.Cells["USERNAME"].Value.ToString();
            bool activate = btnToggle.Text == "Activate";

            if (!activate && userId == Session.UserID)
            {
                MessageBox.Show("You can't deactivate the account you are signed in with.", "User Management", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string question = (activate ? "Activate" : "Deactivate") + " the account \"" + username + "\"?";
            if (MessageBox.Show(question, "User Management", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
            {
                return;
            }

            try
            {
                Database.Execute("UPDATE Users SET IsActive = ? WHERE UserID = ?", activate, userId);
                LoadUsers();
                SelectUser(userId);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Could not update the account: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            int userId = SelectedUserId();
            if (userId == 0)
            {
                MessageBox.Show("Please select an account first.", "User Management", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using (UserAccountForm form = new UserAccountForm(userId))
            {
                form.ShowDialog(this);
            }
        }

        // Deactivated accounts are shown in grey.
        private void dgvUsers_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex >= 0 && dgvUsers.Rows[e.RowIndex].Cells["STATUS"].Value.ToString() == "Inactive")
            {
                e.CellStyle.ForeColor = Color.Gray;
            }
        }
    }
}
