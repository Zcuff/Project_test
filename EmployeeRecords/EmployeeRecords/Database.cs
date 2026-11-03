using System;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Windows.Forms;

namespace EmployeeRecords
{
    public static class Database
    {
        public static readonly string DbPath = Path.Combine(Application.StartupPath, "employees.accdb");

        public static readonly string ConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + DbPath + ";Persist Security Info=False;";

        public static OleDbConnection GetConnection()
        {
            return new OleDbConnection(ConnectionString);
        }

        // Runs a SELECT and returns the rows. Each ? in the SQL is filled with the next value, in order.
        public static DataTable GetTable(string sql, params object[] values)
        {
            using (OleDbConnection con = GetConnection())
            using (OleDbCommand cmd = new OleDbCommand(sql, con))
            using (OleDbDataAdapter adapter = new OleDbDataAdapter(cmd))
            {
                AddParameters(cmd, values);
                DataTable table = new DataTable();
                adapter.Fill(table);
                return table;
            }
        }

        // Runs an INSERT, UPDATE or CREATE TABLE and returns how many rows it changed.
        public static int Execute(string sql, params object[] values)
        {
            using (OleDbConnection con = GetConnection())
            using (OleDbCommand cmd = new OleDbCommand(sql, con))
            {
                AddParameters(cmd, values);
                con.Open();
                return cmd.ExecuteNonQuery();
            }
        }

        // Runs a query that returns one value. Returns null when there is no row.
        public static object Scalar(string sql, params object[] values)
        {
            using (OleDbConnection con = GetConnection())
            using (OleDbCommand cmd = new OleDbCommand(sql, con))
            {
                AddParameters(cmd, values);
                con.Open();
                return cmd.ExecuteScalar();
            }
        }

        // Same as Scalar for queries like COUNT(*) and MAX(...), with an empty result read as 0.
        public static int GetNumber(string sql, params object[] values)
        {
            object value = Scalar(sql, values);
            return value == null || value == DBNull.Value ? 0 : Convert.ToInt32(value);
        }

        private static void AddParameters(OleDbCommand cmd, object[] values)
        {
            foreach (object value in values)
            {
                if (value == null || (value is string && (string)value == ""))
                {
                    // Empty text boxes are saved as empty (NULL) fields.
                    cmd.Parameters.Add("?", OleDbType.VarWChar).Value = DBNull.Value;
                }
                else if (value is DateTime)
                {
                    // Access rejects dates sent the default way (with milliseconds), so send them as Date.
                    cmd.Parameters.Add("?", OleDbType.Date).Value = value;
                }
                else
                {
                    cmd.Parameters.AddWithValue("?", value);
                }
            }
        }

        // Employee IDs look like EMP-0001 and count up from the last employee added.
        public static string NextEmployeeNo()
        {
            int last = GetNumber("SELECT MAX(EmployeeID) FROM Employees");
            return "EMP-" + (last + 1).ToString("0000");
        }

        // Creates employees.accdb and its tables the first time the app runs, if the file isn't there yet.
        // It checks every run, so a first run that stopped halfway (a file with no tables, so nobody
        // can sign in) is finished on the next run instead of staying broken.
        public static void EnsureDatabase()
        {
            if (!File.Exists(DbPath))
            {
                Type catalogType = Type.GetTypeFromProgID("ADOX.Catalog");
                if (catalogType == null)
                {
                    throw new InvalidOperationException("ADOX.Catalog (the part of Windows that creates Access files) is not available on this PC.");
                }

                dynamic catalog = Activator.CreateInstance(catalogType);
                catalog.Create(ConnectionString);
                catalog.ActiveConnection.Close();
            }

            if (!TableExists("Users"))
            {
                Execute("CREATE TABLE Users (UserID COUNTER PRIMARY KEY, Username TEXT(50), PasswordHash TEXT(255), FullName TEXT(100), UserRole TEXT(20), IsActive YESNO, EmployeeID LONG, DateCreated DATETIME)");
            }

            // The first account, so an admin can sign in and create everyone else's. Change this password after the first login.
            // It's added again only if there are no accounts at all, since then nobody could sign in.
            if (GetNumber("SELECT COUNT(*) FROM Users") == 0)
            {
                Execute("INSERT INTO Users (Username, PasswordHash, FullName, UserRole, IsActive, DateCreated) VALUES (?, ?, ?, ?, ?, ?)",
                    "admin", PasswordHelper.HashPassword("admin123"), "System Administrator", "Admin", true, DateTime.Now);
            }

            if (!TableExists("Employees"))
            {
                Execute("CREATE TABLE Employees (EmployeeID COUNTER PRIMARY KEY, EmployeeNo TEXT(20), FirstName TEXT(50), LastName TEXT(50), Gender TEXT(10), BirthDate DATETIME, Email TEXT(100), Phone TEXT(30), Address TEXT(255), Position TEXT(50), Department TEXT(50), HireDate DATETIME, EmploymentStatus TEXT(20), DateCreated DATETIME)");
                AddSampleEmployees();
            }
        }

        private static bool TableExists(string name)
        {
            using (OleDbConnection con = GetConnection())
            {
                con.Open();
                DataTable tables = con.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null, null, name, "TABLE" });
                return tables.Rows.Count > 0;
            }
        }

        // Sample records so the dashboard and reports have something to show. Delete this call to start empty.
        private static void AddSampleEmployees()
        {
            object[][] samples =
            {
                new object[] { "Maria", "Santos", "Female", new DateTime(1990, 4, 12), "HR Officer", "Human Resources", new DateTime(2019, 6, 3), "Active", "0917 555 0101", "Quezon City" },
                new object[] { "John", "Reyes", "Male", new DateTime(1988, 9, 25), "Accountant", "Finance", new DateTime(2018, 1, 15), "Active", "0917 555 0102", "Makati City" },
                new object[] { "Angela", "Cruz", "Female", new DateTime(1995, 2, 8), "Sales Associate", "Sales", new DateTime(2021, 3, 22), "Active", "0917 555 0103", "Pasig City" },
                new object[] { "Mark", "Villanueva", "Male", new DateTime(1992, 11, 30), "IT Support", "IT", new DateTime(2020, 8, 10), "Active", "0917 555 0104", "Taguig City" },
                new object[] { "Patricia", "Lim", "Female", new DateTime(1985, 7, 19), "Supervisor", "Operations", new DateTime(2016, 5, 2), "Active", "0917 555 0105", "Manila" },
                new object[] { "Daniel", "Garcia", "Male", new DateTime(1997, 12, 3), "Sales Associate", "Sales", new DateTime(2022, 10, 17), "Inactive", "0917 555 0106", "Mandaluyong City" },
                new object[] { "Kristine", "Mendoza", "Female", new DateTime(1993, 3, 14), "Accountant", "Finance", new DateTime(2023, 2, 6), "Active", "0917 555 0107", "Quezon City" },
                new object[] { "Paolo", "Ramos", "Male", new DateTime(1991, 6, 21), "IT Support", "IT", new DateTime(2017, 9, 11), "Inactive", "0917 555 0108", "Pasay City" },
            };

            foreach (object[] s in samples)
            {
                Execute("INSERT INTO Employees (EmployeeNo, FirstName, LastName, Gender, BirthDate, Email, Phone, Address, Position, Department, HireDate, EmploymentStatus, DateCreated) VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)",
                    NextEmployeeNo(), s[0], s[1], s[2], s[3], (s[0] + "." + s[1] + "@company.com").ToLower(), s[8], s[9], s[4], s[5], s[6], s[7], DateTime.Now);
            }
        }
    }
}
