using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using InfraManager.BLL.Notification.Templates;
using InfraManager.BLL.ServiceDesk;
using InfraManager.BLL.ServiceDesk.WorkOrders;
using InfraManager.BLL.Settings;
using InfraManager.DAL;
using InfraManager.DAL.Documents;
using InfraManager.DAL.ServiceDesk;
using InfraManager.DAL.ServiceDesk.ChangeRequests;
using InfraManager.DAL.ServiceDesk.Negotiations;
using InfraManager.DAL.ServiceDesk.WorkOrders;

namespace InfraManager.BLL.Messages;

internal class ChangeRequestTemplateBuilder :
    EntityTemplateBuilderBase<ChangeRequest, RFCTemplate>,
    IBuildEntityTemplate<ChangeRequest, RFCTemplate>,
    ISelfRegisteredService<IBuildEntityTemplate<ChangeRequest, RFCTemplate>>
{
    private readonly IReadonlyRepository<ChangeRequest> _repository;
    private readonly IReadonlyRepository<CallReference> _callReferences;
    private readonly IReadonlyRepository<WorkOrder> _workorderRepository;
    private readonly IReadonlyRepository<ChangeRequestDependency> _dependencies;
    private readonly IReadonlyRepository<ProblemDependency> _problemDependencies;

    public ChangeRequestTemplateBuilder(IReadonlyRepository<ChangeRequest> repository,
        IMapper mapper,
        ISettingsBLL settingsBll,
        IConvertSettingValue<string> valueConverter,
        IReadonlyRepository<DocumentReference> documentReferences,
        IReadonlyRepository<CallReference> callReferences,
        IReadonlyRepository<WorkOrder> workorderRepository,
        IReadonlyRepository<ChangeRequestDependency> dependencies,
        IReadonlyRepository<ProblemDependency> problemDependencies,
        IReadonlyRepository<Negotiation> negotiations,
        IFindEntityByGlobalIdentifier<User> userFinder,
        IServiceDeskDateTimeConverter serviceDeskTimeConverter)
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
        _callReferences = callReferences;
        _dependencies = dependencies;
        _workorderRepository = workorderRepository;
        _problemDependencies = problemDependencies;
    }

    protected override async Task<ChangeRequest> GetEntityOrRaiseErrorAsync(Guid entityID, CancellationToken cancellationToken)
    {
        return await _repository
                   .With(x => x.Priority)
                   .With(x => x.Type)
                   .With(x => x.Category)
                   .With(x => x.Urgency)
                   .With(x => x.Influence)
                   .With(x => x.Group)
                   .With(x => x.Owner).ThenWith(x => x.Position)
                   .With(x => x.Owner).ThenWith(x => x.Workplace).ThenWith(x => x.Room).ThenWith(x => x.Floor).ThenWith(x => x.Building)
                   .With(x => x.Owner).ThenWith(x => x.Subdivision).ThenWith(x => x.Organization)
                   .With(x => x.Initiator).ThenWith(x => x.Position)
                   .With(x => x.Initiator).ThenWith(x => x.Workplace).ThenWith(x => x.Room).ThenWith(x => x.Floor).ThenWith(x => x.Building)
                   .With(x => x.Initiator).ThenWith(x => x.Subdivision).ThenWith(x => x.Organization)
                   .DisableTrackingForQuery()
                   .SingleOrDefaultAsync(x => x.IMObjID == entityID, cancellationToken)
               ?? throw new ObjectNotFoundException($"Объект уведомления {entityID} (класс {ObjectClass.ChangeRequest}) не найден");
    }

    protected override async Task AfterAutoMapAsync(ChangeRequest entity, RFCTemplate template, CancellationToken cancellationToken)
    {

        template.WebServerAddress = await GetWebServerAddressAsync(cancellationToken);
        
        template.DocumentCountString = (await CountDocumentAsync(entity.IMObjID, cancellationToken)).ToString();
        template.CallCountString = (await _callReferences.CountAsync(x => x.ObjectID == entity.IMObjID && x.ObjectClassID == ObjectClass.ChangeRequest, cancellationToken)).ToString();
        template.WorkOrderCountString = (await _workorderRepository.With(x=>x.WorkOrderReference).CountAsync(x => x.WorkOrderReference.ObjectID == entity.IMObjID && x.WorkOrderReference.ObjectClassID == ObjectClass.ChangeRequest, cancellationToken)).ToString();
        template.DependencyObjectCountString = (await _dependencies.CountAsync(x => x.OwnerObjectID == entity.IMObjID, cancellationToken)).ToString();
        if(entity.ReasonObjectID!=null && entity.ReasonObjectClassID == ObjectClass.Problem)
            template.DependencyKEObjectCountString = (await _problemDependencies.CountAsync(x => x.OwnerObjectID == entity.ReasonObjectID.Value, cancellationToken)).ToString();
        template.NegotiationCountString = (await CountNegotiationsAsync(entity.IMObjID, ObjectClass.ChangeRequest, cancellationToken)).ToString();

        template.DateSolvedString = await ConvertDateTimeAsync(entity.UtcDateSolved, cancellationToken);
        template.DatePromisedString = await ConvertDateTimeAsync(entity.UtcDatePromised, cancellationToken);
        template.DateDetectedString = await ConvertDateTimeAsync(entity.UtcDateDetected, cancellationToken);
        template.DateClosedString = await ConvertDateTimeAsync(entity.UtcDateClosed, cancellationToken);
        template.DateStartedString = await ConvertDateTimeAsync(entity.UtcDateStarted, cancellationToken);
        template.DateModifiedString = await ConvertDateTimeAsync(entity.UtcDateModified, cancellationToken);

        template.InitiatorSubdivisionName = await GetUserSubdivisionFullNameOrDefaultAsync(entity.InitiatorID, cancellationToken);
        template.OwnerSubdivisionName = await GetUserSubdivisionFullNameOrDefaultAsync(entity.OwnerID, cancellationToken);
    }
}