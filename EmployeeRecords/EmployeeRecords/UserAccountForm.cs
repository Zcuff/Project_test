using System;
using System.Data;
using System.Windows.Forms;

namespace EmployeeRecords
{
    // Creates a login account for an employee (userId 0), or resets the password of an existing account.
    public partial class UserAccountForm : Form
    {
        public int UserId { get; private set; }
        private readonly bool resetMode;

        public UserAccountForm(int userId)
        {
            InitializeComponent();
            UserId = userId;
            resetMode = userId != 0;
        }

        private void UserAccountForm_Load(object sender, EventArgs e)
        {
            try
            {
                if (resetMode)
                {
                    DataRow user = Database.GetTable("SELECT Username, FullName, UserRole FROM Users WHERE UserID = ?", UserId).Rows[0];
                    this.Text = "Reset Password";
                    lblHeading.Text = "RESET PASSWORD";
                    cboEmployee.Items.Add(user["FullName"].ToString());
                    cboEmployee.SelectedIndex = 0;
                    cboEmployee.Enabled = false;
                    txtUsername.Text = user["Username"].ToString();
                    txtUsername.ReadOnly = true;
                    cboRole.SelectedItem = user["UserRole"].ToString();
                    cboRole.Enabled = false;
                    lblPassword.Text = "New password (at least 8 characters)";
                    lblConfirm.Text = "Confirm new password";
                    this.ActiveControl = txtPassword;
                    return;
                }

                // Only active employees who don't have an account yet.
                DataTable employees = Database.GetTable(
                    "SELECT EmployeeID, EmployeeNo & ' - ' & FirstName & ' ' & LastName AS Display, FirstName, LastName FROM Employees " +
                    "WHERE EmploymentStatus = 'Active' AND EmployeeID NOT IN (SELECT EmployeeID FROM Users WHERE EmployeeID Is Not Null) ORDER BY EmployeeNo");

                if (employees.Rows.Count == 0)
                {
                    MessageBox.Show("Every active employee already has a login account.", "User Management", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                    return;
                }

                cboEmployee.DisplayMember = "Display";
                cboEmployee.ValueMember = "EmployeeID";
                cboEmployee.DataSource = employees;
                cboRole.SelectedItem = "Employee";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Could not load the account: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Suggests a username like "msantos" for the chosen employee.
        private void cboEmployee_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataRowView employee = cboEmployee.SelectedItem as DataRowView;
            if (employee != null)
            {
                string first = employee["FirstName"].ToString();
                string last = employee["LastName"].ToString();
                txtUsername.Text = (first.Substring(0, 1) + last).Replace(" ", "").ToLower();
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text;

            if (!resetMode && (cboEmployee.SelectedIndex < 0 || username == "" || cboRole.SelectedIndex < 0))
            {
                MessageBox.Show("Please choose an employee, a username and a role.", "User Account", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (password.Length < 8)
            {
                MessageBox.Show("The password must be at least 8 characters long.", "User Account", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (password != txtConfirm.Text)
            {
                MessageBox.Show("The passwords do not match.", "User Account", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                if (resetMode)
                {
                    Database.Execute("UPDATE Users SET PasswordHash = ? WHERE UserID = ?", PasswordHelper.HashPassword(password), UserId);
                    MessageBox.Show("The password for \"" + username + "\" has been reset.", "User Account", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    if (Database.GetNumber("SELECT COUNT(*) FROM Users WHERE Username = ?", username) > 0)
                    {
                        MessageBox.Show("That username is already taken. Please choose another one.", "User Account", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    DataRowView employee = (DataRowView)cboEmployee.SelectedItem;
                    string fullName = employee["FirstName"] + " " + employee["LastName"];

                    Database.Execute(
                        "INSERT INTO Users (Username, PasswordHash, FullName, UserRole, IsActive, EmployeeID, DateCreated) VALUES (?, ?, ?, ?, ?, ?, ?)",
                        username, PasswordHelper.HashPassword(password), fullName, cboRole.Text, true, Convert.ToInt32(employee["EmployeeID"]), DateTime.Now);
                    UserId = Database.GetNumber("SELECT UserID FROM Users WHERE Username = ?", username);

                    MessageBox.Show("Login account created for " + fullName + ".", "User Account", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                DialogResult = DialogResult.OK;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Could not save the account: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
