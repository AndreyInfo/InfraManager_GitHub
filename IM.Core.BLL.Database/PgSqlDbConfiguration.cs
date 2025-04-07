using System.Data;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;
using InfraManager.BLL;
using InfraManager.DAL;
using InfraManager.DAL.Configuration;
using InfraManager.DAL.Settings;
using Microsoft.Extensions.Logging;
using Npgsql;

namespace IM.Core.BLL.Database
{
    public class PgSqlDbConfiguration : IDbConfiguration
    { 
        private readonly IConnectionStringProvider _connectionStringProvider;
        private readonly ILogger<PgSqlDbConfiguration> _logger;

        public PgSqlDbConfiguration(IConnectionStringProvider connectionStringProvider,
            ILogger<PgSqlDbConfiguration> logger)
        {
            _connectionStringProvider = connectionStringProvider;
            _logger = logger;
        }
        
        #region CheckConnection

        public async Task<bool> CheckConnectionAsync(string server, int port, string databaseName = null, string login = null,
            string password = null, string additionalField = null, CancellationToken cancellationToken = default)
        {
            var connectionString = BuildConnectionString(server, port, databaseName, login, password, additionalField);
            return await CheckConnectionAsync(connectionString, cancellationToken);
        }

        public async Task<bool> CheckConnectionAsync(string connectionString,
            CancellationToken cancellationToken = default)
        {
            try
            {
                await using (var connect = new NpgsqlConnection(connectionString))
                {
                    await connect.OpenAsync(cancellationToken);
                    await connect.CloseAsync();
                }

                return true;
            }
            catch (Exception e)
            {
                var connectionStringBuilder = new NpgsqlConnectionStringBuilder(connectionString)
                {
                    Username = null,
                    Password = null
                };
                
                _logger.LogError(e, $"No connection to pgsql server, connection string = {connectionStringBuilder}");
                return false;
            }
        }

        private async Task ThrowIfNoConnectionAsync(string connectionString, CancellationToken cancellationToken = default)
        {
            var connectionExists = await CheckConnectionAsync(connectionString, cancellationToken);

            if (!connectionExists)
            {
                throw new InvalidObjectException("Нет подключения к базе данных"); //TODO локализация
            }
        }

        private async Task ThrowIfNoConnectionAsync(string server, int port, string databaseName = null, string login = null,
            string password = null, CancellationToken cancellationToken = default)
        {
            var connectionString = BuildConnectionString(server, port, databaseName, login, password);
            await CheckConnectionAsync(connectionString, cancellationToken);
        } 
        #endregion

        #region Connection

        public string BuildConnectionString(string server, int port, string databaseName = null, string login = null,
            string password = null, string additionalField = null)
        {
            return _connectionStringProvider.BuildConnectionString(server, databaseName, login, password,
                additionalField, port);
        }

        #endregion

        #region DatabasesList

        public async Task<string[]> LoadDataBasesAsync(string server, int port, string databaseName = null, string login = null,
            string password = null, string additionalField = null, CancellationToken cancellationToken = default)
        {
            return await GetDatabasesAsync(server, port, databaseName, login, password, additionalField,
                cancellationToken);
        }

        private async Task<string[]> GetDatabasesAsync(string server, int port, string databaseName = null, string login = null,
            string password = null, string additionalField = null, CancellationToken cancellationToken = default)
        {
            var connectionString = BuildConnectionString(server, port, databaseName, login, password, additionalField);
            await ThrowIfNoConnectionAsync(connectionString, cancellationToken);

            var databases = new List<string>();
            
            await using (var connection = new NpgsqlConnection(connectionString))
            {
                await connection.OpenAsync(cancellationToken);
                var sqlCommand = @"SELECT datname FROM pg_database
WHERE datistemplate = false";

                using (var command = connection.CreateCommand())
                {
                    command.CommandTimeout = 60;
                    command.CommandType = CommandType.Text;
                    command.CommandText = sqlCommand;
                    var reader = await command.ExecuteReaderAsync(cancellationToken);

                    if (reader.HasRows)
                    {
                        while (await reader.ReadAsync(cancellationToken))
                        {
                            databases.Add(reader.GetValue(0).ToString());   
                        }
                    }

                    await reader.CloseAsync();
                }
                
                await connection.CloseAsync();
            }

            return await GetIMDatabasesAsync(databases, server, port, login, password, additionalField,
                cancellationToken);
        }


        private async Task<string[]> GetIMDatabasesAsync(List<string> databases, string server, int port,
            string login = null, string password = null, string additionalField = null,
            CancellationToken cancellationToken = default)
        {
            var imDatabases = new List<string>();
            var isIM = false;
            var sqlCommand = "select version from db_info";
            foreach (var el in databases)
            {
                try
                {
                    var connectionString = BuildConnectionString(server, port, el, login, password, additionalField);
                    await using (var connection = new NpgsqlConnection(connectionString))
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
                            imDatabases.Add(el);
                        }

                        await connection.CloseAsync();

                    }
                }
                catch (Exception)
                {
                    continue;
                }
            }
            return imDatabases.ToArray();
        }
        
        #endregion
        
        #region ConnectionToDatabase

        public void ConnectToDatabase(string server, int port, string databaseName = null, string login = null,
            string password = null, string additionalField = null)
        {
            var connectionString = BuildConnectionString(server, port, databaseName, login, password, additionalField);
            _connectionStringProvider.ChangeConnectionString(connectionString);
        }

        #endregion

        #region RestoreDatabase

        public async Task RestoreDatabaseAsync(string server, int port, DbRestoreType dbRestoreType,
            string databaseName = null, string login = null, string password = null,
            CancellationToken cancellationToken = default)
        {
            var connectionString = BuildConnectionString(server, port, "template1", login, password);
            await ThrowIfNoConnectionAsync(connectionString, cancellationToken);

            try
            {
                await using (var connection = new NpgsqlConnection(connectionString))
                {
                    await connection.OpenAsync(cancellationToken);
                    await CreateDatabaseAsync(connection, databaseName, cancellationToken);
                    RestoreDatabase(server, dbRestoreType, databaseName, password);
                    var isExists = await isRoleExistsAsync(connection, cancellationToken);
                    if(!isExists)
                    {
                        await CreateLoginAsync(connection, cancellationToken);   
                    }
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e,$"Restore db Error {databaseName} {login}");
                throw new InvalidObjectException("Не удалось восстановить базу данных");
            }
        }

        private async Task CreateDatabaseAsync(NpgsqlConnection connection, string databaseName, CancellationToken cancellationToken = default)
        {
            var database = $"\"{databaseName}\"";
            var sqlCommand = $@"CREATE DATABASE {database}
    WITH 
    OWNER = postgres
    TEMPLATE = template0
    ENCODING = 'UTF8'
    LC_COLLATE = 'C'
    LC_CTYPE = 'C'
    TABLESPACE = pg_default
    CONNECTION LIMIT = -1;";

            using (var command = connection.CreateCommand())
            {
                command.CommandTimeout = 60;
                command.CommandType = CommandType.Text;
                command.CommandText = sqlCommand;                    
                await command.ExecuteNonQueryAsync(cancellationToken);
            }
        }

        private void RestoreDatabase(string server, DbRestoreType dbRestoreType, string databaseName, string password)
        {
            var windowsCMD = "cmd.exe";
            var linuxCMD = "/bin/bash";
            
            var command = GetRestoreCommand(password, server, databaseName);
            var fileName = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? windowsCMD : linuxCMD;
            using (var proc = new Process())
            {
                proc.StartInfo.FileName = fileName;
                proc.StartInfo.Arguments = command;
                proc.Start();

                proc.WaitForExit();
            }
        }

        private string GetRestoreCommand(string password, string server, string databaseName)
        {
            var isWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
            var set = isWindows ? "/C set " : "-c \"export ";
            var psqlPath = Path.Join(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),"PsqlDLLs");
            var backupFilePath = isWindows ? $"cd /d {psqlPath}" : $"cd {psqlPath}";
            
            string command = $"{set} PGPASSWORD={password}&& " +
                             @$"{backupFilePath} && " +
                             $"pg_restore -h {server} -d {databaseName.ToLower()} -U postgres -p 5432 < backup.tar";

            if (!isWindows)
            {
                command += "\"";
            }
                                 
            return command;
        }
        
        private async Task CreateLoginAsync(NpgsqlConnection connection,CancellationToken cancellationToken = default)
        {
            var sqlCommand = @"CREATE ROLE im WITH
	LOGIN
	SUPERUSER
	CREATEDB
	NOCREATEROLE
	INHERIT
	REPLICATION
	CONNECTION LIMIT -1
	PASSWORD 'jd89$32#90JHgwjn%MLwhb3b';";
            
            using (var command = connection.CreateCommand())
            {
                command.CommandTimeout = 60;
                command.CommandType = CommandType.Text;
                command.CommandText = sqlCommand;                    
                await command.ExecuteNonQueryAsync(cancellationToken);
            }
        }

        private async Task<bool> isRoleExistsAsync(NpgsqlConnection connection, CancellationToken cancellationToken = default)
        {
            var sqlCommand = @"SELECT 1 FROM pg_roles WHERE rolname='im'";
            int result;
            using (var command = connection.CreateCommand())
            {
                command.CommandTimeout = 60;
                command.CommandType = CommandType.Text;
                command.CommandText = sqlCommand;                    
                var roleExists = await command.ExecuteScalarAsync(cancellationToken); 
                result = roleExists is null ? -1 : (int)roleExists;
            }

            return result == 1;
        }
        
        #endregion
    }
}