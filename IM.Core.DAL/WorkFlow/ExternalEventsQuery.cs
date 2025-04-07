using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using InfraManager.DAL.Events;
using InfraManager.DAL.ServiceDesk;
using InfraManager.DAL.ServiceDesk.ChangeRequests;
using InfraManager.DAL.ServiceDesk.MassIncidents;
using InfraManager.DAL.WF;
using InfraManager.DAL.WorkFlow.Events;
using Microsoft.EntityFrameworkCore;

namespace InfraManager.DAL.WorkFlow;

internal class ExternalEventsQuery : IExternalEventsQuery, ISelfRegisteredService<IExternalEventsQuery>
{
    private readonly IRepository<EntityEvent> _entityEventRepository;
    private readonly IRepository<EnvironmentEvent> _envEventRepository;
    private readonly IReadonlyRepository<WorkFlowScheme> _workflowSchemeRepository;
    
    private readonly IRepository<Call> _callRepository;
    private readonly IRepository<WorkOrder> _workOrderRepository;
    private readonly IRepository<Problem> _problemRepository;
    private readonly IRepository<ChangeRequest> _changeRequestRepository;
    private readonly IRepository<Message.Message> _messageRepository;
    private readonly IRepository<WorkflowTracking> _trackingRepository;
    private readonly IRepository<MassIncident> _massIncidentRepository;
    
    public ExternalEventsQuery(IRepository<EntityEvent> entityEventRepository,
        IRepository<EnvironmentEvent> envEventRepository,
        IRepository<WorkFlowScheme> workflowSchemeRepository,
        IRepository<Call> callRepository,
        IRepository<WorkOrder> workOrderRepository,
        IRepository<Problem> problemRepository,
        IRepository<ChangeRequest> changeRequestRepository,
        IRepository<Message.Message> messageRepository,
        IRepository<WorkflowTracking> trackingRepository,
        IRepository<MassIncident> massIncidentRepository)
    {
        _entityEventRepository = entityEventRepository;
        _envEventRepository = envEventRepository;
        _workflowSchemeRepository = workflowSchemeRepository;
        _callRepository = callRepository;
        _workOrderRepository = workOrderRepository;
        _problemRepository = problemRepository;
        _changeRequestRepository = changeRequestRepository;
        _messageRepository = messageRepository;
        _trackingRepository = trackingRepository;
        _massIncidentRepository = massIncidentRepository;
    }

    public async Task<BaseEventItem[]> QueryAsync(CancellationToken cancellationToken = default)
    {
        var envResultQuery = from envEventResult in _envEventRepository.Query().AsNoTracking()
            join wfScheme in _workflowSchemeRepository.Query().AsNoTracking()
                on envEventResult.WorkflowSchemeID equals wfScheme.Id
                orderby envEventResult.Order
            select (new EntityEventListItem
            {
                Order = envEventResult.Order,
                Source = envEventResult.Source,
                Type = envEventResult.Type,
                ID = envEventResult.ID,
                IsProcessed = envEventResult.IsProcessed,
                CauserFullName = User.GetFullName(envEventResult.CauserID),
                CauserID = envEventResult.CauserID,
                OwnerID = envEventResult.OwnerID,
                UtcOwnedUntil = envEventResult.UtcOwnedUntil,
                UtcRegisteredAt = envEventResult.UtcRegisteredAt,
                WorkflowSchemeID = wfScheme.Id,
                WorkflowSchemeFullName = $"{wfScheme.Name} {wfScheme.MajorVersion}.{wfScheme.MinorVersion}, {wfScheme.Identifier}",
            });

        var envEvents = await envResultQuery.ToArrayAsync(cancellationToken);

        var entityResultQuery = from entityQuery in _entityEventRepository.Query().AsNoTracking()
            join workflowTracking in _trackingRepository.Query().AsNoTracking()
                on entityQuery.EntityId equals workflowTracking.EntityID 
            join workflowScheme in _workflowSchemeRepository.Query().AsNoTracking()
                on workflowTracking.WorkflowSchemeID equals workflowScheme.Id
            orderby entityQuery.Order
            
            select (new EntityEventListItem
            {
                Order = entityQuery.Order,
                Source = entityQuery.Source,
                Type = entityQuery.Type,
                ID = entityQuery.Id,
                IsProcessed = entityQuery.IsProcessed,
                CauserFullName = User.GetFullName(entityQuery.CauserId),
                CauserID = entityQuery.CauserId ?? Guid.Empty,
                EntityFullName = "",
                OwnerID = entityQuery.OwnerId,
                UtcOwnedUntil = entityQuery.UtcOwnedUntil,
                UtcRegisteredAt = entityQuery.UtcRegisteredAt,
                TargetStateID = entityQuery.TargetStateId,
                EntityID = entityQuery.EntityId,
                EntityClassID = entityQuery.EntityClassId,
                Argument = entityQuery.Argument,
                WorkflowSchemeID = workflowScheme.Id,
                WorkflowSchemeFullName = $"{workflowScheme.Name} {workflowScheme.MajorVersion}.{workflowScheme.MinorVersion}, {workflowScheme.Identifier}"
            });

        var callQueryResult = from callQuery in _callRepository.Query().AsNoTracking()
            select(new EventTempClass
            {
                Number = callQuery.Number,
                CategoryName = "Заявка",
                ID = callQuery.IMObjID,
                ClassID = ObjectClass.Call,
                ObjectName = Convert.ToString(callQuery.CallSummaryName)
            });
        
        var workOrderQueryResult = from workOrderQuery in _workOrderRepository.Query().AsNoTracking()
            select new EventTempClass
            {
                Number = workOrderQuery.Number,
                CategoryName = "Задание",
                ID = workOrderQuery.IMObjID,
                ClassID = ObjectClass.WorkOrder,
                ObjectName = Convert.ToString(workOrderQuery.Name)
            };

        var problemQueryResult = from problemQuery in _problemRepository.Query().AsNoTracking()
            select new EventTempClass
            {
                CategoryName = "Проблема",
                Number = problemQuery.Number,
                ID = problemQuery.IMObjID,
                ClassID = ObjectClass.Problem,
                ObjectName = Convert.ToString(problemQuery.Summary)
            };

        var changeRequestQueryResult = from changeRequestQuery in _changeRequestRepository.Query().AsNoTracking()
            select new EventTempClass
            {
                ID = changeRequestQuery.IMObjID,
                ClassID = ObjectClass.ChangeRequest,
                CategoryName = "Запрос на изменения",
                Number = changeRequestQuery.Number,
                ObjectName = Convert.ToString(changeRequestQuery.Summary)
            };
        
        var massIncidentQueryResult = from massIncidentQuery in _massIncidentRepository.Query().AsNoTracking()
            select new EventTempClass
            {
                ID = massIncidentQuery.IMObjID,
                ClassID = ObjectClass.MassIncident,
                CategoryName = "Массовый инцидент",
                Number = massIncidentQuery.ID,
                ObjectName = Convert.ToString(massIncidentQuery.Name)
            };

        var messageQueryResult = from messageQuery in _messageRepository.Query().AsNoTracking()
            select new EventTempClass
            {
                ID = messageQuery.IMObjID,
                ClassID = messageQuery.Type == 0 ? ObjectClass.MessageByMonitoring :
                    messageQuery.Type == 1 ? ObjectClass.MessageByInquiry :
                    messageQuery.Type == 2 ? ObjectClass.MessageByEmail :
                    messageQuery.Type == 3 ? ObjectClass.MessageByInquiryTask :
                    messageQuery.Type == 4 ? ObjectClass.MessageByTaskForUsers :
                    messageQuery.Type == 5 ? ObjectClass.MessageByOrganizationStructureImport : ObjectClass.Unknown,
                CategoryName = "",
                Number = 0,
                ObjectName = messageQuery.Type == 0 ? "Сообщение мониторинга" :
                    messageQuery.Type == 1 ? "Сообщение опроса" :
                    messageQuery.Type == 2 ? "Сообщение электронной почты" :
                    messageQuery.Type == 3 ? "Сводное сообщение задачи опроса" :
                    messageQuery.Type == 4 ? "Сводное сообщение импорта пользователей" :
                    messageQuery.Type == 5 ? "Сообщение подсистемы импорта оргструктуры" : "Неизвестно"
            };
        //TODO как придет время добавлять локализацию, убрать отсюда все константные названия и сделать их через локализацию
        
        
        var leftJoinQuery = callQueryResult.Concat(workOrderQueryResult).Concat(problemQueryResult)
            .Concat(changeRequestQueryResult).Concat(messageQueryResult).Concat(massIncidentQueryResult);

        var secondQueryResult = from entityQuery in entityResultQuery
            join leftJoinResult in leftJoinQuery
                on new { ID = entityQuery.EntityID, ClassID = entityQuery.EntityClassID } equals new
                    { leftJoinResult.ID, leftJoinResult.ClassID } into result
            from r in result.DefaultIfEmpty()
            select new EntityEventListItem
            {
                Order = entityQuery.Order,
                Source = entityQuery.Source,
                Type = entityQuery.Type,
                ID = entityQuery.ID,
                IsProcessed = entityQuery.IsProcessed,
                CauserFullName = entityQuery.CauserFullName,
                CauserID = entityQuery.CauserID,
                EntityFullName = r.FullName,
                OwnerID = entityQuery.OwnerID,
                UtcOwnedUntil = entityQuery.UtcOwnedUntil,
                UtcRegisteredAt = entityQuery.UtcRegisteredAt,
                TargetStateID = entityQuery.TargetStateID ?? "",
                EntityID = entityQuery.EntityID,
                EntityClassID = entityQuery.EntityClassID,
                WorkflowSchemeID = entityQuery.WorkflowSchemeID,
                WorkflowSchemeFullName = entityQuery.WorkflowSchemeFullName,
                Argument = entityQuery.Argument
            };

        var eventResult = new List<BaseEventItem>();
        eventResult.AddRange(envEvents);
        eventResult.AddRange(await secondQueryResult.ToArrayAsync(cancellationToken));
        
        return eventResult.ToArray();
    }

    private class EventTempClass
    {
        public Guid ID { get; init; }
        public ObjectClass ClassID { get; init; }
        public string FullName => $"{CategoryName} № {Number} {ObjectName}";
        public string CategoryName { get; init; }
        public int Number { get; init; }
        public string ObjectName { get; init; }
    }
}