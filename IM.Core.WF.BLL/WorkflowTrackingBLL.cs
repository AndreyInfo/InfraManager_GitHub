using IM.Core.WF.BLL.Interfaces;
using IM.Core.WF.BLL.Interfaces.Models;
using InfraManager;
using InfraManager.CrossPlatform.WebApi.Contracts.Common;
using InfraManager.DAL;
using InfraManager.DAL.Message;
using InfraManager.DAL.ServiceDesk;
using InfraManager.DAL.ServiceDesk.ChangeRequests;
using InfraManager.DAL.WF;
using System;
using System.Collections.Generic;
using System.Linq;
using InfraManager.BLL;
using InfraManager.ResourcesArea;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using WorkflowEvent = InfraManager.DAL.WF.WorkflowEvent;
using System.Transactions;
using InfraManager.DAL.ServiceDesk.MassIncidents;

namespace IM.Core.WF.BLL
{
    internal class WorkflowTrackingBLL : IWorkflowTrackingBLL, ISelfRegisteredService<IWorkflowTrackingBLL>
    {
        private readonly IUnitOfWork _saveChangesCommand;
        private readonly IReadonlyRepository<InfraManager.DAL.WF.Workflow> _workflowRepository;
        private readonly IRepository<WorkflowEvent> _workflowEventRepository;
        private readonly IRepository<WorkflowTracking> _workflowTrackingRepository;
        private readonly IRepository<WorkflowStateTracking> _wfStateTrackingRepository;
        private readonly IRepository<WorkflowStateTrackingDetail> _wfStateTrackingDetailsRepository;
        private readonly IReadonlyRepository<WorkFlowScheme> _workflowSchemeRepository;
        private readonly IReadonlyRepository<Call> _callRepository;
        private readonly IReadonlyRepository<CallService> _callServiceRepository;
        private readonly IReadonlyRepository<CallType> _callTypeRepository;
        private readonly IReadonlyRepository<Problem> _problemRepository;
        private readonly IReadonlyRepository<ProblemType> _problemTypeRepository;
        private readonly IReadonlyRepository<ChangeRequest> _changeRequestRepository;
        private readonly IReadonlyRepository<ChangeRequestType> _changeRequestTypeRepository;
        private readonly IReadonlyRepository<WorkOrder> _workOrderRepository;
        private readonly IReadonlyRepository<MassIncident> _massIncidentRepository;
        private readonly IReadonlyRepository<MassIncidentType> _massIncidentTypeRepository;
        private readonly IReadonlyRepository<WorkOrderType> _workOrderTypeRepository;
        private readonly IReadonlyRepository<Message> _messageRepository;
        private readonly IReadonlyRepository<User> _userRepository;
        private readonly IMapper _mapper;

        public WorkflowTrackingBLL(
                    IUnitOfWork saveChangesCommand,
                    IReadonlyRepository<Call> callRepository,
                    IReadonlyRepository<InfraManager.DAL.WF.Workflow> workflowRepository,
                    IRepository<WorkflowEvent> workflowEventRepository,
                    IRepository<WorkflowTracking> workflowTrackingRepository,
                    IReadonlyRepository<CallService> callServiceRepository,
                    IReadonlyRepository<CallType> callTypeRepository,
                    IRepository<WorkflowStateTracking> wfStateTrackingRepository,
                    IReadonlyRepository<WorkFlowScheme> workflowSchemeRepository,
                    IRepository<WorkflowStateTrackingDetail> wfStateTrackingDetailsRepository,
                    IReadonlyRepository<ProblemType> problemTypeRepository,
                    IReadonlyRepository<Problem> problemRepository,
                    IReadonlyRepository<ChangeRequest> changeRequestRepository,
                    IReadonlyRepository<ChangeRequestType> changeRequestTypeRepository,
                    IReadonlyRepository<WorkOrder> workOrderRepository,
                    IReadonlyRepository<WorkOrderType> workOrderTypeRepository,
                    IReadonlyRepository<Message> messageRepository,
                    IMapper mapper,
                    IReadonlyRepository<User> userRepository,
                    IReadonlyRepository<MassIncident> massIncidentRepository, 
                    IReadonlyRepository<MassIncidentType> massIncidentTypeRepository)
        {
            _callRepository = callRepository;
            _workflowEventRepository = workflowEventRepository;
            _workflowRepository = workflowRepository;
            _saveChangesCommand = saveChangesCommand;
            _workflowTrackingRepository = workflowTrackingRepository;
            _wfStateTrackingRepository = wfStateTrackingRepository;
            _workflowSchemeRepository = workflowSchemeRepository;
            _wfStateTrackingDetailsRepository = wfStateTrackingDetailsRepository;
            _callServiceRepository = callServiceRepository;
            _callTypeRepository = callTypeRepository;
            _problemTypeRepository = problemTypeRepository;
            _problemRepository = problemRepository;
            _changeRequestRepository = changeRequestRepository;
            _changeRequestTypeRepository = changeRequestTypeRepository;
            _workOrderRepository = workOrderRepository;
            _workOrderTypeRepository = workOrderTypeRepository;
            _messageRepository = messageRepository;
            _mapper = mapper;
            _userRepository = userRepository;
            _massIncidentRepository = massIncidentRepository;
            _massIncidentTypeRepository = massIncidentTypeRepository;
        }

        public void Insert(Guid workflowTrackingID, WorkflowStateTrackingModel model)
        {
            var state = new WorkflowStateTracking()
            {
                WorkflowTrackingId = workflowTrackingID,
                StateId = model.StateId,
                StateName = model.StateName,
                UtcEnteredAt = model.UtcEnteredAt,
                ExecutorId = model.ExecutorID,
                UtcLeavedAt = model.UtcLeavedAt,
                TimeSpanInWorkMinutes = model.TimeSpanInWorkMinutes
            };
            _wfStateTrackingRepository.Insert(state);
            _saveChangesCommand.Save();
        }

        private IQueryable<WorkflowTrackingModel> GetSearchQuery()
        {
            // Заявки
            var callQuery = from workflowTracking in _workflowTrackingRepository.Query()
                join scheme in _workflowSchemeRepository.Query()
                    on workflowTracking.WorkflowSchemeID equals scheme.Id
                join call in _callRepository.Query()
                    on workflowTracking.ID equals call.IMObjID
                join callService in _callServiceRepository.Query()
                    on call.CallServiceID equals callService.ID
                join callType in _callTypeRepository.Query().IgnoreQueryFilters()
                    on call.CallType.ID equals callType.ID
                    
                let stateName = (from stateTracking in _wfStateTrackingRepository.Query().AsNoTracking()
                    where stateTracking.WorkflowTrackingId == workflowTracking.ID   
                    orderby stateTracking.UtcEnteredAt 
                    select stateTracking.StateName).ToList()

                
                where workflowTracking.EntityClassID == (int)ObjectClass.Call
                select new WorkflowTrackingModel
                {
                    ID = workflowTracking.ID,
                    WorkflowSchemeID = workflowTracking.WorkflowSchemeID,
                    WorkflowSchemeIdentifier = workflowTracking.WorkflowSchemeIdentifier,
                    WorkflowSchemeVersion = workflowTracking.WorkflowSchemeVersion,
                    WorkflowSchemeName = scheme.Name,
                    EntityClassID = workflowTracking.EntityClassID,
                    EntityID = workflowTracking.EntityID,
                    UtcInitializedAt = workflowTracking.UtcInitializedAt,
                    UtcTerminatedAt = workflowTracking.UtcTerminatedAt,
                    EntityCategoryName = Resources.Call,
                    EntityTypeName = CallType.GetFullCallTypeName(callType.ID),
                    Number = call.Number,
                    SummaryName = Convert.ToString(call.CallSummaryName),
                    State = stateName.Last() ?? string.Empty
                };

            //проблема
            var problemQuery = from workflowTracking in _workflowTrackingRepository.Query()
                join workflowScheme in _workflowSchemeRepository.Query()
                    on workflowTracking.WorkflowSchemeID equals workflowScheme.Id
                join problem in _problemRepository.Query()
                    on workflowTracking.ID equals problem.IMObjID
                join problemType in _problemTypeRepository.Query().IgnoreQueryFilters()
                    on problem.TypeID equals problemType.ID
                    
                let stateName = (from stateTracking in _wfStateTrackingRepository.Query().AsNoTracking()
                    where stateTracking.WorkflowTrackingId == workflowTracking.ID   
                    orderby stateTracking.UtcEnteredAt 
                    select stateTracking.StateName).ToList()
                
                where workflowTracking.EntityClassID == (int)ObjectClass.Problem
                select new WorkflowTrackingModel
                {
                    ID = workflowTracking.ID,
                    WorkflowSchemeID = workflowTracking.WorkflowSchemeID,
                    WorkflowSchemeIdentifier = workflowTracking.WorkflowSchemeIdentifier,
                    WorkflowSchemeVersion = workflowTracking.WorkflowSchemeVersion,
                    WorkflowSchemeName = workflowScheme.Name,
                    EntityClassID = workflowTracking.EntityClassID,
                    EntityID = workflowTracking.EntityID,
                    UtcInitializedAt = workflowTracking.UtcInitializedAt,
                    UtcTerminatedAt = workflowTracking.UtcTerminatedAt,
                    EntityCategoryName = Resources.Problem,
                    EntityTypeName = ProblemType.GetFullProblemTypeName(problemType.ID),
                    Number = problem.Number,
                    SummaryName = Convert.ToString(problem.Summary),
                    State = stateName.Last() ?? string.Empty
                };

            var resultQuery = callQuery.Union(problemQuery);

            //запрос на изменения
            var changeQuery = from workflowTracking in _workflowTrackingRepository.Query().AsNoTracking()
                join workflowScheme in _workflowSchemeRepository.Query().AsNoTracking()
                    on workflowTracking.WorkflowSchemeID equals workflowScheme.Id
                join changeRequest in _changeRequestRepository.Query().AsNoTracking()
                    on workflowTracking.ID equals changeRequest.IMObjID
                join changeRequestType in _changeRequestTypeRepository.Query().AsNoTracking().IgnoreQueryFilters()
                    on changeRequest.RFCTypeID equals changeRequestType.ID
                    
                let stateName = (from stateTracking in _wfStateTrackingRepository.Query().AsNoTracking()
                    where stateTracking.WorkflowTrackingId == workflowTracking.ID   
                    orderby stateTracking.UtcEnteredAt 
                    select stateTracking.StateName).ToList()

                
                where workflowTracking.EntityClassID == (int)ObjectClass.ChangeRequest
                select new WorkflowTrackingModel
                {
                    ID = workflowTracking.ID,
                    WorkflowSchemeID = workflowTracking.WorkflowSchemeID,
                    WorkflowSchemeIdentifier = workflowTracking.WorkflowSchemeIdentifier,
                    WorkflowSchemeVersion = workflowTracking.WorkflowSchemeVersion,
                    WorkflowSchemeName = workflowScheme.Name,
                    EntityClassID = workflowTracking.EntityClassID,
                    EntityID = workflowTracking.EntityID,
                    UtcInitializedAt = workflowTracking.UtcInitializedAt,
                    UtcTerminatedAt = workflowTracking.UtcTerminatedAt,
                    EntityCategoryName = Resources.RFC,
                    Number = changeRequest.Number,
                    EntityTypeName = Convert.ToString(changeRequestType.Name),
                    SummaryName = Convert.ToString(changeRequest.Summary),
                    State = stateName.Last() ?? string.Empty
                };
            resultQuery = resultQuery.Union(changeQuery);

            // задание
            var taskQuery = from workflowTracking in _workflowTrackingRepository.Query().AsNoTracking()
                join workflowScheme in _workflowSchemeRepository.Query().AsNoTracking()
                    on workflowTracking.WorkflowSchemeID equals workflowScheme.Id
                join workOrder in _workOrderRepository.Query().AsNoTracking()
                    on workflowTracking.ID equals workOrder.IMObjID
                join workOrderType in _workOrderTypeRepository.Query().AsNoTracking().IgnoreQueryFilters()
                    on workOrder.TypeID equals workOrderType.ID
                    
                let stateName = (from stateTracking in _wfStateTrackingRepository.Query().AsNoTracking()
                    where stateTracking.WorkflowTrackingId == workflowTracking.ID   
                    orderby stateTracking.UtcEnteredAt 
                    select stateTracking.StateName).ToList()

                
                where workflowTracking.EntityClassID == (int)ObjectClass.WorkOrder
                select new WorkflowTrackingModel
                {
                    ID = workflowTracking.ID,
                    WorkflowSchemeID = workflowTracking.WorkflowSchemeID,
                    WorkflowSchemeIdentifier = workflowTracking.WorkflowSchemeIdentifier,
                    WorkflowSchemeVersion = workflowTracking.WorkflowSchemeVersion,
                    WorkflowSchemeName = workflowScheme.Name,
                    EntityClassID = workflowTracking.EntityClassID,
                    EntityID = workflowTracking.EntityID,
                    UtcInitializedAt = workflowTracking.UtcInitializedAt,
                    UtcTerminatedAt = workflowTracking.UtcTerminatedAt,
                    EntityCategoryName = Resources.WorkOrder,
                    Number = workOrder.Number,
                    EntityTypeName = Convert.ToString(workOrderType.Name),
                    SummaryName = Convert.ToString(workOrder.Name),
                    State = stateName.Last() ?? string.Empty
                };
            resultQuery = resultQuery.Union(taskQuery);
            
              var massIncidentQuery = from workflowTracking in _workflowTrackingRepository.Query().AsNoTracking()
                join workflowScheme in _workflowSchemeRepository.Query().AsNoTracking()
                    on workflowTracking.WorkflowSchemeID equals workflowScheme.Id
                join massIncident in _massIncidentRepository.Query().AsNoTracking()
                    on workflowTracking.ID equals massIncident.IMObjID
                join massIncidentType in _massIncidentTypeRepository.Query().AsNoTracking().IgnoreQueryFilters()
                    on massIncident.TypeID equals massIncidentType.ID
                    
                let stateName = (from stateTracking in _wfStateTrackingRepository.Query().AsNoTracking()
                    where stateTracking.WorkflowTrackingId == workflowTracking.ID   
                    orderby stateTracking.UtcEnteredAt 
                    select stateTracking.StateName).ToList()

                
                where workflowTracking.EntityClassID == (int)ObjectClass.MassIncident
                select new WorkflowTrackingModel
                {
                    ID = workflowTracking.ID,
                    WorkflowSchemeID = workflowTracking.WorkflowSchemeID,
                    WorkflowSchemeIdentifier = workflowTracking.WorkflowSchemeIdentifier,
                    WorkflowSchemeVersion = workflowTracking.WorkflowSchemeVersion,
                    WorkflowSchemeName = workflowScheme.Name,
                    EntityClassID = workflowTracking.EntityClassID,
                    EntityID = workflowTracking.EntityID,
                    UtcInitializedAt = workflowTracking.UtcInitializedAt,
                    UtcTerminatedAt = workflowTracking.UtcTerminatedAt,
                    EntityCategoryName = Resources.MassIncident,
                    Number = massIncident.ID,
                    EntityTypeName = Convert.ToString(massIncidentType.Name),
                    SummaryName = Convert.ToString(massIncident.Name),
                    State = stateName.Last() ?? string.Empty
                };
              resultQuery = resultQuery.Union(massIncidentQuery);
              
             var queryTypeList = new Dictionary<int, string>();
                queryTypeList.Add((int)ObjectClass.MessageByEmail, Resources.SubsustemMessage_email);
                queryTypeList.Add((int)ObjectClass.MessageByMonitoring, Resources.SubsustemMessage_monitoring);
                queryTypeList.Add((int)ObjectClass.MessageByInquiry, Resources.SubsustemMessage_inquiry);
                queryTypeList.Add((int)ObjectClass.MessageByInquiryTask, Resources.SubsustemMessage_inquiryTask);
                queryTypeList.Add((int)ObjectClass.MessageByIntegration, Resources.SubsustemMessage_integration);
                queryTypeList.Add((int)ObjectClass.MessageByOrganizationStructureImport, Resources.SubsustemMessage_structureImport);
                queryTypeList.Add((int)ObjectClass.MessageByTaskForUsers, Resources.SubsustemMessage_structureImportTask);

                foreach (var queryType in queryTypeList)
                {
                    var messageQuery = from workflowTracking in _workflowTrackingRepository.Query().AsNoTracking()
                                       join workflowScheme in _workflowSchemeRepository.Query().AsNoTracking()
                                       on workflowTracking.WorkflowSchemeID equals workflowScheme.Id
                                       join message in _messageRepository.Query().AsNoTracking()
                                       on workflowTracking.ID equals message.IMObjID
                                       where workflowTracking.EntityClassID == queryType.Key
                                       
                                       let stateName = (from stateTracking in _wfStateTrackingRepository.Query().AsNoTracking()
                                           where stateTracking.WorkflowTrackingId == workflowTracking.ID   
                                           orderby stateTracking.UtcEnteredAt 
                                           select stateTracking.StateName).ToList()

                                       select new WorkflowTrackingModel()
                                       {
                                           ID = workflowTracking.ID,
                                           WorkflowSchemeID = workflowTracking.WorkflowSchemeID,
                                           WorkflowSchemeIdentifier = workflowTracking.WorkflowSchemeIdentifier,
                                           WorkflowSchemeVersion = workflowTracking.WorkflowSchemeVersion,
                                           WorkflowSchemeName = workflowScheme.Name,
                                           EntityClassID = workflowTracking.EntityClassID,
                                           EntityID = workflowTracking.EntityID,
                                           UtcInitializedAt = workflowTracking.UtcInitializedAt,
                                           UtcTerminatedAt = workflowTracking.UtcTerminatedAt,
                                           EntityCategoryName = Resources.SubsustemMessage,
                                           Number = 0,
                                           SummaryName = queryType.Value,
                                           EntityTypeName = Convert.ToString(Resources.SubsustemMessage + " " + queryType.Value),
                                           State = stateName.Last() ?? string.Empty
                                       };
                    resultQuery = resultQuery.Union(messageQuery);
                }

                return resultQuery.OrderBy(x => x.EntityClassID);
        }

        public WorkflowTrackingModel Get(Guid id)
        {
            var resultQuery = GetSearchQuery();
            return resultQuery.FirstOrDefault(x => x.ID == id);
        }

        public WorkflowTracking GetNameDetailsBySchemeIdentifier(string schemeIdentifier)
        {
            return _workflowTrackingRepository.FirstOrDefault(x => x.WorkflowSchemeIdentifier == schemeIdentifier) ??
                   throw new ObjectNotFoundException("Рабочая процедура не найдена");
        }

        public void Delete()
        {
            var utcnow = DateTime.UtcNow.AddDays(-3.0);
            var wEvents = _workflowEventRepository.Query()
                .Join(_workflowTrackingRepository.Query(),
                      x => x.WorkflowID,
                      x => x.ID,
                      (we, wt) => new
                      {
                          we,
                          wt
                      })
                .Where(x => !_workflowRepository.Query().Any(z => z.ID == x.wt.ID) &&
                       ((x.wt.UtcTerminatedAt != null && x.wt.UtcTerminatedAt < utcnow) ||
                        (x.wt.UtcTerminatedAt == null && x.wt.UtcInitializedAt < utcnow)))
                .Select(x => x.we)
                .ToList();
            wEvents.ForEach(x => _workflowEventRepository.Delete(x));
            _saveChangesCommand.Save();
        }

        public void Delete(Guid workflowTrackingID, bool isRepeatableRead = false)
        {
            var wfStates = _wfStateTrackingRepository
                                .Where(x => x.WorkflowTrackingId == workflowTrackingID)
                                .ToArray();
            wfStates.ForEach(x => _wfStateTrackingRepository.Delete(x));
            SaveChanges(isRepeatableRead);

            var trackings = _workflowTrackingRepository
                                .Where(x => x.ID == workflowTrackingID)
                                .ToArray();
            trackings.ForEach(x => _workflowTrackingRepository.Delete(x));
            SaveChanges(isRepeatableRead);
        }

        public void Update(Guid workflowTrackingID, WorkflowStateTrackingModel workflowStateTracking)
        {
            var states = _wfStateTrackingRepository
                            .Query()
                            .Where(x => x.WorkflowTrackingId == workflowTrackingID && x.UtcLeavedAt == null)
                            .ToArray();
            states.ForEach(x =>
            {
                x.ExecutorId = workflowStateTracking.ExecutorID;
                x.UtcLeavedAt = workflowStateTracking.UtcLeavedAt;
                x.TimeSpanInWorkMinutes = workflowStateTracking.TimeSpanInWorkMinutes;
            });
            _saveChangesCommand.Save();
        }

        public CalendarInfo GetEntityCalendarInfo(Guid entityID)
        {
            var query = from stateTracking in _wfStateTrackingRepository.Query().AsNoTracking()
                        join call in _callRepository.Query().AsNoTracking()
                        on stateTracking.WorkflowTrackingId equals call.IMObjID
                        into qJoin
                        from call in qJoin.DefaultIfEmpty()
                        where stateTracking.WorkflowTrackingId == entityID && stateTracking.UtcLeavedAt == null
                        select new CalendarInfo()
                        {
                            UtcEnteredAt = stateTracking.UtcEnteredAt,
                            CalendarWorkScheduleId = call.CalendarWorkScheduleID,
                            TimeZoneId = call.TimeZoneID
                        };

            return query.FirstOrDefault();
        }

        public void UpdateStateTrackingDetail(Guid id, DateTime? nextUtcDate, int? timeSpanInWorkMinutes, int? stageTimeSpanInMinutes, int? stageTimeSpanInWorkMinutes)
        {
            var detail = _wfStateTrackingDetailsRepository
                            .Query()
                            .FirstOrDefault(x => x.Id == id);
            if (detail != null)
            {
                if (nextUtcDate.HasValue)
                    detail.NextUtcDate = nextUtcDate.Value;
                if (timeSpanInWorkMinutes.HasValue)
                    detail.TimeSpanInWorkMinutes = timeSpanInWorkMinutes.Value;
                if (stageTimeSpanInMinutes.HasValue)
                    detail.StageTimeSpanInMinutes = stageTimeSpanInMinutes.Value;
                if (stageTimeSpanInWorkMinutes.HasValue)
                    detail.StageTimeSpanInWorkMinutes = stageTimeSpanInWorkMinutes.Value;
                _saveChangesCommand.Save();
            }
        }


        public void Update(Guid id, DateTime time)
        {
            var wfTracking = _workflowTrackingRepository.Query().FirstOrDefault(x => x.ID == id);
            if (wfTracking != null)
            {
                wfTracking.UtcTerminatedAt = time;
                _saveChangesCommand.Save();
            }
        }

        public bool Exists(Guid id)
        {
            return _workflowTrackingRepository.Query().Any(x => x.ID == id);
        }

        public void Insert(WorkflowTrackingModel workflowTracking, bool isRepeatableRead = false)
        {
            if (!Exists(workflowTracking.ID))
            {
                var wfTracking = new WorkflowTracking()
                {
                    ID = workflowTracking.ID,
                    WorkflowSchemeID = workflowTracking.WorkflowSchemeID,
                    WorkflowSchemeIdentifier = workflowTracking.WorkflowSchemeIdentifier,
                    WorkflowSchemeVersion = workflowTracking.WorkflowSchemeVersion,
                    EntityClassID = workflowTracking.EntityClassID,
                    EntityID = workflowTracking.EntityID,
                    UtcInitializedAt = workflowTracking.UtcInitializedAt
                };
                _workflowTrackingRepository.Insert(wfTracking);
                SaveChanges(isRepeatableRead);
            }
        }

        public async Task<WorkflowEventModel[]> GetEventsAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var events = await _workflowEventRepository.ToArrayAsync(x => x.WorkflowID == id, cancellationToken);
            return _mapper.Map<WorkflowEventModel[]>(events);
        }


        public async Task<WorkflowStateTrackingModel[]> GetStateTrackingsAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var query = from stateTracking in _wfStateTrackingRepository.Query().AsNoTracking()
                join executor in _userRepository.Query().AsNoTracking()
                    on stateTracking.ExecutorId equals executor.IMObjID into executorJoinResult
                from executorQuery in executorJoinResult.DefaultIfEmpty()
                where stateTracking.WorkflowTrackingId == id
                select new WorkflowStateTrackingModel
                {
                    ExecutorID = stateTracking.ExecutorId,
                    ExecutorName = executorQuery.FullName,
                    StateId = stateTracking.StateId,
                    StateName = stateTracking.StateName,
                    UtcEnteredAt = stateTracking.UtcEnteredAt,
                    UtcLeavedAt = stateTracking.UtcLeavedAt,
                    TimeSpanInWorkMinutes = stateTracking.TimeSpanInWorkMinutes
                };

            return await query.ToArrayAsync(cancellationToken);
        }
        
        public Task<WorkflowTrackingModel[]> GetListAsync(BaseFilter filter, CancellationToken cancellationToken)
        {
                
                var resultQuery = GetSearchQuery();

                if (!string.IsNullOrEmpty(filter.SearchString))
                {
                    resultQuery = resultQuery.Where(x =>
                        (x.EntityCategoryName.ToLower() + " " + x.Number + " " + x.SummaryName.ToLower()).Contains(
                            filter.SearchString.ToLower()));
                }
                resultQuery = resultQuery.Skip(filter.StartRecordIndex).Take(filter.CountRecords);
                return resultQuery.ToArrayAsync(cancellationToken);
        }

        private void SaveChanges(bool isRepeatableRead)
        {
            _saveChangesCommand.Save(isRepeatableRead ? IsolationLevel.RepeatableRead : IsolationLevel.ReadCommitted);
        }
    }
}
