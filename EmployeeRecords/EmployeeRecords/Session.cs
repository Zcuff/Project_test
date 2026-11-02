namespace EmployeeRecords
{
    // Who is signed in right now. LoginForm fills it in and MainForm clears it on logout.
    public static class Session
    {
        public static int UserID;
        public static string Username = "";
        public static string FullName = "";
        public static string Role = "";

        public static void Clear()
        {
            UserID = 0;
            Username = "";
            FullName = "";
            Role = "";
        }
    }
}
