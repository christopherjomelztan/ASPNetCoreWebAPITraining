namespace ASPNetCoreWebAPITraining
{
    /// <summary>
    /// This static class contains shared constants and readonly variables to be used within the solution to prevent duplications.
    /// </summary>
    public static class StaticConfiguration
    {
        public static readonly Version MySqlVersion = new Version(8, 3, 0);
        public const string SqlServer = "SqlServer";
        public const string MySql = "MySql";
        public const string SQLite3 = "SQLite3";
        public const string MicrosoftAccess = "MicrosoftAccess";

    }
}