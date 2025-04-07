using InfraManager.WE.DTL.Events;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using InfraManager.DAL.WorkFlow.Events;

namespace IM.Core.WF.BLL.Interfaces
{
    public interface IExternalEventBLL
    {
        ExternalEvent[] GetList(Guid ownerID, DateTime utcOwnedUntil, int maxConsiderationCount);

        bool EntityHasActiveEvents(Guid entityID);
        
        Guid[] GetWorkflowIDs(Guid externalEventID);

        void DeleteExternalEventReference(Guid? externalEventID, Guid? workflowID, bool isRepeatableRead);
        
        Guid? AcquireEntityEventOwnership(Guid id, Guid ownerID, DateTime utcOwnedUntil);

        Guid? ReleaseEntityEventOwnership(Guid id, Guid ownerID);

        void MarkEntityEventAsProcessed(Guid id);

        HashSet<Guid> GetActiveEventsIDs();

        bool? CheckIsProcessed(Guid id);
        void DeleteEntityEvent(Guid id);
        void DeleteExternalEventReferenceByEntityID(Guid entityID);
        void DeleteEntityEventByEntityID(Guid entityId);
        void InsertEntityEvent(EntityEvent entityEvent, Guid workflowId);
        
        Task<BaseEventItem[]> GetAllListAsync(CancellationToken cancellationToken = default);

        Task DeleteEntityOrEnvironmentEventAsync(Guid id, CancellationToken cancellationToken = default);

        void DeleteEnvironmentEvent(Guid id);
        void IncrementConsiderationCount(Guid externalEventID);
    }
}
