using System;
using IM.Core.DAL.Microsoft.SqlServer;
using InfraManager.DAL.DbConfiguration;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace InfraManager.DAL.Microsoft.SqlServer
{
    public class MsSqlConnectionStringProvider : ConnectionStringProviderWithEncryption
    {
        private readonly string connectionStringPlain;

        protected override string ConnectionStringSettingName => "im-ms";

        public MsSqlConnectionStringProvider(IConfiguration configuration,
            IAppSettingsEditor appSettingsEditor,
            IOldDataSourceLocatorEditor oldDataSourceLocatorEditor) 
            : base(configuration, appSettingsEditor,
                oldDataSourceLocatorEditor)
        {
            connectionStringPlain = SetCleanString();
        }

        private string SetCleanString()
        {
            SqlConnectionStringBuilder stringBuilder = new SqlConnectionStringBuilder(connectionString);
            if (!string.IsNullOrEmpty(user) || !string.IsNullOrEmpty(password))
                stringBuilder.IntegratedSecurity = false;
            if (!string.IsNullOrEmpty(user))
                stringBuilder.UserID = Decrypt(user);
            if (!string.IsNullOrEmpty(password))
                stringBuilder.Password = Decrypt(password);
            return stringBuilder.ConnectionString;
        }

        public override (string Server, string Database, string Login, string Password, int Port, int CommandTimeout) GetConnectionObject()
        {
            const string default_port = "1433";

            var stringBuilder = new SqlConnectionStringBuilder(connectionStringPlain);

            var listParams = stringBuilder.DataSource.Split(new[] { ',' });

            var server = listParams[0];
            var port = (listParams.Length > 1) ? listParams[1] : default_port;

            return new(
                server,
                stringBuilder.InitialCatalog,
                stringBuilder.UserID,
                stringBuilder.Password,
                port.ToInt(),
                stringBuilder.CommandTimeout
            );
        }

        public override void ChangeConnectionString(string connectionString)
        {
            var connectionStringBuilder = new SqlConnectionStringBuilder(connectionString);

            var login = connectionStringBuilder.UserID;
            var password = connectionStringBuilder.Password;
            connectionStringBuilder.UserID = "";
            connectionStringBuilder.Password = "";

            BaseChangeConnectionString(connectionStringBuilder.ConnectionString, login, password);
            BaseChangeOldDataSource(new MsSqlOldDataSourceCreator().Create(connectionStringBuilder, Enc(password), Enc(login)));
        }

        public override string BuildConnectionString(string server, string database, string login, string password,
            string additionalField, int port)
        {
            var master = "master";
            if (string.IsNullOrEmpty(server))
            {
                throw new Exception("Server was null");
            }

            var connectionStringBuilder = new SqlConnectionStringBuilder(connectionString)
            {
                IntegratedSecurity = false,
                DataSource = string.Join(',', server, port.ToString()),
                UserID = string.IsNullOrEmpty(login) ? Decrypt(this.user) : login,
                Password = string.IsNullOrEmpty(password) ? Decrypt(this.password) : password,
                InitialCatalog = string.IsNullOrEmpty(database) ? master : database
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

        public string Enc(string clear)
        {
            return Encrypt(clear);
        }
    }
}
