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
                MessageBox.Show("Could not set up employees.accdb:\n\n" + ex.Message +
                    "\n\nThe usual cause is that the Microsoft Access Database Engine isn't installed. Install the Microsoft Access Database Engine 2016 Redistributable, 64-bit (AccessDatabaseEngine_X64.exe), then run the app again." +
                    "\n\nIf that installer refuses because 32-bit Office is installed, install the 32-bit one (AccessDatabaseEngine.exe) instead and set Platform target to x86 in Project > Properties > Build." +
                    "\n\nDatabase file: " + Database.DbPath,
                    "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                // Signing in can't work without the database, so stop here instead of showing the login screen.
                return;
            }

            // The app runs until the login window closes. LoginForm hides itself while MainForm is open.
            Application.Run(new LoginForm());
        }
    }
}
