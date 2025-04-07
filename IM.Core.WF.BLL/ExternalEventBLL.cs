using IM.Core.WF.BLL.Interfaces;
using InfraManager;
using InfraManager.DAL;
using InfraManager.DAL.Events;
using InfraManager.WE.DTL.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using InfraManager.DAL.WorkFlow.Events;

namespace IM.Core.WF.BLL
{
    internal class ExternalEventBLL : IExternalEventBLL, ISelfRegisteredService<IExternalEventBLL>
    {
        private readonly IUnitOfWork saveChangesCommand;
        private readonly IRepository<InfraManager.DAL.Events.EntityEvent> _entityEventRepository;
        private readonly IRepository<InfraManager.DAL.ServiceDesk.EnvironmentEvent> _envEventRepository;
        private readonly IRepository<ExternalEventReference> externEventRepository;
        private readonly IExternalEventsQuery _eventsQuery;
        
        public ExternalEventBLL(
            IUnitOfWork saveChangesCommand,
            IRepository<InfraManager.DAL.Events.EntityEvent> entityEventRepository,
            IRepository<ExternalEventReference> externEventRepository,
            IRepository<InfraManager.DAL.ServiceDesk.EnvironmentEvent> envEventRepository,
            IExternalEventsQuery eventsQuery)
        {
            this.saveChangesCommand = saveChangesCommand;
            this.externEventRepository = externEventRepository;
            this._envEventRepository = envEventRepository;
            _eventsQuery = eventsQuery;
            this._entityEventRepository = entityEventRepository;
        }
        
        public async Task<BaseEventItem[]> GetAllListAsync(CancellationToken cancellationToken = default)
        {
            return await _eventsQuery.QueryAsync(cancellationToken);
        }

        public ExternalEvent[] GetList(Guid ownerID, DateTime utcOwnedUntil, int maxConsiderationCount)
        {
            if (maxConsiderationCount > 0)
            {
                var extEvents = externEventRepository.Query()
                    .Where(x => x.ConsiderationCount > maxConsiderationCount)
                    .ToList();
                extEvents.ForEach(x => externEventRepository.Delete(x));
                saveChangesCommand.Save();
            }

            var envEvents = _envEventRepository.Query()
                .Where(x => !externEventRepository.Query().Any(z => z.ExternalEventId == x.ID))
                .ToList();
            envEvents.ForEach(x => _envEventRepository.Delete(x));
            saveChangesCommand.Save();

            var utcNow = DateTime.UtcNow;
            var result = new List<ExternalEvent>();
            var envEventList = _envEventRepository.Query()
                .Where(x => x.OwnerID == null || x.UtcOwnedUntil < utcNow)
                .ToList();
            if (envEventList.Count > 0)
            {
                foreach (var envEvent in envEventList)
                {
                    var externalEvent = new InfraManager.WE.DTL.Events.EnvironmentEvent()
                    {
                        ID = envEvent.ID,
                        CauserID = envEvent.CauserID,
                        OwnerID = envEvent.OwnerID,
                        Order = envEvent.Order,
                        Source = (int)envEvent.Source,
                        WorkflowSchemeID = envEvent.WorkflowSchemeID,
                        IsProcessed = envEvent.IsProcessed,
                        UtcOwnedUntil = envEvent.UtcOwnedUntil,
                        UtcRegisteredAt = envEvent.UtcRegisteredAt,
                        Type = (int)envEvent.Type
                    };
                    result.Add(externalEvent);
                }

                envEventList.ForEach(x =>
                {
                    x.OwnerID = ownerID;
                    x.UtcOwnedUntil = utcOwnedUntil;
                });
            }

            saveChangesCommand.Save();

            var entEvents = _entityEventRepository.Query()
                .Where(x => !externEventRepository.Query().Any(z => z.ExternalEventId == x.Id))
                .ToList();
            entEvents.ForEach(x => _entityEventRepository.Delete(x));
            saveChangesCommand.Save();

            var entEventList = _entityEventRepository.Query()
                .Where(x => x.OwnerId == null || x.UtcOwnedUntil < utcNow)
                .ToList();
            if (entEventList.Count > 0)
            {
                foreach (var entEvent in entEventList)
                {
                    var entityEvent = new InfraManager.WE.DTL.Events.EntityEvent()
                    {
                        ID = entEvent.Id,
                        EntityID = entEvent.EntityId,
                        CauserID = entEvent.CauserId,
                        EntityClassID = (int)entEvent.EntityClassId,
                        TargetStateID = entEvent.TargetStateId,
                        Argument = entEvent.Argument,
                        IsProcessed = entEvent.IsProcessed,
                        Order = entEvent.Order,
                        OwnerID = entEvent.OwnerId,
                        Source = (int)entEvent.Source,
                        Type = (int)entEvent.Type,
                        UtcOwnedUntil = entEvent.UtcOwnedUntil,
                        UtcRegisteredAt = entEvent.UtcRegisteredAt
                    };
                    result.Add(entityEvent);
                }

                entEventList.ForEach(x =>
                {
                    x.OwnerId = ownerID;
                    x.UtcOwnedUntil = utcOwnedUntil;
                });
            }

            saveChangesCommand.Save();
            return result.ToArray();
        }

        /// <summary>
        /// Метод определяет, есть ли активные события для сущности
        /// Используется при смене состояния ServiceDesk forms.
        /// Блокируем UI до тех пор, пока есть активные события
        /// </summary>
        /// <param name="entityID">GUID - ID сущности (Call, Problem, WorkOrder)</param>
        public bool EntityHasActiveEvents(Guid entityID)
        {
            var availableTypes = new []
            {
                /* TODO: удалить, как только #6775 пройдет тестирование
                   TODO: править синхроннс с EntityHasActiveEvents из ExternalEventSqlServerDataManager
                EventType.EntityCreated,
                EventType.EntityDeleted,
                EventType.WorkflowCreated,
                EventType.WorkflowDeleted, 
                EventType.WorkflowStateChanging,
                EventType.WorkflowStateChanged,
                */
                EventType.EntityStateSet
            };
            var readUncommittedOptions = new TransactionOptions
            {
                IsolationLevel = IsolationLevel.ReadUncommitted
            };
            bool hasEvents;
            using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, readUncommittedOptions))
            {
                hasEvents = _entityEventRepository
                    .Query()
                    .Any(entityEvent => entityEvent.EntityId == entityID
                                        && !entityEvent.IsProcessed
                                        && availableTypes.Contains(entityEvent.Type) 
                                        && externEventRepository
                                            .Query()
                                            .Any(ext => ext.ExternalEventId == entityEvent.Id)
                                        );
                scope.Complete();
            }
            
            return hasEvents;
        }

        public Guid[] GetWorkflowIDs(Guid externalEventID)
        {
            var entity = _entityEventRepository.Query()
                                    .FirstOrDefault(x => x.Id == externalEventID);
            if (entity == null || !externEventRepository.Query().Any(x => x.ExternalEventId == externalEventID && x.WorkflowId == entity.EntityId))
            {
                return externEventRepository.Query().Where(x => x.ExternalEventId == externalEventID)
                    .Select(x => x.WorkflowId)
                    .ToArray();
            }
            else
            {
                var ids = new List<Guid>
                {
                    entity.EntityId
                };
                var idList = externEventRepository.Query().Where(x => x.ExternalEventId == externalEventID && x.WorkflowId != entity.EntityId)
                    .Select(x => x.WorkflowId)
                    .ToList();
                ids.AddRange(idList);
                return ids.ToArray();
            }
        }

        public void DeleteExternalEventReference(Guid? externalEventID, Guid? workflowID, bool isRepeatableRead = false)
        {
            IQueryable<ExternalEventReference> query;
            var extRefQuery = externEventRepository.Query();
            if (externalEventID.HasValue && workflowID.HasValue)
                query = extRefQuery.Where(x => x.ExternalEventId == externalEventID && x.WorkflowId == workflowID);
            else if (externalEventID.HasValue)
                query = extRefQuery.Where(x => x.ExternalEventId == externalEventID);
            else if (workflowID.HasValue)
                query = extRefQuery.Where(x => x.WorkflowId == workflowID);
            else
                throw new NotSupportedException();
            query.ToList()
                 .ForEach(x => externEventRepository.Delete(x));
            SaveChanges(isRepeatableRead);
        }

        public Guid? AcquireEntityEventOwnership(Guid id, Guid ownerID, DateTime utcOwnedUntil)
        {
            Guid? newOwnerId = null;
            var utcDate = DateTime.UtcNow;
            var entityEvent = _entityEventRepository.Query()
                                .FirstOrDefault(x => x.Id == id && (x.OwnerId == ownerID && x.UtcOwnedUntil >= utcDate) ||
                                                x.OwnerId == null || x.UtcOwnedUntil < utcDate);
            if (entityEvent != null)
            {
                entityEvent.OwnerId = ownerID;
                entityEvent.UtcOwnedUntil = utcOwnedUntil;
                saveChangesCommand.Save();
            }
            else
            {
                entityEvent = _entityEventRepository.Query().FirstOrDefault(x => x.Id == id);
                newOwnerId = entityEvent?.OwnerId;
            }
            return newOwnerId;
        }

        public Guid? ReleaseEntityEventOwnership(Guid id, Guid ownerID)
        {
            Guid? curOwner = null;
            var entityEvent = _entityEventRepository.Query()
                                    .FirstOrDefault(x => x.Id == id && x.OwnerId == ownerID);
            if (entityEvent != null)
            {
                entityEvent.OwnerId = null;
                entityEvent.UtcOwnedUntil = null;
                saveChangesCommand.Save();
            }
            else
            {
                curOwner = _entityEventRepository.Query()
                                .Where(x => x.Id == id)
                                .Select(x => x.OwnerId)
                                .FirstOrDefault();
            }
            return curOwner;
        }

        public void MarkEntityEventAsProcessed(Guid id)
        {
            var entityEvent = _entityEventRepository.Query().First(x => x.Id == id);
            entityEvent.IsProcessed = true;
            saveChangesCommand.Save();
        }

        public HashSet<Guid> GetActiveEventsIDs()
        {
            var availableTypes = new []
            {
                EventType.EntityCreated,
                EventType.EntityDeleted,
                EventType.WorkflowCreated,
                EventType.WorkflowDeleted, 
                EventType.WorkflowStateChanging,
                EventType.WorkflowStateChanged,
                EventType.EntityStateSet
            };
            return _entityEventRepository.Query()
                          .Where(x => x.IsProcessed == false && availableTypes.Contains(x.Type))
                          .Select(x => x.EntityId)
                          .Distinct()
                          .ToHashSet();
        }

        public bool? CheckIsProcessed(Guid id)
        {
            return _entityEventRepository.Query().Where(e => e.Id == id).Select(x => x.IsProcessed).FirstOrDefault();
        }

        public async Task DeleteEntityOrEnvironmentEventAsync(Guid id, CancellationToken cancellationToken = default)
        {
            DeleteEntityEvent(id);
            DeleteEnvironmentEvent(id);
            await saveChangesCommand.SaveAsync(cancellationToken);
        }
        
        public void DeleteEntityEvent(Guid id)
        {
            var entity = _entityEventRepository.FirstOrDefault(x => x.Id == id);
            if (entity != null)
                _entityEventRepository.Delete(entity);
        }
        
        public void DeleteEnvironmentEvent(Guid id)
        {
            var entity = _envEventRepository.FirstOrDefault(x => x.ID == id);
            if (entity != null)
                _envEventRepository.Delete(entity);
        }

        public void DeleteExternalEventReferenceByEntityID(Guid entityID)
        {
            var eventQuery = _entityEventRepository.Query()
                .Where(x=>x.EntityId == entityID && x.Type == 0);

            var query = externEventRepository.Query().Where(x => eventQuery.Any(y => y.Id == x.ExternalEventId));
           

            query.ToList()
                 .ForEach(x => externEventRepository.Delete(x));
            saveChangesCommand.Save();
        }

        public void DeleteEntityEventByEntityID(Guid entityID)
        {
            var query = _entityEventRepository.Query()
                .Where(x => x.EntityId == entityID);

            query.ToList()
                 .ForEach(x => _entityEventRepository.Delete(x));
            saveChangesCommand.Save();
        }

        public void InsertEntityEvent(InfraManager.WE.DTL.Events.EntityEvent entityEvent, Guid workflowID)
        {
            if (entityEvent.Type == 2)
            {
                var eventQuery = _entityEventRepository.Query()
                    .Where(x=>x.EntityId == entityEvent.EntityID && x.Type == 0);

                var query = externEventRepository.Query().Where(x => eventQuery.Any(y => y.Id == x.ExternalEventId));
           

                query.ToList()
                     .ForEach(x => externEventRepository.Delete(x));

                var entityQuery = _entityEventRepository.Query()
                    .Where(x => x.EntityId == entityEvent.EntityID);

                entityQuery.ToList()
                     .ForEach(x => _entityEventRepository.Delete(x));
            }

            var newEntityEvent = new InfraManager.DAL.Events.EntityEvent(
                (EventSource)entityEvent.Source,
                (EventType)entityEvent.Type,
                (ObjectClass)entityEvent.EntityClassID,
                entityEvent.EntityID,
                entityEvent.CauserID,
                entityEvent.TargetStateID);

            _entityEventRepository.Insert(newEntityEvent);

            externEventRepository.Insert(new ExternalEventReference()
            {
                WorkflowId = workflowID,
                ExternalEventId = newEntityEvent.Id,
                ConsiderationCount = 0,
            });

            saveChangesCommand.Save();
        }

        public void IncrementConsiderationCount(Guid externalEventID)
        {
            var externEvent = externEventRepository.Query().First(x => x.ExternalEventId == externalEventID);
            externEvent.ConsiderationCount++;
            saveChangesCommand.Save();
        }

        private void SaveChanges(bool isRepeatableRead)
        {
            saveChangesCommand.Save(isRepeatableRead ? IsolationLevel.RepeatableRead : IsolationLevel.ReadCommitted);
        }
    }
}
