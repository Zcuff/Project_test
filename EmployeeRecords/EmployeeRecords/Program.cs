using System;
using System.Windows.Forms;

namespace EmployeeRecords
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            try
            {
                Database.EnsureDatabase();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Could not set up employees.accdb: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            // The app runs until the login window closes. LoginForm hides itself while MainForm is open.
            Application.Run(new LoginForm());
        }
    }
}
