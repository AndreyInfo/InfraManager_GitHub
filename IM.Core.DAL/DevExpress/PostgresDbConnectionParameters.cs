using DevExpress.DataAccess.ConnectionParameters;

namespace InfraManager.DAL.DevExpress;

public class PostgresDbConnectionParameters : IDevExpressDbConnectionParameters
{
    public SqlServerConnectionParametersBase GetConnectionParameters(
        (string Server, string Database, string Login, string Password, int Port, int CommandTimeout) connectionParameters)
    {
        return new PostgreSqlConnectionParameters(connectionParameters.Server, connectionParameters.Port,
            connectionParameters.Database, connectionParameters.Login, connectionParameters.Password);
    }
}