using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using InfraManager.BLL.ServiceDesk;
using InfraManager.BLL.Settings;
using InfraManager.Core;
using InfraManager.DAL;
using InfraManager.DAL.Documents;
using InfraManager.DAL.ServiceDesk.Negotiations;

namespace InfraManager.BLL.Messages;

internal abstract class EntityTemplateBuilderBase<TEntity, TTemplate> : IBuildEntityTemplate<TEntity, TTemplate>
{
    protected readonly IMapper Mapper;
    protected readonly IReadonlyRepository<Negotiation> Negotiations;
    private readonly IFindEntityByGlobalIdentifier<User> _userFinder;
    private readonly ISettingsBLL _settingsBll;
    private readonly IConvertSettingValue<string> _valueConverter;
    private readonly IServiceDeskDateTimeConverter _serviceDeskTimeConverter;
    private readonly IReadonlyRepository<DocumentReference> _documentReferences;

    protected EntityTemplateBuilderBase(
        IMapper mapper,
        ISettingsBLL settingsBll,
        IConvertSettingValue<string> valueConverter,
        IFindEntityByGlobalIdentifier<User> userFinder,
        IServiceDeskDateTimeConverter serviceDeskTimeConverter,
        IReadonlyRepository<DocumentReference> documentReferences,
        IReadonlyRepository<Negotiation> negotiations)
    {
        Mapper = mapper;
        Negotiations = negotiations;
        _settingsBll = settingsBll;
        _valueConverter = valueConverter;
        _userFinder = userFinder;
        _serviceDeskTimeConverter = serviceDeskTimeConverter;
        _documentReferences = documentReferences;
    }

    public async Task<TTemplate> BuildAsync(Guid id, CancellationToken cancellationToken = default, Guid? userID = null)
    {
        var entity = await GetEntityOrRaiseErrorAsync(id, cancellationToken);
        var template = Mapper.Map<TTemplate>(entity);
        await AfterAutoMapAsync(entity, template, cancellationToken);
        return template;
    }

    protected async Task<string> GetUserSubdivisionFullNameOrDefaultAsync(Guid? userID, CancellationToken cancellationToken)
    {
        if (!userID.HasValue)
        {
            return string.Empty;
        }

        var user = await _userFinder
            .With(x => x.Subdivision)
            .ThenWith(x => x.ParentSubdivision)
            .FindAsync(userID.Value, cancellationToken);

        var stack = new Stack<string>();
        var current = user.Subdivision;
        while (current is not null)
        {
            
            stack.Push(current.Name);
            current = current.ParentSubdivision;
        }

        return stack.Any() ? string.Join(" \\ ", stack) : string.Empty;
    }

    protected async Task<string> GetWebServerAddressAsync(CancellationToken cancellationToken)
    {
        return _valueConverter.Convert(await _settingsBll.GetValueAsync(SystemSettings.WebServerAddress, cancellationToken));
    }

    protected async Task<string> ConvertDateTimeAsync(DateTime? dateTime, CancellationToken cancellationToken)
    {
        return dateTime.HasValue
            ? (await _serviceDeskTimeConverter.ConvertAsync(dateTime.Value, cancellationToken)).ToString(Global.DateTimeFormat)
            : string.Empty;
    }

    protected async Task<int> CountDocumentAsync(Guid entityID, CancellationToken cancellationToken)
    {
        return await _documentReferences.CountAsync(x => x.ObjectID == entityID, cancellationToken);
    }

    protected async Task<int> CountNegotiationsAsync(Guid entityID, ObjectClass classID, CancellationToken cancellationToken)
    {
        return await Negotiations.CountAsync(x => x.ObjectID == entityID && x.ObjectClassID == classID, cancellationToken);
    }
    
    protected async Task<int> CountNegotiationsAsync(Guid entityID, CancellationToken cancellationToken)
    {
        return await Negotiations.CountAsync(x => x.ObjectID == entityID, cancellationToken);
    }

    protected abstract Task<TEntity> GetEntityOrRaiseErrorAsync(Guid entityID, CancellationToken cancellationToken);

    protected abstract Task AfterAutoMapAsync(TEntity entity, TTemplate template, CancellationToken cancellationToken);
}