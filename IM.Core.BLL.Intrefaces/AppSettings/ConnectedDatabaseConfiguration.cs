namespace InfraManager.BLL.AppSettings;

public class ConnectedDatabaseConfiguration
{
    public ConnectedDatabaseConfiguration(string dbName, string serverName)
    {
        DbName = dbName;
        ServerName = serverName;
    }
    
    public string DbName { get; }
    public string ServerName { get; }
}