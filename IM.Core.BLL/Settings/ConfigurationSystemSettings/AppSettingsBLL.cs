using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using InfraManager.BLL.AccessManagement;
using InfraManager.BLL.AppSettings;
using InfraManager.DAL;
using InfraManager.DAL.Configuration;
using InfraManager.DAL.DbConfiguration;
using InfraManager.DAL.Settings;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace InfraManager.BLL.Settings.ConfigurationSystemSettings
{
    internal class AppSettingsBLL : IAppSettingsBLL, ISelfRegisteredService<IAppSettingsBLL>
    {
        private readonly IDbConfiguration _dbConfiguration;
        private readonly IConnectionStringProvider _connectionStringProvider;
        private readonly IAppSettingsEditor _appSettingsEditor;
        private readonly IMapper _mapper;
        private readonly ICurrentUser _currentUser;
        private readonly IUserAccessBLL _access;
        private const string SettingsKey = "Settings";
        
        public AppSettingsBLL(IDbConfiguration dbConfiguration,
            IConnectionStringProvider connectionStringProvider,
            IAppSettingsEditor appSettingsEditor,
            IMapper mapper,
            ICurrentUser currentUser,
            IUserAccessBLL access)
        {
            _dbConfiguration = dbConfiguration;
            _connectionStringProvider = connectionStringProvider;
            _appSettingsEditor = appSettingsEditor;
            _mapper = mapper;
            _currentUser = currentUser;
            _access = access;
        }

        #region db

        public async Task<string[]> GetDatabaseListAsync(string serverName, string login, string password,
            string additionalField, int port, CancellationToken cancellationToken = default)
        {
            return await _dbConfiguration.LoadDataBasesAsync(serverName, port, login: login, password: password,
                additionalField: additionalField, cancellationToken: cancellationToken);
        }

        public async Task RestoreDatabaseAsync(string serverName, int port, string dataBase, string login,
            string password, DbRestoreType dbType, CancellationToken cancellationToken = default)
        {
            await _dbConfiguration.RestoreDatabaseAsync(serverName, port, dbType, dataBase, login, password,
                cancellationToken);
        }

        public void ConnectToDatabase(string serverName, int port, string dataBase, string login = null,
            string password = null, string additionalField = null)
        {
            _dbConfiguration.ConnectToDatabase(serverName, port, dataBase, login, password, additionalField);
        }

        #endregion

        #region settings

        public ConnectedDatabaseConfiguration GetDatabaseConfiguration()
        {
            var connectionObject = _connectionStringProvider.GetConnectionObject();
            return new ConnectedDatabaseConfiguration(connectionObject.Database, connectionObject.Server);
        }


        public async Task<SystemSettingData> GetConfigurationAsync(bool validate = true,
            CancellationToken cancellationToken = default)
        {
            if (validate)
            {
                if (!await _access.HasAdminRoleAsync(_currentUser.UserId, cancellationToken))
                {
                    throw new AccessDeniedException("No admin rights to view app settings");
                }
            }

            var settings =
                JsonConvert.DeserializeObject<SystemSettingsJson>(
                    _appSettingsEditor.GetValues(new[] { SettingsKey })[0]);

            return _mapper.Map<SystemSettingData>(settings);
        }

        public async Task UpdateConfigurationAsync(SystemSettingData data,
            CancellationToken cancellationToken = default)
        {
            if (!await _access.HasAdminRoleAsync(_currentUser.UserId, cancellationToken))
            {
                throw new AccessDeniedException("No admin rights to update app settings");
            }
    
            var jsonSettings = _mapper.Map<SystemSettingsJson>(data);

            var settings =
                JsonConvert.SerializeObject(jsonSettings);
        
            var parsedObj = JsonConvert.DeserializeObject<JObject>(settings);
            
            _appSettingsEditor.Edit(SettingsKey, parsedObj);
        }

        #endregion
    }
}