namespace Venux
{
    class Daten
    {
        public static string database = "";
        public static string username = "";
        public static string password = "";
        public static string host = "";

        public static void setDatabaseData()
        {
            if (Start.ReleaseBuild)
            {
                database = "venux";
                username = "root";
                password = "";
                host = "localhost";
            }
            else
            {
                database = "venux";
                username = "root";
                password = "";
                host = "localhost";
            }
        }

    }
}
