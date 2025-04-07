using Npgsql;

namespace IM.Core.DAL.Postgres;

public class PgSqlOldDataSourceCreator
{
    private string OldDataSourceLocatorTemplate =>
        "<?xml version=\"1.0\" encoding=\"utf-8\"?>\n<imf:dataSourceLocators xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" version=\"5.2.1\" skip=\"false\" maxItemsCount=\"1\" xmlns:imf=\"urn:InfraManager.Framework\">\n  <imf:postgreSqlDataSourceLocator\n      server=\"{0}\" database=\"{1}\" user=\"{2}\" password=\"{3}\" Port=\"{4}\" connectionTimeout=\"5\"\n      commandTimeout=\"180\" disabled=\"false\" SslMode=\"{5}\" TrustServerCertificate=\"{6}\" useSnapshotIsolationLevel=\"false\"\n  />\n</imf:dataSourceLocators>";

    public string Create(NpgsqlConnectionStringBuilder connectionStringBuilder, string encryptedPassword,
        string encryptedLogin)
    {
        return string.Format(OldDataSourceLocatorTemplate, connectionStringBuilder.Host,
            connectionStringBuilder.Database, encryptedLogin, encryptedPassword, connectionStringBuilder.Port,
            connectionStringBuilder.SslMode, connectionStringBuilder.TrustServerCertificate.ToString().ToLower());
    }
}