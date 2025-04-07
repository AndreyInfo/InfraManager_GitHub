using System.Dynamic;
using System.Reflection;
using AutoMapper;
using IM.Core.Import.BLL.Interface;
using IM.Core.Import.BLL.Interface.Import;
using IM.Core.Import.BLL.Interface.Import.Models;
using IM.Core.Import.BLL.Interface.Import.Models.Settings;
using IM.Core.Import.BLL.Interface.Import.OSU;
using IM.Core.Import.BLL.Interface.Import.View;
using IM.Core.Import.BLL.Interface.Ldap;
using IM.Core.Import.BLL.Interface.Ldap.Import;
using InfraManager;
using InfraManager.BLL;
using InfraManager.BLL.Accounts;
using InfraManager.DAL;
using Inframanager.DAL.ActiveDirectory.Import;
using InfraManager.DAL.Import;
using InfraManager.ServiceBase.ScheduleService;
using IM.Core.Import.BLL.Interface.Import.Log;

namespace IM.Core.Import.BLL.Import.OSU
{
    public class ImportLDAPBLL : ImportDeleteSettings<UIADSetting>, IImportLDAPBLL, ISelfRegisteredService<IImportLDAPBLL>
    {
        private const ConcordanceObjectType UserMask = ConcordanceObjectType.UserOrganization
                                                       | ConcordanceObjectType.UserOrganizationExternalID
                                                       | ConcordanceObjectType.UserSubdivision
                                                       | ConcordanceObjectType.UserSubdivisionExternalID;
                                 

        private readonly ILdapBLL _bll;
        private readonly IRepository<UIADSetting> _settings;
        private readonly IRepository<UIADIMFieldConcordance> _fieldConcordances;
        private readonly IRepository<UIADPath> _path;
        private readonly ILocalLogger<ImportLDAPBLL> _logger;
        private readonly IFilterEntity<UIADClass, UIADClassFilter> _classFilter;
        private readonly IScriptData _scriptData;
        private readonly IMapper _mapper;
        private readonly IImportParameterLogic<IUserDetails, User, UserComparisonEnum> _userParameterLogic;

        private readonly IUnitOfWork _unitOfWork;
        public ImportLDAPBLL(ILdapBLL bll,
            IRepository<UIADSetting> settings,
            IUnitOfWork unitOfWork,
            IRepository<UIADIMFieldConcordance> fieldConcordances, 
            ILocalLogger<ImportLDAPBLL> protocolLogger,
            IRepository<UIADPath> path, 
            IFilterEntity<UIADClass, UIADClassFilter> classFilter, 
            IScriptData scriptData, ILocalLogger<ImportLDAPBLL> logger, IMapper mapper, IImportParameterLogic<IUserDetails, User, UserComparisonEnum> userParameterLogic):base(settings)
        {
            _bll = bll;
            _settings = settings;
            _unitOfWork = unitOfWork;
            _fieldConcordances = fieldConcordances;
            _path = path;
            _classFilter = classFilter;
            _scriptData = scriptData;
            _logger = logger;
            _mapper = mapper;
            _userParameterLogic = userParameterLogic;
            protocolLogger = protocolLogger;
        }
        
        public void AttachProtocolLogger(IProtocolLogger logger)
        {
           
        }

        public async Task<Guid?> GetIDConfiguration(Guid id, CancellationToken cancellationToken)
        {
            var settings = await _settings.FirstOrDefaultAsync(x => x.ID == id, cancellationToken)
                ?? throw new ObjectNotFoundException($"Не найден UIADSetting с ID = {id}");
            
            return settings.ADConfigurationID;
        }

        public async Task<ImportModel?[]> GetImportModelsAsync(ImportTaskRequest importDetails, UISetting settings,
            IProtocolLogger protocolLogger,
            Func<UISetting?, IProtocolLogger, CancellationToken, Task> verify,
            CancellationToken cancellationToken)
        {
            try
            {
                //todo:объединить 
                var type = (ConcordanceObjectType)settings.ObjectType;
                var typeObject = (ObjectType) settings.ObjectType;
                //todo:перенести в importdata
                if (type.HasFlag(ConcordanceObjectType.User))
                    type |= UserMask;
                if (typeObject.HasFlag(ObjectType.UserManager))
                    type |= ConcordanceObjectType.UserManager;
                //todo:сделать сервис
                if (settings.UpdateLocation && ((LocationTypeEnum) settings.LocationMode) == LocationTypeEnum.User)
                    type |= ConcordanceObjectType.UserWorkplace;
                //todo:рефакторинг!!!
                protocolLogger.Information("Подключен источник данных: LDAP");
                protocolLogger.AddInputData(InfraManager.ServiceBase.ImportService.Log.ImportInputType.LDAP);
                //todo:рефакторинг!!!
                await verify(settings, protocolLogger, cancellationToken);
                var adSetting = await _settings.FirstOrDefaultAsync(x => x.ID == settings.ID, cancellationToken);

                var fields =
                    await _fieldConcordances.ToArrayAsync(x => x.ConfigurationID == adSetting.ADConfigurationID,
                        cancellationToken);
                
                var classFilter = new UIADClassFilter();
               
                var adClasses = _classFilter.Query(classFilter).ToDictionary(x => x.ID, x=>x.Name);


                var scriptDataDetailsEnumerable = _scriptData.GetScriptDataDetailsEnumerable(fields, type, x => new ScriptDataDetails<ConcordanceObjectType>()
                {
                    ClassName = adClasses[x.ClassID].ToLower(),
                    Script = x.Expression,
                    FieldEnum = (ConcordanceObjectType) x.IMFieldID,
                });


                var scripts = scriptDataDetailsEnumerable
                    .ToLookup(x=>x.ClassName);
                var requestData = GetRequestData(scripts);


                ImportHelper.PrintConfiguration(scripts, _logger);

                var pathDataOrdered = await GetPathsWithOrderAsync(adSetting, cancellationToken);

                if (!(pathDataOrdered?.Any() ?? false))
                {
                    _logger.Information("Не заданы пути к серверам LDAP");
                    return System.Array.Empty<ImportModel>();
                }

                var importData = new List<ImportModel>();
                var settingsUserComparison = (UserComparisonEnum) settings.UserComparison;
                var getDetailKey = _userParameterLogic.GetDetailsKey(settingsUserComparison);
                Func<ImportModel, string> getManagerIdentificator =
                    x => getDetailKey(_mapper.Map<UserDetails>(x))?.GetKey() ?? String.Empty;
                var count = 0;
                foreach (var currentPath in pathDataOrdered)
                {
                    try
                    {
                        _logger.Information($"[Route:{currentPath}]");

                        var classes = scripts.SelectMany(x=>x.Select(y=>y.ClassName)).Distinct().ToArray();
                       
                        PrintScripts(classes);

                        var password = CryptographyHelper.Decrypt(importDetails.Password);
                        
                        var enumerable = _bll.GetImportModels(
                            currentPath,
                            importDetails.AccountName,
                            password,
                            scripts,
                            getManagerIdentificator,
                            requestData).ToArray();

                        var importModels = enumerable;
                        importData.AddRange(importModels);
                        count += importModels.Length;
                        _logger.Information($"В текущей ветке прочитано {importModels.Length}");
                        _logger.Information("Выход из модуля чтения LDAP");
                    }
                    catch (Exception e)
                    {
                        _logger.Error(e, $"Ошибка импорта LDAP. {currentPath}");
                        throw;
                    }
                   
                }

                _logger.Information($"Прочитано {count} записей");
                
                return importData.ToArray();
            }
            catch (Exception e)
            {
                _logger.Error(e, $"Error when import  LDAP");
                throw;
            }
        }

        private LdapLoadRequestData GetRequestData(ILookup<string, ScriptDataDetails<ConcordanceObjectType>> scripts)
        {
            var requestData = new LdapLoadRequestData();
            foreach (var currentClassScripts in scripts)
            {
                var attributesHash = _scriptData.GetScriptRecordFields(currentClassScripts);
                attributesHash.Add("objectClass");
                attributesHash.Add("objectSid");

                var ldapClassRequestData = new LdapClassRequestData();
                ldapClassRequestData.AddAttributes(attributesHash);
                requestData[currentClassScripts.Key] = ldapClassRequestData;
            }

            return requestData;
        }

        /// <summary>
        /// Установка непередаваемых через интерфейс флагов
        /// </summary>
        /// <returns></returns>
        private static ConcordanceObjectType GetAllInputConcordances()
        {
            var typeNames =
                typeof(PolledObjectDetails).GetProperties(BindingFlags.Public | BindingFlags.Instance |
                                                          BindingFlags.GetProperty);
            var enums = typeNames.Select(x => System.Enum.Parse<ConcordanceObjectType>(x.Name));
            var allEnums = enums.FirstOrDefault();
            foreach (var current in enums.Skip(1))
            {
                allEnums |= current;
            }

            return allEnums;
        }

        private static HashSet<string?> GetAttributes(IEnumerable<ScriptDataDetails<ConcordanceObjectType>> scriptDataDetailsEnumerable)
        {
            var attributesHash = new HashSet<string>();
            attributesHash.Add("objectClass");
            return attributesHash;
        }

        private void PrintScripts(string[] classes)
        {
            _logger.Information(" Опрос классов:");
            foreach (var @class in classes)
            {
                _logger.Information($"  {@class}");
            }
        }

        

        private async Task<HashSet<string>?> GetPathsWithOrderAsync(UIADSetting adSetting,
            CancellationToken cancellationToken)
        {
            IEnumerable<UIADPath> paths = await _path.ToArrayAsync(x => x.ADSettingID == adSetting.ID, cancellationToken);
            if (paths is null)
            {
                return null;
            }

            var pathsLookup = paths.ToLookup(x => x.ADPathID);
            var pathOrder = new HashSet<string>();
            var current = pathsLookup[null].ToArray();
            while (current.Any())
            {
                pathOrder.UnionWith(current.Select(x => x.Path));
                current = current.SelectMany(x => pathsLookup[x.ID]).ToArray();
            }

            return pathOrder;
        }

        public async Task SetUISettingAsync(Guid id, Guid? selectedConfiguration, CancellationToken cancellationToken)
        {
            var config = new UIADSetting()
            {
                ADConfigurationID = selectedConfiguration,
                ID = id
            };
            _settings.Insert(config);
            await _unitOfWork.SaveAsync(cancellationToken);
        }

        public async Task UpdateUISettingAsync(Guid iD, Guid? selectedConfiguration, CancellationToken cancellationToken)
        {
            var config = await _settings.FirstOrDefaultAsync(x => x.ID == iD, cancellationToken);
            
            if (config != null)
            {
                config.ADConfigurationID = selectedConfiguration;
            };
            await _unitOfWork.SaveAsync(cancellationToken);
        }

        public async Task<Guid?> GetConfigurationIDBySettingAsync(Guid settingID, CancellationToken cancellationToken)
        {
            UIADSetting? uiADSettings = null;
            try
            {
                uiADSettings = await _settings.FirstOrDefaultAsync(x => x.ID == settingID, cancellationToken);
            }
            catch (Exception e)
            {
                _logger.Error(e, $"Error when getting UIADSetting with id = {settingID}");
                throw;
            }

            return uiADSettings.ADConfigurationID;
        }

        public async Task<IEnumerable<UIIMFieldConcordance>> GetFieldsByConfigurationIDAsync(Guid? idConfiguration,
            CancellationToken token)
        {
            return await _fieldConcordances.ToArrayAsync(x => x.ConfigurationID == idConfiguration, token);
        }
    }
}
