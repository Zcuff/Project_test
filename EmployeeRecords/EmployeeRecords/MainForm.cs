using System;
using System.Windows.Forms;

namespace EmployeeRecords
{
    public partial class MainForm : Form
    {
        // LoginForm checks this when the window closes: true means show the login page again.
        public bool LoggedOut { get; private set; }

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            lblSignedIn.Text = "Signed in as" + Environment.NewLine + Session.FullName + " (" + Session.Role + ")";
            lblDate.Text = DateTime.Now.ToString("dddd, MMMM d, yyyy");
            ShowPage(new DashboardPage(), btnDashboard);
        }

        // Swaps the page shown on the right and highlights its button in the sidebar.
        private void ShowPage(UserControl page, Button button)
        {
            while (pnlContent.Controls.Count > 0)
            {
                pnlContent.Controls[0].Dispose();
            }

            page.Dock = DockStyle.Fill;
            pnlContent.Controls.Add(page);
            lblPageTitle.Text = button.Text;

            foreach (Button navButton in new Button[] { btnDashboard, btnEmployees, btnUsers, btnReports })
            {
                navButton.BackColor = navButton == button ? Theme.Green : Theme.DarkGreen;
            }
        }

        private void btnDashboard_Click(object sender, EventArgs e)
        {
            ShowPage(new DashboardPage(), btnDashboard);
        }

        private void btnEmployees_Click(object sender, EventArgs e)
        {
            ShowPage(new EmployeesPage(), btnEmployees);
        }

        private void btnUsers_Click(object sender, EventArgs e)
        {
            ShowPage(new UsersPage(), btnUsers);
        }

        private void btnReports_Click(object sender, EventArgs e)
        {
            ShowPage(new ReportsPage(), btnReports);
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to logout?", "Logout", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                Session.Clear();
                LoggedOut = true;
                this.Close();
            }
        }
    }
}
