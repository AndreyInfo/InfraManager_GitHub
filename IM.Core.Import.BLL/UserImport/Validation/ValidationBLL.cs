using System.Reflection;
using AutoMapper;
using IM.Core.Import.BLL.Import;
using IM.Core.Import.BLL.Import.Helpers;
using IM.Core.Import.BLL.Interface.Import;
using IM.Core.Import.BLL.Interface.Import.Log;
using IM.Core.Import.BLL.Interface.Import.Models;
using IM.Core.Import.BLL.Interface.Import.View;
using InfraManager;
using InfraManager.DAL;
using Inframanager.DAL.ActiveDirectory.Import;
using InfraManager.DAL.Database.Import;
using InfraManager.DAL.Import;
using InfraManager.DAL.Import.CSV;
using InfraManager.DAL.Location;
using InfraManager.ResourcesArea;
using InfraManager.ServiceBase.ImportService.LdapModels;

namespace IM.Core.Import.BLL;

public class ValidationBLL : IValidationBLL,ISelfRegisteredService<IValidationBLL>
{ 
    private readonly IRepository<UISetting> _settings;
    private readonly IRepository<UIADSetting> _uiadSettings;
    private readonly IRepository<UIADConfiguration> _uiadConfigurations;
    private readonly IRepository<UIADIMFieldConcordance> _fieldConcordances;
    private readonly IRepository<UICSVSetting> _uicsvSettings;
    private readonly IRepository<UIDBSettings> _uidbSettings;
    private readonly IRepository<UICSVConfiguration> _uicsvConfigurations;
    private readonly IRepository<UICSVIMFieldConcordance> _uicsvimFieldConcordances;
    private readonly IRepository<UIDBFields> _dbFields;
    private readonly IRepository<UIDBConfiguration> _uidbConfiguration;
    private readonly IPolledObjectConverter _polledObjectConverter;
    private readonly ILocalLogger<ValidationBLL> _logger;
    private readonly IMapper _mapper;

    private readonly HashSet<string> _organisationFields = new()
    {
        nameof(PolledObjectDetails.OrganizationName),
        nameof(PolledObjectDetails.OrganizationNote),
        nameof(PolledObjectDetails.OrganizationExternalID)

    };

    private readonly HashSet<string> _subdivisionFields = new()
    {
        nameof(PolledObjectDetails.SubdivisionName),
        nameof(PolledObjectDetails.SubdivisionNote),
        nameof(PolledObjectDetails.SubdivisionExternalID),
    };

    private readonly HashSet<string> _userFields = new()
    {
        nameof(PolledObjectDetails.UserPatronymic),
        nameof(PolledObjectDetails.UserEmail),
        nameof(PolledObjectDetails.UserFax),
        nameof(PolledObjectDetails.UserLogin),
        nameof(PolledObjectDetails.UserManager),
        nameof(PolledObjectDetails.UserNote),
        nameof(PolledObjectDetails.UserNumber),
        nameof(PolledObjectDetails.UserPager),
        nameof(PolledObjectDetails.UserPhone),
        nameof(PolledObjectDetails.UserPosition),
        nameof(PolledObjectDetails.UserFirstName),
        nameof(PolledObjectDetails.UserLastName),
        nameof(PolledObjectDetails.UserPhoneInternal),
        nameof(PolledObjectDetails.UserExternalID),
        nameof(PolledObjectDetails.UserSID),
        nameof(PolledObjectDetails.UserWorkplace)
    };

    private readonly Dictionary<ConcordanceObjectType, string> _fieldConcordnaceOrganozations = new()
    {
        {ConcordanceObjectType.Organization, nameof(PolledObjectDetails.Organization)},
        {ConcordanceObjectType.OrganizationName, nameof(PolledObjectDetails.OrganizationName)},
        {ConcordanceObjectType.OrganizationNote, nameof(PolledObjectDetails.OrganizationNote)},
        {ConcordanceObjectType.OrganizationExternalID, nameof(PolledObjectDetails.OrganizationExternalID)}
    };

    private readonly Dictionary<ConcordanceObjectType, string> _fieldNameConcordanceSubdivisions = new()
    {
        {ConcordanceObjectType.Subdivision, nameof(PolledObjectDetails.Subdivision)},
        {ConcordanceObjectType.SubdivisionName, nameof(PolledObjectDetails.SubdivisionName)},
        {ConcordanceObjectType.SubdivisionNote, nameof(PolledObjectDetails.SubdivisionNote)},
        {ConcordanceObjectType.SubdivisionExternalID, nameof(PolledObjectDetails.SubdivisionExternalID)},
    };
    
    private readonly Dictionary<ConcordanceObjectType, string> _fieldNameConcordanceUsers = new()
    {
        {ConcordanceObjectType.User, nameof(PolledObjectDetails.User)},
        {ConcordanceObjectType.UserLogin, nameof(PolledObjectDetails.UserLogin)},
        {ConcordanceObjectType.UserFirstName, nameof(PolledObjectDetails.UserFirstName)},
        {ConcordanceObjectType.UserLastName, nameof(PolledObjectDetails.UserLastName)},
        {ConcordanceObjectType.UserPatronymic, nameof(PolledObjectDetails.UserPatronymic)},
        {ConcordanceObjectType.UserExternalID, nameof(PolledObjectDetails.UserExternalID)},
        {ConcordanceObjectType.UserEmail, nameof(PolledObjectDetails.UserEmail)},
        {ConcordanceObjectType.UserFax, nameof(PolledObjectDetails.UserFax)},
        {ConcordanceObjectType.UserManager,nameof(PolledObjectDetails.UserManager)},
        {ConcordanceObjectType.UserNote,nameof(PolledObjectDetails.UserNote)},
        {ConcordanceObjectType.UserNumber, nameof(PolledObjectDetails.UserNumber)},
        {ConcordanceObjectType.UserPager, nameof(PolledObjectDetails.UserPager)},
        {ConcordanceObjectType.UserPosition,nameof(PolledObjectDetails.UserPosition)},
        {ConcordanceObjectType.UserPhone, nameof(PolledObjectDetails.UserPhone)},
        {ConcordanceObjectType.UserPhoneInternal, nameof(PolledObjectDetails.UserPhoneInternal)},
        {ConcordanceObjectType.UserSID, nameof(PolledObjectDetails.UserSID)},
        {ConcordanceObjectType.UserWorkplace, nameof(PolledObjectDetails.UserWorkplace)}
    };

    public ValidationBLL(IRepository<UISetting> settings,
        IRepository<UIADConfiguration> uiadConfigurations,
        IRepository<UIADIMFieldConcordance> fieldConcordances,
        IRepository<UICSVConfiguration> uicsvConfigurations,
        IRepository<UICSVIMFieldConcordance> uicsvimFieldConcordances,
        IPolledObjectConverter polledObjectConverter,
        IRepository<UIADSetting> uiadSettings,
        IRepository<UICSVSetting> uicsvSettings,
        ILocalLogger<ValidationBLL> logger,
        IMapper mapper,
        IRepository<UIDBSettings> uidbSettings,
        IRepository<UIDBConfiguration> uidbConfiguration,
        IRepository<UIDBFields> dbFields)
    {
        _settings = settings;
        _uiadConfigurations = uiadConfigurations;
        _fieldConcordances = fieldConcordances;
        _uicsvConfigurations = uicsvConfigurations;
        _uicsvimFieldConcordances = uicsvimFieldConcordances;
        _polledObjectConverter = polledObjectConverter;
        _uiadSettings = uiadSettings;
        _uicsvSettings = uicsvSettings;
        _logger = logger;
        _mapper = mapper;
        _uidbSettings = uidbSettings;
        _uidbConfiguration = uidbConfiguration;
        _dbFields = dbFields;
    }

    public async Task<FieldProtocol> ValidateAsync(Guid settingsID, Guid? configurationID, CancellationToken token)
    {
        try
        {
            var commonProtocol = new FieldProtocol();
            var settings = await _settings.FirstOrDefaultAsync(x=>x.ID==settingsID,token);

            if (settings == null)
            {
                var message = Resources.ImportConfigNotFound;
                _logger.Information(message);
                commonProtocol.CommonError.Add(message);
                return commonProtocol;
            }
            
            var polledObject = _polledObjectConverter.GetPolledObjects(settings.ObjectType);
            
            
            HashSet<ConcordanceObjectType> presentScriptFields;
            
            if (settings.ProviderType == 0)
            {
                var adSettings = await _uiadSettings.FirstOrDefaultAsync(x=>x.ID == settings.ID, token);
                if (adSettings == null)
                {
                    var message = Resources.ImportProviderLDAPNotFound;
                    _logger.Information(message);
                    commonProtocol.CommonError.Add(message);
                    return commonProtocol;
                }


                var searchConfigurationID = configurationID ?? adSettings.ADConfigurationID;
               
                var configuration = await _uiadConfigurations.FirstOrDefaultAsync(x => x.ID == searchConfigurationID, token);
                
                if (configuration == null || string.IsNullOrWhiteSpace(configuration.Name))
                {
                    var message = Resources.ImportConfigurationLDAPNotSupported;
                    _logger.Information(message);
                    commonProtocol.CommonError.Add(message);
                    return commonProtocol;
                }

                var fields =
                    await _fieldConcordances.ToArrayAsync(x => x.ConfigurationID == adSettings.ADConfigurationID,
                        token);
                
                presentScriptFields = (from field in fields
                        where !string.IsNullOrWhiteSpace(field.Expression)
                        select (ConcordanceObjectType)field.IMFieldID)
                    .ToHashSet(); 
                
            }
            else if (settings.ProviderType == 1)
            {
                var csvSettings = await _uicsvSettings.FirstOrDefaultAsync(x => x.ID == settingsID, token);

                if (csvSettings is not {CSVConfigurationID: { }})
                {
                    var message = Resources.ImportProviderCSVNotFound;
                    _logger.Information(message);
                    commonProtocol.CommonError.Add(message);
                    return commonProtocol;
                }
                var searchConfigurationID = configurationID ?? csvSettings.CSVConfigurationID;

                var configuration =
                    await _uicsvConfigurations.FirstOrDefaultAsync(x => x.ID == searchConfigurationID, token);

                if (configuration == null || string.IsNullOrWhiteSpace(configuration.Delimiter))
                {
                    var message = Resources.ImportConfigurationCSVNotSupported;
                    _logger.Information(message);
                    commonProtocol.CommonError.Add(message);
                    return commonProtocol;
                }
                
                var concordancesObj = await _uicsvimFieldConcordances.ToArrayAsync(x=>x.CSVConfigurationID == csvSettings.CSVConfigurationID, token);
                
                presentScriptFields =   (from field in concordancesObj
                                    where !string.IsNullOrWhiteSpace(field.Expression)
                                    select (ConcordanceObjectType)field.IMFieldID)
                                    .ToHashSet();
            }
            else if (settings.ProviderType == 2)
            {
                var dbSettings = await _uidbSettings.FirstOrDefaultAsync(x => x.ID == settingsID, token);

                if (dbSettings is not {DBConfigurationID:{}})
                {
                    var message = Resources.ImportProviderDBNotFound;
                    _logger.Information(message);
                    commonProtocol.CommonError.Add(message);
                    return commonProtocol;
                }

                var searchConfigurationID = configurationID ?? dbSettings.DBConfigurationID;
                var configuration =
                    await _uidbConfiguration.FirstOrDefaultAsync(x => x.ID == searchConfigurationID, token);

                
                
                if (configuration == null || (string.IsNullOrWhiteSpace(configuration.OrganizationTableName) && string.IsNullOrWhiteSpace(configuration.SubdivisionTableName)
                    && string.IsNullOrWhiteSpace(configuration.UserTableName)))
                {
                    var message = Resources.ImportConfigurationDBNotSupported;
                    _logger.Information(message);
                    commonProtocol.CommonError.Add(message);
                    return commonProtocol;
                }
                
                

                var fields = await _dbFields.ToArrayAsync(x => x.ConfigurationID == dbSettings.DBConfigurationID);
                
                presentScriptFields =   (from field in fields
                        where !string.IsNullOrWhiteSpace(field.Value)
                        select (ConcordanceObjectType)field.FieldID)
                    .ToHashSet();
            }
            else
            {
                var message =Resources.ImportUnknownProvider;
                _logger.Information(message);
                commonProtocol.CommonError.Add(message);
                return commonProtocol;
            }

            var errors = new List<FieldError>();
            if (!CheckIsOrganizationReady(settings, polledObject, presentScriptFields, errors))
            {
                var message = Resources.ImportOrganizationConfigurationHasError;
                _logger.Information(message);
                commonProtocol.CommonError.Add(message);
            }

            if (!CheckIsSubdivisionReady(settings, polledObject, presentScriptFields, errors))
            {
                string message = Resources.ImportSubdivisionConfigurationHasError;
                _logger.Information(message);
                commonProtocol.CommonError.Add(message);
            }

            if (!CheckIsUserReady(settings, polledObject, presentScriptFields, errors))
            {
                var message = Resources.ImportUserConfigurationHasError;
                _logger.Information(message);
                commonProtocol.CommonError.Add(message);
            }

            var fieldErrors = errors.GroupBy(x => x.Name).ToArray();
            var mappedErrors = _mapper.Map<FieldErrors[]>(fieldErrors);
            commonProtocol.FieldData = mappedErrors;
            commonProtocol.Result = !(commonProtocol.FieldData.Any() || commonProtocol.CommonError.Any());
            return commonProtocol;
        }
        catch (Exception e)
        {
            _logger.Information($"Исключение в процессе валидации {e.Message}");
            throw;
        }
    }

    private bool CheckIsUserReady(
        UISetting setting, 
        PolledObjectDetails polledObject, 
        HashSet<ConcordanceObjectType> presentScriptFields,
        List<FieldError> data)
    {
        
        HashSet<string> selectedKeysUsers = new();

        if (!polledObject.User)
            return true;

        switch ((UserComparisonEnum)setting.UserComparison)
        {
            case UserComparisonEnum.ByFullName:
                selectedKeysUsers.Add(nameof(PolledObjectDetails.UserFirstName));
                selectedKeysUsers.Add(nameof(PolledObjectDetails.UserLastName));
                selectedKeysUsers.Add(nameof(PolledObjectDetails.UserPatronymic));
                break;
            case UserComparisonEnum.ByFirstNameLastName:
                selectedKeysUsers.Add(nameof(PolledObjectDetails.UserFirstName));
                selectedKeysUsers.Add(nameof(PolledObjectDetails.UserLastName));
                break;
            case UserComparisonEnum.ByNumber:
                selectedKeysUsers.Add(nameof(PolledObjectDetails.UserNumber));
                break;
            case UserComparisonEnum.ByLogin:
                selectedKeysUsers.Add(nameof(PolledObjectDetails.UserLogin));
                break;
            case UserComparisonEnum.ByExternalID:
                selectedKeysUsers.Add(nameof(PolledObjectDetails.UserExternalID));
                break;
            case UserComparisonEnum.BySID:
                selectedKeysUsers.Add(nameof(PolledObjectDetails.UserSID));
                break;
        }

       

        var allValues = ToDictionary(polledObject);
        var selectedFieldsUsers = (from keyValue in allValues
                where keyValue.Value.ToLower() == "true"
                select keyValue.Key)
            .ToHashSet();
        
        if (setting.UpdateLocation && (LocationTypeEnum) setting.LocationMode == LocationTypeEnum.User)
            selectedFieldsUsers.Add(nameof(PolledObjectDetails.UserWorkplace));

        var scripts = new HashSet<string>();
        foreach (var presentScriptField in presentScriptFields)
        {
            if (_fieldNameConcordanceUsers.ContainsKey(presentScriptField))
                scripts.Add(_fieldNameConcordanceUsers[presentScriptField]);
        }

        scripts.Add(_fieldNameConcordanceUsers[ConcordanceObjectType.UserSID]);
        
        return 
               CheckObject(selectedKeysUsers,
                   selectedFieldsUsers,
                   scripts,
                   _userFields,
                   data);
    }

    private bool CheckIsSubdivisionReady(UISetting settings, PolledObjectDetails polledObject, HashSet<ConcordanceObjectType> presentScriptFields,
        List<FieldError> data)
    {
        var selectedFieldsSubdivisions = (from keyValue in ToDictionary(polledObject)
                where keyValue.Value.ToLower() == "true"
                select keyValue.Key)
            .ToHashSet();
        
        if (!selectedFieldsSubdivisions.Contains(nameof(polledObject.Subdivision)))
            return true;
        
        HashSet<string> selectedKeysSubdivision = new();
        
        switch ((SubdivisionComparisonEnum)settings.SubdivisionComparison)
        {
            case SubdivisionComparisonEnum.Name:
                selectedKeysSubdivision.Add(nameof(PolledObjectDetails.SubdivisionName));
                break;
            case SubdivisionComparisonEnum.ExternalID:
                selectedKeysSubdivision.Add(nameof(PolledObjectDetails.SubdivisionExternalID));
                break;
            case SubdivisionComparisonEnum.NameOrExternalID:
                selectedKeysSubdivision.Add(nameof(PolledObjectDetails.SubdivisionName));
                selectedKeysSubdivision.Add((nameof(PolledObjectDetails.SubdivisionExternalID)));
                break;
        }

       
        var scriptFieldNames = new HashSet<string>();
        foreach (var presentScriptField in presentScriptFields)
        {
            if (_fieldNameConcordanceSubdivisions.ContainsKey(presentScriptField))
                scriptFieldNames.Add(_fieldNameConcordanceSubdivisions[presentScriptField]);
        }
        var isSubdivisionReady = CheckObject(selectedKeysSubdivision,
            selectedFieldsSubdivisions,
            scriptFieldNames,
            _subdivisionFields,
            data);
        return isSubdivisionReady;
    }

    private bool CheckIsOrganizationReady(
        UISetting settings, 
        PolledObjectDetails polledObject, 
        HashSet<ConcordanceObjectType> presentScriptFields,
        List<FieldError> data)
    {
        var selectedFieldsOrganizations = (from keyValue in ToDictionary(polledObject)
                where keyValue.Value.ToLower() == "true"
                select keyValue.Key)
            .ToHashSet();

        if (!selectedFieldsOrganizations.Contains(nameof(PolledObjectDetails.Organization)))
            return true;
        
        HashSet<string> selectedKeysOrganization = new();

        switch ((OrganisationComparisonEnum)settings.OrganizationComparison) //TODO make enum
        {
            case OrganisationComparisonEnum.Name:
                selectedKeysOrganization.Add(nameof(PolledObjectDetails.OrganizationName));
                break;
            case OrganisationComparisonEnum.ExternalID:
                selectedKeysOrganization.Add(nameof(PolledObjectDetails.OrganizationExternalID));
                break;
            case OrganisationComparisonEnum.NameOrExternalID:
                selectedKeysOrganization.Add(nameof(PolledObjectDetails.OrganizationName));
                selectedKeysOrganization.Add((nameof(PolledObjectDetails.OrganizationExternalID)));
                break;
        }
        
        var scriptFieldNames = new HashSet<string>();
       
        foreach (var presentScriptField in presentScriptFields)
        {
            if (_fieldConcordnaceOrganozations.ContainsKey(presentScriptField))
                scriptFieldNames.Add(_fieldConcordnaceOrganozations[presentScriptField]);
        }
        
        var isOrganizationReady = CheckObject(selectedKeysOrganization,
                                      selectedFieldsOrganizations,
                                      scriptFieldNames,
                                      _organisationFields,
                                      data);
        return isOrganizationReady;
    }

    private bool CheckObject(HashSet<string> keys,
        HashSet<string> selected,
        HashSet<string> scripts,
        HashSet<string> objectFields,
        List<FieldError> errors)
    {
        var result = true;
        
        //Проверка наличия полей, для которых выбрано сравнение
        var missingKeys = keys.Except(selected); //TODO To array
        if (missingKeys.Any())
        {
            _logger.Information("Не выбраны поля учавствующие в сравнении записей:");
            result = false;
        }

        foreach (var notCheckedField in missingKeys)
        {
            _logger.Information($" {notCheckedField}");
            
            var error = _mapper.Map<FieldError>(notCheckedField);
            error.Error = Resources.ImportNotSelectedButInKey;
            errors.Add(error);
        }

        //проверка заполнения выбранных полей
         var notFilledCheckeds = objectFields.Intersect(selected).Except(scripts); //TODO To array
        if (notFilledCheckeds.Any())
        {
            _logger.Information("Не заполнены выбранные поля:");
            result = false;        }
        
        foreach (var notFilledChecked in notFilledCheckeds)
        {
            _logger.Information($" {notFilledChecked}");
            var error = _mapper.Map<FieldError>(notFilledChecked);
            error.Error = Resources.ImportHasNotFilled;
            errors.Add(error);
        }

        return result;
    }

    private Dictionary<string, string?> ToDictionary(PolledObjectDetails data)
    {
        var propertyType = BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetProperty;
        var elements = data.GetType().GetProperties(propertyType)
            .ToDictionary(x => x.Name, x => x.GetValue(data)?.ToString());

        return elements;
    }
}