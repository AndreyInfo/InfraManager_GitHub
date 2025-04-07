using System;
using IM.Core.DAL.Postgres;
using InfraManager.DAL;
using Microsoft.Extensions.Configuration;
using Npgsql;
using InfraManager.DAL.DbConfiguration;

namespace IM.Core.DAL.PGSQL
{
    public class PgSqlConnectionStringProvider : ConnectionStringProviderWithEncryption
    {
        private readonly string connectionStringPlain;

        protected override string ConnectionStringSettingName => "im-pg";

        public PgSqlConnectionStringProvider(IConfiguration configuration,
            IAppSettingsEditor appSettingsEditor,
            IOldDataSourceLocatorEditor oldDataSourceLocatorEditor)
            : base(configuration, appSettingsEditor, oldDataSourceLocatorEditor)
        {
            connectionStringPlain = SetCleanString();
        }

        private string SetCleanString()
        {
            var stringBuilder = new NpgsqlConnectionStringBuilder(connectionString);
            if (!string.IsNullOrEmpty(user) || !string.IsNullOrEmpty(password))
                stringBuilder.IntegratedSecurity = false;
            if (!string.IsNullOrEmpty(user))
                stringBuilder.Username = Decrypt(user);
            if (!string.IsNullOrEmpty(password))
                stringBuilder.Password = Decrypt(password);

            return stringBuilder.ConnectionString;
        }

        public override (string Server, string Database, string Login, string Password, int Port, int CommandTimeout) GetConnectionObject()
        {
            var stringBuilder = new NpgsqlConnectionStringBuilder(connectionStringPlain);

            return new(
                stringBuilder.Host,
                stringBuilder.Database,
                stringBuilder.Username,
                stringBuilder.Password,
                stringBuilder.Port,
                stringBuilder.CommandTimeout
            );
        }

        public override void ChangeConnectionString(string connectionString)
        {
            var connectionStringBuilder = new NpgsqlConnectionStringBuilder(connectionString);

            var login = connectionStringBuilder.Username;
            var password = connectionStringBuilder.Password;
            connectionStringBuilder.Username = null;
            connectionStringBuilder.Password = null;
            
            BaseChangeConnectionString(connectionStringBuilder.ConnectionString, login, password);
            BaseChangeOldDataSource(new PgSqlOldDataSourceCreator().Create(connectionStringBuilder, Enc(password), Enc(login)));
        }

        public override string BuildConnectionString(string server, string database, string login, string password,
            string additionalField, int port)
        {
            var defaultDatabase = "template1";
            if (string.IsNullOrEmpty(server))
            {
                throw new Exception("Server was null");
            }

            var connectionStringBuilder = new NpgsqlConnectionStringBuilder
            {
                IntegratedSecurity = false,
                Host = server,
                Username = string.IsNullOrEmpty(login) ? Decrypt(this.user) : login,
                Password = string.IsNullOrEmpty(password) ? Decrypt(this.password) : password,
                Database = string.IsNullOrEmpty(database) ? defaultDatabase : database,
                Port = port
            };

            if (string.IsNullOrEmpty(additionalField))
            {
                return connectionStringBuilder.ConnectionString;
            }

            return string.Concat(connectionStringBuilder.ConnectionString, ";", additionalField);
        }

        public override string GetConnectionString()
        {
            return connectionStringPlain;
        }

        private string Enc(string clear)
        {
            return Encrypt(clear);
        }
    }
}
