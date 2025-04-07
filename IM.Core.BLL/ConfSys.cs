namespace InfraManager.BLL
{
    public class ConfSys
    {
        public static string DBMS { get; private set; }

        public static void InitDBMS(string dbType)
        {
            if (dbType == "pg")
                DBMS = "PostgreSQL";
            else if (dbType == "mssql" || dbType == "ms")
                DBMS = "MS SQL";
            else
                DBMS = "";
        }
    }
}
