using AutoMapper;
using Inframanager;
using Inframanager.BLL;
using Inframanager.BLL.AccessManagement;
using InfraManager.CrossPlatform.WebApi.Contracts.Common;
using InfraManager.DAL;
using InfraManager.DAL.ServiceCatalogue;
using InfraManager.DAL.ServiceDesk;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using HighlightingCondition = InfraManager.DAL.Highlightings.HighlightingCondition;
using HighlightingConditionValue = InfraManager.DAL.Highlightings.HighlightingConditionValue;
using HighlightingEntity = InfraManager.DAL.Highlightings.Highlighting;

namespace InfraManager.BLL.Highlighting;

internal class HighlightingBLL :
    StandardBLL<Guid, HighlightingEntity, HighlightingData, HighlightingDetails, BaseFilter>,
    IHighlightingBLL,
    ISelfRegisteredService<IHighlightingBLL>
{
    private readonly IMapper _mapper;
    private readonly IRepository<HighlightingCondition> _repositoryCond;
    private readonly IRepository<HighlightingConditionValue> _repositoryCondValue;
    private readonly IValidatePermissions<HighlightingCondition> _highlightingCondPermissions;
    private readonly IRepository<Priority> _repositoryPriority;
    private readonly IRepository<Urgency> _repositoryUrgency;
    private readonly IRepository<Influence> _repositoryInfluence;
    private readonly IRepository<ServiceLevelAgreement> _repositorySLA;

    public HighlightingBLL(IMapper mapper,
        IRepository<HighlightingEntity> repository,
        IRepository<HighlightingCondition> repositoryCond,
        IRepository<HighlightingConditionValue> repositoryCondValue,
        IRepository<Priority> repositoryPriority,
        IRepository<Urgency> repositoryUrgency,
        IRepository<Influence> repositoryInfluence,
        IRepository<ServiceLevelAgreement> repositorySLA,
        IUnitOfWork unitOfWork,
        IValidatePermissions<HighlightingCondition> highlightingCondPermissions,
        ILogger<HighlightingEntity> logger,
        ICurrentUser currentUser,
        IBuildObject<HighlightingDetails, HighlightingEntity> detailsBuilder,
        IInsertEntityBLL<HighlightingEntity, HighlightingData> insertEntityBLL,
        IModifyEntityBLL<Guid, HighlightingEntity, HighlightingData, HighlightingDetails> modifyEntityBLL,
        IRemoveEntityBLL<Guid, HighlightingEntity> removeEntityBLL,
        IGetEntityBLL<Guid, HighlightingEntity, HighlightingDetails> detailsBLL,
        IGetEntityArrayBLL<Guid, HighlightingEntity, HighlightingDetails, BaseFilter> detailsArrayBLL)
        : base
            (repository,
            logger,
            unitOfWork,
            currentUser,
            detailsBuilder,
            insertEntityBLL,
            modifyEntityBLL,
            removeEntityBLL,
            detailsBLL,
            detailsArrayBLL)
    {
        _mapper = mapper;
        _repositoryCond = repositoryCond;
        _repositoryCondValue = repositoryCondValue;
        _highlightingCondPermissions = highlightingCondPermissions;
        _repositoryPriority = repositoryPriority;
        _repositoryUrgency = repositoryUrgency;
        _repositoryInfluence = repositoryInfluence;
        _repositorySLA = repositorySLA;
    }

    public async Task<HighlightingConditionDetails[]> ListValueAsync(Guid HighlightingID,
        CancellationToken cancellationToken = default)
    {
        await _highlightingCondPermissions.ValidateOrRaiseErrorAsync(Logger, CurrentUser.UserId, ObjectAction.ViewDetails,
            cancellationToken);

        var entities = await _repositoryCond.ToArrayAsync(x => x.HighlightingID == HighlightingID)
                ?? throw new ObjectNotFoundException<Guid>(HighlightingID, ObjectClass.HighlightingCondition);
        Logger.LogTrace($"User ID = {CurrentUser.UserId} got Highlighting condition items");

        var result = new List<HighlightingConditionDetails>();

        foreach (var entity in entities)
        {
            var details = _mapper.Map<HighlightingConditionDetails>(entity);

            foreach (var item in entity.HighlightingConditionValue)
            {
                if (entity.EnumParameter.HasValue)
                {
                    if (item.IntValue1.HasValue)
                        details.IntValue1 = item.IntValue1.Value;

                    if (item.IntValue2.HasValue)
                        details.IntValue2 = item.IntValue2.Value;
                }
                else if (entity.DirectoryParameter.HasValue && item.StringValue != null)
                    details.StringValue = item.StringValue;

                else
                {
                    switch (entity.DirectoryParameter)
                    {
                        case ObjectClass.Priority:
                            var priorty = await _repositoryPriority.FirstOrDefaultAsync(x => x.ID == item.PriorityID.Value, cancellationToken);
                            details.GuidValues.Add(new GuidDetail { Id = priorty.ID, Name = priorty.Name });
                            break;

                        case ObjectClass.Urgency:
                            var urgency = await _repositoryUrgency.FirstOrDefaultAsync(x => x.ID == item.UrgencyID.Value, cancellationToken);
                            details.GuidValues.Add(new GuidDetail { Id = urgency.ID, Name = urgency.Name });
                            break;

                        case ObjectClass.Influence:
                            var influence = await _repositoryInfluence.FirstOrDefaultAsync(x => x.ID == item.InfluenceID.Value, cancellationToken);
                            details.GuidValues.Add(new GuidDetail { Id = influence.ID, Name = influence.Name });
                            break;

                        case ObjectClass.SLA:
                            var sla = await _repositorySLA.FirstOrDefaultAsync(x => x.ID == item.SlaID.Value, cancellationToken);
                            details.GuidValues.Add(new GuidDetail { Id = sla.ID, Name = sla.Name });
                            break;

                        default:
                            throw new NotImplementedException($"HighlightingConditionValue not implemented for {entity.DirectoryParameter}");
                    }
                }
            }
            result.Add(details);
        }

        return result.ToArray();
    }

    public async Task AddValueAsync(HighlightingConditionData data,
        CancellationToken cancellationToken = default)
    {
        await _highlightingCondPermissions.ValidateOrRaiseErrorAsync(Logger, CurrentUser.UserId, ObjectAction.Insert,
            cancellationToken);

        var highlightingCondition = _mapper.Map<HighlightingCondition>(data);
        _repositoryCond.Insert(highlightingCondition);

        await SaveOrUpdateHighlightingConditionValueAsync(highlightingCondition.ID, data, cancellationToken);
    }

    public async Task UpdateValueAsync(Guid id, HighlightingConditionData highlightingValue,
        CancellationToken cancellationToken = default)
    {
        await _highlightingCondPermissions.ValidateOrRaiseErrorAsync(Logger, CurrentUser.UserId, ObjectAction.Update,
            cancellationToken);

        var foundHighlightingCondition = await _repositoryCond.FirstOrDefaultAsync(c => c.ID == id, cancellationToken)
                ?? throw new ObjectNotFoundException<Guid>(id, ObjectClass.HighlightingCondition);

        _mapper.Map(highlightingValue, foundHighlightingCondition);

        var foundHighlightingConditionValues = await _repositoryCondValue
            .ToArrayAsync(x => x.HighlightingConditionID == foundHighlightingCondition.ID, cancellationToken);

        foreach (var item in foundHighlightingConditionValues)
            _repositoryCondValue.Delete(item);

        await SaveOrUpdateHighlightingConditionValueAsync(foundHighlightingCondition.ID, highlightingValue, cancellationToken);

        Logger.LogTrace($"User ID = {CurrentUser.UserId} Updated Value Highlighting with ID = {id}");
        await UnitOfWork.SaveAsync(cancellationToken);
    }

    public async Task DeleteValueAsync(Guid id, CancellationToken cancellationToken = default)
    {
        await _highlightingCondPermissions.ValidateOrRaiseErrorAsync(Logger, CurrentUser.UserId, ObjectAction.Update,
            cancellationToken);

        var foundHighlightingCondition = await _repositoryCond.FirstOrDefaultAsync(c => c.ID == id, cancellationToken)
                ?? throw new ObjectNotFoundException<Guid>(id, ObjectClass.HighlightingCondition);

        _repositoryCond.Delete(foundHighlightingCondition);

        await UnitOfWork.SaveAsync(cancellationToken);
    }

    private async Task SaveOrUpdateHighlightingConditionValueAsync(Guid id, HighlightingConditionData data,
        CancellationToken cancellationToken = default)
    {
        if (data.GuidValues == null && data.GuidValues?.Length == 0 && data.StringValue == null && !data.IntValue1.HasValue)
            throw new NotImplementedException($"None of the values are implemented");

        // Если присутствует временной параметр - добавляем его значения
        if (data.EnumParameter.HasValue && data.IntValue1.HasValue)
        {
            HighlightingConditionValue highlightingConditionValue = new HighlightingConditionValue() 
            {
                HighlightingConditionID = id,
                IntValue1 = data.IntValue1.Value
            };

            if (data.IntValue2.HasValue)
                highlightingConditionValue.IntValue2 = data.IntValue2;

            _repositoryCondValue.Insert(highlightingConditionValue);
            Logger.LogTrace($"User ID = {CurrentUser.UserId} Updated HighlightingConditionValue with ID = {highlightingConditionValue.ID}");
        }

        else if (data.DirectoryParameter.HasValue && data.StringValue != null)
        {
            HighlightingConditionValue highlightingConditionValue = new HighlightingConditionValue()
            {
                HighlightingConditionID = id,
                StringValue = data.StringValue
            };

            _repositoryCondValue.Insert(highlightingConditionValue);
            Logger.LogTrace($"User ID = {CurrentUser.UserId} Updated HighlightingConditionValue with ID = {highlightingConditionValue.ID}");
        }

        // Если присутствует список значений - добавляем их
        else if (data.DirectoryParameter.HasValue && data.GuidValues != null)
            foreach (var itemGuid in data.GuidValues)
            {
                HighlightingConditionValue highlightingConditionValue = new HighlightingConditionValue() { HighlightingConditionID = id };

                switch (data.DirectoryParameter)
                {
                    case ObjectClass.Priority:
                        highlightingConditionValue.PriorityID = itemGuid;
                        break;

                    case ObjectClass.Urgency:
                        highlightingConditionValue.UrgencyID = itemGuid;
                        break;

                    case ObjectClass.Influence:
                        highlightingConditionValue.InfluenceID = itemGuid;
                        break;

                    case ObjectClass.SLA:
                        highlightingConditionValue.SlaID = itemGuid;
                        break;

                    default:
                        throw new NotImplementedException($"HighlightingConditionValue not implemented for {data.DirectoryParameter}");
                }

                _repositoryCondValue.Insert(highlightingConditionValue);
                Logger.LogTrace($"User ID = {CurrentUser.UserId} Updated HighlightingConditionValue with ID = {highlightingConditionValue.ID}");
            }
        else
            throw new NotImplementedException($"None of the values are implemented");

        await UnitOfWork.SaveAsync(cancellationToken);
    }
}
