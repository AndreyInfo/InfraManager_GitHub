using System.Data;
using System.Reflection;
using InfraManager.BLL;
using InfraManager.DAL;
using InfraManager.DAL.Configuration;
using InfraManager.DAL.Settings;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace IM.Core.BLL.Database
{
    public class MsSqlDbConfiguration : IDbConfiguration
    {

        private readonly IConnectionStringProvider _connectionStringProvider;
        private readonly ILogger<MsSqlDbConfiguration> _logger;
        private readonly string _dbFilesPath;
        private readonly string _dbFilesSave;

        public MsSqlDbConfiguration(IConnectionStringProvider connectionStringProvider,
            IConfiguration configuration,
            ILogger<MsSqlDbConfiguration> logger)
        {
            _connectionStringProvider = connectionStringProvider;
            _logger = logger;
            _dbFilesSave = configuration["DatabaseFilesSave"];
            _dbFilesPath = configuration["DatabaseFilesPath"];
        }

        #region Consts
        private const int MinimalVersion = 11;
        private const string MasterDatabase = "master";
        #endregion

        #region CheckConnection

        public async Task<bool> CheckConnectionAsync(string connectionString, CancellationToken cancellationToken = default)
        {
            try
            {
                await using (var connect = new SqlConnection(connectionString))
                {
                    await connect.OpenAsync(cancellationToken);
                    await connect.CloseAsync();
                }

                return true;
            }
            catch (Exception e)
            {
                var connectionStringBuilder = new SqlConnectionStringBuilder(connectionString)
                {
                    UserID = null,
                    Password = null
                };
                
                _logger.LogError(e, $"No connection to mssql server, connection string = {connectionStringBuilder}");
                return false;
            }
        }
        
        public async Task<bool> CheckConnectionAsync(string server, int port, string databaseName = null, string login = null,
            string password = null, string additionalField = null, CancellationToken cancellationToken = default) //Think mb return something about why is no connection
        {
            var connectionString = BuildConnectionString(server, port, databaseName, login, password, additionalField);
            return await CheckConnectionAsync(connectionString, cancellationToken);
        }

        private async Task ThrowIfNoConnectionAsync(string server, int port, string databaseName = null,
            string login = null, string password = null, CancellationToken cancellationToken = default)
        {
            var connectionString = BuildConnectionString(server, port, databaseName, login, password);
            await ThrowIfNoConnectionAsync(connectionString, cancellationToken);
        }

        private async Task ThrowIfNoConnectionAsync(string connectionString, CancellationToken cancellationToken = default)
        {
            var connection = await CheckConnectionAsync(connectionString, cancellationToken);

            if (!connection)
            {
                throw new Exception("No connection to MsSql server");
            }
        }
        #endregion

        #region ConnectionStringBuilder

        public string BuildConnectionString(string server, int port, string databaseName = null,
            string login = null, string password = null, string additionalField = null)
        {
            return _connectionStringProvider.BuildConnectionString(server, databaseName, login, password,
                additionalField, port);
        }

        #endregion
        
        #region DatabasesList

        public async Task<string[]> LoadDataBasesAsync(string server, int port, string databaseName = null, string login = null,
            string password = null, string additionalField = null, CancellationToken cancellationToken = default)
        {
            var dbConnection = await CheckConnectionAsync(server, port, databaseName, login, password, additionalField,
                cancellationToken);

            if (!dbConnection)
            {
                _logger.LogTrace($"No DB Connection {databaseName} {login}");
                throw new InvalidObjectException("Нет подключения к базе данных"); //TODO локализация
            }

            return await GetDatabasesAsync(server, port, databaseName, login, password, additionalField,
                cancellationToken);
        }

        private async Task<string[]> GetDatabasesAsync(string server, int port, string databaseName = null,
            string login = null, string password = null, string additionalField = null,
            CancellationToken cancellationToken = default)
        {
            string[] dataBases;

            string connectionString = BuildConnectionString(server, port, databaseName, login, password);
            using (var connection =
                   new SqlConnection(connectionString))
            {
                await connection.OpenAsync(cancellationToken);

                var databasesRequest = connection.GetSchema("Databases");

                var allDatabases = new List<string>();
                foreach (DataRow el in databasesRequest.Rows)
                {
                    allDatabases.Add(el[0].ToString());
                }

                dataBases = await GetIMDatabasesAsync(allDatabases, server, port, login, password, additionalField,
                    cancellationToken);
                
                await connection.CloseAsync();
            }

            return dataBases;
        }

        private async Task<string[]> GetIMDatabasesAsync(List<string> databases, string server, int port,
            string login = null, string password = null, string additionalField = null,
            CancellationToken cancellationToken = default)
        {
            var imDatabases = new List<string>();
            var sqlCommand = FileHelper("GetDbVersion.sql");
            bool isIM;
            
            foreach (var database in databases)
            {
                try
                {
                    var connectionString =
                        BuildConnectionString(server, port, database, login, password, additionalField);
                    
                    using (var connection =
                           new SqlConnection(connectionString))
                    {
                        await connection.OpenAsync(cancellationToken);
                        using (var command = connection.CreateCommand())
                        {
                            command.CommandTimeout = 60;
                            command.CommandType = CommandType.Text;
                            command.CommandText = sqlCommand;
                            isIM = await command.ExecuteScalarAsync(cancellationToken) is not DBNull;
                        }

                        if (isIM)
                        {
                            imDatabases.Add(database);
                        }
                    }
                }
                catch (Exception)
                {
                }
            }

            return imDatabases.ToArray();
        }
        #endregion
        
        #region RestoreDB

        public async Task RestoreDatabaseAsync(string server, int port, DbRestoreType dbRestoreType,
            string databaseName = null, string login = null, string password = null,
            CancellationToken cancellationToken = default)
        {
            var connectionString = BuildConnectionString(server, port, MasterDatabase, login, password); 
            
            await ThrowIfNoConnectionAsync(connectionString, cancellationToken);

            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync(cancellationToken);

                    await ThrowIfNoCreatePermissionsAsync(connection, cancellationToken);
                    await ThrowIfInvalidVersionAsync(connection, cancellationToken);
                    await DeployDatabaseAsync(connection, databaseName, dbRestoreType, cancellationToken);
                    await AddIMUserAsync(connection, cancellationToken);
                    await AddDefaultAuthenticateUserAsync(connection, databaseName, cancellationToken);
                    await SetMultiUserAsync(connection, databaseName, cancellationToken);

                    await connection.CloseAsync();
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Restore db Error {databaseName} {login}");
                throw new InvalidObjectException("Не удалось восстановить базу данных"); //TODO locale
            }

        }


        private async Task ThrowIfNoCreatePermissionsAsync(SqlConnection connection, CancellationToken cancellationToken = default)
        {
            int permissionResult;

            var sqlCommand = "SELECT has_perms_by_name(null, null, 'CREATE ANY DATABASE');";
            using (var command = connection.CreateCommand())
            {
                command.CommandTimeout = 60;
                command.CommandType = CommandType.Text;
                command.CommandText = sqlCommand;
                var result = await command.ExecuteScalarAsync(cancellationToken);
                permissionResult = (int)result;
            }
            
            if (permissionResult != 1)
            {
                _logger.LogInformation($"No rights to restore DB");
                throw new InvalidObjectException("Нет прав на создание баз данных");//TODO locale
            }
        }

        private async Task ThrowIfInvalidVersionAsync(SqlConnection connection, CancellationToken cancellationToken = default)
        {
            int sqlServerVersion = 0;
            using (var command = connection.CreateCommand())
            {
                var sqlCommand = "select @@MICROSOFTVERSION / 0x01000000";
                command.CommandTimeout = 60;
                command.CommandType = CommandType.Text;
                command.CommandText = sqlCommand;
                //
                var sqlServerVersionResult = await command.ExecuteScalarAsync(cancellationToken);

                sqlServerVersion = sqlServerVersionResult is DBNull ? -1 : (int)sqlServerVersionResult;
            }

            if (sqlServerVersion < MinimalVersion)
            {
                _logger.LogTrace("Invalid SqlServer Version");
                throw new InvalidCastException("Invalid SqlServer Version");
            }
        }

        private async Task DeployDatabaseAsync(SqlConnection connection, string database, DbRestoreType dbRestoreType,
            CancellationToken cancellationToken = default)
        {
            using (var command = connection.CreateCommand())
            {
                command.CommandTimeout = 0;
                command.CommandType = CommandType.Text;
                command.CommandText = string.Format(
                    @"
IF EXISTS(SELECT * FROM master.sys.databases WHERE [name] = N'{0}')
    ALTER DATABASE [{0}] SET SINGLE_USER WITH ROLLBACK IMMEDIATE
", database);
                await command.ExecuteNonQueryAsync(cancellationToken);
            }

            var mdfFile = Path.Join(_dbFilesSave, $"{database}.mdf");
            var ldfFile = Path.Join(_dbFilesSave, $"{database}_log.ldf");
            var dbFilePath = Path.Join(_dbFilesPath, GetRestoreFileName(dbRestoreType));
            {
                using (var command = connection.CreateCommand())
                {
                    command.CommandTimeout = 0;
                    command.CommandType = CommandType.Text;
                    command.CommandText = FileHelper("RestoreDb.sql");
                    command.Parameters.Add(new SqlParameter("@Database", database));
                    command.Parameters.Add(new SqlParameter("@File", dbFilePath));
                    command.Parameters.Add(new SqlParameter("@MDFFile", mdfFile));
                    command.Parameters.Add(new SqlParameter("@LDFFile", ldfFile));
                    await command.ExecuteNonQueryAsync(cancellationToken);
                }
            }
        }

        private string GetRestoreFileName(DbRestoreType dbRestoreType)
        {
            switch (dbRestoreType)
            {
                case DbRestoreType.Demo:
                {
                    return "IMDemo.bak";
                }
                case DbRestoreType.Empty:
                {
                    return "IMBlank.bak";
                }
            }
            return null;
        }

        private async Task AddIMUserAsync(SqlConnection connection, CancellationToken cancellationToken = default)
        {
            using (var command = connection.CreateCommand())
            {
                command.CommandTimeout = 0;
                command.CommandType = CommandType.Text;
                command.CommandText = FileHelper("CreateLogin.sql");
                //
                await command.ExecuteNonQueryAsync(cancellationToken);
            }
        }

        private async Task AddDefaultAuthenticateUserAsync(SqlConnection connection, string databaseName,
            CancellationToken cancellationToken = default)
        {
            using (var command = connection.CreateCommand())
            {
                command.CommandTimeout = 0;
                command.CommandType = CommandType.Text;
                command.CommandText = FileHelper("CreateUser.sql");
                command.Parameters.Add(new SqlParameter("@Database", databaseName));
                command.Parameters.Add(new SqlParameter("@Login", "im"));
                command.Parameters.Add(new SqlParameter("@User", "im"));
                //
                await command.ExecuteNonQueryAsync(cancellationToken);
            }
        }

        private async Task SetMultiUserAsync(SqlConnection connection, string databaseName, CancellationToken cancellationToken = default)
        {
            using (var command = connection.CreateCommand())
            {
                command.CommandTimeout = 0;
                command.CommandType = CommandType.Text;
                command.CommandText = string.Format(
                    @"
IF EXISTS(SELECT * FROM master.sys.databases WHERE [name] = N'{0}')
    ALTER DATABASE [{0}] SET MULTI_USER
", databaseName);
                await command.ExecuteNonQueryAsync(cancellationToken);
            }
        }
        
        #endregion

        #region ConnectionToDatabase

        public void ConnectToDatabase(string server,int port, string databaseName = null, string login = null,
            string password = null, string additionalField = null)
        {
            var connectionString = BuildConnectionString(server, port, databaseName, login, password, additionalField);
            _connectionStringProvider.ChangeConnectionString(connectionString);
        }

        #endregion

        private string FileHelper(string fileName)
        {
            var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            return File.ReadAllText($"{path}/Scripts/{fileName}");
        }
    }
}