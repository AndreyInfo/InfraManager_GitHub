using System.Collections.Generic;
using System.Linq;
using DevExpress.DataAccess.ConnectionParameters;
using DevExpress.DataAccess.Sql;
using DevExpress.DataAccess.Web;
using InfraManager.DAL.Settings;

namespace InfraManager.DAL.DevExpress;

//TODO: Вынести в отдельную сборку и убрать зависимость от DevExpress
public class CustomDevExpressConnectionStringProvider : IDataSourceWizardConnectionStringsProvider
{
    private readonly Dictionary<string, IDevExpressDbConnectionParameters> _dbConnectionParameters = new()
    {
        { "pg", new PostgresDbConnectionParameters() },
        { "mssql", new MsSqlDbConnectionParameters() },
        { "ms", new MsSqlDbConnectionParameters() }
    };
    
    private readonly string _connectionString;
    private readonly string _dbType;
    private readonly Dictionary<string, DataConnectionParametersBase> _connectionStrings = new();
    private readonly IConnectionStringProvider _connectionStringProvider;

    public CustomDevExpressConnectionStringProvider(IConnectionStringProvider connectionString, string dbType)
    {
        _connectionStrings.Add("Current Database",
            new CustomStringConnectionParameters(connectionString.GetConnectionString()));
        
        _connectionString = connectionString.GetConnectionString();
        _dbType = dbType;
        _connectionStringProvider = connectionString;
    }
    
    public Dictionary<string, string> GetConnectionDescriptions()
    {
        return _connectionStrings.Keys.ToDictionary(key => key, key => key);
    }

    public Dictionary<string, object> GetReportDataSource()
    {
        return new Dictionary<string, object>
        {
            {
                "Current Database", new SqlDataSource(GetDataConnectionParameters(""))
            }
        };
    }

    public DataConnectionParametersBase GetDataConnectionParameters(string name)
    {
        return _dbConnectionParameters[_dbType]
            .GetConnectionParameters(_connectionStringProvider.GetConnectionObject());
    }
}