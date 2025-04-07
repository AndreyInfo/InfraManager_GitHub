using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using InfraManager.BLL.AppSettings;
using InfraManager.BLL.DatabaseConfiguration;
using InfraManager.BLL.Import;
using InfraManager.BLL.Settings;
using InfraManager.BLL.Workflow;
using InfraManager.DAL.DbConfiguration;
using InfraManager.ServiceBase.ScheduleService;
using InfraManager.UI.Web.Models.Settings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InfraManager.UI.Web.Controllers.BFF
{
    [Route("bff/[controller]")]
    [Authorize]
    public class ConfigurationController : ControllerBase
    {
        private readonly IAppSettingsBLL _appSettings;
        private readonly IWorkflowServiceApi _workflowApi;
        private readonly IImportApi _importApi;
        private readonly IScheduleServiceWebApi _scheduleApi;

        public ConfigurationController(IAppSettingsBLL appSettings,
                                       IWorkflowServiceApi workflowApi,
                                       IImportApi importApi,
                                       IScheduleServiceWebApi scheduleApi)
        {
            _appSettings = appSettings;
            _workflowApi = workflowApi;
            _importApi = importApi;
            _scheduleApi = scheduleApi;
        }

        #region GetConfiguration

        [HttpGet("Database")]
        [AllowAnonymous]
        public ConnectedDatabaseConfiguration GetDatabaseConfiguration()
        {
            return _appSettings.GetDatabaseConfiguration();
        }
        
        [HttpGet("Settings")]
        public Task<SystemSettingData> GetConfiguration()
        {
            return _appSettings.GetConfigurationAsync();
        }

        [HttpPut("Settings")]
        public Task UpdateConfiguration([FromBody] SystemSettingData data,
            CancellationToken cancellationToken = default)
        {
            return _appSettings.UpdateConfigurationAsync(data, cancellationToken);
        }

        #endregion

        #region GetDataBaseList
        [HttpGet]
        [Route("GetDataBaseList")]
        [AllowAnonymous]
        public async Task<string[]> GetDataBaseListAsync([FromQuery] DataBaseListRequest request,
            CancellationToken cancellationToken = default)
        {
            return await _appSettings.GetDatabaseListAsync(request.ServerName, request.Login, request.Password,
                request.AdditionalField, request.Port, cancellationToken);
        }
        #endregion
        
        #region RestoreDB
        [HttpPost]
        [Route("RestoreDB")]
        [AllowAnonymous]
        public async Task RestoreDB([FromBody] RestoreDBData details, CancellationToken cancellationToken = default)
        {
            await _appSettings.RestoreDatabaseAsync(details.ServerName, details.Port, details.DataBase, details.Login,
                details.Password, details.DbRestoreType, cancellationToken);
        }
        #endregion

        #region ConnectDB
        //TODO ключ=значение;ключ=значение; нужна такая проверка на поле serverInfo.AdditionalField
        [HttpPost]
        [Route("ConnectDB")]
        [AllowAnonymous]
        public async Task ConnectDB([FromBody] DBServerInfoData serverInfo, CancellationToken cancellationToken = default)
        {
            _appSettings.ConnectToDatabase(serverInfo.ServerName, serverInfo.Port, serverInfo.DataBase, serverInfo.Login,
                serverInfo.Password, serverInfo.AdditionalField);
            
            await _workflowApi.ChangeConnectionStringAsync(serverInfo.ServerName, serverInfo.Port, serverInfo.DataBase, serverInfo.Login,
                serverInfo.Password, serverInfo.AdditionalField, cancellationToken);
            
            await _importApi.ChangeConnectionStringAsync(serverInfo.ServerName, serverInfo.Port, serverInfo.DataBase, serverInfo.Login,
                serverInfo.Password, serverInfo.AdditionalField, cancellationToken);

            await _scheduleApi.ChangeConnectionStringAsync(serverInfo.ServerName, serverInfo.Port, serverInfo.DataBase, serverInfo.Login,
                serverInfo.Password, serverInfo.AdditionalField, cancellationToken);
        }
        #endregion
    }
}
