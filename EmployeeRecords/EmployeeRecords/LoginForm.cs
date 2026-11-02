using System;
using System.Data;
using System.Windows.Forms;

namespace EmployeeRecords
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text;

            if (username == "" || password == "")
            {
                MessageBox.Show("Please enter your username and password.", "Login", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                DataTable result = Database.GetTable("SELECT UserID, Username, PasswordHash, FullName, UserRole, IsActive FROM Users WHERE Username = ?", username);

                if (result.Rows.Count == 0 || !PasswordHelper.VerifyPassword(password, result.Rows[0]["PasswordHash"].ToString()))
                {
                    MessageBox.Show("Invalid username or password.", "Login", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                DataRow user = result.Rows[0];
                string role = user["UserRole"].ToString();

                if (!Convert.ToBoolean(user["IsActive"]))
                {
                    MessageBox.Show("This account has been deactivated. Please contact an administrator.", "Login", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (role != "Admin" && role != "Manager")
                {
                    MessageBox.Show("Only Admin and Manager accounts can sign in to these pages.", "Login", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                Session.UserID = Convert.ToInt32(user["UserID"]);
                Session.Username = user["Username"].ToString();
                Session.FullName = user["FullName"].ToString();
                Session.Role = role;

                txtPassword.Clear();
                MainForm main = new MainForm();
                main.FormClosed += MainForm_FormClosed;
                main.Show();
                this.Hide();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Could not sign in: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Logging out brings this page back. Closing the main window any other way ends the app.
        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (((MainForm)sender).LoggedOut)
            {
                txtUsername.Clear();
                this.Show();
                txtUsername.Focus();
            }
            else
            {
                this.Close();
            }
        }
    }
}
