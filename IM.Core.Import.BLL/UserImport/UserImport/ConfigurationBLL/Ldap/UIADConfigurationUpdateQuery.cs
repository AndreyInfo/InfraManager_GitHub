using AutoMapper;
using IM.Core.DM.BLL.Interfaces;
using IM.Core.Import.BLL.Interface.Import;
using IM.Core.Import.BLL.Interface.Ldap.Import;
using InfraManager;
using InfraManager.DAL;
using Inframanager.DAL.ActiveDirectory.Import;
using InfraManager.ServiceBase.ImportService.LdapModels;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace IM.Core.Import.BLL.Ldap.Import;

public class UIADConfigurationUpdateQuery:IUpdateQuery<UIADConfigurationsDetails, UIADConfiguration>
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IFilterEntity<UIADIMFieldConcordance, UIADIMFieldConcordancesFilter> _concordanceQuery;
    private readonly IUpdateQuery<UIADIMFieldConcordancesDetails, UIADIMFieldConcordance> _concordanceUpdateQuery;
    private readonly IInsertQuery<UIADIMFieldConcordancesDetails, UIADIMFieldConcordance> _concordanceInsertQuery;
    private readonly IFilterEntity<UIADClass, UIADClassFilter> _classFilter;
    private readonly IRemoveQuery<UIADIMFieldConcordancesKey, UIADIMFieldConcordance> _removeQuery;
    private readonly IUIADIMFieldConcordancesBLL _concordancesBLL;



    public UIADConfigurationUpdateQuery(IMapper mapper,
        IUnitOfWork unitOfWork,
        IUpdateQuery<UIADIMFieldConcordancesDetails, UIADIMFieldConcordance> concordanceUpdateQuery,
        IInsertQuery<UIADIMFieldConcordancesDetails, UIADIMFieldConcordance> concordanceInsertQuery,
        IFilterEntity<UIADIMFieldConcordance, UIADIMFieldConcordancesFilter> concordanceQuery,
        IFilterEntity<UIADClass, UIADClassFilter> classFilter,
        IRemoveQuery<UIADIMFieldConcordancesKey, UIADIMFieldConcordance> removeQuery,
        IUIADIMFieldConcordancesBLL concordancesBLL)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _concordanceUpdateQuery = concordanceUpdateQuery;
        _concordanceInsertQuery = concordanceInsertQuery;
        _concordanceQuery = concordanceQuery;
        _classFilter = classFilter;
        _removeQuery = removeQuery;
        _concordancesBLL = concordancesBLL;
    }

    public async Task<UIADConfiguration> UpdateAsync(UIADConfiguration entity, UIADConfigurationsDetails model, CancellationToken token)
    {
        
        
        
        await UpdateConfigurationsAsync(entity, model, token);

        _mapper.Map(model, entity);

        await _unitOfWork.SaveAsync(token);

        return entity;
    }

    private async Task UpdateConfigurationsAsync(UIADConfiguration entity, UIADConfigurationsDetails model,
        CancellationToken token)
    {
        var fieldConfigurations = JsonConvert.DeserializeObject<FieldConfiguration[]>(model.Configuration);

        fieldConfigurations ??= Array.Empty<FieldConfiguration>();

        var filter = new UIADIMFieldConcordancesFilter
        {
            ConfigurationID = entity.ID
        };

        var entityConfiguration = await _concordanceQuery.Query(filter).ToArrayAsync(token);

        var sourceConfiguration = entityConfiguration.Select(x =>
        (
            x.ClassID,
            ((ConcordanceObjectType) x.IMFieldID).ToString(),
            x.Expression,
            x
        )).ToLookup(x => x.Item2);

        var sourceKeys = sourceConfiguration
            .SelectMany(x =>
                x.Select(y => (x.Key, y.ClassID)))
            .ToHashSet();
        var foundKeys = new HashSet<(string, Guid)>();

        foreach (var configuration in fieldConfigurations.Where(x=>x.FieldEnum.HasValue))
        {
            var concordanceIsEmpty = string.IsNullOrWhiteSpace(configuration.Value);

            foundKeys.Add((configuration.FieldName, configuration.ClassID));

            var hasConfiguration = sourceKeys.Contains((configuration.FieldName, configuration.ClassID));
            
            switch (hasConfiguration)
            {
                case true when concordanceIsEmpty:
                {
                    var removeEntityKey = GetConcordanceKey(entity, configuration, configuration.FieldEnum!.Value);
                    await _concordancesBLL.DeleteAsync(removeEntityKey, token);
                    break;
                }
                case true when !concordanceIsEmpty:
                {
                    var source = sourceConfiguration[configuration.FieldName]
                        .SingleOrDefault(x => x.ClassID == configuration.ClassID);

                    var entityKey = GetConcordanceKey(source.x);

                    var updateConfig = GetConcordanceDetails(configuration);

                    await _concordancesBLL.UpdateAsync(entityKey, updateConfig, token);
                    break;
                }
                case false when !concordanceIsEmpty:
                { 
                    var config = GetInsertDetails(entity, configuration, configuration.FieldEnum!.Value);

                    await _concordancesBLL.AddAsync(config, token);

                    break;
                }
            }
        }

        foreach (var missingKey in sourceKeys.Except(foundKeys))
        {
            if (GetConcordanceKey(entity, missingKey, out var keyToRemove)) continue;

            await _concordancesBLL.DeleteAsync(keyToRemove, token);
        }
    }

    private static UIADIMFieldConcordancesKey GetConcordanceKey(UIADConfiguration entity, FieldConfiguration configuration,
        ConcordanceObjectType concordanceEnum)
    {
        var removeEntityKey = new UIADIMFieldConcordancesKey()
        {
            ADClassID = configuration.ClassID,
            ADConfigurationID = entity.ID,
            IMFieldID = (long) concordanceEnum
        };
        return removeEntityKey;
    }

    private static UIADIMFieldConcordancesDetails GetConcordanceDetails(FieldConfiguration configuration)
    {
        var updateConfig = new UIADIMFieldConcordancesDetails()
        {
            Expression = configuration.Value
        };
        return updateConfig;
    }

    private static UIADIMFieldConcordancesKey GetConcordanceKey(UIADIMFieldConcordance source)
    {
        var entityKey = new UIADIMFieldConcordancesKey
        {
            ADClassID = source.ClassID,
            ADConfigurationID = source.ConfigurationID,
            IMFieldID = source.IMFieldID
        };
        return entityKey;
    }

    private static UIADIMFieldConcordanceInsertDetails GetInsertDetails(UIADConfiguration entity,
        FieldConfiguration configuration, ConcordanceObjectType concordanceEnum)
    {
        var config = new UIADIMFieldConcordanceInsertDetails(adClassID: configuration.ClassID,
            adConfigurationID: entity.ID, imFieldID: (long) concordanceEnum, expression: configuration.Value);
        return config;
    }

    private static bool GetConcordanceKey(UIADConfiguration entity, (string, Guid) key,
        out UIADIMFieldConcordancesKey? keyToRemove)
    {
        if (!GetFieldEnum(key.Item1, out var concordanceEnum))
        {
            keyToRemove = null;
            return true;
        }

        keyToRemove = new UIADIMFieldConcordancesKey()
        {
            ADClassID = key.Item2,
            ADConfigurationID = entity.ID,
            IMFieldID = (long) concordanceEnum
        };
        return false;
    }

    private static bool GetFieldEnum(string fieldName, out  ConcordanceObjectType concordanceEnum)
    {
        return Enum.TryParse(fieldName, out concordanceEnum);
    }
    
}