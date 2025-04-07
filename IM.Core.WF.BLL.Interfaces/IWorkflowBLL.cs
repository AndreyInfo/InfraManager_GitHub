using IM.Core.WF.BLL.Interfaces.Models;
using InfraManager.DAL;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace IM.Core.WF.BLL.Interfaces
{
    public interface IWorkflowBLL
    {
        Guid? AcquireOwnership(Guid id, Guid ownerID, DateTime utcOwnedUntil, bool isRepeatableRead = false);

        Guid? ReleaseOwnership(Guid id, Guid ownerID, bool isRepeatableRead = false);

        bool Exists(Guid id);

        List<Guid> RetrievePlannedWorkflowIDs(DateTime utcPlannedAt, Guid ownerID, DateTime utcOwnedUntil, bool isRepeatableRead = false);

        Guid? Save(WorkflowModel model, Guid? ownerID, DateTime? utcOwnedUntil, bool isRepeatableRead = false);

        void Delete(Guid id, bool isRepeatableRead = false);

        (WorkflowModel workflowModel, Guid? currentOwnerID) Get(Guid id, Guid? ownerID, DateTime? utcOwnedUntil, bool isRepeatableRead = false);
        
        Task RemoveRedundantWorkflow(CancellationToken cancellationToken = default);

        Task EntityModifiedForAllAsync(CancellationToken cancellationToken = default);
    }
}
