using System;
using System.Data;
using System.Windows.Forms;

namespace EmployeeRecords
{
    // Adds a new employee (employeeId 0), edits one, or shows a read-only profile (viewOnly) that can be switched to editing.
    public partial class EmployeeForm : Form
    {
        public int EmployeeId { get; private set; }
        private bool viewOnly;

        public EmployeeForm(int employeeId, bool viewOnly)
        {
            InitializeComponent();
            EmployeeId = employeeId;
            this.viewOnly = viewOnly;
        }

        private void EmployeeForm_Load(object sender, EventArgs e)
        {
            try
            {
                FillChoices(cboPosition, "Position", "HR Officer", "Accountant", "Sales Associate", "IT Support", "Supervisor");
                FillChoices(cboDepartment, "Department", "Human Resources", "Finance", "Sales", "IT", "Operations");

                if (EmployeeId == 0)
                {
                    lblHeading.Text = "ADD EMPLOYEE";
                    txtEmployeeNo.Text = Database.NextEmployeeNo();
                    cboStatus.SelectedItem = "Active";
                    dtpBirthDate.Value = DateTime.Today.AddYears(-25);
                    dtpHireDate.Value = DateTime.Today;
                    lblAccount.Text = "None yet";
                    return;
                }

                LoadEmployee();

                if (viewOnly)
                {
                    lblHeading.Text = "EMPLOYEE PROFILE";
                    SetEditable(false);
                    btnSave.Text = "Edit";
                    btnCancel.Text = "Close";
                }
                else
                {
                    lblHeading.Text = "EDIT EMPLOYEE";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Could not load the employee: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Puts the usual choices, plus any others already saved in the database, into a drop-down.
        private void FillChoices(ComboBox box, string column, params string[] defaults)
        {
            box.Items.AddRange(defaults);
            DataTable used = Database.GetTable("SELECT DISTINCT " + column + " FROM Employees WHERE " + column + " Is Not Null ORDER BY " + column);
            foreach (DataRow row in used.Rows)
            {
                string value = row[0].ToString();
                if (!box.Items.Contains(value))
                {
                    box.Items.Add(value);
                }
            }
        }

        private void LoadEmployee()
        {
            DataRow row = Database.GetTable("SELECT * FROM Employees WHERE EmployeeID = ?", EmployeeId).Rows[0];

            txtEmployeeNo.Text = row["EmployeeNo"].ToString();
            txtFirstName.Text = row["FirstName"].ToString();
            txtLastName.Text = row["LastName"].ToString();
            cboGender.SelectedItem = row["Gender"].ToString();
            txtEmail.Text = row["Email"].ToString();
            txtPhone.Text = row["Phone"].ToString();
            txtAddress.Text = row["Address"].ToString();
            cboPosition.Text = row["Position"].ToString();
            cboDepartment.Text = row["Department"].ToString();
            cboStatus.SelectedItem = row["EmploymentStatus"].ToString();

            if (row["BirthDate"] != DBNull.Value)
            {
                dtpBirthDate.Value = Convert.ToDateTime(row["BirthDate"]);
            }
            if (row["HireDate"] != DBNull.Value)
            {
                dtpHireDate.Value = Convert.ToDateTime(row["HireDate"]);
            }

            DataTable account = Database.GetTable("SELECT Username, IsActive FROM Users WHERE EmployeeID = ?", EmployeeId);
            if (account.Rows.Count == 0)
            {
                lblAccount.Text = "None";
            }
            else
            {
                bool active = Convert.ToBoolean(account.Rows[0]["IsActive"]);
                lblAccount.Text = account.Rows[0]["Username"] + (active ? " (Active)" : " (Deactivated)");
            }
        }

        private void SetEditable(bool editable)
        {
            foreach (Control control in Controls)
            {
                if (control is TextBox && control != txtEmployeeNo)
                {
                    ((TextBox)control).ReadOnly = !editable;
                }
                else if (control is ComboBox || control is DateTimePicker)
                {
                    control.Enabled = editable;
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (viewOnly)
            {
                // On a profile this button says "Edit": it turns the fields on instead of saving.
                viewOnly = false;
                SetEditable(true);
                lblHeading.Text = "EDIT EMPLOYEE";
                btnSave.Text = "Save";
                btnCancel.Text = "Cancel";
                txtFirstName.Focus();
                return;
            }

            string firstName = txtFirstName.Text.Trim();
            string lastName = txtLastName.Text.Trim();
            string gender = cboGender.Text;
            string email = txtEmail.Text.Trim();
            string phone = txtPhone.Text.Trim();
            string address = txtAddress.Text.Trim();
            string position = cboPosition.Text.Trim();
            string department = cboDepartment.Text.Trim();
            string status = cboStatus.Text;
            DateTime birthDate = dtpBirthDate.Value.Date;
            DateTime hireDate = dtpHireDate.Value.Date;

            if (firstName == "" || lastName == "" || gender == "" || position == "" || department == "")
            {
                MessageBox.Show("Please fill in the first name, last name, gender, position and department.", "Employee", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (email != "" && !email.Contains("@"))
            {
                MessageBox.Show("Please enter a valid email address.", "Employee", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (hireDate <= birthDate)
            {
                MessageBox.Show("The hire date must be after the birth date.", "Employee", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                if (EmployeeId == 0)
                {
                    Database.Execute(
                        "INSERT INTO Employees (EmployeeNo, FirstName, LastName, Gender, BirthDate, Email, Phone, Address, Position, Department, HireDate, EmploymentStatus, DateCreated) " +
                        "VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)",
                        txtEmployeeNo.Text, firstName, lastName, gender, birthDate, email, phone, address, position, department, hireDate, status, DateTime.Now);
                    EmployeeId = Database.GetNumber("SELECT EmployeeID FROM Employees WHERE EmployeeNo = ?", txtEmployeeNo.Text);
                }
                else
                {
                    Database.Execute(
                        "UPDATE Employees SET FirstName = ?, LastName = ?, Gender = ?, BirthDate = ?, Email = ?, Phone = ?, Address = ?, " +
                        "Position = ?, Department = ?, HireDate = ?, EmploymentStatus = ? WHERE EmployeeID = ?",
                        firstName, lastName, gender, birthDate, email, phone, address, position, department, hireDate, status, EmployeeId);

                    if (status == "Inactive")
                    {
                        Database.Execute("UPDATE Users SET IsActive = False WHERE EmployeeID = ?", EmployeeId);
                    }
                }

                MessageBox.Show("Employee record saved.", "Employee", MessageBoxButtons.OK, MessageBoxIcon.Information);
                DialogResult = DialogResult.OK;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Could not save the employee: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
