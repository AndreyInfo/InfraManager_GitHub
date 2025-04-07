using System.Text.Json.Nodes;
using AutoMapper;
using IM.Core.DM.BLL.Interfaces;
using IM.Core.Import.BLL.Interface.Import;
using IM.Core.Import.BLL.Interface.Ldap.Import;
using InfraManager;
using Inframanager.DAL.ActiveDirectory.Import;
using InfraManager.ServiceBase.ImportService.LdapModels;
using IronPython.Modules;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace IM.Core.Import.BLL.Ldap.Import;

public class UIADFieldConcoranceBuilder:IBuildModel<UIADConfiguration, UIADConfigurationsOutputDetails>
{
    private readonly IMapper _mapper;
    private IFilterEntity<UIADIMFieldConcordance, UIADIMFieldConcordancesFilter> _filterEntity;
    public UIADFieldConcoranceBuilder(IMapper mapper, IFilterEntity<UIADIMFieldConcordance, UIADIMFieldConcordancesFilter> filterEntity)
    {
        _mapper = mapper;
        _filterEntity = filterEntity;
    }

    public async Task<UIADConfigurationsOutputDetails> BuildAsync(UIADConfiguration entity, CancellationToken token)
    {

        var filter = new UIADIMFieldConcordancesFilter()
        {
            ConfigurationID = entity.ID
        };
        var concordances = await _filterEntity.Query(filter).ToArrayAsync(token);
        var enumData = Enum.GetNames(typeof(ConcordanceObjectType)).Except(new string[]
        {
            nameof(ConcordanceObjectType.All),
            nameof(ConcordanceObjectType.Nothing),
            nameof(ConcordanceObjectType.Organization),
            nameof(ConcordanceObjectType.Subdivision),
            nameof(ConcordanceObjectType.User)
        });
        var configurations = concordances.Select(x =>new FieldConfiguration()
        {
            ClassID = x.ClassID,
            FieldName = ((ConcordanceObjectType) x.IMFieldID).ToString(),
            Value = x.Expression
        }).Where(x=>enumData.Contains(x.FieldName)).ToArray();
        
        var outputResult = _mapper.Map<UIADConfigurationsOutputDetails>(entity);
        
        outputResult.Configuration = JsonConvert.SerializeObject(configurations);

        return outputResult;

    }

    public async Task<UIADConfigurationsOutputDetails[]> BuildArrayAsync(UIADConfiguration[] entities, CancellationToken token)
    {
        var result = new List<UIADConfigurationsOutputDetails>(entities.Length);

        foreach (var current in entities)
        {
            var currentResult = await BuildAsync(current, token);
            result.Add(currentResult);
        }

        return result.ToArray();
    }
}