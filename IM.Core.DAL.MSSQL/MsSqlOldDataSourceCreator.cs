using Microsoft.Data.SqlClient;

namespace IM.Core.DAL.Microsoft.SqlServer;

public class MsSqlOldDataSourceCreator
{
    private string OldDataSourceLocatorTemplate =>
        "<?xml version=\"1.0\" encoding=\"utf-8\"?>\n<imf:dataSourceLocators xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" version=\"5.2.1\" skip=\"false\" maxItemsCount=\"255\" xmlns:imf=\"urn:InfraManager.Framework\">\n  <imf:sqlServerDataSourceLocator server=\"{0}\" database=\"{1}\" user=\"{2}\" password=\"{3}\" connectionTimeout=\"5\" commandTimeout=\"60\" disabled=\"false\" useSnapshotIsolationLevel=\"false\" />\n</imf:dataSourceLocators>";
    
    public string Create(SqlConnectionStringBuilder connectionStringBuilder, string encryptedPassword,
        string encryptedLogin)
    {
        return string.Format(OldDataSourceLocatorTemplate, connectionStringBuilder.DataSource,
            connectionStringBuilder.InitialCatalog, encryptedLogin, encryptedPassword);
    }
}