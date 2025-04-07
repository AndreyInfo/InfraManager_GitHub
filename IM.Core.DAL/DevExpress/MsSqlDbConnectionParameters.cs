using DevExpress.DataAccess.ConnectionParameters;

namespace InfraManager.DAL.DevExpress;

public class MsSqlDbConnectionParameters : IDevExpressDbConnectionParameters
{
    public SqlServerConnectionParametersBase GetConnectionParameters(
        (string Server, string Database, string Login, string Password, int Port, int CommandTimeout)
            connectionParameters)
    {
        return new MsSqlConnectionParameters(connectionParameters.Server,
            connectionParameters.Database, connectionParameters.Login, connectionParameters.Password,
            MsSqlAuthorizationType.SqlServer);
    }
}