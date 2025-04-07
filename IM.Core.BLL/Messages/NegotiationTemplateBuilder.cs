using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using InfraManager.BLL.Notification.Templates;
using InfraManager.BLL.ServiceDesk;
using InfraManager.BLL.Settings;
using InfraManager.DAL;
using InfraManager.DAL.Documents;
using InfraManager.DAL.ServiceDesk;
using InfraManager.DAL.ServiceDesk.ChangeRequests;
using InfraManager.DAL.ServiceDesk.Negotiations;

namespace InfraManager.BLL.Messages;

internal class NegotiationTemplateBuilder : EntityTemplateBuilderBase<Negotiation, NegotiationTemplate>,
    ISelfRegisteredService<IBuildEntityTemplate<Negotiation, NegotiationTemplate>>
{
    private readonly IServiceMapper<ObjectClass, IFindEntityByGlobalIdentifier> _serviceMapper;
    private readonly IReadonlyRepository<WorkOrder> _workOrders;
    private readonly IReadonlyRepository<Call> _calls;
    private readonly IObjectNoteQuery<Call> _callNotes;
    private readonly IReadonlyRepository<ProblemDependency> _problemDependencies;
    private readonly IReadonlyRepository<WorkorderDependency> _woDependencies;
    private readonly IReadonlyRepository<ChangeRequestDependency> _crDependencies;
    private readonly IReadonlyRepository<CallDependency> _callDependencies;


    public NegotiationTemplateBuilder(IMapper mapper,
        IServiceMapper<ObjectClass, IFindEntityByGlobalIdentifier> serviceMapper,
        IReadonlyRepository<DocumentReference> documentReferences,
        IReadonlyRepository<WorkOrder> workOrders, IReadonlyRepository<ProblemDependency> problemDependencies,
        IReadonlyRepository<WorkorderDependency> woDependencies, IReadonlyRepository<ChangeRequestDependency> crDependencies,
        IReadonlyRepository<CallDependency> callDependencies,
        IReadonlyRepository<Call> calls,
        ISettingsBLL settingsBll,
        IConvertSettingValue<string> valueConverter,
        IFindEntityByGlobalIdentifier<User> userFinder,
        IServiceDeskDateTimeConverter serviceDeskTimeConverter,
        IReadonlyRepository<Negotiation> negotiations,
        IObjectNoteQuery<Call> callNotes) : base(
        mapper,
        settingsBll,
        valueConverter,
        userFinder,
        serviceDeskTimeConverter,
        documentReferences,
        negotiations)
    {
        _calls = calls;
        _callNotes = callNotes;
        _serviceMapper = serviceMapper;
        _workOrders = workOrders;
        _problemDependencies = problemDependencies;
        _woDependencies = woDependencies;
        _crDependencies = crDependencies;
        _callDependencies = callDependencies;
    }

    protected override async Task<Negotiation> GetEntityOrRaiseErrorAsync(Guid entityID, CancellationToken cancellationToken)
    {
        return await Negotiations.FirstOrDefaultAsync(n => n.IMObjID == entityID, cancellationToken)
            ?? throw new ObjectNotFoundException($"Объект уведомления {entityID} (класс {ObjectClass.Negotiation}) не найден");;
    }

    protected override async Task AfterAutoMapAsync(Negotiation negotiation, NegotiationTemplate template, CancellationToken cancellationToken)
    {
        var entity = await _serviceMapper.Map(negotiation.ObjectClassID).FindAsync(negotiation.ObjectID, cancellationToken);
        Mapper.Map(entity, template);
        await FillAdditionalParameters(negotiation, entity, template, cancellationToken);
    }
    
    private async Task FillAdditionalParameters(Negotiation negotiation, IGloballyIdentifiedEntity entity,
        NegotiationTemplate template, CancellationToken cancellationToken)
    {
        template.DateVoteStartString = await ConvertDateTimeAsync(negotiation.UtcDateVoteStart, cancellationToken);
        template.DateVoteEndString = await ConvertDateTimeAsync(negotiation.UtcDateVoteEnd, cancellationToken);
        var webServer = await GetWebServerAddressAsync(cancellationToken);
        template.WebServerAddress = webServer;
        template.NegotiationUrl = $"{webServer}/api/negotiations/{negotiation.IMObjID}";
        template.NegotiationCountString = (await CountNegotiationsAsync(entity.IMObjID, cancellationToken)).ToString();

        switch (negotiation.ObjectClassID)
        {
            case ObjectClass.Call:
                await FillAdditionalParameters((Call)entity, template, cancellationToken);
                break;
            case ObjectClass.Problem:
                await FillAdditionalParameters((Problem)entity, template, cancellationToken);
                break;
            case ObjectClass.ChangeRequest:
                await FillAdditionalParameters((ChangeRequest)entity, template, cancellationToken);
                break;
            case ObjectClass.WorkOrder:
                await FillAdditionalParameters((WorkOrder)entity, template, cancellationToken);
                break;
        }
    }
    
    private async Task FillAdditionalParameters(Problem entity, NegotiationTemplate template,
        CancellationToken cancellationToken)
    {
        template.ProblemDocumentCountString = (await CountDocumentAsync(entity.IMObjID, cancellationToken)).ToString(); 
        template.ProblemDependencyObjectCountString = (await _problemDependencies.CountAsync(d => d.OwnerObjectID == entity.IMObjID, cancellationToken)).ToString();
        template.ProblemCallCountString =
            (await _calls.CountAsync(call => entity.CallReferences.Select(r => r.CallID).Contains(call.IMObjID), cancellationToken))
            .ToString();
        template.ProblemWorkOrderCountString = (await _workOrders.CountAsync(wo =>
                entity.WorkOrderReferences.Select(r => r.ID).Contains(wo.WorkOrderReferenceID), cancellationToken))
            .ToString();
    }

    private async Task FillAdditionalParameters(WorkOrder entity, NegotiationTemplate template,
        CancellationToken cancellationToken)
    {
        template.WorkOrderDocumentCountString = (await CountDocumentAsync(entity.IMObjID, cancellationToken)).ToString();
        template.WorkOrderDependencyObjectCountString = (await _woDependencies.CountAsync(d => d.OwnerObjectID == entity.IMObjID, cancellationToken)).ToString();
    }

    private async Task FillAdditionalParameters(ChangeRequest entity, NegotiationTemplate template,
        CancellationToken cancellationToken)
    {
        template.RFCDocumentCountString = (await CountDocumentAsync(entity.IMObjID, cancellationToken)).ToString();
        template.RFCWorkOrderCountString = (await _workOrders.CountAsync(wo =>
                entity.WorkOrderReferences.Select(r => r.ID).Contains(wo.WorkOrderReferenceID), cancellationToken)).ToString();
        template.RFCDependencyObjectCountString = (await _crDependencies.CountAsync(d => d.OwnerObjectID == entity.IMObjID, cancellationToken)).ToString();
    }

    private async Task FillAdditionalParameters(Call entity, NegotiationTemplate template,
        CancellationToken cancellationToken)
    {
        template.CallDocumentCountString = (await CountDocumentAsync(entity.IMObjID, cancellationToken)).ToString();
        template.CallDependencyObjectCountString = (await _callDependencies.CountAsync(d => d.OwnerObjectID == entity.IMObjID, cancellationToken)).ToString();
        template.CallWorkOrderCountString = (await _workOrders.CountAsync(wo =>
            entity.WorkOrderReferences.Select(r => r.ID).Contains(wo.WorkOrderReferenceID), cancellationToken)).ToString();

        var notesItems = await _callNotes.ExecuteAsync(new ObjectNoteQueryCriteria() { ObjectID = entity.IMObjID },
            cancellationToken);
        var notes = notesItems.Select(n => n.NoteEntity).Select(n => n.NoteText).ToArray();
        template.CallMessageString = string.Join("\n", notes);
        template.CallLastMessageString = string.Join("\n", notes.TakeLast(5));
        template.CallFinalMessageString = notes.Last();
    }
}