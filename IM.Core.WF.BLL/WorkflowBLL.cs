using IM.Core.WF.BLL.Interfaces;
using IM.Core.WF.BLL.Interfaces.Models;
using InfraManager;
using InfraManager.DAL;
using InfraManager.DAL.Events;
using InfraManager.DAL.ServiceDesk;
using InfraManager.DAL.ServiceDesk.ChangeRequests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using InfraManager.DAL.Message;
using InfraManager.DAL.WF;
using InfraManager.DAL.WorkFlow;
using InfraManager.DAL.ServiceDesk.MassIncidents;

namespace IM.Core.WF.BLL
{
    internal class WorkflowBLL : IWorkflowBLL, ISelfRegisteredService<IWorkflowBLL>
    {
        private static readonly byte[] WFStatuses = { 2, 6, 18, 34 };

        private readonly IUnitOfWork _saveChangesCommand;
        private readonly IRepository<ChangeRequest> _rfcRepository;
        private readonly IRepository<Call> _callRepository;
        private readonly IRepository<Problem> _problemRepository;
        private readonly IRepository<WorkOrder> _workOrderRepository;
        private readonly IRepository<MassIncident> _massIncidentsRepository;
        private readonly IRepository<ExternalEventReference> _externalEventRepository;
        private readonly IRepository<EntityEvent> _entityEventsRepository;
        private readonly IRepository<Workflow> _workflowRepository;
        private readonly IRemoveWorkflowByRemovedObjectsCommand _removeWorkflowCommand;


        public WorkflowBLL(
            IUnitOfWork saveChangesCommand,
            IRepository<ChangeRequest> rfcRepository,
            IRepository<Call> callRepository,
            IRepository<Problem> problemRepository,
            IRepository<WorkOrder> workOrderRepository,
            IRepository<MassIncident> massIncidentsRepository,
            IRepository<ExternalEventReference> externalEventRepository,
            IRepository<Workflow> workflowRepository,
            IRemoveWorkflowByRemovedObjectsCommand removeWorkflowCommand,
            IRepository<EntityEvent> entityEventsRepository)
        {
            _rfcRepository = rfcRepository;
            _callRepository = callRepository;
            _problemRepository = problemRepository;
            _workOrderRepository = workOrderRepository;
            _massIncidentsRepository = massIncidentsRepository;
            _saveChangesCommand = saveChangesCommand;
            _workflowRepository = workflowRepository;
            _removeWorkflowCommand = removeWorkflowCommand;
            _entityEventsRepository = entityEventsRepository;
            _externalEventRepository = externalEventRepository;
        }

        public Guid? AcquireOwnership(Guid id, Guid ownerID, DateTime utcOwnedUntil, bool isRepeatableRead = false)
        {
            var utcNow = DateTime.UtcNow;
            var changed = _workflowRepository.Query()
                .Where(x => x.ID == id &&
                            ((x.OwnerID == ownerID && x.UtcOwnedUntil >= utcNow) || x.UtcOwnedUntil < utcNow ||
                             x.UtcOwnedUntil == null))
                .ToList();
            changed.ForEach(x =>
            {
                x.OwnerID = ownerID;
                x.UtcOwnedUntil = utcOwnedUntil;
            });
            SaveChanges(isRepeatableRead);

            if (changed.Count == 0)
            {
                return _workflowRepository.Query()
                    .Where(x => x.ID == id)
                    .Select(x => x.OwnerID)
                    .FirstOrDefault();
            }

            return ownerID;
        }

        public Guid? ReleaseOwnership(Guid id, Guid ownerID, bool isRepeatableRead = false)
        {
            Guid? curOwnerId = null;
            var changed = _workflowRepository.Query()
                .Where(x => x.ID == id && x.OwnerID == ownerID)
                .ToList();
            changed.ForEach(x =>
            {
                x.OwnerID = null;
                x.UtcOwnedUntil = null;
            });
            SaveChanges(isRepeatableRead);

            if (changed.Count == 0)
            {
                curOwnerId = _workflowRepository.Query()
                    .Where(x => x.ID == id)
                    .Select(x => x.OwnerID)
                    .FirstOrDefault();
            }

            return curOwnerId;
        }

        public bool Exists(Guid id)
        {
            return _workflowRepository.Query().Any(x => x.ID == id);
        }

        public List<Guid> RetrievePlannedWorkflowIDs(DateTime utcPlannedAt, Guid ownerID, DateTime utcOwnedUntil, bool isRepeatableRead = false)
        {
            var utcNow = DateTime.UtcNow;
            var workflows = _workflowRepository.Query()
                                .Where(x => (x.Status == 1 || x.Status == 8) &&
                                       (x.UtcPlannedAt == null || x.UtcPlannedAt <= utcPlannedAt) &&
                                       (x.OwnerID == null || x.UtcOwnedUntil < utcNow || x.UtcOwnedUntil == null))
                                .ToList();
            if (workflows.Count > 0)
            {
                workflows.ForEach(x =>
                {
                    x.OwnerID = ownerID;
                    x.UtcOwnedUntil = utcOwnedUntil;
                });
                SaveChanges(isRepeatableRead);
            }
            return workflows.Select(x => x.ID).ToList();
        }

        public Guid? Save(WorkflowModel model, Guid? ownerID, DateTime? utcOwnedUntil, bool isRepeatableRead = false)
        {
            Guid? curOwnerId = null;
            var utcNow = DateTime.UtcNow;
            if (WFStatuses.Contains(model.Status))
            {
                var exists = _workflowRepository.Query()
                    .Any(x => x.ID == model.ID &&
                              (x.OwnerID == ownerID || x.OwnerID == null || x.UtcOwnedUntil < utcNow || x.UtcOwnedUntil == null));
                if (exists)
                {
                    var extReferences = _externalEventRepository.Query()
                        .Where(x => x.WorkflowId == model.ID)
                        .ToList();
                    extReferences.ForEach(x => _externalEventRepository.Delete(x));
                    var wf = _workflowRepository.Query()
                        .FirstOrDefault(x => x.ID == model.ID);
                    if (wf != null)
                    {
                        _workflowRepository.Delete(wf);
                    }
                    ResetEntityWfParams(wf);
                    SaveChanges(isRepeatableRead);
                }
                else
                {
                    var ids = _workflowRepository.Query()
                        .Where(x => x.ID == model.ID)
                        .Select(x => x.OwnerID)
                        .ToArray();
                    if (ids != null && ids.Length != 0)
                        curOwnerId = ids[0];
                }
            }
            else
            {
                var isWorkflow = _workflowRepository.Query().Any(x => x.ID == model.ID);
                if (!isWorkflow)
                {
                    var wf = new InfraManager.DAL.WF.Workflow()
                    {
                        ID = model.ID,
                        WorkflowSchemeID = model.WorkflowSchemeID,
                        WorkflowSchemeIdentifier = model.WorkflowSchemeIdentifier,
                        WorkflowSchemeVersion = model.WorkflowSchemeVersion,
                        EntityClassID = model.EntityClassID,
                        EntityID = model.EntityID,
                        Status = model.Status,
                        CurrentStateID = model.CurrentStateID,
                        UtcModifiedAt = DateTime.UtcNow,
                        UtcPlannedAt = model.UtcPlannedAt,
                        OwnerID = ownerID,
                        UtcOwnedUntil = utcOwnedUntil,
                        Binaries = model.Binaries
                    };
                    _workflowRepository.Insert(wf);
                    SaveChanges(isRepeatableRead);
                }
                else
                {
                    var exists = _workflowRepository.Query()
                        .Any(x => x.ID == model.ID &&
                                  ((x.OwnerID == ownerID && x.UtcOwnedUntil >= utcNow) ||
                                   x.OwnerID == null || x.UtcOwnedUntil < utcNow || x.UtcOwnedUntil == null));
                    if (exists)
                    {
                        var wf = _workflowRepository.Query().FirstOrDefault(x => x.ID == model.ID);
                        if (wf != null)
                        {
                            wf.Status = model.Status;
                            wf.CurrentStateID = model.CurrentStateID;
                            wf.UtcModifiedAt = DateTime.UtcNow;
                            wf.UtcPlannedAt = model.UtcPlannedAt;
                            wf.OwnerID = ownerID;
                            wf.UtcOwnedUntil = utcOwnedUntil;
                            wf.Binaries = model.Binaries;
                            SaveChanges(isRepeatableRead);
                        }
                    }
                    else
                    {
                        var ids = _workflowRepository.Query()
                            .Where(x => x.ID == model.ID)
                            .Select(x => x.OwnerID)
                            .ToArray();
                        if (ids != null && ids.Length != 0)
                            curOwnerId = ids[0];
                    }
                }
            }

            return curOwnerId;
        }
        public void Delete(Guid id, bool isRepeatableRead = false)
        {
            var wf = _workflowRepository.Query()
                .FirstOrDefault(x => x.ID == id);
            _workflowRepository.Delete(wf);
            ResetEntityWfParams(wf);
           
            SaveChanges(isRepeatableRead);
        }

        private void ResetEntityWfParams(Workflow workflow)
        {
            IWorkflowEntity entity = workflow.EntityClassID switch
            {
                (int)ObjectClass.WorkOrder =>
                    _workOrderRepository.FirstOrDefault(wo => 
                        wo.IMObjID == workflow.EntityID),
                (int)ObjectClass.Call => _callRepository.FirstOrDefault(c => 
                    c.IMObjID == workflow.EntityID),
                (int)ObjectClass.Problem => _problemRepository.FirstOrDefault(p => 
                    p.IMObjID == workflow.EntityID),
                (int)ObjectClass.MassIncident => _massIncidentsRepository.FirstOrDefault(mi =>
                    mi.IMObjID == workflow.EntityID),
                (int)ObjectClass.ChangeRequest => _rfcRepository.FirstOrDefault(rfc =>
                    rfc.IMObjID == workflow.EntityID),
                _ => null
            };
            if (entity != null)
            {
                entity.EntityStateID = null;
                entity.WorkflowSchemeID = null;
            }
        }

        public (WorkflowModel workflowModel, Guid? currentOwnerID) Get(Guid id, Guid? ownerID, DateTime? utcOwnedUntil, bool isRepeatableRead = false)
        {
            var curOwnerId = ownerID;
            var changeOwner = false;
            
            if (ownerID.HasValue)
            {
                var utcDate = DateTime.UtcNow;
                var workflows = _workflowRepository.Query()
                                    .Where(x => x.ID == id &&
                                            ((x.OwnerID == ownerID && x.UtcOwnedUntil >= utcDate) ||
                                              x.OwnerID == null || x.UtcOwnedUntil < utcDate || x.UtcOwnedUntil == null))
                                    .ToList();
                changeOwner = workflows.Count == 0;
                
                if (workflows.Count > 0)
                {
                    workflows.ForEach(x =>
                    {
                        x.UtcOwnedUntil = utcOwnedUntil;
                        x.OwnerID = ownerID;
                    });
                    SaveChanges(isRepeatableRead);
                }
            }

            var (workflow, reqOwnerId) = _workflowRepository.Query()
                .Where(x => x.ID == id)
                .Select(x => Tuple.Create(new WorkflowModel
                {
                    ID = x.ID,
                    Binaries = x.Binaries,
                    CurrentStateID = x.CurrentStateID,
                    EntityClassID = x.EntityClassID,
                    EntityID = x.EntityID,
                    Status = x.Status,
                    UtcModifiedAt = x.UtcModifiedAt,
                    UtcPlannedAt = x.UtcPlannedAt,
                    WorkflowSchemeID = x.WorkflowSchemeID,
                    WorkflowSchemeIdentifier = x.WorkflowSchemeIdentifier,
                    WorkflowSchemeVersion = x.WorkflowSchemeVersion
                }, x.OwnerID))
                .FirstOrDefault() ?? new(null, null);
            if (workflow != null && changeOwner)
            {
                curOwnerId = reqOwnerId;
            }

            return (workflow, curOwnerId);
        }
        private void SaveChanges(bool isRepeatableRead)
        {
            _saveChangesCommand.Save(isRepeatableRead ? IsolationLevel.RepeatableRead : IsolationLevel.ReadCommitted);
        }
        
        public async Task RemoveRedundantWorkflow(CancellationToken cancellationToken = default)
        {
            await _removeWorkflowCommand.ExecuteAsync(cancellationToken);
            await _saveChangesCommand.SaveAsync(cancellationToken);
        }

        public async Task EntityModifiedForAllAsync(CancellationToken cancellationToken = default)
        {
            var workflows = await _workflowRepository.ToArrayAsync(cancellationToken);

            foreach (var el in workflows)
            {
                var newID = Guid.NewGuid(); // нам нужен один и тот же идентификатор для ExternalEventReference и EntityEvent
                _externalEventRepository.Insert(new ExternalEventReference(newID, el.EntityID)
                {
                    ConsiderationCount = 0
                });

                _entityEventsRepository.Insert(
                    new EntityEvent(newID, EventSource.Application, EventType.EntityModified,
                        (ObjectClass)el.EntityClassID, el.EntityID, Guid.Empty)
                    {
                        IsProcessed = false,
                        Argument = null
                    });
            }
        
            await _saveChangesCommand.SaveAsync(cancellationToken);
        }
    }
}