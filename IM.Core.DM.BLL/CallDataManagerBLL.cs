using IM.Core.DM.BLL.Interfaces;
using IM.Core.DM.BLL.Interfaces.Models;
using InfraManager;
using InfraManager.DAL;
using InfraManager.DAL.ServiceDesk;
using System;
using System.Linq;
using System.Transactions;

namespace IM.Core.DM.BLL
{
    internal class CallDataManagerBLL : ICallDataManagerBLL, ISelfRegisteredService<ICallDataManagerBLL>
    {
        private readonly IUnitOfWork saveChangesCommand;
        private readonly IReadonlyRepository<Call> callRepository;

        public CallDataManagerBLL(
                    IUnitOfWork saveChangesCommand,
                    IRepository<Call> callRepository)
        {
            this.callRepository = callRepository;
            this.saveChangesCommand = saveChangesCommand;
        }

        public bool ExistsByID(Guid id, bool? removed)
        {
            var query = callRepository.Query();
            if (removed == null)
                return query.Any(x => x.IMObjID == id);
            else
                return query.Any(x => x.IMObjID == id && x.Removed == removed);
        }

        public CallModel Get(Guid id, Guid? currentUserID)
        {
            return callRepository.Query()
                       .Where(x => x.IMObjID == id)
                       .Select(x => new CallModel()
                       {
                            ID = x.IMObjID,
                            EntityStateID = x.EntityStateID,
                            EntityStateName = x.EntityStateName,
                            Removed = x.Removed,
                            UtcDateModified = x.UtcDateModified,
                            WorkflowSchemeID = x.WorkflowSchemeID,
                            WorkflowSchemeIdentifier = x.WorkflowSchemeIdentifier,
                            WorkflowSchemeVersion = x.WorkflowSchemeVersion
                       })
                       .FirstOrDefault();
        }

        public bool Update(Guid id, string entityStateID, string entityStateName, Guid? workflowSchemeID, string workflowSchemeIdentifier, string workflowSchemeVersion, bool isRepeatableRead = false)
        {
            var call = callRepository.Query().FirstOrDefault(x => x.IMObjID == id);
            if (call != null)
            {
                call.EntityStateID = entityStateID;
                call.EntityStateName = entityStateName;
                call.WorkflowSchemeID = workflowSchemeID;
                call.WorkflowSchemeIdentifier = workflowSchemeIdentifier;
                call.WorkflowSchemeVersion = workflowSchemeVersion;
                call.UtcDateModified = DateTime.UtcNow;
                SaveChanges(isRepeatableRead);
            }
            return (call != null);
        }

        private void SaveChanges(bool isRepeatableRead)
        {
            saveChangesCommand.Save(isRepeatableRead ? IsolationLevel.RepeatableRead : IsolationLevel.ReadCommitted);
        }
    }
}
