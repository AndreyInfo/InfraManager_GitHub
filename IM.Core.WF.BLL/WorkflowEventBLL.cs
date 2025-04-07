using IM.Core.WF.BLL.Interfaces;
using IM.Core.WF.BLL.Interfaces.Models;
using InfraManager;
using InfraManager.DAL;
using InfraManager.DAL.WF;
using System;
using System.Linq;
using System.Transactions;

namespace IM.Core.WF.BLL
{
    internal class WorkflowEventBLL : IWorkflowEventBLL, ISelfRegisteredService<IWorkflowEventBLL>
    {
        private readonly IUnitOfWork saveChangesCommand;
        private readonly IRepository<WorkflowEvent> workflowEventRepository;

        public WorkflowEventBLL(
                    IUnitOfWork saveChangesCommand,
                    IRepository<WorkflowEvent> workflowEventRepository)
        {
            this.saveChangesCommand = saveChangesCommand;
            this.workflowEventRepository = workflowEventRepository;
        }

        public void Delete(Guid workflowID, bool isRepeatableRead = false)
        {
            var wfs = workflowEventRepository.Query()
                            .Where(x => x.WorkflowID == workflowID)
                            .ToList();
            wfs.ForEach(x => workflowEventRepository.Delete(x));
            SaveChanges(isRepeatableRead);
        }

        public WorkflowEventModel[] GetList(Guid workflowID)
        {
            return workflowEventRepository.Query()
                    .Where(x => x.WorkflowID == workflowID)
                    .OrderBy(x => x.ID)
                    .Select(x => new WorkflowEventModel()
                    {
                        ActivityID = x.ActivityID,
                        Message = x.Message,
                        Type = x.Type,
                        UtcTimeStamp = x.UtcTimeStamp,
                        StateID = x.StateID
                    })
                    .ToArray();
        }

        public void Insert(Guid workflowID, WorkflowEventModel workflowEvent)
        {
            WorkflowEvent wfEvent;
            var count = workflowEventRepository.Query().Count(x => x.WorkflowID == workflowID);
            if (count > 2000)
            {
                wfEvent = workflowEventRepository.Query()
                               .Where(x => x.WorkflowID == workflowID)
                               .OrderBy(x => x.ID)
                               .First();
                workflowEventRepository.Delete(wfEvent);
                saveChangesCommand.Save();
            }
            wfEvent = new WorkflowEvent()
            {
                WorkflowID = workflowID,
                UtcTimeStamp = workflowEvent.UtcTimeStamp,
                Type = workflowEvent.Type,
                Message = workflowEvent.Message,
                StateID = workflowEvent.StateID,
                ActivityID = workflowEvent.ActivityID
            };
            workflowEventRepository.Insert(wfEvent);
            saveChangesCommand.Save();
        }

        private void SaveChanges(bool isRepeatableRead)
        {
            saveChangesCommand.Save(isRepeatableRead ? IsolationLevel.RepeatableRead : IsolationLevel.ReadCommitted);
        }
    }
}
