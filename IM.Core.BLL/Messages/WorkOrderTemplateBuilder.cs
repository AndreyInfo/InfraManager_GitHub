using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using InfraManager.BLL.Notification.Templates;
using InfraManager.BLL.ServiceDesk;
using InfraManager.BLL.Settings;
using InfraManager.DAL;
using InfraManager.DAL.Documents;
using InfraManager.DAL.ServiceDesk;
using InfraManager.DAL.ServiceDesk.Negotiations;

namespace InfraManager.BLL.Messages;

internal class WorkOrderTemplateBuilder :
    EntityTemplateBuilderBase<WorkOrder, WorkOrderTemplate>,
    IBuildEntityTemplate<WorkOrder, WorkOrderTemplate>,
    ISelfRegisteredService<IBuildEntityTemplate<WorkOrder, WorkOrderTemplate>>
{
    private readonly IReadonlyRepository<WorkOrder> _repository;
    private readonly IReadonlyRepository<WorkorderDependency> _dependencies;

    public WorkOrderTemplateBuilder(
        IReadonlyRepository<WorkOrder> repository,
        IMapper mapper,
        ISettingsBLL settingsBll,
        IConvertSettingValue<string> valueConverter,
        IReadonlyRepository<Negotiation> negotiations,
        IReadonlyRepository<WorkorderDependency> dependencies,
        IFindEntityByGlobalIdentifier<User> userFinder,
        IServiceDeskDateTimeConverter serviceDeskTimeConverter,
        IReadonlyRepository<DocumentReference> documentReferences)
        : base (
            mapper,
            settingsBll,
            valueConverter,
            userFinder,
            serviceDeskTimeConverter,
            documentReferences,
            negotiations)
    {
        _repository = repository;
        _dependencies = dependencies;
    }

    protected override async Task<WorkOrder> GetEntityOrRaiseErrorAsync(Guid entityID, CancellationToken cancellationToken)
    {
        return await _repository
                   .With(x => x.WorkOrderReference)
                   .With(x => x.Type)
                   .With(x => x.Priority)
                   .With(x => x.Initiator).ThenWith(x => x.Position)
                   .With(x => x.Initiator).ThenWith(x => x.Workplace).ThenWith(x => x.Room).ThenWith(x => x.Floor).ThenWith(x => x.Building)
                   .With(x => x.Initiator).ThenWith(x => x.Subdivision).ThenWith(x => x.Organization)
                   .With(x => x.Assignee).ThenWith(x => x.Position)
                   .With(x => x.Assignee).ThenWith(x => x.Workplace).ThenWith(x => x.Room).ThenWith(x => x.Floor).ThenWith(x => x.Building)
                   .With(x => x.Assignee).ThenWith(x => x.Subdivision).ThenWith(x => x.Organization)
                   .With(x => x.Executor).ThenWith(x => x.Position)
                   .With(x => x.Executor).ThenWith(x => x.Workplace).ThenWith(x => x.Room).ThenWith(x => x.Floor).ThenWith(x => x.Building)
                   .With(x => x.Executor).ThenWith(x => x.Subdivision).ThenWith(x => x.Organization)
                   .With(x => x.Group)
                   .With(x => x.Aggregate)
                   .With(x => x.BudgetUsage)
                   .With(x => x.BudgetUsageCause)
                   .DisableTrackingForQuery()
                   .SingleOrDefaultAsync(x => x.IMObjID == entityID, cancellationToken)
               ?? throw new ObjectNotFoundException($"Объект уведомления {entityID} (класс {ObjectClass.WorkOrder}) не найден");
    }

    protected override async Task AfterAutoMapAsync(WorkOrder entity, WorkOrderTemplate template, CancellationToken cancellationToken)
    {
        template.WebServerAddress = await GetWebServerAddressAsync(cancellationToken);

        // todo: Получать из Aggregate когда он будет починен (сейчас не обновляется DocumentCount)
        template.DocumentCountString = (await CountDocumentAsync(entity.IMObjID, cancellationToken)).ToString();
        template.NegotiationCountString = (await CountNegotiationsAsync(entity.IMObjID, ObjectClass.WorkOrder, cancellationToken)).ToString();
        template.DependencyObjectCountString = (await _dependencies.CountAsync(x => x.OwnerObjectID == entity.IMObjID, cancellationToken)).ToString();

        template.DateCreatedString = await ConvertDateTimeAsync(entity.UtcDateCreated, cancellationToken);
        template.DateModifiedString = await ConvertDateTimeAsync(entity.UtcDateModified, cancellationToken);
        template.DateAssignedString = await ConvertDateTimeAsync(entity.UtcDateAssigned, cancellationToken);
        template.DateAcceptedString = await ConvertDateTimeAsync(entity.UtcDateAccepted, cancellationToken);
        template.DatePromisedString = await ConvertDateTimeAsync(entity.UtcDatePromised, cancellationToken);
        template.DateStartedString = await ConvertDateTimeAsync(entity.UtcDateStarted, cancellationToken);
        template.DateAccomplishedString = await ConvertDateTimeAsync(entity.UtcDateAccomplished, cancellationToken);

        template.InitiatorSubdivisionName = await GetUserSubdivisionFullNameOrDefaultAsync(entity.InitiatorID, cancellationToken);
        template.AssignorSubdivisionName = await GetUserSubdivisionFullNameOrDefaultAsync(entity.AssigneeID, cancellationToken);
        template.ExecutorSubdivisionName = await GetUserSubdivisionFullNameOrDefaultAsync(entity.ExecutorID, cancellationToken);
    }
}