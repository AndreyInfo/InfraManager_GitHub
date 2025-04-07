using InfraManager.DAL.Import;
using InfraManager.DAL.Import.CSV;
using InfraManager.ServiceBase.ImportService.WebAPIModels;
using InfraManager.ServiceBase.WebApiModes;
using InfraManager.ServiceBase.ScheduleService;
using InfraManager.Services;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using IM.Core.Import.BLL.Interface.Import;
using IM.Core.Import.BLL.Interface.Ldap.Import;
using InfraManager.BLL.Import;
using InfraManager.ServiceBase.ImportService.DBService;
using InfraManager.ServiceBase.ImportService.LdapModels;
using InfraManager.ServiceBase.ImportService.WebAPIModels.Import;
using InfraManager.ServiceBase.WorkflowService.WebAPIModels;
using InfraManager.Services.ScheduleService;
using InfraManager.ServiceBase.ImportService.WebAPIModels.ServiceCatalogue;
using InfraManager.ServiceBase.ImportService.WebAPIModels.ITAsset;

namespace InfraManager.WebAPIClient
{
    public class ImportServiceClient : WebAPIBaseClient, IImportApi

    {

        public ImportServiceClient(string baseUrl)
            : base(baseUrl)
        {
        }

        private void UtilityAdd(NameValueCollection utility, string name, string value)
        {
            if (value != null)
                utility.Add(name, value);
        }

        #region IUIDBSettings
        
        private const string SettingsAddress = "api/Database/UIDBSettings";
        
        public async Task<UIDBSettingsOutputDetails[]> GetSettingsDetailsArrayAsync(UIDBSettingsFilter filter, CancellationToken cancellationToken)
        {
            var utility = HttpUtility.ParseQueryString(string.Empty);
            UtilityAdd(utility, nameof(filter.DBConfigurationID), filter.DBConfigurationID?.ToString());
            UtilityAdd(utility,nameof(filter.DatabaseName), filter.DatabaseName);
            return await GetAsync<UIDBSettingsOutputDetails[]>($"{SettingsAddress}?{utility}", null,cancellationToken);
        }

        public async Task<UIDBSettingsOutputDetails> DbSettingsDetailsAsync(Guid id, CancellationToken cancellationToken)
        {
            return await GetAsync<UIDBSettingsOutputDetails>($"{SettingsAddress}/{id}", null, cancellationToken);
        }

        public async Task<UIDBSettingsOutputDetails> AddSettingsAsync(UIDBSettingsData data, CancellationToken cancellationToken = default)
        {
            return await PostAsync<UIDBSettingsOutputDetails,UIDBSettingsData>(SettingsAddress, data, cancellationToken: cancellationToken);
        }

        public async Task<UIDBSettingsOutputDetails> UpdateSettingsAsync(Guid id, UIDBSettingsData data, CancellationToken cancellationToken = default)
        {
            return await PutAsync<UIDBSettingsOutputDetails, UIDBSettingsData>(
                $"{SettingsAddress}/{id}", data, cancellationToken: cancellationToken);
        }

        public async Task SettingsDeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            await DeleteAsync($"{SettingsAddress}/{id}", cancellationToken);
        }


        #endregion

        #region IUIDBFields

        private const string FieldsAddress = "api/Database/UIDBFields";
        
        public async Task<UIDBFieldsOutputDetails[]> GetFieldsDetailsArrayAsync(UIDBFieldsFilter filter, CancellationToken cancellationToken)
        {
            var utility = HttpUtility.ParseQueryString(string.Empty);
            UtilityAdd(utility,nameof(filter.Value), filter.Value);
            UtilityAdd(utility,nameof(filter.ConfigurationID), filter.ConfigurationID?.ToString());
            UtilityAdd(utility,nameof(filter.FieldID), filter.FieldID?.ToString());
            return await GetAsync<UIDBFieldsOutputDetails[]>($"{FieldsAddress}?{utility}", null,cancellationToken);
        }

        public async Task<UIDBFieldsOutputDetails> DbFieldsDetailsAsync(Guid id, CancellationToken cancellationToken)
        {
            return await GetAsync<UIDBFieldsOutputDetails>($"{FieldsAddress}/{id}", null, cancellationToken);
        }

        public async Task<UIDBFieldsOutputDetails> AddFieldsAsync(UIDBFieldsData data, CancellationToken cancellationToken = default)
        {
            return await PostAsync<UIDBFieldsOutputDetails,UIDBFieldsData>(FieldsAddress, data, cancellationToken: cancellationToken);
        }

        public async Task<UIDBFieldsOutputDetails> UpdateFieldsAsync(Guid id, UIDBFieldsData data, CancellationToken cancellationToken = default)
        {
            return await PutAsync<UIDBFieldsOutputDetails, UIDBFieldsData>(
                $"{FieldsAddress}/{id}", data, cancellationToken: cancellationToken);
        }

        public async Task FieldsDeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            await DeleteAsync($"{FieldsAddress}/{id}", cancellationToken);
        }


        #endregion

        #region IUIDBConnectionStringApi

        private const string ConnectionStringAddress = "api/Database/UIDBFieldConfig";
        
        
        
        public async Task<UIDBConnectionStringOutputDetails[]> GetConnectionStringDetailsArrayAsync(UIDBConnectionStringFilter filter, CancellationToken cancellationToken)
        {
            var utility = HttpUtility.ParseQueryString(string.Empty);
            UtilityAdd(utility,nameof(filter.ConnectionString), filter.ConnectionString);
            UtilityAdd(utility,nameof(filter.SettingsID), filter.SettingsID?.ToString());
            return await GetAsync<UIDBConnectionStringOutputDetails[]>($"{ConnectionStringAddress}?{utility}", null,cancellationToken);
        }

        public async Task<UIDBConnectionStringOutputDetails> DbConnectionStringDetailsAsync(Guid id, CancellationToken cancellationToken)
        {
            return await GetAsync<UIDBConnectionStringOutputDetails>($"{ConnectionStringAddress}/{id}", null, cancellationToken);
        }

        public async Task<UIDBConnectionStringOutputDetails> AddConnectionStringAsync(UIDBConnectionStringData data, CancellationToken cancellationToken = default)
        {
            return await PostAsync<UIDBConnectionStringOutputDetails,UIDBConnectionStringData>(ConnectionStringAddress, data, cancellationToken: cancellationToken);
        }

        public async Task<UIDBConnectionStringOutputDetails> UpdateConnectionStringAsync(Guid id, UIDBConnectionStringData data, CancellationToken cancellationToken = default)
        {
            return await PutAsync<UIDBConnectionStringOutputDetails, UIDBConnectionStringData>(
                $"{ConnectionStringAddress}/{id}", data, cancellationToken: cancellationToken);
        }

        public async Task ConnectionStringDeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            await DeleteAsync($"{ConnectionStringAddress}/{id}", cancellationToken);
        }

        
        
        #endregion
        
        #region IUIDBConfigurationApi

        private const string configurationAddress = "api/Database/UIDBConfiguration";
        
        
        
        public async Task<UIDBConfigurationOutputDetails[]> GetDbConfigurationDetailsArrayAsync(UIDBConfigurationFilter filter, CancellationToken cancellationToken)
        {
            var utility = HttpUtility.ParseQueryString(string.Empty);
            
            UtilityAdd(utility,nameof(filter.Name), filter.Name);
            UtilityAdd(utility,nameof(filter.Note), filter.Note);
            return await GetAsync<UIDBConfigurationOutputDetails[]>($"{configurationAddress}?{utility.ToString()}", null,cancellationToken);
        }

        public async Task<UIDBConfigurationOutputDetails> DbConfigurationDetailsAsync(Guid id, CancellationToken cancellationToken)
        {
            return await GetAsync<UIDBConfigurationOutputDetails>($"{configurationAddress}/{id}", null, cancellationToken);
        }

        public async Task<UIDBConfigurationOutputDetails> AddAsync(UIDBConfigurationData data, CancellationToken cancellationToken = default)
        {
            return await PostAsync<UIDBConfigurationOutputDetails,UIDBConfigurationData>(configurationAddress, data, cancellationToken: cancellationToken);
        }

        public async Task<UIDBConfigurationOutputDetails> UpdateAsync(Guid id, UIDBConfigurationData data, CancellationToken cancellationToken = default)
        {
            return await PutAsync<UIDBConfigurationOutputDetails, UIDBConfigurationData>(
                $"{configurationAddress}/{id}", data, cancellationToken: cancellationToken);
        }

        public async Task ConfigurationDeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            await DeleteAsync($"{configurationAddress}/{id}", cancellationToken);
        }

        #endregion

        #region IConfigurationCSVApi

        public async Task<ConfigurationCSVDetails> GetConfigurationAsync(Guid id, CancellationToken cancellationToken)
        {
            
            await GetAsync<ConfigurationCSVDetails>($"ConfigurationCSV/{id}", null, cancellationToken);
            var result = await GetAsync<ConfigurationCSVDetails>($"ConfigurationCSV/{id}", null, cancellationToken);
            return result;
        }

        public async Task SetConfigurationAsync(ConfigurationCSVData configurationCSVDetails, CancellationToken cancellationToken)
        {
            
            var result = await PostAsync<Task, ConfigurationCSVData>($"ConfigurationCSV", configurationCSVDetails, cancellationToken: cancellationToken);
            
        }

        public async Task UpdateConfigurationAsync(Guid id, ConfigurationCSVData configurationCSVDetails, CancellationToken cancellationToken)
        {
            
            await PutAsync<ConfigurationCSVData>($"ConfigurationCSV/{id}", configurationCSVDetails, cancellationToken: cancellationToken);          
        }

        public async Task DeleteConfigurationAsync(Guid id, CancellationToken cancellationToken)
        {
            
            await DeleteAsync($"ConfigurationCSV/{id}", cancellationToken);
        }
        #endregion

        #region IImportCSVApi
        public async Task<CSVConfigurationTableAPI[]> GetConfigurationTableAsync(CancellationToken cancellationToken)
        {
            
            var configurations = await GetAsync<CSVConfigurationTable[]>($"ImportCSV", null, cancellationToken);
            var result = new List<CSVConfigurationTableAPI>();
            foreach (var configuration in configurations)
            {
                var configurationAPI = new CSVConfigurationTableAPI()
                {
                    ID = configuration.ID,
                    Name = configuration.Name,
                    Note = configuration.Note
                };
                result.Add(configurationAPI);
            }
            return result.ToArray();
        }


        public async Task<string> GetPathAsync(Guid id, CancellationToken cancellationToken)
        {
            
            var result = await GetAsync<string>($"ImportCSV/path/{id}", null, cancellationToken);
            return result;
        }
        public async Task UpdatePathAsync(Guid id, string path, CancellationToken cancellationToken)
        {
            
            await PutAsync<string>($"ImportCSV/path/{id}", path, cancellationToken: cancellationToken);
            
        }

        #endregion

        #region IImportService
        public async Task<Guid> CreateMainDetailsAsync(ImportMainTabDetails mainTabDetails, CancellationToken cancellationToken)
        {
            
            return await PostAsync<Guid, ImportMainTabDetails>($"Import/main", mainTabDetails, null, cancellationToken: cancellationToken);
           
        }

        public async Task<DeleteDetails> DeleteTaskAsync(Guid id, CancellationToken cancellationToken)
        {
            return await DeleteAsync<DeleteDetails>($"Import/{id}",null, cancellationToken);
        }

        public async Task<AdditionalTabDetails> GetAdditionalDetailsAsync(Guid id, CancellationToken cancellationToken)
        {
            
            var result = await GetAsync<AdditionalTabDetails>($"Import/additional/{id}", null, cancellationToken);
            return result;
        }

        public async Task<ImportTasksDetails[]> GetImportTasksAsync(CancellationToken cancellationToken)
        {
            
            var taskDetails = await GetAsync<ImportTasksDetails[]>($"Import", null, cancellationToken);
            return taskDetails;
        }

        public async Task<ImportMainTabDetails> GetMainDetailsAsync(Guid id, CancellationToken cancellationToken)
        {
            
            try
            {
                var result = await GetAsync<ImportMainTabDetails>($"Import/main/{id}", null, cancellationToken);
                return result;
            }
            catch(Exception ex)
            {
                return null;
            }
        }


        public async Task UpdateAdditionalDetailsAsync(Guid id, AdditionalTabData settings, CancellationToken cancellationToken)
        {
            
            await PutAsync<AdditionalTabData>($"Import/additional/{id}", settings, cancellationToken: cancellationToken);
        }

        public async Task UpdateMainDetailsAsync(Guid id, ImportMainTabDetails mainTabDetails, CancellationToken cancellationToken)
        {
            
            await PutAsync<ImportMainTabDetails>($"Import/main/{id}", mainTabDetails, cancellationToken: cancellationToken);
            
        }
        #endregion

        #region UIADClass
        public async Task<UIADClassOutputDetails[]> GetDetailsArrayAsync(UIADClassFilter filter, CancellationToken cancellationToken)
        {
            
            var result = await GetAsync<UIADClassOutputDetails[]>($"api/ActiveDirectory/UIADClass?Name={filter.Name}", null, cancellationToken);
            return result;
        }

        public async Task<UIADClassOutputDetails> DetailsAsync(Guid id, CancellationToken cancellationToken)
        {
            
            var result = await GetAsync<UIADClassOutputDetails>($"api/ActiveDirectory/UIADClass/{id}", null, cancellationToken);
            return result;
        }

        public async Task<UIADClassOutputDetails> AddAsync(UIADClassDetails data, CancellationToken cancellationToken = default)
        {
            
            return await PostAsync<UIADClassOutputDetails, UIADClassDetails>($"api/ActiveDirectory/UIADClass", data, null, cancellationToken: cancellationToken);
        }

        public async Task<UIADClassOutputDetails> UpdateAsync(Guid id, UIADClassDetails data, CancellationToken cancellationToken = default)
        {
            
            return await PutAsync<UIADClassOutputDetails, UIADClassDetails>($"api/ActiveDirectory/UIADClass/{id}", data, cancellationToken: cancellationToken);
        }

        public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            
            await DeleteAsync($"api/ActiveDirectory/UIADClass/{id}", cancellationToken);
        }

        #endregion

        #region UIADConfigurations

        public async Task<UIADConfigurationsOutputDetails[]> GetDetailsArrayAsync(UIADConfigurationsFilter filter, CancellationToken cancellationToken)
        {
            
            var result = await GetAsync<UIADConfigurationsOutputDetails[]>($"api/ActiveDirectory/UIADConfiguration?Name={filter.Name}&ShowUsersInADTree={filter.ShowUsersInADTree}&Note={filter.Note}", filter.Name, null, cancellationToken);
            return result;
        }

        public async Task<UIADConfigurationsOutputDetails> DetailsUIADConfigurationsOutputDetailsAsync(Guid id, CancellationToken cancellationToken)
        {
            
            var result = await GetAsync<UIADConfigurationsOutputDetails>($"api/ActiveDirectory/UIADConfiguration/{id}", null, cancellationToken);
            return result;
        }

        public async Task<UIADConfigurationsOutputDetails> AddAsync(UIADConfigurationsDetails data, CancellationToken cancellationToken = default)
        {
            
            return await PostAsync<UIADConfigurationsOutputDetails, UIADConfigurationsDetails>($"api/ActiveDirectory/UIADConfiguration", data, null, cancellationToken: cancellationToken);
        }

        public async Task<UIADConfigurationsOutputDetails> UpdateAsync(Guid id, UIADConfigurationsDetails data, CancellationToken cancellationToken = default)
        {
            
            return await PutAsync<UIADConfigurationsOutputDetails, UIADConfigurationsDetails>($"api/ActiveDirectory/UIADConfiguration/{id}", data, cancellationToken: cancellationToken);
        }
       
        public async Task DeleteUIADConfigurationsDetailsAsync(Guid id, CancellationToken cancellationToken = default)
        {
            
            await DeleteAsync($"api/ActiveDirectory/UIADConfiguration/{id}", cancellationToken);
        }
        #endregion

        #region UIADIMClassConcordances
        public async Task<UIADIMClassConcordancesOutputDetails[]> GetDetailsArrayAsync(UIADIMClassConcordancesFilter filter, CancellationToken cancellationToken)
        {
            
            var result = await GetAsync<UIADIMClassConcordancesOutputDetails[]>($"api/ActiveDirectory/UIADIMClassConcordance", null, cancellationToken);
            return result;
        }

        public async Task<UIADIMClassConcordancesOutputDetails> DetailsAsync(UIADIMClassConcordancesKey id, CancellationToken cancellationToken)
        {
            
            var result = await GetAsync<UIADIMClassConcordancesOutputDetails>($"api/ActiveDirectory/UIADIMClassConcordance/{id.ADConfigurationID}/{id.ADClassID}/{id.IMClassID}", null, cancellationToken);
            return result;
        }

        public async Task<UIADIMClassConcordancesOutputDetails> AddAsync(UIADIMClassConcordancesDetails data, CancellationToken cancellationToken = default)
        {
            
            return await PostAsync<UIADIMClassConcordancesOutputDetails, UIADIMClassConcordancesDetails>($"api/ActiveDirectory/UIADIMClassConcordance", data, null, cancellationToken: cancellationToken);
        }

        public async Task<UIADIMClassConcordancesOutputDetails> UpdateAsync(UIADIMClassConcordancesKey id, UIADIMClassConcordancesDetails data, CancellationToken cancellationToken = default)
        {
            
            return await PutAsync<UIADIMClassConcordancesOutputDetails, UIADIMClassConcordancesDetails>($"api/ActiveDirectory/UIADIMClassConcordance/{id.ADConfigurationID}/{id.ADClassID}/{id.IMClassID}", data, cancellationToken: cancellationToken);
        }

        public async Task DeleteAsync(UIADIMClassConcordancesKey id, CancellationToken cancellationToken = default)
        {
            
            await DeleteAsync($"api/ActiveDirectory/UIADIMClassConcordance/{id}", cancellationToken);
        }

        #endregion

        #region UIADIMFieldConcordances
        public async Task<UIADIMFieldConcordancesOutputDetails[]> GetDetailsArrayAsync(UIADIMFieldConcordancesFilter filter, CancellationToken cancellationToken)
        {
            
            var result = await GetAsync<UIADIMFieldConcordancesOutputDetails[]>($"api/ActiveDirectory/UIADIMFieldConcordance?Expression={filter.Expression}", null, cancellationToken);
            return result;
        }

        public async Task<UIADIMFieldConcordancesOutputDetails> DetailsAsync(UIADIMFieldConcordancesKey id, CancellationToken cancellationToken)
        {
            
            var result = await GetAsync<UIADIMFieldConcordancesOutputDetails>($"api/ActiveDirectory/UIADIMFieldConcordance/{id.ADConfigurationID}/{id.ADClassID}/{id.IMFieldID}", null, cancellationToken);
            return result;
        }

        public async Task<UIADIMFieldConcordancesOutputDetails> AddAsync(UIADIMFieldConcordancesDetails data, CancellationToken cancellationToken = default)
        {
            
            return await PostAsync<UIADIMFieldConcordancesOutputDetails, UIADIMFieldConcordancesDetails>($"api/ActiveDirectory/UIADIMFieldConcordance", data, null, cancellationToken: cancellationToken);
        }

        public async Task<UIADIMFieldConcordancesOutputDetails> UpdateAsync(UIADIMFieldConcordancesKey id, UIADIMFieldConcordancesDetails data, CancellationToken cancellationToken = default)
        {
            
            return await PutAsync<UIADIMFieldConcordancesOutputDetails, UIADIMFieldConcordancesDetails>($"api/ActiveDirectory/UIADIMFieldConcordance/{id.ADConfigurationID}/{id.ADClassID}/{id.IMFieldID}", data, cancellationToken: cancellationToken);
        }

        public async Task DeleteAsync(UIADIMFieldConcordancesKey id, CancellationToken cancellationToken = default)
        {
            
            await DeleteAsync($"api/ActiveDirectory/UIADIMFieldConcordance/{id}", cancellationToken);
        }

        #endregion

        #region UIADPaths
        public async Task<UIADPathsOutputDetails[]> GetDetailsArrayAsync(UIADPathsFilter filter, CancellationToken cancellationToken)
        {
            
            var result = await GetAsync<UIADPathsOutputDetails[]>($"api/ActiveDirectory/UIADPath?ADSettingID={filter.ADSettingID}&Path={filter.Path}", null, cancellationToken);
            return result;
        }

        public async Task<UIADPathsOutputDetails> DetailsUIADPathsOutputDetailsAsync(Guid id, CancellationToken cancellationToken)
        {
            
            var result = await GetAsync<UIADPathsOutputDetails>($"api/ActiveDirectory/UIADPath/{id}", null, cancellationToken);
            return result;
        }

        public async Task<UIADPathsOutputDetails> AddAsync(UIADPathsDetails data, CancellationToken cancellationToken = default)
        {
            
            return await PostAsync<UIADPathsOutputDetails, UIADPathsDetails>($"api/ActiveDirectory/UIADPath", data, null, cancellationToken: cancellationToken);
        }

        public async Task<UIADPathsOutputDetails> UpdateAsync(Guid id, UIADPathsDetails data, CancellationToken cancellationToken = default)
        {
            
            return await PutAsync<UIADPathsOutputDetails, UIADPathsDetails>($"api/ActiveDirectory/UIADPath/{id}", data, cancellationToken: cancellationToken);
        }

        public async Task DeleteUIADPathsOutputDetailsAsync(Guid id, CancellationToken cancellationToken = default)
        {
            
            await DeleteAsync($"api/ActiveDirectory/UIADPath/{id}", cancellationToken);
        }

        #endregion

        #region UIADSettings
        public async Task<UIADSettingsOutputDetails[]> GetDetailsArrayAsync(UIADSettingsFilter filter, CancellationToken cancellationToken)
        {
            
            var result = await GetAsync<UIADSettingsOutputDetails[]>($"api/ActiveDirectory/UIADSetting?ADConfigurationID={filter.ADConfigurationID}", null, cancellationToken);
            return result;
        }

        public async Task<UIADSettingsOutputDetails> DetailsUIADSettingsOutputDetailsAsync(Guid id, CancellationToken cancellationToken)
        {
            
            var result = await GetAsync<UIADSettingsOutputDetails>($"api/ActiveDirectory/UIADSetting/{id}", null, cancellationToken);
            return result;
        }

        public async Task<UIADSettingsOutputDetails> AddAsync(UIADSettingsDetails data, CancellationToken cancellationToken = default)
        {
            
            return await PostAsync<UIADSettingsOutputDetails, UIADSettingsDetails>($"api/ActiveDirectory/UIADSetting", data, null, cancellationToken: cancellationToken);
        }

        public async Task<UIADSettingsOutputDetails> UpdateAsync(Guid id, UIADSettingsDetails data, CancellationToken cancellationToken = default)
        {
            
            return await PutAsync<UIADSettingsOutputDetails, UIADSettingsDetails>($"api/ActiveDirectory/UIADSetting/{id}", data, cancellationToken: cancellationToken);
        }

        public async Task DeleteUIADSettingsOutputDetailsAsync(Guid id, CancellationToken cancellationToken = default)
        {
            
            await DeleteAsync($"api/ActiveDirectory/UIADSetting/{id}", cancellationToken);
        }
        #endregion

        #region Logger
        public async Task<List<TitleLog>> GetAllTitleLogsByTaskIdAsync(Guid id, CancellationToken cancellationToken= default)
        {
            return await GetAsync<List<TitleLog>>($"Logger/titles/{id}", null, cancellationToken);
        }

        public async Task<SchedulerProtocolsDetail[]> GetAllTitleLogsAsync(SchedulerProtocolsDetail[] tasks, CancellationToken cancellationToken = default)
        {
            return await GetAsync<SchedulerProtocolsDetail[], SchedulerProtocolsDetail[]>($"Logger/titles", tasks, null, cancellationToken);
        }
       
        public async Task<LogTask> GetLogByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
           
            return await GetAsync<LogTask>($"Logger/{id}", null, cancellationToken);
        }

        #endregion

        #region EnsureAsync

        public async Task<bool> EnsureAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                await GetAsync<OperationResult>(
                    "import/ensure", cancellationToken: cancellationToken);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        #endregion
        
        #region ChangeConnectionStringAsync

        public async Task ChangeConnectionStringAsync(string server, int port, string dataBase, string login,
            string password, string additionalField, CancellationToken cancellationToken = default)
        {
            var result = await PostAsync<ImportResultWithBool, ConnectionStringChangeRequest>(
                "/Settings/ConnectionString",
                new ConnectionStringChangeRequest
                {
                    Server = server,
                    Login = login,
                    Password = password,
                    Database = dataBase,
                    Port = port,
                    AdditionalField = additionalField
                }, cancellationToken: cancellationToken);

            if (result.OperationResult.Type != OperationResultType.Success)
            {
                throw new Exception("import api error(cant change ConnectionString)");
            }
        }

        #endregion
        
        #region ChangeConnectionStringAsync

        public async Task<FieldProtocol> ValidateSettingsAsync(Guid configurationID, Guid settingsID,
            CancellationToken cancellationToken = default)
        {
            var result = await GetAsync<FieldProtocol, ImportValidateRequest>(
                "/Import/validate", 
                new ImportValidateRequest()
                {
                    ConfigurationID = configurationID,
                    SettingsID = settingsID
                },
                cancellationToken: cancellationToken);

            if (result.OperationResult.Type != OperationResultType.Success)
            {
                throw new Exception("import api error");
            }

            return result;
        }

        public async Task ImportAsync(ImportTaskRequest taskRequest)
        {
            await PostAsync<Task, ImportTaskRequest>($"import/start", taskRequest);
        }
        #endregion

        #region IImportServiceCatalogue

        public async Task<ServiceCatalogueImportSettingDetails[]> GetAllImportTasksServiceCatalogueAsync(CancellationToken cancellationToken)
        {
            return await GetAsync<ServiceCatalogueImportSettingDetails[]>($"ServiceCatalogue", null, cancellationToken);
        }

        public async Task<ServiceCatalogueImportSettingDetails> GetImportTaskByIDServiceCatalogueAsync(Guid id, CancellationToken cancellationToken)
        {
            return await GetAsync<ServiceCatalogueImportSettingDetails>($"ServiceCatalogue/{id}", null, cancellationToken);
        }

        public async Task<Guid> CreateImportTaskServiceCatalogueAsync(ServiceCatalogueImportSettingData data, CancellationToken cancellationToken)
        {
            return await PostAsync<Guid, ServiceCatalogueImportSettingData>($"ServiceCatalogue", data, cancellationToken: cancellationToken);
        }

        public async Task UpdateImportTaskServiceCatalogueAsync(Guid id, ServiceCatalogueImportSettingData data, CancellationToken cancellationToken)
        {
            await PutAsync<ServiceCatalogueImportSettingData>($"ServiceCatalogue/{id}", data, cancellationToken: cancellationToken);
        }

        public async Task DeleteImportTaskServiceCatalogueAsync(Guid id, CancellationToken cancellationToken)
        {
            await DeleteAsync($"ServiceCatalogue/{id}", cancellationToken);
        }

        public async Task StartImportServiceCatalogueAsync(ImportTaskRequest importStartTaskRequest,
            CancellationToken cancellationToken = default)
        {
            await PostAsync<Task, ImportTaskRequest>("ServiceCatalogue/start", importStartTaskRequest);
        }
        #endregion

        #region IServiceCatalogueConfigurationCSVBLL

        public async Task DeleteConfigurationServiceCatalogueAsync(Guid id, CancellationToken cancellationToken)
        {
            await DeleteAsync($"SCConfigurationCSV/{id}", cancellationToken);
        }

        public async Task<ServiceCatalogueImportCSVConfigurationDetails> GetConfigurationServiceCatalogueAsync(Guid id, CancellationToken cancellationToken)
        {
            return await GetAsync<ServiceCatalogueImportCSVConfigurationDetails>($"SCConfigurationCSV/{id}", null, cancellationToken);
        }

        public async Task<ServiceCatalogueImportCSVConfigurationDetails[]> GetConfigurationsServiceCatalogueAsync(CancellationToken cancellationToken)
        {
            return await GetAsync<ServiceCatalogueImportCSVConfigurationDetails[]>($"SCConfigurationCSV", null, cancellationToken);
        }

        public async Task<Guid?> SetConfigurationServiceCatalogueAsync(ServiceCatalogueImportCSVConfigurationData configurationData, CancellationToken cancellationToken)
        {
            return await PostAsync<Guid?, ServiceCatalogueImportCSVConfigurationData>($"SCConfigurationCSV", configurationData, cancellationToken: cancellationToken);
        }

        public async Task UpdateConfigurationServiceCatalogueAsync(Guid id, ServiceCatalogueImportCSVConfigurationData configurationData, CancellationToken cancellationToken)
        {
            await PutAsync<ServiceCatalogueImportCSVConfigurationData>($"SCConfigurationCSV/{id}", configurationData, cancellationToken: cancellationToken);
        }

        #endregion

        #region IITAsset

        public async Task<ITAssetImportSettingDetails[]> GetAllImportTasksITAssetAsync(CancellationToken cancellationToken)
        {
            return await GetAsync<ITAssetImportSettingDetails[]>($"ITAsset", null, cancellationToken);
        }

        public async Task<ITAssetImportSettingDetails> GetImportTaskByIDITAssetAsync(Guid id, CancellationToken cancellationToken)
        {
            return await GetAsync<ITAssetImportSettingDetails>($"ITAsset/{id}", null, cancellationToken);
        }

        public async Task<Guid> CreateImportTaskITAssetAsync(ITAssetImportSettingData data, CancellationToken cancellationToken)
        {
            return await PostAsync<Guid, ITAssetImportSettingData>($"ITAsset", data, cancellationToken: cancellationToken);
        }

        public async Task UpdateImportTaskITAssetAsync(Guid id, ITAssetImportSettingData data, CancellationToken cancellationToken)
        {
            await PutAsync<ITAssetImportSettingData>($"ITAsset/{id}", data, cancellationToken: cancellationToken);
        }

        public async Task DeleteImportTaskITAssetAsync(Guid id, CancellationToken cancellationToken)
        {
            await DeleteAsync($"ITAsset/{id}", cancellationToken);
        }

        public async Task StartImportITAssetAsync(ImportTaskRequest importStartTaskRequest, CancellationToken cancellationToken = default)
        {
            await PostAsync<Task, ImportTaskRequest>("ITAsset/start", importStartTaskRequest,
                cancellationToken: cancellationToken);
        }

        #endregion

        #region IITAssetConfigurationCSVBLL

        public async Task DeleteConfigurationITAssetAsync(Guid id, CancellationToken cancellationToken)
        {
            await DeleteAsync($"ITAssetConfigurationCSV/{id}", cancellationToken);
        }

        public async Task<ITAssetImportCSVConfigurationDetails> GetConfigurationITAssetAsync(Guid id, CancellationToken cancellationToken)
        {
            return await GetAsync<ITAssetImportCSVConfigurationDetails>($"ITAssetConfigurationCSV/{id}", null, cancellationToken);
        }

        public async Task<ITAssetImportCSVConfigurationDetails[]> GetConfigurationsITAssetAsync(CancellationToken cancellationToken)
        {
            return await GetAsync<ITAssetImportCSVConfigurationDetails[]>($"ITAssetConfigurationCSV", null, cancellationToken);
        }

        public async Task<Guid?> SetConfigurationITAssetAsync(ITAssetImportCSVConfigurationData configurationData, CancellationToken cancellationToken)
        {
            return await PostAsync<Guid?, ITAssetImportCSVConfigurationData>($"ITAssetConfigurationCSV", configurationData, cancellationToken: cancellationToken);
        }

        public async Task UpdateConfigurationITAssetAsync(Guid id, ITAssetImportCSVConfigurationData configurationData, CancellationToken cancellationToken)
        {
            await PutAsync<ITAssetImportCSVConfigurationData>($"ITAssetConfigurationCSV/{id}", configurationData, cancellationToken: cancellationToken);
        }

        public async Task<ProductCatalogTypeDetails[]> GetTypesAsync(CancellationToken cancellationToken)
        {
            return await GetAsync<ProductCatalogTypeDetails[]>($"ITAssetConfigurationCSV/types", null, cancellationToken);
        }

        #endregion
    }
}
